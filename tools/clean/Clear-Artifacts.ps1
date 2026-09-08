<#
.SYNOPSIS
    Cleans up files from the artifacts directory.

.DESCRIPTION
    Deletes files with specified extensions from the 'artifacts' directory via the shared
    Clear-ToolDirectory helper (tools/.shared/clean.ps1, F-38), always recursing into
    subdirectories. Recursion is unconditional (not optional) because artifacts are inherently
    nested report folders (coverage reports, diagnostics output, etc.) — a non-recursive clean
    would leave those behind and defeat the purpose of this script.

.PARAMETER Extensions
    A list of file extensions to delete. Default is '*' (everything).
    Example: -Extensions 'json', 'md'

.PARAMETER All
    No-op if Extensions is default, otherwise forces specific extensions to be ignored and * used.
    Provided for consistency with other scripts.

.EXAMPLE
    .\Clear-Artifacts.ps1
    Recursively deletes ALL files in artifacts and its subdirectories.

.EXAMPLE
    .\Clear-Artifacts.ps1 -Extensions 'json' -WhatIf
    Previews deleting only *.json files under artifacts, recursively.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('*'),
    [switch]$All
)

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
. (Join-Path $PSScriptRoot '..\.shared\clean.ps1')

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

Clear-ToolDirectory -Path $artifactsDir -Extensions $Extensions -ItemLabel 'Artifact File' -AllowRecurse

Write-Host "Cleanup Artifacts Complete." -ForegroundColor Green
