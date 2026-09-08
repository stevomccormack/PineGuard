<#
.SYNOPSIS
    New Coverage Report (D-9 engine front door)

.DESCRIPTION
    Single entry point for coverage report generation across both supported engines (D-9/§3.5).
    Validates -Engine and delegates to the matching engine folder's New-CoverageReport.ps1,
    forwarding every other bound parameter unchanged (whatever the caller did not explicitly
    pass is left for the delegate script's own defaults to resolve, so those defaults only ever
    live in one place). It holds no collection logic of its own: test execution, coverage
    collection, the ReportGenerator call, and the per-scope Clean all live in the engine-named
    scripts -- coverlet/New-CoverageReport.ps1 and dotcover/New-CoverageReport.ps1.

    Run-CodeCoverage.ps1 calls this front door; it never calls an engine script directly.

.PARAMETER Engine
    Coverlet (default) or DotCover. DotCover delegates to dotcover/New-CoverageReport.ps1, T3.11's
    snapshot-only fallback (see that script's own .DESCRIPTION and T3.10's findings in
    docs/ai/plans/tools-review-and-standardisation.md ## Baselines for why it collects a
    Rider .dcvr snapshot instead of HTML/Cobertura).

.PARAMETER Configuration
    Forwarded to the selected engine script. See coverlet/New-CoverageReport.ps1 for its meaning
    and default.

.PARAMETER Scope
    Forwarded to the selected engine script.

.PARAMETER Clean
    Forwarded to the selected engine script.

.PARAMETER NoOpen
    Forwarded to the selected engine script.

.PARAMETER SkipHtml
    Forwarded to the selected engine script.

.PARAMETER ProjectFilter
    Forwarded to the selected engine script.

.PARAMETER Filter
    Forwarded to the selected engine script.

.PARAMETER Isolated
    Forwarded to the selected engine script.

.PARAMETER Format
    Forwarded to the selected engine script.

.PARAMETER Framework
    Forwarded to the selected engine script.
#>

[CmdletBinding()]
param(
    [ValidateSet('Coverlet', 'DotCover')] [string] $Engine = 'Coverlet',
    [ValidateSet('Debug', 'Release')] [string] $Configuration,
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'All', 'Testing')] [string] $Scope,
    [switch] $Clean,
    [switch] $NoOpen,
    [switch] $SkipHtml,
    [string] $ProjectFilter,
    [string] $Filter,
    [switch] $Isolated,
    [ValidateSet('cobertura', 'opencover')] [string] $Format,
    [string] $Framework
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

$engineScriptPaths = @{
    Coverlet = Join-Path $PSScriptRoot 'coverlet' 'New-CoverageReport.ps1'
    DotCover = Join-Path $PSScriptRoot 'dotcover' 'New-CoverageReport.ps1'
}

$engineScript = $engineScriptPaths[$Engine]
if (-not (Test-Path -LiteralPath $engineScript)) {
    throw "$Engine engine script not found at: $engineScript"
}

# Delegates only (D-9): forward exactly what the caller bound, nothing more. Anything the
# caller left unset is resolved by the engine script's own defaults, not duplicated here.
$forwardParams = @{}
foreach ($key in $PSBoundParameters.Keys) {
    if ($key -eq 'Engine') { continue }
    $forwardParams[$key] = $PSBoundParameters[$key]
}

& $engineScript @forwardParams
