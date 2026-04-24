<#
.SYNOPSIS
    git init

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/git-init.ps1

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

Write-MastHead "$($project.Name) Project: Git Repository Initialization"
Write-Var -Name "Project Name" -Value $project.Name -NoIcon
Write-Var -Name "Project Path" -Value $project.LocalPath -NoIcon
Write-NewLine

# ------------------------------------------------------------------------------------------------

# Check if git repository is already initialized
if (Test-GitRepositoryIsInitialized -Path $project.LocalPath) {
    Write-FatalMessage -Title "Git Init" -Message "Git repository is already initialized."
    exit 1
}

# Check if git repository exists
if (Test-GitRepositoryExists -RepositoryUrl $project.GitUrl) {
    Write-FatalMessage -Title "Git Init" -Message "Git repository already exists."
    exit 1
}

# ------------------------------------------------------------------------------------------------

Write-Status "Initialising and configuring Git repository for project:"

# Initialize local repository
git init --initial-branch=$($project.MainBranch)

# Stage and commit files
git add .
git commit -m "Draft: Initial commiting project files
- Created project structure
- Created test project core structure
- Create core Rules scaffolding
- Created .gitiignore
- Created README.md
- Created LICENSE"

# Push to repository
git remote add origin $($project.GitUrl)
git branch --set-upstream-to=origin/$($project.MainBranch) $($project.MainBranch)
git push -u origin $($project.MainBranch) # auto sets upstream

# ------------------------------------------------------------------------------------------------

Write-OkMessage -Title "Git init" -Message "Pushed to existing GitHub repository: $($project.WebUrl)"