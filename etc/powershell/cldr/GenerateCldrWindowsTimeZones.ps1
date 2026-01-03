# =================================================================================================
# GenerateCldrWindowsTimeZones.ps1
# Run the script from the repository root.
# Before running this script, ensure:
#   - etc/powershell/cldr/cldr-windows-zones/windowsZones.xml is up-to-date.
#   - etc/powershell/cldr/cldr-windows-zones/DefaultCldrWindowsTimeZoneData.template.cs is up-to-date.
# =================================================================================================

[CmdletBinding()]
param(
    [switch]$EnableUpdateTarget
)

Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force
$ErrorActionPreference = "Stop"

$cldrRoot = $PSScriptRoot
$repoRoot = (Resolve-Path (Join-Path $cldrRoot "..\..\..")).Path
$windowsZonesDir = Join-Path $cldrRoot "cldr-windows-zones"

$windowsZonesXmlPath = Join-Path $windowsZonesDir "windowsZones.xml"
$templatePath = Join-Path $windowsZonesDir "DefaultCldrWindowsTimeZoneData.template.cs"
$generatorPath = Join-Path $windowsZonesDir "DefaultCldrWindowsTimeZoneDataGenerator.ps1"

$generatedDir = Join-Path $cldrRoot "generated"
$outputPath = Join-Path $generatedDir "DefaultCldrWindowsTimeZoneData.cs"

$targetDir = Join-Path $repoRoot "src\PineGuard.Core\Cldr\TimeZones"
$targetPath = Join-Path $targetDir "DefaultCldrWindowsTimeZoneData.cs"

Write-Host "" 
Write-Host "===============================================================" -ForegroundColor Cyan
Write-Host "  CLDR Windows Time Zone Mapping Generator" -ForegroundColor Cyan
Write-Host "===============================================================" -ForegroundColor Cyan
Write-Host "" 

Write-Host "Validating input files..." -ForegroundColor Yellow

$requiredFiles = @(
    @{ Path = $windowsZonesXmlPath; Name = "windowsZones.xml" }
    @{ Path = $templatePath; Name = "Template file" }
    @{ Path = $generatorPath; Name = "Generator script" }
)

foreach ($file in $requiredFiles) {
    if (-not (Test-Path $file.Path)) {
        Write-Error "Missing required file: $($file.Name) at $($file.Path)"
        exit 1
    }
    Write-Host "  OK $($file.Name): $($file.Path)" -ForegroundColor Green
}

if (-not (Test-Path $generatedDir)) {
    New-Item -ItemType Directory -Path $generatedDir -Force | Out-Null
}

Write-Host "" 
Write-Host "Generating CLDR Windows time zone mapping data..." -ForegroundColor Yellow
Write-Host "" 

try {
    $global:LASTEXITCODE = 0

    & $generatorPath -WindowsZonesXmlPath $windowsZonesXmlPath -TemplatePath $templatePath -OutputPath $outputPath

    if (-not $?) {
        throw "Generator script failed."
    }
}
catch {
    Write-Error "Failed to generate CLDR Windows time zone mapping data: $_"
    exit 1
}

if (-not (Test-Path $outputPath)) {
    Write-Error "Generator did not produce output file: $outputPath"
    exit 1
}

if ($EnableUpdateTarget) {
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
    }

    Copy-Item -Path $outputPath -Destination $targetPath -Force
    Write-Host "  OK Copied to: $targetPath" -ForegroundColor Green
}
else {
    Write-Host "Skipping copy to source tree (EnableUpdateTarget=$EnableUpdateTarget)" -ForegroundColor Yellow
}

Write-Host "" 
Write-Host "===============================================================" -ForegroundColor Green
Write-Host "  OK Generation Complete!" -ForegroundColor Green
Write-Host "===============================================================" -ForegroundColor Green
Write-Host "" 
Write-Host "Generated file locations:" -ForegroundColor White
Write-Host "  - Generated: $outputPath" -ForegroundColor Gray
if ($EnableUpdateTarget) {
    Write-Host "  - Source:    $targetPath" -ForegroundColor Gray
}
Write-Host "" 
