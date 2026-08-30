<#
.SYNOPSIS
    Commit Agent

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
    '.agent',
    '.claude',
    '.pi',
    '.cursor/rules',
    '.cursorrules',
    '.windsurf',
    '.windsurfrules',
    '.clinerules',
    '.junie',
    '.amazonq',
    '.github/agents',
    '.github/instructions',
    '.github/prompts',
    '.github/skills',
    '.github/workflows',
    '.github/copilot-instructions.md',
    '.vscode/settings.json',
    '.vscode/tasks.json',
    'AGENTS.md',
    'CLAUDE.md',
    'GEMINI.md',
    'src/PineGuard.Core/AGENTS.md',
    'src/PineGuard.MustClauses/AGENTS.md',
    'src/PineGuard.GuardClauses/AGENTS.md',
    'src/PineGuard.FluentValidation/AGENTS.md',
    'src/PineGuard.DataAnnotations/AGENTS.md',
    'src/PineGuard.Extensions.Options/AGENTS.md',
    'src/PineGuard.Extensions.DependencyInjection/AGENTS.md',
    'src/PineGuard.ErrorOr/AGENTS.md',
    'src/PineGuard.FluentResults/AGENTS.md',
    'src/PineGuard.OneOf/AGENTS.md',
    'tests/AGENTS.md',
    'tools/AGENTS.md',
    'tools/code-diagnostics/AGENTS.md',
    'tools/sonar-scanner/AGENTS.md'
)

Invoke-Commit -RepoRoot $repoRoot -Title 'Repo: automation updates' -StagePaths $paths -WhatIf:$DryRun -AutoMessage:$AutoMessage -Message $Message
