<#
.SYNOPSIS
    Test Rule50 Unit Test File Structure Normalization

.DESCRIPTION
    Enforces unit test file normalization:
    - Every *Tests.cs must have a sibling *TestData.cs
    - Every *TestData.cs must have a sibling *Tests.cs
    - No [Fact] usage in *Tests.cs (Theory-only policy)

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER FailOnFindings
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
. (Join-Path $PSScriptRoot '..\helpers\Load-TestAuditExceptions.ps1')
. (Join-Path $PSScriptRoot 'Load-Catalog.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule50' } | Select-Object -First 1
if (-not $ruleInfo) { throw 'Catalog is missing Rule50.' }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule50' -Title 'Unit test file structure normalization (*Tests.cs ↔ *TestData.cs + Theory-only)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$exceptions = Import-PineGuardTestAuditExceptions -RepoRoot $repoRootResolved
$allowMissingTestData = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule50' -Key 'AllowMissingTestData'
$allowOrphanTestData = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule50' -Key 'AllowOrphanTestData'
$allowFact = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule50' -Key 'AllowFact'

$testsRoot = Join-Path $repoRootResolved 'tests'

function Test-IsInScopeTestsFile {
    param([string]$RelativePath)

    $p = $RelativePath
    if ($p -match '(?i)(^|/)bin(/|$)') { return $false }
    if ($p -match '(?i)(^|/)obj(/|$)') { return $false }
    if ($p.StartsWith('tests/', [System.StringComparison]::OrdinalIgnoreCase)) { return $true }
    return $false
}

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param([string]$Message)
    $findings.Add($Message) | Out-Null
}

$testFiles = @(Get-ChildItem -LiteralPath $testsRoot -Recurse -File -Filter '*Tests.cs')
$testDataFiles = @(Get-ChildItem -LiteralPath $testsRoot -Recurse -File -Filter '*TestData.cs')

# 1) Every *Tests.cs must have sibling *TestData.cs
foreach ($f in $testFiles) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $f.FullName
    if (-not (Test-IsInScopeTestsFile -RelativePath $rel)) { continue }

    $expected = Join-Path $f.Directory.FullName ($f.BaseName -replace 'Tests$', 'TestData')
    $expected = $expected + '.cs'

    $expectedRel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $expected

    if (-not (Test-Path -LiteralPath $expected)) {
        if (-not $allowMissingTestData.Contains($rel)) {
            Add-Finding ("[MissingTestData] {0} is missing sibling {1}" -f $rel, $expectedRel)
        }
    }

    $text = Get-Content -LiteralPath $f.FullName -Raw
    if ($text -match '(?s)\[\s*Fact(\s*\(|\s*\])') {
        if (-not $allowFact.Contains($rel)) {
            Add-Finding ("[FactNotAllowed] {0} contains [Fact]; only [Theory] is allowed" -f $rel)
        }
    }
}

# 2) Every *TestData.cs must have sibling *Tests.cs
foreach ($f in $testDataFiles) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $f.FullName
    if (-not (Test-IsInScopeTestsFile -RelativePath $rel)) { continue }

    $expected = Join-Path $f.Directory.FullName ($f.BaseName -replace 'TestData$', 'Tests')
    $expected = $expected + '.cs'

    $expectedRel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $expected

    if (-not (Test-Path -LiteralPath $expected)) {
        if (-not $allowOrphanTestData.Contains($rel)) {
            Add-Finding ("[OrphanTestData] {0} is missing sibling {1}" -f $rel, $expectedRel)
        }
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$header = @(
    'Rule50 - Unit Test File Structure Normalization',
    ("RepoRoot: {0}" -f $repoRootResolved),
    ("TestsRoot: {0}" -f $testsRoot),
    ("Findings: {0}" -f $findings.Count),
    'Exceptions: tools/audit-cli/test-audit-exceptions.json',
    ''
)

$header | Out-File -LiteralPath $reportPath -Encoding utf8

if ($findings.Count -eq 0) {
    'PASS' | Add-Content -LiteralPath $reportPath
    Write-Host 'Rule50: PASS' -ForegroundColor Green
    exit 0
}

$findings | Sort-Object | Add-Content -LiteralPath $reportPath
Write-Host ("Rule50: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red

if ($FailOnFindings.IsPresent) { exit 1 }
exit 0
