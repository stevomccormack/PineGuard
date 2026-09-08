<#
.SYNOPSIS
    Shared filesystem path helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Get-RepoRoot into the calling script's scope.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-RepoRoot {
    <#
    .SYNOPSIS
        Resolves the PineGuard repository root, starting from StartDirectory.

    .DESCRIPTION
        The single repo-root resolver for the whole tools/ tree (F-07). It replaces four prior
        implementations: this file's own former PineGuard.slnx-only walk, tools/.shared/git.ps1's
        git-only Resolve-RepoRoot, tools/audit-cli/helpers/Load-AuditHelpers.ps1's
        Resolve-PineGuardRepoRoot, and an inline copy in tools/clean/Test-StructuralIntegrity.ps1.

        Resolution order:
          1. `git -C StartDirectory rev-parse --show-toplevel` — fast and authoritative whenever
             StartDirectory is inside a git working tree, which is true for every caller in this
             repo today.
          2. Falls back to walking up from StartDirectory looking for PineGuard.slnx, for the
             edge case where git is unavailable or StartDirectory is not inside a git working
             tree (e.g. an extracted archive with no .git folder).
        Throws, naming both things it tried, if neither resolves.

    .PARAMETER StartDirectory
        Directory to start resolution from. Callers should pass their own $PSScriptRoot — the
        default here resolves to this file's own directory (tools/.shared), not the calling
        script's location, because $PSScriptRoot inside a function is bound to the script that
        defines the function.
    #>
    [CmdletBinding()]
    param(
        [string] $StartDirectory = $PSScriptRoot
    )

    if (Get-Command git -ErrorAction SilentlyContinue) {
        $gitRoot = $null
        try {
            $gitRoot = & git -C $StartDirectory rev-parse --show-toplevel 2>$null
        }
        catch {
            $gitRoot = $null
        }

        if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($gitRoot)) {
            return ($gitRoot.Trim() -replace '/', [System.IO.Path]::DirectorySeparatorChar)
        }
    }

    $dir = $StartDirectory
    while ($true) {
        if (Test-Path (Join-Path $dir 'PineGuard.slnx')) {
            return $dir
        }

        $parent = Split-Path -Parent $dir
        if ([string]::IsNullOrWhiteSpace($parent) -or $parent -eq $dir) {
            throw ("Could not locate the repo root starting from '{0}'. Tried: (1) 'git -C <dir> " +
                "rev-parse --show-toplevel', which failed or returned nothing (not inside a git " +
                "working tree, or git unavailable); (2) walking up looking for PineGuard.slnx, " +
                "which was not found in any parent directory." -f $StartDirectory)
        }

        $dir = $parent
    }
}
