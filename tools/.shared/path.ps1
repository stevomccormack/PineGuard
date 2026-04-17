<#
.SYNOPSIS
    Shared filesystem path helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Get-RepoRoot and Ensure-Directory
    into the calling script's scope.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-RepoRoot {
    <#
    .SYNOPSIS
        Walks up from StartDirectory until PineGuard.slnx is found; returns the repo root path.
    #>
    [CmdletBinding()]
    param(
        [string] $StartDirectory = $PSScriptRoot
    )

    $dir = $StartDirectory
    while ($true) {
        if (Test-Path (Join-Path $dir 'PineGuard.slnx')) {
            return $dir
        }

        $parent = Split-Path -Parent $dir
        if ([string]::IsNullOrWhiteSpace($parent) -or $parent -eq $dir) {
            throw "Could not locate repo root (expected to find PineGuard.slnx). Started at: $StartDirectory"
        }

        $dir = $parent
    }
}

function Ensure-Directory {
    <#
    .SYNOPSIS
        Creates a directory (and parents) if it does not already exist.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Path
    )

    New-Item -ItemType Directory -Path $Path -Force | Out-Null
}
