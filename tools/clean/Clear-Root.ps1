<#
.SYNOPSIS
    Cleans up files from the repository root directory.

.DESCRIPTION
    Deletes files with specified extensions from the repository root via the shared
    Clear-ToolDirectory helper (tools/.shared/clean.ps1, F-38). This script ONLY ever touches the
    top-level of the repository root — it never recurses into subdirectories, and never accepts a
    -Recursive switch. That is a deliberate, permanent constraint (F-27): recursing from root with
    generic extensions (like txt/log) has no safe way to exclude directories such as .git, so
    recursion is not offered as an option here, and this script never passes -AllowRecurse to the
    shared helper — there is no parameter combination that makes this script recursive.

.PARAMETER Extensions
    A list of file extensions to delete. Default is 'txt', 'log'.
    Example: -Extensions 'tmp'

.PARAMETER All
    Kept as a documented no-op for CLI compatibility with Run-Clean.ps1 -All.
    It has no effect on this script's behavior: -Extensions already controls
    exactly which files are targeted.

.EXAMPLE
    .\Clear-Root.ps1
    Deletes *.txt and *.log files in the root folder only.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('txt', 'log'),
    [switch]$All
)

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'clean.ps1')

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

# Names that must never be matched, even at the top level. Top-level-only
# enumeration with -File already excludes directories like .git, but this list
# is belt-and-suspenders in case that restriction is ever loosened later
# without re-reading this comment.
$excludedNames = @('.git', '.claude', 'node_modules', 'bin', 'obj', '.vs', '.idea', 'packages')

if ($All) {
    # -All is accepted for CLI compatibility with Run-Clean.ps1 but is a no-op here:
    # -Extensions already fully determines what gets targeted, and root cleanup
    # never defaults to '*' regardless of this switch (see the guard below).
}

Write-Host "Cleaning root directory: $repoRoot" -ForegroundColor Cyan

# SAFETY: Never allow a bare '*' filter on root, full stop (F-27). Filtered out here, before
# reaching the shared helper — Clear-ToolDirectory has no opinion of its own on whether '*' is a
# safe filter for a given directory, so this script is the one place that must refuse it.
$safeExtensions = @($Extensions | Where-Object {
        $filter = if ($_ -eq '*') { '*' } elseif ($_ -like '*.*') { $_ } else { "*.$_" }
        if ($filter -eq '*') {
            Write-Warning "Deleting '*' from Root is dangerous. Skipping. Specify extensions explicitly if needed."
            return $false
        }
        return $true
    })

if ($safeExtensions.Count -gt 0) {
    Clear-ToolDirectory -Path $repoRoot -Extensions $safeExtensions -ItemLabel 'Root File' -ExcludeNames $excludedNames
}

Write-Host "Cleanup Root Complete." -ForegroundColor Green
