
[CmdletBinding()]
param (
    # Path to the CSV file containing ISO 4217 currency data to be processed.
    # Must exist and be accessible. Example: ".\iso-4217-currency-codes.csv"
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
    # Must exist and contain the '<< CURRENCY_ROWS >>' placeholder.
    # Example: ".\DefaultIsoCurrencyData.template.cs"
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
    # Example: "..\generated\DefaultIsoCurrencyData.cs"
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$OutputPath
)

# =================================================================================================
# Types
# =================================================================================================

# Represents an ISO 4217 currency with alpha-3 and numeric codes.
# Reference: https://www.iso.org/iso-4217-currency-codes.html
class IsoCurrency {
    [string]$IsoAlpha3
    [string]$NumericCode
    [string]$MinorUnit
    [string]$Name

    IsoCurrency(
        [string]$isoAlpha3,
        [string]$numericCode,
        [string]$minorUnit,
        [string]$name
    ) {
        $this.IsoAlpha3   = $isoAlpha3
        $this.NumericCode = $numericCode
        $this.MinorUnit   = $minorUnit
        $this.Name        = $name
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
        # Example: "  " (whitespace will fail), "" (empty will fail), "USD" (valid)
        [Parameter(Mandatory)][AllowEmptyString()][string]$Value,
        
        # The name of the field being validated, used in error messages.
        # Example: "IsoAlpha3", "CurrencyName"
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
        # Example: PSCustomObject with properties like @{IsoAlpha3="USD"; NumericCode="840"}
        [Parameter(Mandatory)][object]$Object,
        
        # The name of the required property to check for.
        # Example: "IsoAlpha3", "Name"
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
        # Example: "IsoAlpha3", "NumericCode"
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$KeyName,
        
        # The collection of items to check for duplicates.
        # Example: @("USD", "EUR", "GBP") or @("840", "978", "826")
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
# New-IsoCurrency:
# Create and validate an ISO 4217 currency object with all required codes.
# Reference: https://www.iso.org/iso-4217-currency-codes.html
# Standard: ISO 4217:2015
# =================================================================================================

function New-IsoCurrency {
    [CmdletBinding()]
    param (
        # ISO 4217 alpha-3 code (three-letter currency code).
        # Must be exactly 3 letters (case-insensitive, will be normalized to uppercase).
        # Example: "USD", "EUR", "GBP"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^[A-Za-z]{3}$')]
        [string]$IsoAlpha3,

        # ISO 4217 numeric code (three-digit currency code).
        # Must be exactly 3 digits (leading zeros preserved).
        # Example: "840", "978", "826"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^\d{3}$')]
        [string]$NumericCode,

        # Minor unit (number of decimal places).
        # Can be a number (0-4) or "-" for currencies without minor units.
        # Example: "2", "0", "3", "-"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$MinorUnit,

        # Currency name as defined in ISO 4217.
        # Example: "US Dollar", "Euro", "Pound Sterling"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )

    # Normalize input values
    $isoAlpha3   = $IsoAlpha3.Trim().ToUpperInvariant()
    $numericCode = $NumericCode.Trim()
    $minorUnit   = $MinorUnit.Trim()
    $name        = $Name.Trim()

    # Guards - re-validate after normalization
    Assert-NotNullOrWhiteSpace -Value $name -FieldName "Name"

    if ($isoAlpha3.Length -ne 3 -or $isoAlpha3 -notmatch '^[A-Z]{3}$') {
        throw "Invalid IsoAlpha3 after normalization: '$isoAlpha3'. Must be 3 uppercase letters."
    }

    if ($numericCode.Length -ne 3 -or $numericCode -notmatch '^\d{3}$') {
        throw "Invalid NumericCode after normalization: '$numericCode'. Must be 3 digits."
    }

    # Validate MinorUnit - can be a digit or "-"
    if ($minorUnit -ne "-" -and $minorUnit -notmatch '^\d$') {
        throw "Invalid MinorUnit after normalization: '$minorUnit'. Must be a single digit or '-'."
    }

    return [IsoCurrency]::new($isoAlpha3, $numericCode, $minorUnit, $name)
}

# =================================================================================================
# Load CSV
# Reads ISO 4217 currency data from CSV file.
# Expected columns: IsoAlpha3, NumericCode, MinorUnit, Name
# Reference: https://www.iso.org/iso-4217-currency-codes.html
# =================================================================================================

$rows = Import-Csv -Path $CsvPath

if (-not $rows -or $rows.Count -eq 0) {
    throw "CSV contained no rows: $CsvPath"
}

# Validate headers
$firstRow = $rows | Select-Object -First 1
Assert-HasProperty -Object $firstRow -PropertyName "IsoAlpha3"
Assert-HasProperty -Object $firstRow -PropertyName "NumericCode"
Assert-HasProperty -Object $firstRow -PropertyName "MinorUnit"
Assert-HasProperty -Object $firstRow -PropertyName "Name"

# =================================================================================================
# Parse + Validate + Create IsoCurrency objects
# =================================================================================================

$currencies = New-Object System.Collections.Generic.List[IsoCurrency]

foreach ($row in $rows) {

    $currency = New-IsoCurrency `
        -IsoAlpha3 $row.IsoAlpha3 `
        -NumericCode $row.NumericCode `
        -MinorUnit $row.MinorUnit `
        -Name $row.Name

    $currencies.Add($currency)
}

# Deterministic output order (sorted by ISO alpha-3 code)
$currencies = $currencies | Sort-Object -Property IsoAlpha3

# =================================================================================================
# Uniqueness validation (critical)
# ISO 4217 requires all codes to be globally unique.
# Reference: https://www.iso.org/standard/64758.html
# =================================================================================================

Assert-Unique -KeyName "IsoAlpha3"   -Items ($currencies | ForEach-Object { $_.IsoAlpha3 })
Assert-Unique -KeyName "NumericCode" -Items ($currencies | ForEach-Object { $_.NumericCode })

# =================================================================================================
# Generate C# rows
# =================================================================================================

$currencyRows = foreach ($c in $currencies) {
    $safeName = New-EscapedCSharpString -Value $c.Name
    
    # Convert MinorUnit: "-" becomes -1, otherwise parse as int
    $decimalPlaces = if ($c.MinorUnit -eq "-") { "-1" } else { $c.MinorUnit }
    
    "            new IsoCurrency(`"$($c.IsoAlpha3)`", `"$($c.NumericCode)`", $decimalPlaces, `"$safeName`"),"
}

# =================================================================================================
# Template injection
# =================================================================================================

$template = Get-Content -Path $TemplatePath -Raw

if ($template -notmatch '\<< CURRENCY_ROWS >>') {
    throw "Template does not contain required placeholder '<< CURRENCY_ROWS >>': $TemplatePath"
}

$output = $template.Replace("<< CURRENCY_ROWS >>", ($currencyRows -join [Environment]::NewLine))

# =================================================================================================
# Write output
# =================================================================================================

$dir = Split-Path -Path $OutputPath -Parent
if (-not [string]::IsNullOrWhiteSpace($dir) -and -not (Test-Path $dir)) {
    New-Item -ItemType Directory -Path $dir | Out-Null
}

Set-Content -Path $OutputPath -Value $output -Encoding UTF8

Write-Host "✅ Generated: $OutputPath"
Write-Host "Currencies: $($currencies.Count)"
