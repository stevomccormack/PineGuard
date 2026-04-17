<#
.SYNOPSIS
    Compare adapter concept parity against MustClauses.

.DESCRIPTION
    Publishes PineGuard.MustClauses (canonical surface) and compares its public "concepts" against:
      - PineGuard.GuardClauses (extension methods on IGuardClause)
      - PineGuard.FluentValidation (extension methods on IRuleBuilder)
      - PineGuard.DataAnnotations (ValidationAttribute types)

    Parity is evaluated at the normalized concept level using docs/ai/specs/language/vocabulary.json.
    Structural file parity is reported for context but is NOT enforced. Adapter layers may aggregate
    concepts without mirroring internal domain folder structures.

.PARAMETER AuditRuleId
    Audit rule identifier for reporting.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.

.PARAMETER OutputPath
    Optional explicit output path.

.PARAMETER Configuration
    Build configuration for publishing.

.PARAMETER MustProject
    Relative path to PineGuard.MustClauses project.

.PARAMETER GuardProject
    Relative path to PineGuard.GuardClauses project.

.PARAMETER FluentProject
    Relative path to PineGuard.FluentValidation project.

.PARAMETER DataAnnotationsProject
    Relative path to PineGuard.DataAnnotations project.

.PARAMETER ArtifactsSubdir
    Relative artifacts directory.

.PARAMETER OutputFileName
    Output filename when OutputPath is not provided.

.PARAMETER VocabularyPath
    Path to normalization vocabulary JSON.

.PARAMETER FailOnFindings
    If set, throws when any parity violations are found.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string]$AuditRuleId = 'Rule06',
    [string]$RepoRoot = '',
    [string]$OutputPath = '',
    [ValidateSet('Debug', 'Release')] [string]$Configuration = 'Release',
    [string]$MustProject = 'src/PineGuard.MustClauses/PineGuard.MustClauses.csproj',
    [string]$GuardProject = 'src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj',
    [string]$FluentProject = 'src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj',
    [string]$DataAnnotationsProject = 'src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj',
    [string]$ArtifactsSubdir = 'artifacts/audit',
    [string]$OutputFileName = 'compare-adapters-with-must-clauses.txt',
    [string]$VocabularyPath = 'docs/ai/specs/language/vocabulary.json',
    [switch]$FailOnFindings
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$artifactsDir = Join-Path $repoRootResolved $ArtifactsSubdir
Ensure-PineGuardDirectory -Path $artifactsDir

$mustProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $MustProject
$guardProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $GuardProject
$fluentProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $FluentProject
$dataProjPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $DataAnnotationsProject

if (-not (Test-Path $mustProjPath)) { throw "Must project not found: $mustProjPath" }
if (-not (Test-Path $guardProjPath)) { throw "Guard project not found: $guardProjPath" }
if (-not (Test-Path $fluentProjPath)) { throw "Fluent project not found: $fluentProjPath" }
if (-not (Test-Path $dataProjPath)) { throw "DataAnnotations project not found: $dataProjPath" }

$tempPublishMust = ''
$tempPublishGuard = ''
$tempPublishFluent = ''
$tempPublishData = ''

try {
    # 1) Publish projects (self-contained folder for reflection)
    $tempPublishMust = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-PubMust-" + [System.Guid]::NewGuid().ToString('N'))
    $tempPublishGuard = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-PubGuard-" + [System.Guid]::NewGuid().ToString('N'))
    $tempPublishFluent = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-PubFluent-" + [System.Guid]::NewGuid().ToString('N'))
    $tempPublishData = Join-Path ([System.IO.Path]::GetTempPath()) ("PineGuard-Audit-PubDA-" + [System.Guid]::NewGuid().ToString('N'))

    Write-Host "Publishing projects ($Configuration)..." -ForegroundColor Cyan

    & dotnet publish $mustProjPath -c $Configuration -o $tempPublishMust -v quiet | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for: $mustProjPath" }

    & dotnet publish $guardProjPath -c $Configuration -o $tempPublishGuard -v quiet | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for: $guardProjPath" }

    & dotnet publish $fluentProjPath -c $Configuration -o $tempPublishFluent -v quiet | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for: $fluentProjPath" }

    & dotnet publish $dataProjPath -c $Configuration -o $tempPublishData -v quiet | Out-Host
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for: $dataProjPath" }

    $mustBin = Join-Path $tempPublishMust 'PineGuard.MustClauses.dll'
    $guardBin = Join-Path $tempPublishGuard 'PineGuard.GuardClauses.dll'
    $fluentBin = Join-Path $tempPublishFluent 'PineGuard.FluentValidation.dll'
    $dataBin = Join-Path $tempPublishData 'PineGuard.DataAnnotations.dll'

    if (-not (Test-Path $mustBin)) { throw 'Must DLL not found in publish output.' }
    if (-not (Test-Path $guardBin)) { throw 'Guard DLL not found in publish output.' }
    if (-not (Test-Path $fluentBin)) { throw 'Fluent DLL not found in publish output.' }
    if (-not (Test-Path $dataBin)) { throw 'DataAnnotations DLL not found in publish output.' }

    # 2) Optional structural comparison (informational only)
    $mustRoot = Join-Path $repoRootResolved 'src/PineGuard.MustClauses'
    $guardRoot = Join-Path $repoRootResolved 'src/PineGuard.GuardClauses'
    $fluentRoot = Join-Path $repoRootResolved 'src/PineGuard.FluentValidation'
    $dataRoot = Join-Path $repoRootResolved 'src/PineGuard.DataAnnotations'

    function Get-NormalizedFiles(
        [string]$RootPath,
        [string]$Pattern,
        [string]$RemovePrefix,
        [string]$RemoveSuffix,
        [string]$RemovePathPrefix = ''
    ) {
        if (-not (Test-Path $RootPath)) { return @{} }

        $files = @(
            Get-ChildItem -Path $RootPath -Recurse -File -Filter $Pattern |
                Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
        )

        $result = @{}
        foreach ($f in $files) {
            $relPath = [System.IO.Path]::GetRelativePath($RootPath, $f.DirectoryName)
            if ($relPath -eq '.') { $relPath = '' }

            if (-not [string]::IsNullOrEmpty($RemovePathPrefix) -and $relPath.StartsWith($RemovePathPrefix)) {
                $relPath = $relPath.Substring($RemovePathPrefix.Length)
                if ($relPath.StartsWith('\\') -or $relPath.StartsWith('/')) {
                    $relPath = $relPath.Substring(1)
                }
            }

            $name = $f.Name
            if ($name.StartsWith($RemovePrefix)) { $name = $name.Substring($RemovePrefix.Length) }
            if ($name.EndsWith($RemoveSuffix)) { $name = $name.Substring(0, $name.Length - $RemoveSuffix.Length) }

            $key = if ($relPath) { "$relPath\\$name" } else { $name }
            $result[$key] = $f.FullName
        }

        return $result
    }

    $mustFiles = Get-NormalizedFiles -RootPath $mustRoot -Pattern 'Must*Clauses.cs' -RemovePrefix 'Must' -RemoveSuffix 'Clauses.cs'
    $guardFiles = Get-NormalizedFiles -RootPath $guardRoot -Pattern 'Guard*Clauses.cs' -RemovePrefix 'Guard' -RemoveSuffix 'Clauses.cs'
    $fluentFiles = Get-NormalizedFiles -RootPath $fluentRoot -Pattern 'Fluent*Extensions.cs' -RemovePrefix 'Fluent' -RemoveSuffix 'Extensions.cs' -RemovePathPrefix 'Extensions'

    function Compare-FileKeys([hashtable]$Left, [hashtable]$Right) {
        $allKeys = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
        $Left.Keys | ForEach-Object { [void]$allKeys.Add($_) }
        $Right.Keys | ForEach-Object { [void]$allKeys.Add($_) }

        $missingInLeft = @($allKeys | Where-Object { -not $Left.ContainsKey($_) } | Sort-Object)
        $missingInRight = @($allKeys | Where-Object { -not $Right.ContainsKey($_) } | Sort-Object)

        [pscustomobject]@{
            MissingInLeft = $missingInLeft
            MissingInRight = $missingInRight
            Discrepancies = ($missingInLeft.Count + $missingInRight.Count)
        }
    }

    $filesMustVsGuard = Compare-FileKeys -Left $mustFiles -Right $guardFiles
    $filesMustVsFluent = Compare-FileKeys -Left $mustFiles -Right $fluentFiles

    # 3) Concept parity (enforced)
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

    function Normalize-ConceptName([string]$name) {
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

    $mustAsm = [System.Reflection.Assembly]::LoadFrom($mustBin)
    $guardAsm = [System.Reflection.Assembly]::LoadFrom($guardBin)
    $fluentAsm = [System.Reflection.Assembly]::LoadFrom($fluentBin)
    $dataAsm = [System.Reflection.Assembly]::LoadFrom($dataBin)

    $bindingFlags = [System.Reflection.BindingFlags]::Public -bor [System.Reflection.BindingFlags]::Static
    $extensionAttr = [System.Runtime.CompilerServices.ExtensionAttribute]

    function Get-ExtensionConcepts(
        $Asm,
        [string]$NamespacePrefix,
        [string]$FirstParamTypeMatch
    ) {
        $raw = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
        $norm = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)

        foreach ($t in $Asm.GetTypes()) {
            if (-not ($t.IsClass -and $t.IsPublic -and ($t.Namespace -match "^$NamespacePrefix"))) { continue }
            foreach ($m in $t.GetMethods($bindingFlags)) {
                if (-not $m.IsDefined($extensionAttr, $false)) { continue }
                $ps = $m.GetParameters()
                if ($ps.Length -eq 0) { continue }
                if ($ps[0].ParameterType.Name -match $FirstParamTypeMatch) {
                    [void]$raw.Add($m.Name)
                    $normalized = Normalize-ConceptName $m.Name
                    if ($null -ne $normalized) {
                        [void]$norm.Add($normalized)
                    }
                }
            }
        }

        [pscustomobject]@{
            Raw = $raw
            Normalized = $norm
        }
    }

    function Get-DataAnnotationsConceptsFromMustCalls([string]$ProjectRootPath) {
        $raw = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
        $norm = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)

        if (-not (Test-Path $ProjectRootPath)) {
            return [pscustomobject]@{ Raw = $raw; Normalized = $norm }
        }

        $csFiles = @(
            Get-ChildItem -Path $ProjectRootPath -Recurse -File -Filter '*.cs' |
                Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
        )

        foreach ($file in $csFiles) {
            $text = Get-Content -LiteralPath $file.FullName -Raw
            foreach ($m in [regex]::Matches($text, 'Must\.Be\.(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*\(', [System.Text.RegularExpressions.RegexOptions]::None)) {
                $name = $m.Groups['name'].Value
                if ([string]::IsNullOrWhiteSpace($name)) { continue }
                [void]$raw.Add($name)
                $normalized = Normalize-ConceptName $name
                if ($null -ne $normalized) {
                    [void]$norm.Add($normalized)
                }
            }
        }

        [pscustomobject]@{
            Raw = $raw
            Normalized = $norm
        }
    }

    $must = Get-ExtensionConcepts -Asm $mustAsm -NamespacePrefix 'PineGuard.MustClauses' -FirstParamTypeMatch 'IMustClause'
    $guard = Get-ExtensionConcepts -Asm $guardAsm -NamespacePrefix 'PineGuard.GuardClauses' -FirstParamTypeMatch 'IGuardClause'
    $fluent = Get-ExtensionConcepts -Asm $fluentAsm -NamespacePrefix 'PineGuard.FluentValidation' -FirstParamTypeMatch 'IRuleBuilder'
    # DataAnnotations attribute names intentionally diverge from Must clause names (e.g., *StringAttribute).
    # Derive adapter concepts from actual Must.Be.* calls in the integration source to ensure parity checks
    # are anchored on the canonical Must surface.
    $data = Get-DataAnnotationsConceptsFromMustCalls -ProjectRootPath $dataRoot

    function Diff-FromMust([System.Collections.Generic.HashSet[string]]$MustConcepts, [System.Collections.Generic.HashSet[string]]$OtherConcepts) {
        $missing = @($MustConcepts | Where-Object { -not $OtherConcepts.Contains($_) } | Sort-Object)
        $extra = @($OtherConcepts | Where-Object { -not $MustConcepts.Contains($_) } | Sort-Object)
        [pscustomobject]@{
            Missing = $missing
            Extra = $extra
            Discrepancies = ($missing.Count + $extra.Count)
        }
    }

    $diffGuard = Diff-FromMust -MustConcepts $must.Normalized -OtherConcepts $guard.Normalized
    $diffFluent = Diff-FromMust -MustConcepts $must.Normalized -OtherConcepts $fluent.Normalized
    $diffData = Diff-FromMust -MustConcepts $must.Normalized -OtherConcepts $data.Normalized

    # Enforcement policy:
    # - Guard and FluentValidation are expected to match Must exactly (both directions).
    # - DataAnnotations is an adapter layer whose coverage may be partial; enforce only that it does not
    #   reference concepts that do not exist in Must.
    $dataEnforcedDiscrepancies = $diffData.Extra.Count
    $totalDiscrepancies = $diffGuard.Discrepancies + $diffFluent.Discrepancies + $dataEnforcedDiscrepancies

    # 4) Report
    $reportLines = @(
        "AuditRule: $AuditRuleId - Adapters <-> MustClauses parity (concepts)",
        "Script: $($MyInvocation.MyCommand.Name)",
        "Date: $(Get-Date)",
        "RepoRoot: $repoRootResolved",
        "Configuration: $Configuration",
        '',
        'NOTE: Structural file parity is informational only; concept parity is enforced via normalized concept names.',
        ''
    )

    $reportLines += @(
        '=== STRUCTURAL INFO (Files, informational) ===',
        "Must Root: $mustRoot",
        "Guard Root: $guardRoot",
        "Fluent Root: $fluentRoot",
        "DataAnnotations Root: $dataRoot",
        "Detected Must Files: $($mustFiles.Count)",
        "Detected Guard Files: $($guardFiles.Count)",
        "Detected Fluent Files: $($fluentFiles.Count)",
        'Detected DataAnnotations Files: (skipped)',
        '',
        "Must <-> Guard file discrepancies (info): $($filesMustVsGuard.Discrepancies)",
        "Must <-> Fluent file discrepancies (info): $($filesMustVsFluent.Discrepancies)",
        'Must <-> DataAnnotations file discrepancies (info): skipped (adapter types are not expected to mirror Must file structure)',
        ''
    )

    function Add-FileDiscrepancies([string]$Title, $cmp) {
        $localLines = @(
            "--- $Title ---",
            "Missing in left (Must): $($cmp.MissingInLeft.Count)",
            "Missing in right (Adapter): $($cmp.MissingInRight.Count)",
            ''
        )

        if ($cmp.MissingInLeft.Count -gt 0) {
            $localLines += 'MISSING IN MUST (Present in Adapter):'
            $cmp.MissingInLeft | ForEach-Object { $localLines += "- $_" }
            $localLines += ''
        }

        if ($cmp.MissingInRight.Count -gt 0) {
            $localLines += 'MISSING IN ADAPTER (Present in Must):'
            $cmp.MissingInRight | ForEach-Object { $localLines += "- $_" }
            $localLines += ''
        }

        return $localLines
    }

    $reportLines += Add-FileDiscrepancies -Title 'FILES: Must vs Guard' -cmp $filesMustVsGuard
    $reportLines += Add-FileDiscrepancies -Title 'FILES: Must vs FluentValidation' -cmp $filesMustVsFluent
    $reportLines += @(
        '--- FILES: Must vs DataAnnotations ---',
        'SKIPPED: DataAnnotations is a type-based adapter layer and does not imply file parity with Must.',
        ''
    )

    $reportLines += @(
        '=== CONCEPT PARITY (enforced) ===',
        "Normalization: $VocabularyPath",
        "Must Methods (raw): $($must.Raw.Count)",
        "Guard Methods (raw): $($guard.Raw.Count)",
        "Fluent Methods (raw): $($fluent.Raw.Count)",
        "DataAnnotations Must calls (raw): $($data.Raw.Count)",
        "Must Concepts (normalized): $($must.Normalized.Count)",
        "Guard Concepts (normalized): $($guard.Normalized.Count)",
        "Fluent Concepts (normalized): $($fluent.Normalized.Count)",
        "DataAnnotations Concepts (normalized): $($data.Normalized.Count)",
        "Total Concept Discrepancies (enforced): $totalDiscrepancies",
        ''
    )

    function Add-ConceptDiff([string]$Title, $diff) {
        $localLines = @(
            "--- $Title ---",
            "Discrepancies: $($diff.Discrepancies)",
            "Missing vs Must: $($diff.Missing.Count)",
            "Extra vs Must: $($diff.Extra.Count)",
            ''
        )

        if ($diff.Missing.Count -gt 0) {
            $localLines += 'MISSING CONCEPTS (Present in Must):'
            $diff.Missing | ForEach-Object { $localLines += "- $_" }
            $localLines += ''
        }

        if ($diff.Extra.Count -gt 0) {
            $localLines += 'EXTRA CONCEPTS (Not present in Must):'
            $diff.Extra | ForEach-Object { $localLines += "- $_" }
            $localLines += ''
        }

        return $localLines
    }

    $reportLines += Add-ConceptDiff -Title 'CONCEPTS: GuardClauses vs MustClauses' -diff $diffGuard
    $reportLines += Add-ConceptDiff -Title 'CONCEPTS: FluentValidation vs MustClauses' -diff $diffFluent
    $reportLines += Add-ConceptDiff -Title 'CONCEPTS: DataAnnotations vs MustClauses (missing coverage is informational; extra concepts are enforced)' -diff $diffData

    if ($totalDiscrepancies -eq 0) {
        $reportLines += 'Concept parity is perfect across Guard/FluentValidation/DataAnnotations against Must.'
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
    Write-Host "Total Concept Discrepancies: $totalDiscrepancies" -ForegroundColor ($totalDiscrepancies -gt 0 ? 'Yellow' : 'Green')
    Write-Host "Report written to: $reportPath" -ForegroundColor DarkGray

    if ($FailOnFindings.IsPresent -and ($totalDiscrepancies -gt 0)) {
        throw "AuditRule $AuditRuleId failed: TotalConceptDiscrepancies=$totalDiscrepancies. See report: $reportPath"
    }
}
finally {
    if (-not [string]::IsNullOrWhiteSpace($tempPublishMust)) {
        Remove-Item $tempPublishMust -Recurse -Force -ErrorAction SilentlyContinue
    }
    if (-not [string]::IsNullOrWhiteSpace($tempPublishGuard)) {
        Remove-Item $tempPublishGuard -Recurse -Force -ErrorAction SilentlyContinue
    }
    if (-not [string]::IsNullOrWhiteSpace($tempPublishFluent)) {
        Remove-Item $tempPublishFluent -Recurse -Force -ErrorAction SilentlyContinue
    }
    if (-not [string]::IsNullOrWhiteSpace($tempPublishData)) {
        Remove-Item $tempPublishData -Recurse -Force -ErrorAction SilentlyContinue
    }
}
