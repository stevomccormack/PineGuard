<#
.SYNOPSIS
    Run Audit Rules

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

    Compatibility entrypoint.
    Preferred entrypoints:
    - tools/audit-cli/Run-All.ps1
    - tools/audit-cli/Run-AuditLibraryRules.ps1
    - tools/audit-cli/Run-AuditTestingRules.ps1

.PARAMETER Configuration
    See the param block for details.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER AllowViolations
    See the param block for details.

.PARAMETER ListRules
    See the param block for details.

.PARAMETER NoCatalog
    See the param block for details.

.PARAMETER ContinueOnError
    See the param block for details.

.PARAMETER ShowFailures
    See the param block for details.

.PARAMETER NoSummary
    See the param block for details.

.PARAMETER JsonSummaryPath
    See the param block for details.

.PARAMETER RuleId
    See the param block for details.

.PARAMETER RuleName
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [switch]$AllowViolations,
    [switch]$ListRules,
    [switch]$NoCatalog,
    [switch]$ContinueOnError,
    [switch]$ShowFailures,
    [switch]$NoSummary,
    [Alias('JsonSummary')] [string]$JsonSummaryPath,
    [Alias('Rule')] [string[]]$RuleId,
    [string[]]$RuleName
)

    . (Join-Path $PSScriptRoot 'helpers\Load-AuditOrchestrator.ps1')

    Invoke-PineGuardAuditRules -AuditCliRoot $PSScriptRoot @PSBoundParameters

Write-Host ''
Write-Host 'Audit rules complete.' -ForegroundColor Green
