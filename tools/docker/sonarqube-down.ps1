<#
.SYNOPSIS
    Stop the local SonarQube Docker Compose stack.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Runs docker compose down against docker-compose.sonarqube.yml.
    Persistent data is retained in named volumes by default.
    Use -RemoveVolumes to also delete all SonarQube data, extensions, and logs.

.PARAMETER RemoveVolumes
    Also remove named volumes (data, extensions, logs). Resets SonarQube to a clean state.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/sonarqube-down.ps1
    Stops the SonarQube server. Data is preserved in named volumes.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/sonarqube-down.ps1 -RemoveVolumes
    Stops the server and deletes all persistent SonarQube data.
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

$composeFile = Join-Path $PSScriptRoot 'docker-compose.sonarqube.yml'
if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

Write-Host 'Stopping SonarQube stack...' -ForegroundColor Cyan

$downArgs = @('-p', $DockerProjectName, '-f', $composeFile, 'down')
if ($RemoveVolumes) {
    $downArgs += '-v'
    Write-Host 'Removing named volumes (data, extensions, logs).' -ForegroundColor DarkGray
}

docker compose @downArgs
$exitCode = $LASTEXITCODE

if ($exitCode -ne 0) {
    Write-Host "docker compose down failed (exit code: $exitCode)." -ForegroundColor Red
    exit $exitCode
}

Write-Host 'SonarQube stack stopped.' -ForegroundColor Green
