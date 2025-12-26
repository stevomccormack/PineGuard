
[CmdletBinding()]
param (
    # Path to the CSV file containing ISO country data to be processed.
    # Must exist and be accessible. Example: ".\iso-3166-country-data.csv"
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [ValidateScript({ 
        if (-not (Test-Path $_)) {
            throw "CSV file not found: $_"
        }
        $true
    })]
    [string]$CsvPath,

    # Path to the template file used for generating the output C# code.
    # Must exist and contain the '<< COUNTRY_ROWS >>' placeholder.
    # Example: ".\DefaultIsoCountryData.template.cs"
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [ValidateScript({ 
        if (-not (Test-Path $_)) {
            throw "Template file not found: $_"
        }
        $true
    })]
    [string]$TemplatePath,

    # Path where the generated C# output file will be saved.
    # Parent directory will be created if it doesn't exist.
    # Example: "..\generated\DefaultIsoCountryData.cs"
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$OutputPath
)

# =================================================================================================
# Types
# =================================================================================================

# Represents an ISO 3166-1 country with alpha-2, alpha-3, and numeric codes.
# Reference: https://www.iso.org/iso-3166-country-codes.html
class IsoCountry {
    [string]$IsoAlpha2
    [string]$IsoAlpha3
    [string]$IsoNumeric
    [string]$Name

    IsoCountry(
        [string]$isoAlpha2,
        [string]$isoAlpha3,
        [string]$isoNumeric,
        [string]$name
    ) {
        $this.IsoAlpha2  = $isoAlpha2
        $this.IsoAlpha3  = $isoAlpha3
        $this.IsoNumeric = $isoNumeric
        $this.Name       = $name
    }
}

# =================================================================================================
# Guards
# =================================================================================================

# Validates that a string value is not null, empty, or whitespace.
# Throws an exception if validation fails.
function Assert-NotNullOrWhiteSpace {
    [CmdletBinding()]
    param (
        # The string value to validate. Can be empty string for validation purposes.
        # Example: "  " (whitespace will fail), "" (empty will fail), "US" (valid)
        [Parameter(Mandatory)][AllowEmptyString()][string]$Value,
        
        # The name of the field being validated, used in error messages.
        # Example: "IsoAlpha2", "CountryName"
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$FieldName
    )

    if ([string]::IsNullOrWhiteSpace($Value)) {
        throw "Invalid $($FieldName): value cannot be null/empty/whitespace."
    }
}

# Validates that a CSV row object contains a required property/column.
# Throws an exception if the property is missing.
function Assert-HasProperty {
    [CmdletBinding()]
    param (
        # The CSV row object to validate.
        # Example: PSCustomObject with properties like @{IsoAlpha2="US"; IsoAlpha3="USA"}
        [Parameter(Mandatory)][object]$Object,
        
        # The name of the required property to check for.
        # Example: "IsoAlpha2", "EnglishShortName"
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$PropertyName
    )

    if (-not ($Object.PSObject.Properties.Name -contains $PropertyName)) {
        throw "CSV is missing required column '$PropertyName'."
    }
}

# Validates that all items in a collection are unique.
# Throws an exception if duplicates are found, showing up to 5 examples.
function Assert-Unique {
    [CmdletBinding()]
    param (
        # The name of the key being validated, used in error messages.
        # Example: "IsoAlpha2", "IsoNumeric"
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$KeyName,
        
        # The collection of items to check for duplicates.
        # Example: @("US", "CA", "MX") or @("840", "124", "484")
        [Parameter(Mandatory)][System.Collections.IEnumerable]$Items
    )

    $dupes =
        $Items |
        Group-Object |
        Where-Object { $_.Count -gt 1 } |
        Select-Object -First 5

    if ($dupes) {
        $sample = ($dupes | ForEach-Object { "$($_.Name) x$($_.Count)" }) -join ", "
        throw "Duplicate $KeyName detected. Examples: $sample"
    }
}

# =================================================================================================
# Utilities
# =================================================================================================

# Escapes special characters in a string for safe use in C# code.
# Handles backslashes and double quotes.
function New-EscapedCSharpString {
    [CmdletBinding()]
    param (
        # The string value to escape for C# code.
        # Example: "Test\"Value" → "Test\\\"Value"
        [Parameter(Mandatory)][AllowEmptyString()][string]$Value
    )

    return $Value.Replace('\', '\\').Replace('"', '\"')
}

# =================================================================================================
# New-IsoCountry:
# Create and validate an ISO 3166-1 country object with all required codes.
# Reference: https://www.iso.org/iso-3166-country-codes.html
# Standard: ISO 3166-1:2020
# =================================================================================================

function New-IsoCountry {
    [CmdletBinding()]
    param (
        # ISO 3166-1 alpha-2 code (two-letter country code).
        # Must be exactly 2 letters (case-insensitive, will be normalized to uppercase).
        # Example: "US", "GB", "JP"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^[A-Za-z]{2}$')]
        [string]$IsoAlpha2,

        # ISO 3166-1 alpha-3 code (three-letter country code).
        # Must be exactly 3 letters (case-insensitive, will be normalized to uppercase).
        # Example: "USA", "GBR", "JPN"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^[A-Za-z]{3}$')]
        [string]$IsoAlpha3,

        # ISO 3166-1 numeric code (three-digit country code).
        # Must be exactly 3 digits (leading zeros preserved).
        # Example: "840", "826", "392"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^\d{3}$')]
        [string]$IsoNumeric,

        # English short name of the country as defined in ISO 3166-1.
        # Example: "United States", "United Kingdom", "Japan"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )

    # Normalize input values
    $isoAlpha2  = $IsoAlpha2.Trim().ToUpperInvariant()
    $isoAlpha3  = $IsoAlpha3.Trim().ToUpperInvariant()
    $isoNumeric = $IsoNumeric.Trim()
    $name       = $Name.Trim()

    # Guards - re-validate after normalization
    Assert-NotNullOrWhiteSpace -Value $name -FieldName "Name"

    if ($isoAlpha2.Length -ne 2 -or $isoAlpha2 -notmatch '^[A-Z]{2}$') {
        throw "Invalid IsoAlpha2 after normalization: '$isoAlpha2'. Must be 2 uppercase letters."
    }

    if ($isoAlpha3.Length -ne 3 -or $isoAlpha3 -notmatch '^[A-Z]{3}$') {
        throw "Invalid IsoAlpha3 after normalization: '$isoAlpha3'. Must be 3 uppercase letters."
    }

    if ($isoNumeric.Length -ne 3 -or $isoNumeric -notmatch '^\d{3}$') {
        throw "Invalid IsoNumeric after normalization: '$isoNumeric'. Must be 3 digits."
    }

    return [IsoCountry]::new($isoAlpha2, $isoAlpha3, $isoNumeric, $name)
}

# =================================================================================================
# Load CSV
# Reads ISO 3166-1 country data from CSV file.
# Expected columns: EnglishShortName, IsoAlpha2, IsoAlpha3, IsoNumeric
# Reference: https://www.iso.org/obp/ui/#search
# =================================================================================================

$rows = Import-Csv -Path $CsvPath

if (-not $rows -or $rows.Count -eq 0) {
    throw "CSV contained no rows: $CsvPath"
}

# Validate headers (based on our generated schema)
$firstRow = $rows | Select-Object -First 1
Assert-HasProperty -Object $firstRow -PropertyName "EnglishShortName"
Assert-HasProperty -Object $firstRow -PropertyName "IsoAlpha2"
Assert-HasProperty -Object $firstRow -PropertyName "IsoAlpha3"
Assert-HasProperty -Object $firstRow -PropertyName "IsoNumeric"

# =================================================================================================
# Parse + Validate + Create IsoCountry objects
# =================================================================================================

$countries = New-Object System.Collections.Generic.List[IsoCountry]

foreach ($row in $rows) {

    $country = New-IsoCountry `
        -IsoAlpha2 $row.IsoAlpha2 `
        -IsoAlpha3 $row.IsoAlpha3 `
        -IsoNumeric $row.IsoNumeric `
        -Name $row.EnglishShortName

    $countries.Add($country)
}

# Deterministic output order (sorted by ISO alpha-2 code)
$countries = $countries | Sort-Object -Property IsoAlpha2

# =================================================================================================
# Uniqueness validation (critical)
# ISO 3166-1 requires all codes to be globally unique.
# Reference: https://www.iso.org/standard/72482.html
# =================================================================================================

Assert-Unique -KeyName "IsoAlpha2"  -Items ($countries | ForEach-Object { $_.IsoAlpha2 })
Assert-Unique -KeyName "IsoAlpha3"  -Items ($countries | ForEach-Object { $_.IsoAlpha3 })
Assert-Unique -KeyName "IsoNumeric" -Items ($countries | ForEach-Object { $_.IsoNumeric })

# =================================================================================================
# Generate C# rows
# =================================================================================================

$countryRows = foreach ($c in $countries) {
    $safeName = New-EscapedCSharpString -Value $c.Name
    "            new IsoCountry(`"$($c.IsoAlpha2)`", `"$($c.IsoAlpha3)`", `"$($c.IsoNumeric)`", `"$safeName`"),"
}

# =================================================================================================
# Template injection
# =================================================================================================

$template = Get-Content -Path $TemplatePath -Raw

if ($template -notmatch '\<< COUNTRY_ROWS >>') {
    throw "Template does not contain required placeholder '<< COUNTRY_ROWS >>': $TemplatePath"
}

$output = $template.Replace("<< COUNTRY_ROWS >>", ($countryRows -join [Environment]::NewLine))

# =================================================================================================
# Write output
# =================================================================================================

$dir = Split-Path -Path $OutputPath -Parent
if (-not [string]::IsNullOrWhiteSpace($dir) -and -not (Test-Path $dir)) {
    New-Item -ItemType Directory -Path $dir | Out-Null
}

Set-Content -Path $OutputPath -Value $output -Encoding UTF8

Write-Host "✅ Generated: $OutputPath"
Write-Host "Countries: $($countries.Count)"
