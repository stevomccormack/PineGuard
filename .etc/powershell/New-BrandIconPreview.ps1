<#
.SYNOPSIS
    Composite the PineGuard icon onto a solid background, to check it on a hostile colour.

.DESCRIPTION
    The package icon is transparent, so how it reads depends entirely on what sits behind it.
    This renders the icon on a flat background with padding and writes a PNG, which is the
    quickest way to confirm the chroma-key in New-BrandIcon.ps1 left no grey halo before the icon
    ships to nuget.org.

    Nearest-neighbour resampling is deliberate: the icon is composited at its native size, so any
    smoothing here would hide exactly the edge artefacts this preview exists to reveal.

    Output goes to artifacts/brand/, never beside the script. The previous version wrote
    icon-on-yellow.png into .etc/powershell/ and that file was committed - a generated artefact in
    a source folder, which the tools spec's output rules forbid.

.PARAMETER IconPath
    Icon to composite. Relative paths resolve against the repository root.
    Default: docs/brand/pineguard-logo-128px.png.

.PARAMETER BackgroundColor
    Background to composite onto, as #RRGGBB or a .NET colour name. Default: #FFDD00 (vivid
    yellow) - a deliberately unforgiving choice, since a grey halo is most visible against it.

.PARAMETER Padding
    Pixels of background around the icon on each side. Default: 16.

.PARAMETER Open
    Open the generated preview in the default image viewer.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-BrandIconPreview.ps1 -Open

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-BrandIconPreview.ps1 -BackgroundColor '#1E1E1E'
    Checks the icon against a dark listing background instead.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()]
    [string] $IconPath = 'docs/brand/pineguard-logo-128px.png',

    [ValidateNotNullOrEmpty()]
    [string] $BackgroundColor = '#FFDD00',

    [ValidateRange(0, 256)]
    [int] $Padding = 16,

    [switch] $Open
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '../../tools/.shared/path.ps1')
. (Join-Path $PSScriptRoot '../../tools/.shared/console.ps1')

if (-not $IsWindows) {
    Write-Fail 'System.Drawing.Common bitmap access is Windows-only; this script cannot run here.'
    exit 1
}

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$iconFullPath = if ([System.IO.Path]::IsPathRooted($IconPath)) { $IconPath } else { Join-Path $repoRoot $IconPath }

if (-not (Test-Path -LiteralPath $iconFullPath)) {
    Write-Fail "Icon not found: $iconFullPath. Run New-BrandIcon.ps1 -EnableUpdateTarget first."
    exit 1
}

Add-Type -AssemblyName System.Drawing

try {
    $background = [System.Drawing.ColorTranslator]::FromHtml($BackgroundColor)
}
catch {
    Write-Fail "-BackgroundColor '$BackgroundColor' is not a valid colour. Use #RRGGBB or a .NET colour name."
    exit 1
}

$slug = ($BackgroundColor -replace '[^0-9A-Za-z]', '').ToLowerInvariant()
$outputDirectory = Join-Path $repoRoot 'artifacts/brand'
$outputPath = Join-Path $outputDirectory "icon-on-$slug.png"
$null = New-Item -ItemType Directory -Path $outputDirectory -Force

$icon = $null
$canvas = $null
$graphics = $null
try {
    $icon = [System.Drawing.Bitmap]::new($iconFullPath)
    $canvasWidth = $icon.Width + ($Padding * 2)
    $canvasHeight = $icon.Height + ($Padding * 2)

    Write-Step "Compositing $(Split-Path -Leaf $iconFullPath) on $BackgroundColor"
    Write-Detail ("Icon:   {0}x{1}" -f $icon.Width, $icon.Height)
    Write-Detail ("Canvas: {0}x{1} ({2}px padding)" -f $canvasWidth, $canvasHeight, $Padding)

    $canvas = [System.Drawing.Bitmap]::new($canvasWidth, $canvasHeight, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $graphics = [System.Drawing.Graphics]::FromImage($canvas)
    $graphics.Clear($background)
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
    $graphics.DrawImage($icon, $Padding, $Padding, $icon.Width, $icon.Height)
    $graphics.Dispose()
    $graphics = $null

    $canvas.Save($outputPath, [System.Drawing.Imaging.ImageFormat]::Png)
}
finally {
    if ($graphics) { $graphics.Dispose() }
    if ($canvas) { $canvas.Dispose() }
    if ($icon) { $icon.Dispose() }
}

$written = Get-Item -LiteralPath $outputPath
Write-Success ('{0} written ({1:N0} bytes)' -f $outputPath, $written.Length)

if ($Open) {
    Start-Process -FilePath $outputPath
}
