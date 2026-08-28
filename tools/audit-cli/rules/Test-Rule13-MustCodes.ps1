<#
.SYNOPSIS
    Test Rule13 Must Codes

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain. Validates the MustCodes error-code catalogue
    (src/PineGuard.Core/Codes/) against the clause tree via textual source analysis (no build):
    (a) every public Must.Be.* clause passes exactly one MustCodes constant on every Fail(/FromBool(
    call; (b) every catalogue constant (other than Prefix) is referenced by at least one clause,
    DataAnnotations attribute, or Core/AspNetCore call site; (c) no code string literal duplicates a
    catalogue domain outside src/PineGuard.Core/Codes/; (e) every Guard.Against.* clause passes its
    IMustResult (never a string) as GuardFailure.Throw's first argument, so the code and property
    path on the thrown exception are always the Must layer's own; (f) every clause file only
    references constants from its mapped domain; (g) no "using PineGuard" line appears under
    src/PineGuard.Core/Codes/ (the catalogue must stay a dependency-free leaf).

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
. (Join-Path $PSScriptRoot 'Load-Catalog.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule13' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule13." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule13' -Title 'Must error-code catalogue integrity (source scan)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param([string]$Message)
    $findings.Add($Message) | Out-Null
}

# clause-file key (Must<Key>Clauses.cs) -> catalogue domain class name (PascalCase)
$domainMap = @{
    'Bool' = 'Boolean'; 'StringBool' = 'Boolean'
    'Char' = 'Character'
    'Number' = 'Number'; 'StringNumbers' = 'Number'; 'StringNumberTypes' = 'Number'
    'BitWise' = 'Bitwise'
    'Enum' = 'Enum'
    'Guid' = 'Guid'; 'StringGuid' = 'Guid'
    'DateTime' = 'Date'; 'DateOnly' = 'Date'; 'DateTimeOffset' = 'Date'; 'SqlDateTime' = 'Date'; 'StringDateOnly' = 'Date'; 'StringDateTimeOffset' = 'Date'
    'TimeOnly' = 'Time'; 'TimeSpan' = 'Time'; 'StringTimeOnly' = 'Time'; 'StringTimeSpan' = 'Time'
    'DateTimeRange' = 'Range'; 'DateOnlyRange' = 'Range'; 'DateTimeOffsetRange' = 'Range'; 'TimeOnlyRange' = 'Range'
    'Collection' = 'Collection'
    'Dictionary' = 'Dictionary'; 'ReadOnlyDictionary' = 'Dictionary'
    'Email' = 'Email'
    'Phone' = 'Phone'
    'Uri' = 'Uri'
    'Network' = 'Network'
    'FilePath' = 'File'
    'GeoLocation' = 'Geo'; 'StringGeoLocation' = 'Geo'
    'Http' = 'Http'; 'HttpSecurityHeader' = 'Http'
    'Json' = 'Json'
    'Xml' = 'Xml'
    'Csv' = 'Csv'
    'Owasp' = 'Owasp'
    'Identifier' = 'Identifier'
    'Predicate' = 'Predicate'
    'Task' = 'Task'
    'Buffer' = 'Encoding'
    'Null' = 'Value'; 'DefaultEquality' = 'Value'; 'Object' = 'Value'
    'String' = 'Text'; 'StringCasing' = 'Text'
}

$codesDir = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path 'src/PineGuard.Core/Codes'
$mustClausesDir = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path 'src/PineGuard.MustClauses'

if (-not (Test-Path $codesDir)) { throw "Codes directory not found: $codesDir" }
if (-not (Test-Path $mustClausesDir)) { throw "MustClauses directory not found: $mustClausesDir" }

# ---- Discover every MustCodes.*.cs file and every declared constant (qualified path, without the "MustCodes." prefix) ----

$codesFiles = @(Get-ChildItem -Path $codesDir -Filter '*.cs' -File)
$declaredConstants = New-Object System.Collections.Generic.List[string]
$constClassPattern = [regex]::new('^\s*(?:public|internal)\s+static\s+(?:partial\s+)?class\s+(\w+)')
$constFieldPattern = [regex]::new('^\s*(?:public|internal)\s+const\s+string\s+(\w+)\s*=')

foreach ($file in $codesFiles) {
    $lines = Get-Content -LiteralPath $file.FullName
    # Stack of (Indent, Name) for nested classes below the MustCodes root itself.
    $stack = New-Object System.Collections.Generic.List[object]

    foreach ($line in $lines) {
        $indent = ($line.Length - $line.TrimStart(' ').Length)

        $classMatch = $constClassPattern.Match($line)
        if ($classMatch.Success -and $classMatch.Groups[1].Value -ne 'MustCodes') {
            while ($stack.Count -gt 0 -and $stack[$stack.Count - 1].Indent -ge $indent) {
                $stack.RemoveAt($stack.Count - 1)
            }
            $stack.Add([pscustomobject]@{ Indent = $indent; Name = $classMatch.Groups[1].Value })
            continue
        }

        $fieldMatch = $constFieldPattern.Match($line)
        if ($fieldMatch.Success -and $stack.Count -gt 0) {
            $fieldName = $fieldMatch.Groups[1].Value
            if ($fieldName -ne 'Prefix') {
                $qualified = (($stack | ForEach-Object { $_.Name }) -join '.') + '.' + $fieldName
                $declaredConstants.Add($qualified) | Out-Null
            }
        }
    }
}

$declaredConstants = @($declaredConstants | Sort-Object -Unique)
Write-Host "Declared MustCodes constants (excluding Prefix): $($declaredConstants.Count)" -ForegroundColor DarkGray

# ---- (g) No "using PineGuard" line under src/PineGuard.Core/Codes/ ----

foreach ($file in $codesFiles) {
    $lines = Get-Content -LiteralPath $file.FullName
    foreach ($line in $lines) {
        if ($line -match '^\s*using\s+PineGuard') {
            Add-Finding "(g) $($file.Name): 'using PineGuard...' line found under Codes/ — the catalogue must stay a dependency-free leaf ($($line.Trim()))"
        }
    }
}

# ---- Gather clause files and their bodies ----

$clauseFiles = @(Get-ChildItem -Path $mustClausesDir -Filter 'Must*Clauses.cs' -File)
$methodSigPattern = [regex]::new('public\s+static\s+(?:MustResult<|ValueTask<MustResult<)[^\r\n]*?\s(\w+)\s*(?:<[^>]*>)?\s*\(', [System.Text.RegularExpressions.RegexOptions]::None)
$callSitePattern = [regex]::new('MustResult<[^>]*(?:<[^>]*>)?[^>]*>\.(Fail|FromBool)\s*\(')
$codeRefPattern = [regex]::new('MustCodes(?:\.\w+)+')

function Get-BalancedSpan {
    param([string]$Content, [int]$OpenIndex, [char]$Open, [char]$Close)
    $depth = 1
    $i = $OpenIndex + 1
    while ($depth -gt 0 -and $i -lt $Content.Length) {
        if ($Content[$i] -eq $Open) { $depth++ }
        elseif ($Content[$i] -eq $Close) { $depth-- }
        $i++
    }
    return $i
}

$allUsageFiles = New-Object System.Collections.Generic.List[string]
foreach ($extraRoot in @('src/PineGuard.Core', 'src/PineGuard.DataAnnotations', 'src/PineGuard.AspNetCore')) {
    $full = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $extraRoot
    if (Test-Path $full) {
        Get-ChildItem -Path $full -Recurse -File -Filter '*.cs' | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' } | ForEach-Object { $allUsageFiles.Add($_.FullName) | Out-Null }
    }
}

$usedConstants = New-Object System.Collections.Generic.HashSet[string]
$domainTokens = @($declaredConstants | ForEach-Object { $_.Split('.')[0] } | Sort-Object -Unique)

foreach ($file in $clauseFiles) {
    $baseName = $file.BaseName -replace '^Must', '' -replace 'Clauses$', ''
    $domain = $domainMap[$baseName]
    if (-not $domain) {
        Add-Finding "(f) $($file.Name): no domain mapping registered in this audit script for clause-file key '$baseName' — add it to `$domainMap."
        continue
    }

    $content = Get-Content -LiteralPath $file.FullName -Raw

    # (c) no hardcoded code-string literal (matching a catalogue domain) outside Codes/
    foreach ($token in $domainTokens) {
        $litPattern = [regex]::new('"' + [regex]::Escape($token.ToLowerInvariant()) + '\.[a-z][a-z0-9-]*\.[a-z][a-z0-9-]*"')
        foreach ($m in $litPattern.Matches($content)) {
            Add-Finding "(c) $($file.Name): hardcoded code string literal $($m.Value) — use the MustCodes constant instead."
        }
    }

    # (f) every MustCodes.<X>... reference in this file must have X == this file's mapped domain
    foreach ($m in $codeRefPattern.Matches($content)) {
        $usedConstants.Add(($m.Value -replace '^MustCodes\.', '')) | Out-Null
        $parts = $m.Value.Split('.')
        if ($parts.Length -ge 2) {
            $referencedDomain = $parts[1]
            if ($referencedDomain -ne $domain) {
                Add-Finding "(f) $($file.Name): references MustCodes.$referencedDomain.* but is mapped to domain '$domain' — clause files must only use their own domain's constants."
            }
        }
    }

    # (a) every public clause method's Fail(/FromBool( calls pass exactly one MustCodes constant
    foreach ($sigMatch in $methodSigPattern.Matches($content)) {
        $methodName = $sigMatch.Groups[1].Value
        $parenOpen = $content.IndexOf('(', $sigMatch.Index + $sigMatch.Length - 1)
        if ($parenOpen -lt 0) { continue }
        $afterParams = Get-BalancedSpan -Content $content -OpenIndex $parenOpen -Open '(' -Close ')'
        $braceOpen = $content.IndexOf('{', $afterParams)
        if ($braceOpen -lt 0) { continue }
        $braceClose = Get-BalancedSpan -Content $content -OpenIndex $braceOpen -Open '{' -Close '}'
        $body = $content.Substring($braceOpen, $braceClose - $braceOpen)

        foreach ($callMatch in $callSitePattern.Matches($body)) {
            $callOpenParen = $body.IndexOf('(', $callMatch.Index + $callMatch.Length - 1)
            if ($callOpenParen -lt 0) { continue }
            $callEnd = Get-BalancedSpan -Content $body -OpenIndex $callOpenParen -Open '(' -Close ')'
            $argText = $body.Substring($callOpenParen + 1, $callEnd - $callOpenParen - 2)
            $codeRefs = @($codeRefPattern.Matches($argText))
            if ($codeRefs.Count -eq 0) {
                Add-Finding "(a) $($file.Name): $methodName -> .$($callMatch.Groups[1].Value)(...) call passes no MustCodes constant."
            }
            elseif ($codeRefs.Count -gt 1) {
                Add-Finding "(a) $($file.Name): $methodName -> .$($callMatch.Groups[1].Value)(...) call passes $($codeRefs.Count) MustCodes constants, expected exactly one."
            }
        }
    }
}

# ---- (e) every Guard.Against.* clause passes its IMustResult (never a string) to GuardFailure.Throw ----

$guardClausesDir = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path 'src/PineGuard.GuardClauses'
if (-not (Test-Path $guardClausesDir)) { throw "GuardClauses directory not found: $guardClausesDir" }

$guardClauseFiles = @(Get-ChildItem -Path $guardClausesDir -Filter 'Guard*Clauses.cs' -File)
$guardThrowPattern = [regex]::new('GuardFailure\.Throw\s*\(\s*(.)')

foreach ($file in $guardClauseFiles) {
    $content = Get-Content -LiteralPath $file.FullName -Raw

    foreach ($m in $guardThrowPattern.Matches($content)) {
        $firstChar = $m.Groups[1].Value
        if ($firstChar -eq '"' -or $firstChar -eq [char]39) {
            $lineNumber = ($content.Substring(0, $m.Index) -split "`n").Count
            Add-Finding "(e) $($file.Name):$($lineNumber): GuardFailure.Throw(...) is called with a string literal as its first argument — pass the IMustResult itself, e.g. GuardFailure.Throw(result, message, exceptionCreator)."
        }
    }
}

# Usage scan across the extra roots (Core/DataAnnotations/AspNetCore) for check (b)
foreach ($filePath in $allUsageFiles) {
    if ($filePath -match '\\Codes\\') { continue }
    $content = Get-Content -LiteralPath $filePath -Raw
    foreach ($m in $codeRefPattern.Matches($content)) {
        $usedConstants.Add(($m.Value -replace '^MustCodes\.', '')) | Out-Null
    }
}

# ---- (b) every declared constant (other than Prefix) is referenced somewhere ----
# A small number of constants are deliberately reserved for a later phase's adapter (documented on
# the constant itself) and have no current call site — exempt them by exact qualified name here,
# never by pattern, so a genuinely-forgotten constant still fails loudly.
$reservedForLaterPhase = @(
    'Value.Argument.Invalid' # reserved for the Phase 3 ASP.NET Core adapter; no clause emits it directly.
)

foreach ($constant in $declaredConstants) {
    if ($reservedForLaterPhase -contains $constant) { continue }
    if (-not $usedConstants.Contains($constant)) {
        Add-Finding "(b) MustCodes.$constant is declared but never referenced by any clause, DataAnnotations attribute, or Core/AspNetCore call site."
    }
}

# ---- Report ----

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
$reportParent = Split-Path -Parent $reportPath
if (-not [string]::IsNullOrWhiteSpace($reportParent)) {
    Ensure-PineGuardDirectory -Path $reportParent
}

$summaryLines = @(
    "AuditRule: Rule13 - Must error-code catalogue integrity (source scan)",
    "Date: $(Get-Date)",
    "RepoRoot: $repoRootResolved",
    "Clause files scanned: $($clauseFiles.Count)",
    "Guard clause files scanned: $($guardClauseFiles.Count)",
    "Codes files scanned: $($codesFiles.Count)",
    "Declared constants (excl. Prefix): $($declaredConstants.Count)",
    "Findings: $($findings.Count)",
    ''
)

if ($findings.Count -eq 0) {
    $summaryLines += 'PASS - no findings.'
}
else {
    $summaryLines += $findings
}

$summaryLines | Out-File -LiteralPath $reportPath -Encoding utf8

if ($findings.Count -eq 0) {
    Write-Host 'Rule13: PASS' -ForegroundColor Green
    exit 0
}

Write-Host ("Rule13: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red
foreach ($f in $findings) {
    Write-Host ("- {0}" -f $f) -ForegroundColor Red
}

if ($FailOnFindings.IsPresent) {
    exit 1
}

exit 0
