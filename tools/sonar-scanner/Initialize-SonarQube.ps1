<#
.SYNOPSIS
    Initialize the local SonarQube environment.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Installs Java (OpenJDK 21) and dotnet-sonarscanner if not already present,
    then starts the SonarQube Community Edition server via Docker Compose.

    For CI/CD, use the SonarQube GitHub Action with SONARQUBE_TOKEN stored as a
    repository secret - do not use this script in pipelines.

    Prerequisites: Docker Desktop running, Winget available (Windows 10/11).

.PARAMETER Port
    Host port SonarQube is mapped to. Default: 9001.

.PARAMETER HealthTimeoutSeconds
    Maximum seconds to wait for SonarQube to report UP. Default: 120.

.PARAMETER Open
    Open the SonarQube dashboard in the default browser once the server is healthy.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Initialize-SonarQube.ps1
    Installs prerequisites and starts SonarQube.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Initialize-SonarQube.ps1 -Open
    Installs prerequisites, starts SonarQube, and opens the dashboard.
#>

[CmdletBinding()]
param(
    [ValidateRange(1, 65535)] [int] $Port = 9001,
    [ValidateRange(10, 600)]  [int] $HealthTimeoutSeconds = 120,
    [switch] $Open
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/env.ps1')

# --- Java ---

# Refresh all environment variables from the registry (picks up installs and
# env vars like SONARQUBE_TOKEN set in other terminals without a restart).
Sync-Env

if (-not (Test-CommandExists -Name 'java')) {
    Write-Host 'Java not found. Installing OpenJDK 21 via Winget...' -ForegroundColor Cyan

    if (-not (Test-CommandExists -Name 'winget')) {
        throw "Winget not found. Install App Installer from the Microsoft Store or upgrade to Windows 10/11."
    }

    winget install Microsoft.OpenJDK.21 --silent --accept-package-agreements --accept-source-agreements
    if ($LASTEXITCODE -ne 0) {
        Write-Host "OpenJDK 21 installation failed (exit code: $LASTEXITCODE)." -ForegroundColor Red
        exit $LASTEXITCODE
    }

    # Refresh environment again after install.
    Sync-Env

    if (-not (Test-CommandExists -Name 'java')) {
        throw 'OpenJDK 21 installed but java is still not on PATH. Restart your terminal and try again.'
    }

    Write-Host "OpenJDK 21 installed: $(Get-JavaVersion)" -ForegroundColor Green
}
else {
    Write-Host "Java found: $(Get-JavaVersion)" -ForegroundColor DarkGray
}

# --- SonarQube server + dotnet-sonarscanner ---

$upArgs = @{
    Port                 = $Port
    HealthTimeoutSeconds = $HealthTimeoutSeconds
    InstallScanner       = $true
}
if ($Open) { $upArgs['Open'] = $true }

& (Join-Path $PSScriptRoot '../docker/sonarqube-up.ps1') @upArgs
exit $LASTEXITCODE
