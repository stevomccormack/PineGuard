<#
.SYNOPSIS
    Run Util02 Test Data Tuple Scan

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER RepoRoot
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$invokeArgs = @{
    AuditRuleId = 'Util02'
    RepoRoot    = $repoRootResolved
    OutputPath  = 'artifacts/audit/util/Util02-testdata-tuple-scan.txt'
}

& (Join-Path $PSScriptRoot '..\helpers\Test-TestDataTuples.ps1') @invokeArgs
