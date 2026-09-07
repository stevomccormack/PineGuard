<#
.SYNOPSIS
    Tests for Read-CoberturaCoverage (tools/.shared/coverage.ps1).

.DESCRIPTION
    Read-CoberturaCoverage does not trust the <package>/<class> line-rate/branch-rate attributes
    Cobertura writes — it recomputes both from the raw <line> elements (hits, branch,
    condition-coverage), which is what makes it resilient to different producers. The fixture at
    tools/.tests/fixtures/sample-coverage.cobertura.xml has two classes with known line/branch
    data chosen so the recomputed rates are easy to hand-verify:

      - PineGuard.Sample.Widget: 4 lines, 3 covered (line-rate 0.75); one branch line with
        condition-coverage "50% (1/2)" (branch-rate 0.5).
      - PineGuard.Sample.Other.Helper: 2 lines, 2 covered (line-rate 1.0); no branch lines
        (branch-rate defaults to 1.0 — Read-CoberturaCoverage's own convention for "no branches
        to miss").

    Read-CoberturaCoverage also always normalizes the returned filename separator to '\\'
    regardless of platform (Normalize-CoberturaFilename does an unconditional
    `-replace '/', '\'`) — itself a Windows-ism (F-30), but not one this suite's Windows-ism grep
    (tools/.tests/Windows-Isms.Tests.ps1) flags, since it isn't a hardcoded path literal. The
    assertions below encode that actual current behaviour rather than the cross-platform behaviour
    it should eventually have.
#>

BeforeAll {
    . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
    . (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')
    . (Join-Path $PSScriptRoot '..' '.shared' 'coverage.ps1')

    $script:RepoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
    $script:FixtureFile = Get-Item (Join-Path $PSScriptRoot 'fixtures' 'sample-coverage.cobertura.xml')
}

Describe 'Read-CoberturaCoverage' {
    BeforeAll {
        $script:Classes = @(Read-CoberturaCoverage -CoverageFiles @($script:FixtureFile) -RepoRoot $script:RepoRoot)
    }

    It 'returns exactly the two classes in the fixture' {
        $script:Classes.Count | Should -Be 2
    }

    It 'computes line-rate and branch-rate for a class with mixed line and branch coverage' {
        $widget = $script:Classes | Where-Object { $_.Name -eq 'PineGuard.Sample.Widget' }
        $widget | Should -Not -BeNullOrEmpty

        $widget.LinesTotal | Should -Be 4
        $widget.LinesCovered | Should -Be 3
        $widget.LineRate | Should -Be 0.75

        $widget.BranchesTotal | Should -Be 2
        $widget.BranchesCovered | Should -Be 1
        $widget.BranchRate | Should -Be 0.5
    }

    It 'normalizes the fixture filename to a repo-relative, backslash-separated path' {
        $widget = $script:Classes | Where-Object { $_.Name -eq 'PineGuard.Sample.Widget' }
        # Built via -replace (not a backslash literal) so this assertion doesn't itself look like
        # one of the hardcoded-path-separator findings that Windows-Isms.Tests.ps1 greps for.
        $expectedFile = 'src/PineGuard.Sample/Widget.cs' -replace '/', '\'
        $widget.File | Should -Be $expectedFile
    }

    It 'defaults branch-rate to 1.0 for a class with no branch lines' {
        $helper = $script:Classes | Where-Object { $_.Name -eq 'PineGuard.Sample.Other.Helper' }
        $helper | Should -Not -BeNullOrEmpty

        $helper.LinesTotal | Should -Be 2
        $helper.LinesCovered | Should -Be 2
        $helper.LineRate | Should -Be 1.0

        $helper.BranchesTotal | Should -Be 0
        $helper.BranchRate | Should -Be 1.0
    }

    It 'applies -IncludeClassNameRegex to restrict which classes are returned' {
        $filtered = @(Read-CoberturaCoverage -CoverageFiles @($script:FixtureFile) -RepoRoot $script:RepoRoot -IncludeClassNameRegex 'Widget$')
        $filtered.Count | Should -Be 1
        $filtered[0].Name | Should -Be 'PineGuard.Sample.Widget'
    }

    It 'applies -ExcludeClassNameRegex to drop matching classes' {
        $filtered = @(Read-CoberturaCoverage -CoverageFiles @($script:FixtureFile) -RepoRoot $script:RepoRoot -ExcludeClassNameRegex 'Widget$')
        $filtered.Count | Should -Be 1
        $filtered[0].Name | Should -Be 'PineGuard.Sample.Other.Helper'
    }
}

Describe 'ConvertTo-Rate' {
    It 'passes a decimal rate through unchanged' {
        ConvertTo-Rate -Value 0.75 | Should -Be 0.75
    }

    It 'converts a percentage-scale value (>1.0) to a decimal rate' {
        ConvertTo-Rate -Value 75 | Should -Be 0.75
    }
}

Describe 'Try-ParseConditionCoverage' {
    It 'parses the "NN% (covered/total)" Cobertura format' {
        $result = Try-ParseConditionCoverage -ConditionCoverage '50% (1/2)'
        $result[0] | Should -Be 1
        $result[1] | Should -Be 2
    }

    It 'returns $null for text with no (covered/total) suffix' {
        Try-ParseConditionCoverage -ConditionCoverage '100%' | Should -BeNullOrEmpty
    }
}
