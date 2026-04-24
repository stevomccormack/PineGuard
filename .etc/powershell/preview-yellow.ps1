Add-Type -AssemblyName System.Drawing

$iconPath = 'd:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\brand\pineguard-logo-128px.png'
$outPath = 'd:\Steve McCormack\GitHub\@stevomccormack\PineGuard\.etc\powershell\icon-on-yellow.png'
$size = 128
$pad = 16
$canvas = $size + ($pad * 2)

$yellow = [System.Drawing.Color]::FromArgb(255, 255, 221, 0)  # vivid yellow

$icon = New-Object System.Drawing.Bitmap($iconPath)
$bmp = New-Object System.Drawing.Bitmap($canvas, $canvas, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear($yellow)
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::NearestNeighbor
$g.DrawImage($icon, $pad, $pad, $size, $size)
$g.Dispose()
$bmp.Save($outPath, [System.Drawing.Imaging.ImageFormat]::Png)
$bmp.Dispose()
$icon.Dispose()

$info = Get-Item $outPath
Write-Host ("Preview written: {0} ({1:N0} bytes, {2}x{2})" -f $outPath, $info.Length, $canvas)
