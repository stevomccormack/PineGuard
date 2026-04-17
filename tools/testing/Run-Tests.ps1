<#
.SYNOPSIS
    Run Tests wrapper for dotnet test.

.DESCRIPTION
    Executes dotnet test with support for async/detached execution and standard filtering parameters.

.PARAMETER Project
    Path to a specific project file (.csproj).

.PARAMETER Solution
    Path to a specific solution file (.sln).

.PARAMETER Filter
    Filter expression for running selective tests (e.g. "FullyQualifiedName~Tests").

.PARAMETER Output
    Path to directory for test results. If specified, adds trx logger.

.PARAMETER NoBuild
    Skip build phase.

.PARAMETER Async
    If set, runs the test command in a separate process (Start-Process) to avoid blocking the current shell.

.PARAMETER Configuration
    Build configuration (Debug/Release).

.EXAMPLE
    Run-Tests.ps1 -Project "tests/MyProject.UnitTests" -Async
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [string]$Project,
    [string]$Solution,
    [string]$Filter,
    [string]$Output,
    [switch]$NoBuild,
    [switch]$Async,
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Debug'
)

$cmdArgs = @("test")

# Target Selection
if ($Project -and $Solution) {
    Write-Error "Specify either -Project or -Solution, not both."
    exit 1
}

if ($Solution) {
    if (-not (Test-Path $Solution)) { throw "Solution file not found: $Solution" }
    $cmdArgs += $Solution
}
elseif ($Project) {
    if (-not (Test-Path $Project)) { throw "Project file not found: $Project" }
    $cmdArgs += $Project
}

# Configuration
$cmdArgs += "--configuration", $Configuration

# Options
if ($Filter) {
    $cmdArgs += "--filter", $Filter
}

if ($NoBuild) {
    $cmdArgs += "--no-build"
}

if ($Output) {
    # Ensure output directory exists
    if (-not (Test-Path $Output)) {
        New-Item -ItemType Directory -Path $Output -Force | Out-Null
    }
    # Use trx logger with specific path if needed, or just directory
    $cmdArgs += "--logger", "trx;LogFileName=$((Join-Path $Output 'results.trx'))"
}

$cmdStr = "dotnet " + ($cmdArgs -join " ")

if ($PSCmdlet.ShouldProcess($cmdStr, "Execute Tests")) {
    if ($Async) {
        Write-Host "Starting tests asynchronously..." -ForegroundColor Cyan
        Start-Process dotnet -ArgumentList $cmdArgs -NoNewWindow
    }
    else {
        & dotnet $cmdArgs
        if ($LASTEXITCODE -ne 0) {
            exit $LASTEXITCODE
        }
    }
}
