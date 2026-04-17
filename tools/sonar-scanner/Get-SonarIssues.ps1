<#
.SYNOPSIS
    Retrieve SonarQube issues for the PineGuard project, filtered by severity.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Queries the local SonarQube server API for issues and outputs structured JSON
    to stdout. Designed for AI agents to consume and fix issues programmatically.

    Requires a running SonarQube instance. Run Initialize-SonarQube.ps1 first.

.PARAMETER Severity
    Issue severity filter. Default: All.
    - All: All severities (omits filter)
    - Blocker: BLOCKER only
    - High: CRITICAL only
    - Medium: MAJOR only
    - Low: MINOR and INFO

.PARAMETER SonarUrl
    URL of the local SonarQube instance. Default: http://localhost:9001.

.PARAMETER ProjectKey
    SonarQube project key. Default: PineGuard.

.PARAMETER ProjectToken
    SonarQube project authentication token. Falls back to SONARQUBE_TOKEN environment variable.

.PARAMETER MaxIssues
    Maximum number of issues to retrieve. Default: 500.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Get-SonarIssues.ps1 -Severity Blocker
    Retrieves all Blocker-severity issues as JSON.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/sonar-scanner/Get-SonarIssues.ps1 -Severity All -MaxIssues 100
    Retrieves the first 100 issues of any severity.
#>

[CmdletBinding()]
param(
    [ValidateSet('All', 'Blocker', 'High', 'Medium', 'Low')]
    [string] $Severity = 'All',

    [string] $SonarUrl    = $null,
    [string] $ProjectKey  = $null,
    [string] $ProjectToken = '',
    [ValidateRange(1, 10000)] [int] $MaxIssues = 500
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '../.shared/commands.ps1')
. (Join-Path $PSScriptRoot '../.shared/env.ps1')
. (Join-Path $PSScriptRoot '../.shared/sonarqube.ps1')

# Apply defaults from shared constants.
if ([string]::IsNullOrWhiteSpace($SonarUrl))   { $SonarUrl   = $SonarQubeDefaultUrl }
if ([string]::IsNullOrWhiteSpace($ProjectKey)) { $ProjectKey = $SonarQubeDefaultProjectKey }

# Refresh environment variables (picks up SONARQUBE_TOKEN set in other terminals).
Sync-Env

# --- Token ---

$tokenValue = Resolve-SonarQubeToken -ProjectToken $ProjectToken
if ($null -eq $tokenValue) {
    Write-Host 'A SonarQube project token is required. Set SONARQUBE_TOKEN or pass -ProjectToken.' -ForegroundColor Red
    exit 1
}

# --- Verify SonarQube is UP ---

Write-Host "Verifying SonarQube is UP at $SonarUrl..." -ForegroundColor Cyan
if (-not (Test-SonarQubeUp -SonarUrl $SonarUrl)) {
    Write-Host "SonarQube at $SonarUrl is not UP. Run Initialize-SonarQube.ps1 first." -ForegroundColor Red
    exit 1
}

# --- Build query ---

$page     = 1
$pageSize = [Math]::Min($MaxIssues, 500)
$allIssues = @()

$baseParams = @{
    componentKeys = $ProjectKey
    ps            = $pageSize
    resolved      = 'false'
}

if ($Severity -ne 'All' -and $SonarQubeSeverityMap.ContainsKey($Severity)) {
    $baseParams['severities'] = $SonarQubeSeverityMap[$Severity]
}

$authHeader = New-BearerAuthHeader -Token $tokenValue

Write-Host "Fetching issues (Severity: $Severity, Max: $MaxIssues)..." -ForegroundColor Cyan

# --- Paginate ---

do {
    $baseParams['p'] = $page
    $queryString = ($baseParams.GetEnumerator() | ForEach-Object { "$($_.Key)=$([Uri]::EscapeDataString($_.Value))" }) -join '&'
    $uri = "$SonarUrl/api/issues/search?$queryString"

    try {
        $response = Invoke-RestMethod -Uri $uri -Method Get -Headers $authHeader -ErrorAction Stop
    }
    catch {
        Write-Host "API request failed: $_" -ForegroundColor Red
        exit 1
    }

    foreach ($issue in $response.issues) {
        # Strip the project key prefix from the component path (e.g. "PineGuard:src/..." -> "src/...")
        $filePath = $issue.component
        if ($filePath -match '^[^:]+:(.+)$') {
            $filePath = $Matches[1]
        }

        $allIssues += [PSCustomObject]@{
            file      = $filePath
            line      = if ($issue.PSObject.Properties['line']) { $issue.line } else { $null }
            rule      = $issue.rule
            severity  = $issue.severity
            message   = $issue.message
            component = $issue.component
        }
    }

    $totalFetched = $page * $pageSize
    $page++
} while ($response.issues.Count -eq $pageSize -and $totalFetched -lt $MaxIssues -and $totalFetched -lt $response.total)

# --- Output ---

Write-Host "Found $($allIssues.Count) issue(s)." -ForegroundColor Green

$allIssues | ConvertTo-Json -Depth 5
