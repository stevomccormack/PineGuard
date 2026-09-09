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

function Set-DotEnvVariable {
    <#
    .SYNOPSIS
        Creates or updates a single KEY=value line in a .env file.

    .DESCRIPTION
        The write-side counterpart to Import-DotEnv (D-4: secrets are written to a
        gitignored .env file, never to the User/Machine environment — F-24/F-25).

        - If -Path does not exist, it is created (parent directories too).
        - If -Name already appears as a key on some line, that line is replaced in place;
          every other line — other keys, comments, blank lines — is preserved unchanged and in
          the same order.
        - If -Name is not present, a new "KEY=value" line is appended.

        Values are written bare unless they need quoting to round-trip correctly through
        Import-DotEnv's bare-value branch (which trims surrounding whitespace and treats an
        empty match as ''): a -Value that is empty or has leading/trailing whitespace is written
        double-quoted, with any embedded double quote escaped.

    .PARAMETER Path
        Path to the .env file. Created (with parent directories) if it does not already exist.

    .PARAMETER Name
        The key to set (e.g. 'SONARQUBE_TOKEN'). Must match [A-Za-z_][A-Za-z0-9_]*.

    .PARAMETER Value
        The value to write. Never logged or echoed by this function — callers are responsible
        for not printing it themselves.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $Name,
        [Parameter(Mandatory)] [AllowEmptyString()] [string] $Value
    )

    if ($Name -notmatch '^[A-Za-z_][A-Za-z0-9_]*$') {
        throw "Set-DotEnvVariable: '$Name' is not a valid dotenv key (expected [A-Za-z_][A-Za-z0-9_]*)."
    }

    $needsQuoting = [string]::IsNullOrEmpty($Value) -or ($Value -match '^\s') -or ($Value -match '\s$')
    $formattedValue = if ($needsQuoting) { '"' + ($Value -replace '"', '\"') + '"' } else { $Value }
    $newLine = "$Name=$formattedValue"

    $parentDir = Split-Path -Parent $Path
    if (-not [string]::IsNullOrWhiteSpace($parentDir) -and -not (Test-Path -LiteralPath $parentDir)) {
        New-Item -ItemType Directory -Path $parentDir -Force | Out-Null
    }

    if (-not (Test-Path -LiteralPath $Path)) {
        Set-Content -LiteralPath $Path -Value $newLine -Encoding utf8NoBOM
        return
    }

    $lines = @(Get-Content -LiteralPath $Path)
    $keyPattern = "^\s*$([regex]::Escape($Name))\s*="
    $matchIndex = -1
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match $keyPattern) {
            $matchIndex = $i
            break
        }
    }

    if ($matchIndex -ge 0) {
        $lines[$matchIndex] = $newLine
        Set-Content -LiteralPath $Path -Value $lines -Encoding utf8NoBOM
    }
    else {
        Add-Content -LiteralPath $Path -Value $newLine -Encoding utf8NoBOM
    }
}
