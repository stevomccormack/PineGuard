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

# ------------------------------------------------------------------------------------------------

Write-MastHead "DotNet Tools Installation"
Write-NewLine

# ------------------------------------------------------------------------------------------------

dotnet tool install --global dotnet-sonarscanner

# ------------------------------------------------------------------------------------------------

Write-OkMessage -Title "DotNet Tools" -Message "Installed dotnet tools"