<#
.SYNOPSIS
    github rulesets

.DESCRIPTION
    Manages the repository's GitHub rulesets (branch + tag protection) declaratively.

    The catalog of default rulesets lives in $DefaultRulesets at the top of this
    script — each entry captures the full ruleset spec (target, conditions, rules,
    bypass actors). Apply reconciles the catalog against what's live in the repo:
    missing rulesets are POSTed, drifted rulesets are PUT with the catalog payload,
    and rulesets already matching stay untouched.

    Disable and Enable are escape hatches for the rare case where a maintainer
    needs to push a backlog of local commits to a PR-gated main. They flip
    enforcement on the named ruleset (or every catalog ruleset when -Name is
    omitted) without deleting any configuration, so re-enabling restores the
    original spec unchanged.

    Requires the gh CLI to be installed and authenticated with a token that has
    Repository permissions -> Administration: Read and write on this repo.

.PARAMETER Action
    Apply   — idempotently create or update every catalog ruleset (default).
    Disable — set enforcement=disabled on the named ruleset or the whole catalog.
    Enable  — set enforcement=active on the named ruleset or the whole catalog.
    List    — print every ruleset currently live on the repo.

.PARAMETER Name
    Catalog key to target for Disable / Enable / Apply. Valid keys are the top-level
    keys of $DefaultRulesets ('main-branch', 'v-tags'). Omit to operate on every
    catalog entry.

.EXAMPLE
    ./github-rulesets.ps1
    Applies the catalog — creates branch + tag rulesets if missing, updates them
    in place if drifted.

.EXAMPLE
    ./github-rulesets.ps1 Disable main-branch
    Temporarily disables PR enforcement on main so a backlog of local commits can
    be pushed.

.EXAMPLE
    ./github-rulesets.ps1 Enable main-branch
    Re-enables PR enforcement on main after the backlog has landed.

.EXAMPLE
    ./github-rulesets.ps1 List
    Prints every ruleset currently live on the repo (catalog entries and any
    others).
#>

# .etc/powershell/github-rulesets.ps1

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet('Apply', 'Disable', 'Enable', 'List')]
    [string] $Action = 'Apply',

    [Parameter(Position = 1)]
    [string] $Name
)

# -------------------------------------------------------------------------------------------------

try {
    . ".etc/powershell/.shared/index.ps1" *> $null
}
catch {
    throw
}

# -------------------------------------------------------------------------------------------------

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

# -------------------------------------------------------------------------------------------------

$project = $Project

# Default catalog. Each entry is a complete ruleset spec keyed by a short
# friendly name the CLI consumes via -Name. Edit this block to evolve the
# desired state; Apply will reconcile.
$DefaultRulesets = [ordered]@{
    'main-branch' = [ordered]@{
        name        = 'main: PR required, no force push, no delete'
        target      = 'branch'
        enforcement = 'active'
        conditions  = [ordered]@{
            ref_name = [ordered]@{
                include = @('refs/heads/main')
                exclude = @()
            }
        }
        rules       = @(
            [ordered]@{
                type       = 'pull_request'
                parameters = [ordered]@{
                    required_approving_review_count   = 0
                    dismiss_stale_reviews_on_push     = $false
                    require_code_owner_review         = $false
                    require_last_push_approval        = $false
                    required_review_thread_resolution = $false
                }
            },
            [ordered]@{ type = 'non_fast_forward' },
            [ordered]@{ type = 'deletion' }
        )
        # Zero bypass actors — even the maintainer goes through PR.
        bypass_actors = @()
    }
    'v-tags'      = [ordered]@{
        name        = 'v* tags: maintainers only'
        target      = 'tag'
        enforcement = 'active'
        conditions  = [ordered]@{
            ref_name = [ordered]@{
                include = @('refs/tags/v*')
                exclude = @()
            }
        }
        rules       = @(
            [ordered]@{ type = 'creation' },
            [ordered]@{ type = 'update' },
            [ordered]@{ type = 'deletion' }
        )
        # Repository admin role bypasses — so `gh release create v*` still works
        # for the maintainer while blocking contributors.
        bypass_actors = @(
            [ordered]@{
                actor_id    = 5
                actor_type  = 'RepositoryRole'
                bypass_mode = 'always'
            }
        )
    }
}

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($project.Name) Project: GitHub Repository Rulesets"
Write-Var -Name "Project Name" -Value $project.Name -NoIcon
Write-Var -Name "Repository" -Value "$($project.Owner)/$($project.Repository)" -NoIcon
Write-Var -Name "Action" -Value $Action -NoIcon
if (-not [string]::IsNullOrWhiteSpace($Name)) {
    Write-Var -Name "Name" -Value $Name -NoIcon
}
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name "gh")) {
    Write-FailMessage -Title "GitHub CLI" -Message "'gh' was not found on PATH. Install via: winget install GitHub.cli"
    exit 1
}

$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title "gh CLI" -Message "gh CLI is not authenticated. Run 'gh auth login' first.`n$authStatus"
    exit 1
}

# -------------------------------------------------------------------------------------------------
# Helpers
# -------------------------------------------------------------------------------------------------

function Get-ExistingRulesets {
    $repo = "$($project.Owner)/$($project.Repository)"
    $json = gh api "repos/$repo/rulesets" 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to list rulesets on ${repo}: $json"
    }
    return ($json | ConvertFrom-Json)
}

function Get-RulesetByName {
    param(
        [Parameter(Mandatory)][string] $DisplayName
    )

    $existing = Get-ExistingRulesets
    return ($existing | Where-Object { $_.name -eq $DisplayName } | Select-Object -First 1)
}

function Resolve-CatalogKeys {
    param(
        [string] $Key
    )

    if ([string]::IsNullOrWhiteSpace($Key)) {
        return @($DefaultRulesets.Keys)
    }

    if (-not $DefaultRulesets.Contains($Key)) {
        $valid = ($DefaultRulesets.Keys) -join ', '
        Write-FailMessage -Title "Name" -Message "'$Key' is not a catalog key. Valid keys: $valid"
        exit 1
    }

    return @($Key)
}

function Invoke-RulesetWrite {
    param(
        [Parameter(Mandatory)][ValidateSet('POST', 'PUT')][string] $Method,
        [Parameter(Mandatory)][string] $Endpoint,
        [Parameter(Mandatory)][hashtable] $Body
    )

    $repo = "$($project.Owner)/$($project.Repository)"
    $payload = $Body | ConvertTo-Json -Depth 10 -Compress

    $result = $payload | gh api -X $Method $Endpoint --input - 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "gh api $Method $Endpoint on ${repo} failed: $result"
    }
    return ($result | ConvertFrom-Json)
}

function Set-CatalogRuleset {
    param(
        [Parameter(Mandatory)][string] $Key
    )

    $spec = $DefaultRulesets[$Key]
    $repo = "$($project.Owner)/$($project.Repository)"
    $existing = Get-RulesetByName -DisplayName $spec.name

    $body = @{
        name          = $spec.name
        target        = $spec.target
        enforcement   = $spec.enforcement
        conditions    = $spec.conditions
        rules         = $spec.rules
        bypass_actors = $spec.bypass_actors
    }

    if ($null -eq $existing) {
        $created = Invoke-RulesetWrite -Method POST -Endpoint "repos/$repo/rulesets" -Body $body
        Write-OkMessage -Title $Key -Message "Created ruleset #$($created.id) '$($spec.name)'."
    }
    else {
        $updated = Invoke-RulesetWrite -Method PUT -Endpoint "repos/$repo/rulesets/$($existing.id)" -Body $body
        Write-OkMessage -Title $Key -Message "Updated ruleset #$($updated.id) '$($spec.name)'."
    }
}

function Set-CatalogRulesetEnforcement {
    param(
        [Parameter(Mandatory)][string] $Key,
        [Parameter(Mandatory)][ValidateSet('active', 'disabled', 'evaluate')][string] $Enforcement
    )

    $spec = $DefaultRulesets[$Key]
    $repo = "$($project.Owner)/$($project.Repository)"
    $existing = Get-RulesetByName -DisplayName $spec.name

    if ($null -eq $existing) {
        Write-FailMessage -Title $Key -Message "Ruleset '$($spec.name)' does not exist on $repo. Run Apply first."
        return $false
    }

    $body = @{
        name          = $spec.name
        target        = $spec.target
        enforcement   = $Enforcement
        conditions    = $spec.conditions
        rules         = $spec.rules
        bypass_actors = $spec.bypass_actors
    }

    $updated = Invoke-RulesetWrite -Method PUT -Endpoint "repos/$repo/rulesets/$($existing.id)" -Body $body
    Write-OkMessage -Title $Key -Message "Ruleset #$($updated.id) enforcement set to '$Enforcement'."
    return $true
}

function Enable-Ruleset {
    <#
    .SYNOPSIS
        Sets a catalog ruleset's enforcement to 'active'.
    .PARAMETER Key
        Catalog key. Omit to enable every catalog entry.
    #>
    param(
        [Parameter(Position = 0)][string] $Key
    )

    $keys = Resolve-CatalogKeys -Key $Key
    $failed = @()
    foreach ($k in $keys) {
        if (-not (Set-CatalogRulesetEnforcement -Key $k -Enforcement 'active')) {
            $failed += $k
        }
    }

    if ($failed.Count -gt 0) {
        throw ("Failed to enable: {0}" -f ($failed -join ', '))
    }
}

function Disable-Ruleset {
    <#
    .SYNOPSIS
        Sets a catalog ruleset's enforcement to 'disabled'. Intended as a
        temporary escape hatch — pair every Disable-Ruleset call with a matching
        Enable-Ruleset once the protected push has landed.
    .PARAMETER Key
        Catalog key. Omit to disable every catalog entry.
    #>
    param(
        [Parameter(Position = 0)][string] $Key
    )

    $keys = Resolve-CatalogKeys -Key $Key
    $failed = @()
    foreach ($k in $keys) {
        if (-not (Set-CatalogRulesetEnforcement -Key $k -Enforcement 'disabled')) {
            $failed += $k
        }
    }

    if ($failed.Count -gt 0) {
        throw ("Failed to disable: {0}" -f ($failed -join ', '))
    }
}

function Write-RulesetList {
    $existing = Get-ExistingRulesets
    if (@($existing).Count -eq 0) {
        Write-Status "No rulesets configured."
        return
    }

    foreach ($rs in $existing) {
        $summary = "#$($rs.id) '$($rs.name)' [target=$($rs.target), enforcement=$($rs.enforcement)]"
        Write-OkMessage -Title $rs.target -Message $summary
    }
}

# -------------------------------------------------------------------------------------------------
# Dispatch
# -------------------------------------------------------------------------------------------------

switch ($Action) {
    'List' {
        Write-Status "Listing repository rulesets..."
        Write-NewLine
        Write-RulesetList
        break
    }
    'Apply' {
        $keys = Resolve-CatalogKeys -Key $Name
        Write-Status "Applying catalog rulesets: $($keys -join ', ')"
        Write-NewLine

        foreach ($key in $keys) {
            Set-CatalogRuleset -Key $key
            Write-NewLine
        }
        break
    }
    'Disable' {
        $keys = Resolve-CatalogKeys -Key $Name
        Write-Status "Disabling rulesets: $($keys -join ', ')"
        Write-NewLine

        try {
            Disable-Ruleset -Key $Name
        }
        catch {
            Write-FailMessage -Title "GitHub Repository Rulesets" -Message $_.Exception.Message
            exit 1
        }
        break
    }
    'Enable' {
        $keys = Resolve-CatalogKeys -Key $Name
        Write-Status "Enabling rulesets: $($keys -join ', ')"
        Write-NewLine

        try {
            Enable-Ruleset -Key $Name
        }
        catch {
            Write-FailMessage -Title "GitHub Repository Rulesets" -Message $_.Exception.Message
            exit 1
        }
        break
    }
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title "GitHub Repository Rulesets" `
    -Message "Action '$Action' complete on $($project.Owner)/$($project.Repository). View at $($project.WebUrl)/settings/rules"
