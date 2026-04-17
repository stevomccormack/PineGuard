<#
.SYNOPSIS
    Test Rule08 Ordering

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER AllowViolations
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [switch]$AllowViolations
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
. (Join-Path $PSScriptRoot 'Load-Catalog.ps1')
$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule08' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule08." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule08' -Title 'Cross-layer method ordering parity (Rules/Must/Guard/FV/DA)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$invokeArgs = @{
    AuditRuleId     = 'Rule08'
    OutputPath      = $outputPath
    RepoRoot        = $repoRootResolved
    AllowViolations = $AllowViolations.IsPresent
}

& (Join-Path $PSScriptRoot '..\helpers\Test-SpecOrdering.ps1') @invokeArgs
