<#
.SYNOPSIS
    Cut the GitHub Release that triggers publish.yml.

.DESCRIPTION
    publish.yml fires on the `release: published` event, so creating the release IS the entry
    point to the whole publish flow: the workflow builds, tests, packs, and pushes to nuget.org
    over OIDC Trusted Publishing. This script creates the release and the tag behind it, then
    optionally tails the run it triggered.

    Prerelease detection: a version containing -alpha, -beta or -rc is created with --prerelease
    and --latest=false, so an alpha never displaces the stable "Latest" pin on the releases page.

    For the fuller workflow - pushing a local backlog through the protected main branch first,
    and unlisting older prereleases afterwards - use tools/github/Run-Release.ps1, which wraps
    this same `gh release create` call with those two phases.

.NOTES
    The package URLs printed at the end come from $Solution.PackableProjects, which is derived
    from the project registry. The previous version listed six package names by hand and had
    fallen eight behind the fourteen the repository actually ships.

    It also assigned its gh arguments to $args, PowerShell's automatic parameter-array variable.
    That works by accident and breaks confusingly the moment the script grows a function.

.PARAMETER Version
    Semver, with or without a leading 'v'. Examples: 0.1.0-alpha.7, v1.0.0.

.PARAMETER Draft
    Create the release as a draft. publish.yml does not fire until the draft is published in the
    GitHub UI, so -Watch is ignored in this mode.

.PARAMETER Force
    Skip the main-branch, clean-tree and tag-availability pre-flight checks.

.PARAMETER Watch
    After the release is cut, tail the triggered publish.yml run until it completes.

.PARAMETER WaitSeconds
    How long to wait for the triggered run to appear before giving up on finding it. The release
    itself is unaffected. Default: 60.

.PARAMETER WhatIf
    Run pre-flight and print the plan, but do not create the release. -DryRun is a supported
    alias of the same switch.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-GithubRelease.ps1 -Version 0.1.0-alpha.7 -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-GithubRelease.ps1 -Version 0.1.0-alpha.7 -Watch

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/New-GithubRelease.ps1 -Version v1.0.0
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateNotNullOrEmpty()]
    [string] $Version,

    [switch] $Draft,

    [switch] $Force,

    [switch] $Watch,

    [ValidateRange(0, 600)]
    [int] $WaitSeconds = 60,

    [Alias('DryRun')]
    [switch] $WhatIf
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------
# Normalise version

$Version = $Version.Trim()
$tag = if ($Version.StartsWith('v')) { $Version } else { "v$Version" }
$semver = $tag.Substring(1)

if ($semver -notmatch '^\d+\.\d+\.\d+(-[0-9A-Za-z\.-]+)?(\+[0-9A-Za-z\.-]+)?$') {
    Write-FailMessage -Title 'Version' -Message "'$semver' is not a valid semver."
    exit 1
}

$isPrerelease = $semver -match '-(alpha|beta|rc)'

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: GitHub Release"
Write-Var -Name 'Repository' -Value "$($Project.Owner)/$($Project.Repository)" -NoIcon
Write-Var -Name 'Tag' -Value $tag -NoIcon
Write-Var -Name 'Prerelease' -Value $isPrerelease -NoIcon
Write-Var -Name 'Draft' -Value $Draft.IsPresent -NoIcon
Write-Var -Name 'Watch' -Value $Watch.IsPresent -NoIcon
Write-Var -Name 'WhatIf' -Value $WhatIf.IsPresent -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------
# Pre-flight

if (-not (Test-Command -Name 'gh')) {
    Write-FailMessage -Title 'GitHub CLI' -Message "'gh' was not found on PATH. Install via: winget install GitHub.cli"
    exit 1
}

$authStatus = gh auth status 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'gh CLI' -Message "Not authenticated. Run 'gh auth login' first.`n$authStatus"
    exit 1
}
Write-OkMessage -Title 'gh CLI' -Message 'Authenticated.'

if (-not $Force) {
    $branch = (git -C $Solution.RepoRoot rev-parse --abbrev-ref HEAD).Trim()
    if ($branch -ne $Project.MainBranch) {
        Write-FailMessage -Title 'Pre-flight' -Message "On branch '$branch', expected '$($Project.MainBranch)'. Use -Force to override."
        exit 1
    }

    if (git -C $Solution.RepoRoot status --porcelain) {
        Write-FailMessage -Title 'Pre-flight' -Message 'Working tree is dirty. Commit or stash first, or use -Force.'
        exit 1
    }

    git -C $Solution.RepoRoot fetch --tags --quiet

    if (git -C $Solution.RepoRoot tag --list $tag) {
        Write-FailMessage -Title 'Pre-flight' -Message "Tag $tag already exists locally."
        exit 1
    }

    $null = gh api "repos/:owner/:repo/git/refs/tags/$tag" --silent 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-FailMessage -Title 'Pre-flight' -Message "Tag $tag already exists on the remote."
        exit 1
    }

    Write-OkMessage -Title 'Pre-flight' -Message "Clean tree on $branch; tag $tag is available."
}
else {
    Write-Status 'Pre-flight checks skipped (-Force).'
}

Write-NewLine

# -------------------------------------------------------------------------------------------------
# Create release

$releaseArguments = @(
    'release', 'create', $tag,
    '--target', $Project.MainBranch,
    '--title', $tag,
    '--generate-notes'
)
if ($isPrerelease) { $releaseArguments += @('--prerelease', '--latest=false') }
if ($Draft) { $releaseArguments += '--draft' }

if ($WhatIf) {
    Write-Status "WhatIf: would run 'gh $($releaseArguments -join ' ')'."
    exit 0
}

Write-Status "Creating release $tag..."

& gh @releaseArguments
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'gh release create' -Message "Failed (exit code $LASTEXITCODE)."
    exit 1
}

Write-OkMessage -Title 'Release' -Message "$tag created."
Write-NewLine

if ($Draft) {
    Write-Status "Draft created. Publish it at $($Project.WebUrl)/releases to trigger publish.yml."
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Locate the triggered workflow run

Write-Status 'Waiting for the publish.yml run to appear...'

$pollIntervalSeconds = 3
$attempts = [Math]::Max(1, [int]($WaitSeconds / $pollIntervalSeconds))
$runId = $null

for ($attempt = 0; $attempt -lt $attempts; $attempt++) {
    Start-Sleep -Seconds $pollIntervalSeconds

    $runId = gh run list --workflow publish.yml --limit 1 --json 'databaseId,event' `
        --jq '[.[] | select(.event=="release")][0].databaseId' 2>$null

    if ($runId) { break }
}

if (-not $runId) {
    Write-Status "No triggered run found within ${WaitSeconds}s. Check $($Project.WebUrl)/actions"
    exit 0
}

Write-OkMessage -Title 'Workflow' -Message "Run #$runId queued."
Write-Var -Name 'Run' -Value "$($Project.WebUrl)/actions/runs/$runId" -NoIcon
Write-NewLine

if ($Watch) {
    Write-Status 'Tailing the run (Ctrl-C to detach)...'
    gh run watch $runId --exit-status
    if ($LASTEXITCODE -ne 0) {
        Write-FailMessage -Title 'publish.yml' -Message 'Workflow failed. See the run log above.'
        exit 1
    }
    Write-OkMessage -Title 'publish.yml' -Message 'Workflow succeeded.'
}

# -------------------------------------------------------------------------------------------------

Write-NewLine
Write-Status "Packages appear on nuget.org within a few minutes of the workflow completing:"
foreach ($package in $Solution.PackableProjects) {
    Write-Var -Name $package -Value "https://www.nuget.org/packages/$package/$semver" -NoIcon
}

Write-NewLine
Write-OkMessage -Title "$($Project.Name) Project: GitHub Release" -Message "$tag published."
