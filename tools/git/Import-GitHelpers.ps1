<#
.SYNOPSIS
    Import Git Helpers

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Aggregator that dot-sources shared helpers from tools/.shared/.
#>

[CmdletBinding()]
param(
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\.shared\git.ps1')
