<#
.SYNOPSIS
    Shared ReportGenerator dotnet tool helper for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Ensure-ReportGenerator into the calling script's scope.
    Requires path.ps1 (Ensure-Directory) to be loaded first.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Ensure-ReportGenerator {
    <#
    .SYNOPSIS
        Ensures the reportgenerator dotnet tool is installed in .dotnet/tools; returns the exe path.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot
    )

    $toolsDir = Join-Path $RepoRoot '.dotnet/tools'
    $exe = Join-Path $toolsDir 'reportgenerator.exe'

    if (Test-Path $exe) {
        return $exe
    }

    Ensure-Directory -Path $toolsDir

    Write-Host "Installing reportgenerator tool into: $toolsDir" -ForegroundColor Cyan
    & dotnet tool install dotnet-reportgenerator-globaltool --tool-path $toolsDir | Out-Host

    if (-not (Test-Path $exe)) {
        throw "reportgenerator.exe was not found after tool install. Expected at: $exe"
    }

    return $exe
}
