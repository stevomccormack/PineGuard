Add-Type -AssemblyName System.Drawing
$img = New-Object System.Drawing.Bitmap('d:\Steve McCormack\GitHub\@stevomccormack\PineGuard\docs\brand\pineguard-logo-128px.png')
# 9x9 grid sample to eyeball the image contents
for ($yi = 0; $yi -lt 9; $yi++) {
    $y = [int]($img.Height * $yi / 8)
    if ($y -ge $img.Height) { $y = $img.Height - 1 }
    $row = ""
    for ($xi = 0; $xi -lt 9; $xi++) {
        $x = [int]($img.Width * $xi / 8)
        if ($x -ge $img.Width) { $x = $img.Width - 1 }
        $p = $img.GetPixel($x, $y)
        # Symbolic cell: T=transparent, L=logo (teal), P=partial
        $sym = if ($p.A -eq 0) { "  . " } elseif ($p.A -eq 255) { ("{0,4}" -f $p.B) } else { (" p{0,2}" -f $p.A) }
        $row += $sym
    }
    Write-Host $row
}
$img.Dispose()
