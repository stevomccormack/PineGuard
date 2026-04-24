<#
.SYNOPSIS
    github variables

.DESCRIPTION
    Sets GitHub Actions repository variables for the current project.

    Unlike secrets, variables are plain-text configuration read by workflows
    via ${{ vars.NAME }}. Values live in $DefaultVariables inside this script,
    not in .env, because they are configuration rather than credentials.

    - No arguments: pushes every variable in $DefaultVariables, using the
      hard-coded value for each.
    - -Name: pushes a single named variable. Uses the -Value parameter if
      supplied, otherwise falls back to the default in $DefaultVariables.

    Requires the gh CLI to be installed and authenticated.

.PARAMETER Name
    Single variable to set. If omitted, pushes the default set.

.PARAMETER Value
    Explicit value for the single-variable form. If omitted, read from
    $DefaultVariables[$Name].

.EXAMPLE
    ./github-variables.ps1
    Pushes all default variables (QODANA_ENABLED=false).

.EXAMPLE
    ./github-variables.ps1 -Name QODANA_ENABLED
    Pushes QODANA_ENABLED with its default value from $DefaultVariables.

.EXAMPLE
    ./github-variables.ps1 -Name QODANA_ENABLED -Value 'true'
    Toggles QODANA_ENABLED on.
#>

# .etc/powershell/github-variables.ps1

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

# Default set of variables to push when no -Name is supplied. Values are
# authoritative here — variables are configuration, not secrets, so they live
# in the script rather than .env.
$DefaultVariables = [ordered]@{
    QODANA_ENABLED = 'false'
}

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($project.Name) Project: GitHub Repository Variables"
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

function Set-VariableByName {
    param(
        [Parameter(Mandatory)][string] $VariableName,
        [string] $ExplicitValue
    )

    $variableValue = $ExplicitValue
    if ([string]::IsNullOrWhiteSpace($variableValue)) {
        if ($DefaultVariables.Contains($VariableName)) {
            $variableValue = [string] $DefaultVariables[$VariableName]
        }
    }

    if ([string]::IsNullOrWhiteSpace($variableValue)) {
        Write-FailMessage -Title $VariableName -Message "No value supplied and no default exists in `$DefaultVariables. Pass -Value explicitly or add a default."
        return $false
    }

    Set-GitHubRepositoryVariable `
        -Owner $project.Owner `
        -Repository $project.Repository `
        -Name $VariableName `
        -Value $variableValue

    Write-OkMessage -Title $VariableName -Message "Repository variable set to '$variableValue'."
    return $true
}

# -------------------------------------------------------------------------------------------------

# Single-variable mode vs default-set mode.
if (-not [string]::IsNullOrWhiteSpace($Name)) {
    Write-Status "Setting repository variable: $Name"
    Write-NewLine

    if (-not (Set-VariableByName -VariableName $Name -ExplicitValue $Value)) {
        exit 1
    }
}
else {
    Write-Status "Setting default repository variables..."
    Write-NewLine

    $failed = @()
    foreach ($variableName in $DefaultVariables.Keys) {
        if (-not (Set-VariableByName -VariableName $variableName)) {
            $failed += $variableName
        }
        Write-NewLine
    }

    if ($failed.Count -gt 0) {
        Write-FailMessage -Title "GitHub Repository Variables" -Message ("Failed to set: {0}" -f ($failed -join ', '))
        exit 1
    }
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title "GitHub Repository Variables" `
    -Message "Configured on $($project.Owner)/$($project.Repository). View at $($project.WebUrl)/settings/variables/actions"
