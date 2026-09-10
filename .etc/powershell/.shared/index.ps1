<#
.SYNOPSIS
    Bootstrap loader for the PineGuard maintainer PowerShell scripts.

.DESCRIPTION
    Dot-source this file as the first statement of every script in .etc/powershell. It brings
    three things into the caller's scope, in this order:

      1. Get-RepoRoot            - from tools/.shared/path.ps1, the repository's own resolver.
      2. The Onboarding library  - Write-MastHead, Write-Var, New-GitHubProject, Test-Command,
                                   Set-EnvironmentVariable, and the rest of the maintainer's
                                   personal helper library, loaded from a SEPARATE repository.
      3. This folder's variables - $Project, $Solution, $SonarQube, and the GitHub helpers.

    Locating the Onboarding repository (it is not vendored here, by design):
      1. $Env:PINEGUARD_ONBOARDING_ROOT, when set.
      2. A directory named 'Onboarding' beside any ancestor of this repository's root.
    The walk is deliberately not a single 'look at my parent' check: inside a git worktree under
    .claude/worktrees/<name>, Get-RepoRoot returns the worktree, whose parent is
    .claude/worktrees - four levels below the folder that actually holds Onboarding.
    If neither step resolves, this file throws naming both, rather than failing later with a
    confusing "term 'Write-MastHead' is not recognized".

    Onboarding's own index.ps1 dot-sources ITS files with paths relative to the current
    directory, so it is loaded inside a Push-Location/Pop-Location pair. Everything this repo
    owns is resolved from $PSScriptRoot instead, so these scripts work from any working
    directory - the previous version only worked when invoked from the repository root.

    Onboarding's banner output is suppressed so callers do not have to write
    `. index.ps1 *> $null` themselves - a redirection that also swallowed genuine load errors.
    Set $Env:PINEGUARD_SHELL_VERBOSE to see it.

.NOTES
    Secrets policy (D-4 / F-24 / F-25): .env values are loaded into the CURRENT PROCESS only.
    Import-DotEnv defaults to -Scope User, which persists every key - including NUGET_TOKEN,
    GH_TOKEN, QODANA_TOKEN - into the Windows registry. This loader passes -Scope Process
    explicitly so a token never outlives the session that needed it. tools/.shared/secret.ps1
    resolves $env: before .env, so a process-scoped load is all the repo tooling needs.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Repository root (via the repository's own resolver, not a hard-coded path)
# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '../../../tools/.shared/path.ps1')

$PineGuardRepoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$PineGuardShellRoot = Split-Path -Parent $PSScriptRoot
$PineGuardDotEnvPath = Join-Path $PineGuardShellRoot '.env'

# -------------------------------------------------------------------------------------------------
# Onboarding library (external repository)
# -------------------------------------------------------------------------------------------------

function Resolve-OnboardingRoot {
    <#
    .SYNOPSIS
        Locates the maintainer's Onboarding repository, or throws explaining both attempts.

    .PARAMETER RepoRoot
        This repository's root. Its ancestors are searched for a sibling 'Onboarding' directory,
        so the lookup works from a plain clone and from a git worktree alike.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot
    )

    $fromEnv = [Environment]::GetEnvironmentVariable('PINEGUARD_ONBOARDING_ROOT', 'Process')
    if (-not [string]::IsNullOrWhiteSpace($fromEnv)) {
        if (Test-Path -LiteralPath (Join-Path $fromEnv '.shared/index.ps1')) {
            return (Resolve-Path -LiteralPath $fromEnv).Path
        }

        throw ("`$Env:PINEGUARD_ONBOARDING_ROOT is set to '$fromEnv', but no '.shared/index.ps1' " +
            'exists there.')
    }

    $searched = @()
    $ancestor = $RepoRoot
    while (-not [string]::IsNullOrWhiteSpace($ancestor)) {
        $candidate = Join-Path $ancestor 'Onboarding'
        $searched += $candidate

        if (Test-Path -LiteralPath (Join-Path $candidate '.shared/index.ps1')) {
            return (Resolve-Path -LiteralPath $candidate).Path
        }

        $parent = Split-Path -Parent $ancestor
        if ($parent -eq $ancestor) { break }
        $ancestor = $parent
    }

    throw ('Could not locate the Onboarding helper library, which every script in ' +
        '.etc/powershell depends on. Tried: (1) $Env:PINEGUARD_ONBOARDING_ROOT, which is not ' +
        'set; (2) a sibling ''Onboarding'' beside each ancestor of the repository root, none of ' +
        'which has a ''.shared/index.ps1'': ' + ($searched -join '; ') + '. Clone the ' +
        'Onboarding repository beside this one, or point PINEGUARD_ONBOARDING_ROOT at it.')
}

$PineGuardOnboardingRoot = Resolve-OnboardingRoot -RepoRoot $PineGuardRepoRoot
$onboardingIndex = Join-Path $PineGuardOnboardingRoot '.shared/index.ps1'

Push-Location -LiteralPath $PineGuardOnboardingRoot
try {
    if ([string]::IsNullOrWhiteSpace($Env:PINEGUARD_SHELL_VERBOSE)) {
        . $onboardingIndex *> $null
    }
    else {
        . $onboardingIndex
    }
}
catch {
    throw ("Failed to load the Onboarding library from '{0}': {1}" -f $onboardingIndex, $_.Exception.Message)
}
finally {
    Pop-Location
}

# -------------------------------------------------------------------------------------------------
# This repository's .env - process scope only (see .NOTES above)
# -------------------------------------------------------------------------------------------------

if (Test-Path -LiteralPath $PineGuardDotEnvPath) {
    $null = Import-DotEnv -Path $PineGuardDotEnvPath -Scope Process -Force
}

# -------------------------------------------------------------------------------------------------
# This folder's own variables and helpers
# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot 'project.ps1')
. (Join-Path $PSScriptRoot 'solution.ps1')
. (Join-Path $PSScriptRoot 'sonarqube.ps1')
. (Join-Path $PSScriptRoot 'github.ps1')
