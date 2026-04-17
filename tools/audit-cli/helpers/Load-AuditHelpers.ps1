<#
.SYNOPSIS
    Load Audit Helpers

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Resolve-PineGuardRepoRoot {
    param(
        [Parameter(Mandatory = $false)]
        [string]$RepoRoot,

        [Parameter(Mandatory = $false)]
        [string]$ScriptRoot
    )

    if (-not [string]::IsNullOrWhiteSpace($RepoRoot)) {
        return (Resolve-Path $RepoRoot).Path
    }

    if ([string]::IsNullOrWhiteSpace($ScriptRoot)) {
        $ScriptRoot = $PSScriptRoot
    }

    $current = (Resolve-Path $ScriptRoot).Path
    while (-not [string]::IsNullOrWhiteSpace($current)) {
        if (
            (Test-Path (Join-Path $current 'PineGuard.slnx')) -or
            (Test-Path (Join-Path $current '.git')) -or
            ((Test-Path (Join-Path $current 'src')) -and (Test-Path (Join-Path $current 'tests')))
        ) {
            return $current
        }

        $parent = Split-Path $current -Parent
        if ($parent -eq $current) {
            break
        }
        $current = $parent
    }

    throw "Unable to resolve PineGuard repo root from ScriptRoot '$ScriptRoot'. Pass -RepoRoot explicitly."
}

function Resolve-PineGuardPath {
    param(
        [Parameter(Mandatory = $true)]
        [string]$RepoRoot,

        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    if ([System.IO.Path]::IsPathRooted($Path)) {
        return (Resolve-Path $Path).Path
    }

    return (Join-Path $RepoRoot $Path)
}

function Ensure-PineGuardDirectory {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    if (-not (Test-Path $Path)) {
        New-Item -ItemType Directory -Path $Path -Force | Out-Null
    }
}

function Write-PineGuardAuditHeader {
    param(
        [Parameter(Mandatory = $true)]
        [string]$AuditRuleId,

        [Parameter(Mandatory = $true)]
        [string]$Title,

        [Parameter(Mandatory = $true)]
        [string]$RepoRoot,

        [Parameter(Mandatory = $false)]
        [string]$OutputPath = ''
    )

    Write-Host "AuditRule: $AuditRuleId - $Title" -ForegroundColor Cyan
    Write-Host "RepoRoot : $RepoRoot" -ForegroundColor DarkGray
    if (-not [string]::IsNullOrWhiteSpace($OutputPath)) {
        Write-Host "Output  : $OutputPath" -ForegroundColor DarkGray
    }
}
