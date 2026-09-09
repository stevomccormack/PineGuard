<#
.SYNOPSIS
    Shared SonarQube helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import SonarQube constants and helper functions
    into the calling script's scope.
    Used by tools/code-scan/sonarqube/*.

    Self-contained: dot-sources secret.ps1 (Get-ToolSecret, which itself pulls in dotenv.ps1 and
    path.ps1) itself so Resolve-SonarQubeToken below works standalone, per D-2's Pester
    load-order test (every tools/.shared/*.ps1 file must dot-source standalone).
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'secret.ps1')

# -------------------------------------------------------------------------------------------------
# Constants
# -------------------------------------------------------------------------------------------------

$script:SonarQubeDefaultUrl         = 'http://localhost:9001'
$script:SonarQubeDefaultProjectKey  = 'PineGuard'
$script:SonarQubeDefaultProjectName = 'PineGuard'

$script:SonarQubeSeverityMap = @{
    'Blocker' = 'BLOCKER'
    'High'    = 'CRITICAL'
    'Medium'  = 'MAJOR'
    'Low'     = 'MINOR,INFO'
}

# -------------------------------------------------------------------------------------------------
# Functions
# -------------------------------------------------------------------------------------------------

function Resolve-SonarQubeToken {
    <#
    .SYNOPSIS
        Resolves a SonarQube token from an explicit parameter, then Get-ToolSecret.

    .DESCRIPTION
        Resolution order (D-4): -ProjectToken -> $env:SONARQUBE_TOKEN -> the
        SONARQUBE_TOKEN key in .etc/powershell/.env. Wires every SonarQube-domain caller
        (Run-SonarScanner.ps1, Get-SonarQubeIssues.ps1) to the D-4 secrets resolver in one
        place, per T3.05.
    #>
    param([string] $ProjectToken = '')

    if (-not [string]::IsNullOrWhiteSpace($ProjectToken)) {
        return $ProjectToken
    }
    return Get-ToolSecret -Name 'SONARQUBE_TOKEN'
}

function New-BasicAuthHeader {
    <#
    .SYNOPSIS
        Creates an HTTP Basic authentication header hashtable.
    #>
    param([string] $Username, [string] $Password)
    $pair    = "${Username}:${Password}"
    $bytes   = [Text.Encoding]::ASCII.GetBytes($pair)
    $encoded = [Convert]::ToBase64String($bytes)
    return @{ Authorization = "Basic $encoded" }
}

function New-BearerAuthHeader {
    <#
    .SYNOPSIS
        Creates an HTTP Bearer authentication header hashtable.
    #>
    param([string] $Token)
    return @{ Authorization = "Bearer $Token" }
}

function Test-SonarAuth {
    <#
    .SYNOPSIS
        Validates credentials against the SonarQube authentication API.
    #>
    param([string] $Url, [hashtable] $Headers)
    try {
        $response = Invoke-RestMethod -Uri "$Url/api/authentication/validate" -Method Get -Headers $Headers -ErrorAction Stop
        return $response.valid -eq $true
    }
    catch {
        return $false
    }
}

function Test-SonarQubeUp {
    <#
    .SYNOPSIS
        Returns $true if the SonarQube server at the given URL reports status UP.
    #>
    param([string] $SonarUrl)
    try {
        $status = Invoke-RestMethod -Uri "$SonarUrl/api/system/status" -Method Get -ErrorAction Stop
        return $status.status -eq 'UP'
    }
    catch {
        return $false
    }
}

function Import-SonarProperties {
    <#
    .SYNOPSIS
        Parses a Java-style .properties file and returns a hashtable.
    .DESCRIPTION
        Handles # comments, line continuations (\), and key=value pairs.
        Leading/trailing whitespace on continuation lines is trimmed.
    .PARAMETER Path
        Absolute or relative path to the .properties file.
    .OUTPUTS
        [hashtable] of property key → value.
    #>
    param([Parameter(Mandatory)][string] $Path)

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Properties file not found: $Path"
    }

    $properties = [ordered]@{}
    $pendingKey = $null
    $pendingValue = ''

    foreach ($rawLine in [System.IO.File]::ReadAllLines((Resolve-Path $Path).Path)) {
        $line = $rawLine.TrimStart()

        # Skip blank lines and comments.
        if ($line -eq '' -or $line.StartsWith('#')) {
            continue
        }

        if ($null -ne $pendingKey) {
            # Continuation of a previous line.
            $segment = $line.TrimEnd()
            if ($segment.EndsWith('\')) {
                $pendingValue += $segment.Substring(0, $segment.Length - 1).TrimEnd()
            }
            else {
                $pendingValue += $segment
                $properties[$pendingKey] = $pendingValue
                $pendingKey = $null
                $pendingValue = ''
            }
            continue
        }

        # New key=value pair.
        $eqIndex = $line.IndexOf('=')
        if ($eqIndex -lt 0) { continue }

        $key   = $line.Substring(0, $eqIndex).Trim()
        $value = $line.Substring($eqIndex + 1).TrimEnd()

        if ($value.EndsWith('\')) {
            $pendingKey = $key
            $pendingValue = $value.Substring(0, $value.Length - 1).TrimEnd()
        }
        else {
            $properties[$key] = $value
        }
    }

    # Flush any unterminated continuation.
    if ($null -ne $pendingKey) {
        $properties[$pendingKey] = $pendingValue
    }

    return $properties
}

function Wait-SonarQubeHealthy {
    <#
    .SYNOPSIS
        Polls the SonarQube health endpoint until the server reports UP or timeout.
    .OUTPUTS
        $true if the server became healthy, $false on timeout.
    #>
    param(
        [string] $HealthUrl,
        [int]    $TimeoutSeconds
    )
    $deadline = (Get-Date).AddSeconds($TimeoutSeconds)
    Write-Host "Waiting for SonarQube to become healthy (timeout: ${TimeoutSeconds}s)..." -ForegroundColor Cyan
    while ((Get-Date) -lt $deadline) {
        try {
            $response = Invoke-RestMethod -Uri $HealthUrl -Method Get -ErrorAction Stop
            if ($response.status -eq 'UP') {
                Write-Host ''
                return $true
            }
        }
        catch {
            # Server not yet ready - expected during startup.
        }
        Write-Host '.' -NoNewline
        Start-Sleep -Seconds 5
    }
    Write-Host ''
    return $false
}
