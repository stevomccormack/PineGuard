<#
.SYNOPSIS
    Reconcile this repository's GitHub rulesets against a declared catalog.

.DESCRIPTION
    $RulesetCatalog at the top of this script is the desired state: each entry is a complete
    ruleset spec - target, conditions, rules and bypass actors - keyed by a short name the
    -Name parameter accepts.

    Apply reconciles that catalog against what is live on the repository. Missing rulesets are
    POSTed, drifted ones are PUT with the catalog payload, and matching ones are left alone.

    Enable and Disable are the escape hatch for the case where a maintainer has a backlog of
    local commits to push to a PR-gated main. They flip enforcement without deleting any
    configuration, so re-enabling restores the original spec untouched.

    Requires the gh CLI, authenticated with a token carrying
    Repository permissions -> Administration: Read and write.

.NOTES
    tools/github/Set-GithubRuleset.ps1 is the portable equivalent of Enable/Disable, and is what
    tools/github/Run-Release.ps1 calls for its -BypassPR phase. It toggles by DELETE + POST
    because GitHub's fine-grained PAT model returns 404 on PATCH /rulesets/{id}; this script
    round-trips the whole catalog spec through PUT instead, which a classic token accepts. Apply
    and List have no counterpart there - the catalog lives here.

    Two rough edges from the previous version are gone: Resolve-CatalogKeys called `exit 1` from
    inside a function, which made it unusable from anything that wanted to handle a bad key; and
    the Disable/Enable branches resolved the key list twice, once to print and once to act.

.PARAMETER Action
    Apply   - create or update every targeted catalog ruleset (default).
    Disable - set enforcement=disabled on the targeted rulesets.
    Enable  - set enforcement=active on the targeted rulesets.
    List    - print every ruleset currently live on the repository.

.PARAMETER Name
    Catalog key to target: 'main-branch' or 'v-tags'. Omit to target every catalog entry.
    Ignored by List.

.PARAMETER WhatIf
    Print what would change without writing anything. -DryRun is a supported alias of the same
    switch.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Sync-GithubRuleset.ps1 List

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Sync-GithubRuleset.ps1 Apply -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Sync-GithubRuleset.ps1 Disable main-branch
    Temporarily lifts PR enforcement on main so a local backlog can be pushed.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Sync-GithubRuleset.ps1 Enable main-branch
    Restores PR enforcement once the backlog has landed.
#>

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet('Apply', 'Disable', 'Enable', 'List')]
    [string] $Action = 'Apply',

    [Parameter(Position = 1)]
    [string] $Name,

    [Alias('DryRun')]
    [switch] $WhatIf
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Catalog — the desired state. Edit here, then run Apply to reconcile.
# -------------------------------------------------------------------------------------------------

$RulesetCatalog = [ordered]@{
    'main-branch' = [ordered]@{
        name          = 'main: PR required, no force push, no delete'
        target        = 'branch'
        enforcement   = 'active'
        conditions    = [ordered]@{
            ref_name = [ordered]@{
                include = @('refs/heads/main')
                exclude = @()
            }
        }
        rules         = @(
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
        # Zero bypass actors — even the maintainer goes through a PR.
        bypass_actors = @()
    }
    'v-tags'      = [ordered]@{
        name          = 'v* tags: maintainers only'
        target        = 'tag'
        enforcement   = 'active'
        conditions    = [ordered]@{
            ref_name = [ordered]@{
                include = @('refs/tags/v*')
                exclude = @()
            }
        }
        rules         = @(
            [ordered]@{ type = 'creation' },
            [ordered]@{ type = 'update' },
            [ordered]@{ type = 'deletion' }
        )
        # The repository admin role bypasses, so `gh release create v*` still works for the
        # maintainer while contributors stay blocked.
        bypass_actors = @(
            [ordered]@{
                actor_id    = 5
                actor_type  = 'RepositoryRole'
                bypass_mode = 'always'
            }
        )
    }
}

$repository = "$($Project.Owner)/$($Project.Repository)"

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: GitHub Repository Rulesets"
Write-Var -Name 'Repository' -Value $repository -NoIcon
Write-Var -Name 'Action' -Value $Action -NoIcon
Write-Var -Name 'Target' -Value $(if ($Name) { $Name } else { 'all catalog entries' }) -NoIcon
Write-Var -Name 'WhatIf' -Value $WhatIf.IsPresent -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------

if (-not (Test-Command -Name 'gh')) {
    Write-FailMessage -Title 'GitHub CLI' -Message "'gh' was not found on PATH. Install via: winget install GitHub.cli"
    exit 1
}

$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'gh CLI' -Message "Not authenticated. Run 'gh auth login' first.`n$authStatus"
    exit 1
}

# -------------------------------------------------------------------------------------------------
# Helpers
# -------------------------------------------------------------------------------------------------

function Get-LiveRuleset {
    <#
    .SYNOPSIS
        Returns every ruleset currently configured on the repository.
    #>
    [CmdletBinding()]
    param()

    $json = gh api "repos/$repository/rulesets" 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw "Failed to list rulesets on ${repository}: $json"
    }

    return ($json | ConvertFrom-Json)
}

function Resolve-CatalogKey {
    <#
    .SYNOPSIS
        Validates a catalog key and returns it, or returns every key when none is given.

    .DESCRIPTION
        Throws on an unknown key rather than exiting, so callers decide how to report it.
    #>
    [CmdletBinding()]
    param(
        [string] $Key
    )

    if ([string]::IsNullOrWhiteSpace($Key)) {
        return @($RulesetCatalog.Keys)
    }

    if (-not $RulesetCatalog.Contains($Key)) {
        throw ("'{0}' is not a catalog key. Valid keys: {1}" -f $Key, (($RulesetCatalog.Keys) -join ', '))
    }

    return @($Key)
}

function ConvertTo-RulesetBody {
    <#
    .SYNOPSIS
        Builds the create/update payload for a catalog entry, at a given enforcement level.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Key,
        [string] $Enforcement
    )

    $spec = $RulesetCatalog[$Key]

    return @{
        name          = $spec.name
        target        = $spec.target
        enforcement   = if ([string]::IsNullOrWhiteSpace($Enforcement)) { $spec.enforcement } else { $Enforcement }
        conditions    = $spec.conditions
        rules         = $spec.rules
        bypass_actors = $spec.bypass_actors
    }
}

function Invoke-RulesetWrite {
    <#
    .SYNOPSIS
        POSTs or PUTs a ruleset payload and returns the parsed response.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [ValidateSet('POST', 'PUT')] [string] $Method,
        [Parameter(Mandatory)] [string] $Endpoint,
        [Parameter(Mandatory)] [hashtable] $Body
    )

    $payload = $Body | ConvertTo-Json -Depth 10 -Compress
    $result = $payload | gh api -X $Method $Endpoint --input - 2>&1

    if ($LASTEXITCODE -ne 0) {
        throw "gh api $Method $Endpoint on ${repository} failed: $result"
    }

    return ($result | ConvertFrom-Json)
}

function Set-CatalogRuleset {
    <#
    .SYNOPSIS
        Creates or updates one catalog ruleset. Returns $true on success.

    .PARAMETER Enforcement
        Override the catalog's own enforcement value. Used by the Enable/Disable actions.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Key,
        [string] $Enforcement,
        [switch] $RequireExisting
    )

    $spec = $RulesetCatalog[$Key]
    $existing = Get-LiveRuleset | Where-Object { $_.name -eq $spec.name } | Select-Object -First 1

    if ($RequireExisting -and $null -eq $existing) {
        Write-FailMessage -Title $Key -Message "Ruleset '$($spec.name)' does not exist on $repository. Run Apply first."
        return $false
    }

    $body = ConvertTo-RulesetBody -Key $Key -Enforcement $Enforcement

    if ($WhatIf) {
        $verb = if ($null -eq $existing) { 'create' } else { "update #$($existing.id)" }
        Write-Status "WhatIf: would $verb '$($spec.name)' with enforcement '$($body.enforcement)'."
        return $true
    }

    if ($null -eq $existing) {
        $created = Invoke-RulesetWrite -Method POST -Endpoint "repos/$repository/rulesets" -Body $body
        Write-OkMessage -Title $Key -Message "Created ruleset #$($created.id) '$($spec.name)' (enforcement: $($body.enforcement))."
    }
    else {
        $updated = Invoke-RulesetWrite -Method PUT -Endpoint "repos/$repository/rulesets/$($existing.id)" -Body $body
        Write-OkMessage -Title $Key -Message "Updated ruleset #$($updated.id) '$($spec.name)' (enforcement: $($body.enforcement))."
    }

    return $true
}

# -------------------------------------------------------------------------------------------------
# Dispatch
# -------------------------------------------------------------------------------------------------

if ($Action -eq 'List') {
    Write-Status "Rulesets live on ${repository}:"
    Write-NewLine

    $live = @(Get-LiveRuleset)
    if ($live.Count -eq 0) {
        Write-Status 'None configured.'
    }
    else {
        foreach ($ruleset in $live) {
            Write-OkMessage `
                -Title $ruleset.target `
                -Message "#$($ruleset.id) '$($ruleset.name)' [enforcement=$($ruleset.enforcement)]"
        }
    }

    Write-NewLine
    Write-OkMessage -Title 'GitHub Repository Rulesets' -Message "Listed $($live.Count) ruleset(s)."
    exit 0
}

try {
    $keys = Resolve-CatalogKey -Key $Name
}
catch {
    Write-FailMessage -Title 'Name' -Message $_.Exception.Message
    exit 1
}

$enforcement = switch ($Action) {
    'Enable' { 'active' }
    'Disable' { 'disabled' }
    default { $null }
}

Write-Status "$Action : $($keys -join ', ')"
Write-NewLine

$failed = @()
foreach ($key in $keys) {
    try {
        $applied = Set-CatalogRuleset -Key $key -Enforcement $enforcement -RequireExisting:($Action -ne 'Apply')
        if (-not $applied) { $failed += $key }
    }
    catch {
        Write-FailMessage -Title $key -Message $_.Exception.Message
        $failed += $key
    }
}

Write-NewLine

if ($failed.Count -gt 0) {
    Write-FailMessage -Title 'GitHub Repository Rulesets' -Message ("Failed: {0}" -f ($failed -join ', '))
    exit 1
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title 'GitHub Repository Rulesets' `
    -Message "$Action complete on $repository. Review at $($Project.WebUrl)/settings/rules"
