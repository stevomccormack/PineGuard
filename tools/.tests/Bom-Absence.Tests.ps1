<#
.SYNOPSIS
    F-47: asserts no tools/** script file has a UTF-8 byte-order mark.

.DESCRIPTION
    The plan (F-47, S3) found 15 files with a BOM across all of tools/** (14 within this test's
    own scope, once tools/audit-cli/** — a separately-owned subsystem — is excluded). §3.4 sets
    "UTF-8 without BOM" as the target encoding, and T2.06 is the task that mechanically strips
    BOMs and trailing whitespace repo-wide under tools/.

    Whether this test currently passes or fails depends entirely on whether T2.06 has landed on
    this branch yet — check tools/.tests/README.md (written at the same time as this suite) for
    what was actually true when this suite was authored and first run.

    NOTE (Pester v5 discovery vs. run): everything below is computed twice on purpose — once as
    plain top-level script code (Discovery phase, so -ForEach has data to build the test tree
    from) and again inside BeforeAll (Run phase, so It bodies see it). Top-level `function`/
    `$script:` statements that sit outside BeforeAll do NOT carry over into the Run phase in
    Pester 5 — only a -ForEach item's own injected variables do — so a helper function defined
    only at the top of the file works during discovery but throws CommandNotFoundException once
    an It body actually calls it.
#>

# Discovery-time: needed so -ForEach has data to build the test tree from.
. (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
$script:RepoRootBom = Get-RepoRoot -StartDirectory $PSScriptRoot
$script:BomCandidates = @(
    Get-ChildItem -Path (Join-Path $script:RepoRootBom 'tools') -Recurse -File -Include '*.ps1', '*.psm1', '*.psd1' |
        Where-Object { $_.FullName -notmatch '[\\/]audit-cli[\\/]' } |
        ForEach-Object {
            @{
                RelativePath = $_.FullName.Substring($script:RepoRootBom.Length).TrimStart('\', '/')
                FullName     = $_.FullName
            }
        }
)

Describe 'UTF-8 BOM absence (F-47 / T2.06)' {

    BeforeAll {
        # Run-time: re-derive the same data and helper function for the It bodies below.
        . (Join-Path $PSScriptRoot '..' '.shared' 'path.ps1')
        $script:RepoRootBomRun = Get-RepoRoot -StartDirectory $PSScriptRoot
        $script:BomCandidatesRun = @(
            Get-ChildItem -Path (Join-Path $script:RepoRootBomRun 'tools') -Recurse -File -Include '*.ps1', '*.psm1', '*.psd1' |
                Where-Object { $_.FullName -notmatch '[\\/]audit-cli[\\/]' }
        )

        function Test-Utf8Bom {
            <#
            .SYNOPSIS
                Returns $true if the file at -Path starts with the UTF-8 BOM (EF BB BF).
            #>
            param([Parameter(Mandatory)] [string] $Path)

            $bytes = [System.IO.File]::ReadAllBytes($Path)
            return ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF)
        }
    }

    It 'the file scan finds at least one candidate file (sanity check)' {
        $script:BomCandidatesRun.Count | Should -BeGreaterThan 0
    }

    It '<RelativePath> is UTF-8 without a byte-order mark' -ForEach $script:BomCandidates {
        Test-Utf8Bom -Path $FullName |
            Should -BeFalse -Because "F-47: $RelativePath has a UTF-8 BOM; T2.06 strips these repo-wide under tools/"
    }
}
