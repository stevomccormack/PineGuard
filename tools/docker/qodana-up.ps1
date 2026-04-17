<#
.SYNOPSIS
    Start the Qodana Docker Compose stack.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Ensures the Docker network exists, then runs docker compose up against
    docker-compose.qodana.yml.

    To run a Qodana inspection:
        tools/code-inspection/Run-Qodana.ps1

.PARAMETER NetworkName
    Docker network name. Default: pineguard.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/qodana-up.ps1
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

$composeFile = Join-Path $PSScriptRoot 'docker-compose.qodana.yml'
if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

Ensure-DockerNetwork -NetworkName $NetworkName

Write-Host 'Starting Qodana...' -ForegroundColor Cyan
docker compose -p $DockerProjectName -f $composeFile up -d
exit $LASTEXITCODE
