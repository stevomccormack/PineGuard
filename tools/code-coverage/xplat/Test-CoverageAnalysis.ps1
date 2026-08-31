<#
.SYNOPSIS
    Test Coverage Analysis

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER Top
    See the param block for details.

.PARAMETER Scope
    See the param block for details.

.PARAMETER IncludeFileRegex
    See the param block for details.

.PARAMETER ExcludeFileRegex
    See the param block for details.

.PARAMETER IncludeClassNameRegex
    See the param block for details.

.PARAMETER ExcludeClassNameRegex
    See the param block for details.

.PARAMETER ResultsRoot
    See the param block for details.

.PARAMETER OpenHtml
    See the param block for details.

.PARAMETER Enforce100
    See the param block for details.

.PARAMETER FailCoverageBelow
    See the param block for details.

.PARAMETER FailBranchBelow
    See the param block for details.

.PARAMETER AsTable
    See the param block for details.

.PARAMETER Isolated
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateRange(1, 500)] [int] $Top = 30,
    [ValidateSet('Core', 'MustClauses', 'GuardClauses', 'DataAnnotations', 'FluentValidation', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'All', 'Custom', 'Testing')] [string] $Scope = 'Core',
    [string] $IncludeFileRegex,
    [string] $ExcludeFileRegex,
    [string] $IncludeClassNameRegex,
    [string] $ExcludeClassNameRegex,
    [string] $ResultsRoot,
    [switch] $OpenHtml,
    [switch] $Enforce100,
    [ValidateRange(0.0, 100.0)] [double] $FailCoverageBelow = 0.0,
    [ValidateRange(0.0, 100.0)] [double] $FailBranchBelow = 0.0,
    [switch] $AsTable,
    [switch] $Isolated
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

$utilityPath = Join-Path $PSScriptRoot '..\Import-CodeCoverageUtility.ps1'
if (-not (Test-Path $utilityPath)) {
    throw "Import-CodeCoverageUtility.ps1 not found at: $utilityPath"
}

. $utilityPath

$repoRoot = Get-RepoRoot

if ($Scope -notin @('All', 'Custom')) {
    $scopeSourceDir = Join-Path $repoRoot (Get-PineGuardScope -Name $Scope).SourceDir

    if (-not [string]::IsNullOrWhiteSpace($scopeSourceDir) -and (Test-Path $scopeSourceDir)) {
        $anyCs = Get-ChildItem -LiteralPath $scopeSourceDir -Recurse -File -Filter '*.cs' -ErrorAction SilentlyContinue |
        Where-Object { $_.FullName -notmatch '([\\/])(bin|obj)\1' } |
        Select-Object -First 1
        if ($null -eq $anyCs) {
            Write-Warning "No *.cs files found under '$scopeSourceDir' for Scope='$Scope'. Skipping coverage analysis."
            return
        }
    }
}

if ([string]::IsNullOrWhiteSpace($ResultsRoot)) {
    $ResultsRoot = Join-Path $repoRoot 'artifacts/code-coverage/xplat/testresults'
}

$defaultSourcePrefix = 'src\PineGuard.Core'

switch ($Scope) {
    'All' {
        if (-not $IncludeFileRegex) {
            # 'All' = the union of the seven per-scope path filters. Each PathIncludeRegex is
            # self-anchored (^src... / ^tests...), so a plain '|' join is the exact aggregate —
            # and scopes whose folder is not 'PineGuard.<Name>' (Options ->
            # PineGuard.Extensions.Options) stay correct because the registry regex, not the
            # scope Name, is the source.
            $IncludeFileRegex = (Get-PineGuardScope -All | ForEach-Object PathIncludeRegex) -join '|'
        }
        if (-not $ExcludeFileRegex) { $ExcludeFileRegex = '(^|[/\\])obj[/\\]' }
        $defaultSourcePrefix = (Get-PineGuardScope -Name 'Core').DefaultSourcePrefix
    }
    'Custom' {
        # No defaults.
    }
    default {
        # Core, MustClauses, GuardClauses, DataAnnotations, FluentValidation, Testing
        $scopeEntry = Get-PineGuardScope -Name $Scope
        if (-not $IncludeFileRegex) { $IncludeFileRegex = $scopeEntry.PathIncludeRegex }
        if (-not $ExcludeFileRegex) { $ExcludeFileRegex = '(^|[/\\])obj[/\\]' }
        $defaultSourcePrefix = $scopeEntry.DefaultSourcePrefix
    }
}

Write-Host "Repo root: $repoRoot" -ForegroundColor DarkGray
Write-Host "Coverage results: $ResultsRoot" -ForegroundColor DarkGray
Write-Host "Scope: $Scope" -ForegroundColor DarkGray

$coverageFiles = Get-LatestCoverageFiles -ResultsRoot $ResultsRoot

if ($Isolated) {
    Write-Host "Isolated mode: Copying coverage files to temp to avoid locking..." -ForegroundColor Cyan
    $isoDir = Join-Path $env:TEMP "PineGuard-Coverage-Analyze-$([Guid]::NewGuid())"
    Ensure-Directory $isoDir
    
    $isoFiles = @()
    foreach ($file in $coverageFiles) {
        $dest = Join-Path $isoDir (Split-Path $file -Leaf)
        Copy-Item -LiteralPath $file -Destination $dest -Force
        $isoFiles += $dest
    }
    $coverageFiles = $isoFiles
}



$classes = Read-CoberturaCoverage -CoverageFiles $coverageFiles -RepoRoot $repoRoot -IncludeFileRegex $IncludeFileRegex -ExcludeFileRegex $ExcludeFileRegex -IncludeClassNameRegex $IncludeClassNameRegex -ExcludeClassNameRegex $ExcludeClassNameRegex -DefaultSourcePrefix $defaultSourcePrefix



if (-not $classes -or $classes.Count -eq 0) {
    throw "No covered classes matched the include/exclude filters. IncludeFileRegex='$IncludeFileRegex' ExcludeFileRegex='$ExcludeFileRegex'"
}

Write-Host "Found $($classes.Count) classes matching filters." -ForegroundColor Cyan

[int]$totalLines = ($classes | Measure-Object -Property LinesTotal -Sum).Sum
[int]$coveredLines = ($classes | Measure-Object -Property LinesCovered -Sum).Sum
[int]$totalBranches = ($classes | Measure-Object -Property BranchesTotal -Sum).Sum
[int]$coveredBranches = ($classes | Measure-Object -Property BranchesCovered -Sum).Sum

$lineRate = if ($totalLines -gt 0) { [double]$coveredLines / [double]$totalLines } else { 0.0 }
$branchRate = if ($totalBranches -gt 0) { [double]$coveredBranches / [double]$totalBranches } else { 0.0 }

Write-Host ''
Write-Host 'Coverage summary (filtered scope):' -ForegroundColor Cyan
Write-Host ("  Line coverage:   {0:P2} ({1}/{2})" -f $lineRate, $coveredLines, $totalLines)
Write-Host ("  Branch coverage: {0:P2} ({1}/{2})" -f $branchRate, $coveredBranches, $totalBranches)

Write-Host ''
Write-Host ("Lowest-covered classes (Top {0}):" -f $Top) -ForegroundColor Cyan

$bottom = $classes |
Sort-Object BranchRate, LineRate, Name |
Select-Object -First $Top LineRate, BranchRate, LinesCovered, LinesTotal, BranchesCovered, BranchesTotal, Name, File

if ($AsTable) {
    $bottom |
    Select-Object @{Name = 'Line%'; Expression = { "{0:P2}" -f $_.LineRate } }, @{Name = 'Branch%'; Expression = { "{0:P2}" -f $_.BranchRate } }, LinesCovered, LinesTotal, BranchesCovered, BranchesTotal, Name, File |
    Format-Table -AutoSize
}
else {
    Write-Host 'Line%\tBranch%\tLines\tBranches\tClass\tFile'
    foreach ($row in $bottom) {
        $linePct = ("{0:P2}" -f $row.LineRate)
        $branchPct = ("{0:P2}" -f $row.BranchRate)
        $lines = ("{0}/{1}" -f $row.LinesCovered, $row.LinesTotal)
        $branches = ("{0}/{1}" -f $row.BranchesCovered, $row.BranchesTotal)
        Write-Host ("{0}\t{1}\t{2}\t{3}\t{4}\t{5}" -f $linePct, $branchPct, $lines, $branches, $row.Name, $row.File)
    }
}

if ($OpenHtml) {
    $redirectPath = Join-Path $repoRoot "artifacts/code-coverage/xplat-$($Scope.ToLower())-report.html"
    if (Test-Path $redirectPath) {
        Write-Host ''
        Write-Host "Opening HTML report: $redirectPath" -ForegroundColor Cyan
        Start-Process -FilePath $redirectPath | Out-Null
    }
    else {
        Write-Warning "HTML report not found at: $redirectPath"
    }
}

if ($Enforce100) {
    $FailCoverageBelow = 1.0
    $FailBranchBelow = 1.0
}

$requiredLine = ConvertTo-Rate -Value $FailCoverageBelow
$requiredBranch = ConvertTo-Rate -Value $FailBranchBelow

$shouldEnforce = ($requiredLine -gt 0.0) -or ($requiredBranch -gt 0.0)
if ($shouldEnforce) {
    $lineOk = ($lineRate -ge $requiredLine)
    $branchOk = ($totalBranches -eq 0) -or ($branchRate -ge $requiredBranch)

    if (-not $lineOk -or -not $branchOk) {
        Write-Error "Coverage below threshold for the filtered scope. RequiredLine=$requiredLine RequiredBranch=$requiredBranch LineRate=$lineRate BranchRate=$branchRate"
        exit 1
    }
}
