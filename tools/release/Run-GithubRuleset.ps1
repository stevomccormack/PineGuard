<#
.SYNOPSIS
    Toggle enforcement on a GitHub repository ruleset by short key.

.DESCRIPTION
    Flips the `enforcement` field on a named ruleset between 'active' and
    'disabled' by DELETEing the current ruleset and POSTing an identical one
    with the target enforcement value. Configuration (rules, conditions,
    bypass actors) is preserved — the full ruleset body is round-tripped.

    Why not PATCH: GitHub's fine-grained PAT permission model returns 404
    on PATCH /rulesets/{id} even when the token has 'Administration: Read
    and write'. POST and DELETE work correctly. Toggle via DELETE + POST.

    Requires gh CLI authenticated with Repository Administration: Read and write.

.PARAMETER Action
    Disable — set enforcement=disabled on the named ruleset.
    Enable  — set enforcement=active.

.PARAMETER Name
    Short key for the ruleset. Defaults to 'main-branch'. Valid keys:
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

# 1. Locate the existing ruleset (list, filter by name)
$rulesetsJson = gh api 'repos/:owner/:repo/rulesets' 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api repos/:owner/:repo/rulesets failed: $rulesetsJson"
}
$rulesets = $rulesetsJson | ConvertFrom-Json
$existing = $rulesets | Where-Object { $_.name -eq $displayName } | Select-Object -First 1
if (-not $existing) {
    Fail "Ruleset '$displayName' not found on this repo. Run .etc/powershell/github-rulesets.ps1 Apply to create it."
}

$targetEnforcement = if ($Action -eq 'Enable') { 'active' } else { 'disabled' }

# 2. Short-circuit: if already at target enforcement, nothing to do
$fullJson = gh api "repos/:owner/:repo/rulesets/$($existing.id)" 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api GET rulesets/$($existing.id) failed: $fullJson"
}
$full = $fullJson | ConvertFrom-Json
if ($full.enforcement -eq $targetEnforcement) {
    Write-Ok "${Name}: ruleset #$($full.id) already '$targetEnforcement' — no change."
    exit 0
}

# 3. Build the POST body from the fetched config, overriding enforcement.
#    GitHub's create endpoint accepts the same shape as the fetched one
#    EXCEPT for server-managed fields (id, source_type, source, node_id,
#    created_at, updated_at, _links, current_user_can_bypass).
$body = [ordered]@{
    name          = $full.name
    target        = $full.target
    enforcement   = $targetEnforcement
    conditions    = $full.conditions
    rules         = $full.rules
    bypass_actors = $full.bypass_actors
}
$payload = $body | ConvertTo-Json -Depth 20 -Compress

# 4. Safety: persist the current config to artifacts/ before deleting.
#    If POST fails, the operator has the JSON to recreate manually.
$artifactsDir = Join-Path (Split-Path -Parent $PSScriptRoot) '../artifacts/github-rulesets'
$null = New-Item -ItemType Directory -Path $artifactsDir -Force
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$backupPath = Join-Path $artifactsDir "${Name}-${stamp}.json"
$fullJson | Set-Content -Path $backupPath -Encoding utf8
Write-Info "Backup saved: $backupPath"

# 5. DELETE the existing ruleset
$deleteResult = gh api -X DELETE "repos/:owner/:repo/rulesets/$($full.id)" 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api DELETE rulesets/$($full.id) failed: $deleteResult"
}
Write-Info "Deleted ruleset #$($full.id)."

# 6. POST the replacement
$createResult = $payload | gh api -X POST 'repos/:owner/:repo/rulesets' --input - 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api POST rulesets failed: $createResult. Backup at: $backupPath"
}
$created = $createResult | ConvertFrom-Json
Write-Ok "${Name}: ruleset #$($created.id) created with enforcement '$targetEnforcement'."
