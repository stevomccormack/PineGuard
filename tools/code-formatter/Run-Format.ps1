<#
.SYNOPSIS
    Run Code Formatter wrapper for dotnet format.

.DESCRIPTION
    Executes dotnet format against the solution, a specific project, or a named scope.
    Uses .editorconfig rules automatically (dotnet format reads them by default).

    Related scripts:
      - tools/testing/Run-Tests.ps1
      - tools/code-coverage/Run-CodeCoverage.ps1

.PARAMETER Project
    Path to a specific project file (.csproj).

.PARAMETER Solution
    Path to a specific solution file (.sln / .slnx).

.PARAMETER Scope
    Named scope that resolves to a source project. Valid values:
    Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, ErrorOr, FluentResults, OneOf, Testing, All.
    'Testing' targets the PineGuard.Testing support library under tests/.
    'All' targets the full PineGuard.slnx solution (src + tests).

.PARAMETER VerifyNoChanges
    Run in verification mode (--verify-no-changes). Exits non-zero if any files
    would be changed. Useful for CI pipelines and dry-run checks.

.PARAMETER Severity
    Minimum severity of diagnostics to format (info, warn, error).

.PARAMETER NoBuild
    Skip the implicit restore/build phase (--no-restore).

.PARAMETER Verbosity
    MSBuild verbosity level (q[uiet], m[inimal], n[ormal], d[etailed], diag[nostic]).

.PARAMETER Configuration
    Build configuration (Debug/Release).

.EXAMPLE
    Run-Format.ps1 -Scope Core

.EXAMPLE
    Run-Format.ps1 -Scope All -VerifyNoChanges

.EXAMPLE
    Run-Format.ps1 -Solution ./PineGuard.slnx

.EXAMPLE
    Run-Format.ps1 -Project src/PineGuard.Core/PineGuard.Core.csproj -Severity warn
#>

[CmdletBinding()]
param(
    [string]$Project,
    [string]$Solution,

    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'FluentValidation', 'DataAnnotations', 'Options', 'ErrorOr', 'FluentResults', 'OneOf', 'Testing', 'All')]
    [string]$Scope,

    [switch]$VerifyNoChanges,

    [ValidateSet('info', 'warn', 'error')]
    [string]$Severity,

    [switch]$NoBuild,

    [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
    [string]$Verbosity,

    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# --- Mutual exclusion ---
$targetCount = @($Project, $Solution, $Scope).Where({ $_ }).Count
if ($targetCount -gt 1) {
    Write-Error "Specify only one of -Project, -Solution, or -Scope."
    exit 1
}
if ($targetCount -eq 0) {
    Write-Error "Specify one of -Project, -Solution, or -Scope."
    exit 1
}

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
. (Join-Path $PSScriptRoot '..\.shared\dotnet-projects.ps1')

# --- Resolve repo root ---
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

# --- Scope resolution ---
if ($Scope) {
    $target = if ($Scope -eq 'All') {
        Join-Path $repoRoot 'PineGuard.slnx'
    }
    else {
        Join-Path $repoRoot (Get-PineGuardScope -Name $Scope).SourceCsproj
    }
    if (-not (Test-Path $target)) {
        throw "Resolved target not found: $target"
    }
}
elseif ($Solution) {
    if (-not (Test-Path $Solution)) { throw "Solution file not found: $Solution" }
    $target = $Solution
}
else {
    if (-not (Test-Path $Project)) { throw "Project file not found: $Project" }
    $target = $Project
}

# --- Build command args ---
$cmdArgs = @("format", $target)

if ($VerifyNoChanges) {
    $cmdArgs += "--verify-no-changes"
}

if ($Severity) {
    $cmdArgs += "--severity", $Severity
}

if ($NoBuild) {
    $cmdArgs += "--no-restore"
}

if ($Verbosity) {
    $cmdArgs += "--verbosity", $Verbosity
}

# --- Execute ---
$label = if ($Scope) { $Scope } elseif ($Solution) { Split-Path $Solution -Leaf } else { Split-Path $Project -Leaf }
Write-Host "Formatting: $label" -ForegroundColor Cyan
Write-Host "Target:     $target" -ForegroundColor DarkGray
Write-Host "Command:    dotnet $($cmdArgs -join ' ')" -ForegroundColor DarkGray

& dotnet $cmdArgs

if ($LASTEXITCODE -ne 0) {
    Write-Host "dotnet format exited with code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Format complete." -ForegroundColor Green
