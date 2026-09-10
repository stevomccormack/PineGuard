<#
.SYNOPSIS
    Stop every Claude Desktop (Microsoft Store app) process, optionally relaunching it.

.DESCRIPTION
    Targets ONLY the Claude Desktop app installed from the Microsoft Store, identified by an
    image path under Program Files\WindowsApps\Claude_. It deliberately does NOT touch:

      - Claude Code CLI terminals      (AppData\Roaming\Claude\claude-code\*)
      - the Claude Code VS Code extension (.vscode\extensions\anthropic.claude-code-*)

    both of which also run an executable named 'claude', which is why the path filter exists.

    Standalone by design: it dot-sources only tools/.shared/console.ps1 and needs neither the
    Onboarding library nor a loaded $Project, so it still runs when the rest of the environment
    is in a state bad enough to need Claude Desktop restarting.

.PARAMETER Restart
    Relaunch Claude Desktop from the same executable once every process has exited. -ReOpen is a
    supported alias of the same switch.

.PARAMETER TimeoutSeconds
    How long to wait for the processes to exit before giving up. Default: 10.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./.etc/powershell/Stop-ClaudeDesktop.ps1

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./.etc/powershell/Stop-ClaudeDesktop.ps1 -Restart

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./.etc/powershell/Stop-ClaudeDesktop.ps1 -WhatIf
    Lists the processes that would be stopped without stopping any of them.
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [Alias('ReOpen')]
    [switch] $Restart,

    [ValidateRange(1, 120)]
    [int] $TimeoutSeconds = 10
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '../../tools/.shared/console.ps1')

if (-not $IsWindows) {
    Write-Fail 'Claude Desktop is a Microsoft Store app; this script only runs on Windows.'
    exit 1
}

$storeAppPattern = [regex]::Escape('Program Files\WindowsApps\Claude_')

$targets = @(
    Get-Process -Name 'claude' -ErrorAction SilentlyContinue |
        Where-Object { $_.Path -and $_.Path -match $storeAppPattern }
)

if ($targets.Count -eq 0) {
    Write-Detail 'No Claude Desktop processes found.'
    exit 0
}

Write-Step "Found $($targets.Count) Claude Desktop process(es)"
foreach ($target in $targets) {
    Write-Detail ("PID {0,-8} {1}" -f $target.Id, $target.Path)
}

# Captured before Stop-Process so -Restart cannot depend on reading a property off an exited
# process object.
$executablePath = $targets[0].Path

if (-not $PSCmdlet.ShouldProcess("$($targets.Count) Claude Desktop process(es)", 'Stop')) {
    Write-Warn 'WhatIf: nothing was stopped.'
    exit 0
}

$targets | Stop-Process -Force

$deadline = (Get-Date).AddSeconds($TimeoutSeconds)
do {
    $stillRunning = @($targets | Where-Object { -not $_.HasExited })
    if ($stillRunning.Count -eq 0) { break }
    Start-Sleep -Milliseconds 200
} while ((Get-Date) -lt $deadline)

if ($stillRunning.Count -gt 0) {
    Write-Fail ("{0} process(es) did not exit within {1}s: {2}" -f `
            $stillRunning.Count, $TimeoutSeconds, (($stillRunning | ForEach-Object { $_.Id }) -join ', '))
    exit 1
}

Write-Success "Stopped $($targets.Count) process(es)."

if ($Restart) {
    Write-Step 'Relaunching Claude Desktop'
    Start-Process -FilePath $executablePath
    Write-Success "Launched $executablePath"
}
