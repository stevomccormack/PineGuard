<#
.SYNOPSIS
    Run the PineGuard.AuditCli naming audit.

.DESCRIPTION
    Invokes tools/audit-cli/solution to perform the naming/nullability/collision audit.
    This script is called by tools/audit-cli/rules/Test-Rule01-Naming.ps1.

.PARAMETER AuditRuleId
    Audit rule identifier for reporting.

.PARAMETER Project
    AuditCli project key (e.g., MustClauses).

.PARAMETER SpecPath
    Naming spec JSON path.

.PARAMETER ReportPath
    Naming report output path.

.PARAMETER OutputPath
    If provided, overrides ReportPath (convenience for wrappers).

.PARAMETER SnapshotPath
    Snapshot output path (only used when CreateSnapshot is set).

.PARAMETER CreateSpec
    If set, instruct AuditCli to write a spec template to SpecPath.

.PARAMETER CreateSnapshot
    If set, instruct AuditCli to write a snapshot to SnapshotPath.

.PARAMETER AllowViolations
    If set, AuditCli exits 0 even if violations exist.

.PARAMETER RepoRoot
    Repository root. If omitted, auto-detected.
#>

[CmdletBinding()]
param(
    [ValidateNotNullOrEmpty()] [string] $AuditRuleId = 'Rule01',
    [ValidateNotNullOrEmpty()] [string] $Project = 'MustClauses',
    [ValidateNotNullOrEmpty()] [string] $SpecPath = 'artifacts/audit/naming-spec.json',
    [ValidateNotNullOrEmpty()] [string] $ReportPath = 'artifacts/audit/naming-audit.json',
    [string] $OutputPath = '',
    [string] $SnapshotPath = '',
    [switch] $CreateSpec,
    [switch] $CreateSnapshot,
    [switch] $AllowViolations,
    [string] $RepoRoot = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'Load-AuditHelpers.ps1')

$repoRootResolved = Resolve-PineGuardRepoRoot -RepoRoot $RepoRoot -ScriptRoot $PSScriptRoot

if (-not [string]::IsNullOrWhiteSpace($OutputPath)) {
    $ReportPath = $OutputPath
}

$auditCliProject = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path 'tools/audit-cli/solution/PineGuard.AuditCli.csproj'
if (-not (Test-Path $auditCliProject)) {
    throw "AuditCli project not found: $auditCliProject"
}

$specPathResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $SpecPath
$reportPathResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $ReportPath

Ensure-PineGuardDirectory -Path (Split-Path -Parent $specPathResolved)
Ensure-PineGuardDirectory -Path (Split-Path -Parent $reportPathResolved)

$snapshotPathResolved = ''
if ($CreateSnapshot.IsPresent) {
    if ([string]::IsNullOrWhiteSpace($SnapshotPath)) {
        $SnapshotPath = 'artifacts/audit/naming-snapshot.json'
    }
    $snapshotPathResolved = Resolve-PineGuardPath -RepoRoot $repoRootResolved -Path $SnapshotPath
    Ensure-PineGuardDirectory -Path (Split-Path -Parent $snapshotPathResolved)
}

Write-PineGuardAuditHeader -AuditRuleId $AuditRuleId -Title 'MustClauses naming/nullability/collision audit' -RepoRoot $repoRootResolved -OutputPath $ReportPath

$dotnetArgs = @(
    'run',
    '--project', $auditCliProject,
    '-c', 'Release',
    '--',
    '--audit', 'naming',
    '--repoRoot', $repoRootResolved,
    '--project', $Project,
    '--spec', $specPathResolved,
    '--report', $reportPathResolved,
    '--allowViolations', ($AllowViolations.IsPresent ? 'true' : 'false')
)

if ($CreateSpec.IsPresent) {
    $dotnetArgs += @('--createSpecTemplate', 'true')
}

if ($CreateSnapshot.IsPresent) {
    $dotnetArgs += @('--createSnapshot', 'true', '--snapshot', $snapshotPathResolved)
}

& dotnet @dotnetArgs | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw "AuditCli naming audit failed with exit code $LASTEXITCODE. Report: $reportPathResolved"
}
