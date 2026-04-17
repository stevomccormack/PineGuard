<#
.SYNOPSIS
    Commit Solution

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER DryRun
    See the param block for details.

.PARAMETER AutoMessage
    See the param block for details.

.PARAMETER Message
    See the param block for details.
#>

[CmdletBinding()]
param(
    [switch]$DryRun,
    [switch]$AutoMessage,
    [string]$Message
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/Import-GitHelpers.ps1"

$repoRoot = Resolve-RepoRoot

$paths = @(
    'PineGuard.slnx',
    'PineGuard.sln.DotSettings',
    'Directory.Packages.props',
    '.editorconfig',
    '.gitignore',
    'NuGet.config',
    'LICENSE',
    'README.md'
)

Invoke-Commit -RepoRoot $repoRoot -Title 'Solution: updates' -StagePaths $paths -WhatIf:$DryRun -AutoMessage:$AutoMessage -Message $Message
