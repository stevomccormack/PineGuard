Add-Type -AssemblyName System.Drawing

$srcPath = 'd:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\brand\pineguard-logo-512px.png'
$dstPath = 'd:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\brand\pineguard-logo-128px.png'
$size = 128

# Resize at high quality (bicubic, 32bpp ARGB destination)
$src = New-Object System.Drawing.Bitmap($srcPath)
$dst = New-Object System.Drawing.Bitmap($size, $size, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($dst)
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
$g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
$g.CompositingQuality = [System.Drawing.Drawing2D.CompositingQuality]::HighQuality
$g.DrawImage($src, 0, 0, $size, $size)
$g.Dispose()
$src.Dispose()

# Chroma-key: strip light-grey/near-white background, preserve logo edges.
# Source has mixed 220-grey and 248-white background pixels; logo interior is deep teal (mean ~60).
# Keying by luminance mean handles both background shades with a single feathered threshold.
$fullBg = 205.0      # pixels with luminance >= this -> fully transparent
$fullFg = 175.0      # pixels with luminance <= this -> fully opaque
for ($y = 0; $y -lt $size; $y++) {
    for ($x = 0; $x -lt $size; $x++) {
        $p = $dst.GetPixel($x, $y)
        $mean = ([double]$p.R + [double]$p.G + [double]$p.B) / 3.0
        if ($mean -ge $fullBg) {
            $dst.SetPixel($x, $y, [System.Drawing.Color]::FromArgb(0, 0, 0, 0))
        }
        elseif ($mean -gt $fullFg) {
            # Feather band: linear fall-off so the logo's anti-aliased edge is preserved smoothly.
            $alpha = [int][Math]::Round(255.0 * ($fullBg - $mean) / ($fullBg - $fullFg))
            $dst.SetPixel($x, $y, [System.Drawing.Color]::FromArgb($alpha, $p.R, $p.G, $p.B))
        }
        # else: fully opaque, leave pixel unchanged
    }
}

$dst.Save($dstPath, [System.Drawing.Imaging.ImageFormat]::Png)
$dst.Dispose()

# Verification
$verify = New-Object System.Drawing.Bitmap($dstPath)
Write-Host "Corners:"
foreach ($c in @(@(0, 0), @(127, 0), @(0, 127), @(127, 127))) {
    $p = $verify.GetPixel($c[0], $c[1])
    Write-Host ("  ({0},{1}) R={2} G={3} B={4} A={5}" -f $c[0], $c[1], $p.R, $p.G, $p.B, $p.A)
}
Write-Host "Centre: (64,64) = $($verify.GetPixel(64, 64).R),$($verify.GetPixel(64, 64).G),$($verify.GetPixel(64, 64).B),$($verify.GetPixel(64, 64).A)"

# Alpha histogram: how many fully transparent, fully opaque, partial
$t = 0; $o = 0; $p = 0
for ($y = 0; $y -lt 128; $y++) {
    for ($x = 0; $x -lt 128; $x++) {
        $a = $verify.GetPixel($x, $y).A
        if ($a -eq 0) { $t++ } elseif ($a -eq 255) { $o++ } else { $p++ }
    }
}
Write-Host ("Alpha: {0} transparent, {1} opaque, {2} partial (of {3})" -f $t, $o, $p, ($size * $size))
$verify.Dispose()
$info = Get-Item $dstPath
Write-Host ("File size: {0:N0} bytes" -f $info.Length)
