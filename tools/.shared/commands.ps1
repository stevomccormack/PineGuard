<#
.SYNOPSIS
    Shared command-existence helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Test-CommandExists into the calling script's scope.
    Used by tools/docker/* and tools/sonar-scanner/*.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Test-CommandExists {
    <#
    .SYNOPSIS
        Returns $true if the named command is available on PATH.
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string] $Name
    )
    return $null -ne (Get-Command $Name -ErrorAction SilentlyContinue)
}
