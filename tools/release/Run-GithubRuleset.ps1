<#
.SYNOPSIS
    Toggle enforcement on a GitHub repository ruleset by short key.

.DESCRIPTION
    Flips the `enforcement` field on a named ruleset between 'active' and
    'disabled' via the GitHub REST API. Configuration is preserved — this is
    an escape hatch, not a destructive operation. Used to temporarily allow
    direct pushes to a PR-gated branch from the maintainer workstation, then
    restore protection.

    Requires gh CLI authenticated with Repository Administration: Read and write.

.PARAMETER Action
    Disable — set enforcement=disabled on the named ruleset.
    Enable  — set enforcement=active.

.PARAMETER Name
    Short key for the ruleset. Defaults to 'main-branch'. Valid keys are
    resolved against the catalog in .etc/powershell/github-rulesets.ps1:
      - 'main-branch' → "main: PR required, no force push, no delete"
      - 'v-tags'      → "v* tags: maintainers only"

.EXAMPLE
    pwsh -File ./tools/release/Run-GithubRuleset.ps1 Disable
    Disables the main-branch ruleset for a direct push cycle.

.EXAMPLE
    pwsh -File ./tools/release/Run-GithubRuleset.ps1 Enable
    Re-enables the main-branch ruleset after the push.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet('Enable', 'Disable')]
    [string] $Action,

    [Parameter(Position = 1)]
    [string] $Name = 'main-branch'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Write-Step($m) { Write-Host "==> $m" -ForegroundColor Cyan }
function Write-Info($m) { Write-Host "    $m" -ForegroundColor Gray }
function Write-Ok($m) { Write-Host "    OK   $m" -ForegroundColor Green }
function Fail($m) { Write-Host "    FAIL $m" -ForegroundColor Red; exit 1 }

$rulesetKeyToName = @{
    'main-branch' = 'main: PR required, no force push, no delete'
    'v-tags'      = 'v* tags: maintainers only'
}

if (-not $rulesetKeyToName.ContainsKey($Name)) {
    $valid = ($rulesetKeyToName.Keys) -join ', '
    Fail "Unknown ruleset short key '$Name'. Valid keys: $valid"
}
$displayName = $rulesetKeyToName[$Name]

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Fail "gh CLI not found on PATH. Install from https://cli.github.com/"
}
$ghStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh CLI is not authenticated. Run 'gh auth login' first."
}

Write-Step "$Action ruleset '$Name' ($displayName)"

$rulesetsJson = gh api 'repos/:owner/:repo/rulesets' 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api repos/:owner/:repo/rulesets failed: $rulesetsJson"
}
$rulesets = $rulesetsJson | ConvertFrom-Json
$ruleset = $rulesets | Where-Object { $_.name -eq $displayName } | Select-Object -First 1
if (-not $ruleset) {
    Fail "Ruleset '$displayName' not found on this repo. Run .etc/powershell/github-rulesets.ps1 Apply to create it."
}

$enforcement = if ($Action -eq 'Enable') { 'active' } else { 'disabled' }
$payload = @{ enforcement = $enforcement } | ConvertTo-Json -Compress
$result = $payload | gh api -X PATCH "repos/:owner/:repo/rulesets/$($ruleset.id)" --input - 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api PATCH rulesets/$($ruleset.id) failed: $result"
}

Write-Ok "${Name}: ruleset #$($ruleset.id) enforcement set to '$enforcement'"
