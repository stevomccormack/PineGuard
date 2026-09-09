<#
.SYNOPSIS
    Run Compiler Diagnostics

.DESCRIPTION
    Part of the PineGuard PowerShell toolchain.
    Builds the specified scope and captures warning- and error-severity diagnostics from the
    build pipeline: Roslyn (CS-prefixed) today, and generically any other <PREFIX><digits>
    diagnostic code the pipeline emits (e.g. NuGet audit NU19xx, ApiCompat CP0xxx, IL
    trimmer/AOT IL2xxx/IL3xxx) — see docs/ai/plans/tools-review-and-standardisation.md, D-1a.
    Outputs a structured summary (text or JSON) to stdout and writes artifacts to disk.

    The private build this script runs to collect diagnostics passes
    -p:TreatWarningsAsErrors=false, overriding the repo-wide Directory.Build.props setting, so
    genuine warnings surface as "warning" lines instead of "error" lines. It builds into the
    project's/solution's normal bin/obj output — --no-incremental already forces a full
    recompile every run, so staleness is not a concern, and a per-project -o override cannot be
    used safely here because -Scope All builds the whole solution in one dotnet invocation: -o
    on a solution build forces every project x every TFM into a single shared output directory,
    which causes real cross-project binding errors (e.g. a project's internal polyfill type
    shadowing the framework-provided one for a different project). Isolating output would
    require building each project in the scope individually even under -Scope All, which is a
    larger change than this script's diagnostic-reporting mandate warrants.

    If a build target fails to compile outright (a genuine compile failure, not "compiled with
    warnings"), that is reported explicitly and distinctly — it is never reported as
    "No compiler warnings found".

.PARAMETER Scope
    Project scope to analyze. Defaults to All.

.PARAMETER Code
    Optional regex pattern to filter diagnostic codes (e.g. "CS86" for nullability, "NU19" for
    NuGet audit, "CS0618" for obsolete). Matches the combined <PREFIX><digits> code. Never
    suppresses the build-failure signal: a failed build is still reported and still exits
    non-zero even if -Code would exclude every diagnostic on the matching lines.

    Named -Code, not -Filter: per docs/ai/plans/tools-review-and-standardisation.md §3.3,
    -Filter is reserved exclusively for `dotnet test`'s own --filter expression syntax across
    the toolchain, and this is a diagnostic-code regex, not a test filter.

.PARAMETER OutputFormat
    Output format: Text (default) or Json.

.PARAMETER Configuration
    Build configuration. Defaults to Debug.

.PARAMETER Clean
    If set, runs a clean build first.

.NOTES
    Exit codes: 0 = build succeeded, no diagnostics matched; 1 = build succeeded, warnings
    found; 2 = one or more build targets failed to compile.

    This is a deliberate exception to the general §3.4 exit-code table (0 success, 1 failure,
    2 usage/prerequisite, 3 quality-gate-not-met), kept as-is rather than renumbered to fit it:
    - Code 0 already matches §3.4 exactly.
    - "Warnings found" is conceptually a quality-gate result, but keeping it at 1 (not
      remapping to 3) preserves a distinction every /scan-roslyn-* and /fix-roslyn-* caller
      already depends on (see commit 6931ba8, F-17), and a build that fails to compile is not
      a "usage" or "prerequisite" problem with this script (2 in the strict §3.4 sense) — it
      is a hard failure of the exact thing being diagnosed, so collapsing it into 2 would
      misdescribe it just as much as collapsing it into 1 would lose the warnings/build-broken
      distinction.
    - This scheme was smoke-tested end-to-end (clean / warnings / broken build, all three
      exit paths) when it was introduced and again by T1.V's independent verification; a
      blanket renumbering purely for uniformity buys no operational benefit and risks a silent
      regression in any doc, CI step, or agent script that branches on these specific numbers
      today.
#>

[CmdletBinding()]
param(
    [ValidateSet('All', 'Core', 'MustClauses', 'GuardClauses', 'FluentValidation', 'DataAnnotations', 'Options', 'DependencyInjection', 'AspNetCore', 'ErrorOr', 'FluentResults', 'OneOf', 'MediatR', 'Analyzers', 'Testing')]
    [string] $Scope = 'All',

    [string] $Code,

    [ValidateSet('Text', 'Json')]
    [string] $OutputFormat = 'Text',

    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug',

    [switch] $Clean
)

$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'

# ── Scope → Build Target Mapping ──────────────────────────────────────────────

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'transcript.ps1')
$repoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
$transcriptPath = Start-ToolTranscript -Domain 'code-diagnostics' -RepoRoot $repoRoot
Write-Verbose "Transcript: $transcriptPath"

try {
    # A scope can own more than one source project (Analyzers = analyzer + code fixes); each is
    # built in its own pass so diagnostics from every project in the scope land in the report.
    $buildTargets = if ($Scope -eq 'All') {
        @(Join-Path $repoRoot 'PineGuard.slnx')
    }
    else {
        @((Get-PineGuardScope -Name $Scope).SourceCsprojs | ForEach-Object { Join-Path $repoRoot $_ })
    }
    foreach ($buildTarget in $buildTargets) {
        if (-not (Test-Path $buildTarget)) {
            throw "Build target not found: $buildTarget"
        }
    }

    # ── Output Directory ──────────────────────────────────────────────────────────

    $scopeSlug = $Scope.ToLowerInvariant()
    $outputDir = Join-Path $repoRoot 'artifacts' 'code-diagnostics' $scopeSlug
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null

    # ── Build ─────────────────────────────────────────────────────────────────────

    Write-Host "`n=== Compiler Diagnostics ===" -ForegroundColor Cyan
    Write-Host "Scope         : $Scope"
    Write-Host "Build Target  : $($buildTargets -join ', ')"
    Write-Host "Configuration : $Configuration"
    if ($Code) { Write-Host "Code          : $Code" }
    Write-Host ""

    $buildOutput = @()
    $failedBuildTargets = @()

    foreach ($buildTarget in $buildTargets) {
        if ($Clean) {
            Write-Host "Cleaning $(Split-Path $buildTarget -Leaf) first..." -ForegroundColor Yellow
            & dotnet clean $buildTarget -c $Configuration 2>&1 | Out-Null
        }

        $buildArgs = @(
            'build', $buildTarget,
            '--no-incremental',
            '-c', $Configuration,
            '-p:TreatWarningsAsErrors=false'
        )

        Write-Host "Building $(Split-Path $buildTarget -Leaf)..." -ForegroundColor Yellow
        $buildOutput += & dotnet @buildArgs 2>&1 | Out-String -Stream
        $exitCode = $LASTEXITCODE

        if ($exitCode -ne 0) {
            $failedBuildTargets += [PSCustomObject]@{
                Target   = $buildTarget
                ExitCode = $exitCode
            }
        }
    }

    $anyBuildFailed = $failedBuildTargets.Count -gt 0

    # ── Parse Diagnostics ─────────────────────────────────────────────────────────
    # Generic pattern: (warning|error) <PREFIX><digits>, not a regex hardcoded to CS. The prefix
    # and number are captured separately so callers can group/filter on either.
    $diagnosticPattern = '^(?<file>.+?)\((?<line>\d+),(?<col>\d+)\):\s+(?<severity>warning|error)\s+(?<codePrefix>[A-Z]+)(?<codeNumber>\d+):\s+(?<message>.+?)(?:\s+\[(?<project>.+)\])?$'

    $rawDiagnostics = @()
    foreach ($line in $buildOutput) {
        if ($line -match $diagnosticPattern) {
            # Capture everything needed from the outer match before running any nested -match
            # below, which would otherwise overwrite $Matches.
            $file = $Matches['file'].Trim()
            $lineNumber = [int]$Matches['line']
            $column = [int]$Matches['col']
            $severity = $Matches['severity']
            $codePrefix = $Matches['codePrefix']
            $codeNumber = $Matches['codeNumber']
            $message = $Matches['message'].Trim()
            $projectRaw = if ($Matches['project']) { $Matches['project'].Trim() } else { '' }

            # Multi-targeted projects suffix the project path with "::TargetFramework=<tfm>" for
            # each inner per-TFM build pass — that's the source of the one-warning-per-TFM
            # duplication this dedupes below.
            $targetFramework = $null
            $projectPath = $projectRaw
            if ($projectRaw -match '^(?<path>.+)::TargetFramework=(?<tfm>.+)$') {
                $projectPath = $Matches['path']
                $targetFramework = $Matches['tfm']
            }

            $rawDiagnostics += [PSCustomObject]@{
                File            = $file
                Line            = $lineNumber
                Column          = $column
                Severity        = $severity
                CodePrefix      = $codePrefix
                CodeNumber      = $codeNumber
                Code            = "$codePrefix$codeNumber"
                Message         = $message
                Project         = $projectPath
                TargetFramework = $targetFramework
            }
        }
    }

    # ── Dedupe Across TFMs ────────────────────────────────────────────────────────
    # Same (file, line, column, code) reported once per TFM the project multi-targets collapses to
    # one entry; which TFM(s) it was seen in is kept as a field so a TFM-only diagnostic is not
    # hidden by the dedupe.
    $diagnostics = foreach ($group in ($rawDiagnostics | Group-Object -Property File, Line, Column, Code)) {
        $items = $group.Group
        $first = $items[0]
        $targetFrameworks = @($items.TargetFramework | Where-Object { $_ } | Select-Object -Unique | Sort-Object)
        $severity = if ($items.Severity -contains 'error') { 'error' } else { 'warning' }

        [PSCustomObject]@{
            File             = $first.File
            Line             = $first.Line
            Column           = $first.Column
            Severity         = $severity
            CodePrefix       = $first.CodePrefix
            CodeNumber       = $first.CodeNumber
            Code             = $first.Code
            Message          = $first.Message
            Project          = $first.Project
            TargetFrameworks = $targetFrameworks
            Occurrences      = $items.Count
        }
    }

    # ── Apply Code Filter ─────────────────────────────────────────────────────────
    # -Code narrows which diagnostics are reported; it never masks the build-failure signal, which
    # is driven solely by $LASTEXITCODE above.
    if ($Code) {
        $diagnostics = @($diagnostics | Where-Object { $_.Code -match $Code })
    }

    $warningDiagnostics = @($diagnostics | Where-Object { $_.Severity -eq 'warning' })
    $errorDiagnostics = @($diagnostics | Where-Object { $_.Severity -eq 'error' })

    # ── Write Artifacts ───────────────────────────────────────────────────────────

    $jsonPath = Join-Path $outputDir 'diagnostics.json'
    $report = [PSCustomObject]@{
        Scope              = $Scope
        Configuration      = $Configuration
        Code               = if ($Code) { $Code } else { $null }
        Timestamp          = (Get-Date -Format 'o')
        BuildSucceeded     = -not $anyBuildFailed
        FailedBuildTargets = $failedBuildTargets
        TotalWarnings      = $warningDiagnostics.Count
        TotalErrors        = $errorDiagnostics.Count
        ByCode             = ($warningDiagnostics | Group-Object Code | Sort-Object Count -Descending | ForEach-Object {
            [PSCustomObject]@{ Code = $_.Name; Count = $_.Count }
        })
        ByFile             = ($warningDiagnostics | Group-Object File | Sort-Object Count -Descending | ForEach-Object {
            [PSCustomObject]@{ File = $_.Name; Count = $_.Count }
        })
        Warnings           = $warningDiagnostics
        Errors             = $errorDiagnostics
    }

    $report | ConvertTo-Json -Depth 6 | Set-Content -Path $jsonPath -Encoding UTF8
    Write-Host "`nArtifacts written to: $jsonPath" -ForegroundColor DarkGray

    # ── Output ────────────────────────────────────────────────────────────────────

    if ($OutputFormat -eq 'Json') {
        $report | ConvertTo-Json -Depth 6
    }
    elseif ($anyBuildFailed) {
        Write-Host "`n=== Results ===" -ForegroundColor Cyan
        Write-Host "BUILD FAILED — one or more build targets did not compile." -ForegroundColor Red
        foreach ($failed in $failedBuildTargets) {
            Write-Host ("  {0} (dotnet build exit code {1})" -f $failed.Target, $failed.ExitCode) -ForegroundColor Red
        }

        if ($errorDiagnostics.Count -gt 0) {
            Write-Host "`nCompiler errors:" -ForegroundColor Red
            foreach ($e in $errorDiagnostics) {
                $relPath = $e.File -replace [regex]::Escape($repoRoot), ''
                Write-Host ("  {0} ({1},{2}): {3}: {4}" -f $relPath.TrimStart('\', '/'), $e.Line, $e.Column, $e.Code, $e.Message) -ForegroundColor Red
            }
        }

        if ($warningDiagnostics.Count -gt 0) {
            Write-Host "`n$($warningDiagnostics.Count) warning(s) were also detected (see $jsonPath)." -ForegroundColor Yellow
        }
    }
    else {
        Write-Host "`n=== Results ===" -ForegroundColor Cyan
        Write-Host "Total warnings: $($warningDiagnostics.Count)"

        if ($warningDiagnostics.Count -gt 0) {
            Write-Host "`nBy warning code:" -ForegroundColor Yellow
            $warningDiagnostics | Group-Object Code | Sort-Object Count -Descending | ForEach-Object {
                Write-Host ("  {0,-10} {1}" -f $_.Name, $_.Count)
            }

            Write-Host "`nBy file:" -ForegroundColor Yellow
            $warningDiagnostics | Group-Object File | Sort-Object Count -Descending | Select-Object -First 20 | ForEach-Object {
                $relPath = $_.Name -replace [regex]::Escape($repoRoot), ''
                Write-Host ("  {0,-4} {1}" -f $_.Count, $relPath.TrimStart('\', '/'))
            }

            Write-Host "`nDetails:" -ForegroundColor Yellow
            foreach ($w in $warningDiagnostics) {
                $relPath = $w.File -replace [regex]::Escape($repoRoot), ''
                $tfmSuffix = if ($w.TargetFrameworks.Count -gt 0) { " [{0}]" -f ($w.TargetFrameworks -join ', ') } else { '' }
                Write-Host ("  {0} ({1},{2}): {3}: {4}{5}" -f $relPath.TrimStart('\', '/'), $w.Line, $w.Column, $w.Code, $w.Message, $tfmSuffix)
            }
        }
        else {
            Write-Host "`nNo compiler warnings found." -ForegroundColor Green
        }
    }

    # ── Exit Code ─────────────────────────────────────────────────────────────────
    # 0 = clean build, no diagnostics. 1 = build succeeded but warnings were found. 2 = the build
    # itself failed to compile — never conflated with "succeeded with warnings".

    if ($anyBuildFailed) {
        exit 2
    }
    elseif ($warningDiagnostics.Count -gt 0) {
        exit 1
    }
    else {
        exit 0
    }

}
finally {
    Stop-Transcript | Out-Null
}
