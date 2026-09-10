<#
.SYNOPSIS
    Push GitHub Actions repository secrets for this project from the local environment.

.DESCRIPTION
    Secrets are credentials the workflows read as ${{ secrets.NAME }}. Values come from the
    process environment (populated from .etc/powershell/.env by .shared/index.ps1), or from
    -Value for a one-off push. No value is ever echoed.

      - No arguments: pushes every secret in $SecretCatalog, skipping and reporting any whose
        value is empty.
      - -Name: pushes one named secret, whether or not it is in the catalog.
      - -Value: an explicit value for the single-secret form, bypassing the environment.

.NOTES
    $SecretCatalog is the set the WORKFLOWS actually consume, verified against
    .github/workflows/**: NUGET_USER and QODANA_TOKEN.

    The previous default set was NUGET_TOKEN and QODANA_TOKEN, which was wrong in both
    directions. NUGET_TOKEN is a workstation-only push key - no workflow references it, because
    publish.yml authenticates to nuget.org over OIDC and mints its own short-lived key from
    secrets.NUGET_USER. And NUGET_USER, the secret publish.yml genuinely needs, was absent, which
    is why it had to be pushed by hand with an explicit -Name.

.PARAMETER Name
    A single secret to set. Omit to push the whole catalog.

.PARAMETER Value
    Explicit value for the single-secret form. Omit to read $Env:<Name>.

.PARAMETER WhatIf
    Report which secrets would be pushed, and whether each has a value, without calling gh.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubSecret.ps1 -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubSecret.ps1
    Pushes every catalog secret (NUGET_USER, QODANA_TOKEN) from the environment.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubSecret.ps1 -Name QODANA_TOKEN

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubSecret.ps1 -Name QODANA_TOKEN -Value 'eyJ...'
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [Parameter(Position = 0)]
    [string] $Name,

    [Parameter(Position = 1)]
    [string] $Value
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

# The secrets .github/workflows/** reads. Keep this list in step with the workflows, not with
# .env - .env also holds workstation-only credentials that must never leave this machine.
$SecretCatalog = @(
    'NUGET_USER',
    'QODANA_TOKEN'
)

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: GitHub Repository Secrets"
Write-Var -Name 'Repository' -Value "$($Project.Owner)/$($Project.Repository)" -NoIcon
Write-Var -Name 'Target' -Value $(if ($Name) { $Name } else { $SecretCatalog -join ', ' }) -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name 'gh')) {
    Write-FailMessage -Title 'GitHub CLI' -Message "'gh' was not found on PATH. Install via: winget install GitHub.cli"
    exit 1
}

$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'gh CLI' -Message "Not authenticated. Run 'gh auth login' first.`n$authStatus"
    exit 1
}

# -------------------------------------------------------------------------------------------------

function Set-CatalogSecret {
    <#
    .SYNOPSIS
        Resolves one secret's value and pushes it. Returns $true on success.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)] [string] $SecretName,
        [string] $ExplicitValue
    )

    $secretValue = $ExplicitValue
    if ([string]::IsNullOrWhiteSpace($secretValue)) {
        $secretValue = [Environment]::GetEnvironmentVariable($SecretName, 'Process')
    }

    if ([string]::IsNullOrWhiteSpace($secretValue)) {
        Write-FailMessage `
            -Title $SecretName `
            -Message "No value supplied and `$Env:$SecretName is empty. Add it to .etc/powershell/.env or pass -Value."
        return $false
    }

    if (-not $PSCmdlet.ShouldProcess("$($Project.Owner)/$($Project.Repository)", "Set secret $SecretName")) {
        Write-Status "WhatIf: would set '$SecretName' (value resolved, $($secretValue.Length) chars)."
        return $true
    }

    Set-GitHubRepositorySecret `
        -Owner $Project.Owner `
        -Repository $Project.Repository `
        -Name $SecretName `
        -Value $secretValue

    Write-OkMessage -Title $SecretName -Message 'Repository secret set.'
    return $true
}

# -------------------------------------------------------------------------------------------------

if (-not [string]::IsNullOrWhiteSpace($Name)) {
    if ($SecretCatalog -notcontains $Name) {
        Write-Status "'$Name' is not in the catalog ($($SecretCatalog -join ', ')); setting it anyway."
    }

    if (-not (Set-CatalogSecret -SecretName $Name -ExplicitValue $Value)) {
        exit 1
    }
}
else {
    Write-Status 'Setting catalog repository secrets...'
    Write-NewLine

    $failed = @()
    foreach ($secretName in $SecretCatalog) {
        if (-not (Set-CatalogSecret -SecretName $secretName)) {
            $failed += $secretName
        }
    }

    Write-NewLine

    if ($failed.Count -gt 0) {
        Write-FailMessage -Title 'GitHub Repository Secrets' -Message ('Failed to set: {0}' -f ($failed -join ', '))
        exit 1
    }
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title 'GitHub Repository Secrets' `
    -Message "Configured on $($Project.Owner)/$($Project.Repository). Review at $($Project.WebUrl)/settings/secrets/actions"
