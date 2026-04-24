<#
.SYNOPSIS
    solution

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

# .etc/powershell/.shared/solution.ps1

# -------------------------------------------------------------------------------------------------
# Solution Variables
# -------------------------------------------------------------------------------------------------

$SonarQube = [pscustomobject]@{
    Name       = "SonarQube"
    ProjectKey = "PineGuard"
    HostUrl    = 'http://localhost:9001'
    InstallCmd = 'dotnet tool install --global dotnet-sonarscanner'
}

# -------------------------------------------------------------------------------------------------

if ($Global.Log.Enabled) {
    Write-Header "`$SonarQube` Variable:"
    $SonarQube | Format-List
}
