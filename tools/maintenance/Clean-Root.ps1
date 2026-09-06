<#
.SYNOPSIS
    Cleans up files from the repository root directory.

.DESCRIPTION
    This script deletes files with specified extensions from the repository root.
    This script ONLY ever touches the top-level of the repository root — it never
    recurses into subdirectories. That is a deliberate, permanent constraint: recursing
    from root with generic extensions (like txt/log) has no safe way to exclude
    directories such as .git, so recursion is not offered as an option here.

.PARAMETER Extensions
    A list of file extensions to delete. Default is 'txt', 'log'.
    Example: -Extensions 'tmp'

.PARAMETER All
    Kept as a documented no-op for CLI compatibility with Run-Clean.ps1 -All.
    It has no effect on this script's behavior: -Extensions already controls
    exactly which files are targeted.

.EXAMPLE
    .\Cleanup-Root.ps1
    Deletes *.txt and *.log files in the root folder only.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('txt', 'log'),
    [switch]$All
)

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
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

foreach ($ext in $Extensions) {
    # Ensure extension has wildcard
    $filter = if ($ext -eq '*') { '*' } elseif ($ext -like '*.*') { $ext } else { "*.$ext" }

    # SAFETY: Never allow a bare '*' filter on root, full stop.
    if ($filter -eq '*') {
        Write-Warning "Deleting '*' from Root is dangerous. Skipping. Specify extensions explicitly if needed."
        continue
    }

    $params = @{
        Path   = $repoRoot
        Filter = $filter
        File   = $true
        Force  = $true
    }

    $files = Get-ChildItem @params

    foreach ($file in $files) {
        if ($excludedNames -contains $file.Name) {
            continue
        }

        if ($PSCmdlet.ShouldProcess($file.FullName, "Delete Root File")) {
            Remove-Item -LiteralPath $file.FullName -Force
            Write-Host "Deleted: $($file.Name)" -ForegroundColor Gray
        }
    }
}

Write-Host "Cleanup Root Complete." -ForegroundColor Green
