<#
.SYNOPSIS
    Run Commits

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER Agent
    See the param block for details.

.PARAMETER Core
    See the param block for details.

.PARAMETER MustClauses
    See the param block for details.

.PARAMETER GuardClauses
    See the param block for details.

.PARAMETER FluentValidation
    See the param block for details.

.PARAMETER DataAnnotations
    See the param block for details.

.PARAMETER Options
    See the param block for details.

.PARAMETER DependencyInjection
    See the param block for details.

.PARAMETER AspNetCore
    See the param block for details.

.PARAMETER ErrorOr
    See the param block for details.

.PARAMETER FluentResults
    See the param block for details.

.PARAMETER OneOf
    See the param block for details.

.PARAMETER Testing
    See the param block for details.

.PARAMETER Docs
    See the param block for details.

.PARAMETER Tools
    See the param block for details.

.PARAMETER Solution
    See the param block for details.

.PARAMETER All
    See the param block for details.

.PARAMETER IncludeTests
    See the param block for details.

.PARAMETER AutoMessage
    See the param block for details.

.PARAMETER AutoRebase
    See the param block for details.

.PARAMETER Push
    See the param block for details.

.PARAMETER SafePush
    See the param block for details.

.PARAMETER Remote
    See the param block for details.

.PARAMETER DryRun
    See the param block for details.
#>

[CmdletBinding()]
param(
    [switch]$Agent,
    [switch]$Core,
    [switch]$MustClauses,
    [switch]$GuardClauses,
    [switch]$FluentValidation,
    [switch]$DataAnnotations,
    [switch]$Options,
    [switch]$DependencyInjection,
    [switch]$AspNetCore,
    [switch]$ErrorOr,
    [switch]$FluentResults,
    [switch]$OneOf,
    [switch]$Testing,
    [switch]$Docs,
    [switch]$Tools,
    [switch]$Solution,
    [switch]$All,
    [switch]$IncludeTests,
    [switch]$AutoMessage,
    [switch]$AutoRebase,
    [switch]$Push,
    [switch]$SafePush,
    [string]$Remote = 'origin',
    [switch]$DryRun
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/Import-GitHelpers.ps1"

$repoRoot = Resolve-RepoRoot

if ($SafePush.IsPresent) {
    $AutoRebase = $true
    $Push = $true
}

if ($All.IsPresent) {
    $Agent = $true
    $Core = $true
    $MustClauses = $true
    $GuardClauses = $true
    $FluentValidation = $true
    $DataAnnotations = $true
    $Options = $true
    $DependencyInjection = $true
    $AspNetCore = $true
    $ErrorOr = $true
    $FluentResults = $true
    $OneOf = $true
    $Testing = $true
    $Docs = $true
    $Tools = $true
    $Solution = $true
    $IncludeTests = $true
}

$any = $Agent -or $Core -or $MustClauses -or $GuardClauses -or $FluentValidation -or $DataAnnotations -or $Options -or $DependencyInjection -or $AspNetCore -or $ErrorOr -or $FluentResults -or $OneOf -or $Testing -or $Docs -or $Tools -or $Solution
if (-not $any) {
    throw 'No scopes selected. Use -All or specify one or more scopes (e.g. -Core -Tools).'
}

if ($AutoRebase.IsPresent -and -not $DryRun.IsPresent) {
    Invoke-AutoRebaseIfNeeded -RepoRoot $repoRoot -Remote $Remote
}

function Invoke-ScopedCommit {
    param(
        [Parameter(Mandatory = $true)][string]$ScriptName,
        [bool]$IncludeTests = $false
    )

    $path = Join-Path $PSScriptRoot $ScriptName
    if (-not (Test-Path -LiteralPath $path)) {
        throw "Missing script: $path"
    }

    $childArgs = @()
    if ($IncludeTests) { $childArgs += '-IncludeTests' }
    if ($script:AutoMessage.IsPresent) { $childArgs += '-AutoMessage' }
    if ($script:DryRun.IsPresent) { $childArgs += '-DryRun' }

    if ($script:DryRun.IsPresent) {
        Write-Host ("[DryRun] Invoking: {0} {1}" -f $path, ($childArgs -join ' ')) -ForegroundColor DarkGray
    }

    $pwshArgs = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $path) + $childArgs
    & pwsh @pwshArgs
    if ($LASTEXITCODE -ne 0) {
        Write-Host ("Failed invoking: {0}" -f $path) -ForegroundColor Red
        Write-Host ("Args: {0}" -f ($childArgs -join ' ')) -ForegroundColor Red
        throw "Child script failed with exit code $LASTEXITCODE."
    }
}

if ($Solution) { Invoke-ScopedCommit -ScriptName 'Commit-Solution.ps1' }
if ($Tools) { Invoke-ScopedCommit -ScriptName 'Commit-Tools.ps1' }
if ($Agent) { Invoke-ScopedCommit -ScriptName 'Commit-Agent.ps1' }
if ($Docs) { Invoke-ScopedCommit -ScriptName 'Commit-Docs.ps1' }
if ($Testing) { Invoke-ScopedCommit -ScriptName 'Commit-Testing.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($Core) { Invoke-ScopedCommit -ScriptName 'Commit-Core.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($MustClauses) { Invoke-ScopedCommit -ScriptName 'Commit-MustClauses.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($GuardClauses) { Invoke-ScopedCommit -ScriptName 'Commit-GuardClauses.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($FluentValidation) { Invoke-ScopedCommit -ScriptName 'Commit-FluentValidation.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($DataAnnotations) { Invoke-ScopedCommit -ScriptName 'Commit-DataAnnotations.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($Options) { Invoke-ScopedCommit -ScriptName 'Commit-Options.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($DependencyInjection) { Invoke-ScopedCommit -ScriptName 'Commit-DependencyInjection.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($AspNetCore) { Invoke-ScopedCommit -ScriptName 'Commit-AspNetCore.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($ErrorOr) { Invoke-ScopedCommit -ScriptName 'Commit-ErrorOr.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($FluentResults) { Invoke-ScopedCommit -ScriptName 'Commit-FluentResults.ps1' -IncludeTests:$IncludeTests.IsPresent }
if ($OneOf) { Invoke-ScopedCommit -ScriptName 'Commit-OneOf.ps1' -IncludeTests:$IncludeTests.IsPresent }

if ($AutoRebase.IsPresent -and -not $DryRun.IsPresent) {
    Invoke-AutoRebaseIfNeeded -RepoRoot $repoRoot -Remote $Remote
}

if ($Push.IsPresent -and -not $DryRun.IsPresent) {
    if ($SafePush.IsPresent) {
        Invoke-SafePush -RepoRoot $repoRoot -Remote $Remote
    }
    else {
        Invoke-Push -RepoRoot $repoRoot -Remote $Remote
    }
}
