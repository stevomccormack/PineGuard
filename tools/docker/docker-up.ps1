<#
.SYNOPSIS
    Start Qodana and SonarQube Docker Compose stacks.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Ensures the pineguard Docker network exists, then runs docker compose up
    against docker-compose.sonarqube.yml and docker-compose.qodana.yml.

    To run the analysis processes after bringing containers up:
        tools/sonar-scanner/Run-SonarScanner.ps1  (SonarQube analysis)
        tools/code-inspection/Run-Qodana.ps1      (Qodana inspection)

.PARAMETER NetworkName
    Docker network name. Default: pineguard.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-up.ps1
#>

[CmdletBinding()]
param(
    [string] $NetworkName = 'pineguard'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/docker.ps1')

if (-not (Test-CommandExists -Name 'docker')) {
    throw "Docker ('docker') was not found on PATH. Ensure Docker Desktop is running."
}

Ensure-DockerNetwork -NetworkName $NetworkName

$sonarFile  = Join-Path $PSScriptRoot 'docker-compose.sonarqube.yml'
$qodanaFile = Join-Path $PSScriptRoot 'docker-compose.qodana.yml'

Write-Host 'Starting SonarQube...' -ForegroundColor Cyan
docker compose -p $DockerProjectName -f $sonarFile up -d
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host 'Starting Qodana...' -ForegroundColor Cyan
docker compose -p $DockerProjectName -f $qodanaFile up -d
exit $LASTEXITCODE
