<#
.SYNOPSIS
    Shared secret-resolution helper for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Get-ToolSecret into the calling script's scope.

    Self-contained: dot-sources dotenv.ps1 (Import-DotEnv) and path.ps1 (Get-RepoRoot) itself so
    it does not depend on load order (D-2's Pester ordering test requires every .shared/*.ps1 file
    to load standalone).

    Part of the D-4 secrets policy: a single resolver, `$env:` before `.env`, nothing written back
    to the registry or echoed. Get-ToolSecret only covers the last two steps of the resolution
    order (explicit parameter -> $env: -> .env) — the explicit-parameter step is the caller's own
    -Token/-ProjectToken/etc. parameter, checked before calling this function.

    Wiring Get-ToolSecret into each domain's own scripts (SonarQube, Qodana, NuGet, GitHub) is each
    domain's own later task (e.g. T3.05 for SonarQube) — this file only makes the resolver
    available and correct.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'dotenv.ps1')
. (Join-Path $PSScriptRoot 'path.ps1')

function Get-ToolSecret {
    <#
    .SYNOPSIS
        Resolves a named secret from the process environment, then from a .env file.

    .DESCRIPTION
        Resolution order:
          1. $env:<Name> (the current process/session environment — never the User/Machine
             registry; this deliberately does NOT refresh from the registry, unlike the removed
             Sync-Env, so a value set in the current session is never silently overridden, F-25).
          2. The .env file at -EnvFilePath, or `.etc/powershell/.env` under the repo root when
             -EnvFilePath is not given, parsed via Import-DotEnv.
        Returns $null if the secret is found in neither place; it never throws or prompts — a
        caller that wants an interactive fallback (e.g. Read-Host) does that itself.

    .PARAMETER Name
        The environment-variable-style key to resolve (e.g. 'SONARQUBE_TOKEN').

    .PARAMETER EnvFilePath
        Override the .env file path. Defaults to '.etc/powershell/.env' under the resolved repo
        root.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Name,

        [string] $EnvFilePath
    )

    $fromEnv = [Environment]::GetEnvironmentVariable($Name, 'Process')
    if (-not [string]::IsNullOrWhiteSpace($fromEnv)) {
        return $fromEnv
    }

    $envFile = $EnvFilePath
    if ([string]::IsNullOrWhiteSpace($envFile)) {
        $repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
        $envFile = Join-Path $repoRoot '.etc/powershell/.env'
    }

    if (-not (Test-Path -LiteralPath $envFile)) {
        return $null
    }

    $vars = Import-DotEnv -Path $envFile
    if ($vars.ContainsKey($Name)) {
        return $vars[$Name]
    }

    return $null
}
