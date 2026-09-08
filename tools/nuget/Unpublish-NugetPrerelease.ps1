<#
.SYNOPSIS
    Unlist prerelease versions of PineGuard packages on nuget.org.

.DESCRIPTION
    nuget.org has no true delete — unlisting hides a version from search
    and 'Latest prerelease' resolution while keeping existing consumers
    who pinned that version working. This script sends unlist requests via
    `dotnet nuget delete`. The API key referenced by NUGET_TOKEN in
    .etc/powershell/.env must have the Unlist Package scope on the
    PineGuard.* glob.

    Default behaviour: keep the latest prerelease on each package listed
    so "install latest alpha" still works, and unlist everything below.
    Pass -All to unlist every prerelease including the latest one. Stable
    versions (no -alpha/-beta/-rc suffix) are never touched.

.PARAMETER Package
    Limit to specific package IDs. Defaults to the registry-derived list of every packable
    PineGuard project (tools/.shared/dotnet-projects.ps1's Get-PineGuardPackableProjects, F-21) —
    currently 14 packages: every scope under src/ except PineGuard.Analyzers.CodeFixes (bundled
    inside the PineGuard.Analyzers package, not shipped on its own), plus PineGuard.Testing.

.PARAMETER All
    Unlist every prerelease, including the latest. Default keeps the
    latest prerelease listed.

.PARAMETER WhatIf
    Print the unlist plan without making any API calls. -DryRun is a supported alias of the same
    switch (D-1d in docs/ai/plans/tools-review-and-standardisation.md): both spellings resolve to
    one implementation.

.PARAMETER Force
    Skip the confirmation prompt.

.PARAMETER EnvFile
    Path to the .env file that supplies NUGET_TOKEN. Defaults to
    .etc/powershell/.env under the repo root.

.EXAMPLE
    pwsh -File ./tools/nuget/Unpublish-NugetPrerelease.ps1 -WhatIf

.EXAMPLE
    pwsh -File ./tools/nuget/Unpublish-NugetPrerelease.ps1 -All -Force
#>

[CmdletBinding()]
param(
    [string[]] $Package,
    [switch] $All,
    [Alias('DryRun')]
    [switch] $WhatIf,
    [switch] $Force,
    [string] $EnvFile
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'dotenv.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'console.ps1')

function Fail($m) { Write-Fail $m; exit 1 }

$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot

if (-not $PSBoundParameters.ContainsKey('Package')) {
    # Registry-derived default (F-21): computed here, not as the param block's default value,
    # because default-value expressions run before this script's own dot-sourcing above, so
    # Get-PineGuardPackableProjects would not exist yet if called there.
    $Package = Get-PineGuardPackableProjects -RepoRoot $repoRoot
}

if ([string]::IsNullOrWhiteSpace($EnvFile)) {
    $EnvFile = Join-Path $repoRoot '.etc/powershell/.env'
}

$vars = Import-DotEnv -Path $EnvFile
$token = $vars['NUGET_TOKEN']
if ([string]::IsNullOrWhiteSpace($token)) {
    Fail "NUGET_TOKEN not found in $EnvFile"
}
Write-Success "NUGET_TOKEN loaded"

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Fail "dotnet CLI not found on PATH."
}

$source = 'https://api.nuget.org/v3/index.json'
$actions = New-Object System.Collections.Generic.List[object]

Write-Step "Scanning versions"

foreach ($pkg in $Package) {
    $lower = $pkg.ToLowerInvariant()
    $url = "https://api.nuget.org/v3-flatcontainer/$lower/index.json"
    try {
        $resp = Invoke-RestMethod -Uri $url -ErrorAction Stop
    }
    catch {
        Write-Warn "$pkg : could not fetch versions ($($_.Exception.Message))"
        continue
    }

    $versions = @($resp.versions)
    $pre = @($versions | Where-Object { $_ -match '-' })
    if ($pre.Count -eq 0) {
        Write-Detail "$pkg : no prereleases"
        continue
    }

    if ($All) {
        $toUnlist = $pre
    }
    elseif ($pre.Count -le 1) {
        Write-Detail "$pkg : only one prerelease ($($pre[0])) — keeping it"
        continue
    }
    else {
        $toUnlist = $pre[0..($pre.Count - 2)]
        Write-Detail "$pkg : keeping latest prerelease $($pre[-1])"
    }

    foreach ($v in $toUnlist) {
        $actions.Add([pscustomobject]@{ Package = $pkg; Version = $v })
    }
}

if ($actions.Count -eq 0) {
    Write-Step "Nothing to unlist"
    exit 0
}

Write-Step "Unlist plan"
foreach ($a in $actions) {
    Write-Detail ("{0,-34} {1}" -f $a.Package, $a.Version)
}
Write-Host ""
$distinctCount = ($actions | Select-Object -ExpandProperty Package -Unique).Count
Write-Detail "Total: $($actions.Count) operations across $distinctCount packages"

if ($WhatIf) {
    Write-Step "WhatIf — no API calls made"
    exit 0
}

if (-not $Force) {
    $confirm = Read-Host "Proceed? [y/N]"
    if ($confirm -ne 'y' -and $confirm -ne 'Y') {
        Write-Warn "Aborted."
        exit 0
    }
}

Write-Step "Unlisting"
$failures = 0
foreach ($a in $actions) {
    $output = dotnet nuget delete $a.Package $a.Version `
        --api-key $token `
        --source $source `
        --non-interactive 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Success ("{0,-34} {1}" -f $a.Package, $a.Version)
    }
    else {
        Write-Host "    FAIL $($a.Package) $($a.Version)" -ForegroundColor Red
        Write-Host ($output -join [Environment]::NewLine) -ForegroundColor DarkGray
        $failures++
    }
}

Write-Host ""
if ($failures -gt 0) {
    Fail "$failures of $($actions.Count) operations failed."
}
Write-Success "All $($actions.Count) operations succeeded."
