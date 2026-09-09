<#
.SYNOPSIS
    Tests for the dotCover engine (T3.11), the snapshot-only fallback spike T3.10 proved out.

.DESCRIPTION
    ADJUSTED FROM THE PLAN'S ORIGINAL ASK. The plan originally wanted "a Pester test that both
    engines produce the same class list for Core" -- that is not achievable under the
    snapshot-only fallback T3.10 found necessary: a .dcvr snapshot is JetBrains Rider's own binary
    format, not a parseable class-by-class report (dotCover 2025.3.3's --xml-report-output, the
    only thing that could have produced a comparable class list, throws "Snapshot container is
    not initialized" on both TFMs -- see docs/ai/plans/tools-review-and-standardisation.md
    ## Baselines, "T3.10 -- dotCover spike"). There is no second class list to compare against
    Coverlet's.

    Instead, this file tests what actually exists and actually matters:

      1. dotcover/New-CoverageReport.ps1 -Scope Core really runs dotCover end-to-end and produces
         a real, non-trivially-sized .dcvr snapshot at the documented path (not a hang, not a
         crash, not an empty/placeholder file).
      2. Test-Coverage.ps1 -Engine DotCover throws the accurate "not supported" error (by design,
         not by omission -- see that script's own -Engine DotCover throw site) rather than
         crashing unexpectedly or silently pretending to gate on nothing.

    Context 1 is genuinely slow (a real `dotnet dotCover cover -- test ...` run, not a mock) --
    -Framework net8.0 restricts it to one target framework so the suite pays roughly T3.10's own
    observed 15-25s for this file rather than ~50s+ for both TFMs. This is the first Pester test
    in this suite that shells out to a real external tool for real work rather than exercising a
    dot-sourced function or a fixture file; that cost buys a genuine, non-mocked regression check
    on the one thing T3.10/T3.11 actually promise: the snapshot path does not hang and does not
    silently produce nothing.
#>

BeforeAll {
    . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
    $script:RepoRootDC = Get-RepoRoot -StartDirectory $PSScriptRoot
    $script:DotCoverScript = Join-Path $script:RepoRootDC 'tools' 'code-coverage' 'dotcover' 'New-CoverageReport.ps1'
    $script:GateScript = Join-Path $script:RepoRootDC 'tools' 'code-coverage' 'Test-Coverage.ps1'
}

Describe 'dotCover engine (T3.10 snapshot-only fallback, T3.11)' {

    Context 'dotcover/New-CoverageReport.ps1 -Scope Core produces a real .dcvr snapshot' {
        BeforeAll {
            $script:ExpectedSnapshotPath = Join-Path $script:RepoRootDC 'artifacts' 'code-coverage' 'dotcover' 'core' 'snapshots' 'PineGuard.Core.UnitTests.net8.0.dcvr'

            # Real invocation of the proven T3.10 fallback path against the live repo (not a mock,
            # not a fixture) -- one TFM only, to keep this test's runtime close to T3.10's own
            # observed per-TFM timing rather than paying for both. -Clean rules out a stale
            # snapshot from an earlier run making a broken script look like it passed.
            & $script:DotCoverScript -Scope Core -Framework net8.0 -Clean
            $script:DotCoverExitCode = $LASTEXITCODE
        }

        It 'exits cleanly (0) -- the snapshot-only fallback is proven not to hang or crash (T3.10)' {
            $script:DotCoverExitCode | Should -Be 0
        }

        It 'produces a .dcvr snapshot file at the documented artifacts/code-coverage/dotcover/[scope]/snapshots/ path' {
            # NOTE: the documented path segment is written as [scope], not <scope>, in this test's
            # own name (unlike elsewhere in this repo's prose) -- Pester 5 treats any '<name>' inside
            # an It/Context/Describe *name string* as a -ForEach template placeholder to interpolate,
            # and throws "The variable '$scope' cannot be retrieved because it has not been set" when
            # no -ForEach data or matching variable exists to fill it. Found the hard way while
            # writing this file: the assertion body and BeforeAll were correct throughout -- the
            # snapshot was always produced -- only this It's own *name* was the problem.
            $script:ExpectedSnapshotPath | Should -Exist
        }

        It 'the snapshot is non-trivially sized -- not an empty or truncated file' {
            (Get-Item -LiteralPath $script:ExpectedSnapshotPath).Length | Should -BeGreaterThan 102400
        }
    }

    Context 'Test-Coverage.ps1 -Engine DotCover (no Cobertura output exists to gate on)' {

        It 'throws, rather than crashing unexpectedly or silently succeeding' {
            { & $script:GateScript -Engine DotCover -Scope Core } | Should -Throw -ExpectedMessage '*not supported*'
        }

        It 'the thrown message says WHY -- cites the T3.10 upstream dotCover bug, not a generic failure' {
            { & $script:GateScript -Engine DotCover -Scope Core } | Should -Throw -ExpectedMessage '*Snapshot container is not initialized*'
        }

        It 'the thrown message points the reader at Coverlet as the actually-gateable engine' {
            { & $script:GateScript -Engine DotCover -Scope Core } | Should -Throw -ExpectedMessage '*Coverlet*'
        }
    }
}
