<#
.SYNOPSIS
    Run Util01 Analyze Latest Coverage Data Annotations

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
    AuditRuleId  = 'Util01'
    RepoRoot     = $repoRootResolved
    TargetFilter = '*DataAnnotations*'
    OutputPath   = 'artifacts/audit/util/Util01-latest-coverage-dataannotations.txt'
}

& (Join-Path $PSScriptRoot '..\helpers\Test-CoverageLatest.ps1') @invokeArgs
