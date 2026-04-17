<#
.SYNOPSIS
    Shared environment helpers for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Sync-Env, Sync-Path, and Get-JavaVersion
    into the calling script's scope.
    Used by tools that depend on executables or env vars set in other terminal sessions.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Sync-Env {
    <#
    .SYNOPSIS
        Refreshes all session environment variables from the current Machine and User registry values.
    .DESCRIPTION
        Equivalent to Chocolatey's refreshenv. Merges Machine and User environment variables into the
        current process, with User values taking precedence. PATH is concatenated (Machine + User).
    #>
    $targets = @([EnvironmentVariableTarget]::Machine, [EnvironmentVariableTarget]::User)
    $merged  = @{}

    foreach ($target in $targets) {
        $vars = [Environment]::GetEnvironmentVariables($target)
        foreach ($key in $vars.Keys) {
            $merged[$key] = $vars[$key]
        }
    }

    # PATH is special - concatenate Machine + User rather than overwriting.
    $merged['Path'] = [Environment]::GetEnvironmentVariable('Path', 'Machine') + ';' + [Environment]::GetEnvironmentVariable('Path', 'User')

    foreach ($key in $merged.Keys) {
        [Environment]::SetEnvironmentVariable($key, $merged[$key], 'Process')
    }
}

function Sync-Path {
    <#
    .SYNOPSIS
        Refreshes only the session PATH from the current Machine and User environment variables.
    .DESCRIPTION
        Lightweight alternative to Sync-Env when only PATH needs refreshing (e.g. after installing
        a tool via winget). Use Sync-Env when you also need env vars like SONARQUBE_TOKEN.
    #>
    $env:Path = [Environment]::GetEnvironmentVariable('Path', 'Machine') + ';' + [Environment]::GetEnvironmentVariable('Path', 'User')
}

function Get-JavaVersion {
    <#
    .SYNOPSIS
        Returns the first line of java -version output without triggering ErrorActionPreference.
    .DESCRIPTION
        java -version writes to stderr, which PowerShell treats as an error record when
        ErrorActionPreference is Stop. This helper suppresses that by running in a
        Continue scope.
    #>
    $prev = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try {
        $output = & java -version 2>&1
        return ($output | Select-Object -First 1).ToString()
    }
    finally {
        $ErrorActionPreference = $prev
    }
}
