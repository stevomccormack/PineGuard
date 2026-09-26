<#
.SYNOPSIS
    Exercises ordinary test-project discovery against isolated synthetic project families.
#>

Describe 'Ordinary test-project discovery' {
    BeforeAll {
        . (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')

        function New-DiscoveryFixture {
            param([string] $Root, [string] $Name, [switch] $Empty)
            $directory = Join-Path (Join-Path $Root 'tests') $Name
            New-Item -ItemType Directory -Path $directory -Force | Out-Null
            $project = Join-Path $directory "$Name.csproj"
            '<Project Sdk="Microsoft.NET.Sdk" />' | Set-Content -LiteralPath $project
            if (-not $Empty) {
                'internal static class Fixture { }' | Set-Content -LiteralPath (Join-Path $directory 'Fixture.cs')
            }
            return $project
        }
    }

    BeforeEach {
        $script:FixtureRoot = Join-Path $TestDrive ([Guid]::NewGuid().ToString('N'))
        New-Item -ItemType Directory -Path (Join-Path $script:FixtureRoot 'tests') -Force | Out-Null
    }

    It 'accepts existing unit, grouped unit and integration families and rejects specialized harnesses' {
        $expected = @(
            'PineGuard.Core.UnitTests',
            'PineGuard.Testing.UnitTests',
            'PineGuard.Testing.UnitTests.Rules',
            'PineGuard.Testing.UnitTests.Architecture',
            'PineGuard.Testing.IntegrationTests'
        )
        $excluded = @('PineGuard.Testing.Fuzz', 'PineGuard.Testing.Consumers', 'PineGuard.Benchmarks')
        foreach ($name in @($expected + $excluded)) {
            New-DiscoveryFixture -Root $script:FixtureRoot -Name $name | Out-Null
        }
        $actual = @(Get-TestProjects -RepoRoot $script:FixtureRoot | ForEach-Object { [IO.Path]::GetFileNameWithoutExtension($_) })
        $actual.Count | Should -Be $expected.Count
        foreach ($name in $expected) { $actual | Should -Contain $name }
        foreach ($name in $excluded) { $actual | Should -Not -Contain $name }
    }

    It 'preserves explicit caller filters even for specialized projects' {
        New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Core.UnitTests' | Out-Null
        $special = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.Fuzz'
        $actual = @(Get-TestProjects -RepoRoot $script:FixtureRoot -ProjectFilter '*.Fuzz.csproj')
        $actual.Count | Should -Be 1
        $actual[0] | Should -Be $special
    }

    It 'retains trimmed comma and semicolon filters and returns unique projects' {
        $unit = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Core.UnitTests'
        $integration = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.IntegrationTests'
        $actual = @(Get-TestProjects -RepoRoot $script:FixtureRoot -ProjectFilter ' *.UnitTests.csproj ; *.UnitTests.csproj, *.IntegrationTests.csproj ')
        $actual.Count | Should -Be 2
        $actual | Should -Contain $unit
        $actual | Should -Contain $integration
    }

    It 'honors an explicitly bound legacy unit-test wildcard without expanding its families' {
        $legacy = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Core.UnitTests'
        New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.UnitTests.Rules' | Out-Null
        New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.IntegrationTests' | Out-Null
        $actual = @(Get-TestProjects -RepoRoot $script:FixtureRoot -ProjectFilter '*.UnitTests.csproj')
        $actual.Count | Should -Be 1
        $actual[0] | Should -Be $legacy
    }

    It 'excludes empty projects unless IncludeEmpty is supplied' {
        $runnable = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Core.UnitTests'
        $empty = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.UnitTests.Rules' -Empty
        $actual = @(Get-TestProjects -RepoRoot $script:FixtureRoot)
        $actual.Count | Should -Be 1
        $actual[0] | Should -Be $runnable
        $includingEmpty = @(Get-TestProjects -RepoRoot $script:FixtureRoot -IncludeEmpty)
        $includingEmpty.Count | Should -Be 2
        $includingEmpty | Should -Contain $empty
    }

    It 'fails closed for an empty discovery result or all-empty projects' {
        { Get-TestProjects -RepoRoot $script:FixtureRoot } | Should -Throw
        New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.IntegrationTests' -Empty | Out-Null
        { Get-TestProjects -RepoRoot $script:FixtureRoot } | Should -Throw
        { Get-TestProjects -RepoRoot $script:FixtureRoot -ProjectFilter ' ; , ' } | Should -Throw
    }

    It 'keeps Testing coverage broad while its Run-Tests target remains the helper test project' {
        $testing = Get-PineGuardScope -Name Testing
        $testing.TestCsproj | Should -Be 'tests/PineGuard.Testing.UnitTests/PineGuard.Testing.UnitTests.csproj'
        $testing.DefaultProjectFilter | Should -Be (Get-DefaultTestProjectFilter)
        New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.UnitTests.Rules' | Out-Null
        New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Testing.IntegrationTests' | Out-Null
        @(Get-TestProjects -RepoRoot $script:FixtureRoot -ProjectFilter $testing.DefaultProjectFilter).Count | Should -Be 2
        (Get-PineGuardScope -Name Core).DefaultProjectFilter | Should -Be 'PineGuard.Core.UnitTests.csproj'
    }

    It 'derives the exact dotCover assembly list and excludes only ordinary test families' {
        $filter = Get-DefaultTestAssemblyFilter
        $filter | Should -Be '*.UnitTests,*.UnitTests.*,*.IntegrationTests'
        $patterns = @($filter -split ',')
        foreach ($name in @('PineGuard.Core.UnitTests', 'PineGuard.Testing.UnitTests.Rules',
                'PineGuard.Testing.UnitTests.Architecture', 'PineGuard.Testing.IntegrationTests')) {
            @($patterns | Where-Object { $name -like $_ }).Count | Should -BeGreaterThan 0
        }
        foreach ($name in @('PineGuard.Testing.Fuzz', 'PineGuard.Testing.Consumers',
                'PineGuard.Benchmarks', 'PineGuard.NotIntegrationTests')) {
            @($patterns | Where-Object { $name -like $_ }).Count | Should -Be 0
        }
    }

    It 'handles zero, one and many source files under StrictMode' {
        $empty = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Empty.UnitTests' -Empty
        $one = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.One.UnitTests'
        $many = New-DiscoveryFixture -Root $script:FixtureRoot -Name 'PineGuard.Many.UnitTests'
        'internal static class SecondFixture { }' | Set-Content -LiteralPath (Join-Path (Split-Path -Parent $many) 'Second.cs')
        Test-ProjectHasSources -ProjectPath $empty | Should -BeFalse
        Test-ProjectHasSources -ProjectPath $one | Should -BeTrue
        Test-ProjectHasSources -ProjectPath $many | Should -BeTrue
        foreach ($folder in @('bin', 'obj')) {
            $generated = Join-Path (Split-Path -Parent $empty) $folder
            New-Item -ItemType Directory -Path $generated -Force | Out-Null
            'internal static class GeneratedFixture { }' | Set-Content -LiteralPath (Join-Path $generated 'Generated.cs')
        }
        Test-ProjectHasSources -ProjectPath $empty | Should -BeFalse
    }
}
