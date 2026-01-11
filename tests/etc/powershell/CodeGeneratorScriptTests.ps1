# GeneratorScriptTests.ps1
# Runs PineGuard generator scripts with default parameters and verifies success.
# Cleans up generated outputs afterwards.
#
# Intended usage (from repo root):
#   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tests/etc/powershell/CodeGeneratorScriptTests.ps1

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

$generatedRoot = Join-Path $repoRoot "etc\artifacts\code-generators"

$generators = @(
    @{ Script = "etc\powershell\code-generators\iso\GenerateIsoCountries.ps1";  Category = "iso";  Output = "DefaultIsoCountryData.cs" },
    @{ Script = "etc\powershell\code-generators\iso\GenerateIsoCurrencies.ps1"; Category = "iso";  Output = "DefaultIsoCurrencyData.cs" },
    @{ Script = "etc\powershell\code-generators\iso\GenerateIsoLanguages.ps1";  Category = "iso";  Output = "DefaultIsoLanguageData.cs" },
    @{ Script = "etc\powershell\code-generators\iana\GenerateIanaTimeZones.ps1"; Category = "iana"; Output = "DefaultIanaTimeZoneData.cs" },
    @{ Script = "etc\powershell\code-generators\cldr\GenerateCldrWindowsTimeZones.ps1"; Category = "cldr"; Output = "DefaultCldrWindowsTimeZoneData.cs" }
)

$generatedFiles = $generators | ForEach-Object { Join-Path (Join-Path $generatedRoot $_.Category) $_.Output }

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host "Generated root: $generatedRoot" -ForegroundColor DarkGray

Set-Location -LiteralPath $repoRoot

# Ensure generated directory exists.
if (-not (Test-Path -LiteralPath $generatedRoot)) {
    New-Item -ItemType Directory -Path $generatedRoot -Force | Out-Null
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

        $outputPath = Join-Path (Join-Path $generatedRoot $g.Category) $g.Output
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
