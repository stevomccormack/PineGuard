<#
.SYNOPSIS
    Pack the solution and push the packages to nuget.org from this workstation.

.DESCRIPTION
    The manual fallback for publishing: it packs PineGuard.slnx in Release and pushes every
    resulting .nupkg with the maintainer's own API key.

    Prefer New-GithubRelease.ps1 (or tools/github/Run-Release.ps1). Those cut a GitHub Release,
    which fires publish.yml, which authenticates to nuget.org over OIDC Trusted Publishing and
    mints a short-lived key. This script exists for the case where that path is unavailable -
    a workflow outage, or a package that has to go out without a tag.

.NOTES
    The credential is NUGET_TOKEN, resolved through tools/.shared/secret.ps1's Get-ToolSecret:
    the process environment first, then .etc/powershell/.env. It is never echoed.

    The previous version of this script read $Env:NUGET_API_KEY, which is set NOWHERE on a
    workstation. NUGET_API_KEY exists only inside publish.yml, as the runtime output of
    NuGet/login@v1 - a short-lived OIDC-minted key scoped to that one job step. Reading it here
    silently produced an empty --api-key.

    Three further faults in that version are gone: both of its guards aborted whenever the
    repository WAS initialised (so it could never reach the pack), the tag was hard-coded to
    v1.2.0, and it pushed a glob over a folder it had not cleared first.

.PARAMETER Version
    Package version to stamp, with or without a leading 'v'. Omit to let MinVer derive the version
    from the current git tag, which is what CI does.

.PARAMETER OutputPath
    Directory for the packed .nupkg files. Relative paths resolve against the repository root.
    Default: artifacts/packages/<version>, or artifacts/packages/local when -Version is omitted.

.PARAMETER Source
    NuGet push source. Default: https://api.nuget.org/v3/index.json.

.PARAMETER Token
    Explicit API key. Omit to resolve NUGET_TOKEN from the environment, then from .env.

.PARAMETER PackOnly
    Pack, then stop before pushing. Useful for inspecting the .nupkg contents.

.PARAMETER Force
    Skip the clean-tree and main-branch pre-flight checks, and the confirmation prompt.

.PARAMETER WhatIf
    Print the plan without packing or pushing. -DryRun is a supported alias of the same switch.

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Publish-NugetPackage.ps1 -WhatIf

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Publish-NugetPackage.ps1 -Version 0.1.0-alpha.7 -PackOnly

.EXAMPLE
    pwsh -NoProfile -File ./.etc/powershell/Publish-NugetPackage.ps1 -Version 0.1.0-alpha.7
#>

[CmdletBinding()]
param(
    [string] $Version,

    [string] $OutputPath,

    [ValidateNotNullOrEmpty()]
    [string] $Source = 'https://api.nuget.org/v3/index.json',

    [string] $Token,

    [switch] $PackOnly,

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
# Normalise version

if (-not [string]::IsNullOrWhiteSpace($Version)) {
    $Version = $Version.Trim().TrimStart('v')
    if ($Version -notmatch '^\d+\.\d+\.\d+(-[0-9A-Za-z\.-]+)?(\+[0-9A-Za-z\.-]+)?$') {
        Write-FailMessage -Title 'Version' -Message "'$Version' is not a valid semver."
        exit 1
    }
}

$packageFolder = if ([string]::IsNullOrWhiteSpace($Version)) { 'local' } else { $Version }

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $Solution.RepoRoot "artifacts/packages/$packageFolder"
}
elseif (-not [System.IO.Path]::IsPathRooted($OutputPath)) {
    $OutputPath = Join-Path $Solution.RepoRoot $OutputPath
}

# -------------------------------------------------------------------------------------------------

Write-MastHead "$($Project.Name) Project: Local NuGet Publish"
Write-Var -Name 'Solution' -Value $Solution.Path -NoIcon
Write-Var -Name 'Version' -Value $(if ($Version) { $Version } else { 'MinVer (from git tag)' }) -NoIcon
Write-Var -Name 'Output' -Value $OutputPath -NoIcon
Write-Var -Name 'Source' -Value $Source -NoIcon
Write-Var -Name 'Packable projects' -Value $Solution.PackableProjects.Count -NoIcon
Write-Var -Name 'Mode' -Value $(if ($WhatIf) { 'WhatIf' } elseif ($PackOnly) { 'Pack only' } else { 'Pack and push' }) -NoIcon
Write-NewLine

# -------------------------------------------------------------------------------------------------
# Pre-flight

if (-not (Test-Command -Name 'dotnet')) {
    Write-FailMessage -Title 'dotnet' -Message "'dotnet' was not found on PATH."
    exit 1
}

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

    Write-OkMessage -Title 'Pre-flight' -Message "Clean tree on $branch."
}
else {
    Write-Status 'Pre-flight checks skipped (-Force).'
}

if (-not $PackOnly) {
    if ([string]::IsNullOrWhiteSpace($Token)) {
        $Token = Get-ToolSecret -Name 'NUGET_TOKEN' -EnvFilePath (Join-Path $PSScriptRoot '.env')
    }

    if ([string]::IsNullOrWhiteSpace($Token)) {
        Write-FailMessage `
            -Title 'NUGET_TOKEN' `
            -Message 'Not found in the environment or in .etc/powershell/.env. Create a key at https://nuget.org/account/apikeys with Push scope on the PineGuard.* glob, or pass -Token.'
        exit 1
    }

    Write-OkMessage -Title 'NUGET_TOKEN' -Message 'Resolved.'
}

Write-NewLine

if ($WhatIf) {
    Write-Status "WhatIf: would pack $($Solution.Path) into $OutputPath, then push *.nupkg to $Source."
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Pack

Write-Status 'Packing...'

$null = New-Item -ItemType Directory -Path $OutputPath -Force
Get-ChildItem -Path $OutputPath -Filter '*.nupkg' -ErrorAction SilentlyContinue | Remove-Item -Force

$packArguments = @('pack', $Solution.Path, '-c', 'Release', '--output', $OutputPath)
if (-not [string]::IsNullOrWhiteSpace($Version)) {
    $packArguments += "-p:Version=$Version"
}

dotnet @packArguments
if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'dotnet pack' -Message "Failed (exit code $LASTEXITCODE)."
    exit $LASTEXITCODE
}

$packages = @(Get-ChildItem -Path $OutputPath -Filter '*.nupkg')
if ($packages.Count -eq 0) {
    Write-FailMessage -Title 'dotnet pack' -Message "No .nupkg produced in $OutputPath."
    exit 1
}

Write-NewLine
Write-OkMessage -Title 'Pack' -Message "$($packages.Count) package(s) in $OutputPath"
foreach ($package in $packages) {
    Write-Var -Name $package.BaseName -Value ('{0:N0} bytes' -f $package.Length) -NoIcon
}
Write-NewLine

if ($PackOnly) {
    Write-OkMessage -Title 'Pack only' -Message 'Stopping before push (-PackOnly).'
    exit 0
}

# -------------------------------------------------------------------------------------------------
# Push

if (-not $Force) {
    $confirmation = Read-Host "Push $($packages.Count) package(s) to $Source? [y/N]"
    if ($confirmation -ne 'y' -and $confirmation -ne 'Y') {
        Write-Status 'Aborted.'
        exit 0
    }
}

Write-Status 'Pushing...'

dotnet nuget push (Join-Path $OutputPath '*.nupkg') `
    --api-key $Token `
    --source $Source `
    --skip-duplicate

if ($LASTEXITCODE -ne 0) {
    Write-FailMessage -Title 'dotnet nuget push' -Message "Failed (exit code $LASTEXITCODE)."
    exit $LASTEXITCODE
}

# -------------------------------------------------------------------------------------------------

Write-OkMessage `
    -Title "$($Project.Name) Project: Local NuGet Publish" `
    -Message "Pushed $($packages.Count) package(s) to $Source."
