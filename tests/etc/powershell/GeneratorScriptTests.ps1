# GeneratorScriptTests.ps1
# Runs PineGuard generator scripts with default parameters and verifies success.
# Cleans up generated outputs afterwards.
#
# Intended usage (from repo root):
#   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tests/etc/powershell/GeneratorScriptTests.ps1

[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

function Assert-FileExistsAndNotEmpty {
    param(
        [Parameter(Mandatory = $true)][string]$Path
    )

    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Expected generated file was not created: $Path"
    }

    $fileInfo = Get-Item -LiteralPath $Path
    if ($fileInfo.Length -le 0) {
        throw "Generated file is empty: $Path"
    }
}

function Invoke-Generator {
    param(
        [Parameter(Mandatory = $true)][string]$ScriptPath
    )

    if (-not (Test-Path -LiteralPath $ScriptPath)) {
        throw "Generator script not found: $ScriptPath"
    }

    Write-Host "`n=== Running $ScriptPath (default params) ===" -ForegroundColor Cyan

    # Default params means we do NOT pass -EnableUpdateTarget.
    # Run in a separate pwsh process so this test script's StrictMode
    # cannot change the behavior of the generator scripts.
    & pwsh -NoProfile -ExecutionPolicy Bypass -File $ScriptPath

    if ($LASTEXITCODE -ne 0) {
        throw "Generator script failed (exit code $LASTEXITCODE): $ScriptPath"
    }
}

$scriptRoot = $PSScriptRoot
$repoRoot = (Resolve-Path (Join-Path $scriptRoot "..\..\..")).Path

$generatedDir = Join-Path $repoRoot "etc\generated"

$generators = @(
    @{ Script = "etc\powershell\iso\GenerateIsoCountries.ps1";  Output = "DefaultIsoCountryData.cs" },
    @{ Script = "etc\powershell\iso\GenerateIsoCurrencies.ps1"; Output = "DefaultIsoCurrencyData.cs" },
    @{ Script = "etc\powershell\iso\GenerateIsoLanguages.ps1";  Output = "DefaultIsoLanguageData.cs" },
    @{ Script = "etc\powershell\iana\GenerateIanaTimeZones.ps1"; Output = "DefaultIanaTimeZoneData.cs" },
    @{ Script = "etc\powershell\cldr\GenerateCldrWindowsTimeZones.ps1"; Output = "DefaultCldrWindowsTimeZoneData.cs" }
)

$generatedFiles = $generators | ForEach-Object { Join-Path $generatedDir $_.Output }

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host "Generated dir: $generatedDir" -ForegroundColor DarkGray

Set-Location -LiteralPath $repoRoot

# Ensure generated directory exists.
if (-not (Test-Path -LiteralPath $generatedDir)) {
    New-Item -ItemType Directory -Path $generatedDir -Force | Out-Null
}

# Clean any previous outputs for determinism.
foreach ($file in $generatedFiles) {
    if (Test-Path -LiteralPath $file) {
        Remove-Item -LiteralPath $file -Force
    }
}

try {
    foreach ($g in $generators) {
        $scriptPath = Join-Path $repoRoot $g.Script
        Invoke-Generator -ScriptPath $scriptPath

        $outputPath = Join-Path $generatedDir $g.Output
        Assert-FileExistsAndNotEmpty -Path $outputPath
        Write-Host "  OK Output: $outputPath" -ForegroundColor Green
    }

    Write-Host "`nAll generator scripts succeeded with defaults (EnableUpdateTarget=false)." -ForegroundColor Green
}
finally {
    Write-Host "`nCleaning up generated outputs..." -ForegroundColor Yellow

    foreach ($file in $generatedFiles) {
        if (Test-Path -LiteralPath $file) {
            Remove-Item -LiteralPath $file -Force
            Write-Host "  Deleted: $file" -ForegroundColor DarkGray
        }
    }
}
