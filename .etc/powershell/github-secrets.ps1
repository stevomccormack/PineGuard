<#
.SYNOPSIS
    github secrets

.DESCRIPTION
    Sets GitHub Actions repository secrets for the current project from local
    environment variables (typically loaded from .etc/powershell/.env).

    - No arguments: pushes every secret in the default set (see $DefaultSecrets),
      skipping any whose env var is empty and warning on each skip.
    - -Name: pushes a single named secret.
    - -Value: optional explicit value for the single-secret form (bypasses env).

    Requires the gh CLI to be installed and authenticated.

.PARAMETER Name
    Single secret to set. If omitted, pushes the default set.

.PARAMETER Value
    Explicit value for the single-secret form. If omitted, read from $Env:$Name.

.EXAMPLE
    ./github-secrets.ps1
    Pushes all default secrets (NUGET_TOKEN, QODANA_TOKEN) from the environment.

.EXAMPLE
    ./github-secrets.ps1 -Name NUGET_TOKEN
    Pushes just NUGET_TOKEN from the environment.

.EXAMPLE
    ./github-secrets.ps1 -Name NUGET_TOKEN -Value 'oy2...'
    Pushes an explicit value (bypasses .env lookup).
#>

# .etc/powershell/github-secrets.ps1

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [string] $Name,

    [Parameter(Position = 1)]
    [string] $Value
)

# -------------------------------------------------------------------------------------------------

try {
    . ".etc/powershell/.shared/index.ps1" *> $null
}
catch {
    throw
}

# -------------------------------------------------------------------------------------------------

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# -------------------------------------------------------------------------------------------------

$project = $Project

# Default set of secrets to push when no -Name is supplied. Order is cosmetic.
$DefaultSecrets = @(
    'NUGET_TOKEN',
    'QODANA_TOKEN'
)

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($project.Name) Project: GitHub Repository Secrets"
Write-Var -Name "Project Name" -Value $project.Name -NoIcon
Write-Var -Name "Repository" -Value "$($project.Owner)/$($project.Repository)" -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name "gh")) {
    Write-FailMessage -Title "GitHub CLI" -Message "'gh' was not found on PATH. Install via: winget install GitHub.cli"
    exit 1
}

$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title "gh CLI" -Message "gh CLI is not authenticated. Run 'gh auth login' first.`n$authStatus"
    exit 1
}

# -------------------------------------------------------------------------------------------------

function Set-SecretByName {
    param(
        [Parameter(Mandatory)][string] $SecretName,
        [string] $ExplicitValue
    )

    $secretValue = $ExplicitValue
    if ([string]::IsNullOrWhiteSpace($secretValue)) {
        $secretValue = [System.Environment]::GetEnvironmentVariable($SecretName)
    }

    if ([string]::IsNullOrWhiteSpace($secretValue)) {
        Write-FailMessage -Title $SecretName -Message "No value supplied and `$Env:$SecretName is empty. Add it to .etc/powershell/.env or pass -Value explicitly."
        return $false
    }

    Set-GitHubRepositorySecret `
        -Owner $project.Owner `
        -Repository $project.Repository `
        -Name $SecretName `
        -Value $secretValue

    Write-OkMessage -Title $SecretName -Message "Repository secret set."
    return $true
}

# -------------------------------------------------------------------------------------------------

# Single-secret mode vs default-set mode.
if (-not [string]::IsNullOrWhiteSpace($Name)) {
    Write-Status "Setting repository secret: $Name"
    Write-NewLine

    if (-not (Set-SecretByName -SecretName $Name -ExplicitValue $Value)) {
        exit 1
    }
}
else {
    Write-Status "Setting default repository secrets..."
    Write-NewLine

    $failed = @()
    foreach ($secretName in $DefaultSecrets) {
        if (-not (Set-SecretByName -SecretName $secretName)) {
            $failed += $secretName
        }
        Write-NewLine
    }

    if ($failed.Count -gt 0) {
        Write-FailMessage -Title "GitHub Repository Secrets" -Message ("Failed to set: {0}" -f ($failed -join ', '))
        exit 1
    }
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title "GitHub Repository Secrets" `
    -Message "Configured on $($project.Owner)/$($project.Repository). View at $($project.WebUrl)/settings/secrets/actions"
