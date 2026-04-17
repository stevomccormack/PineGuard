<#
.SYNOPSIS
    Find unused PineGuard.Core Rules types.

.DESCRIPTION
    Builds PineGuard.Core, loads the Rules assembly, discovers all public *Rules classes,
    then scans source roots (typically MustClauses) for references.

.PARAMETER AuditRuleId
    Audit rule identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER OutputPath
    Optional explicit output path. If omitted, writes under ArtifactsSubdir using OutputFileName.

.PARAMETER Configuration
    Build configuration.

.PARAMETER CoreProject
    Relative path to PineGuard.Core project.

.PARAMETER ScanRoots
    Relative roots to scan for usage.

.PARAMETER ArtifactsSubdir
    Relative artifacts directory.

.PARAMETER OutputFileName
    Output filename when OutputPath is not provided.

.PARAMETER FailOnFindings
    If set, throws when unused rules or unknown references are found.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Rule02',
    [string]$RepoRoot = '',
    [string]$OutputPath = '',
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [string]$CoreProject = 'src/PineGuard.Core/PineGuard.Core.csproj',
    [string[]]$ScanRoots = @( 'src/PineGuard.MustClauses' ),
    [string]$ArtifactsSubdir = 'artifacts/audit',
    [string]$OutputFileName = 'find-unused-rules-with-must-clauses.txt',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$artifactsDir = Join-Path $repoRootResolved $ArtifactsSubdir
Ensure-PineGuardDirectory -Path $artifactsDir

$coreProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $CoreProject
if (-not (Test-Path $coreProjPath)) {
    throw "Core project not found at $coreProjPath"
}

Write-Host "Building PineGuard.Core ($Configuration)..." -ForegroundColor Cyan
& dotnet build $coreProjPath -c $Configuration -v quiet | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw "dotnet build failed for: $coreProjPath"
}

$coreBinDir = Join-Path $repoRootResolved "src/PineGuard.Core/bin/$Configuration/net8.0"
$coreDll = Join-Path $coreBinDir 'PineGuard.Core.dll'
if (-not (Test-Path $coreDll)) {
    throw "Build failed or DLL not found: $coreDll"
}

$tempPath = ''
try {
    $tempPath = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-Rules-" + [System.Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $tempPath -Force | Out-Null
    Copy-Item $coreDll -Destination $tempPath -Force

    $loadedDllPath = Join-Path $tempPath 'PineGuard.Core.dll'
    $asm = [System.Reflection.Assembly]::LoadFrom($loadedDllPath)

    # 1. Find all *Rules classes
    $allRules = @(
        $asm.GetTypes()
        | Where-Object {
            $_.IsClass -and $_.IsPublic -and $_.IsSealed -and $_.IsAbstract -and
            ($_.Namespace -eq 'PineGuard.Rules' -or $_.Namespace.StartsWith('PineGuard.Rules.')) -and
            $_.Name.EndsWith('Rules')
        }
        | Select-Object -ExpandProperty Name
        | Sort-Object -Unique
    )

    if ($allRules.Count -eq 0) {
        Write-Warning 'No *Rules classes found in PineGuard.Core.'
    }

    # 2. Scan ScanRoots for references to rules
    $usedRules = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    $regex = [regex]::new('\b([A-Za-z_][A-Za-z0-9_]*)Rules\.', [System.Text.RegularExpressions.RegexOptions]::Compiled)
    $scannedFiles = 0

    foreach ($relRoot in $ScanRoots) {
        $fullPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $relRoot
        if (Test-Path $fullPath) {
            $files = @(
                Get-ChildItem -Path $fullPath -Recurse -File -Filter '*.cs'
                | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
            )

            $scannedFiles += $files.Count
            foreach ($file in $files) {
                $content = Get-Content $file.FullName -Raw
                foreach ($match in $regex.Matches($content)) {
                    [void]$usedRules.Add($match.Groups[1].Value + 'Rules')
                }
            }
        }
        else {
            Write-Warning "Scan root not found: $fullPath"
        }
    }

    # 3. Analyze
    $unusedRules = @($allRules | Where-Object { -not $usedRules.Contains($_) })
    $unknownRefs = @($usedRules | Where-Object { -not ($allRules -contains $_) } | Sort-Object)

    # 4. Report
    $reportLines = @(
        "AuditRule: $AuditRuleId - Public Rules \\u2192 MustClauses mapping (usage scan)",
        "Script: $($MyInvocation.MyCommand.Name)",
        "Date: $(Get-Date)",
        "RepoRoot: $repoRootResolved",
        "Configuration: $Configuration",
        "Core Assembly: $coreDll",
        "Files Scanned: $scannedFiles",
        "Total Rules Found: $($allRules.Count)",
        "Used Rules Found: $($usedRules.Count)",
        "Unused Rules Count: $($unusedRules.Count)",
        '',
        ''
    )

    if ($unusedRules.Count -gt 0) {
        $reportLines += 'UNUSED RULES (Approved for deletion if not needed):'
        $unusedRules | ForEach-Object { $reportLines += "- $_" }
        $reportLines += ''
    }
    else {
        $reportLines += 'All rules are used.'
        $reportLines += ''
    }

    if ($unknownRefs.Count -gt 0) {
        $reportLines += 'UNKNOWN REFERENCES (Referenced class ending in Rules but not in assembly):'
        $unknownRefs | ForEach-Object { $reportLines += "? $_" }
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
    Write-Host "Unused Rules: $($unusedRules.Count)" -ForegroundColor ($unusedRules.Count -gt 0 ? 'Red' : 'Green')
    Write-Host "Report written to: $reportPath" -ForegroundColor DarkGray

    if ($FailOnFindings.IsPresent -and (($unusedRules.Count -gt 0) -or ($unknownRefs.Count -gt 0))) {
        throw "AuditRule $AuditRuleId failed: UnusedRules=$($unusedRules.Count), UnknownRefs=$($unknownRefs.Count). See report: $reportPath"
    }
}
finally {
    if (-not [string]::IsNullOrWhiteSpace($tempPath)) {
        Remove-Item $tempPath -Recurse -Force -ErrorAction SilentlyContinue
    }
}
