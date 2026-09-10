<#
.SYNOPSIS
    Commission the local Qodana environment and optionally run a scan.

.DESCRIPTION
    Checks the Qodana prerequisites, records the Qodana Cloud credentials this workstation needs,
    and - unless -NoScan is passed - runs a scan over the repository.

    This is the workstation counterpart to tools/code-scan/sonarqube/Initialize-SonarQube.ps1:
    it configures an already-installed tool rather than installing it. Install the CLI and bring
    the container up with tools/code-scan/qodana/Install-Qodana.ps1 first. For scoped scans with
    per-scope configs and result directories, use tools/code-scan/qodana/Run-Qodana.ps1.

.NOTES
    Two bugs made the previous version unrunnable past its prerequisite checks. It called
    Test-CommandExists, which belongs to tools/.shared/commands.ps1 and is not loaded here
    (the Onboarding library's equivalent is Test-Command), and Ensure-Directory, which does not
    exist in either library. Both threw before a scan could start.

    Credential handling changed to match the repository's secrets policy (D-4 / F-24 / F-25):
    -SetScope, which offered to persist QODANA_TOKEN into the Windows User registry, is replaced
    by -Persist, which writes it to the gitignored .etc/powershell/.env via
    tools/.shared/dotenv.ps1's Set-DotEnvVariable. Without -Persist the token is set for this
    process only. The token is never echoed.

.PARAMETER Token
    Qodana Cloud token. Omit to resolve QODANA_TOKEN from the environment, then from .env, then
    to prompt.

.PARAMETER Endpoint
    Qodana Cloud endpoint override. Sets QODANA_ENDPOINT for this process.

.PARAMETER Persist
    Write the resolved token to .etc/powershell/.env so later sessions pick it up.

.PARAMETER NoScan
    Verify prerequisites and credentials, then stop without scanning.

.PARAMETER ResultsDir
    Directory for scan results. Relative paths resolve against the repository root.
    Default: artifacts/qodana.

.PARAMETER Clean
    Delete the results directory before scanning.

.PARAMETER OpenReport
    Open the generated HTML report afterwards.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-Qodana.ps1 -NoScan
    Checks that qodana and docker are present and that a token is resolvable.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Initialize-Qodana.ps1 -Persist -OpenReport
#>

[CmdletBinding()]
param(
    [string] $Token,

    [string] $Endpoint,

    [switch] $Persist,

    [switch] $NoScan,

    [ValidateNotNullOrEmpty()]
    [string] $ResultsDir = 'artifacts/qodana',

    [switch] $Clean,

    [switch] $OpenReport
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')
. (Join-Path $PSScriptRoot '../../tools/.shared/secret.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

$dotEnvPath = Join-Path $PSScriptRoot '.env'
$resultsFullPath = if ([System.IO.Path]::IsPathRooted($ResultsDir)) { $ResultsDir } else { Join-Path $Solution.RepoRoot $ResultsDir }

Write-MastHead "$($Project.Name) Project: Qodana Local Scan"
Write-Var -Name 'Project Path' -Value $Solution.RepoRoot -NoIcon
Write-Var -Name 'Results' -Value $resultsFullPath -NoIcon
Write-Var -Name 'Persist token' -Value $Persist.IsPresent -NoIcon
Write-Var -Name 'Scan' -Value (-not $NoScan.IsPresent) -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------
# Prerequisites

if (-not (Test-Command -Name 'qodana')) {
    Write-FailMessage `
        -Title 'Qodana CLI' `
        -Message "'qodana' was not found on PATH. Install it with tools/code-scan/qodana/Install-Qodana.ps1"
    exit 1
}

if (-not (Test-Command -Name 'docker')) {
    Write-FailMessage `
        -Title 'Docker' `
        -Message "'docker' was not found on PATH. Qodana runs its linters in a container - install Docker Desktop and start the daemon."
    exit 1
}

Write-Status 'Tooling versions:'
foreach ($tool in @('qodana', 'docker')) {
    try {
        $version = (& $tool --version 2>&1 | Out-String).Trim()
        if (-not [string]::IsNullOrWhiteSpace($version)) {
            Write-Var -Name $tool -Value ($version -split "`n")[0] -NoIcon
        }
    }
    catch {
        # A version query failing is not fatal; the scan itself will report a broken install.
        Write-Var -Name $tool -Value 'version unavailable' -NoIcon
    }
}
Write-NewLine

# -------------------------------------------------------------------------------------------------
# Credentials

if ([string]::IsNullOrWhiteSpace($Token)) {
    $Token = Get-ToolSecret -Name 'QODANA_TOKEN' -EnvFilePath $dotEnvPath
}

if ([string]::IsNullOrWhiteSpace($Token) -and -not $NoScan) {
    Write-Status 'QODANA_TOKEN is not set. A token links the scan to Qodana Cloud; leave blank to scan locally only.'
    $secureToken = Read-Host 'Qodana Cloud token' -AsSecureString
    if ($secureToken.Length -gt 0) {
        $Token = Get-PlainTextFromSecureString -SecureString $secureToken
    }
}

if (-not [string]::IsNullOrWhiteSpace($Token)) {
    [Environment]::SetEnvironmentVariable('QODANA_TOKEN', $Token, 'Process')

    if ($Persist) {
        Set-DotEnvVariable -Path $dotEnvPath -Name 'QODANA_TOKEN' -Value $Token
        Write-OkMessage -Title 'QODANA_TOKEN' -Message "Written to $dotEnvPath (gitignored)."
    }
    else {
        Write-OkMessage -Title 'QODANA_TOKEN' -Message 'Set for this process. Pass -Persist to keep it.'
    }
}
else {
    Write-Status 'No token — results stay local and are not uploaded to Qodana Cloud.'
}

if (-not [string]::IsNullOrWhiteSpace($Endpoint)) {
    [Environment]::SetEnvironmentVariable('QODANA_ENDPOINT', $Endpoint, 'Process')
    Write-Var -Name 'QODANA_ENDPOINT' -Value $Endpoint -NoIcon
}

Write-NewLine

if ($NoScan) {
    Write-OkMessage -Title "$($Project.Name) Project: Qodana" -Message 'Prerequisites and credentials verified (-NoScan).'
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Scan

if ($Clean -and (Test-Path -LiteralPath $resultsFullPath)) {
    Write-Status "Clearing $resultsFullPath"
    Remove-Item -LiteralPath $resultsFullPath -Recurse -Force
}

if (-not (Test-EnsureDirectory -Path $resultsFullPath)) {
    Write-FailMessage -Title 'Results' -Message "Could not create $resultsFullPath"
    exit 1
}

Write-Status "Running Qodana scan (results: $resultsFullPath)"

Push-Location -LiteralPath $Solution.RepoRoot
try {
    qodana scan --results-dir $resultsFullPath
    $scanExitCode = $LASTEXITCODE
}
finally {
    Pop-Location
}

if ($scanExitCode -ne 0) {
    Write-FailMessage `
        -Title "$($Project.Name) Project: Qodana" `
        -Message "Scan failed (exit code $scanExitCode). Partial results may exist in $resultsFullPath"
    exit $scanExitCode
}

if ($OpenReport) {
    $reportPath = Join-Path $resultsFullPath 'report/index.html'
    if (Test-Path -LiteralPath $reportPath) {
        Write-Status "Opening $reportPath"
        Start-Process $reportPath
    }
    else {
        Write-Status "No HTML report at $reportPath"
    }
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage -Title "$($Project.Name) Project: Qodana" -Message "Scan complete. Results: $resultsFullPath"
