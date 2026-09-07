<#
.SYNOPSIS
    Combined quality gate for tools/**: PSScriptAnalyzer plus the Pester suite.

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain (T2.04,
    docs/ai/plans/tools-review-and-standardisation.md, Phase 2). Runs two independent checks over
    tools/**, excluding tools/audit-cli/** (reviewed separately), and reports one combined result:

      1. PSScriptAnalyzer, using the repo-root PSScriptAnalyzerSettings.psd1 (T2.01), across
         every .ps1 file under -Path (default: tools).
      2. The Pester suite at tools/.tests/ (T2.03, D-7). This always runs from that fixed location
         regardless of -Path, since the suite's tests (registry parity, dotenv/Cobertura parsers,
         Get-RepoRoot, git helpers, help/BOM/Windows-ism hygiene) are not organised per subfolder.

    A summary — PSScriptAnalyzer finding counts by rule and severity, Pester pass/fail counts — is
    printed to the console and written to artifacts/tools-lint/test-tools-summary.txt.

    Exit codes follow §3.4 of the plan:
      0   both checks are clean.
      1   an unexpected script-level error occurred (not a quality-gate failure).
      2   a required module is missing: PSScriptAnalyzer, or Pester >= 5.0.
      3   PSScriptAnalyzer reported an Error- or Warning-severity finding, or a Pester test failed.

    As of Phase 2, this gate is EXPECTED to exit 3. PSScriptAnalyzer still carries the pre-Phase-3
    whitespace/indentation/verb findings recorded in the plan's T2.02 baseline, and the Pester
    suite still carries roughly 21-23 intentionally red tests (Help-Placeholder, Windows-Isms)
    that Phase 3 through 5 clear. The plan's own CI wiring (T2.05) runs this script with
    continue-on-error: true for exactly that reason — a non-zero exit here today is correct, not a
    bug in this script.

.PARAMETER Path
    Root folder to scan with PSScriptAnalyzer, relative to the repository root (or an absolute
    path). Defaults to tools. tools/audit-cli/ is always excluded, even when it falls under
    -Path. Does not affect the Pester suite, which always runs from tools/.tests/.

.PARAMETER SkipLint
    Skip the PSScriptAnalyzer pass entirely.

.PARAMETER SkipPester
    Skip the Pester suite entirely.

.EXAMPLE
    ./tools/testing/Test-Tools.ps1

    Runs both checks over all of tools/** (excluding audit-cli/) and reports the combined result.

.EXAMPLE
    ./tools/testing/Test-Tools.ps1 -SkipPester -Path tools/git

    Lints only tools/git/ and skips the Pester suite.
#>

[CmdletBinding()]
param(
    [string] $Path = 'tools',
    [switch] $SkipLint,
    [switch] $SkipPester
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')

try {
    $repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
    $exitCode = 0

    # ── Prerequisite check ──────────────────────────────────────────────────────
    $missingModules = [System.Collections.Generic.List[string]]::new()

    $pssaAvailable = [bool](Get-Module -ListAvailable -Name PSScriptAnalyzer)
    if (-not $SkipLint -and -not $pssaAvailable) {
        $missingModules.Add('PSScriptAnalyzer')
    }

    $pesterModule = Get-Module -ListAvailable -Name Pester |
        Where-Object { $_.Version -ge [version] '5.0.0' } |
        Sort-Object Version -Descending |
        Select-Object -First 1
    if (-not $SkipPester -and -not $pesterModule) {
        $missingModules.Add('Pester (>= 5.0)')
    }

    if ($missingModules.Count -gt 0) {
        Write-Host "`nMissing required module(s): $($missingModules -join ', ')" -ForegroundColor Red
        if ($missingModules -contains 'PSScriptAnalyzer') {
            Write-Host '  Install-Module -Name PSScriptAnalyzer -Scope CurrentUser' -ForegroundColor Yellow
        }
        if ($missingModules -contains 'Pester (>= 5.0)') {
            Write-Host '  Install-Module -Name Pester -MinimumVersion 5.0 -Scope CurrentUser' -ForegroundColor Yellow
        }
        exit 2
    }

    $artifactDir = Join-Path $repoRoot 'artifacts' 'tools-lint'
    if (-not (Test-Path $artifactDir)) { New-Item -ItemType Directory -Path $artifactDir -Force | Out-Null }
    $summaryPath = Join-Path $artifactDir 'test-tools-summary.txt'
    $summaryLines = [System.Collections.Generic.List[string]]::new()

    Write-Host "`n=== Test-Tools ===" -ForegroundColor Cyan
    $summaryLines.Add("Test-Tools run: $(Get-Date -Format 'o')")

    # ── PSScriptAnalyzer ──────────────────────────────────────────────────────────
    $lintFindings = @()
    if ($SkipLint) {
        Write-Host 'Lint    : skipped (-SkipLint)' -ForegroundColor Yellow
        $summaryLines.Add('PSScriptAnalyzer: skipped')
    }
    else {
        Import-Module PSScriptAnalyzer -Force

        $lintRoot = if ([IO.Path]::IsPathRooted($Path)) { $Path } else { Join-Path $repoRoot $Path }
        $settingsPath = Join-Path $repoRoot 'PSScriptAnalyzerSettings.psd1'

        $scriptFiles = Get-ChildItem -Path $lintRoot -Recurse -File -Include '*.ps1' |
            Where-Object { $_.FullName -notmatch '[\\/]audit-cli[\\/]' }

        Write-Host "Lint    : scanning $($scriptFiles.Count) file(s) under $Path (excluding audit-cli/)..." -ForegroundColor Cyan

        $lintFindings = @(foreach ($file in $scriptFiles) {
                Invoke-ScriptAnalyzer -Path $file.FullName -Settings $settingsPath
            })

        $bySeverity = $lintFindings | Group-Object Severity
        $errorCount = [int]($bySeverity | Where-Object Name -eq 'Error' | Select-Object -ExpandProperty Count)
        $warningCount = [int]($bySeverity | Where-Object Name -eq 'Warning' | Select-Object -ExpandProperty Count)
        $infoCount = [int]($bySeverity | Where-Object Name -eq 'Information' | Select-Object -ExpandProperty Count)

        $lintColor = if ($errorCount -gt 0 -or $warningCount -gt 0) { 'Red' } else { 'Green' }
        Write-Host ("Lint    : {0} finding(s) - {1} Error, {2} Warning, {3} Information" -f $lintFindings.Count, $errorCount, $warningCount, $infoCount) -ForegroundColor $lintColor

        $summaryLines.Add("PSScriptAnalyzer: $($lintFindings.Count) finding(s) - $errorCount Error, $warningCount Warning, $infoCount Information")
        foreach ($group in ($lintFindings | Group-Object RuleName | Sort-Object Count -Descending)) {
            $summaryLines.Add("  $($group.Name): $($group.Count)")
        }

        if ($errorCount -gt 0 -or $warningCount -gt 0) {
            $exitCode = 3
        }
    }

    # ── Pester ────────────────────────────────────────────────────────────────────
    if ($SkipPester) {
        Write-Host 'Pester  : skipped (-SkipPester)' -ForegroundColor Yellow
        $summaryLines.Add('Pester: skipped')
    }
    else {
        Import-Module Pester -RequiredVersion $pesterModule.Version -Force

        $testsPath = Join-Path $repoRoot 'tools' '.tests'
        Write-Host "Pester  : running suite at tools/.tests/ (Pester $($pesterModule.Version))..." -ForegroundColor Cyan

        $pesterConfig = New-PesterConfiguration
        $pesterConfig.Run.Path = $testsPath
        $pesterConfig.Run.PassThru = $true
        $pesterConfig.Output.Verbosity = 'Normal'

        $pesterResult = Invoke-Pester -Configuration $pesterConfig

        $pesterColor = if ($pesterResult.FailedCount -gt 0) { 'Red' } else { 'Green' }
        Write-Host ("Pester  : {0} test(s) - {1} passed, {2} failed, {3} skipped" -f $pesterResult.TotalCount, $pesterResult.PassedCount, $pesterResult.FailedCount, $pesterResult.SkippedCount) -ForegroundColor $pesterColor

        $summaryLines.Add("Pester: $($pesterResult.TotalCount) test(s) - $($pesterResult.PassedCount) passed, $($pesterResult.FailedCount) failed, $($pesterResult.SkippedCount) skipped")

        if ($pesterResult.FailedCount -gt 0) {
            $exitCode = 3
        }
    }

    $summaryLines | Out-File -FilePath $summaryPath -Encoding utf8
    Write-Host "`nSummary written to: $summaryPath" -ForegroundColor Gray

    if ($exitCode -eq 0) {
        Write-Host "`nTest-Tools: PASSED`n" -ForegroundColor Green
    }
    else {
        Write-Host "`nTest-Tools: FAILED - quality gate not met (exit $exitCode)`n" -ForegroundColor Red
    }

    exit $exitCode
}
catch {
    Write-Host "`nTest-Tools: unexpected error - $($_.Exception.Message)" -ForegroundColor Red
    Write-Host $_.ScriptStackTrace -ForegroundColor DarkRed
    exit 1
}
