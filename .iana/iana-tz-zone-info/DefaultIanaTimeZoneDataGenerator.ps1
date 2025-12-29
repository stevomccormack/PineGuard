[CmdletBinding()]
param (
    # Path to zone1970.tab (IANA tzdb)
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [ValidateScript({
        if (-not (Test-Path $_)) {
            throw "Input file not found: $_"
        }
        $true
    })]
    [string]$Zone1970TabPath,

    # Path to the template file used for generating the output C# code.
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
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [string]$OutputPath
)

$ErrorActionPreference = "Stop"

function New-EscapedCSharpString {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory)][AllowEmptyString()][string]$Value
    )

    return $Value.Replace('\\', '\\\\').Replace('"', '\\"')
}

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
        Select-Object -First 10

    if ($dupes) {
        $sample = ($dupes | ForEach-Object { "$($_.Name) x$($_.Count)" }) -join ", "
        throw "Duplicate $KeyName detected. Examples: $sample"
    }
}

function Assert-CountryAlpha2 {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][ValidateNotNullOrEmpty()][string]$Value
    )

    $v = $Value.Trim().ToUpperInvariant()
    if ($v -notmatch '^[A-Z]{2}$') {
        throw "Invalid ISO alpha-2 country code in zone1970.tab: '$Value'"
    }

    return $v
}

# Read file (UTF-8)
$lines = Get-Content -Path $Zone1970TabPath -Encoding UTF8

$zones = New-Object System.Collections.Generic.List[object]

foreach ($line in $lines) {
    if ([string]::IsNullOrWhiteSpace($line)) { continue }
    if ($line.StartsWith('#')) { continue }

    # Columns are separated by a single tab (per tzdb docs).
    $parts = $line -split "`t"

    if ($parts.Count -lt 3) {
        throw "Invalid zone1970.tab line (expected >= 3 columns): $line"
    }

    $countryCodesRaw = $parts[0]
    $coordinatesRaw = $parts[1]
    $zoneIdRaw      = $parts[2]
    $commentRaw     = $null

    if ($parts.Count -ge 4) {
        $commentRaw = $parts[3]
    }

    $zoneId = $zoneIdRaw.Trim()
    if ([string]::IsNullOrWhiteSpace($zoneId)) {
        throw "Invalid zone id in zone1970.tab: '$zoneIdRaw'"
    }

    $coordinates = $coordinatesRaw.Trim()
    if ([string]::IsNullOrWhiteSpace($coordinates)) {
        throw "Invalid coordinates in zone1970.tab for '$zoneId': '$coordinatesRaw'"
    }

    $countryCodes = $countryCodesRaw -split ',' | ForEach-Object { Assert-CountryAlpha2 -Value $_ }

    if ($countryCodes.Count -lt 1) {
        throw "No country codes parsed for '$zoneId'"
    }

    $comment = $null
    if ($commentRaw -ne $null) {
        $c = $commentRaw.Trim()
        if (-not [string]::IsNullOrWhiteSpace($c)) {
            $comment = $c
        }
    }

    $zones.Add([pscustomobject]@{
        ZoneId = $zoneId
        Coordinates = $coordinates
        CountryCodes = $countryCodes
        Comment = $comment
    })
}

# Deterministic order by ZoneId
$zones = $zones | Sort-Object -Property ZoneId

Assert-Unique -KeyName "ZoneId" -Items ($zones | ForEach-Object { $_.ZoneId })

# Generate C# rows
$zoneRows = foreach ($z in $zones) {
    $id = New-EscapedCSharpString -Value $z.ZoneId
    $coords = New-EscapedCSharpString -Value $z.Coordinates

    $countries = $z.CountryCodes | ForEach-Object { New-EscapedCSharpString -Value $_ }
    $countryArray = "new[] { " + (($countries | ForEach-Object { '"' + $_ + '"' }) -join ", ") + " }"

    if ($z.Comment -ne $null) {
        $comment = New-EscapedCSharpString -Value $z.Comment
        "            new IanaTimeZone(""$id"", $countryArray, ""$coords"", ""$comment""),"
    }
    else {
        "            new IanaTimeZone(""$id"", $countryArray, ""$coords"", null),"
    }
}

# Remove trailing comma from last line (matches style used in ISO generators)
if ($zoneRows.Count -gt 0) {
    $zoneRows[-1] = $zoneRows[-1].TrimEnd(',')
}

$template = Get-Content -Path $TemplatePath -Raw -Encoding UTF8

if (-not $template.Contains('<< TIME_ZONE_ROWS >>')) {
    throw "Template does not contain required placeholder '<< TIME_ZONE_ROWS >>': $TemplatePath"
}

$output = $template.Replace("<< TIME_ZONE_ROWS >>", ($zoneRows -join [Environment]::NewLine))

# Write output
$outDir = Split-Path -Path $OutputPath -Parent
if (-not [string]::IsNullOrWhiteSpace($outDir) -and -not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$output | Set-Content -Path $OutputPath -Encoding UTF8 -NoNewline

Write-Host "Generated: $OutputPath" -ForegroundColor Green
