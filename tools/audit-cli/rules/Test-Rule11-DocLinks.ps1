<#
.SYNOPSIS
    Test Rule11 Doc Links

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule11' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule11." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule11' -Title 'Documentation link + script path resolution' -RepoRoot $repoRootResolved -OutputPath $outputPath

function Convert-ToRepoRelativePath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$Path
    )

    $rel = [System.IO.Path]::GetRelativePath($RepoRoot, $Path)
    $rel.Replace('\\', '/').Replace('\', '/')
}

function Get-TrackedMarkdownFile {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot
    )

    $tracked = @()

    try {
        $tracked = @(& git -C $RepoRoot ls-files '*.md' '**/*.md' 2>$null)
    }
    catch {
        $tracked = @()
    }

    $tracked = @($tracked | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | Select-Object -Unique)

    if ($tracked.Count -gt 0) {
        return @(
            $tracked | ForEach-Object { Join-Path $RepoRoot $_ } | Where-Object { Test-Path -LiteralPath $_ }
        )
    }

    @(
        Get-ChildItem -LiteralPath $RepoRoot -Recurse -File -Filter '*.md' |
            Where-Object {
                $rel = Convert-ToRepoRelativePath -RepoRoot $RepoRoot -Path $_.FullName
                -not (
                    $rel -match '(?i)^(artifacts|logs|\.etc|\.git|\.vs)/' -or
                    $rel -match '(?i)(^|/)(bin|obj|node_modules)(/|$)'
                )
            } |
            Select-Object -ExpandProperty FullName
    )
}

function ConvertTo-NormalizedReference {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][AllowEmptyString()][string]$Value
    )

    $v = $Value.Trim()
    $v = $v.Trim('<', '>', '"', "'", '`', '(', ')', '[', ']', ',', ';', ':')
    $v = $v.Replace('\', '/')

    $hash = $v.IndexOf('#')
    if ($hash -ge 0) { $v = $v.Substring(0, $hash) }

    $query = $v.IndexOf('?')
    if ($query -ge 0) { $v = $v.Substring(0, $query) }

    $v = $v.TrimEnd('.', ',', ';', ':')
    $v = $v -replace '^\./', ''

    $v.Trim()
}

function Test-IsPlaceholderReference {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Value
    )

    if ($Value -match '[\*\?<>\{\}\[\]\|\$%\s"`]') { return $true }
    if ($Value.Contains("'")) { return $true }
    if ($Value.Contains('...')) { return $true }
    if ($Value -match '(?i)(xxx|yyy|zzz|nnn|placeholder)') { return $true }
    if ($Value -match '(?i)(^|/)path/to(/|$)') { return $true }
    if ($Value -match '(?i)(^|/)PineGuard\.X(\.|/|$)') { return $true }
    if ($Value -match '(?i)\.(md|ps1|mdc)/') { return $true }

    $false
}

function Test-IsCheckableReference {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][AllowEmptyString()][string]$Value,
        [switch]$AllowBareFileName
    )

    if ([string]::IsNullOrWhiteSpace($Value)) { return $false }
    if ($Value -match '(?i)^[a-z][a-z0-9+.-]*:') { return $false }
    if ($Value.StartsWith('#')) { return $false }
    if ($Value.StartsWith('/')) { return $false }
    if ($Value.StartsWith('~')) { return $false }
    if (Test-IsPlaceholderReference -Value $Value) { return $false }

    if (-not $Value.Contains('/') -and -not $AllowBareFileName.IsPresent) { return $false }

    if ($Value -match '(?i)\.(md|ps1|mdc)$') { return $true }
    if ($AllowBareFileName.IsPresent) { return $true }
    if ($Value -match '(?i)^(docs/|tools/|src/|tests/|\.claude/|\.agent/|\.pi/|\.github/|\.vscode/|\.clinerules/|\.cursor/|\.windsurf/|\.amazonq/|\.junie/)') { return $true }

    $false
}

function Test-ReferenceResolves {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$SourceFile,
        [Parameter(Mandatory = $true)][string]$Reference
    )

    $candidates = New-Object System.Collections.Generic.List[string]
    $candidates.Add((Join-Path $RepoRoot $Reference)) | Out-Null
    $candidates.Add((Join-Path (Join-Path $RepoRoot 'docs/ai') $Reference)) | Out-Null

    $sourceDir = Split-Path -Parent $SourceFile
    if (-not [string]::IsNullOrWhiteSpace($sourceDir)) {
        $candidates.Add((Join-Path $sourceDir $Reference)) | Out-Null
    }

    foreach ($candidate in $candidates) {
        if (Test-Path -LiteralPath $candidate) { return $true }
    }

    $false
}

function Get-MarkdownBodyReference {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Text
    )

    $refs = New-Object System.Collections.Generic.List[string]

    foreach ($m in [regex]::Matches($Text, '\[[^\]]*\]\(\s*([^)\s]+)')) {
        $refs.Add($m.Groups[1].Value) | Out-Null
    }

    foreach ($m in [regex]::Matches($Text, '`([^`\r\n]+)`')) {
        $refs.Add($m.Groups[1].Value) | Out-Null
    }

    foreach ($m in [regex]::Matches($Text, '(?<![\w`\(\[/.-])((?:\.?[\w][\w.-]*/)+[\w][\w.-]*\.(?:md|ps1))')) {
        $refs.Add($m.Groups[1].Value) | Out-Null
    }

    @($refs)
}

function Get-FrontMatterReference {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Text
    )

    $refs = New-Object System.Collections.Generic.List[string]

    $m = [regex]::Match($Text, '(?s)^﻿?---\r?\n(.*?)\r?\n---')
    if (-not $m.Success) { return @($refs) }

    $inPathList = $false

    foreach ($line in ($m.Groups[1].Value -split '\r?\n')) {
        if ($line -match '^\s*-\s+(.+)$') {
            if ($inPathList) { $refs.Add($Matches[1]) | Out-Null }
            continue
        }

        if ($line -match '^\s*([A-Za-z_][\w-]*)\s*:\s*(.*)$') {
            $key = $Matches[1]
            $value = $Matches[2].Trim()
            $inPathList = $false

            if ($key -notmatch '(?i)^(template|parent|dependencies)$') { continue }

            if ([string]::IsNullOrWhiteSpace($value)) {
                $inPathList = $true
                continue
            }

            if ($value.StartsWith('[')) {
                foreach ($part in ($value.Trim('[', ']') -split ',')) { $refs.Add($part) | Out-Null }
                continue
            }

            $refs.Add($value) | Out-Null
        }
    }

    @($refs)
}

function Get-TasksJsonReference {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Text
    )

    $refs = New-Object System.Collections.Generic.List[string]

    foreach ($m in [regex]::Matches($Text, '"([^"]+\.(?:ps1|md|json|slnx|csproj|yml))"')) {
        $refs.Add($m.Groups[1].Value) | Out-Null
    }

    @($refs)
}

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param(
        [Parameter(Mandatory = $true)][string]$RelativePath,
        [Parameter(Mandatory = $true)][string]$Message
    )

    $findings.Add("[$RelativePath] $Message") | Out-Null
}

$markdownFiles = @(Get-TrackedMarkdownFile -RepoRoot $repoRootResolved)
$scanned = 0
$checked = 0

foreach ($file in $markdownFiles) {
    $rel = Convert-ToRepoRelativePath -RepoRoot $repoRootResolved -Path $file

    # Archived plans are historical records; their references describe the repo as it was.
    if ($rel -match '(?i)^docs/ai/plans/completed/') { continue }

    $text = Get-Content -LiteralPath $file -Raw -ErrorAction SilentlyContinue
    if ([string]::IsNullOrWhiteSpace($text)) { continue }

    $scanned++

    $seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

    foreach ($raw in (Get-FrontMatterReference -Text $text)) {
        $reference = ConvertTo-NormalizedReference -Value $raw
        if (-not (Test-IsCheckableReference -Value $reference -AllowBareFileName)) { continue }
        if (-not $seen.Add("fm:$reference")) { continue }

        $checked++
        if (-not (Test-ReferenceResolves -RepoRoot $repoRootResolved -SourceFile $file -Reference $reference)) {
            Add-Finding -RelativePath $rel -Message ("Unresolved front-matter path: {0}" -f $reference)
        }
    }

    foreach ($raw in (Get-MarkdownBodyReference -Text $text)) {
        $reference = ConvertTo-NormalizedReference -Value $raw
        if (-not (Test-IsCheckableReference -Value $reference)) { continue }
        if (-not $seen.Add($reference)) { continue }

        $checked++
        if (-not (Test-ReferenceResolves -RepoRoot $repoRootResolved -SourceFile $file -Reference $reference)) {
            Add-Finding -RelativePath $rel -Message ("Unresolved reference: {0}" -f $reference)
        }
    }
}

$tasksPath = Join-Path $repoRootResolved '.vscode/tasks.json'

if (Test-Path -LiteralPath $tasksPath) {
    $scanned++
    $tasksText = Get-Content -LiteralPath $tasksPath -Raw
    $tasksSeen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)

    foreach ($raw in (Get-TasksJsonReference -Text $tasksText)) {
        $reference = ConvertTo-NormalizedReference -Value $raw
        if (-not (Test-IsCheckableReference -Value $reference)) { continue }
        if (-not $tasksSeen.Add($reference)) { continue }

        $checked++
        if (-not (Test-ReferenceResolves -RepoRoot $repoRootResolved -SourceFile $tasksPath -Reference $reference)) {
            Add-Finding -RelativePath '.vscode/tasks.json' -Message ("Unresolved script path: {0}" -f $reference)
        }
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$summary = "Scanned {0} file(s); resolved {1} reference(s)." -f $scanned, $checked

if ($findings.Count -eq 0) {
    @("Doc links: PASS", $summary) | Out-File -LiteralPath $reportPath -Encoding utf8
    Write-Host 'Doc links: PASS' -ForegroundColor Green
    Write-Host $summary -ForegroundColor DarkGray
    exit 0
}

@("Doc links: FAIL ($($findings.Count) finding(s))", $summary, '') + $findings | Out-File -LiteralPath $reportPath -Encoding utf8

Write-Host ("Doc links: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red
Write-Host $summary -ForegroundColor DarkGray

foreach ($f in ($findings | Select-Object -First 50)) {
    Write-Host ("- {0}" -f $f) -ForegroundColor Red
}

if ($findings.Count -gt 50) {
    Write-Host ("... and {0} more (see {1})" -f ($findings.Count - 50), $outputPath) -ForegroundColor DarkGray
}

if ($FailOnFindings.IsPresent) {
    exit 1
}

exit 0
