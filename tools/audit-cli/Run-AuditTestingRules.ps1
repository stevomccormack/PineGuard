<#
.SYNOPSIS
    Run Audit Testing Rules

.DESCRIPTION
    Runs PineGuard audit rules for unit testing conventions (Rule50..Rule54).

    If -Rule or -RuleName is provided, the explicit filter overrides the default subset.

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

if ((-not $RuleId -or $RuleId.Count -eq 0) -and (-not $RuleName -or $RuleName.Count -eq 0)) {
    $RuleId = @('Rule50', 'Rule51', 'Rule52', 'Rule53', 'Rule54')
}

Invoke-PineGuardAuditRules -AuditCliRoot $PSScriptRoot @PSBoundParameters
