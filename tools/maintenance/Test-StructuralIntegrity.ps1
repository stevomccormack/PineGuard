<#
.SYNOPSIS
    Verifies structural integrity of the PineGuard codebase after folder/namespace moves.

.DESCRIPTION
    Runs a series of checks to catch regressions after structural changes:
    - Build compilation (dotnet build)
    - Test execution (dotnet test)
    - Stale path references in .md files
    - Stale path references in .ps1 files
    - Stale namespace references in .cs files
    - Sonar path validation (hardcoded paths exist on disk)
    - Namespace/folder alignment (namespace matches folder path)

.PARAMETER Scope
    Which checks to run: All, Build, Test, Paths, Namespaces, Sonar.
    Default: All.

.PARAMETER StalePaths
    Array of old path patterns to flag as stale.
    These patterns should NOT appear in .md or .ps1 files after a move.

.PARAMETER StaleNamespaces
    Array of old namespace patterns to flag as stale.
    These should NOT appear in .cs files after a move.

.PARAMETER SkipBuild
    Skip the dotnet build check.

.PARAMETER SkipTest
    Skip the dotnet test check.

.EXAMPLE
    # Quick build-only check:
    .\Test-StructuralIntegrity.ps1 -Scope Build
#>

[CmdletBinding()]
param(
    [ValidateSet('All', 'Build', 'Test', 'Paths', 'Namespaces', 'Sonar')]
    [string]$Scope = 'All',

    [string[]]$StalePaths = @(),

    [string[]]$StaleNamespaces = @(),

    [switch]$SkipBuild,

    [switch]$SkipTest
)

$ErrorActionPreference = 'Continue'
$repoRoot = (git -C $PSScriptRoot rev-parse --show-toplevel 2>$null)
if (-not $repoRoot) {
    $repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
}

$artifactDir = Join-Path $repoRoot 'artifacts/audit'
if (-not (Test-Path $artifactDir)) { New-Item -ItemType Directory -Path $artifactDir -Force | Out-Null }

$reportPath = Join-Path $artifactDir 'structural-integrity.txt'
$totalIssues = 0
$results = [System.Collections.Generic.List[string]]::new()

function Write-Check([string]$Name) {
    $divider = '=' * 60
    $results.Add('')
    $results.Add($divider)
    $results.Add("CHECK: $Name")
    $results.Add($divider)
    Write-Host "`n$divider" -ForegroundColor Cyan
    Write-Host "CHECK: $Name" -ForegroundColor Cyan
    Write-Host $divider -ForegroundColor Cyan
}

function Write-Pass([string]$Message) {
    $line = "  PASS  $Message"
    $results.Add($line)
    Write-Host $line -ForegroundColor Green
}

function Write-Fail([string]$Message) {
    $script:totalIssues++
    $line = "  FAIL  $Message"
    $results.Add($line)
    Write-Host $line -ForegroundColor Red
}

function Write-Info([string]$Message) {
    $line = "  INFO  $Message"
    $results.Add($line)
    Write-Host $line -ForegroundColor Gray
}

# ─────────────────────────────────────────────
# CHECK 1: Build
# ─────────────────────────────────────────────
if ($Scope -in 'All', 'Build' -and -not $SkipBuild) {
    Write-Check 'dotnet build'

    $slnx = Join-Path $repoRoot 'PineGuard.slnx'
    if (-not (Test-Path $slnx)) {
        $slnx = Get-ChildItem -Path $repoRoot -Filter '*.sln' -File | Select-Object -First 1 -ExpandProperty FullName
    }

    $buildOutput = & dotnet build $slnx --no-restore --verbosity quiet 2>&1
    $buildExitCode = $LASTEXITCODE

    if ($buildExitCode -eq 0) {
        Write-Pass 'Solution builds successfully (0 errors)'
    }
    else {
        Write-Fail "Build FAILED (exit code $buildExitCode)"
        $buildOutput | Where-Object { $_ -match 'error ' } | Select-Object -First 10 | ForEach-Object {
            Write-Info "  $_"
        }
    }
}

# ─────────────────────────────────────────────
# CHECK 2: Tests
# ─────────────────────────────────────────────
if ($Scope -in 'All', 'Test' -and -not $SkipTest) {
    Write-Check 'dotnet test'

    $slnx = Join-Path $repoRoot 'PineGuard.slnx'
    if (-not (Test-Path $slnx)) {
        $slnx = Get-ChildItem -Path $repoRoot -Filter '*.sln' -File | Select-Object -First 1 -ExpandProperty FullName
    }

    $testOutput = & dotnet test $slnx --no-build --verbosity quiet 2>&1
    $testExitCode = $LASTEXITCODE

    if ($testExitCode -eq 0) {
        Write-Pass 'All tests pass'
    }
    else {
        Write-Fail "Tests FAILED (exit code $testExitCode)"
        $testOutput | Where-Object { $_ -match 'Failed|Error' } | Select-Object -First 10 | ForEach-Object {
            Write-Info "  $_"
        }
    }
}

# ─────────────────────────────────────────────
# CHECK 3: Stale path references in .md and .ps1
# ─────────────────────────────────────────────
if ($Scope -in 'All', 'Paths' -and $StalePaths.Count -gt 0) {
    Write-Check 'Stale path references (.md + .ps1)'

    $mdFiles = Get-ChildItem -Path $repoRoot -Recurse -Include '*.md' -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|node_modules|\.git)[\\/]' }

    $ps1Files = Get-ChildItem -Path $repoRoot -Recurse -Include '*.ps1' -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|node_modules|\.git)[\\/]' }

    $staleFound = 0
    foreach ($pattern in $StalePaths) {
        # Normalise slashes for matching
        $fwdPattern = $pattern.Replace('\', '/')
        $bkPattern = $pattern.Replace('/', '\')

        foreach ($file in ($mdFiles + $ps1Files)) {
            $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
            if (-not $content) { continue }

            if ($content -match [regex]::Escape($fwdPattern) -or $content -match [regex]::Escape($bkPattern)) {
                $relFile = [System.IO.Path]::GetRelativePath($repoRoot, $file.FullName)
                Write-Fail "Stale path '$pattern' found in: $relFile"
                $staleFound++
            }
        }
    }

    if ($staleFound -eq 0) {
        Write-Pass "No stale path references found ($($StalePaths.Count) patterns checked)"
    }
}

# ─────────────────────────────────────────────
# CHECK 4: Stale namespace references in .cs
# ─────────────────────────────────────────────
if ($Scope -in 'All', 'Namespaces' -and $StaleNamespaces.Count -gt 0) {
    Write-Check 'Stale namespace references (.cs)'

    $csFiles = Get-ChildItem -Path $repoRoot -Recurse -Include '*.cs' -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|\.git)[\\/]' }

    $staleFound = 0
    foreach ($ns in $StaleNamespaces) {
        foreach ($file in $csFiles) {
            $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
            if (-not $content) { continue }

            # Match as namespace declaration or using statement
            if ($content -match "(?m)^(namespace|using)\s+$([regex]::Escape($ns))\b") {
                $relFile = [System.IO.Path]::GetRelativePath($repoRoot, $file.FullName)
                Write-Fail "Stale namespace '$ns' in: $relFile"
                $staleFound++
            }
        }
    }

    if ($staleFound -eq 0) {
        Write-Pass "No stale namespace references found ($($StaleNamespaces.Count) patterns checked)"
    }
}

# ─────────────────────────────────────────────
# CHECK 5: Namespace/folder alignment
# ─────────────────────────────────────────────
if ($Scope -in 'All', 'Namespaces') {
    Write-Check 'Namespace/folder alignment (src/ only)'

    $srcDir = Join-Path $repoRoot 'src'
    $csFiles = Get-ChildItem -Path $srcDir -Recurse -Include '*.cs' -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|Common|Polyfills)[\\/]' -and $_.Name -ne 'GlobalUsings.cs' }

    $misaligned = 0
    foreach ($file in $csFiles) {
        $content = Get-Content $file.FullName -Raw -ErrorAction SilentlyContinue
        if (-not $content) { continue }

        if ($content -match '(?m)^namespace\s+([A-Za-z0-9_.]+)\s*;') {
            $declaredNs = $Matches[1]

            # Derive expected namespace from folder path
            $relPath = [System.IO.Path]::GetRelativePath($srcDir, $file.DirectoryName)
            # e.g. PineGuard.MustClauses\Rules → PineGuard.MustClauses.Rules
            $expectedNs = $relPath.Replace('\', '.').Replace('/', '.')

            if ($declaredNs -ne $expectedNs) {
                $relFile = [System.IO.Path]::GetRelativePath($repoRoot, $file.FullName)
                Write-Info "Misalignment: $relFile — declared '$declaredNs', folder suggests '$expectedNs'"
                $misaligned++
            }
        }
    }

    if ($misaligned -eq 0) {
        Write-Pass 'All namespaces align with folder structure'
    }
    else {
        Write-Info "$misaligned file(s) have namespace/folder misalignment (may be intentional — review above)"
    }
}

# ─────────────────────────────────────────────
# CHECK 6: Sonar hardcoded paths exist on disk
# ─────────────────────────────────────────────
if ($Scope -in 'All', 'Sonar') {
    Write-Check 'Sonar path validation'

    $sonarFile = Join-Path $repoRoot 'sonar-project.properties'
    if (Test-Path $sonarFile) {
        $sonarContent = Get-Content $sonarFile -Raw
        $hardcodedPaths = [regex]::Matches($sonarContent, 'resourceKey=(src/[^\s,]+\.cs)')

        $missing = 0
        foreach ($match in $hardcodedPaths) {
            $filePath = $match.Groups[1].Value
            $fullPath = Join-Path $repoRoot $filePath
            if (-not (Test-Path $fullPath)) {
                Write-Fail "Sonar references missing file: $filePath"
                $missing++
            }
        }

        if ($missing -eq 0 -and $hardcodedPaths.Count -gt 0) {
            Write-Pass "All $($hardcodedPaths.Count) sonar hardcoded paths exist on disk"
        }
        elseif ($hardcodedPaths.Count -eq 0) {
            Write-Info 'No hardcoded sonar paths found to check'
        }
    }
    else {
        Write-Info 'sonar-project.properties not found — skipping'
    }
}

# ─────────────────────────────────────────────
# Summary
# ─────────────────────────────────────────────
$divider = '=' * 60
$results.Add('')
$results.Add($divider)

if ($totalIssues -eq 0) {
    $summary = "ALL CHECKS PASSED"
    $results.Add($summary)
    Write-Host "`n$divider" -ForegroundColor Green
    Write-Host $summary -ForegroundColor Green
    Write-Host $divider -ForegroundColor Green
}
else {
    $summary = "FAILED: $totalIssues issue(s) found"
    $results.Add($summary)
    Write-Host "`n$divider" -ForegroundColor Red
    Write-Host $summary -ForegroundColor Red
    Write-Host $divider -ForegroundColor Red
}

$results.Add($divider)

# Write report
$results | Out-File -FilePath $reportPath -Encoding utf8
Write-Host "`nReport written to: $reportPath" -ForegroundColor Gray

exit $totalIssues
