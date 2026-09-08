<#
.SYNOPSIS
    Run SonarQube analysis against the PineGuard solution.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Runs the full SonarQube analysis pipeline:
      1. Verifies the local SonarQube server is UP (start it with Initialize-SonarQube.ps1).
      2. Begins the SonarScanner session.
      3. Builds the solution.
      4. Collects code coverage via the existing coverage script.
      5. Submits findings to the local SonarQube instance.

    Requires dotnet-sonarscanner, restored from the repo-root local tool manifest
    (.config/dotnet-tools.json). This script runs `dotnet tool restore` itself before invoking
    the scanner; run it manually first only if you want to pre-warm the tool cache.

.PARAMETER ProjectToken
    SonarQube project authentication token. Falls back to SONARQUBE_TOKEN environment variable.

.PARAMETER RepoRoot
    Absolute path to the repository root. Auto-resolved from script location if not specified.

.PARAMETER SonarUrl
    URL of the local SonarQube instance. Default: http://localhost:9001.

.PARAMETER ProjectKey
    SonarQube project key. Default: PineGuard.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Run-SonarScanner.ps1 -ProjectToken sqp_xxx
    Runs a full SonarQube analysis.
#>

[CmdletBinding()]
param(
    [string] $ProjectToken = '',
    [string] $RepoRoot     = '',
    [string] $SonarUrl     = $null,
    [string] $ProjectKey   = $null
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/path.ps1')
. (Join-Path $PSScriptRoot '../.shared/sonarqube.ps1')

# Apply defaults from shared constants.
if ([string]::IsNullOrWhiteSpace($SonarUrl))   { $SonarUrl   = $SonarQubeDefaultUrl }
if ([string]::IsNullOrWhiteSpace($ProjectKey)) { $ProjectKey = $SonarQubeDefaultProjectKey }

# --- Token ---
#
# No registry refresh here (the removed Sync-Env used to merge Machine+User registry values into
# the session before this point): Sync-Env clobbered a session-scoped SONARQUBE_TOKEN with a
# stale persisted one (F-25). $env:SONARQUBE_TOKEN in the current session is authoritative.

$tokenValue = Resolve-SonarQubeToken -ProjectToken $ProjectToken
if ($null -eq $tokenValue) {
    $tokenValue = Read-Host 'Enter your SonarQube project token'
}
if ([string]::IsNullOrWhiteSpace($tokenValue)) {
    Write-Host 'A SonarQube project token is required.' -ForegroundColor Red
    exit 1
}

# --- Resolve paths ---

$repoRootResolved = if (-not [string]::IsNullOrWhiteSpace($RepoRoot)) {
    (Resolve-Path -LiteralPath $RepoRoot).Path
}
else {
    Get-RepoRoot -StartDirectory $PSScriptRoot
}
$slnPath          = Join-Path $repoRootResolved 'PineGuard.slnx'
$coveragePath     = 'artifacts/code-coverage/coverlet/all/testresults/**/coverage.opencover.xml'
$coverageScript   = Join-Path $repoRootResolved 'tools/code-coverage/coverlet/New-CoverageReport.ps1'

# --- Version (from latest git tag, or fallback) ---

$projectVersion = '1.0.0'
try {
    $gitTag = & git -C $repoRootResolved describe --tags --abbrev=0 2>$null
    if (-not [string]::IsNullOrWhiteSpace($gitTag)) {
        $projectVersion = $gitTag -replace '^v', ''
    }
}
catch { }

# --- Validate prerequisites ---

if (-not (Test-CommandExists -Name 'java')) {
    Write-Host 'Java not found. Run Initialize-SonarQube.ps1 to install OpenJDK 21.' -ForegroundColor Red
    Write-Host 'Or install manually: winget install Microsoft.OpenJDK.21' -ForegroundColor DarkGray
    exit 1
}

# --- Restore local dotnet tools (dotnet-sonarscanner) ---
#
# dotnet-sonarscanner comes from the repo-root local tool manifest, not a global install.
# `dotnet tool restore` must run from the repo root (or a subdirectory of it) so it finds
# .config/dotnet-tools.json; it is fast and idempotent, so it is safe to call unconditionally
# here rather than trying to detect whether the tool is already available.
Write-Host 'Restoring local dotnet tools...' -ForegroundColor Cyan
Push-Location $repoRootResolved
try {
    dotnet tool restore | Out-Host
    if ($LASTEXITCODE -ne 0) {
        Write-Host "dotnet tool restore failed with exit code $LASTEXITCODE." -ForegroundColor Red
        exit $LASTEXITCODE
    }
}
finally {
    Pop-Location
}

# --- Verify SonarQube is UP ---

Write-Host "Verifying SonarQube is UP at $SonarUrl..." -ForegroundColor Cyan
if (-not (Test-SonarQubeUp -SonarUrl $SonarUrl)) {
    Write-Host "SonarQube at $SonarUrl is not UP. Run Initialize-SonarQube.ps1 first." -ForegroundColor Red
    exit 1
}

# --- Load sonar-project.properties ---

$propsFile = Join-Path $PSScriptRoot 'sonar-project.properties'
Write-Host "Loading SonarQube properties from $propsFile..." -ForegroundColor Cyan
$sonarProps = Import-SonarProperties -Path $propsFile

# --- Run pipeline ---

Write-Host 'Starting SonarQube analysis pipeline...' -ForegroundColor Cyan

# Build /d: arguments from the properties file. Scanner-managed keys (host.url,
# token, coverage paths, encoding) are set explicitly; everything else comes
# from the properties file.
$propertyArgs = @()
foreach ($key in $sonarProps.Keys) {
    $propertyArgs += "/d:$key=$($sonarProps[$key])"
}

Push-Location $repoRootResolved

# The properties file lives under tools/sonar-scanner/ (not the repo root) because
# dotnet-sonarscanner refuses to run if it finds a sonar-project.properties file in its
# working directory. The scanner runs from $repoRootResolved, where no such file exists,
# so no hide/restore step is needed.

try {
    # Step 1: Begin
    Write-Host 'Step 1: Begin SonarScanner...' -ForegroundColor Cyan
    $beginArgs = @(
        'sonarscanner', 'begin'
        "/k:$ProjectKey"
        "/n:$SonarQubeDefaultProjectName"
        "/v:$projectVersion"
        "/d:sonar.host.url=$SonarUrl"
        "/d:sonar.token=$tokenValue"
        "/d:sonar.cs.opencover.reportsPaths=$coveragePath"
        "/d:sonar.sourceEncoding=utf-8"
    ) + $propertyArgs
    & dotnet @beginArgs

    # Step 2: Build
    Write-Host 'Step 2: Building solution...' -ForegroundColor Cyan
    dotnet build $slnPath --no-incremental

    # Step 3: Coverage (OpenCover format for SonarQube, all scopes, skip HTML)
    Write-Host 'Step 3: Collecting code coverage...' -ForegroundColor Cyan
    if (Test-Path -LiteralPath $coverageScript) {
        & $coverageScript -Scope All -Format opencover -SkipHtml -NoOpen -Clean -Framework net8.0
    }
    else {
        Write-Host "Coverage script not found at $coverageScript - skipping." -ForegroundColor Yellow
    }

    # Step 4: End
    Write-Host 'Step 4: Submitting to SonarQube...' -ForegroundColor Cyan
    dotnet sonarscanner end /d:sonar.token="$tokenValue"
}
finally {
    Pop-Location
}

Write-Host "Analysis complete. Results: $SonarUrl/dashboard?id=$ProjectKey" -ForegroundColor Green
