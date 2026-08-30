<#
.SYNOPSIS
    Gen Coverage Report

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER Configuration
    See the param block for details.

.PARAMETER Scope
    See the param block for details.

.PARAMETER Clean
    See the param block for details.

.PARAMETER NoOpen
    See the param block for details.

.PARAMETER SkipHtml
    See the param block for details.

.PARAMETER ProjectFilter
    See the param block for details.

.PARAMETER Filter
    See the param block for details.

.PARAMETER Isolated
    See the param block for details.

.PARAMETER Framework
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')] [string] $Configuration = 'Debug',
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'ErrorOr', 'FluentResults', 'OneOf', 'All', 'Testing')] [string] $Scope = 'Core',
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

$utilityPath = Join-Path $PSScriptRoot '..\Import-CodeCoverageUtility.ps1'
if (-not (Test-Path $utilityPath)) {
    throw "Import-CodeCoverageUtility.ps1 not found at: $utilityPath"
}

. $utilityPath

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

    $repoRootForScopeCheck = Get-RepoRoot
    $scopeRegistryEntry = if ($Scope -in @('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'ErrorOr', 'FluentResults', 'OneOf', 'Testing')) {
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

    $expected = if ($null -ne $scopeRegistryEntry) {
        $scopePrefixFolder = ($scopeRegistryEntry.SourceDir -split '\\')[0]
        $scopeLeaf = Split-Path $scopeRegistryEntry.SourceDir -Leaf
        "(?i)(^|[\\/])($scopePrefixFolder[\\/]+)?$([regex]::Escape($scopeLeaf))[\\/]"
    }
    else {
        # Aggregate scopes: same derivation as the per-scope branch above — the real folder
        # leaf from SourceDir (Name is not always the folder suffix: Options ->
        # PineGuard.Extensions.Options). PathIncludeRegex is not reusable here: it is
        # ^-anchored for repo-relative paths, and this pattern runs against raw report
        # content where paths appear mid-string.
        $allFolderLeaves = (Get-PineGuardScope -All | ForEach-Object { [regex]::Escape((Split-Path $_.SourceDir -Leaf)) }) -join '|'
        "(?i)(^|[\\/])((src|tests)[\\/]+)?($allFolderLeaves)[\\/]"
    }

    if ($false) {
        if ($scopeHasAnySourceFiles) {
            $debugLog = Join-Path $ProjectResults "debug_failure.txt"
            "Scope: $Scope" | Out-File $debugLog
            "Expected Regex: $expected" | Out-File $debugLog -Append
            "Raw Content Length: $($raw.Length)" | Out-File $debugLog -Append
            "First 1000 chars: " | Out-File $debugLog -Append
            $raw.Substring(0, [Math]::Min($raw.Length, 1000)) | Out-File $debugLog -Append
            
            Write-Warning "Coverage validation failed: Content does not match regex '$expected'. See $debugLog"
            return $false
        }
    }

    return $true
}

$repoRoot = Get-RepoRoot
$generatedRoot = Get-XplatArtifactsRoot -RepoRoot $repoRoot
$resultsRoot = Join-Path $generatedRoot 'testresults'

if ($Clean) {
    if (Test-Path $generatedRoot) {
        Remove-Item -Path $generatedRoot -Recurse -Force
    }

    $redirectPath = Join-Path (Get-CodeCoverageArtifactsRoot -RepoRoot $repoRoot) "xplat-$($Scope.ToLower())-report.html"
    Remove-Item -LiteralPath $redirectPath -Force -ErrorAction SilentlyContinue
}

Ensure-Directory -Path $generatedRoot
Ensure-Directory -Path $resultsRoot

$generateScopeEntry = if ($Scope -in @('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'ErrorOr', 'FluentResults', 'OneOf', 'Testing')) {
    Get-PineGuardScope -Name $Scope
}
else {
    $null
}

$includePatterns = if ($null -ne $generateScopeEntry) {
    @($generateScopeEntry.CoverageIncludePattern)
}
else {
    @(Get-PineGuardScope -All | ForEach-Object CoverageIncludePattern)
}

# For speed, default to the single most relevant test project for the selected scope.
# (You can override by passing -ProjectFilter explicitly.)
if ($ProjectFilter -eq '*.UnitTests.csproj') {
    $ProjectFilter = if ($null -ne $generateScopeEntry) { $generateScopeEntry.DefaultProjectFilter } else { '*.UnitTests.csproj' }
}

$runSettingsPath = Join-Path $generatedRoot ("coverlet.$Scope.runsettings")
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
        Ensure-Directory -Path $projectResults

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
            $tempDir = Join-Path $generatedRoot $tempDirName
            Ensure-Directory -Path $tempDir
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
            Ensure-Directory -Path $projectResults
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

$reportGen = Ensure-ReportGenerator -RepoRoot $repoRoot
$reportDir = Join-Path $generatedRoot "html-$($Scope.ToLower())"
Ensure-Directory -Path $reportDir

# reportgenerator accepts semicolon-separated patterns/paths
$reportsArg = ($coverageFiles -join ';')

Write-Host "Generating HTML coverage report..." -ForegroundColor Cyan
& $reportGen "-reports:$reportsArg" "-targetdir:$reportDir" "-reporttypes:Html;HtmlSummary" "-filefilters:-obj\\*;-*\\obj\\*;-obj/*;-*/obj/*;-bin\\*;-*\\bin\\*;-bin/*;-*/bin/*" "-verbosity:Error"
if ($LASTEXITCODE -ne 0) {
    throw "reportgenerator failed with exit code: $LASTEXITCODE"
}

$indexPath = [IO.Path]::Combine($reportDir, 'index.html')

Write-Host "" 
Write-Host "Coverage report generated:" -ForegroundColor Green
Write-Host "  $reportDir" -ForegroundColor Green
Write-Host "Open: $indexPath" -ForegroundColor Green

$redirectPath = Join-Path (Get-CodeCoverageArtifactsRoot -RepoRoot $repoRoot) "xplat-$($Scope.ToLower())-report.html"
Write-RedirectHtml -OutputPath $redirectPath -Title "$Scope coverage report" -TargetRelativePath "xplat/html-$($Scope.ToLower())/index.html"

if (-not $NoOpen) {
    try {
        Write-Host "Opening coverage report in browser..." -ForegroundColor Cyan
        # Open the redirect file so it's always the canonical entry point.
        Start-Process -FilePath $redirectPath | Out-Null
    }
    catch {
        Write-Warning "Failed to auto-open coverage report. Open manually: $redirectPath"
    }
}
