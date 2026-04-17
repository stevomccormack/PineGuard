<#
.SYNOPSIS
    Run All Audit Rules

.DESCRIPTION
    Runs the full PineGuard audit rule catalog.

    This is the preferred entrypoint.

    Related entrypoints:
    - tools/audit-cli/Run-AuditLibraryRules.ps1
    - tools/audit-cli/Run-AuditTestingRules.ps1

    Legacy compatibility entrypoint:
    - tools/audit-cli/Run-AuditRules.ps1

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

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'helpers\Load-AuditOrchestrator.ps1')

Invoke-PineGuardAuditRules -AuditCliRoot $PSScriptRoot @PSBoundParameters
