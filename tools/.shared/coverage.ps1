<#
.SYNOPSIS
    Shared code coverage helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import coverage artifact path helpers, Coverlet RunSettings
    generation, and Cobertura XML parsing into the calling script's scope.
    Requires path.ps1 (Get-RepoRoot) to be loaded first.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Artifact Paths
# -------------------------------------------------------------------------------------------------

function Get-CodeCoverageArtifactsRoot {
    <#
    .SYNOPSIS
        Returns the artifacts/code-coverage path for the given repo root.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot
    )

    return (Join-Path $RepoRoot 'artifacts/code-coverage')
}

function Get-XplatArtifactsRoot {
    <#
    .SYNOPSIS
        Returns the artifacts/code-coverage/xplat path.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot
    )

    return (Join-Path (Get-CodeCoverageArtifactsRoot -RepoRoot $RepoRoot) 'xplat')
}

# -------------------------------------------------------------------------------------------------
# Coverlet RunSettings
# -------------------------------------------------------------------------------------------------

function Write-CoverletRunSettings {
    <#
    .SYNOPSIS
        Generates a .runsettings XML file for Coverlet code coverage configuration.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $OutputPath,

        [Parameter(Mandatory)]
        [string[]] $IncludePatterns,

        [ValidateSet('cobertura', 'opencover')]
        [string] $Format = 'cobertura'
    )

    $includeValue = ($IncludePatterns -join ';')

    $xml = @"
<?xml version="1.0" encoding="utf-8"?>
<RunSettings>
    <DataCollectionRunSettings>
        <DataCollectors>
            <DataCollector friendlyName="XPlat Code Coverage">
                <Configuration>
                    <Format>$Format</Format>
                    <!-- Explicitly include the assemblies for the requested scope to keep coverage focused and avoid collector regressions -->
                    <Include>$includeValue</Include>
                    <!-- Exclude build artifacts and generated sources from coverage to keep reports stable -->
                    <ExcludeByFile>**/obj/**;**\\obj\\**;**/bin/**;**\\bin\\**</ExcludeByFile>
                    <!-- Exclude compiler/source-generated code (including GeneratedRegex output) -->
                    <ExcludeByAttribute>GeneratedCodeAttribute;CompilerGeneratedAttribute;ExcludeFromCodeCoverageAttribute</ExcludeByAttribute>
                    <!-- Exclude RegexGenerator output which skews coverage -->
                    <Exclude>[*]System.Text.RegularExpressions.Generated.*</Exclude>
                </Configuration>
            </DataCollector>
        </DataCollectors>
    </DataCollectionRunSettings>
</RunSettings>
"@

    Set-Content -LiteralPath $OutputPath -Value $xml -Encoding UTF8
}

# -------------------------------------------------------------------------------------------------
# Cobertura Parsing
# -------------------------------------------------------------------------------------------------

function Get-LatestCoverageFiles {
    <#
    .SYNOPSIS
        Returns the newest coverage.cobertura.xml file per test-project folder.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $ResultsRoot
    )

    $files = @(
        Get-ChildItem -LiteralPath $ResultsRoot -Recurse -File -Filter 'coverage.cobertura.xml' -ErrorAction Stop
    )
    if (-not $files -or $files.Count -eq 0) {
        throw "No 'coverage.cobertura.xml' files found under: $ResultsRoot"
    }

    return @(
        $files |
        Group-Object { Split-Path -Parent (Split-Path -Parent $_.FullName) } |
        ForEach-Object { $_.Group | Sort-Object LastWriteTime -Descending | Select-Object -First 1 } |
        Sort-Object FullName
    )
}

function ConvertTo-Rate {
    <#
    .SYNOPSIS
        Converts a percentage (>1.0) to a decimal rate, or returns as-is if already <=1.0.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [double] $Value
    )

    if ($Value -gt 1.0) {
        return ($Value / 100.0)
    }

    return $Value
}

function Try-ParseConditionCoverage {
    <#
    .SYNOPSIS
        Parses "50% (1/2)" format from Cobertura condition-coverage; returns [covered, total].
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $ConditionCoverage
    )

    if ($ConditionCoverage -match '\((\d+)\/(\d+)\)') {
        return @([int]$Matches[1], [int]$Matches[2])
    }

    return $null
}

function Normalize-CoberturaFilename {
    <#
    .SYNOPSIS
        Normalizes Cobertura class filenames to repo-relative paths.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $RepoRoot,

        [Parameter(Mandatory)]
        [string] $CoberturaFilename,

        [string] $DefaultSourcePrefix = 'src\PineGuard.Core'
    )

    $matchFilename = ($CoberturaFilename -replace '/', '\')

    $repoRootNormalized = $RepoRoot.TrimEnd([char]'\', [char]'/')
    $repoRootWithSep = $repoRootNormalized + '\'

    if ($matchFilename.StartsWith($repoRootWithSep, [System.StringComparison]::OrdinalIgnoreCase)) {
        $matchFilename = $matchFilename.Substring($repoRootWithSep.Length)
    }
    else {
        if ($repoRootNormalized.Length -ge 3 -and $repoRootNormalized[1] -eq ':' -and $repoRootNormalized[2] -eq '\') {
            $repoRootNoDriveWithSep = $repoRootNormalized.Substring(3)
            if (-not [string]::IsNullOrWhiteSpace($repoRootNoDriveWithSep)) {
                $repoRootNoDriveWithSep = $repoRootNoDriveWithSep.TrimStart([char]'\') + '\'
                if ($matchFilename.StartsWith($repoRootNoDriveWithSep, [System.StringComparison]::OrdinalIgnoreCase)) {
                    $matchFilename = $matchFilename.Substring($repoRootNoDriveWithSep.Length)
                }
            }
        }
    }

    $matchFilename = [regex]::Replace(
        $matchFilename,
        '^(?i).*?[\\/](?=(src|tests)[\\/])',
        ''
    )

    if ((-not [System.IO.Path]::IsPathRooted($matchFilename)) -and ($matchFilename -notmatch '^(src|tests)[\\/]+')) {
        if (-not (Test-Path variable:script:CoberturaFilenameResolutionCache)) {
            $script:CoberturaFilenameResolutionCache = @{}
        }

        $cacheKey = $repoRootNormalized + '|' + $DefaultSourcePrefix + '|' + $matchFilename
        if ($script:CoberturaFilenameResolutionCache.ContainsKey($cacheKey)) {
            return $script:CoberturaFilenameResolutionCache[$cacheKey]
        }

        $resolved = $null

        $prefixes = @($DefaultSourcePrefix, 'src\PineGuard.Core', 'src\PineGuard.MustClauses', 'src\PineGuard.GuardClauses', 'src\PineGuard.DataAnnotations', 'src\PineGuard.FluentValidation', 'tests\PineGuard.Testing') | Select-Object -Unique
        foreach ($prefix in $prefixes) {
            $candidate = Join-Path $prefix $matchFilename
            if (Test-Path -LiteralPath (Join-Path $repoRootNormalized $candidate)) {
                $resolved = $candidate
                break
            }

            $prefixName = Split-Path $prefix -Leaf
            if ($matchFilename.StartsWith($prefixName + '\', [System.StringComparison]::OrdinalIgnoreCase)) {
                $stripped = $matchFilename.Substring($prefixName.Length + 1)
                $candidateStripped = Join-Path $prefix $stripped
                if (Test-Path -LiteralPath (Join-Path $repoRootNormalized $candidateStripped)) {
                    $resolved = $candidateStripped
                    break
                }
            }
        }

        if (-not $resolved) {
            $resolved = Join-Path $DefaultSourcePrefix $matchFilename
        }

        $script:CoberturaFilenameResolutionCache[$cacheKey] = $resolved
        return $resolved
    }

    return $matchFilename
}

function Read-CoberturaCoverage {
    <#
    .SYNOPSIS
        Parses Cobertura XML coverage files; aggregates class coverage; applies include/exclude filters.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [System.IO.FileInfo[]] $CoverageFiles,

        [Parameter(Mandatory)]
        [string] $RepoRoot,

        [string] $IncludeFileRegex,
        [string] $ExcludeFileRegex,
        [string] $IncludeClassNameRegex,
        [string] $ExcludeClassNameRegex,

        [string] $DefaultSourcePrefix = 'src\PineGuard.Core'
    )

    $classMap = @{}

    foreach ($coverageFile in $CoverageFiles) {
        $settings = New-Object System.Xml.XmlReaderSettings
        $settings.IgnoreComments = $true
        $settings.IgnoreWhitespace = $false
        $settings.DtdProcessing = [System.Xml.DtdProcessing]::Ignore

        $stream = [System.IO.File]::OpenRead($coverageFile.FullName)
        try {
            $reader = [System.Xml.XmlReader]::Create($stream, $settings)

            while ($reader.Read()) {
                if ($reader.NodeType -ne [System.Xml.XmlNodeType]::Element -or $reader.Name -ne 'class') {
                    continue
                }

                $className = [string]$reader.GetAttribute('name')
                $filenameRaw = [string]$reader.GetAttribute('filename')
                if ([string]::IsNullOrWhiteSpace($filenameRaw)) {
                    $reader.Skip()
                    continue
                }

                if ($IncludeClassNameRegex -and ($className -notmatch $IncludeClassNameRegex)) {
                    $reader.Skip()
                    continue
                }

                if ($ExcludeClassNameRegex -and ($className -match $ExcludeClassNameRegex)) {
                    $reader.Skip()
                    continue
                }

                $matchFilename = Normalize-CoberturaFilename -RepoRoot $RepoRoot -CoberturaFilename $filenameRaw -DefaultSourcePrefix $DefaultSourcePrefix

                if ($IncludeFileRegex -and ($matchFilename -notmatch $IncludeFileRegex)) {
                    $reader.Skip()
                    continue
                }

                if ($ExcludeFileRegex -and ($matchFilename -match $ExcludeFileRegex)) {
                    $reader.Skip()
                    continue
                }

                $classKey = "$className|$matchFilename"

                if (-not $classMap.ContainsKey($classKey)) {
                    $classMap[$classKey] = [pscustomobject]@{
                        Name     = $className
                        File     = $matchFilename
                        Lines    = @{}
                        Branches = @{}
                    }
                }

                $classAgg = $classMap[$classKey]

                $classSub = $reader.ReadSubtree()
                try {
                    [void]$classSub.Read()

                    while ($classSub.Read()) {
                        if ($classSub.NodeType -ne [System.Xml.XmlNodeType]::Element -or $classSub.Name -ne 'line') {
                            continue
                        }

                        $lineNumber = 0
                        [void][int]::TryParse([string]$classSub.GetAttribute('number'), [ref]$lineNumber)
                        if ($lineNumber -le 0) {
                            continue
                        }

                        $hits = 0
                        [void][int]::TryParse([string]$classSub.GetAttribute('hits'), [ref]$hits)

                        if (-not $classAgg.Lines.ContainsKey($lineNumber)) {
                            $classAgg.Lines[$lineNumber] = $false
                        }

                        if ($hits -gt 0) {
                            $classAgg.Lines[$lineNumber] = $true
                        }

                        $isBranch = ([string]$classSub.GetAttribute('branch') -eq 'true')
                        if (-not $isBranch) {
                            continue
                        }

                        if (-not $classAgg.Branches.ContainsKey($lineNumber)) {
                            $classAgg.Branches[$lineNumber] = [pscustomobject]@{
                                Conditions = @{}
                                Fallback   = $null
                            }
                        }

                        $branchAgg = $classAgg.Branches[$lineNumber]

                        $condCoverage = [string]$classSub.GetAttribute('condition-coverage')

                        $lineSub = $classSub.ReadSubtree()
                        try {
                            [void]$lineSub.Read()

                            while ($lineSub.Read()) {
                                if ($lineSub.NodeType -eq [System.Xml.XmlNodeType]::Element -and $lineSub.Name -eq 'condition') {

                                    $condNumber = [string]$lineSub.GetAttribute('number')
                                    if ([string]::IsNullOrWhiteSpace($condNumber)) {
                                        $condNumber = [Guid]::NewGuid().ToString('N')
                                    }

                                    $coverage = [string]$lineSub.GetAttribute('coverage')
                                    $covered = ($coverage -and ($coverage -ne '0%'))

                                    if (-not $branchAgg.Conditions.ContainsKey($condNumber)) {
                                        $branchAgg.Conditions[$condNumber] = $false
                                    }

                                    if ($covered) {
                                        $branchAgg.Conditions[$condNumber] = $true
                                    }
                                }
                            }

                            if (-not [string]::IsNullOrWhiteSpace($condCoverage)) {
                                $parsed = Try-ParseConditionCoverage -ConditionCoverage $condCoverage
                                if ($null -ne $parsed) {
                                    $coveredCount = $parsed[0]
                                    $totalCount = $parsed[1]

                                    if ($null -eq $branchAgg.Fallback) {
                                        $branchAgg.Fallback = [pscustomobject]@{ Covered = $coveredCount; Total = $totalCount }
                                    }
                                    else {
                                        if ($branchAgg.Fallback.Total -eq $totalCount) {
                                            $branchAgg.Fallback.Covered = [Math]::Max([int]$branchAgg.Fallback.Covered, [int]$coveredCount)
                                        }
                                        else {
                                            $branchAgg.Fallback.Total = [Math]::Max([int]$branchAgg.Fallback.Total, [int]$totalCount)
                                            $branchAgg.Fallback.Covered = [Math]::Max([int]$branchAgg.Fallback.Covered, [int]$coveredCount)
                                        }
                                    }
                                }
                            }
                        }
                        finally {
                            $lineSub.Dispose()
                        }
                    }
                }
                finally {
                    $classSub.Dispose()
                }

                $reader.Skip()
            }
        }
        finally {
            $stream.Dispose()
        }
    }

    $classes = foreach ($entry in $classMap.Values) {
        $linesTotal = $entry.Lines.Count
        $linesCovered = @($entry.Lines.GetEnumerator() | Where-Object { $_.Value }).Count

        $branchesTotal = 0
        $branchesCovered = 0

        foreach ($b in $entry.Branches.GetEnumerator()) {
            $branch = $b.Value

            if ($null -ne $branch.Fallback) {
                $branchesTotal += [int]$branch.Fallback.Total
                $branchesCovered += [int]$branch.Fallback.Covered
                continue
            }

            if ($branch.Conditions.Count -gt 0) {
                $branchesTotal += $branch.Conditions.Count
                $branchesCovered += @($branch.Conditions.GetEnumerator() | Where-Object { $_.Value }).Count
                continue
            }
        }

        $lineRate = if ($linesTotal -gt 0) { [double]$linesCovered / [double]$linesTotal } else { 0.0 }
        $branchRate = if ($branchesTotal -gt 0) { [double]$branchesCovered / [double]$branchesTotal } else { 1.0 }

        [pscustomobject]@{
            LineRate        = $lineRate
            BranchRate      = $branchRate
            LinesCovered    = [int]$linesCovered
            LinesTotal      = [int]$linesTotal
            BranchesCovered = [int]$branchesCovered
            BranchesTotal   = [int]$branchesTotal
            Name            = [string]$entry.Name
            File            = [string]$entry.File
        }
    }

    return @($classes)
}
