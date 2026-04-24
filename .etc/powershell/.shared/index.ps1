<#
.SYNOPSIS
  index

.DESCRIPTION
  Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/.shared/project.ps1

# -------------------------------------------------------------------------------------------------

Push-Location "D:\Steve McCormack\GitHub\@stevomccormack\Onboarding\"

. ".shared\index.ps1"

Pop-Location

# -------------------------------------------------------------------------------------------------

# Import .env file if it exists
# Import-DotEnv -Path ".etc/powershell/.env" -Force -ErrorAction SilentlyContinue
Import-DotEnv -Path ".etc/powershell/.env" -ErrorAction SilentlyContinue

# -------------------------------------------------------------------------------------------------

. ".etc/powershell/.shared/project.ps1"
. ".etc/powershell/.shared/solution.ps1"
. ".etc/powershell/.shared/sonarqube.ps1"
. ".etc/powershell/.shared/github.ps1"
