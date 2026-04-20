<#
.SYNOPSIS
    Shared .env parser for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Import-DotEnv, which loads key/value
    pairs from a .env file and optionally exports them to the current
    process environment. Used by release tooling to pick up NUGET_TOKEN,
    GH_TOKEN, and similar machine-local secrets without hard-coding paths.

    Supported line formats (one per line):
      KEY=value
      KEY='value'
      KEY="value"
      KEY = value          (whitespace tolerated around =)
    Lines beginning with # and blank lines are ignored. Values are treated
    as literals — $VAR expansion and shell metacharacters are not honored.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Import-DotEnv {
    <#
    .SYNOPSIS
        Parses a .env file and returns a hashtable of key/value pairs.

    .PARAMETER Path
        Path to the .env file. Defaults to .etc/powershell/.env relative
        to the repo root derived from the calling script.

    .PARAMETER ExpandInProcess
        If set, each key is exported to the current process environment
        ($env:KEY) in addition to being returned.
    #>
    [CmdletBinding()]
    param(
        [string] $Path,
        [switch] $ExpandInProcess
    )

    if ([string]::IsNullOrWhiteSpace($Path)) {
        throw "Import-DotEnv requires -Path. Callers should resolve the repo-root .env path explicitly."
    }

    if (-not (Test-Path $Path)) {
        throw ".env file not found at $Path"
    }

    $result = @{}
    foreach ($line in Get-Content $Path) {
        if ($line -match '^\s*#' -or $line -match '^\s*$') { continue }

        # Try quoted forms first (single, then double), then bare value.
        if ($line -match "^\s*([A-Za-z_][A-Za-z0-9_]*)\s*=\s*'([^']*)'\s*$") {
            $key = $Matches[1]; $value = $Matches[2]
        }
        elseif ($line -match '^\s*([A-Za-z_][A-Za-z0-9_]*)\s*=\s*"([^"]*)"\s*$') {
            $key = $Matches[1]; $value = $Matches[2]
        }
        elseif ($line -match '^\s*([A-Za-z_][A-Za-z0-9_]*)\s*=\s*(.*?)\s*$') {
            $key = $Matches[1]; $value = $Matches[2]
        }
        else {
            continue
        }

        $result[$key] = $value
        if ($ExpandInProcess) {
            [Environment]::SetEnvironmentVariable($key, $value, 'Process')
        }
    }

    return $result
}
