<#
.SYNOPSIS
    New Coverage Report (dotCover engine, snapshot-only fallback)

.DESCRIPTION
    Collects a JetBrains dotCover coverage snapshot for the given scope via `dotCover cover
    --snapshot-output` (a local dotnet tool, `.config/dotnet-tools.json`). This is the dotCover
    engine script behind the D-9 front door (tools/code-coverage/New-CoverageReport.ps1 -Engine
    DotCover); it holds all of dotCover's own collection logic and none of the engine-selection
    logic, which lives in the front door.

    THIS IS THE SNAPSHOT-ONLY FALLBACK, NOT THE FULL-FEATURED ENGINE THE PLAN ORIGINALLY
    DESCRIBED FOR T3.11. Spike T3.10 (docs/ai/plans/tools-review-and-standardisation.md
    ## Baselines, "T3.10 -- dotCover spike") found that dotCover 2025.3.3's `--xml-report-output`
    collection genuinely fails on both net8.0 and net10.0: it either hangs indefinitely against a
    persistent VBCSCompiler (the Roslyn compiler server) process, or -- once that hang is avoided --
    throws "Unhandled exception: Snapshot container is not initialized" from
    JetBrains.dotCover.ConsoleRunner.Components.ReportBuilder.BuildReports and produces no XML at
    all. This was reproduced four times, including a --no-build run that rules out the build step
    as the cause. It is a real, documented upstream JetBrains bug, not a local misconfiguration.

    What DOES work, proven by T3.10 and re-verified while building this script: `dotCover cover
    --snapshot-output <path>.dcvr --exclude-assemblies *.UnitTests --exclude-processes
    VBCSCompiler --exclude-processes MSBuild -- test <csproj> -c <cfg> -f <tfm>` (process names
    without their platform-specific extension here; the script itself appends '.exe' on Windows
    only -- see $exeSuffix below) (no --xml-report-output at all) completes cleanly on both TFMs
    and produces a real, valid .dcvr
    snapshot -- JetBrains Rider's native coverage format. A developer can open that file directly
    in Rider for visual, file-by-file coverage exploration, but it CANNOT be converted to
    Cobertura/HTML by ReportGenerator (that path was never proven working, since dotCover never
    produced an XML to test it against). Accordingly, this script:

      - does NOT invoke ReportGenerator or attempt to produce HTML/Cobertura output;
      - does NOT gate on a numeric threshold (there is nothing to gate on -- see
        tools/code-coverage/Test-Coverage.ps1 -Engine DotCover for the accurate "not supported"
        error this repo throws instead of a fake pass/fail);
      - DOES print, after collection, exactly where each .dcvr snapshot landed and a pointer to
        open it in Rider's coverage viewer.

    Output lands at artifacts/code-coverage/dotcover/<scope>/snapshots/<project>.<tfm>.dcvr (§3.5's
    documented raw-output path convention for this engine; tools/.shared/coverage.ps1's
    Get-CoverageScopeRoot already documents "snapshots/ for dotCover" alongside "testresults/ for
    Coverlet"). One .dcvr file is produced per (test project, target framework) pair.

    --target-working-directory is passed explicitly to every dotCover invocation. This was found
    while verifying this script, not documented by T3.10 itself: dotCover's own default working
    directory is the *target executable's* directory (dotnet's own install folder), not the
    caller's current directory, so a relative csproj path fails with MSBuild error MSB1009
    ("Project file does not exist") unless either the csproj path is absolute or
    --target-working-directory is set. This script does both, for safety.

.PARAMETER Configuration
    Build configuration: Debug (default) or Release. Forwarded to `dotnet test -c` inside the
    dotCover-wrapped invocation (`dotCover cover ... -- test <csproj> -c <Configuration> -f
    <tfm>`).

.PARAMETER Scope
    Registry coverage scope (see tools/.shared/dotnet-projects.ps1's Get-PineGuardScope), or
    'All' to run every *.UnitTests.csproj project with no scope-specific narrowing. Same scope
    vocabulary as the Coverlet engine (§3.5: both engines share one parameter contract).

.PARAMETER Clean
    Delete this scope's own previous dotCover snapshot output (the snapshots/ folder under
    artifacts/code-coverage/dotcover/<scope>/) before collecting.

.PARAMETER NoOpen
    Accepted for parameter-contract parity with the Coverlet engine only (§3.5, D-9: the front
    door forwards whatever the caller bound via a blind splat, so both engine scripts must accept
    the same parameter set or that splat fails whenever the caller passes a Coverlet-only switch
    under -Engine DotCover). Has NO EFFECT here: dotCover snapshot collection never auto-opens
    anything -- there is no HTML report to open (T3.10). See the printed .dcvr path(s) after
    collection and open them manually in Rider's coverage viewer.

.PARAMETER SkipHtml
    Accepted for parameter-contract parity with the Coverlet engine only (see -NoOpen above for
    why). Has NO EFFECT here: this engine never produces HTML output in the first place (T3.10:
    dotCover 2025.3.3's --xml-report-output path throws "Snapshot container is not initialized"
    on both TFMs, so only the raw --snapshot-output .dcvr is ever collected) -- there is no HTML
    step to skip.

.PARAMETER ProjectFilter
    Glob used to discover test projects. Defaults to the scope's own DefaultProjectFilter when
    left at the generic '*.UnitTests.csproj' value.

.PARAMETER Filter
    `dotnet test --filter` expression, forwarded through dotCover's target-arguments (`-- test
    <csproj> -c <Configuration> -f <tfm> --filter <Filter>`).

.PARAMETER Isolated
    Publish each test project to a temp directory (once per target framework, since a published
    output is TFM-specific) before testing it under dotCover, to avoid locking source bin/
    folders. Mirrors coverlet/New-CoverageReport.ps1's -Isolated, extended to loop per TFM here
    because this engine can collect more than one TFM per invocation.

.PARAMETER Format
    Accepted for parameter-contract parity with the Coverlet engine only (see -NoOpen above for
    why). Has NO EFFECT here: dotCover's snapshot-only fallback has no Cobertura/OpenCover output
    to format (T3.10) -- there is no report-generation step this value could influence.

.PARAMETER Framework
    TFM passthrough to `dotnet test -f`. When omitted, every TargetFramework the resolved test
    project actually builds (net8.0 and net10.0 today, resolved per-project via `dotnet msbuild
    -getProperty:TargetFrameworks` rather than hardcoded, so a future TFM change needs no edit
    here) gets its own .dcvr snapshot.
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
    [ValidateSet('cobertura', 'opencover')] [string] $Format,
    [string] $Framework
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '..' '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '..' '.shared' 'dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '..' '..' '.shared' 'coverage.ps1')

# Process names on Windows carry a '.exe' suffix; dotCover's cross-platform console runner
# expects bare names on Linux/macOS. $IsWindows is a pwsh 7+ automatic variable.
$exeSuffix = if ($IsWindows) { '.exe' } else { '' }

function Resolve-DotCoverTargetFrameworks {
    <#
    .SYNOPSIS
        Resolves the effective TargetFramework(s) a test csproj actually builds.

    .DESCRIPTION
        Evaluated via `dotnet msbuild -getProperty`, not read as a literal XML element, because
        every PineGuard test project inherits <TargetFrameworks> from tests/Directory.Build.props
        (net8.0;net10.0 today) rather than declaring it in its own csproj -- a literal-element
        read would find nothing. Falls back to the singular TargetFramework property for a
        single-targeted project.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $CsprojPath
    )

    $multi = & dotnet msbuild $CsprojPath '-getProperty:TargetFrameworks' '-nologo' 2>$null
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($multi)) {
        $tfms = @(($multi -split ';') | ForEach-Object { $_.Trim() } | Where-Object { -not [string]::IsNullOrWhiteSpace($_) })
        if ($tfms.Count -gt 0) {
            return $tfms
        }
    }

    $single = & dotnet msbuild $CsprojPath '-getProperty:TargetFramework' '-nologo' 2>$null
    if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($single)) {
        return @($single.Trim())
    }

    throw "Could not resolve TargetFramework(s) for '$CsprojPath' via 'dotnet msbuild -getProperty'."
}

# See -NoOpen/-SkipHtml/-Format PARAMETER blocks above: these three are kept in the param block
# (not omitted) purely so the D-9 front door's blind splat (& $engineScript @forwardParams) never
# fails to bind a parameter the caller passed for the *other* engine. Accept-and-warn here is
# cheaper and safer than teaching the front door which parameters are engine-specific.
if ($PSBoundParameters.ContainsKey('Format')) {
    Write-Warning "-Format ('$Format') has no effect for -Engine DotCover: the snapshot-only fallback never produces Cobertura/OpenCover output (T3.10). Ignored."
}
if ($SkipHtml) {
    Write-Warning '-SkipHtml has no effect for -Engine DotCover: this engine never generates HTML in the first place (T3.10). Ignored.'
}
if ($NoOpen) {
    Write-Warning '-NoOpen has no effect for -Engine DotCover: nothing is auto-opened either way -- there is no HTML/report to open (T3.10). Ignored.'
}

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$scopeRoot = Get-CoverageScopeRoot -RepoRoot $repoRoot -Engine 'DotCover' -Scope $Scope
$snapshotsRoot = Join-Path $scopeRoot 'snapshots'

if ($Clean) {
    if (Test-Path -LiteralPath $snapshotsRoot) {
        Remove-Item -LiteralPath $snapshotsRoot -Recurse -Force
    }
}

New-Item -ItemType Directory -Path $scopeRoot -Force | Out-Null
New-Item -ItemType Directory -Path $snapshotsRoot -Force | Out-Null

$generateScopeEntry = if ($Scope -in @(Get-PineGuardScope -All | ForEach-Object Name)) {
    Get-PineGuardScope -Name $Scope
}
else {
    $null
}

# For speed, default to the single most relevant test project for the selected scope, matching
# coverlet/New-CoverageReport.ps1's own default resolution (you can override by passing
# -ProjectFilter explicitly).
if ($ProjectFilter -eq '*.UnitTests.csproj') {
    $ProjectFilter = if ($null -ne $generateScopeEntry) { $generateScopeEntry.DefaultProjectFilter } else { '*.UnitTests.csproj' }
}

$includeEmptyTestProjects = if ($null -ne $generateScopeEntry) { [bool]$generateScopeEntry.IncludeEmptyTestProjects } else { $false }
$testProjects = @(Get-TestProjects -RepoRoot $repoRoot -ProjectFilter $ProjectFilter -IncludeEmpty:$includeEmptyTestProjects)

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host 'Engine: dotCover (snapshot-only fallback -- see T3.10)' -ForegroundColor DarkGray
Write-Host "Configuration: $Configuration" -ForegroundColor DarkGray
Write-Host "Scope: $Scope" -ForegroundColor DarkGray
Write-Host "Test projects: $($testProjects.Count)" -ForegroundColor DarkGray

Push-Location $repoRoot
try {
    Write-Host 'Restoring local dotnet tools...' -ForegroundColor DarkGray
    dotnet tool restore | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet tool restore failed with exit code: $LASTEXITCODE"
    }

    $snapshotFiles = @()

    foreach ($project in $testProjects) {
        $name = [IO.Path]::GetFileNameWithoutExtension($project)

        $tfms = if (-not [string]::IsNullOrWhiteSpace($Framework)) {
            @($Framework)
        }
        else {
            @(Resolve-DotCoverTargetFrameworks -CsprojPath $project)
        }

        Write-Host "Collecting dotCover snapshot(s): $name [$($tfms -join ', ')]" -ForegroundColor Cyan

        foreach ($tfm in $tfms) {
            $targetToTest = $project
            $cleanupTempDir = $null

            if ($Isolated) {
                $tempDirName = 'iso-dotcover-' + [Guid]::NewGuid().ToString('N')
                $tempDir = Join-Path $scopeRoot $tempDirName
                New-Item -ItemType Directory -Path $tempDir -Force | Out-Null
                $cleanupTempDir = $tempDir

                try {
                    Write-Host "  Isolating: Publishing to $tempDir ($tfm)..." -ForegroundColor Gray
                    & dotnet publish $project -c $Configuration -f $tfm -o $tempDir | Out-Null
                    if ($LASTEXITCODE -ne 0) {
                        throw "dotnet publish failed for project: $project ($tfm)"
                    }

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

            $snapshotPath = Join-Path $snapshotsRoot "$name.$tfm.dcvr"
            if (Test-Path -LiteralPath $snapshotPath) {
                # A stale file from an earlier, possibly-successful run must not survive to mask a
                # genuine failure of *this* run if the size/existence check below is ever skipped.
                Remove-Item -LiteralPath $snapshotPath -Force
            }

            # T3.10's proven-working invocation: --snapshot-output only, no --xml-report-output
            # (that path throws "Snapshot container is not initialized" on both TFMs -- see this
            # script's top-level .DESCRIPTION). --exclude-processes is load-bearing, not optional:
            # dropping it reverts to the multi-minute VBCSCompiler hang T3.10 documented, so it is
            # fixed here, not exposed as a parameter. --target-working-directory is required too
            # (found verifying this script): its default is the *target executable's* directory,
            # not the caller's cwd, so a relative csproj path fails MSB1009 without it.
            $dotCoverArgs = @(
                'cover',
                '--snapshot-output', $snapshotPath,
                '--target-working-directory', $repoRoot,
                '--exclude-assemblies', '*.UnitTests',
                '--exclude-processes', "VBCSCompiler$exeSuffix",
                '--exclude-processes', "MSBuild$exeSuffix",
                '--',
                'test', $targetToTest,
                '-c', $Configuration,
                '-f', $tfm
            )

            if (-not [string]::IsNullOrWhiteSpace($Filter)) {
                $dotCoverArgs += @('--filter', $Filter)
            }

            & dotnet dotCover @dotCoverArgs
            $dotCoverExitCode = $LASTEXITCODE

            if ($Isolated) {
                Remove-Item -Path $cleanupTempDir -Recurse -Force -ErrorAction SilentlyContinue
            }

            if ($dotCoverExitCode -ne 0) {
                throw "dotCover cover failed (exit $dotCoverExitCode) for project: $project ($tfm)"
            }

            $snapshotItem = Get-Item -LiteralPath $snapshotPath -ErrorAction SilentlyContinue
            if ($null -eq $snapshotItem -or $snapshotItem.Length -lt 1024) {
                throw "dotCover reported success but no valid snapshot was produced at: $snapshotPath"
            }

            Write-Host "  Snapshot: $snapshotPath ($($snapshotItem.Length) bytes)" -ForegroundColor Green
            $snapshotFiles += $snapshotPath
        }
    }
}
finally {
    Pop-Location
}

Write-Host ''
Write-Host 'dotCover snapshot collection complete.' -ForegroundColor Green
Write-Host 'This is the snapshot-only fallback (T3.10, docs/ai/plans/tools-review-and-standardisation.md' -ForegroundColor Green
Write-Host '## Baselines): dotCover 2025.3.3''s --xml-report-output path throws "Snapshot container is not' -ForegroundColor Green
Write-Host 'initialized" on both net8.0 and net10.0, so this engine never produces HTML or Cobertura --' -ForegroundColor Green
Write-Host 'only the raw .dcvr snapshot(s) below. Open each one in Rider''s coverage viewer for visual,' -ForegroundColor Green
Write-Host 'file-by-file coverage exploration.' -ForegroundColor Green
foreach ($snapshotFile in $snapshotFiles) {
    Write-Host "  $snapshotFile" -ForegroundColor Green
}
Write-Host ''
Write-Host 'Coverlet remains the only engine this repo''s automated coverage gate can enforce against' -ForegroundColor DarkGray
Write-Host '(Test-Coverage.ps1 -Engine DotCover explains why and throws rather than pretending to gate).' -ForegroundColor DarkGray
