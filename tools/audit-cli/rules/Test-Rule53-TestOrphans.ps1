<#
.SYNOPSIS
    Test Rule53 Test Orphans
    
.DESCRIPTION
    Ensures every *Tests.cs file corresponds to a valid Source class file in the matching Source project.
    Handles standard naming (FooTests -> Foo) and normalized naming (StringRulesBoolTests -> StringRules.Bool.cs).

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule53' } | Select-Object -First 1
if (-not $ruleInfo) { throw 'Catalog is missing Rule53.' }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule53' -Title 'Test Orphans (*Tests.cs must have Source class)' -RepoRoot $repoRootResolved -OutputPath $outputPath

# Load Exceptions
$exceptions = Import-PineGuardTestAuditExceptions -RepoRoot $repoRootResolved
$allowOrphan = Get-PineGuardTestAuditExceptionSet -Exceptions $exceptions -RuleId 'Rule53' -Key 'AllowOrphanTest'

$testsRoot = Join-Path $repoRootResolved 'tests'
$srcRoot = Join-Path $repoRootResolved 'src'

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param([string]$Message)
    $findings.Add($Message) | Out-Null
}

function Test-IsInScopeTestsFile {
    param([string]$RelativePath)
    $p = $RelativePath
    if ($p -match '(?i)(^|/)bin(/|$)') { return $false }
    if ($p -match '(?i)(^|/)obj(/|$)') { return $false }
    return $true
}

# 1. Discover UnitTests Projects
$testProjects = Get-ChildItem -Path $testsRoot -Directory | Where-Object { $_.Name -match '\.UnitTests$' }

foreach ($tp in $testProjects) {
    $projectName = $tp.Name
    # Derive Source Project Name: PineGuard.Core.UnitTests -> PineGuard.Core
    $sourceProjectName = $projectName -replace '\.UnitTests$', ''
    $sourceProjectPath = Join-Path $srcRoot $sourceProjectName

    if (-not (Test-Path $sourceProjectPath)) {
        # If source project doesn't exist, maybe it's a Testing-only project (like PineGuard.Testing)? 
        # Skip if src not found, or log warning?
        # PineGuard.Testing might be in src/PineGuard.Testing?
        # If not in src, skip.
        continue
    }

    # 2. Build Source Map (Exact + Normalized)
    $sourceFiles = Get-ChildItem -Path $sourceProjectPath -Recurse -File -Filter "*.cs" | 
    Where-Object { $_.Name -notmatch "AssemblyAttributes|GlobalUsings" }
    
    $sourceBaseNames = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    $normalizedMap = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

    foreach ($f in $sourceFiles) {
        $base = $f.BaseName
        [void]$sourceBaseNames.Add($base)
        
        # Normalization: StringRules.Bool -> StringRulesBool
        if ($base -match "\.") {
            $norm = $base -replace "\.", ""
            [void]$normalizedMap.Add($norm)
        }
    }

    # 3. Check Tests
    $testFiles = Get-ChildItem -Path $tp.FullName -Recurse -File -Filter "*Tests.cs" |
    Where-Object { $_.Name -notmatch "AssemblyAttributes|GlobalUsings" }

    foreach ($tFile in $testFiles) {
        $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $tFile.FullName
        if (-not (Test-IsInScopeTestsFile -RelativePath $rel)) { continue }
        
        # Check Exception
        if ($allowOrphan.Contains($rel)) { continue }

        $baseName = $tFile.BaseName
        
        # Ignore obvious utility
        if ($baseName -match "Base|Utility|Common|Abstract") { continue }

        $targetClass = $baseName -replace "Tests$", ""
        
        $isMatch = $false
        
        # Strategy A: Exact Match
        if ($sourceBaseNames.Contains($targetClass)) {
            $isMatch = $true
        }
        # Strategy B: Normalized Match (StringRulesBoolTests -> StringRules.Bool)
        elseif ($normalizedMap.Contains($targetClass)) {
            $isMatch = $true
        }

        if (-not $isMatch) {
            Add-Finding ("[OrphanTest] {0} refers to '{1}' which was not found in {2}" -f $rel, $targetClass, $sourceProjectName)
        }
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$header = @(
    'Rule53 - Test Orphans',
    ("RepoRoot: {0}" -f $repoRootResolved),
    ("Findings: {0}" -f $findings.Count),
    'Exceptions: tools/audit-cli/test-audit-exceptions.json',
    ''
)

$header | Out-File -LiteralPath $reportPath -Encoding utf8

if ($findings.Count -eq 0) {
    'PASS' | Add-Content -LiteralPath $reportPath
    Write-Host 'Rule53: PASS' -ForegroundColor Green
    exit 0
}

$findings | Sort-Object | Add-Content -LiteralPath $reportPath
Write-Host ("Rule53: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red

if ($FailOnFindings.IsPresent) { exit 1 }
exit 0
