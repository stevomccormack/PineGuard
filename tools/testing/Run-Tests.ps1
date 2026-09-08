<#
.SYNOPSIS
    Run Tests wrapper for dotnet test.

.DESCRIPTION
    Executes dotnet test against a specific project, solution, or a registry scope, with
    support for async/detached execution and standard filtering parameters.

    Related scripts:
      - tools/code-format/Run-Format.ps1 (this script's -Scope mutual-exclusion pattern)
      - tools/code-diagnostics/Run-CompilerDiagnostics.ps1

.PARAMETER Project
    Path to a specific project file (.csproj).

.PARAMETER Solution
    Path to a specific solution file (.sln / .slnx).

.PARAMETER Scope
    Named scope that resolves to that scope's TestCsproj from the shared registry. Valid
    values: Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options,
    DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers,
    Testing, All.
    'All' resolves to PineGuard.slnx (every project in the solution) rather than a single
    TestCsproj. Exactly one of -Project, -Solution, or -Scope must be specified.

.PARAMETER Filter
    Filter expression for running selective tests (e.g. "FullyQualifiedName~Tests"). This is
    dotnet test's own --filter syntax; it is never repurposed for anything else.

.PARAMETER Framework
    Target framework moniker (e.g. "net10.0") passed through as --framework. Optional; no
    validation beyond what `dotnet test` itself accepts.

.PARAMETER OutputPath
    Path to directory for test results. If specified, adds --results-directory and a trx
    logger. A relative path is resolved against the repository root (found via
    PineGuard.slnx), not the caller's current working directory. An absolute path is used
    as-is.

    The trx logger is configured with LogFilePrefix (not LogFileName): LogFileName is a fixed
    name that a multi-project run (-Scope All, or a -Solution with more than one test
    project) would overwrite once per project, silently losing every result but the last.
    LogFilePrefix makes the trx logger append a unique, per-assembly/per-target-framework
    suffix to the prefix instead, so every project's results land in their own file.

.PARAMETER NoBuild
    Skip build phase.

.PARAMETER Async
    If set, runs the test command in a separate process (Start-Process) to avoid blocking the
    current shell. The spawned process object is returned so its exit code can be recovered
    later (e.g. `$proc.WaitForExit(); $proc.ExitCode`); its PID is also printed to the console.

.PARAMETER Configuration
    Build configuration (Debug/Release).

.NOTES
    Exit codes: 0 = dotnet test succeeded; 2 = usage/prerequisite problem (conflicting or
    missing -Project/-Solution/-Scope, or a resolved target that does not exist on disk) —
    never reached once dotnet test itself has started running. Any other nonzero code is
    dotnet test's own exit code, propagated as-is rather than collapsed to a generic 1:
    dotnet test / VSTest already distinguish "one or more tests failed" from other run-aborted
    conditions in its own exit-code space, and remapping that away would destroy information
    without a clear benefit (see docs/ai/plans/tools-review-and-standardisation.md, T3.06).

.EXAMPLE
    Run-Tests.ps1 -Scope Core

.EXAMPLE
    Run-Tests.ps1 -Scope All -OutputPath artifacts/testing

.EXAMPLE
    Run-Tests.ps1 -Project "tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj" -Async
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [string]$Project,
    [string]$Solution,

    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'FluentValidation', 'DataAnnotations', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'Testing', 'All')]
    [string]$Scope,

    [string]$Filter,
    [string]$Framework,
    [string]$OutputPath,
    [switch]$NoBuild,
    [switch]$Async,
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Debug'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'transcript.ps1')

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$transcriptPath = Start-ToolTranscript -Domain 'testing' -RepoRoot $repoRoot
Write-Verbose "Transcript: $transcriptPath"

try {
    # --- Mutual exclusion (same pattern as Run-Format.ps1) ---
    # Write-Error is non-terminating (-ErrorAction Continue) at every usage-error site below so
    # the following `exit 2` actually runs — under this script's $ErrorActionPreference = 'Stop',
    # a terminating Write-Error would abort the script immediately and fall through to pwsh's
    # default uncaught-error exit code (1), silently losing the intended 2.
    $targetCount = @($Project, $Solution, $Scope).Where({ $_ }).Count
    if ($targetCount -gt 1) {
        Write-Error "Specify only one of -Project, -Solution, or -Scope." -ErrorAction Continue
        exit 2
    }
    if ($targetCount -eq 0) {
        Write-Error "Specify one of -Project, -Solution, or -Scope." -ErrorAction Continue
        exit 2
    }

    # --- Target resolution ---
    if ($Scope) {
        $target = if ($Scope -eq 'All') {
            Join-Path $repoRoot 'PineGuard.slnx'
        }
        else {
            Join-Path $repoRoot (Get-PineGuardScope -Name $Scope).TestCsproj
        }
        if (-not (Test-Path $target)) {
            Write-Error "Resolved target not found: $target" -ErrorAction Continue
            exit 2
        }
    }
    elseif ($Solution) {
        if (-not (Test-Path $Solution)) {
            Write-Error "Solution file not found: $Solution" -ErrorAction Continue
            exit 2
        }
        $target = $Solution
    }
    else {
        if (-not (Test-Path $Project)) {
            Write-Error "Project file not found: $Project" -ErrorAction Continue
            exit 2
        }
        $target = $Project
    }

    $cmdArgs = @('test', $target)

    # Configuration
    $cmdArgs += '--configuration', $Configuration

    # Options
    if ($Filter) {
        $cmdArgs += '--filter', $Filter
    }

    if ($Framework) {
        $cmdArgs += '--framework', $Framework
    }

    if ($NoBuild) {
        $cmdArgs += '--no-build'
    }

    if ($OutputPath) {
        # Resolve a relative path against the repo root, not the caller's current working
        # directory, so results land in a predictable place regardless of where this script
        # was invoked from. An absolute path the caller passed explicitly is used as-is.
        if (-not [IO.Path]::IsPathRooted($OutputPath)) {
            $OutputPath = Join-Path $repoRoot $OutputPath
        }

        if (-not (Test-Path $OutputPath)) {
            New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
        }

        # LogFilePrefix, not LogFileName: see .PARAMETER OutputPath above for why a fixed
        # LogFileName is unsafe for -Scope All / multi-project -Solution runs.
        $trxPrefix = if ($Scope) { $Scope } elseif ($Solution) { [IO.Path]::GetFileNameWithoutExtension($Solution) } else { [IO.Path]::GetFileNameWithoutExtension($Project) }
        $cmdArgs += '--results-directory', $OutputPath
        $cmdArgs += '--logger', "trx;LogFilePrefix=$trxPrefix"
    }

    $cmdStr = "dotnet " + ($cmdArgs -join " ")

    if ($PSCmdlet.ShouldProcess($cmdStr, "Execute Tests")) {
        if ($Async) {
            $proc = Start-Process dotnet -ArgumentList $cmdArgs -NoNewWindow -PassThru
            Write-Host "Started async test run (PID $($proc.Id)). Check `$proc.ExitCode` after it exits (`$proc.WaitForExit()`), or inspect the results at -OutputPath if provided." -ForegroundColor Cyan
            return $proc
        }
        else {
            & dotnet $cmdArgs
            if ($LASTEXITCODE -ne 0) {
                exit $LASTEXITCODE
            }
        }
    }
}
finally {
    Stop-Transcript | Out-Null
}
