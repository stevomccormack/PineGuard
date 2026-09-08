<#
.SYNOPSIS
    Cleans up log files from the logs directory.

.DESCRIPTION
    Deletes files with specified extensions from the 'logs' directory via the shared
    Clear-ToolDirectory helper (tools/.shared/clean.ps1, F-38). Supports recursive deletion and a
    safety switch to target all files.

.PARAMETER Extensions
    A list of file extensions to delete. Default is 'txt', 'log'.
    Example: -Extensions 'log', 'tmp'

.PARAMETER All
    If specified, deletes *all* items in the logs directory (equivalent to Extensions '*').

.PARAMETER Recursive
    If specified, searches subdirectories of the logs directory.

.EXAMPLE
    .\Clear-Logs.ps1
    Deletes *.txt and *.log files in the logs directory.

.EXAMPLE
    .\Clear-Logs.ps1 -Recursive -All
    Deletes everything in logs and its subdirectories.
#>
[CmdletBinding(SupportsShouldProcess)]
param(
    [string[]]$Extensions = @('txt', 'log'),
    [switch]$All,
    [switch]$Recursive
)

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'clean.ps1')

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

Clear-ToolDirectory -Path $logsDir -Extensions $Extensions -ItemLabel 'Log File' -AllowRecurse:$Recursive

Write-Host "Cleanup Logs Complete." -ForegroundColor Green
