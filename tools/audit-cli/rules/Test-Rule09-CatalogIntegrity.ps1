<#
.SYNOPSIS
    Test Rule09 Catalog Integrity

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule09' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule09." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule09' -Title 'Audit rule catalog integrity (paths + wrapper consistency)' -RepoRoot $repoRootResolved -OutputPath $outputPath

$rules = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param([string]$Message)
    $findings.Add($Message) | Out-Null
}

foreach ($r in $rules) {
    if ([string]::IsNullOrWhiteSpace($r.Id)) { Add-Finding "Rule has empty Id."; continue }

    if ([string]::IsNullOrWhiteSpace($r.ScriptPath)) {
        Add-Finding "[$($r.Id)] ScriptPath is empty."
    }
    elseif (-not (Test-Path -LiteralPath $r.ScriptPath)) {
        Add-Finding "[$($r.Id)] ScriptPath not found: $($r.ScriptPath)"
    }

    if ([string]::IsNullOrWhiteSpace($r.OutputPath)) {
        Add-Finding "[$($r.Id)] OutputPath is empty."
    }
    elseif (-not ($r.OutputPath -like 'artifacts/audit/*')) {
        Add-Finding "[$($r.Id)] OutputPath should be under artifacts/audit/: $($r.OutputPath)"
    }

    # Wrapper scripts should source output paths from the catalog to avoid drift.
    if ($r.ScriptPath -and (Test-Path -LiteralPath $r.ScriptPath)) {
        $text = Get-Content -LiteralPath $r.ScriptPath -Raw
        if ($text -notmatch '\$ruleInfo\.OutputPath') {
            Add-Finding "[$($r.Id)] Wrapper does not appear to use catalog OutputPath (expected '$ruleInfo.OutputPath' pattern): $($r.ScriptPath)"
        }
    }
}

# Detect duplicate IDs
$dupIds = @(
    $rules |
        Group-Object -Property Id |
        Where-Object { $_.Count -gt 1 } |
        Select-Object -ExpandProperty Name
)
if ($dupIds.Count -gt 0) {
    Add-Finding "Duplicate rule Id(s): $($dupIds -join ', ')"
}

if ($findings.Count -eq 0) {
    'Catalog integrity: PASS' | Out-File -LiteralPath (Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath) -Encoding utf8
    Write-Host 'Catalog integrity: PASS' -ForegroundColor Green
    exit 0
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
$findings | Out-File -LiteralPath $reportPath -Encoding utf8

Write-Host ("Catalog integrity: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red
foreach ($f in $findings) {
    Write-Host ("- {0}" -f $f) -ForegroundColor Red
}

if ($FailOnFindings.IsPresent) {
    exit 1
}

exit 0
