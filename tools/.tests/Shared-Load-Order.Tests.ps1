<#
.SYNOPSIS
    D-2's condition for keeping tools/.shared/ dot-sourced instead of a script module: every
    tools/.shared/*.ps1 file must be dot-sourceable standalone, in plain filename-alphabetical
    order, with no unenforced "must load X first" comment.

.DESCRIPTION
    T3.01 consolidated tools/.shared/ (F-06, F-07, F-10) without converting it to a
    PineGuard.Tools script module (D-2 reversed the plan's original default). D-2's own condition
    for keeping the dot-source pattern is this test: no file may secretly depend on another
    .shared/*.ps1 file having been loaded first without declaring that dependency itself. A file
    that needs another file's functions must dot-source it at its own top (self-contained) — e.g.
    tools/.shared/coverage.ps1 dot-sources dotnet-projects.ps1 itself for Get-PineGuardScope, and
    tools/.shared/secret.ps1 dot-sources both dotenv.ps1 and path.ps1 itself.

    Each candidate file is dot-sourced in its own fresh `pwsh` child process, with nothing else
    pre-loaded, and in isolation from every other candidate (not just in alphabetical order) — a
    hidden dependency on file load order would otherwise hide behind whichever order this suite
    happens to enumerate files in. A file that dot-sources its own prerequisites (the pattern
    above) still passes, because dot-sourcing it pulls those prerequisites in as part of loading
    that one file.

    NOTE (Pester v5 discovery vs. run): computed twice on purpose, once as top-level script code
    (Discovery phase, so -ForEach has data) and again inside BeforeAll (Run phase, so It bodies see
    it) — see Bom-Absence.Tests.ps1 for the same pattern and its rationale.
#>

# Discovery-time: needed so -ForEach has data to build the test tree from.
. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
$script:RepoRootSLO = Get-RepoRoot -StartDirectory $PSScriptRoot
$script:SharedDirSLO = Join-Path $script:RepoRootSLO 'tools' '.shared'
$script:SharedFilesSLO = @(
    Get-ChildItem -LiteralPath $script:SharedDirSLO -Filter '*.ps1' -File |
        Sort-Object Name |
        ForEach-Object {
            @{
                Name = $_.Name
                FullName = $_.FullName
            }
        }
)

Describe 'tools/.shared load-order independence (D-2)' {

    BeforeAll {
        # Run-time: re-derive the same data for the It bodies below.
        . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
        $script:RepoRootSLORun = Get-RepoRoot -StartDirectory $PSScriptRoot
        $script:SharedDirSLORun = Join-Path $script:RepoRootSLORun 'tools' '.shared'
        $script:SharedFilesSLORun = @(
            Get-ChildItem -LiteralPath $script:SharedDirSLORun -Filter '*.ps1' -File | Sort-Object Name
        )

        function Test-StandaloneDotSource {
            <#
            .SYNOPSIS
                Dot-sources -Path in a fresh pwsh child process; returns @{ Success; Output }.
            .DESCRIPTION
                A fresh process guarantees nothing from this test file, Pester, or any other
                .shared/*.ps1 file is already loaded into scope — the only thing available to
                -Path is whatever it dot-sources itself.
            #>
            param([Parameter(Mandatory)] [string] $Path)

            $command = "`$ErrorActionPreference = 'Stop'; . '$Path'"
            $output = & pwsh -NoProfile -NonInteractive -Command $command 2>&1
            return @{
                Success = ($LASTEXITCODE -eq 0)
                Output = ($output | Out-String)
            }
        }
    }

    It 'the .shared directory scan finds at least one file (sanity check)' {
        $script:SharedFilesSLORun.Count | Should -BeGreaterThan 0
    }

    It '<Name> dot-sources standalone in a fresh session with no other .shared/*.ps1 file pre-loaded' -ForEach $script:SharedFilesSLO {
        $result = Test-StandaloneDotSource -Path $FullName
        $result.Success | Should -BeTrue -Because "dot-sourcing $Name on its own (nothing else pre-loaded) failed — it has an undeclared dependency on another .shared/*.ps1 file having been loaded first; declare it via a self-contained dot-source instead. Output: $($result.Output)"
    }
}
