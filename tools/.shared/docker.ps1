<#
.SYNOPSIS
    Shared Docker helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import the Docker project name constant and helper
    functions into the calling script's scope.
    Used by tools/docker/* and tools/sonar-scanner/*.

    All Docker Compose stacks run under a single project name ($DockerProjectName)
    so that `docker compose ls` shows one unified PineGuard project.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Canonical Docker Compose project name — used by all stacks.
$script:DockerProjectName = 'pineguard'

function Test-DockerNetwork {
    <#
    .SYNOPSIS
        Returns $true if a Docker network with the given name exists.
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string] $NetworkName
    )
    $result = docker network ls --filter "name=^${NetworkName}$" --format '{{.Name}}' 2>&1
    return ($result -contains $NetworkName)
}

function Ensure-DockerNetwork {
    <#
    .SYNOPSIS
        Creates a Docker network if it does not already exist.
    #>
    param(
        [Parameter(Mandatory = $true)]
        [string] $NetworkName,
        [string] $Driver = 'bridge'
    )
    if (-not (Test-DockerNetwork -NetworkName $NetworkName)) {
        Write-Host "Network '$NetworkName' not found - creating..." -ForegroundColor Cyan
        docker network create --driver $Driver $NetworkName | Out-Null
        Write-Host "Network created: $NetworkName" -ForegroundColor Green
    }
}
