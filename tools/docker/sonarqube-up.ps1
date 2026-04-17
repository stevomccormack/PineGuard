<#
.SYNOPSIS
    Start the local SonarQube server using Docker Compose.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Starts the SonarQube Community Edition server via docker-compose.sonarqube.yml in detached
    mode, creates the Docker network if it does not exist, and waits for the server to become
    healthy before returning.

    Once UP, run the analysis with:
        pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Run-SonarScanner.ps1

    Default credentials (first run): admin / admin - SonarQube will prompt you to change them.

.PARAMETER NetworkName
    Docker network name. Default: pineguard.

.PARAMETER Port
    Host port SonarQube is mapped to. Default: 9001.

.PARAMETER HealthTimeoutSeconds
    Maximum seconds to wait for SonarQube to report UP. Default: 120.

.PARAMETER InstallScanner
    Install the dotnet-sonarscanner global tool if not already present.

.PARAMETER Open
    Open the SonarQube dashboard in the default browser once the server is healthy.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/sonarqube-up.ps1
    Starts the SonarQube server and waits for it to be healthy.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/docker/sonarqube-up.ps1 -InstallScanner -Open
    Starts the server, installs dotnet-sonarscanner if missing, and opens the dashboard.
#>

[CmdletBinding()]
param(
    [string] $NetworkName = 'pineguard',
    [ValidateRange(1, 65535)] [int] $Port = 9001,
    [ValidateRange(10, 600)] [int] $HealthTimeoutSeconds = 120,
    [switch] $InstallScanner,
    [switch] $Open
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/docker.ps1')
. (Join-Path $PSScriptRoot '../.shared/sonarqube.ps1')

# --- Validation ---

if (-not (Test-CommandExists -Name 'docker')) {
    throw "Docker ('docker') was not found on PATH. Ensure Docker Desktop is running."
}

$composeFile = Join-Path $PSScriptRoot 'docker-compose.sonarqube.yml'
if (-not (Test-Path -LiteralPath $composeFile)) {
    throw "Compose file not found: $composeFile"
}

# --- Network ---

Ensure-DockerNetwork -NetworkName $NetworkName

# --- Start server ---

$sonarUrl  = "http://localhost:$Port"
$healthUrl = "$sonarUrl/api/system/status"

Write-Host 'Starting SonarQube...' -ForegroundColor Cyan

docker compose -p $DockerProjectName -f $composeFile up -d
$exitCode = $LASTEXITCODE

if ($exitCode -ne 0) {
    Write-Host "docker compose up failed (exit code: $exitCode)." -ForegroundColor Red
    exit $exitCode
}

# --- Health check ---

$isHealthy = Wait-SonarQubeHealthy -HealthUrl $healthUrl -TimeoutSeconds $HealthTimeoutSeconds

if (-not $isHealthy) {
    Write-Host "SonarQube did not become healthy within ${HealthTimeoutSeconds}s." -ForegroundColor Red
    Write-Host 'Check logs: docker logs pineguard-sonarqube' -ForegroundColor DarkGray
    exit 1
}

Write-Host "SonarQube is UP: $sonarUrl" -ForegroundColor Green
Write-Host 'Default credentials (first run): admin / admin' -ForegroundColor DarkGray

# --- Install dotnet-sonarscanner ---

if ($InstallScanner) {
    Write-Host 'Checking dotnet-sonarscanner...' -ForegroundColor Cyan
    $toolList = dotnet tool list -g 2>&1
    if ($toolList -notmatch 'dotnet-sonarscanner') {
        Write-Host 'Installing dotnet-sonarscanner...' -ForegroundColor Cyan
        dotnet tool install --global dotnet-sonarscanner
        Write-Host 'dotnet-sonarscanner installed.' -ForegroundColor Green
    }
    else {
        Write-Host 'dotnet-sonarscanner already installed.' -ForegroundColor DarkGray
    }
}

# --- Open dashboard ---

if ($Open) {
    Write-Host "Opening: $sonarUrl" -ForegroundColor Cyan
    Start-Process $sonarUrl
}
