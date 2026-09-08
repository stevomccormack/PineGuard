<#
.SYNOPSIS
    Shared console-output helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Write-Step, Write-Success, Write-Warn, Write-Fail, and
    Write-Detail into the calling script's scope.

    Consolidates the console-helper functions that used to be redefined separately (with
    differing names and colours) in tools/release/Run-GithubRelease.ps1, Run-GithubRuleset.ps1,
    Run-NugetUnlist.ps1, and tools/clean/Test-StructuralIntegrity.ps1 (F-37). Write-Fail only
    writes; it does not exit — callers that want "log and stop" compose it themselves (e.g. a local
    `Fail` wrapper that calls Write-Fail then `exit 1`), because how "stop" should look (immediate
    exit vs. accumulating an issue count) is a caller decision, not a console-output one.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Step {
    <#
    .SYNOPSIS
        Announces the start of a new step or phase.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Message
    )

    Write-Host "==> $Message" -ForegroundColor Cyan
}

function Write-Success {
    <#
    .SYNOPSIS
        Reports that a step completed successfully.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Message
    )

    Write-Host "    OK   $Message" -ForegroundColor Green
}

function Write-Warn {
    <#
    .SYNOPSIS
        Reports a non-fatal warning.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Message
    )

    Write-Host "    WARN $Message" -ForegroundColor Yellow
}

function Write-Fail {
    <#
    .SYNOPSIS
        Reports a failure. Does not exit or throw — see .DESCRIPTION above.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Message
    )

    Write-Host "    FAIL $Message" -ForegroundColor Red
}

function Write-Detail {
    <#
    .SYNOPSIS
        Writes a neutral, indented detail line under the current step.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Message
    )

    Write-Host "    $Message" -ForegroundColor Gray
}
