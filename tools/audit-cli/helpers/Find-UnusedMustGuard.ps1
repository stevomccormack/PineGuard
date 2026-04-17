<#
.SYNOPSIS
    Find unused MustClauses methods from GuardClauses.

.DESCRIPTION
    Builds PineGuard.MustClauses, discovers public Must.Be.* extension methods, then scans
    GuardClauses sources for usage. Reports Must methods not used by Guard.

.PARAMETER AuditRuleId
    Audit rule identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER OutputPath
    Optional explicit output path. If omitted, writes under ArtifactsSubdir using OutputFileName.

.PARAMETER Configuration
    Build configuration.

.PARAMETER MustProject
    Relative path to PineGuard.MustClauses project.

.PARAMETER ScanRoot
    Relative root to scan for usage (typically GuardClauses).

.PARAMETER ArtifactsSubdir
    Relative artifacts directory.

.PARAMETER OutputFileName
    Output filename when OutputPath is not provided.

.PARAMETER FailOnFindings
    If set, throws when unused Must methods are found.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Rule03',
    [string]$RepoRoot = '',
    [string]$OutputPath = '',
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [string]$MustProject = 'src/PineGuard.MustClauses/PineGuard.MustClauses.csproj',
    [string]$ScanRoot = 'src/PineGuard.GuardClauses',
    [string]$ArtifactsSubdir = 'artifacts/audit',
    [string]$OutputFileName = 'find-unused-must-with-guard-clauses.txt',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$artifactsDir = Join-Path $repoRootResolved $ArtifactsSubdir
Ensure-PineGuardDirectory -Path $artifactsDir

$mustProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $MustProject
if (-not (Test-Path $mustProjPath)) {
    throw "MustClauses project not found at $mustProjPath"
}

Write-Host "Building PineGuard.MustClauses ($Configuration)..." -ForegroundColor Cyan
& dotnet build $mustProjPath -c $Configuration -v quiet | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw "dotnet build failed for: $mustProjPath"
}

$mustBinDir = Join-Path $repoRootResolved "src/PineGuard.MustClauses/bin/$Configuration/net8.0"
$mustDll = Join-Path $mustBinDir 'PineGuard.MustClauses.dll'

if (-not (Test-Path $mustDll)) {
    throw "Build failed or DLL not found: $mustDll"
}

$tempPath = ''
try {
    # Load assembly from a temp copy to avoid file locks.
    $tempPath = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-Must-" + [System.Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $tempPath -Force | Out-Null
    Copy-Item "$mustBinDir\*" -Destination $tempPath -Force -Recurse

    $loadedDllPath = Join-Path $tempPath 'PineGuard.MustClauses.dll'
    $asm = [System.Reflection.Assembly]::LoadFrom($loadedDllPath)

    $extensionAttrType = [System.Runtime.CompilerServices.ExtensionAttribute]
    $bindingFlags = [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static

    # 1. Discover all Must.Be.* extension methods (by receiver type)
    $mustMethods = New-Object System.Collections.Generic.List[object]

    foreach ($t in $asm.GetTypes()) {
        if (-not ($t.IsClass -and $t.IsPublic -and $t.Namespace -and ($t.Namespace -match '^PineGuard\\.MustClauses'))) { continue }

        foreach ($m in $t.GetMethods($bindingFlags)) {
            if (-not $m.IsDefined($extensionAttrType, $false)) { continue }

            $ps = $m.GetParameters()
            if ($ps.Length -lt 1) { continue }
            if ($ps[0].ParameterType.FullName -ne 'PineGuard.MustClauses.IMustClause') { continue }

            $sigParamTypes = @($ps | Select-Object -Skip 1 | ForEach-Object { $_.ParameterType.Name })
            $sig = "{0}::{1}({2})" -f $t.Name, $m.Name, ($sigParamTypes -join ', ')

            $mustMethods.Add([pscustomobject]@{
                Name      = $m.Name
                Signature = $sig
            }) | Out-Null
        }
    }

    $allMustNames = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    $mustMethods | ForEach-Object { [void]$allMustNames.Add($_.Name) }

    # 2. Scan GuardClauses for usages
    $scanRootFull = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $ScanRoot
    if (-not (Test-Path $scanRootFull)) {
        throw "Scan root not found: $scanRootFull"
    }

    $usedMethods = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)

    # Common patterns:
    #   Must.Be.SomeClause(...)
    #   MustXxxClauses.SomeClause(Must.Be, ...)
    $regexMustBe = [regex]::new(
        '(?x)\bMust\s*\.\s*Be\s*\.\s*([A-Za-z_][A-Za-z0-9_]*)\s*(?:<[^()]*>)?\s*\(',
        [System.Text.RegularExpressions.RegexOptions]::Compiled
    )

    $regexStaticMust = [regex]::new(
        '(?x)\bMust[A-Za-z0-9_]*Clauses\s*\.\s*([A-Za-z_][A-Za-z0-9_]*)\s*(?:<[^()]*>)?\s*\(\s*Must\s*\.\s*Be\b',
        [System.Text.RegularExpressions.RegexOptions]::Compiled
    )

    $files = @(
        Get-ChildItem -Path $scanRootFull -Recurse -File -Filter '*.cs'
        | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
    )

    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw

        foreach ($match in $regexMustBe.Matches($content)) {
            [void]$usedMethods.Add($match.Groups[1].Value)
        }

        foreach ($match in $regexStaticMust.Matches($content)) {
            [void]$usedMethods.Add($match.Groups[1].Value)
        }
    }

    # 3. Analyze
    # Treat NotX complements as "covered" (Guard often uses complements).
    $covered = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    foreach ($name in $allMustNames) {
        if ($usedMethods.Contains($name)) {
            [void]$covered.Add($name)
            continue
        }

        if ($name.StartsWith('Not') -and $usedMethods.Contains($name.Substring(3))) {
            [void]$covered.Add($name)
            continue
        }

        $notName = 'Not' + $name
        if ($usedMethods.Contains($notName)) {
            [void]$covered.Add($name)
            continue
        }
    }

    $unusedNames = @($allMustNames | Where-Object { -not $covered.Contains($_) } | Sort-Object)
    $unusedSigs = @(
        $mustMethods
        | Where-Object { $unusedNames -contains $_.Name }
        | Sort-Object Signature
    )

    # 4. Report
    $reportLines = @(
        "AuditRule: $AuditRuleId - MustClauses \u2192 GuardClauses mapping (usage scan)",
        "Script: $($MyInvocation.MyCommand.Name)",
        "Date: $(Get-Date)",
        "RepoRoot: $repoRootResolved",
        "Configuration: $Configuration",
        "Must Assembly: $mustDll",
        "Files Scanned: $($files.Count)",
        "Total Must Methods Found: $($allMustNames.Count)",
        "Used Methods Found: $($usedMethods.Count)",
        "Unused Must Extension Names: $($unusedNames.Count)",
        ""
    )

    if ($unusedNames.Count -gt 0) {
        $reportLines += 'UNUSED MUST CLAUSES (Not used in GuardClauses):'
        $unusedNames | ForEach-Object { $reportLines += "- $_" }
        $reportLines += ''
        $reportLines += 'UNUSED SIGNATURES:'
        $unusedSigs | ForEach-Object { $reportLines += "- $($_.Signature)" }
        $reportLines += ''
    }
    else {
        $reportLines += 'All MustClauses public extensions are used in GuardClauses.'
        $reportLines += ''
    }

    $reportPath = if ([string]::IsNullOrWhiteSpace($OutputPath)) {
        Join-Path $artifactsDir $OutputFileName
    }
    else {
        Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $OutputPath
    }

    $reportParent = Split-Path -Parent $reportPath
    if (-not [string]::IsNullOrWhiteSpace($reportParent)) {
        Ensure-PineGuardDirectory -Path $reportParent
    }

    $reportLines | Set-Content -Path $reportPath

    Write-Host 'Analysis Complete.' -ForegroundColor Green
    Write-Host "Unused Must Names: $($unusedNames.Count)" -ForegroundColor ($unusedNames.Count -gt 0 ? 'Red' : 'Green')
    Write-Host "Report written to: $reportPath" -ForegroundColor DarkGray

    if ($FailOnFindings.IsPresent -and ($unusedNames.Count -gt 0)) {
        throw "AuditRule $AuditRuleId failed: UnusedMust=$($unusedNames.Count). See report: $reportPath"
    }
}
finally {
    if (-not [string]::IsNullOrWhiteSpace($tempPath)) {
        Remove-Item $tempPath -Recurse -Force -ErrorAction SilentlyContinue
    }
}
