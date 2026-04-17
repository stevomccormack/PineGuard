<#
.SYNOPSIS
    Test Rule54 Unit Test Tuple Conventions

.DESCRIPTION
    Enforces tuple usage conventions in *TestData.cs files (heuristic):
    - Named tuple element identifiers should be camelCase (e.g., (int min, int max))
    - Discourage tuple layering identifiers like Args/Arguments/Context unless allowlisted

    Notes:
    - This rule scans for tuple *type* declarations inside record primary ctor parameters.
    - This rule also scans for tuple literals that use named elements (e.g., (min: 1, max: 2)).
    - The checks are intentionally heuristic to keep the PowerShell implementation lightweight.

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule54' } | Select-Object -First 1
if (-not $ruleInfo) { throw 'Catalog is missing Rule54.' }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule54' -Title 'Unit test tuple conventions (camelCase element names; discourage layering)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$exceptions = Import-PineGuardTestAuditExceptions -RepoRoot $repoRootResolved
$allowPascalCaseTupleElements = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule54' -Key 'AllowPascalCaseTupleElements'
$allowTupleLayering = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule54' -Key 'AllowTupleLayering'

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
$findingSet = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

function Add-Finding {
    param([string]$RelativePath, [string]$Message)

    $row = "[$RelativePath] $Message"
    if ($findingSet.Add($row)) {
        $findings.Add($row) | Out-Null
    }
}

function Split-TopLevelCommas {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)][string]$Text)

    $parts = New-Object System.Collections.Generic.List[string]
    $acc = New-Object System.Text.StringBuilder

    $angle = 0
    $paren = 0
    $bracket = 0

    foreach ($ch in $Text.ToCharArray()) {
        switch ($ch) {
            '<' { $angle++; break }
            '>' { if ($angle -gt 0) { $angle-- }; break }
            '(' { $paren++; break }
            ')' { if ($paren -gt 0) { $paren-- }; break }
            '[' { $bracket++; break }
            ']' { if ($bracket -gt 0) { $bracket-- }; break }
            ',' {
                if ($angle -eq 0 -and $paren -eq 0 -and $bracket -eq 0) {
                    $parts.Add($acc.ToString()) | Out-Null
                    $acc.Clear() | Out-Null
                    break
                }
                break
            }
        }

        [void]$acc.Append($ch)
    }

    $parts.Add($acc.ToString()) | Out-Null
    return @($parts)
}

function Test-IsPascalCaseIdentifier {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)][string]$Name)

    if ([string]::IsNullOrWhiteSpace($Name)) { return $false }
    $first = $Name.Substring(0, 1)
    return ($first -cmatch '[A-Z]')
}

function Test-IsLayeringIdentifier {
    [CmdletBinding()]
    param([Parameter(Mandatory = $true)][string]$Name)

    switch ($Name) {
        'Args' { return $true }
        'Arguments' { return $true }
        'Context' { return $true }
        default { return $false }
    }
}

$testDataFiles = @(Get-ChildItem -LiteralPath $testsRoot -Recurse -File -Filter '*TestData.cs')

foreach ($f in $testDataFiles) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $f.FullName
    if (-not (Test-IsInScopeTestsFile -RelativePath $rel)) { continue }

    $text = Get-Content -LiteralPath $f.FullName -Raw
    if ([string]::IsNullOrWhiteSpace($text)) { continue }

    # 1) Tuple type declarations, usually in record primary ctor params:
    #    (int min, int max) Value
    $tupleTypeMatches = [regex]::Matches(
        $text,
        '\(\s*(?<inner>[^\)]*?\b[A-Za-z_][A-Za-z0-9_]*\s+[A-Za-z_][A-Za-z0-9_]*[^\)]*?)\s*\)\s+(?<var>[A-Za-z_][A-Za-z0-9_]*)',
        [System.Text.RegularExpressions.RegexOptions]::Singleline
    )

    foreach ($m in $tupleTypeMatches) {
        $inner = $m.Groups['inner'].Value
        if ([string]::IsNullOrWhiteSpace($inner)) { continue }

        $elements = Split-TopLevelCommas -Text $inner
        foreach ($e in $elements) {
            $element = ($e ?? '').Trim()
            if ([string]::IsNullOrWhiteSpace($element)) { continue }

            # Parse last identifier as tuple element name.
            $mName = [regex]::Match($element, '\b(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*$')
            if (-not $mName.Success) { continue }

            $name = $mName.Groups['name'].Value
            if (Test-IsPascalCaseIdentifier -Name $name) {
                if (-not $allowPascalCaseTupleElements.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("Tuple element name should be camelCase (found: '{0}')." -f $name)
                }
            }

            if (Test-IsLayeringIdentifier -Name $name) {
                if (-not $allowTupleLayering.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("Discouraged tuple layering identifier '{0}' (prefer flat tuple or dedicated record)." -f $name)
                }
            }
        }
    }

    # 2) Tuple literals with named elements (avoid matching Foo(bar: 1) named args by requiring
    #    the '(' not be preceded by an identifier character).
    $tupleLiteralMatches = [regex]::Matches(
        $text,
        '(?<![A-Za-z0-9_])\(\s*(?<inner>[^\)]*?:[^\)]*?)\)',
        [System.Text.RegularExpressions.RegexOptions]::Singleline
    )

    foreach ($m in $tupleLiteralMatches) {
        $inner = $m.Groups['inner'].Value
        if ([string]::IsNullOrWhiteSpace($inner)) { continue }

        $labelMatches = [regex]::Matches($inner, '\b(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*:')
        foreach ($lm in $labelMatches) {
            $name = $lm.Groups['name'].Value
            if ([string]::IsNullOrWhiteSpace($name)) { continue }

            if (Test-IsPascalCaseIdentifier -Name $name) {
                if (-not $allowPascalCaseTupleElements.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("Tuple literal element label should be camelCase (found: '{0}')." -f $name)
                }
            }

            if (Test-IsLayeringIdentifier -Name $name) {
                if (-not $allowTupleLayering.Contains($rel)) {
                    Add-Finding -RelativePath $rel -Message ("Discouraged tuple layering label '{0}' (prefer flat tuple or dedicated record)." -f $name)
                }
            }
        }
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$header = @(
    'Rule54 - Unit Test Tuple Conventions',
    ("RepoRoot: {0}" -f $repoRootResolved),
    ("TestsRoot: {0}" -f $testsRoot),
    ("Findings: {0}" -f $findings.Count),
    'Exceptions: tools/audit-cli/test-audit-exceptions.json',
    ''
)

$header | Out-File -LiteralPath $reportPath -Encoding utf8

if ($findings.Count -eq 0) {
    'PASS' | Add-Content -LiteralPath $reportPath
    Write-Host 'Rule54: PASS' -ForegroundColor Green
    exit 0
}

$findings | Sort-Object | Add-Content -LiteralPath $reportPath
Write-Host ("Rule54: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red

if ($FailOnFindings.IsPresent) { exit 1 }
exit 0
