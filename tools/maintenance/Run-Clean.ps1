<#
.SYNOPSIS
    Orchestrates the execution of cleanup scripts.

.DESCRIPTION
    Runs Cleanup-Logs, Cleanup-Artifacts, and Cleanup-Root based on switches.
    Passes common parameters (Extensions, All, Recursive) to the called scripts.

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
    Switch to pass to children.

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

# Common parameter map
$commonParams = @{
    All       = $All
    Recursive = $Recursive
}

# If user specifies Extensions, add them
if ($Extensions) {
    $commonParams['Extensions'] = $Extensions
}

# Helper wrapper to run script
function Invoke-CleanupScript {
    param($Name)
    $path = Join-Path $scriptDir $Name
    if (Test-Path $path) {
        Write-Host ">>> Invoking $Name" -ForegroundColor Magenta
        # Splat common params
        & $path @commonParams
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
    Invoke-CleanupScript "Clean-Logs.ps1"
}

if ($Artifacts) {
    Invoke-CleanupScript "Clean-Artifacts.ps1"
}

if ($Root) {
    Invoke-CleanupScript "Clean-Root.ps1"
}

Write-Host "Run-Clean Sequence Complete." -ForegroundColor Green
