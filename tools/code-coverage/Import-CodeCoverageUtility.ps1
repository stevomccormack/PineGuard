<#
.SYNOPSIS
    Import Code Coverage Utility

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Aggregator that dot-sources shared helpers from tools/.shared/.
#>

Set-StrictMode -Version Latest

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

$script:SharedDir = Join-Path $PSScriptRoot '..\.shared'

. (Join-Path $script:SharedDir 'path.ps1')
. (Join-Path $script:SharedDir 'html.ps1')
. (Join-Path $script:SharedDir 'dotnet-projects.ps1')
. (Join-Path $script:SharedDir 'dotnet-tools-reportgenerator.ps1')
. (Join-Path $script:SharedDir 'coverage.ps1')
