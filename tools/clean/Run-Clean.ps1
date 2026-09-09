<#
.SYNOPSIS
    Orchestrates the execution of the clean/ scripts.

.DESCRIPTION
    Runs Clear-Logs.ps1, Clear-Artifacts.ps1, and Clear-Root.ps1 for the targets named in
    -Target (or all three when -All is given without an explicit -Target). Passes common
    parameters (Extensions, All) to all called scripts. -Recursive is passed through to
    Clear-Logs.ps1 only: Clear-Root.ps1 does not accept -Recursive (root cleanup is always
    top-level-only, for safety, F-27) and Clear-Artifacts.ps1 does not accept -Recursive either
    (artifacts cleanup is always recursive, unconditionally). If -Recursive is passed while
    targeting Root or Artifacts, it is simply ignored for those targets since their behavior no
    longer varies on it.

.PARAMETER Target
    Which cleanup scripts to run: any combination of 'Artifacts', 'Logs', 'Root'.
    Example: -Target Artifacts,Logs

.PARAMETER Extensions
    Extensions to pass to children.

.PARAMETER All
    Switch to pass to children (clean all file types, regardless of -Extensions). Independently,
    when -Target is not itself specified, -All also acts as a shorthand for -Target
    Artifacts,Logs,Root — the two meanings do not interfere: `-Target Artifacts -All` cleans only
    the Artifacts target, with every extension, not all three targets.

.PARAMETER Recursive
    Switch to pass to Clear-Logs.ps1 only. Has no effect on the Root or Artifacts targets, since
    those scripts no longer accept it (root is always top-level-only, artifacts is always
    recursive).

.EXAMPLE
    .\Run-Clean.ps1 -Target Logs,Artifacts
    Runs log and artifact cleanup.

.EXAMPLE
    .\Run-Clean.ps1 -All
    Runs log, artifact, and root cleanup (Target defaults to all three) with every extension.

.EXAMPLE
    .\Run-Clean.ps1 -Target Root -WhatIf
    Previews root cleanup without deleting anything.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [ValidateSet('Artifacts', 'Logs', 'Root')]
    [string[]]$Target = @(),
    [string[]]$Extensions,
    [switch]$All,
    [switch]$Recursive
)

$scriptDir = $PSScriptRoot

# -All is a shorthand for selecting every target, but only when -Target itself was not given.
# Independently of that, -All is also always forwarded to children below as their own "-All
# extensions" switch (see .PARAMETER All) — the two meanings do not interact.
$effectiveTargets = $Target
if (-not $effectiveTargets -or $effectiveTargets.Count -eq 0) {
    if ($All) {
        $effectiveTargets = @('Artifacts', 'Logs', 'Root')
    }
}

if (-not $effectiveTargets -or $effectiveTargets.Count -eq 0) {
    Write-Warning "No targets specified. Use -Target Artifacts, Logs, and/or Root, or -All for all three."
    return
}

# Parameters accepted by every child script.
$commonParams = @{
    All = $All
}

# If user specifies Extensions, add them
if ($Extensions) {
    $commonParams['Extensions'] = $Extensions
}

# Clear-Logs.ps1 still accepts -Recursive; the others no longer do.
$logsParams = $commonParams.Clone()
$logsParams['Recursive'] = $Recursive

if ($Recursive -and ($effectiveTargets -contains 'Root' -or $effectiveTargets -contains 'Artifacts')) {
    Write-Verbose "-Recursive has no effect on Root or Artifacts targets: Clear-Root.ps1 is always top-level-only and Clear-Artifacts.ps1 is always recursive."
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

if ($effectiveTargets -contains 'Logs') {
    Invoke-CleanupScript "Clear-Logs.ps1" $logsParams
}

if ($effectiveTargets -contains 'Artifacts') {
    Invoke-CleanupScript "Clear-Artifacts.ps1" $commonParams
}

if ($effectiveTargets -contains 'Root') {
    Invoke-CleanupScript "Clear-Root.ps1" $commonParams
}

Write-Host "Run-Clean Sequence Complete." -ForegroundColor Green
