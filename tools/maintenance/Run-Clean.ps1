<#
.SYNOPSIS
    Orchestrates the execution of cleanup scripts.

.DESCRIPTION
    Runs Cleanup-Logs, Cleanup-Artifacts, and Cleanup-Root based on switches.
    Passes common parameters (Extensions, All) to all called scripts.
    -Recursive is passed through to Clean-Logs.ps1 only: Clean-Root.ps1 no longer
    accepts -Recursive (root cleanup is always top-level-only, for safety) and
    Clean-Artifacts.ps1 no longer accepts -Recursive either (artifacts cleanup is
    always recursive, unconditionally). If -Recursive is passed while targeting
    -Root or -Artifacts, it is simply ignored for those targets since their
    behavior no longer varies on it.

.PARAMETER Logs
    Runs Cleanup-Logs.ps1.

.PARAMETER Artifacts
    Runs Cleanup-Artifacts.ps1.

.PARAMETER Root
    Runs Cleanup-Root.ps1.

.PARAMETER Extensions
    Extensions to pass to children.

.PARAMETER All
    Switch to pass to children.

.PARAMETER Recursive
    Switch to pass to Clean-Logs.ps1 only. Has no effect on -Root or -Artifacts,
    since those scripts no longer accept it (root is always top-level-only,
    artifacts is always recursive).

.EXAMPLE
    .\Run-Cleanup.ps1 -Logs -Artifacts
    Runs log and artifact cleanup.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [switch]$Logs,
    [switch]$Artifacts,
    [switch]$Root,
    [string[]]$Extensions,
    [switch]$All,
    [switch]$Recursive
)

$scriptDir = $PSScriptRoot

# Parameters accepted by every child script.
$commonParams = @{
    All = $All
}

# If user specifies Extensions, add them
if ($Extensions) {
    $commonParams['Extensions'] = $Extensions
}

# Clean-Logs.ps1 still accepts -Recursive; the others no longer do.
$logsParams = $commonParams.Clone()
$logsParams['Recursive'] = $Recursive

if ($Recursive -and ($Root -or $Artifacts)) {
    Write-Verbose "-Recursive has no effect on -Root or -Artifacts: Clean-Root.ps1 is always top-level-only and Clean-Artifacts.ps1 is always recursive."
}

# Helper wrapper to run script
function Invoke-CleanupScript {
    param($Name, $Params)
    $path = Join-Path $scriptDir $Name
    if (Test-Path $path) {
        Write-Host ">>> Invoking $Name" -ForegroundColor Magenta
        # Splat params
        & $path @Params
    }
    else {
        Write-Error "Script not found: $path"
    }
}

# If no targets specified, ask or do nothing?
# Requirement: "Run-Cleanup.ps1 -Logs -Artifacts -Root -Recursive -All -Extensions" implied explicitly calling them.
# If none are specified, we will warn and do nothing to be safe.
if (-not ($Logs -or $Artifacts -or $Root)) {
    Write-Warning "No targets specified. Use -Logs, -Artifacts, or -Root."
    return
}

if ($Logs) {
    Invoke-CleanupScript "Clean-Logs.ps1" $logsParams
}

if ($Artifacts) {
    Invoke-CleanupScript "Clean-Artifacts.ps1" $commonParams
}

if ($Root) {
    Invoke-CleanupScript "Clean-Root.ps1" $commonParams
}

Write-Host "Run-Clean Sequence Complete." -ForegroundColor Green
