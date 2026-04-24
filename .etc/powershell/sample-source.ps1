Add-Type -AssemblyName System.Drawing
$src = New-Object System.Drawing.Bitmap('d:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\brand\pineguard-logo-512px.png')
# Grid sample: 5x5 evenly-spaced
for ($yi = 0; $yi -lt 5; $yi++) {
    $y = [int](($src.Height - 1) * $yi / 4)
    $row = ""
    for ($xi = 0; $xi -lt 5; $xi++) {
        $x = [int](($src.Width - 1) * $xi / 4)
        $p = $src.GetPixel($x, $y)
        $row += ("({0,3},{1,3}) " -f $p.R, $p.B)
    }
    Write-Host $row
}
$src.Dispose()
