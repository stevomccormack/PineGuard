<#
.SYNOPSIS
    Auto-commission a fresh SonarQube instance (password, project, token).

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Automates the first-run setup of a local SonarQube Community Edition server:
      1. Changes the default admin:admin password.
      2. Creates the PineGuard project.
      3. Generates a user token.
      4. Persists the token as a User environment variable (SONARQUBE_TOKEN).

    Idempotent — safe to re-run. Detects existing configuration and skips
    completed steps.

    Prerequisites: SonarQube must be running. Start it with Initialize-SonarQube.ps1.

.PARAMETER SonarUrl
    Base URL of the local SonarQube instance. Default: http://localhost:9001.

.PARAMETER NewPassword
    Password to set for the admin account (replaces the default admin:admin).
    Default: Scanner-1234.

.PARAMETER ProjectKey
    SonarQube project key. Default: PineGuard.

.PARAMETER ProjectName
    SonarQube project display name. Default: PineGuard.

.PARAMETER TokenName
    Name of the user token to generate. Default: LocalDev.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Setup-SonarQube.ps1
    Commissions SonarQube with default settings.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Setup-SonarQube.ps1 -NewPassword "MyPassword123"
    Commissions SonarQube with a custom admin password.
#>

[CmdletBinding()]
param(
    [string] $SonarUrl     = $null,
    [string] $NewPassword  = 'Scanner-1234',
    [string] $ProjectKey   = $null,
    [string] $ProjectName  = $null,
    [string] $TokenName    = 'LocalDev'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference    = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/env.ps1')
. (Join-Path $PSScriptRoot '../.shared/sonarqube.ps1')

# Apply defaults from shared constants.
if ([string]::IsNullOrWhiteSpace($SonarUrl))     { $SonarUrl     = $SonarQubeDefaultUrl }
if ([string]::IsNullOrWhiteSpace($ProjectKey))   { $ProjectKey   = $SonarQubeDefaultProjectKey }
if ([string]::IsNullOrWhiteSpace($ProjectName)) { $ProjectName = $SonarQubeDefaultProjectName }

# --- Banner ---

Write-Host ''
Write-Host '=== SonarQube Setup ===' -ForegroundColor Cyan
Write-Host "Server        : $SonarUrl"
Write-Host "Project       : $ProjectName ($ProjectKey)"
Write-Host "Token Name    : $TokenName"
Write-Host ''

# --- 1. Sync environment ---

Sync-Env

# --- 2. Health check ---

Write-Host 'Checking server health...     ' -NoNewline
if (Test-SonarQubeUp -SonarUrl $SonarUrl) {
    Write-Host 'UP' -ForegroundColor Green
}
else {
    Write-Host 'FAILED' -ForegroundColor Red
    Write-Host "SonarQube at $SonarUrl is not UP. Run Initialize-SonarQube.ps1 first." -ForegroundColor Red
    exit 1
}

# --- 3. Determine auth state ---

Write-Host 'Checking admin credentials... ' -NoNewline

$newHeaders = New-BasicAuthHeader -Username 'admin' -Password $NewPassword
$defaultHeaders = New-BasicAuthHeader -Username 'admin' -Password 'admin'

$isFreshInstall = $false
$activeHeaders = $null

if (Test-SonarAuth -Url $SonarUrl -Headers $newHeaders) {
    Write-Host 'Already configured' -ForegroundColor DarkGray
    $activeHeaders = $newHeaders
}
elseif (Test-SonarAuth -Url $SonarUrl -Headers $defaultHeaders) {
    Write-Host 'Fresh install detected' -ForegroundColor Yellow
    $isFreshInstall = $true
    $activeHeaders = $defaultHeaders
}
else {
    Write-Host 'FAILED' -ForegroundColor Red
    Write-Host 'Cannot authenticate with admin account using default or expected password.' -ForegroundColor Red
    exit 1
}

# --- 4. Change password ---

if ($isFreshInstall) {
    Write-Host 'Changing admin password...    ' -NoNewline
    try {
        $body = @{
            login            = 'admin'
            previousPassword = 'admin'
            password         = $NewPassword
        }
        Invoke-RestMethod -Uri "$SonarUrl/api/users/change_password" -Method Post -Headers $defaultHeaders -Body $body -ErrorAction Stop
        $activeHeaders = New-BasicAuthHeader -Username 'admin' -Password $NewPassword
        Write-Host 'Done' -ForegroundColor Green
    }
    catch {
        Write-Host 'FAILED' -ForegroundColor Red
        Write-Host "Password change failed: $_" -ForegroundColor Red
        exit 1
    }
}

# --- 5. Create project ---

Write-Host 'Creating project...           ' -NoNewline
try {
    $body = @{
        name    = $ProjectName
        project = $ProjectKey
    }
    Invoke-RestMethod -Uri "$SonarUrl/api/projects/create" -Method Post -Headers $activeHeaders -Body $body -ErrorAction Stop
    Write-Host "Done ($ProjectKey)" -ForegroundColor Green
}
catch {
    $err = $_.Exception.Response
    if ($err -and $err.StatusCode.value__ -eq 400) {
        Write-Host "Already exists ($ProjectKey)" -ForegroundColor DarkGray
    }
    else {
        Write-Host 'FAILED' -ForegroundColor Red
        Write-Host "Project creation failed: $_" -ForegroundColor Red
        exit 1
    }
}

# --- 6. Generate token ---

Write-Host 'Checking existing token...    ' -NoNewline

$existingToken = $env:SONARQUBE_TOKEN
$tokenValue = $null

if (-not [string]::IsNullOrWhiteSpace($existingToken)) {
    # Validate the existing token
    $bearerHeaders = New-BearerAuthHeader -Token $existingToken
    try {
        $validation = Invoke-RestMethod -Uri "$SonarUrl/api/authentication/validate" -Method Get -Headers $bearerHeaders -ErrorAction Stop
        if ($validation.valid -eq $true) {
            Write-Host 'Valid (using existing)' -ForegroundColor DarkGray
            $tokenValue = $existingToken
        }
    }
    catch {
        # Token invalid, will regenerate
    }
}

if ($null -eq $tokenValue) {
    Write-Host 'None' -ForegroundColor Yellow
    Write-Host 'Generating token...           ' -NoNewline

    # Try to generate; if name exists, revoke first then retry
    $generated = $false
    for ($attempt = 1; $attempt -le 2; $attempt++) {
        try {
            $body = @{ name = $TokenName }
            $response = Invoke-RestMethod -Uri "$SonarUrl/api/user_tokens/generate" -Method Post -Headers $activeHeaders -Body $body -ErrorAction Stop
            $tokenValue = $response.token
            $generated = $true
            break
        }
        catch {
            if ($attempt -eq 1) {
                # Token name might already exist - revoke and retry
                try {
                    $revokeBody = @{ name = $TokenName }
                    Invoke-RestMethod -Uri "$SonarUrl/api/user_tokens/revoke" -Method Post -Headers $activeHeaders -Body $revokeBody -ErrorAction Stop
                }
                catch {
                    # Revoke failed - token name might not exist, the original error was something else
                }
            }
        }
    }

    if (-not $generated -or [string]::IsNullOrWhiteSpace($tokenValue)) {
        Write-Host 'FAILED' -ForegroundColor Red
        Write-Host 'Token generation failed.' -ForegroundColor Red
        exit 1
    }

    Write-Host 'Done' -ForegroundColor Green
}

# --- 7. Persist token ---

Write-Host 'Persisting SONARQUBE_TOKEN... ' -NoNewline

[Environment]::SetEnvironmentVariable('SONARQUBE_TOKEN', $tokenValue, 'User')
$env:SONARQUBE_TOKEN = $tokenValue

Write-Host 'Done (User environment variable)' -ForegroundColor Green

# --- 8. Summary ---

Write-Host ''
Write-Host 'Setup complete. You can now run:' -ForegroundColor Green
Write-Host '  pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Run-SonarScanner.ps1' -ForegroundColor White
Write-Host ''
