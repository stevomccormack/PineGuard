<#
.SYNOPSIS
    Test Rule07 Nullability

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule07' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule07." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule07' -Title 'Hybrid nullability strategy (Must/Guard primary parameter)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$invokeArgs = @{
    AuditRuleId     = 'Rule07'
    RepoRoot        = $repoRootResolved
    OutputPath      = $outputPath
    AllowViolations = $AllowViolations.IsPresent
}

& (Join-Path $PSScriptRoot '..\helpers\Test-SpecNullability.ps1') @invokeArgs
