<#
.SYNOPSIS
    Tests for Get-RepoRoot (tools/.shared/path.ps1).

.DESCRIPTION
    Get-RepoRoot walks up from -StartDirectory until it finds PineGuard.slnx. This test
    establishes an independent oracle for "the actual repo root" via `git rev-parse
    --show-toplevel` (rather than re-deriving the answer from the same PineGuard.slnx walk
    Get-RepoRoot itself performs), normalizes the separator so a forward-slash git answer can be
    compared against Get-RepoRoot's native-separator answer, then calls Get-RepoRoot from several
    different starting points and checks they all agree with it.
#>

BeforeAll {
    . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')

    $gitTopLevel = & git -C $PSScriptRoot rev-parse --show-toplevel 2>$null
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($gitTopLevel)) {
        throw 'git rev-parse --show-toplevel failed; cannot establish an independent oracle for the repo root.'
    }

    $script:ExpectedRoot = ($gitTopLevel.Trim() -replace '/', [System.IO.Path]::DirectorySeparatorChar).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}

Describe 'Get-RepoRoot' {

    It 'the independent (git) oracle root actually contains PineGuard.slnx' {
        Test-Path -LiteralPath (Join-Path $script:ExpectedRoot 'PineGuard.slnx') | Should -BeTrue
    }

    It 'resolves correctly starting from this test file''s own directory' {
        (Get-RepoRoot -StartDirectory $PSScriptRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar) |
            Should -Be $script:ExpectedRoot
    }

    It 'resolves correctly starting from a nested fixtures subdirectory' {
        $start = Join-Path $PSScriptRoot 'fixtures'
        (Get-RepoRoot -StartDirectory $start).TrimEnd([System.IO.Path]::DirectorySeparatorChar) |
            Should -Be $script:ExpectedRoot
    }

    It 'resolves correctly starting from a deeply nested src project directory' {
        $start = Join-Path $script:ExpectedRoot 'src' 'PineGuard.Core'
        (Get-RepoRoot -StartDirectory $start).TrimEnd([System.IO.Path]::DirectorySeparatorChar) |
            Should -Be $script:ExpectedRoot
    }

    It 'resolves correctly starting from the repo root itself' {
        (Get-RepoRoot -StartDirectory $script:ExpectedRoot).TrimEnd([System.IO.Path]::DirectorySeparatorChar) |
            Should -Be $script:ExpectedRoot
    }

    It 'throws when started from a location with no PineGuard.slnx anywhere above it' {
        # $TestDrive is Pester's own ephemeral scratch directory, well outside the repo tree.
        { Get-RepoRoot -StartDirectory $TestDrive } | Should -Throw
    }
}
