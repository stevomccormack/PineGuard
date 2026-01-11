[CmdletBinding()]
param(
    # Number of lowest-covered classes to show
    [ValidateRange(1, 500)]
    [int] $Top = 30,

    # Regex used to include files from coverage (matches Cobertura class filename values)
    [string] $IncludeFileRegex = '^src[\\/]+PineGuard\.Core[\\/]+',

    # Regex used to exclude files from coverage (matches Cobertura class filename values)
    [string] $ExcludeFileRegex = '^src[\\/]+PineGuard\.Core[\\/]obj[\\/]+',

    # If set, opens the generated HTML report (index.html) after printing the summary.
    [switch] $OpenHtml,

    # If set, exits non-zero when line/branch coverage are not 100% for the filtered scope.
    [switch] $Enforce100

    # If set, uses a formatted table (may truncate long names/paths depending on console width).
    ,
    [switch] $AsTable
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

function Get-RepoRoot {
    $start = Split-Path -Parent $PSScriptRoot

    $dir = $start
    while ($true) {
        if (Test-Path (Join-Path $dir 'PineGuard.slnx')) {
            return $dir
        }

        $parent = Split-Path -Parent $dir
        if ([string]::IsNullOrWhiteSpace($parent) -or $parent -eq $dir) {
            throw "Could not locate repo root (expected to find PineGuard.slnx). Started at: $start"
        }

        $dir = $parent
    }
}

function Get-LatestCoverageFiles {
    param(
        [Parameter(Mandatory)]
        [string] $ResultsRoot
    )

    $files = Get-ChildItem -LiteralPath $ResultsRoot -Recurse -File -Filter 'coverage.cobertura.xml' -ErrorAction Stop
    if (-not $files -or $files.Count -eq 0) {
        throw "No 'coverage.cobertura.xml' files found under: $ResultsRoot"
    }

    # Prefer the newest file per test-project folder: testresults/<ProjectName>/<guid>/coverage.cobertura.xml
    $files |
        Group-Object { Split-Path -Parent (Split-Path -Parent $_.FullName) } |
        ForEach-Object {
            $_.Group | Sort-Object LastWriteTime -Descending | Select-Object -First 1
        } |
        Sort-Object FullName
}

function Try-ParseConditionCoverage {
    param(
        [Parameter(Mandatory)]
        [string] $ConditionCoverage
    )

    # Example: "50% (1/2)"
    if ($ConditionCoverage -match '\((\d+)\/(\d+)\)') {
        return @([int]$Matches[1], [int]$Matches[2])
    }

    return $null
}

$repoRoot = Get-RepoRoot
$resultsRoot = Join-Path $repoRoot 'etc/artifacts/code-coverage/xplat/testresults'

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host "Coverage results: $resultsRoot" -ForegroundColor DarkGray

$coverageFiles = Get-LatestCoverageFiles -ResultsRoot $resultsRoot

$classes = New-Object System.Collections.Generic.List[object]

[int]$totalLines = 0
[int]$coveredLines = 0
[int]$totalBranches = 0
[int]$coveredBranches = 0

foreach ($coverageFile in $coverageFiles) {
    [xml]$doc = Get-Content -Raw -LiteralPath $coverageFile.FullName

    foreach ($pkg in $doc.coverage.packages.package) {
        foreach ($cls in $pkg.classes.class) {
            $filename = [string]$cls.filename

            if ([string]::IsNullOrWhiteSpace($filename)) {
                continue
            }

            # Cobertura class filenames may be repo-relative (preferred) or absolute.
            # Normalize to repo-relative where possible so include/exclude regexes are stable.
            # Normalize separators to Windows-style so prefix checks work even when Cobertura emits '/'.
            $matchFilename = ($filename -replace '/', '\\')

            $repoRootNormalized = $repoRoot.TrimEnd([char]'\', [char]'/')
            $repoRootWithSep = $repoRootNormalized + '\\'

            if ($matchFilename.StartsWith($repoRootWithSep, [System.StringComparison]::OrdinalIgnoreCase)) {
                $matchFilename = $matchFilename.Substring($repoRootWithSep.Length)
            }
            else {
                # Some Cobertura outputs omit the drive letter on Windows.
                if ($repoRootNormalized.Length -ge 3 -and $repoRootNormalized[1] -eq ':' -and $repoRootNormalized[2] -eq '\\') {
                    $repoRootNoDriveWithSep = $repoRootNormalized.Substring(3)
                    if (-not [string]::IsNullOrWhiteSpace($repoRootNoDriveWithSep)) {
                        $repoRootNoDriveWithSep = $repoRootNoDriveWithSep.TrimStart([char]'\') + '\\'
                        if ($matchFilename.StartsWith($repoRootNoDriveWithSep, [System.StringComparison]::OrdinalIgnoreCase)) {
                            $matchFilename = $matchFilename.Substring($repoRootNoDriveWithSep.Length)
                        }
                    }
                }
            }

            # If Cobertura emitted a path relative to a drive root (e.g., source='D:\' and filename='Steve McCormack\...\src\...'),
            # trim everything before the first 'src\' or 'tests\' segment so include/exclude regexes remain stable.
            $matchFilename = [regex]::Replace(
                $matchFilename,
                '^(?i).*?[\\/](?=(src|tests)[\\/])',
                ''
            )

            # Some Cobertura outputs use filenames relative to the source root (e.g. 'Rules\\Foo.cs').
            # Prefix only when the path is not rooted and not already repo-relative to a known top-level folder.
            if ((-not [System.IO.Path]::IsPathRooted($matchFilename)) -and ($matchFilename -notmatch '^(src|tests)[\\/]+')) {
                $matchFilename = Join-Path 'src\\PineGuard.Core' $matchFilename
            }

            if ($IncludeFileRegex -and ($matchFilename -notmatch $IncludeFileRegex)) {
                continue
            }

            if ($ExcludeFileRegex -and ($matchFilename -match $ExcludeFileRegex)) {
                continue
            }

            $classTotalLines = 0
            $classCoveredLines = 0

            $classTotalBranches = 0
            $classCoveredBranches = 0

            $linesNode = $cls.lines
            if ($null -ne $linesNode -and $null -ne $linesNode.line) {
                foreach ($line in $linesNode.line) {
                    $classTotalLines++

                    $hits = 0
                    [void][int]::TryParse([string]$line.hits, [ref]$hits)
                    if ($hits -gt 0) {
                        $classCoveredLines++
                    }

                    if ($line.branch -eq 'true' -and $line.'condition-coverage') {
                        $parsed = Try-ParseConditionCoverage -ConditionCoverage ([string]$line.'condition-coverage')
                        if ($null -ne $parsed) {
                            $classCoveredBranches += $parsed[0]
                            $classTotalBranches += $parsed[1]
                        }
                    }
                }
            }

            if ($classTotalLines -gt 0) {
                $lr = [double]$classCoveredLines / [double]$classTotalLines
            }
            else {
                $lr = [double]$cls.'line-rate'
            }

            if ($classTotalBranches -gt 0) {
                $br = [double]$classCoveredBranches / [double]$classTotalBranches
            }
            else {
                $br = [double]$cls.'branch-rate'
            }

            $totalLines += $classTotalLines
            $coveredLines += $classCoveredLines
            $totalBranches += $classTotalBranches
            $coveredBranches += $classCoveredBranches

            $classes.Add([pscustomobject]@{
                LineRate       = $lr
                BranchRate     = $br
                LinesCovered   = $classCoveredLines
                LinesTotal     = $classTotalLines
                BranchesCovered = $classCoveredBranches
                BranchesTotal   = $classTotalBranches
                Name           = [string]$cls.name
                File           = $matchFilename
            }) | Out-Null
        }
    }
}

if ($classes.Count -eq 0) {
    throw "No covered classes matched the include/exclude filters. IncludeFileRegex='$IncludeFileRegex' ExcludeFileRegex='$ExcludeFileRegex'"
}

$lineRate = if ($totalLines -gt 0) { [double]$coveredLines / [double]$totalLines } else { 0.0 }
$branchRate = if ($totalBranches -gt 0) { [double]$coveredBranches / [double]$totalBranches } else { 0.0 }

Write-Host ''
Write-Host 'Coverage summary (filtered scope):' -ForegroundColor Cyan
Write-Host ("  Line coverage:   {0:P2} ({1}/{2})" -f $lineRate, $coveredLines, $totalLines)
Write-Host ("  Branch coverage: {0:P2} ({1}/{2})" -f $branchRate, $coveredBranches, $totalBranches)

Write-Host ''
Write-Host ("Lowest-covered classes (Top {0}):" -f $Top) -ForegroundColor Cyan

$bottom = $classes |
    Sort-Object LineRate, BranchRate, Name |
    Select-Object -First $Top LineRate, BranchRate, LinesCovered, LinesTotal, BranchesCovered, BranchesTotal, Name, File

if ($AsTable) {
    $bottom |
        Select-Object @{Name='Line%';Expression={"{0:P2}" -f $_.LineRate}}, @{Name='Branch%';Expression={"{0:P2}" -f $_.BranchRate}}, LinesCovered, LinesTotal, BranchesCovered, BranchesTotal, Name, File |
        Format-Table -AutoSize
}
else {
    Write-Host 'Line%\tBranch%\tLines\tBranches\tClass\tFile'
    foreach ($row in $bottom) {
        $linePct = ("{0:P2}" -f $row.LineRate)
        $branchPct = ("{0:P2}" -f $row.BranchRate)
        $lines = ("{0}/{1}" -f $row.LinesCovered, $row.LinesTotal)
        $branches = ("{0}/{1}" -f $row.BranchesCovered, $row.BranchesTotal)
        Write-Host ("{0}\t{1}\t{2}\t{3}\t{4}\t{5}" -f $linePct, $branchPct, $lines, $branches, $row.Name, $row.File)
    }
}

if ($OpenHtml) {
    $indexPath = Join-Path $repoRoot 'etc/artifacts/code-coverage/xplat/html/index.html'
    if (Test-Path $indexPath) {
        Write-Host ''
        Write-Host "Opening HTML report: $indexPath" -ForegroundColor Cyan
        Start-Process -FilePath $indexPath | Out-Null
    }
    else {
        Write-Warning "HTML report not found at: $indexPath"
    }
}

if ($Enforce100) {
    $lineOk = ($lineRate -ge 1.0)
    $branchOk = ($totalBranches -eq 0) -or ($branchRate -ge 1.0)

    if (-not $lineOk -or -not $branchOk) {
        Write-Error "Coverage is not 100% for the filtered scope. LineRate=$lineRate BranchRate=$branchRate"
        exit 1
    }
}
