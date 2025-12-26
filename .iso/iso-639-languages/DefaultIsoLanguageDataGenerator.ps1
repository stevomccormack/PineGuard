
[CmdletBinding()]
param (
    # Path to the CSV file containing ISO 639 language data to be processed.
    # Must exist and be accessible. Example: ".\iso-639-language-codes.csv"
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
    # Must exist and contain the '<< LANGUAGE_ROWS >>' placeholder.
    # Example: ".\DefaultIsoLanguageData.template.cs"
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
    # Example: "..\generated\DefaultIsoLanguageData.cs"
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$OutputPath
)

# =================================================================================================
# Types
# =================================================================================================

# Represents an ISO 639 language with alpha-2 and alpha-3 codes.
# Reference: https://www.iso.org/iso-639-language-codes.html
class IsoLanguage {
    [string]$Alpha2Code
    [string]$Alpha3Code
    [string]$Name

    IsoLanguage(
        [string]$alpha2Code,
        [string]$alpha3Code,
        [string]$name
    ) {
        $this.Alpha2Code = $alpha2Code
        $this.Alpha3Code = $alpha3Code
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
        [Parameter(Mandatory)][AllowEmptyString()][string]$Value,
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
        [Parameter(Mandatory)][object]$Object,
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
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$KeyName,
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
function New-EscapedCSharpString {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)][AllowEmptyString()][string]$Value
    )

    return $Value.Replace('\', '\\').Replace('"', '\"')
}

# =================================================================================================
# New-IsoLanguage:
# Create and validate an ISO 639 language object with all required codes.
# Reference: https://www.iso.org/iso-639-language-codes.html
# Standard: ISO 639
# =================================================================================================

function New-IsoLanguage {
    [CmdletBinding()]
    param (
        # ISO 639-1 alpha-2 code (two-letter language code).
        # Must be exactly 2 letters (case-insensitive, will be normalized to lowercase).
        # Example: "en", "es", "fr"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^[A-Za-z]{2}$')]
        [string]$Alpha2Code,

        # ISO 639-2/T alpha-3 code (three-letter language code).
        # Must be exactly 3 letters (case-insensitive, will be normalized to lowercase).
        # Example: "eng", "spa", "fra"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [ValidatePattern('^[A-Za-z]{3}$')]
        [string]$Alpha3Code,

        # English name of the language as defined in ISO 639.
        # Example: "English", "Spanish", "French"
        [Parameter(Mandatory)]
        [ValidateNotNullOrEmpty()]
        [string]$Name
    )

    # Normalize input values
    $alpha2Code = $Alpha2Code.Trim().ToLowerInvariant()
    $alpha3Code = $Alpha3Code.Trim().ToLowerInvariant()
    $name       = $Name.Trim()

    # Guards - re-validate after normalization
    Assert-NotNullOrWhiteSpace -Value $name -FieldName "Name"

    if ($alpha2Code.Length -ne 2 -or $alpha2Code -notmatch '^[a-z]{2}$') {
        throw "Invalid Alpha2Code after normalization: '$alpha2Code'. Must be 2 lowercase letters."
    }

    if ($alpha3Code.Length -ne 3 -or $alpha3Code -notmatch '^[a-z]{3}$') {
        throw "Invalid Alpha3Code after normalization: '$alpha3Code'. Must be 3 lowercase letters."
    }

    return [IsoLanguage]::new($alpha2Code, $alpha3Code, $name)
}

# =================================================================================================
# Main Processing
# =================================================================================================

$ErrorActionPreference = "Stop"

try {
    Write-Verbose "Loading CSV from: $CsvPath"
    $rows = Import-Csv -Path $CsvPath -ErrorAction Stop

    if (-not $rows -or $rows.Count -eq 0) {
        throw "CSV file is empty or could not be parsed: $CsvPath"
    }

    # Validate headers
    $firstRow = $rows | Select-Object -First 1
    Assert-HasProperty -Object $firstRow -PropertyName "Alpha2Code"
    Assert-HasProperty -Object $firstRow -PropertyName "Alpha3Code"
    Assert-HasProperty -Object $firstRow -PropertyName "Name"

    Write-Verbose "Parsing $($rows.Count) language records..."
    
    # Parse + Validate + Create IsoLanguage objects
    $languages = New-Object System.Collections.Generic.List[IsoLanguage]

    foreach ($row in $rows) {
        $language = New-IsoLanguage `
            -Alpha2Code $row.Alpha2Code `
            -Alpha3Code $row.Alpha3Code `
            -Name $row.Name
        
        $languages.Add($language)
    }

    # Sort by Alpha2Code
    $languages = $languages | Sort-Object -Property Alpha2Code

    Write-Verbose "Validating uniqueness..."
    Assert-Unique -KeyName "Alpha2Code" -Items ($languages | ForEach-Object { $_.Alpha2Code })
    Assert-Unique -KeyName "Alpha3Code" -Items ($languages | ForEach-Object { $_.Alpha3Code })

    Write-Verbose "Generating C# code..."
    $languageRows = foreach ($lang in $languages) {
        $safeName = New-EscapedCSharpString -Value $lang.Name
        "            new IsoLanguage(`"$($lang.Alpha2Code)`", `"$($lang.Alpha3Code)`", `"$safeName`"),"
    }

    # Remove trailing comma from last line
    if ($languageRows.Count -gt 0) {
        $languageRows[-1] = $languageRows[-1].TrimEnd(',')
    }

    $generatedCode = $languageRows -join [Environment]::NewLine

    Write-Verbose "Loading template from: $TemplatePath"
    $template = Get-Content -Path $TemplatePath -Raw -ErrorAction Stop

    if ($template -notmatch '<< LANGUAGE_ROWS >>') {
        throw "Template file does not contain '<< LANGUAGE_ROWS >>' placeholder: $TemplatePath"
    }

    $output = $template -replace '<< LANGUAGE_ROWS >>', $generatedCode

    Write-Verbose "Writing output to: $OutputPath"
    $outputDir = Split-Path -Path $OutputPath -Parent
    if ($outputDir -and -not (Test-Path $outputDir)) {
        New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    }

    $output | Set-Content -Path $OutputPath -Encoding UTF8 -NoNewline -ErrorAction Stop

    Write-Host "✅ Generated: $OutputPath" -ForegroundColor Green
    Write-Host "Languages: $($languages.Count)" -ForegroundColor Cyan
}
catch {
    Write-Error "Failed to generate ISO language data: $_"
    exit 1
}
