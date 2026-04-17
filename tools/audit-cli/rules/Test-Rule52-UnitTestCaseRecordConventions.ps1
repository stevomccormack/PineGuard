<#
.SYNOPSIS
    Test Rule52 Unit TestCase Record Conventions

.DESCRIPTION
    Enforces unit test case record conventions in *TestData.cs:
    - ValidCase records must inherit from a shared case base (must have ': Base(...)')
    - ValidCase must not inherit directly from BaseCase or ValueCase<>
    - (Heuristic) Other case records should not inherit directly from BaseCase or ValueCase<> unless allowlisted

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule52' } | Select-Object -First 1
if (-not $ruleInfo) { throw 'Catalog is missing Rule52.' }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule52' -Title 'Unit test case record conventions (ValidCase inheritance; avoid BaseCase/ValueCase)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$exceptions = Import-PineGuardTestAuditExceptions -RepoRoot $repoRootResolved
$allowValidBaseCase = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule52' -Key 'AllowValidCaseBaseCase'
$allowValidValueCase = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule52' -Key 'AllowValidCaseValueCase'
$allowOtherBaseCase = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule52' -Key 'AllowOtherCaseBaseCase'
$allowOtherValueCase = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule52' -Key 'AllowOtherCaseValueCase'

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

$testDataFiles = @(Get-ChildItem -LiteralPath $testsRoot -Recurse -File -Filter '*TestData.cs')

function Get-RecordHeader {
    param(
        [string[]]$Lines,
        [int]$StartIndex
    )

    $acc = New-Object System.Text.StringBuilder
    for ($j = $StartIndex; $j -lt $Lines.Count; $j++) {
        $line = $Lines[$j]
        [void]$acc.AppendLine($line)

        if ($line -match ';\s*$') {
            break
        }

        # Stop if we hit an opening brace (record body style) and there's no primary ctor
        if ($line -match '{\s*$') {
            break
        }

        if (($j - $StartIndex) -ge 15) { break }
    }

    return $acc.ToString()
}

function Get-BaseTypeName {
    param([string]$Header)

    # Try to pull the base type token after ':'
    $m = [regex]::Match($Header, ':(?<rest>[^;{]+)')
    if (-not $m.Success) { return '' }

    $rest = $m.Groups['rest'].Value.Trim()
    if ([string]::IsNullOrWhiteSpace($rest)) { return '' }

    # Base might be like "ReturnCase<string, bool>(...)" or "ValueCase<string>(...)"
    $m2 = [regex]::Match($rest, '^(?<t>[A-Za-z_][A-Za-z0-9_]*)(?<gen>\s*<[^>]+>)?\s*\(')
    if ($m2.Success) {
        return ($m2.Groups['t'].Value + $m2.Groups['gen'].Value).Trim()
    }

    # Fallback: token up to whitespace or '(' 
    $m3 = [regex]::Match($rest, '^(?<t>[^\s\(]+)')
    if ($m3.Success) { return $m3.Groups['t'].Value.Trim() }

    return ''
}

foreach ($f in $testDataFiles) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $f.FullName
    if (-not (Test-IsInScopeTestsFile -RelativePath $rel)) { continue }

    $lines = Get-Content -LiteralPath $f.FullName

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]

        if ($line -notmatch '\brecord\s+\w+') { continue }

        $header = Get-RecordHeader -Lines $lines -StartIndex $i

        # Detect ValidCase record
        if ($header -match '\brecord\s+ValidCase\b') {
            if ($header -notmatch ':') {
                Add-Finding -RelativePath $rel -Message 'ValidCase record must inherit from a shared case base (missing inheritance clause).'
                continue
            }

            $baseName = Get-BaseTypeName -Header $header
            if ([string]::IsNullOrWhiteSpace($baseName)) {
                Add-Finding -RelativePath $rel -Message 'ValidCase record inheritance clause could not be parsed.'
                continue
            }

            if ($baseName -match '^BaseCase\b') {
                if (-not $allowValidBaseCase.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("ValidCase must not inherit from BaseCase directly (found: {0})." -f $baseName)
                }
            }
            elseif ($baseName -match '^ValueCase\b') {
                if (-not $allowValidValueCase.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("ValidCase must not inherit from ValueCase<> directly (found: {0})." -f $baseName)
                }
            }
        }

        # Heuristic: other case records inheriting from BaseCase/ValueCase
        if ($header -match '\brecord\s+(?<n>\w+Case)\b' -and $header -match ':') {
            $caseName = $Matches['n']
            $baseName = Get-BaseTypeName -Header $header

            if ($baseName -match '^BaseCase\b') {
                if (-not $allowOtherBaseCase.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("{0} should not inherit from BaseCase directly (found: {1})." -f $caseName, $baseName)
                }
            }
            elseif ($baseName -match '^ValueCase\b') {
                if (-not $allowOtherValueCase.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("{0} should not inherit from ValueCase<> directly (found: {1})." -f $caseName, $baseName)
                }
            }
        }
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$header = @(
    'Rule52 - Unit TestCase Record Conventions',
    ("RepoRoot: {0}" -f $repoRootResolved),
    ("TestsRoot: {0}" -f $testsRoot),
    ("Findings: {0}" -f $findings.Count),
    'Exceptions: tools/audit-cli/test-audit-exceptions.json',
    ''
)

$header | Out-File -LiteralPath $reportPath -Encoding utf8

if ($findings.Count -eq 0) {
    'PASS' | Add-Content -LiteralPath $reportPath
    Write-Host 'Rule52: PASS' -ForegroundColor Green
    exit 0
}

$findings | Sort-Object | Add-Content -LiteralPath $reportPath
Write-Host ("Rule52: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red

if ($FailOnFindings.IsPresent) { exit 1 }
exit 0
