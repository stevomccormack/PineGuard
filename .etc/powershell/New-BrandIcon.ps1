<#
.SYNOPSIS
    Generate the transparent PineGuard package icon from the master brand artwork.

.DESCRIPTION
    Downscales docs/brand/pineguard-logo-512px.png to the requested size with high-quality
    bicubic resampling, then chroma-keys the master's opaque grey/white backdrop away so the
    result sits cleanly on nuget.org's light and dark listing backgrounds.

    Chroma-key: the master mixes 220-grey and 248-white background pixels while the logo interior
    is deep teal (luminance mean around 60), so a single feathered luminance threshold handles
    both backdrop shades. Pixels at or above -BackgroundLuminance become fully transparent, pixels
    at or below -ForegroundLuminance stay fully opaque, and the band between them gets a linear
    alpha fall-off that preserves the artwork's anti-aliased edge.

    Preview-first, per the tools spec: by default the icon is written to artifacts/brand/ and
    docs/brand/ is left untouched. Pass -EnableUpdateTarget to overwrite the committed icon.

.PARAMETER SourcePath
    Master artwork to downscale. Relative paths resolve against the repository root.
    Default: docs/brand/pineguard-logo-512px.png.

.PARAMETER Size
    Output edge length in pixels; the icon is square. Default: 128.

.PARAMETER BackgroundLuminance
    Mean RGB at or above which a pixel is treated as backdrop and made fully transparent.
    Default: 205.

.PARAMETER ForegroundLuminance
    Mean RGB at or below which a pixel is treated as artwork and left fully opaque.
    Default: 175. Must be below -BackgroundLuminance; the gap is the feather band.

.PARAMETER EnableUpdateTarget
    Write to docs/brand/pineguard-logo-<Size>px.png instead of the artifacts/ preview location.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-BrandIcon.ps1
    Writes a preview to artifacts/brand/pineguard-logo-128px.png and reports the alpha histogram.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-BrandIcon.ps1 -EnableUpdateTarget
    Regenerates the committed docs/brand/pineguard-logo-128px.png.
#>

[CmdletBinding(SupportsShouldProcess)]
param(
    [ValidateNotNullOrEmpty()]
    [string] $SourcePath = 'docs/brand/pineguard-logo-512px.png',

    [ValidateRange(16, 1024)]
    [int] $Size = 128,

    [ValidateRange(0, 255)]
    [double] $BackgroundLuminance = 205,

    [ValidateRange(0, 255)]
    [double] $ForegroundLuminance = 175,

    [switch] $EnableUpdateTarget
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '../../tools/.shared/path.ps1')
. (Join-Path $PSScriptRoot '../../tools/.shared/console.ps1')

if (-not $IsWindows) {
    Write-Fail 'System.Drawing.Common bitmap access is Windows-only; this script cannot run here.'
    exit 1
}

if ($ForegroundLuminance -ge $BackgroundLuminance) {
    Write-Fail "-ForegroundLuminance ($ForegroundLuminance) must be below -BackgroundLuminance ($BackgroundLuminance); the gap between them is the feather band."
    exit 1
}

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$sourceFullPath = if ([System.IO.Path]::IsPathRooted($SourcePath)) { $SourcePath } else { Join-Path $repoRoot $SourcePath }

if (-not (Test-Path -LiteralPath $sourceFullPath)) {
    Write-Fail "Master artwork not found: $sourceFullPath"
    exit 1
}

$fileName = "pineguard-logo-${Size}px.png"
$outputDirectory = if ($EnableUpdateTarget) { Join-Path $repoRoot 'docs/brand' } else { Join-Path $repoRoot 'artifacts/brand' }
$outputPath = Join-Path $outputDirectory $fileName

Write-Step "Generating $fileName"
Write-Detail "Source: $sourceFullPath"
Write-Detail "Target: $outputPath"
Write-Detail ("Key:    transparent >= {0}, opaque <= {1}" -f $BackgroundLuminance, $ForegroundLuminance)

if (-not $EnableUpdateTarget) {
    Write-Warn 'Preview only — pass -EnableUpdateTarget to write into docs/brand/.'
}

if (-not $PSCmdlet.ShouldProcess($outputPath, 'Write icon')) {
    exit 0
}

Add-Type -AssemblyName System.Drawing

$null = New-Item -ItemType Directory -Path $outputDirectory -Force

$source = $null
$target = $null
$graphics = $null
try {
    $source = [System.Drawing.Bitmap]::new($sourceFullPath)
    $target = [System.Drawing.Bitmap]::new($Size, $Size, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)

    $graphics = [System.Drawing.Graphics]::FromImage($target)
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
    $graphics.DrawImage($source, 0, 0, $Size, $Size)
    $graphics.Dispose()
    $graphics = $null

    $featherRange = $BackgroundLuminance - $ForegroundLuminance
    $transparentCount = 0
    $opaqueCount = 0
    $featheredCount = 0

    for ($y = 0; $y -lt $Size; $y++) {
        for ($x = 0; $x -lt $Size; $x++) {
            $pixel = $target.GetPixel($x, $y)
            $luminanceMean = ([double]$pixel.R + [double]$pixel.G + [double]$pixel.B) / 3.0

            if ($luminanceMean -ge $BackgroundLuminance) {
                $target.SetPixel($x, $y, [System.Drawing.Color]::FromArgb(0, 0, 0, 0))
                $transparentCount++
            }
            elseif ($luminanceMean -gt $ForegroundLuminance) {
                $alpha = [int][Math]::Round(255.0 * ($BackgroundLuminance - $luminanceMean) / $featherRange)
                $target.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($alpha, $pixel.R, $pixel.G, $pixel.B))
                $featheredCount++
            }
            else {
                $opaqueCount++
            }
        }
    }

    $target.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
}
finally {
    if ($graphics) { $graphics.Dispose() }
    if ($target) { $target.Dispose() }
    if ($source) { $source.Dispose() }
}

# --- Verification -------------------------------------------------------------------------------

$verify = [System.Drawing.Bitmap]::new($outputPath)
try {
    $edge = $Size - 1
    $corners = @(@(0, 0), @($edge, 0), @(0, $edge), @($edge, $edge))
    Write-Detail 'Corners (expected fully transparent):'
    foreach ($corner in $corners) {
        $pixel = $verify.GetPixel($corner[0], $corner[1])
        Write-Detail ('  ({0},{1}) R={2} G={3} B={4} A={5}' -f $corner[0], $corner[1], $pixel.R, $pixel.G, $pixel.B, $pixel.A)
    }

    $centre = $verify.GetPixel([int]($Size / 2), [int]($Size / 2))
    Write-Detail ('Centre ({0},{0}) R={1} G={2} B={3} A={4}' -f [int]($Size / 2), $centre.R, $centre.G, $centre.B, $centre.A)
    Write-Detail ('Alpha: {0} transparent, {1} opaque, {2} feathered (of {3})' -f `
            $transparentCount, $opaqueCount, $featheredCount, ($Size * $Size))
}
finally {
    $verify.Dispose()
}

$written = Get-Item -LiteralPath $outputPath
Write-Success ('{0} written ({1:N0} bytes)' -f $outputPath, $written.Length)
