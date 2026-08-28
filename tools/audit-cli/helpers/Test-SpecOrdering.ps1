<#
.SYNOPSIS
    Test Spec Ordering

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.

.PARAMETER AuditRuleId
    See the param block for details.

.PARAMETER ReportPath
    See the param block for details.

.PARAMETER OutputPath
    See the param block for details.

.PARAMETER AllowViolations
    See the param block for details.

.PARAMETER RepoRoot
    See the param block for details.
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory = $false)] [ValidateNotNullOrEmpty()] [string] $AuditRuleId = "Rule08",
    [Parameter(Mandatory = $false)] [ValidateNotNullOrEmpty()] [string] $ReportPath = "artifacts/audit/Rule08-method-ordering-parity.txt",
    [Parameter(Mandatory = $false)] [string] $OutputPath = "",
    [Parameter(Mandatory = $false)] [switch] $AllowViolations,
    [Parameter(Mandatory = $false)] [string] $RepoRoot = ""
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Write-Host "AuditRule: $AuditRuleId - Cross-layer method ordering parity" -ForegroundColor Cyan
Write-Host "Note: GuardClauses ordering is compared by the Must clause each Guard method invokes (complement-based ordering)." -ForegroundColor DarkGray

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')
$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

if (-not [string]::IsNullOrWhiteSpace($OutputPath)) {
    $ReportPath = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $OutputPath
}

$projectFile = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path 'tools/audit-cli/solution/PineGuard.AuditCli.csproj'

$reportPathResolved = if ([System.IO.Path]::IsPathRooted($ReportPath)) { $ReportPath } else { Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $ReportPath }
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPathResolved)

$runId = [Guid]::NewGuid().ToString('n')
$tmpBase = Join-Path (Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path 'artifacts/audit/tmp') $runId
$objPath = Join-Path $tmpBase 'obj/'
$binPath = Join-Path $tmpBase 'bin/'

New-Item -ItemType Directory -Force -Path $tmpBase | Out-Null

$argsList = @(
    "run",
    "--project", $projectFile,
    "-c", "Release",
    "-p:MSBuildProjectExtensionsPath=$objPath",
    "-p:IntermediateOutputPath=$objPath",
    "-p:OutputPath=$binPath",
    "--",
    "--audit", "ordering",
    "--report", $reportPathResolved,
    "--repoRoot", $repoRootResolved
)

if ($AllowViolations) {
    $argsList += @("--allowViolations", "true")
}

Write-Host "dotnet $($argsList -join ' ')" -ForegroundColor Cyan
& dotnet @argsList
