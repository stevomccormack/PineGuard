<#
.SYNOPSIS
    Run Code Coverage

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER Mode
    See the param block for details.

.PARAMETER Scope
    See the param block for details.

.PARAMETER Configuration
    See the param block for details.

.PARAMETER Clean
    See the param block for details.

.PARAMETER NoOpen
    See the param block for details.

.PARAMETER SkipHtml
    See the param block for details.

.PARAMETER ProjectFilter
    See the param block for details.

.PARAMETER Top
    See the param block for details.

.PARAMETER IncludeFileRegex
    See the param block for details.

.PARAMETER ExcludeFileRegex
    See the param block for details.

.PARAMETER IncludeClassNameRegex
    See the param block for details.

.PARAMETER ExcludeClassNameRegex
    See the param block for details.

.PARAMETER FailCoverageBelow
    See the param block for details.

.PARAMETER FailBranchBelow
    See the param block for details.

.PARAMETER Enforce100
    See the param block for details.

.PARAMETER Isolated
    See the param block for details.

.PARAMETER Relaxed
    See the param block for details.

.PARAMETER Filter
    See the param block for details.

.PARAMETER Framework
    See the param block for details.

.PARAMETER Format
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateSet('Generate', 'Analyze', 'GenerateAndAnalyze')] [string] $Mode = 'GenerateAndAnalyze',
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'All', 'Testing')] [string] $Scope = 'Core',
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

$utilityPath = Join-Path $PSScriptRoot 'Import-CodeCoverageUtility.ps1'
if (-not (Test-Path $utilityPath)) {
    throw "Import-CodeCoverageUtility.ps1 not found at: $utilityPath"
}

. $utilityPath

# Every real (non-aggregate) scope is held to 100% unless -Relaxed says otherwise. Derived from
# the registry rather than a literal list so a newly registered scope cannot silently opt out.
$registryScopeNames = @(Get-PineGuardScope -All | ForEach-Object Name)

if ($Scope -in $registryScopeNames) {
    if (-not $Relaxed) {
        $Enforce100 = $true
    }
}

$xplatGenerate = Join-Path $PSScriptRoot 'xplat\Gen-CoverageReport.ps1'
$xplatAnalyze = Join-Path $PSScriptRoot 'xplat\Test-CoverageAnalysis.ps1'

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
        Configuration = $Configuration
        Scope         = $Scope
        Clean         = $Clean
        NoOpen        = $NoOpen
        SkipHtml      = $SkipHtml
        ProjectFilter = $effectiveProjectFilter
        Isolated      = $Isolated
        Filter        = $Filter
        Framework     = $Framework
    }
    if ($PSBoundParameters.ContainsKey('Format')) {
        $generateParams['Format'] = $Format
    }
    & $xplatGenerate @generateParams
}

if ($Mode -in @('Analyze', 'GenerateAndAnalyze')) {
    & $xplatAnalyze -Scope $Scope -Top $Top -IncludeFileRegex $IncludeFileRegex -ExcludeFileRegex $ExcludeFileRegex -IncludeClassNameRegex $IncludeClassNameRegex -ExcludeClassNameRegex $ExcludeClassNameRegex -FailCoverageBelow $FailCoverageBelow -FailBranchBelow $FailBranchBelow -Enforce100:$Enforce100
}
