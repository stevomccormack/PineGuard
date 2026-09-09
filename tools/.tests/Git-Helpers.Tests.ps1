<#
.SYNOPSIS
    Regression test for Assert-IndexClean (tools/.shared/git.ps1), fixed in T1.01 / commit 6a4736e.

.DESCRIPTION
    F-16 (S1, Tier 0 safety violation): the old Ensure-IndexClean ran `git restore --staged .`
    whenever the index had staged changes and then continued — `git restore --staged .` is listed
    as a NEVER command in docs/ai/specs/safety.md §2.1 because it can silently discard the user's
    own staged work. T1.01 replaced it with Assert-IndexClean, which throws and never touches the
    index.

    This test creates real throwaway git repositories under Pester's $TestDrive (auto-cleaned) —
    not the PineGuard repo itself — and exercises Assert-IndexClean against them directly, so it
    is a genuine regression test rather than a mock of git's behaviour.
#>

BeforeAll {
    . (Join-Path $PSScriptRoot '..' '.shared' 'git.ps1')

    function New-ScratchGitRepo {
        <#
        .SYNOPSIS
            Creates a throwaway git repo with one committed file, for Assert-IndexClean tests.
        #>
        param([Parameter(Mandatory)] [string] $Path)

        New-Item -ItemType Directory -Path $Path -Force | Out-Null

        & git init --quiet $Path
        if ($LASTEXITCODE -ne 0) { throw "git init failed for scratch repo at $Path" }

        & git -C $Path config user.email 'pester@example.invalid'
        & git -C $Path config user.name 'Pester'
        # Never sign scratch commits: the real repo's global/system git config may require GPG,
        # which would make this test depend on machine-local key material it has no business
        # needing.
        & git -C $Path config commit.gpgsign false

        Set-Content -LiteralPath (Join-Path $Path 'file.txt') -Value 'hello'
        & git -C $Path add file.txt
        & git -C $Path commit --quiet -m 'initial commit'
        if ($LASTEXITCODE -ne 0) { throw "git commit failed for scratch repo at $Path" }

        # Leave a further, uncommitted change so the 'staged changes' scenario has something
        # real to stage.
        Add-Content -LiteralPath (Join-Path $Path 'file.txt') -Value 'more'
    }
}

Describe 'Assert-IndexClean' {

    Context 'clean index' {
        BeforeAll {
            $script:CleanRepo = Join-Path $TestDrive 'clean-repo'
            New-ScratchGitRepo -Path $script:CleanRepo
        }

        It 'does not throw, and does not touch the working tree' {
            { Assert-IndexClean -RepoRoot $script:CleanRepo } | Should -Not -Throw

            $status = @(& git -C $script:CleanRepo status --porcelain)
            # The uncommitted (unstaged) modification from New-ScratchGitRepo should still be
            # present and still unstaged: Assert-IndexClean must not have staged, committed, or
            # discarded anything.
            $status | Should -Contain ' M file.txt'
        }
    }

    Context 'staged changes present (F-16 regression)' {
        BeforeAll {
            $script:StagedRepo = Join-Path $TestDrive 'staged-repo'
            New-ScratchGitRepo -Path $script:StagedRepo
            & git -C $script:StagedRepo add file.txt
        }

        It 'throws' {
            { Assert-IndexClean -RepoRoot $script:StagedRepo } | Should -Throw
        }

        It 'names the staged file in the exception message' {
            { Assert-IndexClean -RepoRoot $script:StagedRepo } | Should -Throw -ExpectedMessage '*file.txt*'
        }

        It 'leaves the file staged after throwing — it must never run `git restore --staged .` (F-16 is a Tier 0 NEVER command)' {
            try { Assert-IndexClean -RepoRoot $script:StagedRepo } catch { }

            $staged = @(& git -C $script:StagedRepo diff --cached --name-only)
            $staged | Should -Contain 'file.txt'
        }
    }
}
