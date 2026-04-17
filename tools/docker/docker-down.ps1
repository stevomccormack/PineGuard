<#
.SYNOPSIS
    Stop Qodana and SonarQube Docker Compose stacks.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Runs docker compose down against docker-compose.qodana.yml and docker-compose.sonarqube.yml.

.PARAMETER RemoveVolumes
    Also remove named volumes from both stacks.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-down.ps1

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-down.ps1 -RemoveVolumes
#>

[CmdletBinding()]
param(
    [switch] $RemoveVolumes
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/docker.ps1')

if (-not (Test-CommandExists -Name 'docker')) {
    throw "Docker ('docker') was not found on PATH. Ensure Docker Desktop is running."
}

$sonarFile  = Join-Path $PSScriptRoot 'docker-compose.sonarqube.yml'
$qodanaFile = Join-Path $PSScriptRoot 'docker-compose.qodana.yml'

$downArgs = @('down')
if ($RemoveVolumes) {
    $downArgs += '-v'
    Write-Host 'Removing named volumes.' -ForegroundColor DarkGray
}

Write-Host 'Stopping Qodana...' -ForegroundColor Cyan
docker compose -p $DockerProjectName -f $qodanaFile @downArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host 'Stopping SonarQube...' -ForegroundColor Cyan
docker compose -p $DockerProjectName -f $sonarFile @downArgs
exit $LASTEXITCODE
