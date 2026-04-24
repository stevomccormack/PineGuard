<#
.SYNOPSIS
    Cuts a GitHub Release that triggers the publish.yml workflow.

.DESCRIPTION
    publish.yml is triggered by the `release: published` event. This script
    creates the release (and the underlying tag) so the workflow fires,
    builds, packs, and publishes to nuget.org via OIDC (Trusted Publishing).

    Pre-flight:
      - gh CLI installed and authenticated
      - working tree clean, on main (override with -Force)
      - tag does not already exist

    Prerelease detection:
      Versions containing -alpha, -beta, or -rc are flagged with --prerelease
      and --latest=false so they don't displace the stable "Latest" release.

.PARAMETER Version
    Semver, with or without leading "v". Examples: 0.1.0-alpha.4, v1.0.0.

.PARAMETER Draft
    Create as draft. Workflow won't trigger until the draft is published.

.PARAMETER Force
    Skip the main-branch and clean-tree checks.

.PARAMETER Watch
    After creating the release, tail the triggered workflow run in the terminal.

.EXAMPLE
    pwsh -File .etc/powershell/publish-release.ps1 -Version 0.1.0-alpha.4 -Watch

.EXAMPLE
    pwsh -File .etc/powershell/publish-release.ps1 -Version v1.0.0
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [string] $Version,

    [switch] $Draft,
    [switch] $Force,
    [switch] $Watch
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Helpers

function Write-Step($msg)   { Write-Host "==> $msg" -ForegroundColor Cyan }
function Write-Info($msg)   { Write-Host "    $msg" -ForegroundColor Gray }
function Write-Ok($msg)     { Write-Host "    OK   $msg" -ForegroundColor Green }
function Write-Warn($msg)   { Write-Host "    WARN $msg" -ForegroundColor Yellow }
function Fail($msg)         { Write-Host "    FAIL $msg" -ForegroundColor Red; exit 1 }

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
Write-Info "Tag:        $tag"
Write-Info "Prerelease: $isPrerelease"
Write-Info "Draft:      $($Draft.IsPresent)"
Write-Info "Watch run:  $($Watch.IsPresent)"

# -------------------------------------------------------------------------------------------------
# Pre-flight

Write-Step "Pre-flight"

if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Fail "gh CLI not found. Install from https://cli.github.com/"
}

$ghStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Fail "gh not authenticated. Run 'gh auth login' or set GH_TOKEN."
}
Write-Ok "gh authenticated"

if (-not $Force) {
    $branch = (git rev-parse --abbrev-ref HEAD).Trim()
    if ($branch -ne 'main') {
        Fail "Current branch is '$branch', expected 'main'. Use -Force to override."
    }
    Write-Ok "On main"

    $dirty = git status --porcelain
    if ($dirty) {
        Fail "Working tree is dirty. Commit or stash first, or use -Force."
    }
    Write-Ok "Working tree clean"

    git fetch --tags --quiet
    $existing = git tag --list $tag
    if ($existing) {
        Fail "Tag $tag already exists locally."
    }
    $remoteExists = gh api "repos/:owner/:repo/git/refs/tags/$tag" --silent 2>$null
    if ($LASTEXITCODE -eq 0) {
        Fail "Tag $tag already exists on the remote."
    }
    Write-Ok "Tag $tag is available"
} else {
    Write-Warn "Pre-flight checks skipped (-Force)"
}

# -------------------------------------------------------------------------------------------------
# Create release

Write-Step "Creating release $tag"

$args = @(
    'release', 'create', $tag,
    '--target', 'main',
    '--title', $tag,
    '--generate-notes'
)
if ($isPrerelease) { $args += '--prerelease'; $args += '--latest=false' }
if ($Draft)        { $args += '--draft' }

& gh @args
if ($LASTEXITCODE -ne 0) { Fail "gh release create failed." }
Write-Ok "Release created"

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
    $runId = gh run list --workflow publish.yml --limit 1 --json databaseId,headBranch,event,status `
        --jq "[.[] | select(.event==`"release`")][0].databaseId" 2>$null
    if ($runId) { break }
}

if (-not $runId) {
    Write-Warn "Did not find a triggered run within 60s. Check https://github.com/stevomccormack/PineGuard/actions"
    exit 0
}

Write-Ok "Run #$runId queued"
$repo = gh repo view --json nameWithOwner --jq .nameWithOwner
Write-Info "https://github.com/$repo/actions/runs/$runId"

if ($Watch) {
    Write-Step "Tailing run (Ctrl-C to detach)"
    gh run watch $runId --exit-status
    if ($LASTEXITCODE -ne 0) { Fail "Workflow failed. See run log above." }
    Write-Ok "Workflow succeeded"
}

Write-Step "Done"
Write-Info "Packages will appear on nuget.org within a few minutes of workflow completion:"
Write-Info "  https://www.nuget.org/packages/PineGuard.Core/$semver"
Write-Info "  https://www.nuget.org/packages/PineGuard.MustClauses/$semver"
Write-Info "  https://www.nuget.org/packages/PineGuard.GuardClauses/$semver"
Write-Info "  https://www.nuget.org/packages/PineGuard.FluentValidation/$semver"
Write-Info "  https://www.nuget.org/packages/PineGuard.DataAnnotations/$semver"
Write-Info "  https://www.nuget.org/packages/PineGuard.Testing/$semver"
