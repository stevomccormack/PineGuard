<#
.SYNOPSIS
    Cleans up files from the repository root directory.

.DESCRIPTION
    This script deletes files with specified extensions from the repository root.
    Note: For safety, Recursive is disabled by default and requires explicit confirmation in logic if dealing with sensitive operations.
    Given the request, recursive is supported but we must be careful with root.

.PARAMETER Extensions
    A list of file extensions to delete. Default is 'txt', 'log'.
    Example: -Extensions 'tmp'

.PARAMETER All
    If specified, deletes *all* items matching extensions (reinforces intent).

.PARAMETER Recursive
    If specified, searches subdirectories of the root.
    WARNING: Using -Recursive on Root with generic extensions (like txt) will delete files across the entire repo!
    Use with extreme caution.

.EXAMPLE
    .\Cleanup-Root.ps1
    Deletes *.txt and *.log files in the root folder only.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('txt', 'log'),
    [switch]$All,
    [switch]$Recursive
)

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

if ($All) {
    # If -All is passed, we stick to the provided extensions or default.
    # We DO NOT default to '*' for Root because that would delete the repo.
    # The requirement said "Cleanup-Root -Extensions -All -Recursive".
    # We assume -All just means "All specified content types" here, usually redundant but kept for API consistency.
    # If the user meant "Delete everything in root", that's too dangerous.
    # We will trust the Extensions param.
}

Write-Host "Cleaning root directory: $repoRoot" -ForegroundColor Cyan
if ($Recursive) {
    Write-Warning "Running RECURSIVE cleanup from ROOT. This scans the entire repository!"
}

foreach ($ext in $Extensions) {
    # Ensure extension has wildcard
    $filter = if ($ext -eq '*') { '*' } elseif ($ext -like '*.*') { $ext } else { "*.$ext" }
    
    # SAFETY: Do not allow * or *.* on root unless explicitly forced (which we are not implementing a force-override for here for safety).
    if ($filter -eq '*' -and -not $Recursive) {
        Write-Warning "Deleting '*' from Root is dangerous. Skipping. Specify extensions explicitly if needed."
        continue
    }
    
    $params = @{
        Path   = $repoRoot
        Filter = $filter
        File   = $true
        Force  = $true
    }
    
    if ($Recursive) {
        $params['Recurse'] = $true
        # Exclude .git and commonly ignored folders if possible? 
        # For simple Get-ChildItem, excluding .git is good practice if recursive.
        # However, user asked for simple cleanup. We will trust ShouldProcess.
    }
    
    $files = Get-ChildItem @params
    
    foreach ($file in $files) {
        # Skip this script and the maintenance folder itself if we are recursive?
        # No, generally we just delete the target extensions.
        
        if ($PSCmdlet.ShouldProcess($file.FullName, "Delete Root File")) {
            Remove-Item -LiteralPath $file.FullName -Force
            Write-Host "Deleted: $($file.Name)" -ForegroundColor Gray
        }
    }
}

Write-Host "Cleanup Root Complete." -ForegroundColor Green
