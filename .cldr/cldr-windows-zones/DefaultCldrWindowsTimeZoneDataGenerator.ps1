[CmdletBinding()]
param (
    # Path to windowsZones.xml (CLDR supplemental)
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [ValidateScript({
        if (-not (Test-Path $_)) {
            throw "Input file not found: $_"
        }
        $true
    })]
    [string]$WindowsZonesXmlPath,

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

function Assert-UniquePair {
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

# Load XML
$raw = Get-Content -Path $WindowsZonesXmlPath -Raw -Encoding UTF8
[xml]$doc = $raw

$mapZones = @($doc.supplementalData.windowsZones.mapTimezones.mapZone)
if (-not $mapZones -or $mapZones.Count -lt 1) {
    throw "No mapZone elements found in: $WindowsZonesXmlPath"
}

$mappings = New-Object System.Collections.Generic.List[object]

foreach ($mz in $mapZones) {
    $windowsId = [string]$mz.other
    $territory = [string]$mz.territory
    $type = [string]$mz.type

    if ([string]::IsNullOrWhiteSpace($windowsId)) {
        throw "mapZone missing 'other' attribute (Windows ID)."
    }
    if ([string]::IsNullOrWhiteSpace($territory)) {
        throw "mapZone missing 'territory' attribute for Windows ID '$windowsId'."
    }
    if ([string]::IsNullOrWhiteSpace($type)) {
        throw "mapZone missing 'type' attribute for Windows ID '$windowsId' territory '$territory'."
    }

    $ianaIds = $type -split '\s+' | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    if ($ianaIds.Count -lt 1) {
        throw "No IANA IDs parsed from 'type' for Windows ID '$windowsId' territory '$territory'."
    }

    $mappings.Add([pscustomobject]@{
        WindowsId = $windowsId.Trim()
        Territory = $territory.Trim()
        IanaIds = $ianaIds
    })
}

# Deterministic order
$mappings = $mappings | Sort-Object -Property WindowsId, Territory

# Uniqueness: (WindowsId, Territory)
Assert-UniquePair -KeyName "(WindowsId, Territory)" -Items ($mappings | ForEach-Object { "$($_.WindowsId)|$($_.Territory)" })

$rows = foreach ($m in $mappings) {
    $w = New-EscapedCSharpString -Value $m.WindowsId
    $t = New-EscapedCSharpString -Value $m.Territory
    $ids = $m.IanaIds | ForEach-Object { New-EscapedCSharpString -Value $_ }

    $ianaArray = "new[] { " + (($ids | ForEach-Object { '"' + $_ + '"' }) -join ", ") + " }"

    "            new CldrWindowsTimeZoneMapping(""$w"", ""$t"", $ianaArray),"
}

# Remove trailing comma from last line
if ($rows.Count -gt 0) {
    $rows[-1] = $rows[-1].TrimEnd(',')
}

$template = Get-Content -Path $TemplatePath -Raw -Encoding UTF8
if (-not $template.Contains('<< WINDOWS_ZONE_ROWS >>')) {
    throw "Template does not contain required placeholder '<< WINDOWS_ZONE_ROWS >>': $TemplatePath"
}

$output = $template.Replace("<< WINDOWS_ZONE_ROWS >>", ($rows -join [Environment]::NewLine))

$outDir = Split-Path -Path $OutputPath -Parent
if (-not [string]::IsNullOrWhiteSpace($outDir) -and -not (Test-Path $outDir)) {
    New-Item -ItemType Directory -Path $outDir -Force | Out-Null
}

$output | Set-Content -Path $OutputPath -Encoding UTF8 -NoNewline

Write-Host "Generated: $OutputPath" -ForegroundColor Green
