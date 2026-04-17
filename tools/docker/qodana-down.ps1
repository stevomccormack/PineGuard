<#
.SYNOPSIS
    Stop and remove the Qodana Docker Compose stack.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Runs docker compose down against docker-compose.qodana.yml.
    Use -RemoveVolumes to also delete the pineguard-qodana-cache named volume.

.PARAMETER RemoveVolumes
    Also remove named volumes (pineguard-qodana-cache). Clears the Qodana analysis cache.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/qodana-down.ps1
    Stops and removes the Qodana container.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/qodana-down.ps1 -RemoveVolumes
    Stops the container and removes the Qodana cache volume.
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

$composeFile = Join-Path $PSScriptRoot 'docker-compose.qodana.yml'
if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

Write-Host 'Stopping Qodana stack...' -ForegroundColor Cyan

$downArgs = @('-p', $DockerProjectName, '-f', $composeFile, 'down')
if ($RemoveVolumes) {
    $downArgs += '-v'
    Write-Host 'Removing named volumes.' -ForegroundColor DarkGray
}

docker compose @downArgs
$exitCode = $LASTEXITCODE

if ($exitCode -ne 0) {
    Write-Host "docker compose down failed (exit code: $exitCode)." -ForegroundColor Red
    exit $exitCode
}

Write-Host 'Qodana stack stopped.' -ForegroundColor Green
