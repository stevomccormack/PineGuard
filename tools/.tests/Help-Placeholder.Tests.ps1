<#
.SYNOPSIS
    F-46: asserts no tools/** script still carries the placeholder help text
    "See the param block for details".

.DESCRIPTION
    The plan (F-46, S3) found 21 scripts under tools/** whose .PARAMETER blocks were never
    filled in — they still say "See the param block for details" instead of real documentation.
    Phase 5 (§3.4 "Help" rule: "every .PARAMETER has real text... a Pester test fails on the
    placeholder string") is what actually fixes the prose; this test is that Pester test, added
    now (Phase 2) so the debt is tracked from today rather than discovered mid-Phase-5.

    This is EXPECTED TO FAIL today — see tools/.tests/README.md for the current count.

    SCOPE NOTE: like Windows-Isms.Tests.ps1, this excludes tools/.tests/** in addition to
    tools/audit-cli/**. A test that greps for the literal placeholder string must contain that
    string itself (see the -Because message below, and the Should -Not -Match argument above it),
    which would otherwise make this file flag its own source as a false positive.

    NOTE (Pester v5 discovery vs. run): the candidate list is computed twice on purpose — once as
    top-level script code (Discovery phase, so -ForEach has data to build the test tree from) and
    again inside BeforeAll (Run phase, so the sanity-check It, which reads $script:-scoped state
    directly rather than via -ForEach injection, actually sees it).
#>

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
$script:RepoRootHP = Get-RepoRoot -StartDirectory $PSScriptRoot
$script:PlaceholderCandidates = @(
    Get-ChildItem -Path (Join-Path $script:RepoRootHP 'tools') -Recurse -File -Include '*.ps1', '*.psm1', '*.psd1' |
        Where-Object {
            $_.FullName -notmatch '[\\/]audit-cli[\\/]' -and
            $_.FullName -notmatch '[\\/]\.tests[\\/]'
        } |
        ForEach-Object {
            @{
                RelativePath = $_.FullName.Substring($script:RepoRootHP.Length).TrimStart('\', '/')
                FullName     = $_.FullName
            }
        }
)

Describe 'Placeholder help text absence (F-46)' {

    BeforeAll {
        . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
        $script:RepoRootHPRun = Get-RepoRoot -StartDirectory $PSScriptRoot
        $script:PlaceholderCandidatesRun = @(
            Get-ChildItem -Path (Join-Path $script:RepoRootHPRun 'tools') -Recurse -File -Include '*.ps1', '*.psm1', '*.psd1' |
                Where-Object {
                    $_.FullName -notmatch '[\\/]audit-cli[\\/]' -and
                    $_.FullName -notmatch '[\\/]\.tests[\\/]'
                }
        )
    }

    It 'the file scan finds at least one candidate file (sanity check)' {
        $script:PlaceholderCandidatesRun.Count | Should -BeGreaterThan 0
    }

    It '<RelativePath> does not contain the placeholder ".PARAMETER" text' -ForEach $script:PlaceholderCandidates {
        (Get-Content -Raw $FullName) |
            Should -Not -Match 'See the param block for details' `
            -Because "F-46: $RelativePath still has placeholder help text; Phase 5 replaces it with real .PARAMETER documentation"
    }
}
