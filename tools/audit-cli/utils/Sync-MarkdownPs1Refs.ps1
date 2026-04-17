<#
.SYNOPSIS
    Sync Markdown Ps1 Refs

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER RepoRoot
    See the param block for details.

.PARAMETER ArtifactsSubdir
    See the param block for details.

.PARAMETER InventoryFileName
    See the param block for details.

.PARAMETER MissingFileName
    See the param block for details.

.PARAMETER Fix
    See the param block for details.
#>

[CmdletBinding()]
param(
    [string]$RepoRoot = '',
    [string]$ArtifactsSubdir = 'artifacts/audit',
    [string]$InventoryFileName = 'ps1-files.json',
    [string]$MissingFileName = 'md-ps1-missing.json',
    [switch]$Fix
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot
$artifactsDir = Join-Path $repoRootResolved $ArtifactsSubdir
Ensure-PineGuardDirectory -Path $artifactsDir

function Get-RepoRelativePath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$AbsolutePath
    )

    return [System.IO.Path]::GetRelativePath($repoRootResolved, $AbsolutePath).Replace('\', '/')
}

$ps1Files = @(
    Get-ChildItem -Path $repoRootResolved -Recurse -File -Filter '*.ps1'
    | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
    | ForEach-Object {
        [pscustomobject]@{
            path = (Get-RepoRelativePath -AbsolutePath $_.FullName)
            name = $_.Name
        }
    }
    | Sort-Object path
)

$inventoryPath = Join-Path $artifactsDir $InventoryFileName
$ps1Files | ConvertTo-Json -Depth 4 | Set-Content -Path $inventoryPath -Encoding UTF8

$existingPaths = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$existingNames = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
foreach ($f in $ps1Files) {
    [void]$existingPaths.Add($f.path)
    [void]$existingNames.Add($f.name)
}

# Match things like:
# - tools/testing/Run-Tests.ps1
# - .\\tools\\testing\\Run-Tests.ps1
# - "./tools/testing/Run-Tests.ps1"
# We intentionally don't try to be a full Markdown parser.
$pattern = '(?i)(?:^|[^A-Za-z0-9_./\\-])(?<ref>(?:\./|\.\\)?[A-Za-z0-9_][A-Za-z0-9_./\\-]*\.ps1)'

$mdFiles = @(
    Get-ChildItem -Path $repoRootResolved -Recurse -File -Filter '*.md'
    | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }
)

$missing = New-Object System.Collections.Generic.List[object]
$fixedCount = 0

foreach ($md in $mdFiles) {
    $relMdPath = Get-RepoRelativePath -AbsolutePath $md.FullName
    $content = Get-Content -LiteralPath $md.FullName -Raw
    $matches = [regex]::Matches($content, $pattern)

    if ($matches.Count -eq 0) {
        continue
    }

    $newContent = $content
    $anyChange = $false

    foreach ($m in $matches) {
        $raw = $m.Groups['ref'].Value

        $trimmed = $raw.Trim('"', "'", '`', '(', ')', '[', ']', '{', '}', '<', '>')
        $trimmed = $trimmed.TrimEnd('.', ',', ';', ':')
        $trimmed = $trimmed -replace '^\./', '' -replace '^\.\\', ''
        $normalized = $trimmed.Replace('\', '/')

        $isPathLike = $normalized.Contains('/')
        $exists = $false

        if ($isPathLike) {
            # Primary: repo-root relative paths.
            if ($existingPaths.Contains($normalized)) {
                $exists = $true
            }
            else {
                # Secondary: resolve relative to the markdown file's directory (e.g., tools/code-coverage/README.md -> xplat/*.ps1).
                $absCandidate = Join-Path $md.DirectoryName $trimmed
                if (Test-Path -LiteralPath $absCandidate) {
                    $exists = $true
                }
            }
        }
        else {
            $exists = $existingNames.Contains($normalized)
        }

        if ($exists) {
            continue
        }

        $suggestion = $null
        if ($normalized -match '(?i)RunTests\.ps1$') {
            $suggestion = ($normalized -replace '(?i)RunTests\.ps1$', 'Run-Tests.ps1')
            if ($isPathLike -and (-not $existingPaths.Contains($suggestion))) { $suggestion = $null }
            if (-not $isPathLike -and (-not $existingNames.Contains([System.IO.Path]::GetFileName($suggestion)))) { $suggestion = $null }
        }
        elseif ($normalized -match '(?i)Run-CodeCoverage\.ps1$') {
            $suggestion = ($normalized -replace '(?i)Run-CodeCoverage\.ps1$', 'Run-CodeCoverage.ps1')
            if ($isPathLike -and (-not $existingPaths.Contains($suggestion))) { $suggestion = $null }
            if (-not $isPathLike -and (-not $existingNames.Contains([System.IO.Path]::GetFileName($suggestion)))) { $suggestion = $null }
        }

        $missing.Add([pscustomobject]@{
                markdownPath = $relMdPath
                reference    = $raw
                normalized   = $normalized
                suggestion   = $suggestion
            })

        if ($Fix.IsPresent -and (-not [string]::IsNullOrWhiteSpace($suggestion))) {
            # Preserve leading ./ or .\ from the original ref
            $prefix = ''
            if ($raw.StartsWith('./')) { $prefix = './' }
            elseif ($raw.StartsWith('.\\')) { $prefix = './' }

            $replacement = $prefix + $suggestion

            # Replace exact raw token (not trimmed) to avoid over-replacing.
            if ($newContent.Contains($raw)) {
                $newContent = $newContent.Replace($raw, $replacement)
                $anyChange = $true
                $fixedCount++
            }
        }
    }

    if ($Fix.IsPresent -and $anyChange -and ($newContent -ne $content)) {
        Set-Content -LiteralPath $md.FullName -Value $newContent -Encoding UTF8
    }
}

$missingPath = Join-Path $artifactsDir $MissingFileName
$missing | ConvertTo-Json -Depth 6 | Set-Content -Path $missingPath -Encoding UTF8

Write-Host "PS1 inventory written: $inventoryPath" -ForegroundColor Green
Write-Host "MD missing refs written: $missingPath" -ForegroundColor Green
Write-Host "Missing refs: $($missing.Count)" -ForegroundColor Yellow
if ($Fix.IsPresent) {
    Write-Host "Auto-fixed refs: $fixedCount" -ForegroundColor Cyan
}
