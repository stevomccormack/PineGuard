<#
.SYNOPSIS
    New Coverage Report (Coverlet engine)

.DESCRIPTION
    Collects code coverage for the given scope via Coverlet's "XPlat Code Coverage" collector
    (`dotnet test --collect`), then renders it to HTML with ReportGenerator. This is the Coverlet
    engine script behind the D-9 front door (tools/code-coverage/New-CoverageReport.ps1
    -Engine Coverlet); it holds all of Coverlet's own collection logic and none of the
    engine-selection logic, which lives in the front door.

    Output lands at artifacts/code-coverage/coverlet/<scope>/ (F-11: named after the tool,
    Coverlet, not the "XPlat Code Coverage" collector friendlyName that the old xplat/ folder
    was named after):
      - testresults/<ProjectName>/<RunId>/coverage.<format>.xml -- raw collection output
      - report/index.html                                       -- ReportGenerator HTML output

.PARAMETER Configuration
    Build configuration: Debug (default) or Release.

.PARAMETER Scope
    Registry coverage scope (see tools/.shared/dotnet-projects.ps1's Get-PineGuardScope), or
    'All' to run every *.UnitTests.csproj project with no scope-specific narrowing.

.PARAMETER Clean
    Delete this scope's own previous output (testresults/ and report/) before collecting.

.PARAMETER NoOpen
    Do not open the generated HTML report in the default browser.

.PARAMETER SkipHtml
    Collect coverage XML only; skip the ReportGenerator HTML step entirely.

.PARAMETER ProjectFilter
    Glob used to discover test projects. Defaults to the scope's own DefaultProjectFilter when
    left at the generic '*.UnitTests.csproj' value.

.PARAMETER Filter
    `dotnet test --filter` expression, forwarded as-is.

.PARAMETER Isolated
    Publish each test project to a temp directory before testing it, to avoid locking source
    bin/ folders (useful when running coverage alongside an open IDE build).

.PARAMETER Format
    Coverlet collector output format: cobertura (default) or opencover.

.PARAMETER Framework
    TFM passthrough to `dotnet test -f`.
#>

[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')] [string] $Configuration = 'Debug',
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'All', 'Testing')] [string] $Scope = 'Core',
    [switch] $Clean,
    [switch] $NoOpen,
    [switch] $SkipHtml,
    [string] $ProjectFilter = '*.UnitTests.csproj',
    [string] $Filter,
    [switch] $Isolated,
    [ValidateSet('cobertura', 'opencover')] [string] $Format = 'cobertura',
    [string] $Framework
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '..\..\.shared\path.ps1')
. (Join-Path $PSScriptRoot '..\..\.shared\dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '..\..\.shared\coverage.ps1')

$coverageFileName = "coverage.$Format.xml"

function Test-CoverageLooksValid {
    param(
        [Parameter(Mandatory)]
        [string] $ProjectResults,

        [Parameter(Mandatory)]
        [string] $Scope
    )

    $latest = Get-ChildItem -LiteralPath $ProjectResults -Recurse -File -Filter $script:coverageFileName -ErrorAction SilentlyContinue |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1

    if ($null -eq $latest) {
        return $false
    }

    $raw = Get-Content -Raw -LiteralPath $latest.FullName -ErrorAction SilentlyContinue
    if ([string]::IsNullOrWhiteSpace($raw)) {
        return $false
    }

    $repoRootForScopeCheck = Get-RepoRoot -StartDirectory $PSScriptRoot
    $scopeRegistryEntry = if ($Scope -in @(Get-PineGuardScope -All | ForEach-Object Name)) {
        Get-PineGuardScope -Name $Scope
    }
    else {
        $null
    }
    $scopeSourceDir = if ($null -ne $scopeRegistryEntry) { Join-Path $repoRootForScopeCheck $scopeRegistryEntry.SourceDir } else { $null }

    $scopeHasAnySourceFiles = $true
    if (-not [string]::IsNullOrWhiteSpace($scopeSourceDir) -and (Test-Path $scopeSourceDir)) {
        $anyCs = Get-ChildItem -LiteralPath $scopeSourceDir -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '([\\/])(bin|obj)\1' } |
        Select-Object -First 1
        $scopeHasAnySourceFiles = ($null -ne $anyCs)
    }

    if ($scopeHasAnySourceFiles -and ($raw -match 'lines-valid="0"' -or $raw -match '<packages\s*/>')) {
        Write-Warning "Coverage validation failed: Zero lines valid or empty packages."
        return $false
    }

    return $true
}

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$scopeRoot = Get-CoverageScopeRoot -RepoRoot $repoRoot -Engine 'Coverlet' -Scope $Scope
$resultsRoot = Join-Path $scopeRoot 'testresults'

if ($Clean) {
    # Delete only this scope's own previous output: its testresults/<project> folder(s) and its
    # report/ folder. Every other scope's coverlet/<otherscope>/ must survive untouched -- wiping
    # the shared xplat/ root wholesale used to destroy every scope's already-collected results at
    # once (F-19, the "stale data problem"); physically separating scopes under their own
    # coverlet/<scope>/ folder (rather than a testresults/ pool shared across scopes) is what
    # makes that isolation hold today.
    $scopeResultsPaths = @(Get-ScopeTestResultsPaths -RepoRoot $repoRoot -ResultsRoot $resultsRoot -Scope $Scope)
    foreach ($scopeResultsPath in $scopeResultsPaths) {
        if (Test-Path -LiteralPath $scopeResultsPath) {
            Remove-Item -LiteralPath $scopeResultsPath -Recurse -Force
        }
    }

    $scopeReportDir = Join-Path $scopeRoot 'report'
    if (Test-Path -LiteralPath $scopeReportDir) {
        Remove-Item -LiteralPath $scopeReportDir -Recurse -Force
    }
}

New-Item -ItemType Directory -Path $scopeRoot -Force | Out-Null
New-Item -ItemType Directory -Path $resultsRoot -Force | Out-Null

$generateScopeEntry = if ($Scope -in @(Get-PineGuardScope -All | ForEach-Object Name)) {
    Get-PineGuardScope -Name $Scope
}
else {
    $null
}

# A scope contributes one pattern per assembly it ships (Analyzers ships two), and
# Write-CoverletRunSettings joins them into the ';'-separated <Include> coverlet expects.
$includePatterns = if ($null -ne $generateScopeEntry) {
    @($generateScopeEntry.CoverageIncludePatterns)
}
else {
    @(Get-PineGuardScope -All | ForEach-Object CoverageIncludePatterns)
}

# For speed, default to the single most relevant test project for the selected scope.
# (You can override by passing -ProjectFilter explicitly.)
if ($ProjectFilter -eq '*.UnitTests.csproj') {
    $ProjectFilter = if ($null -ne $generateScopeEntry) { $generateScopeEntry.DefaultProjectFilter } else { '*.UnitTests.csproj' }
}

$runSettingsPath = Join-Path $scopeRoot 'coverlet.runsettings'
Write-CoverletRunSettings -OutputPath $runSettingsPath -IncludePatterns $includePatterns -Format $Format

$includeEmptyTestProjects = if ($null -ne $generateScopeEntry) { [bool]$generateScopeEntry.IncludeEmptyTestProjects } else { $false }
$testProjects = @(Get-TestProjects -RepoRoot $repoRoot -ProjectFilter $ProjectFilter -IncludeEmpty:$includeEmptyTestProjects)

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host "Configuration: $Configuration" -ForegroundColor DarkGray
Write-Host "Scope: $Scope" -ForegroundColor DarkGray
Write-Host "Test projects: $($testProjects.Count)" -ForegroundColor DarkGray

Push-Location $repoRoot
try {
    $runCoverageFiles = @()

    foreach ($project in $testProjects) {
        $name = [IO.Path]::GetFileNameWithoutExtension($project)
        $projectResults = Join-Path $resultsRoot $name
        New-Item -ItemType Directory -Path $projectResults -Force | Out-Null

        Write-Host "Running tests + collecting coverage: $name" -ForegroundColor Cyan

        $settingsArg = @('--settings', $runSettingsPath)

        if (-not [string]::IsNullOrWhiteSpace($Framework)) {
            $settingsArg += @('-f', $Framework)
        }

        if (-not [string]::IsNullOrWhiteSpace($Filter)) {
            $settingsArg += @('--filter', $Filter)
        }

        # Isolation logic
        $targetToTest = $project
        $cleanupTempDir = $null

        if ($Isolated) {
            # Publish to temp directory to avoid locking source bin folders
            $tempDirName = "iso-test-" + [Guid]::NewGuid().ToString('N')
            $tempDir = Join-Path $scopeRoot $tempDirName
            New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
            $cleanupTempDir = $tempDir

            try {
                Write-Host "  Isolating: Publishing to $tempDir ..." -ForegroundColor Gray
                & dotnet publish $project -c $Configuration -o $tempDir | Out-Null
                if ($LASTEXITCODE -ne 0) {
                    throw "dotnet publish failed for project: $project"
                }

                # Locate the published dll
                $publishedDll = Join-Path $tempDir "$name.dll"
                if (-not (Test-Path $publishedDll)) {
                    throw "Could not locate published dll at $publishedDll"
                }

                $targetToTest = $publishedDll
                Write-Host "  Testing isolated dll: $publishedDll" -ForegroundColor Gray
            }
            catch {
                if ($cleanupTempDir) { Remove-Item -Path $cleanupTempDir -Recurse -Force -ErrorAction SilentlyContinue }
                throw
            }
        }

        $attempt = 0
        while ($true) {
            $attempt++

            & dotnet test $targetToTest -c $Configuration --collect:"XPlat Code Coverage" --results-directory $projectResults @settingsArg
            if ($LASTEXITCODE -ne 0) {
                if ($Isolated) { Remove-Item -Path $cleanupTempDir -Recurse -Force -ErrorAction SilentlyContinue }
                throw "dotnet test failed for project: $project"
            }

            if (Test-CoverageLooksValid -ProjectResults $projectResults -Scope $Scope) {
                break
            }

            if ($attempt -ge 2) {
                if ($Isolated) { Remove-Item -Path $cleanupTempDir -Recurse -Force -ErrorAction SilentlyContinue }
                throw "Coverage output was empty or missing expected scope '$Scope' for project '$name'. See: $projectResults"
            }

            Write-Warning "Coverage output looked invalid for '$name' (attempt $attempt). Retrying once..."
            Remove-Item -LiteralPath $projectResults -Recurse -Force -ErrorAction SilentlyContinue
            New-Item -ItemType Directory -Path $projectResults -Force | Out-Null
        }

        if ($Isolated) {
            Remove-Item -Path $cleanupTempDir -Recurse -Force -ErrorAction SilentlyContinue
        }

        $latest = Get-ChildItem -LiteralPath $projectResults -Recurse -File -Filter $coverageFileName -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1

        if ($null -eq $latest) {
            throw "Coverage file not found after successful test run for '$name'. Expected a '$coverageFileName' under: $projectResults"
        }

        $runCoverageFiles += $latest.FullName
    }
}
finally {
    Pop-Location
}

$coverageFiles = @($runCoverageFiles | Sort-Object -Unique)
Write-Host "Collected coverage files: $($coverageFiles.Count)" -ForegroundColor Cyan
$coverageFiles | ForEach-Object { Write-Host " - $_" -ForegroundColor Cyan }

if ($SkipHtml) {
    Write-Host ""
    Write-Host "Coverage collection complete (HTML skipped)." -ForegroundColor Green
    Write-Host "Coverage XML files ($Format):" -ForegroundColor Green
    foreach ($f in $coverageFiles) {
        Write-Host "  $f" -ForegroundColor Green
    }

    return
}

$reportDir = Join-Path $scopeRoot 'report'
New-Item -ItemType Directory -Path $reportDir -Force | Out-Null

# reportgenerator accepts semicolon-separated patterns/paths
$reportsArg = ($coverageFiles -join ';')

# reportgenerator is a local dotnet tool (.config/dotnet-tools.json), invoked via
# `dotnet reportgenerator`. `dotnet tool restore` must run from the repo root (or a
# subdirectory of it) so it finds the manifest; it is fast and idempotent, so it is safe
# to call unconditionally rather than trying to detect whether a restore already happened.
Push-Location $repoRoot
try {
    Write-Host "Restoring local dotnet tools..." -ForegroundColor DarkGray
    dotnet tool restore | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet tool restore failed with exit code: $LASTEXITCODE"
    }

    Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
    # Also emits a merged Cobertura.xml alongside the HTML (§3.5): one canonical per-scope
    # cobertura file, in the same report/ output shape the dotCover engine will use (T3.11), even
    # though Test-Coverage.ps1 reads the raw per-project testresults/ files directly today.
    & dotnet reportgenerator "-reports:$reportsArg" "-targetdir:$reportDir" "-reporttypes:Html;HtmlSummary;Cobertura" "-filefilters:-obj\\*;-*\\obj\\*;-obj/*;-*/obj/*;-bin\\*;-*\\bin\\*;-bin/*;-*/bin/*" "-verbosity:Error"
    if ($LASTEXITCODE -ne 0) {
        throw "reportgenerator failed with exit code: $LASTEXITCODE"
    }
}
finally {
    Pop-Location
}

$indexPath = [IO.Path]::Combine($reportDir, 'index.html')

Write-Host ""
Write-Host "Coverage report generated:" -ForegroundColor Green
Write-Host "  $reportDir" -ForegroundColor Green
Write-Host "Open: $indexPath" -ForegroundColor Green

if (-not $NoOpen) {
    try {
        Write-Host "Opening coverage report in browser..." -ForegroundColor Cyan
        # F-39: open index.html directly. The redirect-page mechanism this used to go through
        # (a meta-refresh page one level up in artifacts/code-coverage/) existed only because
        # every scope's report shared one xplat/ folder with no stable per-scope entry point;
        # now that each scope has its own coverlet/<scope>/report/ folder, index.html itself is
        # already the stable entry point and the redirect has nothing left to do.
        Start-Process -FilePath $indexPath | Out-Null
    }
    catch {
        Write-Warning "Failed to auto-open coverage report. Open manually: $indexPath"
    }
}
