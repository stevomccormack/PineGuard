<#
.SYNOPSIS
    Test Rule51 Unit Test Class Semantic Structure

.DESCRIPTION
    Enforces unit test class semantic structure:
    - Tests must be organized into nested static classes (one group per method/operation)
    - No [Theory]/[Fact] methods declared directly on the outer *Tests class
    - Test groups containing tests must be declared as public static class

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule51' } | Select-Object -First 1
if (-not $ruleInfo) { throw 'Catalog is missing Rule51.' }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule51' -Title 'Unit test class semantic structure (nested static groups; no top-level test methods)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$exceptions = Import-PineGuardTestAuditExceptions -RepoRoot $repoRootResolved
$allowMissingNested = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule51' -Key 'AllowMissingNestedStaticGroups'
$allowTopLevelMethods = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule51' -Key 'AllowTopLevelTestMethods'
$allowNonStaticNested = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule51' -Key 'AllowNonStaticNestedTestGroups'

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
    param([string]$RelativePath, [string]$Message)
    $findings.Add("[$RelativePath] $Message") | Out-Null
}

$testFiles = @(Get-ChildItem -LiteralPath $testsRoot -Recurse -File -Filter '*Tests.cs')

foreach ($f in $testFiles) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $f.FullName
    if (-not (Test-IsInScopeTestsFile -RelativePath $rel)) { continue }

    $lines = Get-Content -LiteralPath $f.FullName

    $nesting = 0
    $outerDepth = $null
    $hasNestedStaticGroup = $false

    $currentGroupName = ''
    $currentGroupIsStatic = $false
    $groupTestCounts = @{}

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        $trim = $line.Trim()

        # Detect outer test class
        if ($null -eq $outerDepth -and $trim -match '\bclass\s+\w+Tests\b') {
            $outerDepth = $nesting
        }

        # Detect nested group declarations
        if ($null -ne $outerDepth -and $nesting -ge ($outerDepth + 1)) {
            if ($trim -match '^public\s+static\s+class\s+(?<n>\w+)\b') {
                $hasNestedStaticGroup = $true
                $currentGroupName = $Matches['n']
                $currentGroupIsStatic = $true
                if (-not $groupTestCounts.ContainsKey($currentGroupName)) { $groupTestCounts[$currentGroupName] = 0 }
            }
            elseif ($trim -match '^public\s+class\s+(?<n>\w+)\b' -or $trim -match '^internal\s+class\s+(?<n>\w+)\b') {
                $currentGroupName = $Matches['n']
                $currentGroupIsStatic = $false
                if (-not $groupTestCounts.ContainsKey($currentGroupName)) { $groupTestCounts[$currentGroupName] = 0 }
            }
        }

        $isTestAttribute = ($trim -match '^\[\s*(Theory|Fact)\b')

        if ($isTestAttribute) {
            $isTopLevel = ($null -ne $outerDepth -and $nesting -eq ($outerDepth + 1))

            if ($isTopLevel -and (-not $allowTopLevelMethods.Contains($rel))) {
                Add-Finding -RelativePath $rel -Message ("Top-level test attribute found; tests must be inside nested static groups: '{0}'" -f $trim)
            }

            if (-not [string]::IsNullOrWhiteSpace($currentGroupName)) {
                $groupTestCounts[$currentGroupName] = [int]$groupTestCounts[$currentGroupName] + 1

                if (-not $currentGroupIsStatic -and (-not $allowNonStaticNested.Contains($rel))) {
                    Add-Finding -RelativePath $rel -Message ("Nested test group '{0}' contains tests but is not declared 'public static class'." -f $currentGroupName)
                }
            }
        }

        # Update nesting based on braces (heuristic)
        $openCount = ([regex]::Matches($line, '{')).Count
        $closeCount = ([regex]::Matches($line, '}')).Count
        $nesting += $openCount
        $nesting -= $closeCount

        if ($nesting -lt 0) { $nesting = 0 }

        # Reset group when exiting a nested class scope (heuristic: on a close brace at outer+1 depth)
        if ($closeCount -gt 0 -and $null -ne $outerDepth -and $nesting -le ($outerDepth + 1)) {
            $currentGroupName = ''
            $currentGroupIsStatic = $false
        }
    }

    if (-not $hasNestedStaticGroup -and (-not $allowMissingNested.Contains($rel))) {
        Add-Finding -RelativePath $rel -Message 'No nested public static class groups found; group tests by method/operation using nested static classes.'
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$header = @(
    'Rule51 - Unit Test Class Semantic Structure',
    ("RepoRoot: {0}" -f $repoRootResolved),
    ("TestsRoot: {0}" -f $testsRoot),
    ("Findings: {0}" -f $findings.Count),
    'Exceptions: tools/audit-cli/test-audit-exceptions.json',
    ''
)

$header | Out-File -LiteralPath $reportPath -Encoding utf8

if ($findings.Count -eq 0) {
    'PASS' | Add-Content -LiteralPath $reportPath
    Write-Host 'Rule51: PASS' -ForegroundColor Green
    exit 0
}

$findings | Sort-Object | Add-Content -LiteralPath $reportPath
Write-Host ("Rule51: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red

if ($FailOnFindings.IsPresent) { exit 1 }
exit 0
