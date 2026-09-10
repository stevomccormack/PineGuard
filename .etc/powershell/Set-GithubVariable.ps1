<#
.SYNOPSIS
    Push GitHub Actions repository variables for this project.

.DESCRIPTION
    Variables are plain-text configuration the workflows read as ${{ vars.NAME }}. Unlike
    secrets, their values live in $VariableCatalog in this script rather than in .env, because
    they are configuration, not credentials - so the committed script IS the record of what the
    repository is configured to.

      - No arguments: pushes every variable in $VariableCatalog with its catalog value.
      - -Name: pushes one variable, using -Value if given, otherwise its catalog value.

.NOTES
    $VariableCatalog is the set .github/workflows/** actually reads, verified against the
    workflows:

      MIN_CODE_COVERAGE - the coverage gate in ci.yml. One variable gates BOTH line and branch
                          coverage; ci.yml falls back to 100 when it is unset, so the catalog
                          value matches that fallback rather than silently weakening the gate.
      QODANA_ENABLED    - opt-in switch for ci.yml's Qodana job.

    MIN_CODE_COVERAGE was missing from the previous version's default set, so a full push left
    the gate riding on ci.yml's fallback rather than on declared configuration.

.PARAMETER Name
    A single variable to set. Omit to push the whole catalog.

.PARAMETER Value
    Explicit value for the single-variable form. Omit to use the catalog value.

.PARAMETER WhatIf
    Report which variables would be set, and to what, without calling gh.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubVariable.ps1 -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubVariable.ps1
    Pushes MIN_CODE_COVERAGE=100 and QODANA_ENABLED=false.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GithubVariable.ps1 -Name QODANA_ENABLED -Value 'true'
    Turns the opt-in Qodana job on.
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

# Authoritative values - see .NOTES. Edit here, then run the script to reconcile the repository.
$VariableCatalog = [ordered]@{
    MIN_CODE_COVERAGE = '100'
    QODANA_ENABLED    = 'false'
}

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: GitHub Repository Variables"
Write-Var -Name 'Repository' -Value "$($Project.Owner)/$($Project.Repository)" -NoIcon
Write-Var -Name 'Target' -Value $(if ($Name) { $Name } else { ($VariableCatalog.Keys) -join ', ' }) -NoIcon
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

function Set-CatalogVariable {
    <#
    .SYNOPSIS
        Resolves one variable's value and pushes it. Returns $true on success.
    #>
    [CmdletBinding(SupportsShouldProcess)]
    param(
        [Parameter(Mandatory)] [string] $VariableName,
        [string] $ExplicitValue
    )

    $variableValue = $ExplicitValue
    if ([string]::IsNullOrWhiteSpace($variableValue) -and $VariableCatalog.Contains($VariableName)) {
        $variableValue = [string] $VariableCatalog[$VariableName]
    }

    if ([string]::IsNullOrWhiteSpace($variableValue)) {
        Write-FailMessage `
            -Title $VariableName `
            -Message "No value supplied and no catalog entry exists. Pass -Value, or add it to `$VariableCatalog."
        return $false
    }

    if (-not $PSCmdlet.ShouldProcess("$($Project.Owner)/$($Project.Repository)", "Set variable $VariableName = $variableValue")) {
        Write-Status "WhatIf: would set '$VariableName' to '$variableValue'."
        return $true
    }

    Set-GitHubRepositoryVariable `
        -Owner $Project.Owner `
        -Repository $Project.Repository `
        -Name $VariableName `
        -Value $variableValue

    Write-OkMessage -Title $VariableName -Message "Repository variable set to '$variableValue'."
    return $true
}

# -------------------------------------------------------------------------------------------------

if (-not [string]::IsNullOrWhiteSpace($Name)) {
    if (-not $VariableCatalog.Contains($Name)) {
        Write-Status "'$Name' is not in the catalog ($(($VariableCatalog.Keys) -join ', ')); setting it anyway."
    }

    if (-not (Set-CatalogVariable -VariableName $Name -ExplicitValue $Value)) {
        exit 1
    }
}
else {
    Write-Status 'Setting catalog repository variables...'
    Write-NewLine

    $failed = @()
    foreach ($variableName in $VariableCatalog.Keys) {
        if (-not (Set-CatalogVariable -VariableName $variableName)) {
            $failed += $variableName
        }
    }

    Write-NewLine

    if ($failed.Count -gt 0) {
        Write-FailMessage -Title 'GitHub Repository Variables' -Message ('Failed to set: {0}' -f ($failed -join ', '))
        exit 1
    }
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title 'GitHub Repository Variables' `
    -Message "Configured on $($Project.Owner)/$($Project.Repository). Review at $($Project.WebUrl)/settings/variables/actions"
