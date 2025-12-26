# =================================================================================================
# GenerateIsoCountries.ps1
# Run the script from the .iso/ directory.
# Before running this script, ensure:
#   - .iso/iso-3166-countries/iso-3166-country-codes.csv is up-to-date. 
#   - .iso/iso-3166-countries/DefaultIsoCountryData.template.cs is up-to-date.
# =================================================================================================
# Orchestrates the generation of ISO 3166-1 country data for the PineGuard library.
#
# This script:
#   1. Validates the presence of required input files (CSV data and C# template)
#   2. Executes the generator script to produce the C# data file
#   3. Copies the generated file to the appropriate location in the source tree
#
# Directory Structure:
#   .iso/
#   ├── iso-3166-countries/
#   │   ├── iso-3166-country-codes.csv              # ISO 3166-1 source data
#   │   ├── DefaultIsoCountryData.template.cs       # C# template with placeholder
#   │   └── DefaultIsoCountryDataGenerator.ps1      # Generator script
#   ├── generated/
#   │   └── DefaultIsoCountryData.cs                # Generated output (git-ignored)
#   └── GenerateIsoCountries.ps1                    # This orchestration script
#
# Reference: https://www.iso.org/iso-3166-country-codes.html
# Standard: ISO 3166-1:2020
# =================================================================================================

[CmdletBinding()]
param(
    [bool]$EnableUpdateTarget = $false
)

# Set execution policy for this process to allow script execution
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force

$ErrorActionPreference = "Stop"

# =================================================================================================
# Configuration
# =================================================================================================

$scriptRoot = $PSScriptRoot # Ensure you are in the .iso/ directory!

# Input files
$csvPath      = Join-Path $scriptRoot "iso-3166-countries\iso-3166-country-codes.csv"
$templatePath = Join-Path $scriptRoot "iso-3166-countries\DefaultIsoCountryData.template.cs"
$generatorPath = Join-Path $scriptRoot "iso-3166-countries\DefaultIsoCountryDataGenerator.ps1"

# Output paths
$generatedDir = Join-Path $scriptRoot "generated"
$outputPath   = Join-Path $generatedDir "DefaultIsoCountryData.cs"

# Target location in source tree
$targetDir  = Join-Path $scriptRoot "..\src\PineGuard.Core\Standards\Iso"
$targetPath = Join-Path $targetDir "DefaultIsoCountryData.cs"

# =================================================================================================
# Validation
# =================================================================================================

Write-Host ""
Write-Host "═══════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host "  ISO 3166-1 Country Data Generator" -ForegroundColor Cyan
Write-Host "═══════════════════════════════════════════════════════════════════" -ForegroundColor Cyan
Write-Host ""

Write-Host "Validating input files..." -ForegroundColor Yellow

# Validate required files exist
$requiredFiles = @(
    @{ Path = $csvPath; Name = "CSV data file" }
    @{ Path = $templatePath; Name = "Template file" }
    @{ Path = $generatorPath; Name = "Generator script" }
)

foreach ($file in $requiredFiles) {
    if (-not (Test-Path $file.Path)) {
        Write-Error "Missing required file: $($file.Name) at $($file.Path)"
        exit 1
    }
    Write-Host "  ✓ $($file.Name): $($file.Path)" -ForegroundColor Green
}

# Validate target directory exists
if (-not (Test-Path $targetDir)) {
    Write-Error "Target directory does not exist: $targetDir"
    exit 1
}
Write-Host "  ✓ Target directory: $targetDir" -ForegroundColor Green

Write-Host ""

# =================================================================================================
# Ensure generated directory exists
# =================================================================================================

if (-not (Test-Path $generatedDir)) {
    Write-Host "Creating generated directory..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $generatedDir -Force | Out-Null
    Write-Host "  ✓ Created: $generatedDir" -ForegroundColor Green
    Write-Host ""
}

# =================================================================================================
# Execute Generator
# =================================================================================================

Write-Host "Generating ISO country data..." -ForegroundColor Yellow
Write-Host ""

try {
    & $generatorPath -CsvPath $csvPath -TemplatePath $templatePath -OutputPath $outputPath
    
    if ($LASTEXITCODE -ne 0 -and $null -ne $LASTEXITCODE) {
        throw "Generator script exited with code: $LASTEXITCODE"
    }
}
catch {
    Write-Error "Failed to generate ISO country data: $_"
    exit 1
}

Write-Host ""

# =================================================================================================
# Verify Output
# =================================================================================================

Write-Host "Verifying generated file..." -ForegroundColor Yellow

if (-not (Test-Path $outputPath)) {
    Write-Error "Generator did not produce output file: $outputPath"
    exit 1
}

$fileInfo = Get-Item $outputPath
Write-Host "  ✓ Generated file size: $($fileInfo.Length) bytes" -ForegroundColor Green

# Basic content validation
$content = Get-Content $outputPath -Raw
if ($content -notmatch 'namespace PineGuard\.Standards\.Iso') {
    Write-Error "Generated file does not contain expected namespace"
    exit 1
}
if ($content -match '<< COUNTRY_ROWS >>') {
    Write-Error "Generated file still contains template placeholder"
    exit 1
}

Write-Host "  ✓ Content validation passed" -ForegroundColor Green
Write-Host ""

# =================================================================================================
# Copy to Target Location
# =================================================================================================

if ($EnableUpdateTarget) {
    Write-Host "Copying to source tree..." -ForegroundColor Yellow

    try {
        Copy-Item -Path $outputPath -Destination $targetPath -Force
        Write-Host "  ✓ Copied to: $targetPath" -ForegroundColor Green
    }
    catch {
        Write-Error "Failed to copy file to target location: $_"
        exit 1
    }

    Write-Host ""
}
else {
    Write-Host "Skipping copy to source tree (EnableUpdateTarget=$false)" -ForegroundColor Yellow
    Write-Host ""
}

# =================================================================================================
# Summary
# =================================================================================================

Write-Host "═══════════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host "  ✓ Generation Complete!" -ForegroundColor Green
Write-Host "═══════════════════════════════════════════════════════════════════" -ForegroundColor Green
Write-Host ""
Write-Host "Generated file locations:" -ForegroundColor White
Write-Host "  • Generated: $outputPath" -ForegroundColor Gray
if ($EnableUpdateTarget) {
    Write-Host "  • Source:    $targetPath" -ForegroundColor Gray
}
Write-Host ""
Write-Host "Next steps:" -ForegroundColor White
Write-Host "  1. Review the changes in $targetPath" -ForegroundColor Gray
Write-Host "  2. Build the PineGuard.Core project to verify compilation" -ForegroundColor Gray
Write-Host "  3. Run unit tests to ensure data integrity" -ForegroundColor Gray
Write-Host "  4. Commit the updated DefaultIsoCountryData.cs file" -ForegroundColor Gray
Write-Host ""
