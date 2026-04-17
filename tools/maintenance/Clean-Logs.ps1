<#
.SYNOPSIS
    Cleans up log files from the logs directory.

.DESCRIPTION
    This script deletes files with specified extensions from the 'logs' directory.
    It supports recursive deletion and a safety switch to target all files.

.PARAMETER Extensions
    A list of file extensions to delete. Default is 'txt', 'log'.
    Example: -Extensions 'log', 'tmp'

.PARAMETER All
    If specified, deletes *all* items in the logs directory (equivalent to Extensions '*').

.PARAMETER Recursive
    If specified, searches subdirectories of the logs directory.

.EXAMPLE
    .\Cleanup-Logs.ps1
    Deletes *.txt and *.log files in the logs directory.

.EXAMPLE
    .\Cleanup-Logs.ps1 -Recursive -All
    Deletes everything in logs and its subdirectories.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('txt', 'log'),
    [switch]$All,
    [switch]$Recursive
)

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$logsDir = Join-Path $repoRoot 'logs'

if (-not (Test-Path $logsDir)) {
    Write-Warning "Logs directory not found: $logsDir"
    return
}

if ($All) {
    $Extensions = @('*')
}

Write-Host "Cleaning logs in: $logsDir" -ForegroundColor Cyan

foreach ($ext in $Extensions) {
    # Ensure extension has wildcard if not present (unless it is exactly '*')
    $filter = if ($ext -eq '*') { '*' } elseif ($ext -like '*.*') { $ext } else { "*.$ext" }
    
    $params = @{
        Path   = $logsDir
        Filter = $filter
        File   = $true
        Force  = $true
    }
    
    if ($Recursive) {
        $params['Recurse'] = $true
    }
    
    $files = Get-ChildItem @params
    
    foreach ($file in $files) {
        if ($PSCmdlet.ShouldProcess($file.FullName, "Delete Log File")) {
            Remove-Item -LiteralPath $file.FullName -Force
            Write-Host "Deleted: $($file.Name)" -ForegroundColor Gray
        }
    }
}

Write-Host "Cleanup Logs Complete." -ForegroundColor Green
