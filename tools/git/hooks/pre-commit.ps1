<#
.SYNOPSIS
    Pre-commit hook: formats the staged C# files with dotnet format and re-stages them.
.DESCRIPTION
    Runs only when at least one .cs file is staged, and formats only those files
    (dotnet format --include), so a documentation-only commit runs nothing and the
    rest of the solution is never rewritten.

    Line endings are governed by .gitattributes (eol=lf) and .editorconfig
    (end_of_line = lf) together, so formatting never changes a line ending and git
    never reports phantom "modified" files after this hook runs.
.NOTES
    Invoked by tools/git/hooks/pre-commit (sh shim). Enable once per clone with:
        git config core.hooksPath tools/git/hooks
.EXAMPLE
    pwsh -NoProfile -File tools/git/hooks/pre-commit.ps1
    Runs the hook by hand against whatever is currently staged.
#>
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$repoRoot = (& git rev-parse --show-toplevel).Trim()
$staged = @(@(& git diff --cached --name-only --diff-filter=ACMR -- '*.cs') | Where-Object { $_ })
if ($staged.Count -eq 0) {
    exit 0
}

$solution = Join-Path $repoRoot 'PineGuard.slnx'
Write-Host "pre-commit: dotnet format on $($staged.Count) staged C# file(s)..."
& dotnet format $solution --include @staged --exclude-diagnostics CS1591 --verbosity minimal
if ($LASTEXITCODE -ne 0) {
    Write-Host "pre-commit: dotnet format failed (exit $LASTEXITCODE); commit aborted." -ForegroundColor Red
    exit $LASTEXITCODE
}

# Re-stage only the staged files that formatting actually changed.
$changed = @(@(& git diff --name-only -- @staged) | Where-Object { $_ })
if ($changed.Count -gt 0) {
    & git add -- @changed
    Write-Host "pre-commit: re-staged $($changed.Count) reformatted file(s)."
}
exit 0
