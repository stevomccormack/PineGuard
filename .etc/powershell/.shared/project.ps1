<#
.SYNOPSIS
    Declares the $Project variable describing this repository and its GitHub remote.

.DESCRIPTION
    Dot-sourced by index.ps1. Supplies $Project - owner, repository, local path, git identity
    and pull strategy - to every script in .etc/powershell.

    LocalPath is derived from $PineGuardRepoRoot (resolved by index.ps1 via the repository's own
    Get-RepoRoot) rather than hard-coded, so a clone at any path, and any git worktree under
    .claude/worktrees/, resolves correctly.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Project variables
# -------------------------------------------------------------------------------------------------

$Project = New-GitHubProject `
    -Owner 'stevomccormack' `
    -Repository 'PineGuard' `
    -Name 'PineGuard' `
    -Description 'Validation that thinks like you do' `
    -LocalPath $PineGuardRepoRoot `
    -UserName 'Steve McCormack' `
    -UserEmail 'hello@iamstevo.co' `
    -MainBranch 'main' `
    -FastForward $false `
    -UseRebase $true

# -------------------------------------------------------------------------------------------------

if ($Global.Log.Enabled) {
    Write-Header "`$Project variable:"
    $Project | Format-List
}
