<#
.SYNOPSIS
    Find Unused Must Fluent

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER AuditRuleId
    See the param block for details.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER OutputPath
    See the param block for details.

.PARAMETER Configuration
    See the param block for details.

.PARAMETER MustProject
    See the param block for details.

.PARAMETER ScanRoot
    See the param block for details.

.PARAMETER ArtifactsSubdir
    See the param block for details.

.PARAMETER OutputFileName
    See the param block for details.

.PARAMETER FailOnFindings
    See the param block for details.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Rule04',
    [string]$RepoRoot = '',
    [string]$OutputPath = '',
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [string]$MustProject = 'src/PineGuard.MustClauses/PineGuard.MustClauses.csproj',
    [string]$ScanRoot = 'src/PineGuard.FluentValidation',
    [string]$ArtifactsSubdir = 'artifacts/audit',
    [string]$OutputFileName = 'find-unused-must-with-fluent-extensions.txt',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')
$repoRoot = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$artifactsDir = Join-Path $repoRoot $ArtifactsSubdir

# Ensure output directory exists
if (-not (Test-Path $artifactsDir)) {
    New-Item -ItemType Directory -Path $artifactsDir -Force | Out-Null
}

$mustProjPath = Join-Path $repoRoot $MustProject
if (-not (Test-Path $mustProjPath)) {
    throw "MustClauses project not found at $mustProjPath"
}

# 1. Build MustClauses to get the assembly
Write-Host "Building PineGuard.MustClauses ($Configuration)..." -ForegroundColor Cyan
dotnet build $mustProjPath -c $Configuration -v quiet | Out-Host

$mustBinDir = Join-Path $repoRoot "src/PineGuard.MustClauses/bin/$Configuration/net8.0"
$mustDll = Join-Path $mustBinDir 'PineGuard.MustClauses.dll'

if (-not (Test-Path $mustDll)) {
    throw "Build failed or DLL not found: $mustDll"
}

# 2. Load Assembly (Safe copy)
$tempPath = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-Must-Fluent-" + [System.Guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $tempPath -Force | Out-Null
Get-ChildItem -Path $mustBinDir -Filter *.dll | Copy-Item -Destination $tempPath -Force
$loadedDllPath = Join-Path $tempPath 'PineGuard.MustClauses.dll'

$asm = [System.Reflection.Assembly]::LoadFrom($loadedDllPath)
$extensionAttrType = [System.Runtime.CompilerServices.ExtensionAttribute]
$bindingFlags = [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static

# 3. Find all Must.Be.* extension methods
$mustMethods = New-Object System.Collections.Generic.List[object]

foreach ($t in $asm.GetTypes()) {
    if (-not ($t.IsClass -and $t.IsPublic -and $t.Namespace -and ($t.Namespace -match '^PineGuard\\.MustClauses'))) { continue }

    foreach ($m in $t.GetMethods($bindingFlags)) {
        if (-not $m.IsDefined($extensionAttrType, $false)) { continue }
        $ps = $m.GetParameters()
        if ($ps.Length -lt 1 -or $ps[0].ParameterType.FullName -ne 'PineGuard.MustClauses.IMustClause') { continue }

        $sigParamTypes = @($ps | Select-Object -Skip 1 | ForEach-Object { $_.ParameterType.Name })
        $sig = "{0}::{1}({2})" -f $t.Name, $m.Name, ($sigParamTypes -join ', ')

        $mustMethods.Add([pscustomobject]@{
                Name      = $m.Name
                Signature = $sig
            })
    }
}

$allMustNames = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
$mustMethods | ForEach-Object { [void]$allMustNames.Add($_.Name) }

# 4. Scan ScanRoot for usages
$scanRootFull = Join-Path $repoRoot $ScanRoot
if (-not (Test-Path $scanRootFull)) { throw "Scan root not found: $scanRootFull" }

$usedMethods = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
$regexMustBe = [regex]::new(
    '(?x)\bMust\s*\.\s*Be\s*\.\s*([A-Za-z_][A-Za-z0-9_]*)\s*(?:<[^()]*>)?\s*\(',
    [System.Text.RegularExpressions.RegexOptions]::Compiled)

# Some FluentValidation adapters call the Must facade helpers directly (e.g. MustReadOnlyDictionaryClauses.HasKey(Must.Be, ...)).
$regexMustFacade = [regex]::new(
    '(?x)\bMust[A-Za-z0-9_]*Clauses\s*\.\s*([A-Za-z_][A-Za-z0-9_]*)\s*(?:<[^()]*>)?\s*\(\s*Must\s*\.\s*Be\b',
    [System.Text.RegularExpressions.RegexOptions]::Compiled)

$files = @(Get-ChildItem -Path $scanRootFull -Recurse -File -Filter '*.cs' | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' })

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    foreach ($match in $regexMustBe.Matches($content)) {
        [void]$usedMethods.Add($match.Groups[1].Value)
    }

    foreach ($match in $regexMustFacade.Matches($content)) {
        [void]$usedMethods.Add($match.Groups[1].Value)
    }
}

# 5. Analyze
$unusedNames = @($allMustNames | Where-Object { -not $usedMethods.Contains($_) } | Sort-Object)
$unusedSigs = @($mustMethods | Where-Object { $unusedNames -contains $_.Name } | Sort-Object Signature)

# 6. Report
$reportLines = @(
    "AuditRule: $AuditRuleId - MustClauses \u2192 FluentValidation mapping (usage scan)",
    "Script: $($MyInvocation.MyCommand.Name)",
    "Date: $(Get-Date)",
    "RepoRoot: $repoRoot",
    "Configuration: $Configuration",
    "MustClauses Assembly: $mustDll",
    "Files Scanned: $($files.Count)",
    "Total Must Extension Names: $($allMustNames.Count)",
    "Used Must Extension Names: $($usedMethods.Count)",
    "Unused Must Extension Names: $($unusedNames.Count)",
    ""
)

if ($unusedNames.Count -gt 0) {
    $reportLines += "UNUSED MUST CLAUSES (Not used in FluentValidation):"
    $unusedNames | ForEach-Object { $reportLines += "- $_" }
    $reportLines += ""
    $reportLines += "UNUSED SIGNATURES:"
    $unusedSigs | ForEach-Object { $reportLines += "- $($_.Signature)" }
}
else {
    $reportLines += "All MustClauses public extensions are used in FluentValidation extensions."
}

$reportPath = if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    Join-Path $artifactsDir $OutputFileName
}
else {
    Resolve-PineGuardPath -RepoRoot $repoRoot -Path $OutputPath
}

$reportLines | Set-Content -LiteralPath $reportPath -Encoding utf8

if ($unusedNames.Count -eq 0) {
    Write-Host 'MustClauses → FluentValidation usage: PASS' -ForegroundColor Green
    exit 0
}

Write-Host ("MustClauses → FluentValidation usage: FAIL ({0} unused Must method(s))" -f $unusedNames.Count) -ForegroundColor Red
Write-Host ("Output: {0}" -f $reportPath) -ForegroundColor DarkGray

if ($FailOnFindings.IsPresent) {
    exit 1
}

exit 0
