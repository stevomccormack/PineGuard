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
    - Namespace/folder alignment (namespace matches folder path), registry-driven
    - Sonar path validation (sonar.*.exclusions entries resolve to real paths on disk)

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

. (Join-Path $PSScriptRoot '..\.shared\path.ps1')
. (Join-Path $PSScriptRoot '..\.shared\dotnet-projects.ps1')

$ErrorActionPreference = 'Continue'
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

$artifactDir = Join-Path $repoRoot 'artifacts/clean'
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
# CHECK 4 + 5: Stale namespace references, and namespace/folder alignment
# ─────────────────────────────────────────────
# These two checks used to each run their own separate Get-ChildItem + Get-Content sweep over
# an overlapping set of .cs files: CHECK 5's src/-scoped file set is always a subset of CHECK 4's
# repo-wide file set. They now share a single enumeration and a single Get-Content -Raw read per
# file. CHECK 5 always runs when Namespaces is in scope; CHECK 4 only runs when the caller passed
# -StaleNamespaces. To avoid forcing a full-repo walk on the common invocation (no -StaleNamespaces,
# CHECK 4 inactive), the scan root narrows to src/ in that case — the full repo-wide walk only
# happens when CHECK 4 is actually going to use it.
if ($Scope -in 'All', 'Namespaces') {
    $runStaleNamespaceCheck = $StaleNamespaces.Count -gt 0
    $scanRoot = if ($runStaleNamespaceCheck) { $repoRoot } else { Join-Path $repoRoot 'src' }

    $csFileEntries = @(
        Get-ChildItem -Path $scanRoot -Recurse -Include '*.cs' -File |
            Where-Object { $_.FullName -notmatch '[\\/](bin|obj|\.git)[\\/]' } |
            ForEach-Object {
                $content = Get-Content $_.FullName -Raw -ErrorAction SilentlyContinue
                if ($null -ne $content) {
                    [pscustomobject]@{ File = $_; Content = $content }
                }
            }
    )

    # ─── CHECK 4: Stale namespace references in .cs ───
    if ($runStaleNamespaceCheck) {
        Write-Check 'Stale namespace references (.cs)'

        $staleFound = 0
        foreach ($ns in $StaleNamespaces) {
            foreach ($entry in $csFileEntries) {
                # Match as namespace declaration or using statement
                if ($entry.Content -match "(?m)^(namespace|using)\s+$([regex]::Escape($ns))\b") {
                    $relFile = [System.IO.Path]::GetRelativePath($repoRoot, $entry.File.FullName)
                    Write-Fail "Stale namespace '$ns' in: $relFile"
                    $staleFound++
                }
            }
        }

        if ($staleFound -eq 0) {
            Write-Pass "No stale namespace references found ($($StaleNamespaces.Count) patterns checked)"
        }
    }

    # ─── CHECK 5: Namespace/folder alignment (registry-driven, src/ scopes only) ───
    Write-Check 'Namespace/folder alignment (src/ only)'

    $srcDir = Join-Path $repoRoot 'src'

    # Registry-driven (F-31 follow-up): the authoritative set of real scope-root folders under
    # src/ comes from the registry (Get-PineGuardScope -All), not a hand-maintained folder-name
    # exclude list — a new scope is picked up automatically the moment it is registered, with
    # nothing here to remember to update.
    #
    # Polyfills/ subfolders are still excluded deliberately, not via the registry: every Polyfills
    # file found in this repo declares a real BCL namespace (System.Runtime.CompilerServices,
    # System.Diagnostics.CodeAnalysis, …) by design — a polyfill shim only works if it uses the
    # exact namespace the compiler expects, so it can never "align" with its folder, and that is
    # a permanent property of what a polyfill is, not something the registry could express.
    #
    # 'Common' is deliberately NOT excluded here, unlike the prior hand-maintained list: checking
    # this repo's actual Common/ folders shows most of them already align with their folder path
    # (src/PineGuard.DataAnnotations/Common -> PineGuard.DataAnnotations.Common,
    # src/PineGuard.FluentValidation/Common -> PineGuard.FluentValidation.Common) and would have
    # passed without an exclude. The one real exception, src/PineGuard.Core/Common (declared as
    # the shared PineGuard.Common namespace, not PineGuard.Core.Common), is a genuine divergence
    # worth surfacing — CHECK 5 only ever reports misalignment as an informational note (it does
    # not fail the run), so letting it show up here is strictly more accurate than hiding it.
    $srcScopeDirs = @(
        Get-PineGuardScope -All |
            Where-Object { $_.SourceDir -match '^src[\\/]' } |
            ForEach-Object { (Join-Path $repoRoot $_.SourceDir) }
    )

    $scopedEntries = @(
        $csFileEntries | Where-Object {
            $dir = $_.File.DirectoryName
            $underScope = $false
            foreach ($scopeDir in $srcScopeDirs) {
                if ($dir.Equals($scopeDir, [System.StringComparison]::OrdinalIgnoreCase) -or
                    $dir.StartsWith("$scopeDir$([System.IO.Path]::DirectorySeparatorChar)", [System.StringComparison]::OrdinalIgnoreCase)) {
                    $underScope = $true
                    break
                }
            }
            $underScope -and $_.File.FullName -notmatch '[\\/]Polyfills[\\/]' -and $_.File.Name -ne 'GlobalUsings.cs'
        }
    )

    $misaligned = 0
    foreach ($entry in $scopedEntries) {
        if ($entry.Content -match '(?m)^namespace\s+([A-Za-z0-9_.]+)\s*;') {
            $declaredNs = $Matches[1]

            # Derive expected namespace from folder path
            $relPath = [System.IO.Path]::GetRelativePath($srcDir, $entry.File.DirectoryName)
            # e.g. PineGuard.MustClauses\Rules → PineGuard.MustClauses.Rules
            $expectedNs = $relPath.Replace('\', '.').Replace('/', '.')

            if ($declaredNs -ne $expectedNs) {
                $relFile = [System.IO.Path]::GetRelativePath($repoRoot, $entry.File.FullName)
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
# CHECK 6: Sonar exclusion paths exist on disk
# ─────────────────────────────────────────────
function Get-SonarExclusionEntry {
    <#
    .SYNOPSIS
        Extracts the comma-separated, backslash-continued entries of one
        sonar.*.exclusions property from a sonar-project.properties file.
    .DESCRIPTION
        tools/.shared/sonarqube.ps1's Import-SonarProperties treats blank
        lines and comments as line terminators even mid-continuation, which
        corrupts multi-line values that are followed by a blank line or a
        comment (as sonar.exclusions is in this file) — the next property's
        key=value line gets appended as if it were part of the previous
        value, and the following key is lost entirely. This local parser
        keeps consuming raw lines while the previous one ends in '\',
        regardless of blank/comment content, matching standard .properties
        continuation semantics.
    #>
    param(
        # Not [Parameter(Mandatory)]: PowerShell's mandatory-parameter check rejects an
        # entire array argument if any element is an empty string, and this file's blank
        # lines (used as visual separators between properties) make that guaranteed.
        [string[]] $Lines,
        [Parameter(Mandatory)][string] $Key
    )

    $prefix = "$Key="
    for ($i = 0; $i -lt $Lines.Count; $i++) {
        $trimmed = $Lines[$i].TrimStart()
        if (-not $trimmed.StartsWith($prefix)) { continue }

        $value = $trimmed.Substring($prefix.Length)
        while ($value.TrimEnd().EndsWith('\')) {
            $value = $value.TrimEnd()
            $value = $value.Substring(0, $value.Length - 1).TrimEnd()
            $i++
            if ($i -ge $Lines.Count) { break }
            $value += $Lines[$i].Trim()
        }

        return @($value -split ',' | ForEach-Object { $_.Trim() } | Where-Object { $_ -ne '' })
    }

    return @()
}

if ($Scope -in 'All', 'Sonar') {
    Write-Check 'Sonar path validation'

    $sonarFile = Join-Path $repoRoot 'tools/code-scan/sonarqube/sonar-project.properties'
    if (Test-Path $sonarFile) {
        $sonarLines = [System.IO.File]::ReadAllLines((Resolve-Path $sonarFile).Path)

        $exclusionKeys = 'sonar.exclusions', 'sonar.coverage.exclusions', 'sonar.cpd.exclusions'
        $entries = @(foreach ($key in $exclusionKeys) { Get-SonarExclusionEntry -Lines $sonarLines -Key $key })

        $missing = 0
        $checkedCount = 0
        foreach ($entry in $entries) {
            $checkedCount++

            # Base path = the leading run of whole path segments that contain no wildcard.
            # Splitting on segments rather than at the first '*' character is what keeps a
            # filename-level glob such as 'src/PineGuard.MustClauses/MustString*.cs' resolving
            # to its real containing directory instead of the truncated, never-existent
            # 'src/PineGuard.MustClauses/MustString'.
            $segments = @($entry -split '/')
            $baseSegments = @()
            foreach ($segment in $segments) {
                if ($segment -match '[*?]') { break }
                $baseSegments += $segment
            }
            $basePath = ($baseSegments -join '/').TrimEnd('/')
            if (-not $basePath) { continue }

            # Only exclusions pointing into the analysed source tree are load-bearing: this
            # file sets sonar.sources to src/, so an exclusion outside it is a defensive no-op
            # that may legitimately name a directory absent from a clean checkout (.vs/,
            # coverage-results/, and the gitignored artifacts/). Failing on those made the
            # check's result depend on whether a build had been run.
            $isAnalysedTree = ($basePath -eq 'src') -or ($basePath -like 'src/*')

            $fullPath = Join-Path $repoRoot $basePath
            if (-not (Test-Path $fullPath)) {
                if ($isAnalysedTree) {
                    Write-Fail "Sonar exclusion references missing path: $entry (resolved base: $basePath)"
                    $missing++
                }
                else {
                    Write-Info "Sonar exclusion base absent from this checkout, outside sonar.sources: $entry"
                }
                continue
            }

            # The base directory exists; if the entry ends in a filename-level glob, confirm it
            # still matches at least one file, so a renamed-away exclusion is caught rather than
            # silently passing on the strength of its parent folder.
            $leaf = $segments[-1]
            if ($isAnalysedTree -and $leaf -match '[*?]' -and $leaf -ne '*' -and $leaf -ne '**' -and
                $baseSegments.Count -eq ($segments.Count - 1)) {
                if (-not (Test-Path -Path (Join-Path $fullPath $leaf))) {
                    Write-Fail "Sonar exclusion matches no file: $entry (searched: $basePath)"
                    $missing++
                }
            }
        }

        if ($checkedCount -eq 0) {
            Write-Info 'No sonar exclusion entries found to check'
        }
        elseif ($missing -eq 0) {
            Write-Pass "All $checkedCount sonar exclusion paths resolve to real locations on disk"
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

if ($totalIssues -eq 0) { exit 0 } else { exit 1 }
