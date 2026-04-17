<#
.SYNOPSIS
    Compare FluentValidation vs GuardClauses parity.

.DESCRIPTION
    Compares public extension method "concepts" between PineGuard.GuardClauses and
    PineGuard.FluentValidation by loading published assemblies and normalizing method names
    using docs/ai/specs/language/vocabulary.json.

    Structural file parity is reported for context but is NOT enforced, since adapter layers
    aggregate by surface area and may not mirror internal folder structures.

.PARAMETER AuditRuleId
    Audit rule identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER OutputPath
    Optional explicit output path. If omitted, writes under ArtifactsSubdir using OutputFileName.

.PARAMETER Configuration
    Build configuration for publishing.

.PARAMETER GuardProject
    Relative path to PineGuard.GuardClauses project.

.PARAMETER FluentProject
    Relative path to PineGuard.FluentValidation project.

.PARAMETER ArtifactsSubdir
    Relative artifacts directory.

.PARAMETER OutputFileName
    Output filename when OutputPath is not provided.

.PARAMETER VocabularyPath
    Path to normalization vocabulary JSON.

.PARAMETER FailOnFindings
    If set, throws when concept parity violations are found.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Rule06',
    [string]$RepoRoot = '',
    [string]$OutputPath = '',
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [string]$GuardProject = 'src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj',
    [string]$FluentProject = 'src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj',
    [string]$ArtifactsSubdir = 'artifacts/audit',
    [string]$OutputFileName = 'compare-fluent-with-guard-clauses.txt',
    [string]$VocabularyPath = 'docs/ai/specs/language/vocabulary.json',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$artifactsDir = Join-Path $repoRootResolved $ArtifactsSubdir
Ensure-PineGuardDirectory -Path $artifactsDir

$guardProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $GuardProject
$fluentProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $FluentProject

if (-not (Test-Path $guardProjPath)) { throw "Guard project not found: $guardProjPath" }
if (-not (Test-Path $fluentProjPath)) { throw "Fluent project not found: $fluentProjPath" }

$tempPublishGuard = ''
$tempPublishFluent = ''
try {
    # 1. Publish both projects (self-contained folder for reflection)
    $tempPublishGuard = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-PubGuard-" + [System.Guid]::NewGuid().ToString('N'))
    $tempPublishFluent = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-PubFluent-" + [System.Guid]::NewGuid().ToString('N'))

    Write-Host "Publishing projects ($Configuration)..." -ForegroundColor Cyan
    & dotnet publish $guardProjPath -c $Configuration -o $tempPublishGuard -v quiet | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for: $guardProjPath" }

    & dotnet publish $fluentProjPath -c $Configuration -o $tempPublishFluent -v quiet | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for: $fluentProjPath" }

    $guardBin = Join-Path $tempPublishGuard 'PineGuard.GuardClauses.dll'
    $fluentBin = Join-Path $tempPublishFluent 'PineGuard.FluentValidation.dll'

    if (-not (Test-Path $guardBin)) { throw 'Guard DLL not found in publish output.' }
    if (-not (Test-Path $fluentBin)) { throw 'Fluent DLL not found in publish output.' }

    # 2. Optional structural comparison (informational only)
    $guardRoot = Join-Path $repoRootResolved 'src/PineGuard.GuardClauses'
    $fluentRoot = Join-Path $repoRootResolved 'src/PineGuard.FluentValidation'

    function Get-NormalizedFiles($rootPath, $pattern, $removePrefix, $removeSuffix, $removePathPrefix = '') {
        if (-not (Test-Path $rootPath)) { return @{} }
        $files = @(
            Get-ChildItem -Path $rootPath -Recurse -File -Filter $pattern
            | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
        )

        $result = @{}
        foreach ($f in $files) {
            $relPath = [System.IO.Path]::GetRelativePath($rootPath, $f.DirectoryName)
            if ($relPath -eq '.') { $relPath = '' }

            if (-not [string]::IsNullOrEmpty($removePathPrefix) -and $relPath.StartsWith($removePathPrefix)) {
                $relPath = $relPath.Substring($removePathPrefix.Length)
                if ($relPath.StartsWith('\\') -or $relPath.StartsWith('/')) {
                    $relPath = $relPath.Substring(1)
                }
            }

            $name = $f.Name
            if ($name.StartsWith($removePrefix)) { $name = $name.Substring($removePrefix.Length) }
            if ($name.EndsWith($removeSuffix)) { $name = $name.Substring(0, $name.Length - $removeSuffix.Length) }

            $key = if ($relPath) { "$relPath\\$name" } else { $name }
            $result[$key] = $f.FullName
        }
        return $result
    }

    $guardFiles = Get-NormalizedFiles -rootPath $guardRoot -pattern 'Guard*Clauses.cs' -removePrefix 'Guard' -removeSuffix 'Clauses.cs'
    $fluentFiles = Get-NormalizedFiles -rootPath $fluentRoot -pattern 'Fluent*Extensions.cs' -removePrefix 'Fluent' -removeSuffix 'Extensions.cs' -removePathPrefix 'Extensions'

    $allKeys = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    $guardFiles.Keys | ForEach-Object { [void]$allKeys.Add($_) }
    $fluentFiles.Keys | ForEach-Object { [void]$allKeys.Add($_) }

    $missingInGuard = @($allKeys | Where-Object { -not $guardFiles.ContainsKey($_) } | Sort-Object)
    $missingInFluent = @($allKeys | Where-Object { -not $fluentFiles.ContainsKey($_) } | Sort-Object)
    $fileDiscrepancies = $missingInGuard.Count + $missingInFluent.Count

    # 3. Method parity (enforced)
    $vocabularyFullPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $VocabularyPath
    $vocabulary = $null
    if (Test-Path $vocabularyFullPath) {
        try {
            $vocabulary = Get-Content -Path $vocabularyFullPath -Raw | ConvertFrom-Json
        }
        catch {
            throw "Failed to read vocabulary JSON at '$vocabularyFullPath'. Error: $($_.Exception.Message)"
        }
    }

    $stripPrefixes = @('Not')
    $ignoreMethods = @('MustBe')
    $aliases = @{}

    if ($null -ne $vocabulary) {
        if ($vocabulary.stripPrefixes) { $stripPrefixes = @($vocabulary.stripPrefixes) }
        if ($vocabulary.ignoreMethods) { $ignoreMethods = @($vocabulary.ignoreMethods) }

        if ($vocabulary.aliases) {
            foreach ($p in $vocabulary.aliases.PSObject.Properties) {
                $aliases[$p.Name] = [string]$p.Value
            }
        }
    }

    function Normalize-MethodName([string]$name) {
        if ([string]::IsNullOrWhiteSpace($name)) { return $null }
        if ($ignoreMethods -contains $name) { return $null }

        if ($aliases.ContainsKey($name)) {
            $name = $aliases[$name]
        }

        foreach ($prefix in $stripPrefixes) {
            if ($name.StartsWith($prefix)) {
                $name = $name.Substring($prefix.Length)
                break
            }
        }

        if ($aliases.ContainsKey($name)) {
            $name = $aliases[$name]
        }

        if ($ignoreMethods -contains $name) { return $null }
        return $name
    }

    $guardAsm = [System.Reflection.Assembly]::LoadFrom($guardBin)
    $fluentAsm = [System.Reflection.Assembly]::LoadFrom($fluentBin)

    $bindingFlags = [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static
    $extensionAttr = [System.Runtime.CompilerServices.ExtensionAttribute]

    function Get-ExtensionNames($asm, $namespacePrefix, $firstParamTypeMatch, [switch]$Normalize) {
        $names = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
        foreach ($t in $asm.GetTypes()) {
            if (-not ($t.IsClass -and $t.IsPublic -and ($t.Namespace -match "^$namespacePrefix"))) { continue }
            foreach ($m in $t.GetMethods($bindingFlags)) {
                if (-not $m.IsDefined($extensionAttr, $false)) { continue }
                $ps = $m.GetParameters()
                if ($ps.Length -eq 0) { continue }
                if ($ps[0].ParameterType.Name -match $firstParamTypeMatch) {
                    if ($Normalize.IsPresent) {
                        $normalized = Normalize-MethodName $m.Name
                        if ($null -ne $normalized) {
                            [void]$names.Add($normalized)
                        }
                    }
                    else {
                        [void]$names.Add($m.Name)
                    }
                }
            }
        }
        return $names
    }

    $guardMethodsRaw = Get-ExtensionNames -asm $guardAsm -namespacePrefix 'PineGuard.GuardClauses' -firstParamTypeMatch 'IGuardClause'
    $fluentMethodsRaw = Get-ExtensionNames -asm $fluentAsm -namespacePrefix 'PineGuard.FluentValidation' -firstParamTypeMatch 'IRuleBuilder'

    $guardMethods = Get-ExtensionNames -asm $guardAsm -namespacePrefix 'PineGuard.GuardClauses' -firstParamTypeMatch 'IGuardClause' -Normalize
    $fluentMethods = Get-ExtensionNames -asm $fluentAsm -namespacePrefix 'PineGuard.FluentValidation' -firstParamTypeMatch 'IRuleBuilder' -Normalize

    $allMethodNames = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
    $guardMethods | ForEach-Object { [void]$allMethodNames.Add($_) }
    $fluentMethods | ForEach-Object { [void]$allMethodNames.Add($_) }

    $missingMethodsInGuard = @($allMethodNames | Where-Object { -not $guardMethods.Contains($_) } | Sort-Object)
    $missingMethodsInFluent = @($allMethodNames | Where-Object { -not $fluentMethods.Contains($_) } | Sort-Object)
    $methodDiscrepancies = $missingMethodsInGuard.Count + $missingMethodsInFluent.Count

    # 4. Report
    $reportLines = @(
        "AuditRule: $AuditRuleId - FluentValidation \u2194 GuardClauses parity (concepts)",
        "Script: $($MyInvocation.MyCommand.Name)",
        "Date: $(Get-Date)",
        "RepoRoot: $repoRootResolved",
        "Configuration: $Configuration",
        "",
        'NOTE: Structural file parity is informational only; concept parity is enforced via normalized method names.',
        ''
    )

    $reportLines += @(
        '=== STRUCTURAL INFO (Files) ===',
        "Guard Root: $guardRoot",
        "Fluent Root: $fluentRoot",
        "Detected Guard Files: $($guardFiles.Count)",
        "Detected Fluent Files: $($fluentFiles.Count)",
        "File Discrepancies (informational): $fileDiscrepancies",
        ''
    )

    if ($missingInGuard.Count -gt 0) {
        $reportLines += 'MISSING IN GUARD (Present in Fluent):'
        $missingInGuard | ForEach-Object { $reportLines += "- $_" }
        $reportLines += ''
    }

    if ($missingInFluent.Count -gt 0) {
        $reportLines += 'MISSING IN FLUENT (Present in Guard):'
        $missingInFluent | ForEach-Object { $reportLines += "- $_" }
        $reportLines += ''
    }

    $reportLines += @(
        '=== CONCEPT PARITY (Public Extensions) ===',
        "Normalization: $VocabularyPath",
        "Guard Methods (raw): $($guardMethodsRaw.Count)",
        "Fluent Methods (raw): $($fluentMethodsRaw.Count)",
        "Guard Concepts (normalized): $($guardMethods.Count)",
        "Fluent Concepts (normalized): $($fluentMethods.Count)",
        "Method Discrepancies (enforced): $methodDiscrepancies",
        ''
    )

    if ($missingMethodsInGuard.Count -gt 0) {
        $reportLines += 'MISSING CONCEPTS IN GUARD (Present in Fluent):'
        $missingMethodsInGuard | ForEach-Object { $reportLines += "- $_" }
        $reportLines += ''
    }

    if ($missingMethodsInFluent.Count -gt 0) {
        $reportLines += 'MISSING CONCEPTS IN FLUENT (Present in Guard):'
        $missingMethodsInFluent | ForEach-Object { $reportLines += "- $_" }
        $reportLines += ''
    }

    if ($methodDiscrepancies -eq 0) {
        $reportLines += 'Concept parity is perfect.'
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
    Write-Host "File Discrepancies (info): $fileDiscrepancies" -ForegroundColor ($fileDiscrepancies -gt 0 ? 'Yellow' : 'Green')
    Write-Host "Method Discrepancies: $methodDiscrepancies" -ForegroundColor ($methodDiscrepancies -gt 0 ? 'Yellow' : 'Green')
    Write-Host "Report written to: $reportPath" -ForegroundColor DarkGray

    if ($FailOnFindings.IsPresent -and ($methodDiscrepancies -gt 0)) {
        throw "AuditRule $AuditRuleId failed: MethodDiscrepancies=$methodDiscrepancies. See report: $reportPath"
    }
}
finally {
    if (-not [string]::IsNullOrWhiteSpace($tempPublishGuard)) {
        Remove-Item $tempPublishGuard -Recurse -Force -ErrorAction SilentlyContinue
    }
    if (-not [string]::IsNullOrWhiteSpace($tempPublishFluent)) {
        Remove-Item $tempPublishFluent -Recurse -Force -ErrorAction SilentlyContinue
    }
}
