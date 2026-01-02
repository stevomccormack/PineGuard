# =================================================================================================
# GenerateIanaTimeZones.ps1
# Run the script from the repository root.
# Before running this script, ensure:
#   - .iana/iana-tz-zone-info/zone1970.tab is up-to-date.
#   - .iana/iana-tz-zone-info/DefaultIanaTimeZoneData.template.cs is up-to-date.
# =================================================================================================

[CmdletBinding()]
param(
    [switch]$EnableUpdateTarget
)

Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force
$ErrorActionPreference = "Stop"

$ianaRoot = $PSScriptRoot
$repoRoot = Join-Path $ianaRoot ".."
$ianaTzZoneInfoDir = Join-Path $ianaRoot "iana-tz-zone-info"

$zone1970Path = Join-Path $ianaTzZoneInfoDir "zone1970.tab"
$templatePath = Join-Path $ianaTzZoneInfoDir "DefaultIanaTimeZoneData.template.cs"
$generatorPath = Join-Path $ianaTzZoneInfoDir "DefaultIanaTimeZoneDataGenerator.ps1"

$generatedDir = Join-Path $ianaRoot "generated"
$outputPath = Join-Path $generatedDir "DefaultIanaTimeZoneData.cs"

$targetDir = Join-Path $repoRoot "src\PineGuard.Core\Iana\TimeZones"
$targetPath = Join-Path $targetDir "DefaultIanaTimeZoneData.cs"

Write-Host "" 
Write-Host "===============================================================" -ForegroundColor Cyan
Write-Host "  IANA tzdb Time Zone Data Generator" -ForegroundColor Cyan
Write-Host "===============================================================" -ForegroundColor Cyan
Write-Host "" 

Write-Host "Validating input files..." -ForegroundColor Yellow

$requiredFiles = @(
    @{ Path = $zone1970Path; Name = "zone1970.tab" }
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
Write-Host "Generating IANA time zone data..." -ForegroundColor Yellow
Write-Host "" 

try {
    $global:LASTEXITCODE = 0
    & $generatorPath -Zone1970TabPath $zone1970Path -TemplatePath $templatePath -OutputPath $outputPath

    if (-not $?) {
        throw "Generator script failed."
    }
}
catch {
    Write-Error "Failed to generate IANA time zone data: $_"
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
