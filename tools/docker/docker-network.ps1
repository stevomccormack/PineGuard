<#
.SYNOPSIS
    Manage the pineguard Docker network.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Creates the named Docker bridge network used by docker-compose.qodana.yml and
    docker-compose.sonarqube.yml. Run once before the first docker-up.ps1 or
    sonarqube-up.ps1 invocation (they also auto-create the network if missing).

.PARAMETER Name
    Network name. Default: pineguard.

.PARAMETER Driver
    Docker network driver. Default: bridge.

.PARAMETER Remove
    Remove the network instead of creating it.

.PARAMETER Info
    Inspect and display network details.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-network.ps1
    Creates the pineguard network if it does not already exist.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-network.ps1 -Info
    Inspects and displays the network configuration.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/docker-network.ps1 -Remove
    Removes the network.
#>

[CmdletBinding()]
param(
    [string] $Name = 'pineguard',
    [ValidateSet('bridge', 'host', 'overlay')] [string] $Driver = 'bridge',
    [switch] $Remove,
    [switch] $Info
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/docker.ps1')

if (-not (Test-CommandExists -Name 'docker')) {
    throw "Docker ('docker') was not found on PATH. Ensure Docker Desktop is running."
}

if ($Remove) {
    if (Test-DockerNetwork -NetworkName $Name) {
        Write-Host "Removing network: $Name" -ForegroundColor Cyan
        docker network rm $Name
        Write-Host "Network removed: $Name" -ForegroundColor Green
    }
    else {
        Write-Host "Network not found: $Name" -ForegroundColor Yellow
    }
    return
}

if ($Info) {
    if (Test-DockerNetwork -NetworkName $Name) {
        docker network inspect $Name
    }
    else {
        Write-Host "Network not found: $Name" -ForegroundColor Yellow
    }
    return
}

# Default: ensure the network exists.
Ensure-DockerNetwork -NetworkName $Name -Driver $Driver
