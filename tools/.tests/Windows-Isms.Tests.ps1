<#
.SYNOPSIS
    F-30: greps tools/** for hardcoded Windows-only path separators and mechanisms that break
    pwsh-on-Linux.

.DESCRIPTION
    Four categories, matched to the plan's own evidence for F-30 and to §3.4's cross-platform
    rule ("no \ in path literals, no .exe, no $env:TEMP, no registry access. A Pester test greps
    for these."):

      A. Hardcoded backslash path separators — either inside a Join-Path/dot-source call's own
         string-literal argument (e.g. `Join-Path $PSScriptRoot '..\.shared\path.ps1'`), or a bare
         `'src\...'`/`'tests\...'`/`'artifacts\...'`/`'xplat\...'`/`'tools\...'` string literal
         used as a path (this also catches tools/.shared/dotnet-projects.ps1's own registry
         fields — SourceDir, SourceCsprojs, TestCsproj, DefaultSourcePrefix — which the plan
         explicitly cites as F-30 evidence, not just the orchestrator scripts that dot-source it).
      B. A hardcoded `.exe` extension (e.g. `reportgenerator.exe`).
      C. `$env:TEMP` (Windows-specific; cross-platform code should use
         `[IO.Path]::GetTempPath()`).
      D. `SetEnvironmentVariable(..., 'User')` / `..., [EnvironmentVariableTarget]::User` —
         Windows registry-backed persistent env vars. `..., 'Process'` is NOT flagged: it only
         touches the current process and is not Windows-specific.

    T3.09 (D-5 sweep) is the task that actually fixes these; T1.03 already fixed the one
    `$env:TEMP` usage that used to exist (tools/code-coverage/Test-Coverage.ps1's isolated-copy path), which
    is why category C is expected to already pass.

    SCOPE NOTE: unlike the other hygiene tests in this suite, this one excludes tools/.tests/**
    (in addition to tools/audit-cli/**). A pattern-matching test must contain its own search
    patterns as string literals — e.g. the text "Join-Path...'...\\" or "$env:TEMP" itself — so
    scanning tools/.tests would make this suite flag its own source as a false positive. That is
    a property of grepping for regexes-as-strings, not a weakening of the check against the
    actual tooling under review.

    NOTE (Pester v5 discovery vs. run): this file has no -ForEach, so — unlike the other test
    files in this suite — it needs no top-level (Discovery-phase) computation at all. The file
    list and the pattern-matching helper functions live entirely inside BeforeAll, since top-level
    `function`/`$script:` statements outside BeforeAll do not carry over into the Run phase in
    Pester 5, and nothing here needs to be visible any earlier than that.
#>

Describe 'Windows-ism absence (F-30)' {

    BeforeAll {
        . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
        $script:RepoRootWIRun = Get-RepoRoot -StartDirectory $PSScriptRoot
        $script:WindowsIsmFiles = @(
            Get-ChildItem -Path (Join-Path $script:RepoRootWIRun 'tools') -Recurse -File -Include '*.ps1', '*.psm1', '*.psd1' |
                Where-Object {
                    $_.FullName -notmatch '[\\/]audit-cli[\\/]' -and
                    $_.FullName -notmatch '[\\/]\.tests[\\/]'
                } |
                ForEach-Object {
                    [pscustomobject]@{
                        RelativePath = $_.FullName.Substring($script:RepoRootWIRun.Length).TrimStart('\', '/')
                        FullName     = $_.FullName
                    }
                }
        )

        function Measure-PatternHits {
            <#
            .SYNOPSIS
                Counts regex matches for -Pattern across -Files; returns @{ Total; ByFile }.
            #>
            param(
                [Parameter(Mandatory)] [array] $Files,
                [Parameter(Mandatory)] [string[]] $Pattern
            )

            $total = 0
            $byFile = [ordered]@{}

            foreach ($file in $Files) {
                $content = Get-Content -Raw $file.FullName
                $count = 0
                foreach ($p in $Pattern) {
                    $count += [regex]::Matches($content, $p).Count
                }
                if ($count -gt 0) {
                    $byFile[$file.RelativePath] = $count
                    $total += $count
                }
            }

            return @{ Total = $total; ByFile = $byFile }
        }

        function Format-HitSummary {
            param([hashtable] $ByFile)

            if ($ByFile.Count -eq 0) { return '(none)' }
            return (($ByFile.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join '; ')
        }
    }

    It 'the file scan finds at least one candidate file (sanity check)' {
        $script:WindowsIsmFiles.Count | Should -BeGreaterThan 0
    }

    Context 'A: hardcoded backslash path separators' {
        It 'has zero occurrences across tools/** (excluding audit-cli and .tests)' {
            $patterns = @(
                "Join-Path[^\r\n]*?'[^']*\\",
                'Join-Path[^\r\n]*?"[^"]*\\',
                "'(?:src|tests|artifacts|xplat|tools)\\[^']*'",
                '"(?:src|tests|artifacts|xplat|tools)\\[^"]*"'
            )
            $result = Measure-PatternHits -Files $script:WindowsIsmFiles -Pattern $patterns
            $result.Total | Should -Be 0 -Because "F-30 backslash path separators break pwsh-on-Linux; T3.09 fixes this. Hits: $(Format-HitSummary $result.ByFile)"
        }
    }

    Context 'B: hardcoded .exe extension' {
        It 'has zero occurrences across tools/** (excluding audit-cli and .tests)' {
            $result = Measure-PatternHits -Files $script:WindowsIsmFiles -Pattern '(?i)\b[\w-]+\.exe\b'
            $result.Total | Should -Be 0 -Because "F-30 .exe is Windows-only; T3.09 fixes this. Hits: $(Format-HitSummary $result.ByFile)"
        }
    }

    Context 'C: $env:TEMP' {
        It 'has zero occurrences across tools/** (excluding audit-cli and .tests) — already fixed by T1.03' {
            $result = Measure-PatternHits -Files $script:WindowsIsmFiles -Pattern '\$env:TEMP\b'
            $result.Total | Should -Be 0 -Because "F-30 `$env:TEMP is Windows-only; use [IO.Path]::GetTempPath(). Hits: $(Format-HitSummary $result.ByFile)"
        }
    }

    Context "D: SetEnvironmentVariable(...,'User')" {
        It 'has zero occurrences across tools/** (excluding audit-cli and .tests)' {
            $result = Measure-PatternHits -Files $script:WindowsIsmFiles -Pattern "SetEnvironmentVariable\([^)]*?(?:'User'|\[EnvironmentVariableTarget\]::User)\)"
            $result.Total | Should -Be 0 -Because "F-24/F-30: persistent 'User'-target env vars are Windows-registry-backed; T3.09/D-4 fixes this. Hits: $(Format-HitSummary $result.ByFile)"
        }
    }
}
