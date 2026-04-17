<#
.SYNOPSIS
    Scan test data for tuple-shaped records.

.DESCRIPTION
    Heuristically scans *TestData.cs files for record declarations containing tuple-typed
    parameters. Used as an internal utility for maintaining test data conventions.

.PARAMETER AuditRuleId
    Identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER TestsRoot
    Tests root to scan.

.PARAMETER OutputPath
    Output report path.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Util02',
    [ValidateNotNullOrEmpty()] [string]$RepoRoot = '',
    [ValidateNotNullOrEmpty()] [string]$TestsRoot = 'tests',
    [ValidateNotNullOrEmpty()] [string]$OutputPath = 'artifacts/audit/util/Util02-testdata-tuple-scan.txt'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$testsRootResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $TestsRoot
$outputPathResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $OutputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $outputPathResolved)

Write-PineGuardAuditHeader -AuditRuleId $AuditRuleId -Title 'Scan test data for tuple-shaped records' -RepoRoot $repoRootResolved -OutputPath $OutputPath

$testDataFiles = @(Get-ChildItem -Path $testsRootResolved -Recurse -File -Filter '*TestData.cs')

$results = New-Object System.Collections.Generic.List[object]

foreach ($file in $testDataFiles) {
    $content = Get-Content $file.FullName
    $className = $file.BaseName
    $category = $className -replace 'TestData$', ''

    # Simple state machine to track nested class (MethodName)
    $currentMethodInfo = ''

    for ($i = 0; $i -lt $content.Count; $i++) {
        $line = $content[$i].Trim()

        if ($line -match 'public static class (\w+)') {
            $currentMethodInfo = $matches[1]
        }

        # Heuristic: single-line record definition containing a tuple parameter.
        if ($line -match 'public sealed record \w+\(.*\(([^)]+ \w+)\) (\w+)[,)]') {
            $tupleBody = $matches[1]
            $propName = $matches[2]

            $params = $tupleBody -split ',' | ForEach-Object { $_.Trim().Split(' ')[-1] }

            $results.Add([PSCustomObject]@{
                File       = $file.FullName.Substring($repoRootResolved.Length).TrimStart('\\', '/')
                Category   = $category
                Method     = $currentMethodInfo
                TupleProps = ($params -join ', ')
                PropName   = $propName
            }) | Out-Null
        }
    }
}

$reportHeader = @(
    "AuditRule: $AuditRuleId - Scan test data for tuple-shaped records",
    "RepoRoot : $repoRootResolved",
    "TestsRoot: $testsRootResolved",
    "Matches : $($results.Count)",
    ''
)

$reportHeader | Set-Content -Path $outputPathResolved

if ($results.Count -eq 0) {
    'No tuple-shaped records found (heuristic scan).' | Add-Content -Path $outputPathResolved
    Write-Host 'No tuple-shaped records found (heuristic scan).' -ForegroundColor Green
}
else {
    $results | Sort-Object Category, Method, File | Format-Table -AutoSize | Out-String | Add-Content -Path $outputPathResolved
    $results | Sort-Object Category, Method, File | Format-Table -AutoSize
}

Write-Host "Report written to: $outputPathResolved" -ForegroundColor DarkGray
