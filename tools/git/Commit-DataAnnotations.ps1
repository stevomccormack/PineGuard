<#
.SYNOPSIS
    Commit Data Annotations

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

. "$PSScriptRoot/../.shared/path.ps1"
. "$PSScriptRoot/../.shared/git.ps1"

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

$paths = @('src/PineGuard.DataAnnotations')
if ($IncludeTests.IsPresent) {
    $paths += 'tests/PineGuard.DataAnnotations.UnitTests'
}

Invoke-Commit -RepoRoot $repoRoot -Title 'DataAnnotations: updates' -StagePaths $paths -WhatIf:$DryRun -AutoMessage:$AutoMessage -Message $Message
