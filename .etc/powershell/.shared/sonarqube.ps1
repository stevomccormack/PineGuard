<#
.SYNOPSIS
    Declares the $SonarQube variable describing the local SonarQube server.

.DESCRIPTION
    Dot-sourced by index.ps1. Supplies the coordinates of the local SonarQube Community server
    that tools/code-scan/sonarqube/ brings up, so maintainer scripts can print dashboard links
    and resolve the token name without restating the port in three places.

.NOTES
    The scanner itself is NOT installed globally. dotnet-sonarscanner is pinned in the repo-root
    local tool manifest (.config/dotnet-tools.json) and restored by
    tools/code-scan/sonarqube/Run-SonarScanner.ps1 via `dotnet tool restore`. The previous version
    of this file carried an InstallCmd of 'dotnet tool install --global dotnet-sonarscanner',
    which contradicted that and would have shadowed the pinned version.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# SonarQube variables
# -------------------------------------------------------------------------------------------------

$sonarQubeHostUrl = 'http://localhost:9001'

$SonarQube = [pscustomobject]@{
    Name         = 'SonarQube'
    ProjectKey   = 'PineGuard'
    HostUrl      = $sonarQubeHostUrl
    DashboardUrl = "$sonarQubeHostUrl/dashboard?id=PineGuard"
    TokenUrl     = "$sonarQubeHostUrl/account/security"
    TokenName    = 'SONARQUBE_TOKEN'
    InstallPath  = 'tools/code-scan/sonarqube/Install-SonarQube.ps1'
    ScanPath     = 'tools/code-scan/sonarqube/Run-SonarScanner.ps1'
}

# -------------------------------------------------------------------------------------------------

if ($Global.Log.Enabled) {
    Write-Header "`$SonarQube variable:"
    $SonarQube | Format-List
}
