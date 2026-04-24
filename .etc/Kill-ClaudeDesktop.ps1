<#
.SYNOPSIS
    Kills all Claude Desktop (Windows Store app) processes.

.DESCRIPTION
    Targets ONLY the Claude Desktop app installed from the Microsoft Store
    (WindowsApps\Claude_*). Does NOT touch:
      - Claude Code CLI terminals (AppData\Roaming\Claude\claude-code\*)
      - Claude Code VS Code extension (.vscode\extensions\anthropic.claude-code-*)

.PARAMETER ReOpen
    Re-launch Claude Desktop after killing it.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/Kill-ClaudeDesktop.ps1

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/Kill-ClaudeDesktop.ps1 -ReOpen
#>

param(
    [switch]$ReOpen
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$pattern = [regex]::Escape('Program Files\WindowsApps\Claude_')

$targets = Get-Process -Name 'claude' -ErrorAction SilentlyContinue |
    Where-Object { $_.Path -and $_.Path -match $pattern }

if (-not $targets -or $targets.Count -eq 0) {
    Write-Host 'No Claude Desktop processes found.' -ForegroundColor DarkGray
    exit 0
}

Write-Host "Found $($targets.Count) Claude Desktop process(es):" -ForegroundColor Cyan
$targets | ForEach-Object {
    Write-Host "  PID $($_.Id)  $($_.Path)" -ForegroundColor White
}

Write-Host ''
Write-Host 'Stopping...' -NoNewline

$targets | Stop-Process -Force

if ($ReOpen) {
    $exe = ($targets | Select-Object -First 1).Path
    Write-Host "Re-opening Claude Desktop..." -ForegroundColor Cyan
    Start-Process -FilePath $exe
    Write-Host 'Launched.' -ForegroundColor Green
}

Write-Host ' Done' -ForegroundColor Green
