<#
.SYNOPSIS
    Unlist prerelease versions of PineGuard packages on nuget.org.

.DESCRIPTION
    nuget.org has no true delete — unlisting hides a version from search
    and 'Latest prerelease' resolution while keeping existing consumers
    who pinned that version working. This script sends unlist requests via
    `dotnet nuget delete`. The API key referenced by NUGET_TOKEN in
    .etc/powershell/.env must have the Unlist Package scope on the
    PineGuard.* glob.

    Default behaviour: keep the latest prerelease on each package listed
    so "install latest alpha" still works, and unlist everything below.
    Pass -All to unlist every prerelease including the latest one. Stable
    versions (no -alpha/-beta/-rc suffix) are never touched.

.PARAMETER Package
    Limit to specific package IDs. Defaults to the six PineGuard packages.

.PARAMETER All
    Unlist every prerelease, including the latest. Default keeps the
    latest prerelease listed.

.PARAMETER DryRun
    Print the unlist plan without making any API calls.

.PARAMETER Force
    Skip the confirmation prompt.

.PARAMETER EnvFile
    Path to the .env file that supplies NUGET_TOKEN. Defaults to
    .etc/powershell/.env under the repo root.

.EXAMPLE
    pwsh -File ./tools/release/Run-NugetUnlist.ps1 -DryRun

.EXAMPLE
    pwsh -File ./tools/release/Run-NugetUnlist.ps1 -All -Force
#>

[CmdletBinding()]
param(
    [string[]] $Package = @(
        'PineGuard.Core',
        'PineGuard.MustClauses',
        'PineGuard.GuardClauses',
        'PineGuard.FluentValidation',
        'PineGuard.DataAnnotations',
        'PineGuard.Extensions.Options',
        'PineGuard.Extensions.DependencyInjection',
        'PineGuard.AspNetCore',
        'PineGuard.ErrorOr',
        'PineGuard.FluentResults',
        'PineGuard.OneOf',
        'PineGuard.Testing'
    ),
    [switch] $All,
    [switch] $DryRun,
    [switch] $Force,
    [string] $EnvFile
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Step($m) { Write-Host "==> $m" -ForegroundColor Cyan }
function Write-Info($m) { Write-Host "    $m" -ForegroundColor Gray }
function Write-Ok($m) { Write-Host "    OK   $m" -ForegroundColor Green }
function Write-Warn($m) { Write-Host "    WARN $m" -ForegroundColor Yellow }
function Fail($m) { Write-Host "    FAIL $m" -ForegroundColor Red; exit 1 }

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
. (Join-Path $PSScriptRoot '..\.shared\dotenv.ps1')

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($EnvFile)) {
    $EnvFile = Join-Path $repoRoot '.etc/powershell/.env'
}

$vars = Import-DotEnv -Path $EnvFile
$token = $vars['NUGET_TOKEN']
if ([string]::IsNullOrWhiteSpace($token)) {
    Fail "NUGET_TOKEN not found in $EnvFile"
}
Write-Ok "NUGET_TOKEN loaded"

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Fail "dotnet CLI not found on PATH."
}

$source = 'https://api.nuget.org/v3/index.json'
$actions = New-Object System.Collections.Generic.List[object]

Write-Step "Scanning versions"

foreach ($pkg in $Package) {
    $lower = $pkg.ToLowerInvariant()
    $url = "https://api.nuget.org/v3-flatcontainer/$lower/index.json"
    try {
        $resp = Invoke-RestMethod -Uri $url -ErrorAction Stop
    }
    catch {
        Write-Warn "$pkg : could not fetch versions ($($_.Exception.Message))"
        continue
    }

    $versions = @($resp.versions)
    $pre = @($versions | Where-Object { $_ -match '-' })
    if ($pre.Count -eq 0) {
        Write-Info "$pkg : no prereleases"
        continue
    }

    if ($All) {
        $toUnlist = $pre
    }
    elseif ($pre.Count -le 1) {
        Write-Info "$pkg : only one prerelease ($($pre[0])) — keeping it"
        continue
    }
    else {
        $toUnlist = $pre[0..($pre.Count - 2)]
        Write-Info "$pkg : keeping latest prerelease $($pre[-1])"
    }

    foreach ($v in $toUnlist) {
        $actions.Add([pscustomobject]@{ Package = $pkg; Version = $v })
    }
}

if ($actions.Count -eq 0) {
    Write-Step "Nothing to unlist"
    exit 0
}

Write-Step "Unlist plan"
foreach ($a in $actions) {
    Write-Info ("{0,-34} {1}" -f $a.Package, $a.Version)
}
Write-Host ""
$distinctCount = ($actions | Select-Object -ExpandProperty Package -Unique).Count
Write-Info "Total: $($actions.Count) operations across $distinctCount packages"

if ($DryRun) {
    Write-Step "Dry run — no API calls made"
    exit 0
}

if (-not $Force) {
    $confirm = Read-Host "Proceed? [y/N]"
    if ($confirm -ne 'y' -and $confirm -ne 'Y') {
        Write-Warn "Aborted."
        exit 0
    }
}

Write-Step "Unlisting"
$failures = 0
foreach ($a in $actions) {
    $output = dotnet nuget delete $a.Package $a.Version `
        --api-key $token `
        --source $source `
        --non-interactive 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Ok ("{0,-34} {1}" -f $a.Package, $a.Version)
    }
    else {
        Write-Host "    FAIL $($a.Package) $($a.Version)" -ForegroundColor Red
        Write-Host ($output -join [Environment]::NewLine) -ForegroundColor DarkGray
        $failures++
    }
}

Write-Host ""
if ($failures -gt 0) {
    Fail "$failures of $($actions.Count) operations failed."
}
Write-Ok "All $($actions.Count) operations succeeded."
