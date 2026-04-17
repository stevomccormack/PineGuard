<#
.SYNOPSIS
    Load Test Audit Exceptions

.DESCRIPTION
    Dot-source friendly helper for loading allowlisted exceptions for test audits.

    Source of truth:
    - tools/audit-cli/test-audit-exceptions.json

    Paths are repo-relative and must use forward slashes.
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Convert-ToRepoRelativePath {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$Path
    )

    $rel = [System.IO.Path]::GetRelativePath($RepoRoot, $Path)
    return ($rel -replace '\\', '/')
}

function New-StringSet {
    [CmdletBinding()]
    param([string[]]$Items)

    $set = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($i in @($Items)) {
        if ([string]::IsNullOrWhiteSpace($i)) { continue }
        $set.Add(($i.Trim() -replace '\\', '/')) | Out-Null
    }
    # Return as a single object (PowerShell would otherwise enumerate it).
    Write-Output -NoEnumerate $set
    return
}

function Get-ExceptionSet {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][object]$Exceptions,
        [Parameter(Mandatory = $true)][string]$RuleId,
        [Parameter(Mandatory = $true)][string]$Key
    )

    if ($null -eq $Exceptions) {
        $set = New-StringSet -Items @()
        Write-Output -NoEnumerate $set
        return
    }

    $rule = $null
    try { $rule = $Exceptions.$RuleId } catch { $rule = $null }
    if ($null -eq $rule) {
        $set = New-StringSet -Items @()
        Write-Output -NoEnumerate $set
        return
    }

    $items = @()
    try { $items = @($rule.$Key) } catch { $items = @() }

    $set = New-StringSet -Items $items
    Write-Output -NoEnumerate $set
    return
}

function Get-PineGuardTestAuditExceptions {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][string]$RepoRoot,
        [Parameter(Mandatory = $true)][string]$ExceptionsPath
    )

    $path = if ([System.IO.Path]::IsPathRooted($ExceptionsPath)) { $ExceptionsPath } else { (Join-Path $RepoRoot $ExceptionsPath) }
    if (-not (Test-Path -LiteralPath $path)) {
        return $null
    }

    $raw = Get-Content -LiteralPath $path -Raw
    if ([string]::IsNullOrWhiteSpace($raw)) { return $null }

    return ($raw | ConvertFrom-Json -Depth 50)
}

function Import-PineGuardTestAuditExceptions {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()][string]$RepoRoot,
        [ValidateNotNullOrEmpty()] [string]$ExceptionsPath = 'tools/audit-cli/test-audit-exceptions.json'
    )

    $script:__PineGuardTestAuditExceptions = Get-PineGuardTestAuditExceptions -RepoRoot $RepoRoot -ExceptionsPath $ExceptionsPath
    return $script:__PineGuardTestAuditExceptions
}

function Get-PineGuardTestAuditExceptionSet {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory = $false)][object]$Exceptions,
        [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()][string]$RuleId,
        [Parameter(Mandatory = $true)][ValidateNotNullOrEmpty()][string]$Key
    )

    $set = Get-ExceptionSet -Exceptions $Exceptions -RuleId $RuleId -Key $Key
    Write-Output -NoEnumerate $set
    return
}

function Get-PineGuardTestAuditExceptionsObject {
    [CmdletBinding()]
    param()

    return $script:__PineGuardTestAuditExceptions
}
