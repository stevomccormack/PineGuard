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
    Named scope that resolves to that scope's source project(s). Valid values:
    Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All.
    'Analyzers' targets two projects (the analyzer and its code fixes) — 'dotnet format' skips
    referenced projects, so each is formatted in its own pass.
    'Testing' targets the PineGuard.Testing support library under tests/.
    'All' targets the full PineGuard.slnx solution (src + tests).

.PARAMETER VerifyNoChanges
    Run in verification mode (--verify-no-changes). Exits non-zero if any files
    would be changed. Useful for CI pipelines and dry-run checks.

.PARAMETER Severity
    Minimum severity of diagnostics to format (info, warn, error).

.PARAMETER NoRestore
    Skip the implicit restore phase (--no-restore).

.PARAMETER Verbosity
    MSBuild verbosity level (q[uiet], m[inimal], n[ormal], d[etailed], diag[nostic]).

.PARAMETER Configuration
    Build configuration (Debug/Release).

.NOTES
    Exit codes: 0 = no formatting differences (or changes applied outside
    -VerifyNoChanges); 2 = usage/prerequisite problem (conflicting or missing
    -Project/-Solution/-Scope, or a resolved target that does not exist). Any other nonzero
    code is `dotnet format`'s own exit code, propagated as-is — most notably the code it uses
    under -VerifyNoChanges to report that files would be reformatted (a quality-gate-not-met
    result), which is not remapped here to avoid guessing at a mapping `dotnet format` does
    not document as stable.

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

    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'FluentValidation', 'DataAnnotations', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'Testing', 'All')]
    [string]$Scope,

    [switch]$VerifyNoChanges,

    [ValidateSet('info', 'warn', 'error')]
    [string]$Severity,

    [switch]$NoRestore,

    [ValidateSet('q', 'quiet', 'm', 'minimal', 'n', 'normal', 'd', 'detailed', 'diag', 'diagnostic')]
    [string]$Verbosity,

    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'transcript.ps1')

# --- Resolve repo root ---
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$transcriptPath = Start-ToolTranscript -Domain 'code-format' -RepoRoot $repoRoot
Write-Verbose "Transcript: $transcriptPath"

try {
    # Write-Error is non-terminating (-ErrorAction Continue) at every usage-error site below so
    # the following `exit 2` actually runs — under this script's $ErrorActionPreference = 'Stop',
    # a terminating Write-Error would abort the script immediately and fall through to pwsh's
    # default uncaught-error exit code (1), silently losing the intended 2.

    # --- Mutual exclusion ---
    $targetCount = @($Project, $Solution, $Scope).Where({ $_ }).Count
    if ($targetCount -gt 1) {
        Write-Error "Specify only one of -Project, -Solution, or -Scope." -ErrorAction Continue
        exit 2
    }
    if ($targetCount -eq 0) {
        Write-Error "Specify one of -Project, -Solution, or -Scope." -ErrorAction Continue
        exit 2
    }

    # --- Scope resolution ---
    # A scope can own more than one source project (Analyzers = analyzer + code fixes), and
    # 'dotnet format' formats only the project it is handed — it logs "Skipping referenced
    # project" for the rest — so every project in the scope gets its own pass.
    if ($Scope) {
        $targets = if ($Scope -eq 'All') {
            @(Join-Path $repoRoot 'PineGuard.slnx')
        }
        else {
            @((Get-PineGuardScope -Name $Scope).SourceCsprojs | ForEach-Object { Join-Path $repoRoot $_ })
        }
        foreach ($target in $targets) {
            if (-not (Test-Path $target)) {
                Write-Error "Resolved target not found: $target" -ErrorAction Continue
                exit 2
            }
        }
    }
    elseif ($Solution) {
        if (-not (Test-Path $Solution)) {
            Write-Error "Solution file not found: $Solution" -ErrorAction Continue
            exit 2
        }
        $targets = @($Solution)
    }
    else {
        if (-not (Test-Path $Project)) {
            Write-Error "Project file not found: $Project" -ErrorAction Continue
            exit 2
        }
        $targets = @($Project)
    }

    # --- Build command args ---
    $commonArgs = @()

    if ($VerifyNoChanges) {
        $commonArgs += "--verify-no-changes"
    }

    if ($Severity) {
        $commonArgs += "--severity", $Severity
    }

    if ($NoRestore) {
        $commonArgs += "--no-restore"
    }

    if ($Verbosity) {
        $commonArgs += "--verbosity", $Verbosity
    }

    # --- Execute ---
    $label = if ($Scope) { $Scope } elseif ($Solution) { Split-Path $Solution -Leaf } else { Split-Path $Project -Leaf }
    Write-Host "Formatting: $label" -ForegroundColor Cyan

    foreach ($target in $targets) {
        $cmdArgs = @("format", $target) + $commonArgs

        Write-Host "Target:     $target" -ForegroundColor DarkGray
        Write-Host "Command:    dotnet $($cmdArgs -join ' ')" -ForegroundColor DarkGray

        & dotnet $cmdArgs

        if ($LASTEXITCODE -ne 0) {
            Write-Host "dotnet format exited with code $LASTEXITCODE" -ForegroundColor Red
            exit $LASTEXITCODE
        }
    }

    Write-Host "Format complete." -ForegroundColor Green
}
finally {
    Stop-Transcript | Out-Null
}
