<#
.SYNOPSIS
    Restore the repository's pinned .NET local tools.

.DESCRIPTION
    PineGuard pins every .NET CLI tool it uses in the repo-root manifest
    .config/dotnet-tools.json - reportgenerator, dotnet-sonarscanner and dotCover, each with an
    exact version and rollForward disabled. This script restores that manifest so a fresh
    workstation ends up with exactly the versions CI uses.

.NOTES
    The previous version ran `dotnet tool install --global dotnet-sonarscanner`. That was wrong
    in three ways: it installed globally, so the version drifted per machine; it shadowed the
    pinned 11.3.0 that tools/code-scan/sonarqube/Run-SonarScanner.ps1 restores and expects; and
    it covered one of the three tools the repository actually needs.

    A global install of any manifest tool will still take precedence on PATH. -List prints both
    the pinned versions and any global installs that would shadow them, so a machine carrying a
    stale global copy is easy to spot.

.PARAMETER List
    Report the manifest's pinned tools and any shadowing global installs, then exit without
    restoring.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Install-DotnetTool.ps1

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Install-DotnetTool.ps1 -List
#>

[CmdletBinding()]
param(
    [switch] $List
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

$manifestPath = Join-Path $Solution.RepoRoot '.config/dotnet-tools.json'

Write-MastHead "$($Project.Name) Project: .NET Local Tools"
Write-Var -Name 'Manifest' -Value $manifestPath -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name 'dotnet')) {
    Write-FailMessage -Title 'dotnet' -Message "'dotnet' was not found on PATH. Install the .NET SDK from https://dot.net"
    exit 1
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    Write-FailMessage -Title 'Manifest' -Message "No tool manifest at $manifestPath. Create one with 'dotnet new tool-manifest'."
    exit 1
}

$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
$toolNames = @($manifest.tools.PSObject.Properties.Name)

Write-Status "Pinned tools ($($toolNames.Count)):"
foreach ($toolName in $toolNames) {
    Write-Var -Name $toolName -Value $manifest.tools.$toolName.version -NoIcon
}
Write-NewLine

# -------------------------------------------------------------------------------------------------

if ($List) {
    Write-Status 'Global installs that would shadow the manifest:'

    $globalOutput = dotnet tool list --global 2>&1
    $shadowing = @($globalOutput | Where-Object { $line = $_; $toolNames | Where-Object { $line -match [regex]::Escape($_) } })

    if ($shadowing.Count -eq 0) {
        Write-OkMessage -Title 'Global tools' -Message 'None — the manifest versions win.'
    }
    else {
        foreach ($line in $shadowing) {
            Write-FailMessage -Title 'Shadowed' -Message "$($line.ToString().Trim())  (uninstall with: dotnet tool uninstall --global <id>)"
        }
    }

    exit 0
}

# -------------------------------------------------------------------------------------------------

Write-Status 'Restoring...'

dotnet tool restore --tool-manifest $manifestPath
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'dotnet tool restore' -Message "Failed (exit code $LASTEXITCODE)."
    exit $LASTEXITCODE
}

# -------------------------------------------------------------------------------------------------

Write-NewLine
Write-OkMessage -Title '.NET Local Tools' -Message "Restored $($toolNames.Count) pinned tool(s). Invoke them with 'dotnet <command>' from the repository root."
