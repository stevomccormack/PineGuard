<#
.SYNOPSIS
    Commit Guard Clauses

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER IncludeTests
    See the param block for details.

.PARAMETER DryRun
    See the param block for details.

.PARAMETER AutoMessage
    See the param block for details.

.PARAMETER Message
    See the param block for details.
#>

[CmdletBinding()]
param(
    [switch]$IncludeTests,
    [switch]$DryRun,
    [switch]$AutoMessage,
    [string]$Message
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/Import-GitHelpers.ps1"

$repoRoot = Resolve-RepoRoot

$paths = @('src/PineGuard.GuardClauses')
if ($IncludeTests.IsPresent) {
    $paths += 'tests/PineGuard.GuardClauses.UnitTests'
}

Invoke-Commit -RepoRoot $repoRoot -Title 'GuardClauses: updates' -StagePaths $paths -WhatIf:$DryRun -AutoMessage:$AutoMessage -Message $Message
