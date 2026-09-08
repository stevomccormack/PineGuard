<#
.SYNOPSIS
    Cut a GitHub Release that triggers publish.yml, optionally bypassing
    branch protection to push local commits first and unlisting older
    prereleases after publish succeeds.

.DESCRIPTION
    publish.yml is triggered by the `release: published` event. This script
    orchestrates the full release workflow:

      1. Pre-flight — gh authenticated, working tree clean, on main, tag free
      2. [optional, -BypassPR] — disable main ruleset, git push, re-enable
      3. gh release create — cuts the GitHub Release, triggers publish.yml
      4. [optional, -Watch] — tails the workflow run until complete
      5. [optional, -Unlist] — unlists older prereleases (keeps latest)

    Prerelease detection: versions containing -alpha, -beta, or -rc are
    flagged with --prerelease and --latest=false so they don't displace the
    stable "Latest" release.

    Sub-scripts used (callable standalone):
      tools/github/Set-GithubRuleset.ps1          (BypassPR phase)
      tools/nuget/Unpublish-NugetPrerelease.ps1   (Unlist phase)

.PARAMETER Version
    Semver, with or without leading "v". Examples: 0.1.0-alpha.6, v1.0.0.

.PARAMETER BypassPR
    Before cutting the release, disable the main-branch ruleset, push the
    local main branch, then re-enable the ruleset. Use when the release
    depends on commits that haven't landed on origin/main yet.

.PARAMETER Unlist
    After the publish workflow succeeds, run Unpublish-NugetPrerelease.ps1 to unlist
    older prereleases on nuget.org. The latest prerelease is kept listed.
    Ignored when -Draft is set.

.PARAMETER Draft
    Create as a draft. The workflow does not trigger until the draft is
    published in the GitHub UI. -Watch and -Unlist are ignored in this mode.

.PARAMETER Force
    Skip the main-branch and clean-tree pre-flight checks.

.PARAMETER Watch
    After the release is cut, tail the triggered publish.yml run in the
    terminal until it completes. Required for -Unlist to know when to run.

.PARAMETER WhatIf
    Run through pre-flight and print the plan, but skip the ruleset cycle,
    skip `gh release create`, and skip the unlist. Use this to validate the
    release arguments before committing to a real cut. -DryRun is a supported
    alias of the same switch (D-1d in docs/ai/plans/tools-review-and-standardisation.md):
    both spellings resolve to one implementation.

.EXAMPLE
    pwsh -File ./tools/github/Run-Release.ps1 -Version 0.1.0-alpha.6 -Watch

.EXAMPLE
    pwsh -File ./tools/github/Run-Release.ps1 -Version 0.1.0-alpha.6 -BypassPR -Unlist -Watch -WhatIf
    Prints the full plan without performing any action.

.EXAMPLE
    pwsh -File ./tools/github/Run-Release.ps1 -Version 0.1.0-alpha.6 -BypassPR -Unlist -Watch
    Full workflow: push pending commits through protected main, cut the
    release, watch the publish run, then unlist older alphas.

.EXAMPLE
    pwsh -File ./tools/github/Run-Release.ps1 -Version v1.0.0
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string] $Version,

    [switch] $BypassPR,
    [switch] $Unlist,
    [switch] $Draft,
    [switch] $Force,
    [switch] $Watch,

    [Alias('DryRun')]
    [switch] $WhatIf
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '../.shared/path.ps1')
. (Join-Path $PSScriptRoot '../.shared/dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '../.shared/console.ps1')

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

function Fail($msg) { Write-Fail $msg; exit 1 }

# -------------------------------------------------------------------------------------------------
# Normalise version

$Version = $Version.Trim()
$tag = if ($Version.StartsWith('v')) { $Version } else { "v$Version" }
$semver = $tag.Substring(1)

if ($semver -notmatch '^\d+\.\d+\.\d+(-[0-9A-Za-z\.-]+)?(\+[0-9A-Za-z\.-]+)?$') {
    Fail "Version '$semver' is not a valid semver."
}

$isPrerelease = $semver -match '-(alpha|beta|rc)'

Write-Step "Release plan"
Write-Detail "Tag:        $tag"
Write-Detail "Prerelease: $isPrerelease"
Write-Detail "BypassPR:   $($BypassPR.IsPresent)"
Write-Detail "Draft:      $($Draft.IsPresent)"
Write-Detail "Watch:      $($Watch.IsPresent)"
Write-Detail "Unlist:     $($Unlist.IsPresent)"
Write-Detail "WhatIf:     $($WhatIf.IsPresent)"

# -------------------------------------------------------------------------------------------------
# Pre-flight

Write-Step "Pre-flight"

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Fail "gh CLI not found. Install from https://cli.github.com/"
}

$null = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh not authenticated. Run 'gh auth login' or set GH_TOKEN."
}
Write-Success "gh authenticated"

if (-not $Force) {
    $branch = (git rev-parse --abbrev-ref HEAD).Trim()
    if ($branch -ne 'main') {
        Fail "Current branch is '$branch', expected 'main'. Use -Force to override."
    }
    Write-Success "On main"

    $dirty = git status --porcelain
    if ($dirty) {
        Fail "Working tree is dirty. Commit or stash first, or use -Force."
    }
    Write-Success "Working tree clean"

    git fetch --tags --quiet
    $existing = git tag --list $tag
    if ($existing) {
        Fail "Tag $tag already exists locally."
    }
    $null = gh api "repos/:owner/:repo/git/refs/tags/$tag" --silent 2>$null
    if ($LASTEXITCODE -eq 0) {
        Fail "Tag $tag already exists on the remote."
    }
    Write-Success "Tag $tag is available"
}
else {
    Write-Warn "Pre-flight checks skipped (-Force)"
}

# -------------------------------------------------------------------------------------------------
# Optional BypassPR: ruleset cycle + push

if ($BypassPR) {
    $rulesetScript = Join-Path $PSScriptRoot 'Set-GithubRuleset.ps1'
    if (-not (Test-Path $rulesetScript)) {
        Fail "Set-GithubRuleset.ps1 not found at $rulesetScript"
    }

    $aheadCountRaw = git rev-list --count '@{upstream}..HEAD' 2>$null
    $aheadCount = if ([string]::IsNullOrWhiteSpace($aheadCountRaw)) { 0 } else { [int]$aheadCountRaw.Trim() }

    if ($aheadCount -eq 0) {
        Write-Detail "No local commits ahead of upstream — skipping BypassPR cycle."
    }
    elseif ($WhatIf) {
        Write-Warn "WhatIf: would disable main-branch ruleset, push $aheadCount commit(s), re-enable."
    }
    else {
        Write-Step "BypassPR: disabling main-branch ruleset, pushing $aheadCount commit(s), re-enabling"
        & pwsh -NoProfile -ExecutionPolicy Bypass -File $rulesetScript Disable main-branch
        if ($LASTEXITCODE -ne 0) { Fail "Ruleset disable failed" }

        try {
            & git push origin main
            if ($LASTEXITCODE -ne 0) { throw "git push origin main failed" }
            Write-Success "Pushed $aheadCount commit(s) to origin/main"
        }
        finally {
            & pwsh -NoProfile -ExecutionPolicy Bypass -File $rulesetScript Enable main-branch
            if ($LASTEXITCODE -ne 0) { Write-Warn "Ruleset re-enable failed — check repo settings manually" }
        }
    }
}

# -------------------------------------------------------------------------------------------------
# Create release

$releaseArgs = @(
    'release', 'create', $tag,
    '--target', 'main',
    '--title', $tag,
    '--generate-notes'
)
if ($isPrerelease) { $releaseArgs += '--prerelease'; $releaseArgs += '--latest=false' }
if ($Draft) { $releaseArgs += '--draft' }

if ($WhatIf) {
    Write-Step "WhatIf: would create release $tag"
    Write-Detail ("gh " + ($releaseArgs -join ' '))
    Write-Step "WhatIf complete — no release created, no workflow triggered, no unlist performed."
    exit 0
}

Write-Step "Creating release $tag"
& gh @releaseArgs
if ($LASTEXITCODE -ne 0) { Fail "gh release create failed." }
Write-Success "Release created"

if ($Draft) {
    Write-Warn "Draft created. Publish it in the GitHub UI to trigger publish.yml."
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Workflow run

Write-Step "Waiting for publish.yml run to appear"

$runId = $null
for ($i = 0; $i -lt 20; $i++) {
    Start-Sleep -Seconds 3
    $runId = gh run list --workflow publish.yml --limit 1 --json databaseId, headBranch, event, status `
        --jq "[.[] | select(.event==`"release`")][0].databaseId" 2>$null
    if ($runId) { break }
}

if (-not $runId) {
    Write-Warn "Did not find a triggered run within 60s. Check https://github.com/$(gh repo view --json nameWithOwner --jq .nameWithOwner)/actions"
    exit 0
}

Write-Success "Run #$runId queued"
$repo = gh repo view --json nameWithOwner --jq .nameWithOwner
Write-Detail "https://github.com/$repo/actions/runs/$runId"

$workflowSucceeded = $false
if ($Watch) {
    Write-Step "Tailing run (Ctrl-C to detach)"
    gh run watch $runId --exit-status
    if ($LASTEXITCODE -ne 0) { Fail "Workflow failed. See run log above." }
    Write-Success "Workflow succeeded"
    $workflowSucceeded = $true
}

# -------------------------------------------------------------------------------------------------
# Optional Unlist (only after Watch confirms success)

if ($Unlist) {
    if (-not $Watch) {
        Write-Warn "-Unlist requires -Watch so we know the publish succeeded. Skipping unlist."
    }
    elseif (-not $workflowSucceeded) {
        Write-Warn "Workflow did not succeed; skipping unlist."
    }
    else {
        Write-Step "Waiting 30s for nuget.org flat-container indexing before unlist"
        Start-Sleep -Seconds 30
        $unlistScript = Join-Path $repoRoot 'tools/nuget/Unpublish-NugetPrerelease.ps1'
        if (-not (Test-Path $unlistScript)) {
            Fail "Unpublish-NugetPrerelease.ps1 not found at $unlistScript"
        }
        & pwsh -NoProfile -ExecutionPolicy Bypass -File $unlistScript -Force
        if ($LASTEXITCODE -ne 0) { Write-Warn "Unlist reported failures — inspect output above" }
    }
}

# -------------------------------------------------------------------------------------------------

Write-Step "Done"
Write-Detail "Packages will appear on nuget.org within a few minutes of workflow completion:"
foreach ($pkg in (Get-PineGuardPackableProjects -RepoRoot $repoRoot)) {
    Write-Detail "  https://www.nuget.org/packages/$pkg/$semver"
}
