<#
.SYNOPSIS
    Test Rule04 Fluent Usage

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER Configuration
    See the param block for details.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER FailOnFindings
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
. (Join-Path $PSScriptRoot 'Load-Catalog.ps1')
$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule04' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule04." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule04' -Title 'MustClauses → FluentValidation mapping (usage scan)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$invokeArgs = @{
    AuditRuleId   = 'Rule04'
    Configuration = $Configuration
    OutputPath    = $outputPath
    RepoRoot      = $repoRootResolved
    FailOnFindings = $FailOnFindings.IsPresent
}

& (Join-Path $PSScriptRoot '..\helpers\Find-UnusedMustFluent.ps1') @invokeArgs
