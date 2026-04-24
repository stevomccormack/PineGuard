<#
.SYNOPSIS
    git init

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/git-release.ps1

# -------------------------------------------------------------------------------------------------

try {
    . ".etc/powershell/.shared/index.ps1" *> $null
}
catch {
    throw
}

$tag = "v1.2.0"

# -------------------------------------------------------------------------------------------------

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# -------------------------------------------------------------------------------------------------

$project = $Project

# ------------------------------------------------------------------------------------------------

Write-MastHead "$($project.Name) Project: GitHub Releases"
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
gh release create $tag --generate-notes --draft

# review the draft on GitHub, then:
# gh release edit $tag --draft=false



# ------------------------------------------------------------------------------------------------

dotnet pack -c Release --output ./artifacts/releases/$tag
dotnet nuget push ./artifacts/releases/$tag/*.nupkg --api-key $Env:NUGET_API_KEY --source https://api.nuget.org/v3/index.json



# ------------------------------------------------------------------------------------------------

Write-OkMessage -Title "Git init" -Message "Pushed to existing GitHub repository: $($project.WebUrl)"
