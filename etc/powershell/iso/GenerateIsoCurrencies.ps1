# =================================================================================================
# GenerateIsoCurrencies.ps1
# Run the script from the repository root.
# Before running this script, ensure:
#   - .iso/iso-4217-currencies/iso-4217-currency-codes.csv is up-to-date. 
#   - .iso/iso-4217-currencies/DefaultIsoCurrencyData.template.cs is up-to-date.
# =================================================================================================
# Orchestrates the generation of ISO 4217 currency data for the PineGuard library.
#
# This script:
#   1. Validates the presence of required input files (CSV data and C# template)
#   2. Executes the generator script to produce the C# data file
#   3. Copies the generated file to the appropriate location in the source tree
#
# Directory Structure:
#   .iso/
#   ├── iso-4217-currencies/
#   │   ├── iso-4217-currency-codes.csv              # ISO 4217 source data
#   │   ├── DefaultIsoCurrencyData.template.cs       # C# template with placeholder
#   │   └── DefaultIsoCurrencyDataGenerator.ps1      # Generator script
#   ├── generated/
#   │   └── DefaultIsoCurrencyData.cs                # Generated output (git-ignored)
#   └── GenerateIsoCurrencies.ps1                    # This orchestration script
#
# Reference: https://www.iso.org/iso-4217-currency-codes.html
# Standard: ISO 4217:2015
# =================================================================================================

[CmdletBinding()]
param(
    [switch]$EnableUpdateTarget
)

# Set execution policy for this process to allow script execution
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass -Force

$ErrorActionPreference = "Stop"

# =================================================================================================
# Configuration
# =================================================================================================

$scriptRoot = $PSScriptRoot # Ensure you are in the .iso/ directory!

# Input files
$csvPath      = Join-Path $scriptRoot "iso-4217-currencies\iso-4217-currency-codes.csv"
$templatePath = Join-Path $scriptRoot "iso-4217-currencies\DefaultIsoCurrencyData.template.cs"
$generatorPath = Join-Path $scriptRoot "iso-4217-currencies\DefaultIsoCurrencyDataGenerator.ps1"

# Output paths
$generatedDir = Join-Path $scriptRoot "generated"
$outputPath   = Join-Path $generatedDir "DefaultIsoCurrencyData.cs"

# Target location in source tree
$targetDir  = Join-Path $scriptRoot "..\src\PineGuard.Core\Iso\Currencies"
$targetPath = Join-Path $targetDir "DefaultIsoCurrencyData.cs"

# =================================================================================================
# Validation
# =================================================================================================

Write-Host ""
Write-Host "===============================================================" -ForegroundColor Cyan
Write-Host "  ISO 4217 Currency Data Generator" -ForegroundColor Cyan
Write-Host "===============================================================" -ForegroundColor Cyan
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
    Write-Host "  OK $($file.Name): $($file.Path)" -ForegroundColor Green
}

Write-Host ""

# =================================================================================================
# Ensure generated directory exists
# =================================================================================================

if (-not (Test-Path $generatedDir)) {
    Write-Host "Creating generated directory..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $generatedDir -Force | Out-Null
    Write-Host "  OK Created: $generatedDir" -ForegroundColor Green
    Write-Host ""
}

# =================================================================================================
# Execute Generator
# =================================================================================================

Write-Host "Generating ISO currency data..." -ForegroundColor Yellow
Write-Host ""

try {
    $global:LASTEXITCODE = 0
    & $generatorPath -CsvPath $csvPath -TemplatePath $templatePath -OutputPath $outputPath

    if (-not $?) {
        throw "Generator script failed."
    }
}
catch {
    Write-Error "Failed to generate ISO currency data: $_"
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
Write-Host "  OK Generated file size: $($fileInfo.Length) bytes" -ForegroundColor Green

# Basic content validation
$content = Get-Content $outputPath -Raw
if (-not $content.Contains('namespace PineGuard.Iso.Currencies;')) {
    Write-Error "Generated file does not contain expected namespace"
    exit 1
}
if ($content.Contains('<< CURRENCY_ROWS >>')) {
    Write-Error "Generated file still contains template placeholder"
    exit 1
}

Write-Host "  OK Content validation passed" -ForegroundColor Green
Write-Host ""

# =================================================================================================
# Copy to Target Location
# =================================================================================================

if ($EnableUpdateTarget) {
    if (-not (Test-Path $targetDir)) {
        New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
    }

    Write-Host "Copying to source tree..." -ForegroundColor Yellow

    try {
        Copy-Item -Path $outputPath -Destination $targetPath -Force
        Write-Host "  OK Copied to: $targetPath" -ForegroundColor Green
    }
    catch {
        Write-Error "Failed to copy file to target location: $_"
        exit 1
    }

    Write-Host ""
}
else {
    Write-Host "Skipping copy to source tree (EnableUpdateTarget=$EnableUpdateTarget)" -ForegroundColor Yellow
    Write-Host ""
}

# =================================================================================================
# Summary
# =================================================================================================

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
Write-Host "Next steps:" -ForegroundColor White
Write-Host "  1. Review the changes in $targetPath" -ForegroundColor Gray
Write-Host "  2. Build the PineGuard.Core project to verify compilation" -ForegroundColor Gray
Write-Host "  3. Run unit tests to ensure data integrity" -ForegroundColor Gray
Write-Host "  4. Commit the updated DefaultIsoCurrencyData.cs file" -ForegroundColor Gray
Write-Host ""
