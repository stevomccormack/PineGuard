<#
.SYNOPSIS
    Run Code Coverage (the coverage entry point)

.DESCRIPTION
    The one script a developer or CI job calls to get coverage for a scope. It orchestrates the
    two halves of the pipeline and holds no collection or gating logic of its own: -Mode Generate
    delegates to the D-9 engine front door (New-CoverageReport.ps1), and -Mode Analyze delegates
    to the engine-agnostic gate (Test-Coverage.ps1). GenerateAndAnalyze, the default, runs both
    in that order.

    Its one piece of real behaviour is scope policy. Every scope in the registry
    (Get-PineGuardScope, tools/.shared/dotnet-projects.ps1) is held to 100% line and branch
    coverage automatically unless -Relaxed is passed, and an unbound -ProjectFilter resolves to
    that scope's own DefaultProjectFilter rather than the generic '*.UnitTests.csproj'. Both are
    derived from the registry rather than a literal list here, so a newly registered scope cannot
    silently opt out of the gate or drag unrelated test projects into the run.

.PARAMETER Mode
    Which half of the pipeline to run: Generate collects coverage via the D-9 front door
    (New-CoverageReport.ps1), Analyze runs only the gate (Test-Coverage.ps1) over whatever has
    already been collected, and GenerateAndAnalyze (default) does both in order.

.PARAMETER Engine
    Coverage engine: Coverlet (default) or DotCover. Forwarded to the D-9 front door
    (New-CoverageReport.ps1) and to the engine-agnostic gate (Test-Coverage.ps1). DotCover
    collects a Rider-native .dcvr snapshot only (T3.10's proven fallback; see
    dotcover/New-CoverageReport.ps1) -- -Mode Analyze/GenerateAndAnalyze then fails at the
    Test-Coverage.ps1 step, since there is no Cobertura output for it to gate on (by design; see
    that script's own -Engine DotCover error for why). Use -Mode Generate -Engine DotCover to
    collect the snapshot without hitting that gate.

.PARAMETER Scope
    Registry coverage scope -- see Get-PineGuardScope in tools/.shared/dotnet-projects.ps1 for
    the authoritative list -- or 'All' to run every *.UnitTests.csproj with no scope narrowing.
    Every real (non-aggregate) scope is held to 100% automatically unless -Relaxed says
    otherwise; the list is derived from the registry so a newly registered scope cannot silently
    opt out of the gate.

.PARAMETER Configuration
    Build configuration to test under: Debug (default) or Release. Forwarded to the engine
    script.

.PARAMETER Clean
    Delete this scope's previous output (its testresults/ and report/ folders) before
    collecting, so a stale run cannot be mistaken for a fresh one. Forwarded to the engine
    script.

.PARAMETER NoOpen
    Do not open the generated HTML report in the default browser once collection finishes.
    Forwarded to the engine script.

.PARAMETER SkipHtml
    Collect the coverage XML only and skip the ReportGenerator HTML step entirely. Useful when
    only the gate's numbers are wanted. Forwarded to the engine script.

.PARAMETER ProjectFilter
    Glob used to discover test projects. Left unbound, each scope's own DefaultProjectFilter
    from the registry is used instead ('*.UnitTests.csproj' for -Scope All), which keeps coverage
    loops fast by not running unrelated test projects.

.PARAMETER Top
    How many of the lowest-covered classes to list in the gate's summary. Defaults to 30;
    forwarded to Test-Coverage.ps1.

.PARAMETER IncludeFileRegex
    Restrict the analysed classes to those whose source file path matches this regex. Forwarded
    to Test-Coverage.ps1.

.PARAMETER ExcludeFileRegex
    Drop classes whose source file path matches this regex. Applied after -IncludeFileRegex and
    forwarded to Test-Coverage.ps1.

.PARAMETER IncludeClassNameRegex
    Restrict the analysed classes to those whose class name matches this regex. Forwarded to
    Test-Coverage.ps1.

.PARAMETER ExcludeClassNameRegex
    Drop classes whose class name matches this regex. Applied after -IncludeClassNameRegex and
    forwarded to Test-Coverage.ps1.

.PARAMETER FailCoverageBelow
    Minimum line coverage for the filtered scope, below which the run fails. Accepts either a
    percentage (90) or a rate (0.9). 0 (the default) means do not gate on line coverage.

.PARAMETER FailBranchBelow
    Minimum branch coverage for the filtered scope, below which the run fails. Accepts either a
    percentage (90) or a rate (0.9). 0 (the default) means do not gate on branch coverage.

.PARAMETER Enforce100
    Shorthand for -FailCoverageBelow 100 -FailBranchBelow 100. Set automatically for every real
    registry scope, so passing it explicitly only matters for -Scope All or alongside -Relaxed.

.PARAMETER Isolated
    Publish each test project to a temp directory before testing it, so an open IDE build cannot
    lock the source bin/ folders mid-run. Forwarded to the engine script.

.PARAMETER Relaxed
    Opt this run out of the automatic 100% enforcement that every real registry scope otherwise
    gets, leaving -FailCoverageBelow/-FailBranchBelow to decide. Intended for exploratory runs,
    never for the gate.

.PARAMETER Filter
    `dotnet test --filter` expression, forwarded to the engine script as-is.

.PARAMETER Framework
    Target framework moniker, forwarded to the engine script's `dotnet test -f`. Restricts
    collection to one TFM rather than all of them.

.PARAMETER Format
    Coverlet collector output format: cobertura (the default the engine script resolves) or
    opencover. Only forwarded when explicitly bound.
#>

[CmdletBinding()]
param(
    [ValidateSet('Generate', 'Analyze', 'GenerateAndAnalyze')] [string] $Mode = 'GenerateAndAnalyze',
    [ValidateSet('Coverlet', 'DotCover')] [string] $Engine = 'Coverlet',
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Xml', 'Analyzers', 'All', 'Testing')] [string] $Scope = 'Core',
    [ValidateSet('Debug', 'Release')] [string] $Configuration = 'Debug',
    [switch] $Clean,
    [switch] $NoOpen,
    [switch] $SkipHtml,
    [string] $ProjectFilter = '*.UnitTests.csproj',
    [ValidateRange(1, 500)] [int] $Top = 30,
    [string] $IncludeFileRegex,
    [string] $ExcludeFileRegex,
    [string] $IncludeClassNameRegex,
    [string] $ExcludeClassNameRegex,
    [ValidateRange(0.0, 100.0)] [double] $FailCoverageBelow = 0.0,
    [ValidateRange(0.0, 100.0)] [double] $FailBranchBelow = 0.0,
    [switch] $Enforce100,
    [switch] $Isolated,
    [switch] $Relaxed,
    [string] $Filter,
    [string] $Framework,
    [string] $Format
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

. (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')

# Every real (non-aggregate) scope is held to 100% unless -Relaxed says otherwise. Derived from
# the registry rather than a literal list so a newly registered scope cannot silently opt out.
$registryScopeNames = @(Get-PineGuardScope -All | ForEach-Object Name)

if ($Scope -in $registryScopeNames) {
    if (-not $Relaxed) {
        $Enforce100 = $true
    }
}

$coverageFrontDoor = Join-Path $PSScriptRoot 'New-CoverageReport.ps1'
$coverageGate = Join-Path $PSScriptRoot 'Test-Coverage.ps1'

# If the user didn't explicitly supply a ProjectFilter, prefer the tightest default per scope
# (keeps coverage loops fast and avoids running unrelated test projects).
$effectiveProjectFilter = $ProjectFilter
if (-not $PSBoundParameters.ContainsKey('ProjectFilter')) {
    if ($Scope -eq 'All') {
        $effectiveProjectFilter = '*.UnitTests.csproj'
    }
    else {
        $effectiveProjectFilter = (Get-PineGuardScope -Name $Scope).DefaultProjectFilter
    }
}

if ($Mode -in @('Generate', 'GenerateAndAnalyze')) {
    $generateParams = @{
        Engine = $Engine
        Configuration = $Configuration
        Scope = $Scope
        Clean = $Clean
        NoOpen = $NoOpen
        SkipHtml = $SkipHtml
        ProjectFilter = $effectiveProjectFilter
        Isolated = $Isolated
        Filter = $Filter
        Framework = $Framework
    }
    if ($PSBoundParameters.ContainsKey('Format')) {
        $generateParams['Format'] = $Format
    }
    & $coverageFrontDoor @generateParams
}

if ($Mode -in @('Analyze', 'GenerateAndAnalyze')) {
    & $coverageGate -Engine $Engine -Scope $Scope -Top $Top -IncludeFileRegex $IncludeFileRegex -ExcludeFileRegex $ExcludeFileRegex -IncludeClassNameRegex $IncludeClassNameRegex -ExcludeClassNameRegex $ExcludeClassNameRegex -FailCoverageBelow $FailCoverageBelow -FailBranchBelow $FailBranchBelow -Enforce100:$Enforce100
}
