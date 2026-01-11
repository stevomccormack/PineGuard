[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug',

    [switch] $Clean
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

function Get-ReportGeneratorPath {
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot
    )

    $toolsDir = Join-Path $RepoRoot '.dotnet/tools'
    $exe = Join-Path $toolsDir 'reportgenerator.exe'

    if (Test-Path $exe) {
        return $exe
    }

    New-Item -ItemType Directory -Path $toolsDir -Force | Out-Null

    Write-Host "Installing reportgenerator tool into: $toolsDir" -ForegroundColor Cyan
    & dotnet tool install dotnet-reportgenerator-globaltool --tool-path $toolsDir | Out-Host

    if (-not (Test-Path $exe)) {
        throw "reportgenerator.exe was not found after tool install. Expected at: $exe"
    }

    return $exe
}

function Get-LegacyDotCoverPath {
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot
    )

    # dotCover CommandLineTools 2025.x has a crashing `report` command on this machine.
    # As a fallback, we install an older tool version that can generate HTML from a .dcvr snapshot.
    $fallbackVersion = '2024.3.9'
    $toolsDir = Join-Path $RepoRoot ".dotnet/dotcover/$fallbackVersion"
    $exe = Join-Path $toolsDir 'dotnet-dotCover.exe'

    if (Test-Path $exe) {
        return $exe
    }

    New-Item -ItemType Directory -Path $toolsDir -Force | Out-Null

    Write-Host "Installing legacy dotCover runner into: $toolsDir (v$fallbackVersion)" -ForegroundColor Cyan
    & dotnet tool install JetBrains.dotCover.CommandLineTools --tool-path $toolsDir --version $fallbackVersion | Out-Host

    if (-not (Test-Path $exe)) {
        throw "Legacy dotCover runner was not found after tool install. Expected at: $exe"
    }

    return $exe
}

function Assert-DotCoverAvailable {
    $cmd = Get-Command dotCover -ErrorAction SilentlyContinue
    if ($null -ne $cmd) {
        return
    }

    throw @"
The 'dotCover' CLI was not found on PATH.

Install it as a .NET global tool:
  dotnet tool install -g JetBrains.dotCover.CommandLineTools

Or update it:
  dotnet tool update -g JetBrains.dotCover.CommandLineTools
"@
}

function Test-HasTestSources {
    param(
        [Parameter(Mandatory)]
        [string] $ProjectPath
    )

    $projectDir = Split-Path -Parent $ProjectPath

    $csFiles = Get-ChildItem -Path $projectDir -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }

    return ($null -ne $csFiles -and $csFiles.Count -gt 0)
}

$repoRoot = Get-RepoRoot
$generatedRoot = Join-Path $repoRoot 'etc/artifacts/code-coverage'
$dotCoverRoot = Join-Path $generatedRoot 'dotcover'
$dotCoverSnapshotsDir = Join-Path $dotCoverRoot 'snapshots'
$dotCoverXmlDir = Join-Path $dotCoverRoot 'xml'
$dotCoverHtmlDir = Join-Path $dotCoverRoot 'html'
$dotCoverLogsDir = Join-Path $dotCoverRoot 'logs'

if ($Clean) {
    if (Test-Path $dotCoverRoot) {
        Remove-Item -Path $dotCoverRoot -Recurse -Force
    }
}

New-Item -ItemType Directory -Path $generatedRoot -Force | Out-Null
New-Item -ItemType Directory -Path $dotCoverRoot -Force | Out-Null
New-Item -ItemType Directory -Path $dotCoverSnapshotsDir -Force | Out-Null
New-Item -ItemType Directory -Path $dotCoverXmlDir -Force | Out-Null
New-Item -ItemType Directory -Path $dotCoverHtmlDir -Force | Out-Null
New-Item -ItemType Directory -Path $dotCoverLogsDir -Force | Out-Null

Assert-DotCoverAvailable

# Find test projects (conservative: only *.UnitTests.csproj)
$testsDir = Join-Path $repoRoot 'tests'
$testProjects = Get-ChildItem -Path $testsDir -Recurse -File -Filter '*.UnitTests.csproj' |
    Sort-Object FullName |
    Select-Object -ExpandProperty FullName

if (-not $testProjects -or $testProjects.Count -eq 0) {
    throw "No test projects found under '$testsDir' matching '*.UnitTests.csproj'."
}

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host "Configuration: $Configuration" -ForegroundColor DarkGray
Write-Host "dotCover output: $dotCoverRoot" -ForegroundColor DarkGray

$runnableTestProjects = foreach ($project in $testProjects) {
    if (Test-HasTestSources -ProjectPath $project) {
        $project
        continue
    }

    $name = [IO.Path]::GetFileNameWithoutExtension($project)
    Write-Host "Skipping empty test project (no *.cs): $name" -ForegroundColor Yellow
}

$testProjects = @($runnableTestProjects)

if (-not $testProjects -or $testProjects.Count -eq 0) {
    throw "No runnable test projects found under '$testsDir' matching '*.UnitTests.csproj'."
}

Write-Host "Test projects: $($testProjects.Count)" -ForegroundColor DarkGray

$excludeAssemblies = "*.UnitTests"
$excludeAttributes = "System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute"

Push-Location $repoRoot
try {
    foreach ($project in $testProjects) {
        $name = [IO.Path]::GetFileNameWithoutExtension($project)
        $snapshotPath = Join-Path $dotCoverSnapshotsDir "$name.dcvr"
        $logPath = Join-Path $dotCoverLogsDir "$name.cover.log"

        Write-Host "Running dotCover cover: $name" -ForegroundColor Cyan

        # dotCover CLI parameters are case-sensitive and use kebab-case.
        # We run dotnet test under coverage and produce both snapshot + XML.
        dotCover cover `
            --log-file $logPath `
            --target-working-directory $repoRoot `
            --snapshot-output $snapshotPath `
            --exclude-assemblies $excludeAssemblies `
            --exclude-attributes $excludeAttributes `
            -- test $project -c $Configuration

        if ($LASTEXITCODE -ne 0) {
            Write-Warning "dotCover cover failed (exit code: $LASTEXITCODE) for project: $project"
            if (Test-Path -LiteralPath $logPath) {
                Write-Host "dotCover log: $logPath" -ForegroundColor Yellow
                Get-Content -LiteralPath $logPath -Tail 80 | Out-Host
            }
            throw "dotCover cover failed for project: $project"
        }

        if (-not (Test-Path $snapshotPath)) {
            Write-Warning "dotCover reported success but snapshot file was not found. Expected at: $snapshotPath"
            if (Test-Path -LiteralPath $logPath) {
                Write-Host "dotCover log: $logPath" -ForegroundColor Yellow
                Get-Content -LiteralPath $logPath -Tail 80 | Out-Host
            }
            throw "dotCover snapshot was not created. Expected at: $snapshotPath"
        }
    }
}
finally {
    Pop-Location
}

# Merge snapshots then produce a single XML report.
$snapshots = Get-ChildItem -Path $dotCoverSnapshotsDir -File -Filter '*.dcvr' |
    Sort-Object FullName |
    Select-Object -ExpandProperty FullName

if (-not $snapshots -or $snapshots.Count -eq 0) {
    throw "No dotCover snapshots found under: $dotCoverSnapshotsDir"
}

$mergedSnapshotPath = Join-Path $dotCoverRoot 'dotcover.merged.dcvr'
$mergeLogPath = Join-Path $dotCoverLogsDir 'merge.log'
$snapshotSourceArg = ($snapshots -join ',')

Write-Host "Merging dotCover snapshots..." -ForegroundColor Cyan
dotCover merge --log-file $mergeLogPath --snapshot-source $snapshotSourceArg --snapshot-output $mergedSnapshotPath
if ($LASTEXITCODE -ne 0 -or -not (Test-Path -LiteralPath $mergedSnapshotPath)) {
    Write-Warning "dotCover merge failed (exit code: $LASTEXITCODE)."
    if (Test-Path -LiteralPath $mergeLogPath) {
        Write-Host "dotCover merge log: $mergeLogPath" -ForegroundColor Yellow
        Get-Content -LiteralPath $mergeLogPath -Tail 120 | Out-Host
    }
    throw "dotCover merge failed."
}

$mergedXmlPath = Join-Path $dotCoverXmlDir 'dotcover.merged.xml'
$reportLogPath = Join-Path $dotCoverLogsDir 'report.log'

Write-Host "Generating dotCover XML report..." -ForegroundColor Cyan
dotCover report --log-file $reportLogPath --snapshot-source $mergedSnapshotPath --xml-report-output $mergedXmlPath
if ($LASTEXITCODE -ne 0 -or -not (Test-Path -LiteralPath $mergedXmlPath)) {
    Write-Warning "dotCover report failed (exit code: $LASTEXITCODE)."
    if (Test-Path -LiteralPath $reportLogPath) {
        Write-Host "dotCover report log: $reportLogPath" -ForegroundColor Yellow
        Get-Content -LiteralPath $reportLogPath -Tail 120 | Out-Host
    }

    Write-Warning "dotCover reporting appears to be crashing on this machine/version. Attempting fallback: legacy dotCover HTML report..."
    $legacyDotCoverExe = Get-LegacyDotCoverPath -RepoRoot $repoRoot
    $fallbackHtmlPath = Join-Path $dotCoverRoot 'dotcover.report.html'

    & $legacyDotCoverExe report --Source=$mergedSnapshotPath --ReportType=HTML --Output=$fallbackHtmlPath | Out-Host
    if ($LASTEXITCODE -ne 0 -or -not (Test-Path -LiteralPath $fallbackHtmlPath)) {
        throw "dotCover report failed, and fallback legacy report also failed. See logs under: $dotCoverLogsDir"
    }

    Write-Host "" 
    Write-Host "Coverage report generated (fallback):" -ForegroundColor Green
    Write-Host "  $fallbackHtmlPath" -ForegroundColor Green
    try {
        Write-Host "Opening coverage report in browser..." -ForegroundColor Cyan
        Start-Process -FilePath $fallbackHtmlPath | Out-Null
    }
    catch {
        Write-Warning "Failed to auto-open coverage report. Open manually: $fallbackHtmlPath"
    }

    return
}

# Generate HTML from dotCover XML via reportgenerator (keeps output comparable to the coverlet workflow).
$reportGen = Get-ReportGeneratorPath -RepoRoot $repoRoot
$reportsArg = $mergedXmlPath

Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
& $reportGen "-reports:$reportsArg" "-targetdir:$dotCoverHtmlDir" "-reporttypes:Html;HtmlSummary" "-filefilters:-*\\obj\\*;-*/obj/*;-*\\bin\\*;-*/bin/*" "-verbosity:Error"
if ($LASTEXITCODE -ne 0) {
    throw "reportgenerator failed with exit code: $LASTEXITCODE"
}

Write-Host "" 
Write-Host "Coverage report generated:" -ForegroundColor Green
Write-Host "  $dotCoverHtmlDir" -ForegroundColor Green

$candidateIndexPaths = @(
    (Join-Path $dotCoverHtmlDir 'index.html'),
    (Join-Path $dotCoverHtmlDir 'index.htm')
)

$indexPath = $candidateIndexPaths |
    Where-Object { Test-Path -LiteralPath $_ } |
    Select-Object -First 1

if ($null -eq $indexPath) {
    $indexPath = Get-ChildItem -LiteralPath $dotCoverHtmlDir -File -Filter 'index.htm*' -ErrorAction SilentlyContinue |
        Sort-Object FullName |
        Select-Object -First 1 -ExpandProperty FullName
}

if ([string]::IsNullOrWhiteSpace($indexPath)) {
    Write-Warning "Could not locate an index.* file under: $dotCoverHtmlDir"
    Write-Host "Open folder: $dotCoverHtmlDir" -ForegroundColor Green
}
else {
    Write-Host "Open: $indexPath" -ForegroundColor Green
}

try {
    Write-Host "Opening coverage report in browser..." -ForegroundColor Cyan
    if (-not [string]::IsNullOrWhiteSpace($indexPath)) {
        Start-Process -FilePath $indexPath | Out-Null
    }
    else {
        Start-Process -FilePath $dotCoverHtmlDir | Out-Null
    }
}
catch {
    if (-not [string]::IsNullOrWhiteSpace($indexPath)) {
        Write-Warning "Failed to auto-open coverage report. Open manually: $indexPath"
    }
    else {
        Write-Warning "Failed to auto-open coverage report. Open manually: $dotCoverHtmlDir"
    }
}
