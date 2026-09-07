<#
.SYNOPSIS
    Parity test (plan §3.4): registry <-> src/*.csproj <-> tests/*.csproj <-> PineGuard.slnx <->
    packable list <-> Qodana config presence <-> per-project AGENTS.md.

.DESCRIPTION
    tools/.shared/dotnet-projects.ps1's Get-PineGuardScope registry is meant to be the single
    place a PineGuard scope is spelled out (plan §3.4). Every other list that enumerates scopes
    by hand — PineGuard.slnx, the Qodana config folder, tools/release/Run-NugetUnlist.ps1's
    packable -Package default, and each project's own AGENTS.md — must agree with it. This test
    walks every registry entry and checks each of those other sources for a match, so a future
    drift (a new scope added to the registry but forgotten in one of the hand-maintained lists)
    shows up here instead of silently shipping.

    T1.05 (commit history on this branch) already added the previously-missing MediatR Qodana
    config and PineGuard.MediatR.Qodana.slnx, so the Qodana-config-presence check is expected to
    pass today. The AGENTS.md check is a target-state assertion, not a guess: it was written after
    confirming (via `Get-ChildItem src/PineGuard.*/AGENTS.md`) that every scope currently has one.

    NOTE (Pester v5 discovery vs. run): the registry scope list is computed twice on purpose —
    once as top-level script code (Discovery phase, so -ForEach has data to build the test tree
    from) and again inside BeforeAll (Run phase, so It bodies that read $script:RepoRoot /
    $script:SlnxContent / $script:PackableList directly, rather than via -ForEach injection,
    actually see them) — top-level `$script:` statements outside BeforeAll do not carry over into
    the Run phase in Pester 5. Get-ScopeTestCases returns a FRESH set of hashtables on every call
    (rather than one array shared across all six Context blocks below) since it is called
    separately per Context.
#>

. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
. (Join-Path $PSScriptRoot '..' '.shared' 'dotnet-projects.ps1')

function Get-ScopeTestCases {
    return @(
        Get-PineGuardScope -All | ForEach-Object {
            @{
                Name          = $_.Name
                SourceCsprojs = $_.SourceCsprojs
                TestCsproj    = $_.TestCsproj
                QodanaConfig  = $_.QodanaConfig
                SourceDir     = $_.SourceDir
            }
        }
    )
}

Describe 'Registry parity (tools/.shared/dotnet-projects.ps1 vs. disk and other lists)' {

    BeforeAll {
        . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
        $script:RepoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
        $script:SlnxContent = Get-Content -Raw (Join-Path $script:RepoRoot 'PineGuard.slnx')

        $nugetUnlistPath = Join-Path $script:RepoRoot 'tools' 'release' 'Run-NugetUnlist.ps1'
        $nugetUnlistContent = Get-Content -Raw $nugetUnlistPath
        $script:PackableList = @()
        if ($nugetUnlistContent -match '(?s)\$Package\s*=\s*@\((.*?)\)') {
            $script:PackableList = @(
                [regex]::Matches($Matches[1], "'([^']+)'") | ForEach-Object { $_.Groups[1].Value }
            )
        }
    }

    Context 'Every registry SourceCsproj exists on disk' {
        It 'SourceCsprojs for <Name> all exist' -ForEach (Get-ScopeTestCases) {
            foreach ($csproj in $SourceCsprojs) {
                $full = Join-Path $script:RepoRoot $csproj
                Test-Path -LiteralPath $full |
                    Should -BeTrue -Because "registry SourceCsprojs entry '$csproj' for scope '$Name' should exist on disk"
            }
        }
    }

    Context 'Every registry TestCsproj exists on disk' {
        It 'TestCsproj for <Name> exists' -ForEach (Get-ScopeTestCases) {
            $full = Join-Path $script:RepoRoot $TestCsproj
            Test-Path -LiteralPath $full |
                Should -BeTrue -Because "registry TestCsproj '$TestCsproj' for scope '$Name' should exist on disk"
        }
    }

    Context 'Every registry QodanaConfig exists on disk (F-20, fixed by T1.05)' {
        It 'QodanaConfig for <Name> exists' -ForEach (Get-ScopeTestCases) {
            $full = Join-Path $script:RepoRoot $QodanaConfig
            Test-Path -LiteralPath $full |
                Should -BeTrue -Because "registry QodanaConfig '$QodanaConfig' for scope '$Name' should exist on disk"
        }
    }

    Context 'PineGuard.slnx lists every registry project' {
        It 'PineGuard.slnx references every SourceCsproj for <Name>' -ForEach (Get-ScopeTestCases) {
            foreach ($csproj in $SourceCsprojs) {
                $slnxStyle = $csproj -replace '\\', '/'
                $script:SlnxContent |
                    Should -Match ([regex]::Escape($slnxStyle)) -Because "PineGuard.slnx should list '$slnxStyle' for scope '$Name'"
            }
        }

        It 'PineGuard.slnx references the TestCsproj for <Name>' -ForEach (Get-ScopeTestCases) {
            $slnxStyle = $TestCsproj -replace '\\', '/'
            $script:SlnxContent |
                Should -Match ([regex]::Escape($slnxStyle)) -Because "PineGuard.slnx should list '$slnxStyle' for scope '$Name'"
        }
    }

    Context 'Packable list (tools/release/Run-NugetUnlist.ps1 -Package) matches the registry' {
        It 'the packable-project default list is non-empty (sanity check on the extraction regex)' {
            $script:PackableList.Count | Should -BeGreaterThan 0 -Because 'Run-NugetUnlist.ps1 should declare a non-empty default -Package array'
        }

        It '<Name>''s package name appears in Run-NugetUnlist.ps1''s -Package default' -ForEach (Get-ScopeTestCases) {
            # The package name is the leaf of the registry's own SourceDir (e.g.
            # 'src\PineGuard.Extensions.Options' -> 'PineGuard.Extensions.Options'), which is
            # exactly the convention every packable PineGuard project follows.
            $packageName = Split-Path $SourceDir -Leaf
            $script:PackableList |
                Should -Contain $packageName -Because "scope '$Name' (package '$packageName') should be in Run-NugetUnlist.ps1's default -Package list (F-21)"
        }
    }

    Context 'Per-project AGENTS.md (F-52)' {
        It 'AGENTS.md exists for <Name>''s source directory' -ForEach (Get-ScopeTestCases) {
            $agentsPath = Join-Path $script:RepoRoot $SourceDir 'AGENTS.md'
            Test-Path -LiteralPath $agentsPath |
                Should -BeTrue -Because "src/.../AGENTS.md should exist for scope '$Name' per the plan's parity requirement (§3.4)"
        }
    }
}
