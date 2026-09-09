<#
.SYNOPSIS
    Shared transcript helper for the PineGuard PowerShell toolchain.

.DESCRIPTION
    Dot-source this file to import Start-ToolTranscript into the calling script's scope.

    Self-contained: dot-sources path.ps1 (Get-RepoRoot) itself so it does not depend on load
    order.

    Addresses F-43: nothing currently writes to logs/, despite tools/README.md promising output
    there. Wiring Start-ToolTranscript into every entry script is each domain's own later task —
    this file only makes the helper available and correct.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'path.ps1')

function Start-ToolTranscript {
    <#
    .SYNOPSIS
        Starts a PowerShell transcript under logs/<Domain>/<yyyyMMdd-HHmmss>.log.

    .DESCRIPTION
        Creates logs/<Domain>/ under the repo root if it does not already exist, then starts a
        transcript there. Returns the transcript's full path. Callers are responsible for calling
        `Stop-Transcript` when done (e.g. in a `finally` block).

    .PARAMETER Domain
        The tool domain the transcript belongs to (e.g. 'code-coverage', 'code-scan/sonarqube').
        Used verbatim as the subfolder name under logs/.

    .PARAMETER RepoRoot
        Override the repo root. Defaults to the result of Get-RepoRoot.
    #>
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)]
        [string] $Domain,

        [string] $RepoRoot
    )

    $root = $RepoRoot
    if ([string]::IsNullOrWhiteSpace($root)) {
        $root = Get-RepoRoot -StartDirectory $PSScriptRoot
    }

    $logsDir = Join-Path $root (Join-Path 'logs' $Domain)
    New-Item -ItemType Directory -Path $logsDir -Force | Out-Null

    $stamp = (Get-Date).ToString('yyyyMMdd-HHmmss')
    $logPath = Join-Path $logsDir "$stamp.log"

    Start-Transcript -Path $logPath -Append | Out-Null

    return $logPath
}
