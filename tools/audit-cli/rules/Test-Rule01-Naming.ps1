<#
.SYNOPSIS
    Test Rule01 Naming

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER SpecPath
    See the param block for details.

.PARAMETER AllowViolations
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [ValidateNotNullOrEmpty()] [string]$SpecPath = 'artifacts/audit/naming-spec.json',
    [switch]$AllowViolations
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
. (Join-Path $PSScriptRoot 'Load-Catalog.ps1')
$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule01' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule01." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule01' -Title 'MustClauses nullability + overload-collision audit' -RepoRoot $repoRootResolved -OutputPath $outputPath

$invokeArgs = @{
    AuditRuleId     = 'Rule01'
    Project         = 'MustClauses'
    SpecPath        = $SpecPath
    OutputPath      = $outputPath
    AllowViolations = $AllowViolations.IsPresent
    RepoRoot        = $repoRootResolved
}

& (Join-Path $PSScriptRoot '..\helpers\Test-SpecNaming.ps1') @invokeArgs

