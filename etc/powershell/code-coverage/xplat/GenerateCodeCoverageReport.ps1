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

function Has-TestSources {
    param(
        [Parameter(Mandatory)]
        [string] $ProjectPath
    )

    $projectDir = Split-Path -Parent $ProjectPath

    $csFiles = Get-ChildItem -Path $projectDir -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }

    return ($null -ne $csFiles -and $csFiles.Count -gt 0)
}

function Test-CoverageLooksValid {
    param(
        [Parameter(Mandatory)]
        [string] $ProjectResults
    )

    $latest = Get-ChildItem -LiteralPath $ProjectResults -Recurse -File -Filter 'coverage.cobertura.xml' -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1

    if ($null -eq $latest) {
        return $false
    }

    $raw = Get-Content -Raw -LiteralPath $latest.FullName -ErrorAction SilentlyContinue
    if ([string]::IsNullOrWhiteSpace($raw)) {
        return $false
    }

    # Guard against intermittent collector output that yields an empty report.
    if ($raw -match 'lines-valid="0"' -or $raw -match '<packages\s*/>') {
        return $false
    }

    # Ensure we actually captured the main library (PineGuard.Core). Without this, coverage may silently focus on PineGuard.Testing.
    if ($raw -notmatch '(?i)(^|[\\/])src[\\/]+PineGuard\.Core[\\/]') {
        return $false
    }

    return $true
}

$repoRoot = Get-RepoRoot
$generatedRoot = Join-Path $repoRoot 'etc/artifacts/code-coverage/xplat'
$resultsRoot = Join-Path $generatedRoot 'testresults'
$runSettingsPath = Join-Path (Split-Path -Parent $PSScriptRoot) 'coverlet.runsettings'

if ($Clean) {
    if (Test-Path $generatedRoot) {
        Remove-Item -Path $generatedRoot -Recurse -Force
    }
}

New-Item -ItemType Directory -Path $generatedRoot -Force | Out-Null
New-Item -ItemType Directory -Path $resultsRoot -Force | Out-Null

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

$runnableTestProjects = foreach ($project in $testProjects) {
    if (Has-TestSources -ProjectPath $project) {
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

Push-Location $repoRoot
try {
    foreach ($project in $testProjects) {
        $name = [IO.Path]::GetFileNameWithoutExtension($project)
        $projectResults = Join-Path $resultsRoot $name
        New-Item -ItemType Directory -Path $projectResults -Force | Out-Null

        Write-Host "Running tests + collecting coverage: $name" -ForegroundColor Cyan

        $settingsArg = @()
        if (Test-Path $runSettingsPath) {
            $settingsArg = @('--settings', $runSettingsPath)
        }

        $attempt = 0
        while ($true) {
            $attempt++

            & dotnet test $project -c $Configuration --collect:"XPlat Code Coverage" --results-directory $projectResults @settingsArg
            if ($LASTEXITCODE -ne 0) {
                throw "dotnet test failed for project: $project"
            }

            if (Test-CoverageLooksValid -ProjectResults $projectResults) {
                break
            }

            if ($attempt -ge 2) {
                throw "Coverage output was empty or missing PineGuard.Core for project '$name'. See: $projectResults"
            }

            Write-Warning "Coverage output looked invalid for '$name' (attempt $attempt). Retrying once..."
            Remove-Item -LiteralPath $projectResults -Recurse -Force -ErrorAction SilentlyContinue
            New-Item -ItemType Directory -Path $projectResults -Force | Out-Null
        }
    }
}
finally {
    Pop-Location
}

# Collect coverage XML files (newest per test project)
$coverageFiles = Get-ChildItem -Path $resultsRoot -Recurse -File -Filter 'coverage.cobertura.xml' |
    Group-Object { Split-Path -Parent (Split-Path -Parent $_.FullName) } |
    ForEach-Object { $_.Group | Sort-Object LastWriteTime -Descending | Select-Object -First 1 } |
    Sort-Object FullName |
    Select-Object -ExpandProperty FullName

if (-not $coverageFiles -or $coverageFiles.Count -eq 0) {
    throw "No 'coverage.cobertura.xml' files found under: $resultsRoot"
}

$reportGen = Get-ReportGeneratorPath -RepoRoot $repoRoot
$reportDir = Join-Path $generatedRoot 'html'
New-Item -ItemType Directory -Path $reportDir -Force | Out-Null

# reportgenerator accepts semicolon-separated patterns/paths
$reportsArg = ($coverageFiles -join ';')

Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
& $reportGen "-reports:$reportsArg" "-targetdir:$reportDir" "-reporttypes:Html;HtmlSummary" "-filefilters:-*\\obj\\*;-*/obj/*;-*\\bin\\*;-*/bin/*" "-verbosity:Error"
if ($LASTEXITCODE -ne 0) {
    throw "reportgenerator failed with exit code: $LASTEXITCODE"
}

Write-Host "" 
Write-Host "Coverage report generated:" -ForegroundColor Green
Write-Host "  $reportDir" -ForegroundColor Green
$indexPath = [IO.Path]::Combine($reportDir, 'index.html')
Write-Host "Open: $indexPath" -ForegroundColor Green

try {
    Write-Host "Opening coverage report in browser..." -ForegroundColor Cyan
    Start-Process -FilePath $indexPath | Out-Null
}
catch {
    Write-Warning "Failed to auto-open coverage report. Open manually: $indexPath"
}
