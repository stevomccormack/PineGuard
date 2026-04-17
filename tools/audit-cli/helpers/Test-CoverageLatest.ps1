<#
.SYNOPSIS
    Analyze latest coverage report (Cobertura).

.DESCRIPTION
    Locates the latest `coverage.cobertura.xml` under a search root and reports any classes
    with less than 100% line or branch coverage.

.PARAMETER AuditRuleId
    Identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER SearchPath
    Root directory to search under (relative to RepoRoot unless rooted).

.PARAMETER CoverageFileName
    Coverage file name to search for.

.PARAMETER TargetFilter
    Wildcard filter applied to FullName to narrow which coverage file(s) to consider.

.PARAMETER OutputPath
    Output report path (relative to RepoRoot unless rooted).
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Util01',
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [ValidateNotNullOrEmpty()] [string]$SearchPath = 'artifacts',
    [ValidateNotNullOrEmpty()] [string]$CoverageFileName = 'coverage.cobertura.xml',
    [ValidateNotNullOrEmpty()] [string]$TargetFilter = '*DataAnnotations*',
    [ValidateNotNullOrEmpty()] [string]$OutputPath = 'artifacts/audit/util/Util01-latest-coverage-analysis.txt'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$searchPathResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $SearchPath
$outputPathResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $OutputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $outputPathResolved)

Write-PineGuardAuditHeader -AuditRuleId $AuditRuleId -Title 'Analyze latest coverage (Cobertura)' -RepoRoot $repoRootResolved -OutputPath $OutputPath
Write-Host "Searching for '$CoverageFileName' in: $searchPathResolved" -ForegroundColor DarkGray

$allFiles = @(Get-ChildItem -Path $searchPathResolved -Filter $CoverageFileName -Recurse -File)
$files = @($allFiles | Where-Object { $_.FullName -like $TargetFilter })

if ($files.Count -eq 0) {
    $message = "No '$CoverageFileName' files found matching '$TargetFilter'."
    $reportLines = @(
        "AuditRule: $AuditRuleId - Analyze latest coverage (Cobertura)",
        "RepoRoot : $repoRootResolved",
        "SearchPath: $searchPathResolved",
        "Filter   : $TargetFilter",
        "Message  : $message",
        ''
    )

    $reportLines | Set-Content -Path $outputPathResolved

    Write-Warning $message
    Write-Host "Report written to: $outputPathResolved" -ForegroundColor DarkGray
    return
}

$latestFile = $files | Sort-Object LastWriteTime -Descending | Select-Object -First 1

Write-Host "Latest coverage file: $($latestFile.FullName)" -ForegroundColor Cyan
Write-Host "Last Modified      : $($latestFile.LastWriteTime)" -ForegroundColor DarkGray

[xml]$coverage = Get-Content $latestFile.FullName

$incomplete = New-Object System.Collections.Generic.List[object]

foreach ($package in $coverage.coverage.packages.package) {
    foreach ($class in $package.classes.class) {
        $lineRate = [double]$class.'line-rate'
        $branchRate = [double]$class.'branch-rate'

        if ($lineRate -lt 1.0 -or $branchRate -lt 1.0) {
            $incomplete.Add([PSCustomObject]@{
                Name       = $class.name
                LineRate   = $lineRate
                BranchRate = $branchRate
                Filename   = $class.filename
            }) | Out-Null
        }
    }
}

$reportHeader = @(
    "AuditRule: $AuditRuleId - Analyze latest coverage (Cobertura)",
    "RepoRoot : $repoRootResolved",
    "Coverage : $($latestFile.FullName)",
    "Modified : $($latestFile.LastWriteTime)",
    "Under100 : $($incomplete.Count)",
    ''
)

$reportHeader | Set-Content -Path $outputPathResolved

if ($incomplete.Count -eq 0) {
    Write-Host 'All classes have 100% line + branch coverage.' -ForegroundColor Green
    'All classes have 100% line + branch coverage.' | Add-Content -Path $outputPathResolved
}
else {
    Write-Host 'Classes with < 100% coverage:' -ForegroundColor Yellow
    $incomplete | Sort-Object Name | Format-Table -AutoSize | Out-String | Add-Content -Path $outputPathResolved
    $incomplete | Sort-Object Name | Format-Table -AutoSize
}

Write-Host "Report written to: $outputPathResolved" -ForegroundColor DarkGray
