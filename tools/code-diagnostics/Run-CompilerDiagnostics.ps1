<#
.SYNOPSIS
    Run Roslyn Compiler Diagnostics

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Builds the specified scope and captures all CS warnings from the Roslyn compiler.
    Outputs a structured summary (text or JSON) to stdout and writes artifacts to disk.

.PARAMETER Scope
    Project scope to analyze. Defaults to All.

.PARAMETER Filter
    Optional regex pattern to filter warning codes (e.g. "CS86" for nullability, "CS0618" for obsolete).

.PARAMETER OutputFormat
    Output format: Text (default) or Json.

.PARAMETER Configuration
    Build configuration. Defaults to Debug.

.PARAMETER Clean
    If set, runs a clean build first.
#>

[CmdletBinding()]
param(
    [ValidateSet('All', 'Core', 'MustClauses', 'GuardClauses', 'FluentValidation', 'DataAnnotations', 'Testing')]
    [string] $Scope = 'All',

    [string] $Filter,

    [ValidateSet('Text', 'Json')]
    [string] $OutputFormat = 'Text',

    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug',

    [switch] $Clean
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

# ── Scope → Build Target Mapping ──────────────────────────────────────────────

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

$scopeTargets = @{
    'All'              = Join-Path $repoRoot 'PineGuard.slnx'
    'Core'             = Join-Path $repoRoot 'src\PineGuard.Core\PineGuard.Core.csproj'
    'MustClauses'      = Join-Path $repoRoot 'src\PineGuard.MustClauses\PineGuard.MustClauses.csproj'
    'GuardClauses'     = Join-Path $repoRoot 'src\PineGuard.GuardClauses\PineGuard.GuardClauses.csproj'
    'FluentValidation' = Join-Path $repoRoot 'src\PineGuard.FluentValidation\PineGuard.FluentValidation.csproj'
    'DataAnnotations'  = Join-Path $repoRoot 'src\PineGuard.DataAnnotations\PineGuard.DataAnnotations.csproj'
    'Testing'          = Join-Path $repoRoot 'tests\PineGuard.Testing\PineGuard.Testing.csproj'
}

$buildTarget = $scopeTargets[$Scope]
if (-not (Test-Path $buildTarget)) {
    throw "Build target not found: $buildTarget"
}

# ── Output Directory ──────────────────────────────────────────────────────────

$scopeSlug = $Scope.ToLowerInvariant()
$outputDir = Join-Path $repoRoot "artifacts\code-diagnostics\$scopeSlug"
Ensure-Directory -Path $outputDir

# ── Build ─────────────────────────────────────────────────────────────────────

Write-Host "`n=== Roslyn Compiler Diagnostics ===" -ForegroundColor Cyan
Write-Host "Scope         : $Scope"
Write-Host "Build Target  : $buildTarget"
Write-Host "Configuration : $Configuration"
if ($Filter) { Write-Host "Filter        : $Filter" }
Write-Host ""

$buildArgs = @('build', $buildTarget, '--no-incremental', '-c', $Configuration)

if ($Clean) {
    Write-Host "Cleaning first..." -ForegroundColor Yellow
    & dotnet clean $buildTarget -c $Configuration 2>&1 | Out-Null
}

Write-Host "Building..." -ForegroundColor Yellow
$buildOutput = & dotnet @buildArgs 2>&1 | Out-String -Stream

# ── Parse Warnings ────────────────────────────────────────────────────────────

$warningPattern = '(?<file>.+)\((?<line>\d+),(?<col>\d+)\):\s+warning\s+(?<code>CS\d+):\s+(?<message>.+?)(?:\s+\[(?<project>.+)\])?$'

$warnings = @()
foreach ($line in $buildOutput) {
    if ($line -match $warningPattern) {
        $warning = [PSCustomObject]@{
            File    = $Matches['file'].Trim()
            Line    = [int]$Matches['line']
            Column  = [int]$Matches['col']
            Code    = $Matches['code']
            Message = $Matches['message'].Trim()
            Project = if ($Matches['project']) { $Matches['project'].Trim() } else { '' }
        }
        $warnings += $warning
    }
}

# ── Apply Filter ──────────────────────────────────────────────────────────────

if ($Filter) {
    $warnings = $warnings | Where-Object { $_.Code -match $Filter }
}

# ── Write Artifacts ───────────────────────────────────────────────────────────

$jsonPath = Join-Path $outputDir 'diagnostics.json'
$report = [PSCustomObject]@{
    Scope         = $Scope
    Configuration = $Configuration
    Filter        = if ($Filter) { $Filter } else { $null }
    Timestamp     = (Get-Date -Format 'o')
    TotalWarnings = $warnings.Count
    ByCode        = ($warnings | Group-Object Code | Sort-Object Count -Descending | ForEach-Object {
        [PSCustomObject]@{ Code = $_.Name; Count = $_.Count }
    })
    ByFile        = ($warnings | Group-Object File | Sort-Object Count -Descending | ForEach-Object {
        [PSCustomObject]@{ File = $_.Name; Count = $_.Count }
    })
    Warnings      = $warnings
}

$report | ConvertTo-Json -Depth 5 | Set-Content -Path $jsonPath -Encoding UTF8
Write-Host "`nArtifacts written to: $jsonPath" -ForegroundColor DarkGray

# ── Output ────────────────────────────────────────────────────────────────────

if ($OutputFormat -eq 'Json') {
    $report | ConvertTo-Json -Depth 5
}
else {
    Write-Host "`n=== Results ===" -ForegroundColor Cyan
    Write-Host "Total warnings: $($warnings.Count)"

    if ($warnings.Count -gt 0) {
        Write-Host "`nBy warning code:" -ForegroundColor Yellow
        $warnings | Group-Object Code | Sort-Object Count -Descending | ForEach-Object {
            Write-Host ("  {0,-10} {1}" -f $_.Name, $_.Count)
        }

        Write-Host "`nBy file:" -ForegroundColor Yellow
        $warnings | Group-Object File | Sort-Object Count -Descending | Select-Object -First 20 | ForEach-Object {
            $relPath = $_.Name -replace [regex]::Escape($repoRoot), ''
            Write-Host ("  {0,-4} {1}" -f $_.Count, $relPath.TrimStart('\'))
        }

        Write-Host "`nDetails:" -ForegroundColor Yellow
        foreach ($w in $warnings) {
            $relPath = $w.File -replace [regex]::Escape($repoRoot), ''
            Write-Host ("  {0} ({1},{2}): {3}: {4}" -f $relPath.TrimStart('\'), $w.Line, $w.Column, $w.Code, $w.Message)
        }
    }
    else {
        Write-Host "`nNo compiler warnings found." -ForegroundColor Green
    }
}

# ── Exit Code ─────────────────────────────────────────────────────────────────

if ($warnings.Count -gt 0) {
    exit 1
}
else {
    exit 0
}
