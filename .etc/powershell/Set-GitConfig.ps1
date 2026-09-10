<#
.SYNOPSIS
    Apply this repository's local git configuration - identity, pull strategy and SSH rewrite.

.DESCRIPTION
    Writes --local git config only, so nothing here touches the machine's global git settings.
    Values come from $Project (see .shared/project.ps1), which is where the maintainer identity
    and branch conventions are declared once.

    Every setting is applied with `git -C <repo>`, so the script produces the same result from any
    working directory. The previous version relied on the shell already sitting in the repository
    root and silently configured whatever repo it happened to be run from.

    Each `git config` call is checked. The previous version issued six writes in a row without
    testing a single exit code, then printed a success banner regardless.

.PARAMETER WhatIf
    Print the settings that would be written without writing any of them.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GitConfig.ps1

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Set-GitConfig.ps1 -WhatIf
#>

[CmdletBinding(SupportsShouldProcess)]
param()

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: Local Git Configuration"
Write-Var -Name 'Project Name' -Value $Project.Name -NoIcon
Write-Var -Name 'Project Path' -Value $Project.LocalPath -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name 'git')) {
    Write-FailMessage -Title 'Git' -Message "'git' was not found on PATH."
    exit 1
}

if (-not (Test-GitRepositoryIsInitialized -Path $Project.LocalPath)) {
    Write-FailMessage -Title 'Git Config' -Message "Not a git repository: $($Project.LocalPath). Run Initialize-GitRepository.ps1 first."
    exit 1
}

# -------------------------------------------------------------------------------------------------

# git parses booleans case-insensitively, but writing 'true'/'false' keeps `git config --list`
# readable and matches what every other tool in the repo emits.
function ConvertTo-GitBoolean {
    param([Parameter(Mandatory)] [bool] $Value)
    if ($Value) { return 'true' } else { return 'false' }
}

$settings = [ordered]@{
    'user.name'          = $Project.UserName
    'user.email'         = $Project.UserEmail
    'init.defaultBranch' = $Project.MainBranch
    'pull.ff'            = (ConvertTo-GitBoolean -Value $Project.FastForward)
    'pull.rebase'        = (ConvertTo-GitBoolean -Value $Project.UseRebase)
    # Push and fetch over SSH even where a remote is recorded as HTTPS, so pushes use the
    # maintainer's key rather than prompting for a credential helper.
    'url.git@github.com:.insteadOf' = 'https://github.com/'
}

Write-Status 'Configuring local git settings:'
Write-NewLine

foreach ($key in $settings.Keys) {
    $value = $settings[$key]

    if (-not $PSCmdlet.ShouldProcess("$key = $value", 'git config --local')) {
        Write-Var -Name $key -Value "$value (WhatIf)" -NoIcon
        continue
    }

    $output = git -C $Project.LocalPath config --local $key $value 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-FailMessage -Title 'git config' -Message "Failed to set '$key': $output"
        exit 1
    }

    Write-Var -Name $key -Value (git -C $Project.LocalPath config --local --get $key) -NoIcon
}

Write-NewLine

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title "$($Project.Name) Project: Git Configuration" `
    -Message "Configured local git for $($Project.WebUrl)"
