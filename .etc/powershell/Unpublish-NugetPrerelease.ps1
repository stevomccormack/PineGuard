<#
.SYNOPSIS
    Unlist prerelease versions of the PineGuard packages on nuget.org.

.DESCRIPTION
    nuget.org has no true delete. Unlisting hides a version from search and from "latest
    prerelease" resolution while leaving anyone who pinned it working, which is what
    `dotnet nuget delete` actually performs.

    Default behaviour keeps the newest prerelease of each package listed - so "install the latest
    alpha" still resolves - and unlists everything below it. -All unlists every prerelease
    including the newest. Stable versions are never touched.

.NOTES
    tools/nuget/Unpublish-NugetPrerelease.ps1 does the same job and is the portable, CI-safe
    implementation; this one is the maintainer-shell equivalent, printing through the same
    console chrome as the rest of .etc/powershell and resolving its package list from $Solution.
    Prefer the tools/ script unless you are already in a maintainer-shell session.

    Two faults are fixed relative to the previous version. Its package list was six names typed
    by hand and had fallen eight behind the fourteen the repository ships, so more than half the
    catalogue was silently skipped; the list now comes from $Solution.PackableProjects, projected
    from the project registry. And it parsed .env itself with a regex that only matched
    single-quoted values, so a bare or double-quoted NUGET_TOKEN read as absent; resolution now
    goes through Get-ToolSecret, which checks the process environment first and then .env.

.PARAMETER Package
    Limit to specific package ids. Defaults to every packable PineGuard project.

.PARAMETER All
    Unlist every prerelease, including the newest one.

.PARAMETER Token
    Explicit API key. Omit to resolve NUGET_TOKEN from the environment, then from .env. The key
    needs the Unlist Package scope on the PineGuard.* glob.

.PARAMETER Force
    Skip the confirmation prompt.

.PARAMETER WhatIf
    Print the plan without calling nuget.org. -DryRun is a supported alias of the same switch.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Unpublish-NugetPrerelease.ps1 -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Unpublish-NugetPrerelease.ps1 -Package PineGuard.Core -All -Force
#>

[CmdletBinding()]
param(
    [string[]] $Package,

    [switch] $All,

    [string] $Token,

    [switch] $Force,

    [Alias('DryRun')]
    [switch] $WhatIf
)

# -------------------------------------------------------------------------------------------------

. (Join-Path $PSScriptRoot '.shared/index.ps1')
. (Join-Path $PSScriptRoot '../../tools/.shared/secret.ps1')

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# -------------------------------------------------------------------------------------------------

# Computed here rather than as a param default, because a default-value expression runs before
# this script's dot-sourcing above, when $Solution does not yet exist.
if (-not $PSBoundParameters.ContainsKey('Package')) {
    $Package = $Solution.PackableProjects
}

$source = 'https://api.nuget.org/v3/index.json'

Write-MastHead "$($Project.Name) Project: Unlist NuGet Prereleases"
Write-Var -Name 'Packages' -Value $Package.Count -NoIcon
Write-Var -Name 'Keep newest prerelease' -Value (-not $All.IsPresent) -NoIcon
Write-Var -Name 'Mode' -Value $(if ($WhatIf) { 'WhatIf' } else { 'Unlist' }) -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------
# Credentials and prerequisites

if (-not (Test-Command -Name 'dotnet')) {
    Write-FailMessage -Title 'dotnet' -Message "'dotnet' was not found on PATH."
    exit 1
}

if (-not $WhatIf) {
    if ([string]::IsNullOrWhiteSpace($Token)) {
        $Token = Get-ToolSecret -Name 'NUGET_TOKEN' -EnvFilePath (Join-Path $PSScriptRoot '.env')
    }

    if ([string]::IsNullOrWhiteSpace($Token)) {
        Write-FailMessage `
            -Title 'NUGET_TOKEN' `
            -Message 'Not found in the environment or in .etc/powershell/.env. The key needs the Unlist Package scope on the PineGuard.* glob.'
        exit 1
    }

    Write-OkMessage -Title 'NUGET_TOKEN' -Message 'Resolved.'
}

# -------------------------------------------------------------------------------------------------
# Build the plan

Write-Status 'Scanning published versions...'
Write-NewLine

$planned = New-Object System.Collections.Generic.List[object]

foreach ($packageId in $Package) {
    $indexUrl = "https://api.nuget.org/v3-flatcontainer/$($packageId.ToLowerInvariant())/index.json"

    try {
        $response = Invoke-RestMethod -Uri $indexUrl -ErrorAction Stop
    }
    catch {
        Write-Status "$packageId : not published, or versions unavailable ($($_.Exception.Message))"
        continue
    }

    # The flat container returns versions in ascending order, so the last entry is the newest.
    $prereleases = @(@($response.versions) | Where-Object { $_ -match '-' })

    if ($prereleases.Count -eq 0) {
        Write-Status "$packageId : no prereleases"
        continue
    }

    if ($All) {
        $toUnlist = $prereleases
    }
    elseif ($prereleases.Count -le 1) {
        Write-Status "$packageId : only one prerelease ($($prereleases[0])) — keeping it"
        continue
    }
    else {
        $toUnlist = $prereleases[0..($prereleases.Count - 2)]
        Write-Status "$packageId : keeping newest prerelease $($prereleases[-1])"
    }

    foreach ($version in $toUnlist) {
        $planned.Add([pscustomobject]@{ Package = $packageId; Version = $version })
    }
}

Write-NewLine

if ($planned.Count -eq 0) {
    Write-OkMessage -Title 'Unlist' -Message 'Nothing to unlist.'
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Present the plan

Write-Status "Unlist plan ($($planned.Count) operation(s) across $(($planned | Select-Object -ExpandProperty Package -Unique).Count) package(s)):"
foreach ($operation in $planned) {
    Write-Var -Name $operation.Package -Value $operation.Version -NoIcon
}
Write-NewLine

if ($WhatIf) {
    Write-OkMessage -Title 'WhatIf' -Message 'No API calls were made.'
    exit 0
}

if (-not $Force) {
    $confirmation = Read-Host "Unlist $($planned.Count) version(s)? [y/N]"
    if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
        Write-Status 'Aborted.'
        exit 0
    }
}

# -------------------------------------------------------------------------------------------------
# Execute

Write-Status 'Unlisting...'
Write-NewLine

$failed = @()
foreach ($operation in $planned) {
    $output = dotnet nuget delete $operation.Package $operation.Version `
        --api-key $Token `
        --source $source `
        --non-interactive 2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-OkMessage -Title $operation.Package -Message "$($operation.Version) unlisted."
    }
    else {
        Write-FailMessage -Title $operation.Package -Message "$($operation.Version): $($output -join [Environment]::NewLine)"
        $failed += "$($operation.Package) $($operation.Version)"
    }
}

Write-NewLine

if ($failed.Count -gt 0) {
    Write-FailMessage `
        -Title 'Unlist' `
        -Message "$($failed.Count) of $($planned.Count) operation(s) failed: $($failed -join ', ')"
    exit 1
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage -Title 'Unlist' -Message "All $($planned.Count) operation(s) succeeded."
