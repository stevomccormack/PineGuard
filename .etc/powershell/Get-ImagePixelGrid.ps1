<#
.SYNOPSIS
    Sample an image on an evenly-spaced grid and print the pixels, to eyeball its contents.

.DESCRIPTION
    A read-only diagnostic used when tuning New-BrandIcon.ps1's chroma-key: it answers "is the
    background actually transparent, and did the logo survive?" without opening an image editor.

    Replaces the two near-identical throwaway scripts sample-source.ps1 (5x5 RGB grid over the
    512px master) and sample-dst.ps1 (9x9 alpha grid over the 128px output), which differed only
    in path, grid size and cell format - all three of which are parameters here.

    Grid lines are written to the pipeline, not the host, so the output can be captured,
    redirected or diffed between two runs.

.PARAMETER Path
    Image to sample. Relative paths resolve against the repository root.

.PARAMETER GridSize
    Number of sample points per axis, spread evenly from edge to edge. Default: 9.

.PARAMETER Format
    Alpha - transparent cells print '.', opaque cells print the blue channel, partially
            transparent cells print 'p<alpha>'. Best for checking a chroma-keyed result.
    Rgb   - every cell prints '(R,B)'. Best for inspecting an un-keyed source.
    Default: Alpha.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Get-ImagePixelGrid.ps1 -Path docs/brand/pineguard-logo-128px.png
    The former sample-dst.ps1: a 9x9 alpha grid over the generated icon.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Get-ImagePixelGrid.ps1 -Path docs/brand/pineguard-logo-512px.png -GridSize 5 -Format Rgb
    The former sample-source.ps1: a 5x5 RGB grid over the master artwork.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateNotNullOrEmpty()]
    [string] $Path,

    [ValidateRange(2, 64)]
    [int] $GridSize = 9,

    [ValidateSet('Alpha', 'Rgb')]
    [string] $Format = 'Alpha'
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
$imagePath = if ([System.IO.Path]::IsPathRooted($Path)) { $Path } else { Join-Path $repoRoot $Path }

if (-not (Test-Path -LiteralPath $imagePath)) {
    Write-Fail "Image not found: $imagePath"
    exit 1
}

Add-Type -AssemblyName System.Drawing

$image = [System.Drawing.Bitmap]::new($imagePath)
try {
    Write-Step ("{0} ({1}x{2}, {3} grid, {4} format)" -f `
        (Split-Path -Leaf $imagePath), $image.Width, $image.Height, "${GridSize}x${GridSize}", $Format)

    $lastIndex = $GridSize - 1
    foreach ($rowIndex in 0..$lastIndex) {
        $y = [int](($image.Height - 1) * $rowIndex / $lastIndex)
        $cells = foreach ($columnIndex in 0..$lastIndex) {
            $x = [int](($image.Width - 1) * $columnIndex / $lastIndex)
            $pixel = $image.GetPixel($x, $y)

            if ($Format -eq 'Rgb') {
                '({0,3},{1,3})' -f $pixel.R, $pixel.B
            }
            elseif ($pixel.A -eq 0) { '   .' }
            elseif ($pixel.A -eq 255) { '{0,4}' -f $pixel.B }
            else { ' p{0,2}' -f $pixel.A }
        }

        $cells -join ' '
    }
}
finally {
    $image.Dispose()
}
