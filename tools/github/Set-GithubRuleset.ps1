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

.PARAMETER WhatIf
    Look up the ruleset and print what would change, but skip the backup, the DELETE, and the
    POST. -DryRun is a supported alias of the same switch (D-1d in
    docs/ai/plans/tools-review-and-standardisation.md): both spellings resolve to one
    implementation.

.EXAMPLE
    pwsh -File ./tools/github/Set-GithubRuleset.ps1 Disable
    Disables the main-branch ruleset for a direct push cycle.

.EXAMPLE
    pwsh -File ./tools/github/Set-GithubRuleset.ps1 Enable
    Re-enables the main-branch ruleset after the push.

.EXAMPLE
    pwsh -File ./tools/github/Set-GithubRuleset.ps1 Disable -WhatIf
    Previews the toggle without deleting or recreating the ruleset.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet('Enable', 'Disable')]
    [string] $Action,

    [Parameter(Position = 1)]
    [string] $Name = 'main-branch',

    [Alias('DryRun')]
    [switch] $WhatIf
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '../.shared/path.ps1')
. (Join-Path $PSScriptRoot '../.shared/console.ps1')

function Fail($m) { Write-Fail $m; exit 1 }

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
$null = gh auth status 2>&1
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
    Fail "Ruleset '$displayName' not found on this repo. Run .etc/powershell/Sync-GithubRuleset.ps1 Apply to create it."
}

$targetEnforcement = if ($Action -eq 'Enable') { 'active' } else { 'disabled' }

# 2. Short-circuit: if already at target enforcement, nothing to do
$fullJson = gh api "repos/:owner/:repo/rulesets/$($existing.id)" 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api GET rulesets/$($existing.id) failed: $fullJson"
}
$full = $fullJson | ConvertFrom-Json
if ($full.enforcement -eq $targetEnforcement) {
    Write-Success "${Name}: ruleset #$($full.id) already '$targetEnforcement' — no change."
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

if ($WhatIf) {
    Write-Warn "WhatIf: would back up ruleset #$($full.id), DELETE it, then POST it back with enforcement '$targetEnforcement'."
    exit 0
}

# 4. Safety: persist the current config to artifacts/ before deleting.
#    If POST fails, the operator has the JSON to recreate manually.
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$artifactsDir = Join-Path $repoRoot 'artifacts/github-rulesets'
$null = New-Item -ItemType Directory -Path $artifactsDir -Force
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$backupPath = Join-Path $artifactsDir "${Name}-${stamp}.json"
$fullJson | Set-Content -Path $backupPath -Encoding utf8
Write-Detail "Backup saved: $backupPath"

# 5. DELETE the existing ruleset
$deleteResult = gh api -X DELETE "repos/:owner/:repo/rulesets/$($full.id)" 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api DELETE rulesets/$($full.id) failed: $deleteResult"
}
Write-Detail "Deleted ruleset #$($full.id)."

# 6. POST the replacement
$createResult = $payload | gh api -X POST 'repos/:owner/:repo/rulesets' --input - 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh api POST rulesets failed: $createResult. Backup at: $backupPath"
}
$created = $createResult | ConvertFrom-Json
Write-Success "${Name}: ruleset #$($created.id) created with enforcement '$targetEnforcement'."
