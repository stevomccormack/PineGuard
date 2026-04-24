<#
.SYNOPSIS
    git config

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/git-config.ps1

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

# ------------------------------------------------------------------------------------------------

Write-MastHead "$($project.Name) Project: Local Git Configuration"
Write-Var -Name "Project Name" -Value $project.Name -NoIcon
Write-Var -Name "Project Path" -Value $project.LocalPath -NoIcon
Write-NewLine

# ------------------------------------------------------------------------------------------------

# Check if git repository is initialized
if (-not (Test-GitRepositoryIsInitialized -Path $project.LocalPath)) {
    Write-FailMessage -Title "Git Init" -Message "Git repository is not initialized. Run 'git init' first."
    exit 1
}

# ------------------------------------------------------------------------------------------------
Write-Status "Configuring local git settings for project:"

# Configure Git user details
git config --local user.name $project.UserName
git config --local user.email $project.UserEmail

# Set default branch name
git config --local init.defaultBranch $project.MainBranch

# Set merge & pull strategy
git config --local pull.ff $project.FastForward
git config --local pull.rebase $project.UseRebase

# Set Git to use SSH instead of HTTPS for GitHub
git config --local url."git@github.com:".insteadOf "https://github.com/"

# ------------------------------------------------------------------------------------------------

Write-Var -Name "git --local config user.name" -Value (git config --local user.name) -NoIcon
Write-Var -Name "git --local config user.email" -Value (git config --local user.email) -NoIcon
Write-Var -Name "git --local config init.defaultBranch" -Value (git config --local init.defaultBranch) -NoIcon
Write-Var -Name "git --local config pull.ff" -Value (git config --local pull.ff) -NoIcon
Write-Var -Name "git --local config pull.rebase" -Value (git config --local pull.rebase) -NoIcon
Write-Var -Name "git --local config url.'git@github.com:'.insteadOf" -Value (git config --local url.'git@github.com:'.insteadOf) -NoIcon
Write-NewLine

# ------------------------------------------------------------------------------------------------

Write-OkMessage -Title "$($project.Name) Project: Git Configuration" -Message "Configured Git for project: $($project.WebUrl)"