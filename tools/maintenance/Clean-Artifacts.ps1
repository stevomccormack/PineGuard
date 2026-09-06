<#
.SYNOPSIS
    Cleans up files from the artifacts directory.

.DESCRIPTION
    This script deletes files with specified extensions from the 'artifacts' directory,
    always recursing into subdirectories. Recursion is unconditional (not optional)
    because artifacts are inherently nested report folders (coverage reports,
    diagnostics output, etc.) — a non-recursive clean would leave those behind and
    defeat the purpose of this script.

.PARAMETER Extensions
    A list of file extensions to delete. Default is '*' (everything).
    Example: -Extensions 'json', 'md'

.PARAMETER All
    No-op if Extensions is default, otherwise forces specific extensions to be ignored and * used.
    Provided for consistency with other scripts.

.EXAMPLE
    .\Cleanup-Artifacts.ps1
    Recursively deletes ALL files in artifacts and its subdirectories.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('*'),
    [switch]$All
)

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$artifactsDir = Join-Path $repoRoot 'artifacts'

if (-not (Test-Path $artifactsDir)) {
    Write-Warning "Artifacts directory not found: $artifactsDir"
    return
}

if ($All) {
    $Extensions = @('*')
}

Write-Host "Cleaning artifacts in: $artifactsDir" -ForegroundColor Cyan

foreach ($ext in $Extensions) {
    # Ensure extension has wildcard if not present (unless it is exactly '*')
    $filter = if ($ext -eq '*') { '*' } elseif ($ext -like '*.*') { $ext } else { "*.$ext" }

    $params = @{
        Path    = $artifactsDir
        Filter  = $filter
        File    = $true
        Force   = $true
        Recurse = $true
    }

    $files = Get-ChildItem @params

    foreach ($file in $files) {
        if ($PSCmdlet.ShouldProcess($file.FullName, "Delete Artifact File")) {
            Remove-Item -LiteralPath $file.FullName -Force
            Write-Host "Deleted: $($file.Name)" -ForegroundColor Gray
        }
    }
}

Write-Host "Cleanup Artifacts Complete." -ForegroundColor Green
