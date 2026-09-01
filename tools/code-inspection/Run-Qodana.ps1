<#
.SYNOPSIS
    Run a Qodana code inspection locally using Docker.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Runs Qodana via the Docker container (--within-docker) for local development.
    Results are written to artifacts/qodana/<scope>/.

    For CI/CD, use the JetBrains Qodana GitHub Action with QODANA_TOKEN stored
    as a repository secret - do not use this script in pipelines.

    Prerequisites: Docker Desktop running, Qodana CLI on PATH.
    Run Initialize-Qodana.ps1 to install prerequisites.

.PARAMETER Scope
    The inspection scope. Determines the Qodana config and results directory.
    Valid values: Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All.
    Default: Core.

.PARAMETER RepoRoot
    Absolute path to the repository root. Auto-resolved if not specified.

.PARAMETER ResultsDir
    Override the results output directory. Defaults to artifacts/qodana/<scope>.

.PARAMETER Clean
    Delete the results directory before scanning.

.PARAMETER ShowReport
    Open the Qodana report viewer after the scan.

.PARAMETER OpenReport
    Open the HTML report in the default browser after the scan.

.PARAMETER NonInteractive
    Suppress Qodana's interactive prompts. Default: true (script-friendly).

.PARAMETER TimeoutMinutes
    Hard timeout in minutes. Default: 30.

.PARAMETER TimeoutExitCode
    Exit code returned on timeout. Default: 124.

.PARAMETER Token
    Qodana Cloud token. Falls back to QODANA_TOKEN environment variable.
    Optional for local dev - omit to keep results local only.

.PARAMETER Endpoint
    Qodana Cloud endpoint override. Falls back to QODANA_ENDPOINT environment variable.

.PARAMETER Linter
    Qodana linter image. Default: qodana-dotnet.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Run-Qodana.ps1 -Scope Core
    Runs a Core inspection locally. Results written to artifacts/qodana/core/.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Run-Qodana.ps1 -Scope All -Clean -OpenReport
    Runs a full inspection, clears previous results, and opens the HTML report.
#>

[CmdletBinding()]
param(
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'FluentValidation', 'DataAnnotations', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'Testing', 'All')] [string] $Scope = 'Core',
    [ValidateNotNullOrEmpty()] [string] $RepoRoot = '',
    [string] $ResultsDir = '',
    [switch] $Clean,
    [switch] $ShowReport,
    [switch] $OpenReport,
    [switch] $NonInteractive,
    [ValidateRange(1, 1440)] [int] $TimeoutMinutes = 30,
    [ValidateRange(1, 255)] [int] $TimeoutExitCode = 124,
    [string] $Token,
    [string] $Endpoint,
    [ValidateSet('qodana-dotnet', 'auto')] [string] $Linter = 'qodana-dotnet'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '../audit-cli/helpers/Load-AuditHelpers.ps1')

function Get-QodanaConfigPath {
    param(
        [Parameter(Mandatory = $true)]
        [string] $RepoRootResolved,
        [Parameter(Mandatory = $true)]
        [string] $Scope
    )

    if ($Scope -eq 'All') {
        return (Join-Path $RepoRootResolved 'tools/code-inspection/qodana/config/qodana.all.yaml')
    }

    return (Join-Path $RepoRootResolved (Get-PineGuardScope -Name $Scope).QodanaConfig)
}

function Get-DefaultResultsDir {
    param(
        [Parameter(Mandatory = $true)]
        [string] $RepoRootResolved,
        [Parameter(Mandatory = $true)]
        [string] $Scope
    )

    $name = if ($Scope -eq 'All') { 'all' } else { (Get-PineGuardScope -Name $Scope).QodanaSlug }

    return (Join-Path $RepoRootResolved (Join-Path 'artifacts/qodana' $name))
}

function Write-SarifSummary {
    param(
        [Parameter(Mandatory = $true)]
        [string] $ResultsDirResolved
    )

    $sarif = $null
    $preferredSarifJson = Join-Path $ResultsDirResolved 'qodana.sarif.json'
    if (Test-Path -LiteralPath $preferredSarifJson) {
        $sarif = Get-Item -LiteralPath $preferredSarifJson
    }

    if ($null -eq $sarif) {
        $preferredSarif = Join-Path $ResultsDirResolved 'qodana.sarif'
        if (Test-Path -LiteralPath $preferredSarif) {
            $sarif = Get-Item -LiteralPath $preferredSarif
        }
    }

    if ($null -eq $sarif) {
        $sarif = Get-ChildItem -LiteralPath $ResultsDirResolved -Filter '*.sarif.json' -File -ErrorAction SilentlyContinue | Select-Object -First 1
    }
    if ($null -eq $sarif) {
        $sarif = Get-ChildItem -LiteralPath $ResultsDirResolved -Filter '*.sarif' -File -ErrorAction SilentlyContinue | Select-Object -First 1
    }

    if ($null -eq $sarif) {
        Write-Host "No SARIF file found under: $ResultsDirResolved" -ForegroundColor Yellow
        return
    }

    Write-Host ''
    Write-Host "SARIF: $($sarif.FullName)" -ForegroundColor DarkGray

    try {
        $json = Get-Content -LiteralPath $sarif.FullName -Raw -Encoding utf8 | ConvertFrom-Json
    }
    catch {
        Write-Host "Failed to parse SARIF JSON: $($sarif.FullName)" -ForegroundColor Yellow
        return
    }

    $allResults = @()
    foreach ($run in @($json.runs)) {
        foreach ($result in @($run.results)) {
            $allResults += $result
        }
    }

    $total = $allResults.Count
    $byLevel = @(
        $allResults |
            Group-Object -Property level |
            Sort-Object -Property Count -Descending
    )

    Write-Host "Problems: $total" -ForegroundColor Cyan
    foreach ($g in $byLevel) {
        $level = if ([string]::IsNullOrWhiteSpace($g.Name)) { '<none>' } else { $g.Name }
        Write-Host ("- {0}: {1}" -f $level, $g.Count) -ForegroundColor DarkGray
    }

    $byRule = @(
        $allResults |
            Where-Object { $_.ruleId } |
            Group-Object -Property ruleId |
            Sort-Object Count -Descending |
            Select-Object -First 10
    )

    if ($byRule.Count -gt 0) {
        Write-Host ''
        Write-Host 'Top rules:' -ForegroundColor Cyan
        foreach ($r in $byRule) {
            Write-Host ("- {0}: {1}" -f $r.Name, $r.Count) -ForegroundColor DarkGray
        }
    }
}

# -------------------------------------------------------------------------------------------------

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$configPath = Get-QodanaConfigPath -RepoRootResolved $repoRootResolved -Scope $Scope
if (-not (Test-Path -LiteralPath $configPath)) {
    throw "Qodana config not found for scope '$Scope': $configPath"
}

$resultsDirResolved = $ResultsDir
if ([string]::IsNullOrWhiteSpace($resultsDirResolved)) {
    $resultsDirResolved = Get-DefaultResultsDir -RepoRootResolved $repoRootResolved -Scope $Scope
}

# Note: Resolve-PineGuardPath uses Resolve-Path for rooted paths, which requires existence.
# Results directory may not exist yet, so only make it absolute without requiring it to exist.
if ([System.IO.Path]::IsPathRooted($resultsDirResolved)) {
    $resultsDirResolved = [System.IO.Path]::GetFullPath($resultsDirResolved)
}
else {
    $resultsDirResolved = Join-Path $repoRootResolved $resultsDirResolved
}

Write-Host "Running Qodana ($Scope)..." -ForegroundColor Cyan
Write-Host "RepoRoot : $repoRootResolved" -ForegroundColor DarkGray
Write-Host "Config   : $configPath" -ForegroundColor DarkGray
Write-Host "Results  : $resultsDirResolved" -ForegroundColor DarkGray

if (-not (Test-CommandExists -Name 'qodana')) {
    throw "Qodana CLI ('qodana') was not found on PATH. See docs/ai/specs/tools/code-inspection/qodana.md for install steps."
}

if (-not (Test-CommandExists -Name 'docker')) {
    throw "Docker ('docker') was not found on PATH. Ensure Docker Desktop is running."
}

if ($Clean -and (Test-Path -LiteralPath $resultsDirResolved)) {
    Remove-Item -LiteralPath $resultsDirResolved -Recurse -Force
}

Ensure-PineGuardDirectory -Path $resultsDirResolved

# token/endpoint (do not print)
if (-not [string]::IsNullOrWhiteSpace($Token)) { $env:QODANA_TOKEN = $Token }
if (-not [string]::IsNullOrWhiteSpace($Endpoint)) { $env:QODANA_ENDPOINT = $Endpoint }

Write-Host "QODANA_TOKEN set: $(-not [string]::IsNullOrWhiteSpace($env:QODANA_TOKEN))" -ForegroundColor DarkGray
if (-not [string]::IsNullOrWhiteSpace($env:QODANA_ENDPOINT)) {
    Write-Host "QODANA_ENDPOINT : (set)" -ForegroundColor DarkGray
}

# Qodana CLI uses NONINTERACTIVE to suppress prompts (e.g., "open latest report").
# This wrapper defaults to non-interactive for script-friendly runs.
$nonInteractiveDesired = $true
if ($PSBoundParameters.ContainsKey('NonInteractive')) {
    $nonInteractiveDesired = [bool]$NonInteractive
}

if ($nonInteractiveDesired) {
    if ([string]::IsNullOrWhiteSpace($env:NONINTERACTIVE)) {
        $env:NONINTERACTIVE = '1'
    }
}
else {
    Remove-Item Env:NONINTERACTIVE -ErrorAction SilentlyContinue
}

# Qodana --within-docker mounts repo root at /data/project.
# --config must be relative to repo root with forward slashes so it resolves inside the Linux container.
$configRelative = [System.IO.Path]::GetRelativePath($repoRootResolved, $configPath).Replace('\', '/')
$scanArgs = @('scan', '-i', $repoRootResolved, '-o', $resultsDirResolved, '--config', $configRelative)

# Newer Qodana linters require Qodana Cloud authentication; fail fast rather than prompting.
if ($Linter -ne 'auto' -and [string]::IsNullOrWhiteSpace($env:QODANA_TOKEN)) {
    Write-Host "QODANA_TOKEN is required to run Qodana ($Linter)." -ForegroundColor Yellow
    Write-Host "Set it and re-run (example):" -ForegroundColor DarkGray
    Write-Host "  `$env:QODANA_TOKEN = '<token>'; pwsh -NoProfile -ExecutionPolicy Bypass -File './tools/code-inspection/Run-Qodana.ps1' -Scope $Scope -Clean" -ForegroundColor DarkGray
    exit 2
}

if ($Linter -ne 'auto') {
    $scanArgs += '--linter'
    $scanArgs += $Linter
}

# Hard timeout to prevent Qodana from running indefinitely.
if ($TimeoutMinutes -gt 0) {
    $timeoutMs = [int64]$TimeoutMinutes * 60 * 1000
    $scanArgs += '--timeout'
    $scanArgs += $timeoutMs
    $scanArgs += '--timeout-exit-code'
    $scanArgs += $TimeoutExitCode
}

$scanArgs += '--within-docker'
$scanArgs += 'true'

if (-not [string]::IsNullOrWhiteSpace($env:QODANA_TOKEN)) {
    $scanArgs += '-e'
    $scanArgs += ("QODANA_TOKEN=$($env:QODANA_TOKEN)")
}

if (-not [string]::IsNullOrWhiteSpace($env:QODANA_ENDPOINT)) {
    $scanArgs += '-e'
    $scanArgs += ("QODANA_ENDPOINT=$($env:QODANA_ENDPOINT)")
}

if ($ShowReport) {
    $scanArgs += '--show-report'
}

& qodana @scanArgs
$exitCode = $LASTEXITCODE

if ($exitCode -ne 0) {
    if ($exitCode -eq $TimeoutExitCode) {
        Write-Host "Qodana scan timed out after $TimeoutMinutes minute(s) (exit code: $TimeoutExitCode)." -ForegroundColor Yellow
        Write-SarifSummary -ResultsDirResolved $resultsDirResolved
        exit $exitCode
    }

    Write-Host "Qodana scan failed (exit code: $exitCode)." -ForegroundColor Red
    Write-SarifSummary -ResultsDirResolved $resultsDirResolved
    exit $exitCode
}

Write-SarifSummary -ResultsDirResolved $resultsDirResolved

$reportPath = Join-Path $resultsDirResolved 'report/index.html'
if (Test-Path -LiteralPath $reportPath) {
    Write-Host ''
    Write-Host "Report: $reportPath" -ForegroundColor Cyan

    if ($OpenReport) {
        Start-Process $reportPath
    }
}

Write-Host "Qodana completed ($Scope)." -ForegroundColor Green
