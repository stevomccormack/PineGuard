<#
.SYNOPSIS
    Initialize the local Qodana environment.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Installs the Qodana CLI via Winget if not already present, then starts the
    Qodana Docker Compose stack via qodana-up.ps1.

    For CI/CD, use the JetBrains Qodana GitHub Action - do not use this script
    in pipelines.

    Prerequisites: Docker Desktop running, Winget available (Windows 10/11).

.PARAMETER NetworkName
    Docker network name. Default: pineguard.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-inspection/Initialize-Qodana.ps1
    Installs Qodana CLI if missing and starts the Qodana container.
#>

[CmdletBinding()]
param(
    [string] $NetworkName = 'pineguard'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')

# --- Qodana CLI ---

if (-not (Test-CommandExists -Name 'qodana')) {
    Write-Host 'Qodana CLI not found. Installing via Winget...' -ForegroundColor Cyan

    if (-not (Test-CommandExists -Name 'winget')) {
        throw "Winget not found. Install App Installer from the Microsoft Store or upgrade to Windows 10/11."
    }

    winget install JetBrains.Qodana --silent --accept-package-agreements --accept-source-agreements
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Qodana CLI installation failed (exit code: $LASTEXITCODE)." -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host 'Qodana CLI installed.' -ForegroundColor Green
}
else {
    Write-Host 'Qodana CLI already installed.' -ForegroundColor DarkGray
}

# --- Container ---

& (Join-Path $PSScriptRoot '../docker/qodana-up.ps1') -NetworkName $NetworkName
exit $LASTEXITCODE
