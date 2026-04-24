<#
.SYNOPSIS
    qodana init

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/qodana-init.ps1

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)]
    [string] $Token,

    [Parameter(Mandatory = $false)]
    [string] $Endpoint,

    [Parameter(Mandatory = $false)]
    [ValidateSet("Process", "User")]
    [string] $SetScope = "Process",

    [Parameter(Mandatory = $false)]
    [switch] $Force,

    [Parameter(Mandatory = $false)]
    [switch] $NoScan,

    [Parameter(Mandatory = $false)]
    [string] $ResultsDir = "artifacts/qodana",

    [Parameter(Mandatory = $false)]
    [switch] $OpenReport
)

try {
    . ".etc/powershell/.shared/index.ps1" *> $null
}
catch {
    throw
}

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# -------------------------------------------------------------------------------------------------

$project = $Project

Write-MastHead "$($project.Name) Project: Qodana Local Scan"
Write-Var -Name "Project Name" -Value $project.Name -NoIcon
Write-Var -Name "Project Path" -Value $project.LocalPath -NoIcon
Write-Var -Name "Results Dir" -Value $ResultsDir -NoIcon
Write-Var -Name "SetScope" -Value $SetScope -NoIcon
Write-NewLine

if (-not (Test-Command -Name "qodana")) {
    Write-FailMessage -Title "Qodana CLI" -Message "'qodana' was not found on PATH. See docs/qodana.md for install steps (Scoop)."
    exit 1
}

if (-not (Test-CommandExists -Name "docker")) {
    Write-FailMessage -Title "Docker" -Message "'docker' was not found on PATH. Qodana CLI typically requires Docker to run scans. Install Docker Desktop and ensure the daemon is running."
    exit 1
}

Write-Status "Tooling versions:"
try {
    $qodanaVersion = (qodana --version | Out-String).Trim()
    if (-not [string]::IsNullOrWhiteSpace($qodanaVersion)) {
        Write-Var -Name "qodana" -Value $qodanaVersion -NoIcon
    }
}
catch {
    # Ignore version query failures; scan will still run.
}

try {
    $dockerVersion = (docker --version | Out-String).Trim()
    if (-not [string]::IsNullOrWhiteSpace($dockerVersion)) {
        Write-Var -Name "docker" -Value $dockerVersion -NoIcon
    }
}
catch {
    # Ignore version query failures; scan will still run.
}

Write-NewLine

# -------------------------------------------------------------------------------------------------
# Configure environment variables (optional)

if (-not $NoScan -and [string]::IsNullOrWhiteSpace($Token) -and [string]::IsNullOrWhiteSpace($env:QODANA_TOKEN)) {
    Write-Status "QODANA_TOKEN is not set."
    $secureToken = Read-Host "Enter Qodana Cloud token (leave blank to skip)" -AsSecureString
    if ($secureToken.Length -gt 0) {
        $Token = Get-PlainTextFromSecureString -SecureString $secureToken
    }
}

if (-not [string]::IsNullOrWhiteSpace($Token)) {
    Write-Status "Setting QODANA_TOKEN ($SetScope scope)"
    Set-EnvironmentVariable -Name "QODANA_TOKEN" -Value $Token -Scope $SetScope -Force:$Force
}

if (-not [string]::IsNullOrWhiteSpace($Endpoint)) {
    Write-Status "Setting QODANA_ENDPOINT ($SetScope scope)"
    Set-EnvironmentVariable -Name "QODANA_ENDPOINT" -Value $Endpoint -Scope $SetScope -Force:$Force
}

Write-NewLine
Write-Var -Name "QODANA_TOKEN set" -Value (-not [string]::IsNullOrWhiteSpace($env:QODANA_TOKEN)) -NoIcon
Write-Var -Name "QODANA_ENDPOINT" -Value ($env:QODANA_ENDPOINT) -NoIcon
Write-NewLine

if ($NoScan) {
    Write-OkMessage -Title "$($project.Name) Project: Qodana" -Message "Prerequisites look good (-NoScan)."
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Run scan

Ensure-Directory -Path $ResultsDir

Write-Status "Running Qodana scan (results: $ResultsDir)"

Push-Location $project.LocalPath
try {
    qodana scan --results-dir $ResultsDir
    $exitCode = $LASTEXITCODE
}
finally {
    Pop-Location
}

if ($exitCode -ne 0) {
    Write-FailMessage -Title "$($project.Name) Project: Qodana" -Message "Qodana scan failed (exit code: $exitCode). Results may still exist in: $ResultsDir"
    exit $exitCode
}

if ($OpenReport) {
    $reportPath = Join-Path $project.LocalPath (Join-Path $ResultsDir "report/index.html")
    if (Test-Path -LiteralPath $reportPath) {
        Write-Status "Opening report: $reportPath"
        Start-Process $reportPath
    }
    else {
        Write-Status "No HTML report found at: $reportPath"
    }
}

Write-OkMessage -Title "$($project.Name) Project: Qodana" -Message "Scan completed. Results: $ResultsDir"
