<#
.SYNOPSIS
    Test Rule12 Adapter Parity

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

$ruleInfo = Get-PineGuardAuditRule-Catalog -RulesRoot $PSScriptRoot | Where-Object { $_.Id -eq 'Rule12' } | Select-Object -First 1
if (-not $ruleInfo) { throw "Catalog is missing Rule12." }

$outputPath = $ruleInfo.OutputPath
Write-PineGuardAuditHeader -AuditRuleId 'Rule12' -Title 'Adapter command parity against docs/ai/agents' -RepoRoot $repoRootResolved -OutputPath $outputPath

$surfacesRelativePath = 'docs/ai/meta/adapter-surfaces.md'

function Get-TableRow {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Text,
        [Parameter(Mandatory = $true)][string]$SectionPattern
    )

    $rows = New-Object System.Collections.Generic.List[object]
    $inSection = $false

    foreach ($line in ($Text -split '\r?\n')) {
        if ($line -match '^##\s') {
            $inSection = ($line -match $SectionPattern)
            continue
        }

        if (-not $inSection) { continue }
        if ($line -notmatch '^\s*\|') { continue }
        if ($line -match '^\s*\|[\s:|-]+\|\s*$') { continue }

        $cells = @(($line.Trim().Trim('|') -split '\|') | ForEach-Object { $_.Trim() })
        if ($cells.Count -lt 2) { continue }

        $rows.Add([pscustomobject]@{ Cells = [string[]]$cells }) | Out-Null
    }

    $rows.ToArray()
}

function Get-BacktickToken {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][AllowEmptyString()][string]$Text
    )

    @(
        foreach ($m in [regex]::Matches($Text, '`([^`]+)`')) { $m.Groups[1].Value.Trim() }
    )
}

$surfacesPath = Join-Path $repoRootResolved $surfacesRelativePath
if (-not (Test-Path -LiteralPath $surfacesPath)) {
    throw "Adapter surface inventory not found: $surfacesRelativePath"
}

$surfacesText = Get-Content -LiteralPath $surfacesPath -Raw

# --- Section 2: full adapters (surface, tool, command directory) ---
$surfaces = New-Object System.Collections.Generic.List[object]

foreach ($row in (Get-TableRow -Text $surfacesText -SectionPattern '^##\s*2\.')) {
    $cells = $row.Cells
    if ($cells[0] -match '(?i)^surface$') { continue }

    $surfaceToken = @(Get-BacktickToken -Text $cells[0]) | Select-Object -First 1
    if (-not $surfaceToken) { continue }

    $commandDir = ''
    if ($cells.Count -ge 3) {
        $commandDir = @(Get-BacktickToken -Text $cells[2]) | Select-Object -First 1
    }
    if (-not $commandDir) { continue }

    $tool = ''
    if ($cells.Count -ge 2) { $tool = $cells[1] }

    $surfaces.Add([pscustomobject]@{
        Surface    = $surfaceToken.TrimEnd('/')
        Tool       = $tool
        CommandDir = ($surfaceToken.TrimEnd('/') + '/' + $commandDir.TrimEnd('/'))
    }) | Out-Null
}

if ($surfaces.Count -eq 0) {
    throw "No full adapter surfaces parsed from $surfacesRelativePath (section 2)."
}

# --- Brain agents + workflows ---
$agentsDir = Join-Path $repoRootResolved 'docs/ai/agents'
if (-not (Test-Path -LiteralPath $agentsDir)) {
    throw "Brain agent directory not found: docs/ai/agents"
}

$agentNames = @(
    Get-ChildItem -LiteralPath $agentsDir -File -Filter '*.md' |
        Where-Object { $_.BaseName -notmatch '(?i)^(README|INDEX|AGENTS)$' } |
        Select-Object -ExpandProperty BaseName |
        Sort-Object
)

$agentNameSet = [System.Collections.Generic.HashSet[string]]::new([string[]]$agentNames, [System.StringComparer]::OrdinalIgnoreCase)

$workflowsDir = Join-Path $repoRootResolved 'docs/ai/workflows'
$workflowNames = @()
if (Test-Path -LiteralPath $workflowsDir) {
    $workflowNames = @(
        Get-ChildItem -LiteralPath $workflowsDir -File -Filter '*.md' | Select-Object -ExpandProperty BaseName
    )
}

$knownNames = [System.Collections.Generic.HashSet[string]]::new([string[]]($agentNames + $workflowNames), [System.StringComparer]::OrdinalIgnoreCase)

# --- Section 4: declared parity exceptions ---
$exemptSurfaces = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
$agentExemptions = @{}

foreach ($row in (Get-TableRow -Text $surfacesText -SectionPattern '^##\s*4\.')) {
    $cells = $row.Cells
    if ($cells[0] -match '(?i)^exception$') { continue }
    if ($cells.Count -lt 2) { continue }

    $surfaceCell = $cells[1]
    $namedSurfaces = New-Object System.Collections.Generic.List[string]

    foreach ($surface in $surfaces) {
        $token = @(Get-BacktickToken -Text $surfaceCell) | Where-Object { $_.TrimEnd('/') -ieq $surface.Surface }
        if ($token) {
            $namedSurfaces.Add($surface.Surface) | Out-Null
            continue
        }

        if (-not [string]::IsNullOrWhiteSpace($surface.Tool) -and $surfaceCell -match ('\b' + [regex]::Escape($surface.Tool) + '\b')) {
            $namedSurfaces.Add($surface.Surface) | Out-Null
        }
    }

    $exemptAgents = @(Get-BacktickToken -Text $cells[0] | Where-Object { $agentNameSet.Contains($_) })

    if ($exemptAgents.Count -eq 0) {
        foreach ($surface in $namedSurfaces) { $exemptSurfaces.Add($surface) | Out-Null }
        continue
    }

    foreach ($agent in $exemptAgents) {
        if (-not $agentExemptions.ContainsKey($agent)) {
            $agentExemptions[$agent] = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
        }
        foreach ($surface in $namedSurfaces) { $agentExemptions[$agent].Add($surface) | Out-Null }
    }
}

# --- Palettes: a surface's boot file must list every agent it adapts ---
$palettes = @(
    [pscustomobject]@{ Surface = '.claude'; Path = 'CLAUDE.md' }
    [pscustomobject]@{ Surface = '.pi'; Path = '.pi/AGENTS.md' }
)

foreach ($agent in ($agentExemptions.Keys | Sort-Object)) {
    Write-Host ("Declared exception: '{0}' expected on {1} only" -f $agent, (($agentExemptions[$agent] | Sort-Object) -join ', ')) -ForegroundColor DarkGray
}

$findings = New-Object System.Collections.Generic.List[string]

function Add-Finding {
    param(
        [Parameter(Mandatory = $true)][string]$Scope,
        [Parameter(Mandatory = $true)][string]$Message
    )

    $findings.Add("[$Scope] $Message") | Out-Null
}

function Test-IsAgentExempt {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$Agent,
        [Parameter(Mandatory = $true)][string]$Surface,
        [Parameter(Mandatory = $true)][hashtable]$Exemptions
    )

    if (-not $Exemptions.ContainsKey($Agent)) { return $false }

    -not $Exemptions[$Agent].Contains($Surface)
}

foreach ($surface in $surfaces) {
    if ($exemptSurfaces.Contains($surface.Surface)) {
        Write-Host ("Skipping {0} (declared subset policy in {1} section 4)" -f $surface.Surface, $surfacesRelativePath) -ForegroundColor DarkGray
        continue
    }

    $commandDirPath = Join-Path $repoRootResolved $surface.CommandDir
    if (-not (Test-Path -LiteralPath $commandDirPath)) {
        Add-Finding -Scope $surface.CommandDir -Message 'Command directory declared in the adapter surface inventory does not exist.'
        continue
    }

    $present = @{}
    foreach ($file in (Get-ChildItem -LiteralPath $commandDirPath -File -Filter '*.md')) {
        $stem = $file.Name
        $dot = $stem.IndexOf('.')
        if ($dot -ge 0) { $stem = $stem.Substring(0, $dot) }
        $present[$stem] = $file.Name
    }

    foreach ($agent in $agentNames) {
        if (Test-IsAgentExempt -Agent $agent -Surface $surface.Surface -Exemptions $agentExemptions) { continue }
        if ($present.ContainsKey($agent)) { continue }

        Add-Finding -Scope $surface.CommandDir -Message ("Missing adapter for agent '{0}'." -f $agent)
    }

    foreach ($stem in $present.Keys) {
        if ($knownNames.Contains($stem)) { continue }

        Add-Finding -Scope $surface.CommandDir -Message ("Orphan adapter '{0}' has no docs/ai/agents or docs/ai/workflows counterpart." -f $present[$stem])
    }
}

foreach ($palette in $palettes) {
    if ($exemptSurfaces.Contains($palette.Surface)) { continue }

    $palettePath = Join-Path $repoRootResolved $palette.Path
    if (-not (Test-Path -LiteralPath $palettePath)) {
        Add-Finding -Scope $palette.Path -Message 'Palette file not found.'
        continue
    }

    $paletteText = Get-Content -LiteralPath $palettePath -Raw

    foreach ($agent in $agentNames) {
        if (Test-IsAgentExempt -Agent $agent -Surface $palette.Surface -Exemptions $agentExemptions) { continue }
        if ($paletteText -match [regex]::Escape("docs/ai/agents/$agent.md")) { continue }

        Add-Finding -Scope $palette.Path -Message ("Palette does not list agent '{0}'." -f $agent)
    }
}

$reportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $outputPath
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPath)

$summary = "Checked {0} agent(s) across {1} full adapter surface(s) and {2} palette(s)." -f $agentNames.Count, $surfaces.Count, $palettes.Count

if ($findings.Count -eq 0) {
    @('Adapter parity: PASS', $summary) | Out-File -LiteralPath $reportPath -Encoding utf8
    Write-Host 'Adapter parity: PASS' -ForegroundColor Green
    Write-Host $summary -ForegroundColor DarkGray
    exit 0
}

@("Adapter parity: FAIL ($($findings.Count) finding(s))", $summary, '') + $findings | Out-File -LiteralPath $reportPath -Encoding utf8

Write-Host ("Adapter parity: FAIL ({0} finding(s))" -f $findings.Count) -ForegroundColor Red
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
