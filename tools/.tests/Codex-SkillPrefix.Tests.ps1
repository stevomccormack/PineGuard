<#
.SYNOPSIS
    Behavioural tests for tools/codex/Remove-CodexSkillPrefix.ps1.

.DESCRIPTION
    Exercises the script against real throwaway git repositories under Pester's $TestDrive, each
    seeded with the exact SKILL.md shape that OpenAI Codex's session import writes
    (`.agents/skills/source-command-<name>/SKILL.md` with the placeholder frontmatter). Never
    touches the PineGuard repo's own .agents/skills/.

    Covers: the rename and frontmatter rewrite, the default release-family exclusion, refusal to
    overwrite a differing unprefixed skill, removal of an identical prefixed duplicate, the
    scoped commit (only the promoted folders are staged), -NoCommit, -WhatIf, and the
    clean-index guard inherited from Invoke-Commit.
#>

BeforeAll {
    $script:ScriptPath = Join-Path $PSScriptRoot '..' 'codex' 'Remove-CodexSkillPrefix.ps1'

    function New-CodexScratchRepo {
        <#
        .SYNOPSIS
            Creates a throwaway git repo with an empty committed .agents/skills tree.
        #>
        param([Parameter(Mandatory)] [string] $Path)

        New-Item -ItemType Directory -Path $Path -Force | Out-Null

        & git init --quiet $Path
        if ($LASTEXITCODE -ne 0) { throw "git init failed for scratch repo at $Path" }

        & git -C $Path config user.email 'pester@example.invalid'
        & git -C $Path config user.name 'Pester'
        & git -C $Path config commit.gpgsign false

        $skills = Join-Path $Path '.agents' 'skills'
        New-Item -ItemType Directory -Path $skills -Force | Out-Null
        Set-Content -LiteralPath (Join-Path $skills '.gitkeep') -Value ''
        & git -C $Path add -- .agents/skills/.gitkeep
        & git -C $Path commit --quiet -m 'initial commit'
        if ($LASTEXITCODE -ne 0) { throw "git commit failed for scratch repo at $Path" }
    }

    function Add-CodexSkill {
        <#
        .SYNOPSIS
            Writes one SKILL.md in the exact shape Codex's migrated-command-skills template emits.
        #>
        param(
            [Parameter(Mandatory)] [string] $RepoPath,
            [Parameter(Mandatory)] [string] $Name,
            [string] $Role = 'DevOps Engineer'
        )

        $dir = Join-Path $RepoPath '.agents' 'skills' "source-command-$Name"
        New-Item -ItemType Directory -Path $dir -Force | Out-Null
        $lines = @(
            '---',
            "name: ""source-command-$Name""",
            "description: ""Migrated source command ``$Name``""",
            '---',
            '',
            "# source-command-$Name",
            '',
            "Use this skill when the user asks to run the migrated source command ``$Name``.",
            '',
            '## Command Template',
            '',
            "Act as **$Role**. Read and execute ``docs/ai/agents/$Name.md``."
        )
        Set-Content -LiteralPath (Join-Path $dir 'SKILL.md') -Value ($lines -join "`n")
        return $dir
    }

    function Invoke-Script {
        <#
        .SYNOPSIS
            Runs the script under test against a scratch repo and returns its console output as
            one string. Switches are forwarded by name through a hashtable splat: splatting an
            empty remaining-arguments array would pass $null positionally into -SkillsDir.
        #>
        param(
            [Parameter(Mandatory)] [string] $RepoPath,
            [switch] $Force,
            [switch] $NoCommit,
            [switch] $WhatIf,
            [switch] $DryRun
        )
        $splat = @{ RepoRoot = $RepoPath }
        if ($Force.IsPresent) { $splat.Force = $true }
        if ($NoCommit.IsPresent) { $splat.NoCommit = $true }
        if ($WhatIf.IsPresent) { $splat.WhatIf = $true }
        if ($DryRun.IsPresent) { $splat.DryRun = $true }
        & $script:ScriptPath @splat *>&1 | Out-String
    }
}

Describe 'Remove-CodexSkillPrefix.ps1' {

    Context 'a fresh Codex import' {
        BeforeAll {
            $script:Repo = Join-Path $TestDrive 'fresh'
            New-CodexScratchRepo -Path $script:Repo
            Add-CodexSkill -RepoPath $script:Repo -Name 'commit-core' | Out-Null
            Add-CodexSkill -RepoPath $script:Repo -Name 'github-release-publish' | Out-Null
            $script:Output = Invoke-Script -RepoPath $script:Repo
            $script:Target = Join-Path $script:Repo '.agents' 'skills' 'commit-core' 'SKILL.md'
        }

        It 'moves the folder to its bare command name and deletes the prefixed original' {
            Test-Path $script:Target | Should -BeTrue
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'source-command-commit-core') | Should -BeFalse
        }

        It 'rewrites the frontmatter name to match the directory' {
            (Get-Content -Raw $script:Target) | Should -Match '(?m)^name: commit-core$'
        }

        It 'replaces the placeholder description with the command and its playbook' {
            (Get-Content -Raw $script:Target) |
                Should -Match '(?m)^description: "Run the PineGuard /commit-core command: read and execute docs/ai/agents/commit-core\.md\."$'
        }

        It 'leaves no trace of the vendor prefix or the "migrated source command" wording' {
            $text = Get-Content -Raw $script:Target
            $text | Should -Not -Match 'source-command-'
            $text | Should -Not -Match 'migrated source command'
            $text | Should -Match '(?m)^# commit-core$'
            $text | Should -Match 'run the `/commit-core` command'
        }

        It 'keeps the command template body intact' {
            (Get-Content -Raw $script:Target) | Should -Match 'Act as \*\*DevOps Engineer\*\*\. Read and execute `docs/ai/agents/commit-core\.md`\.'
        }

        It 'writes UTF-8 without a byte-order mark' {
            $bytes = [System.IO.File]::ReadAllBytes($script:Target)
            ($bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) | Should -BeFalse
        }

        It 'excludes the release family by default and leaves its prefixed folder in place' {
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'github-release-publish') | Should -BeFalse
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'source-command-github-release-publish' 'SKILL.md') | Should -BeTrue
            $script:Output | Should -Match 'github-release-publish: excluded'
        }

        It 'commits with a chore(agents) subject' {
            $subject = & git -C $script:Repo log -1 --format=%s
            $subject | Should -Match '^chore\(agents\): promote 1 Codex-migrated command skill to bare names$'
        }

        It 'names the promoted skill and the script in the commit body' {
            $body = (& git -C $script:Repo log -1 --format=%b) -join "`n"
            $body | Should -Match 'commit-core'
            $body | Should -Match 'tools/codex/Remove-CodexSkillPrefix\.ps1'
        }

        It 'stages only the promoted folder' {
            $files = @(& git -C $script:Repo show --name-only --format= HEAD)
            $files | Should -Be @('.agents/skills/commit-core/SKILL.md')
        }

        It 'leaves a clean index afterwards' {
            @(& git -C $script:Repo diff --cached --name-only).Count | Should -Be 0
        }
    }

    Context 'an unprefixed skill with different content already exists' {
        BeforeAll {
            $script:Repo = Join-Path $TestDrive 'collision'
            New-CodexScratchRepo -Path $script:Repo
            $curated = Join-Path $script:Repo '.agents' 'skills' 'ask-council'
            New-Item -ItemType Directory -Path $curated -Force | Out-Null
            Set-Content -LiteralPath (Join-Path $curated 'SKILL.md') -Value "---`nname: ask-council`ndescription: hand-written`n---`n"
            & git -C $script:Repo add -- .agents/skills/ask-council
            & git -C $script:Repo commit --quiet -m 'curated skill'
            Add-CodexSkill -RepoPath $script:Repo -Name 'ask-council' -Role 'Architect / Council' | Out-Null
            $script:Before = & git -C $script:Repo rev-parse HEAD
            $script:Output = Invoke-Script -RepoPath $script:Repo
        }

        It 'does not overwrite the curated skill' {
            (Get-Content -Raw (Join-Path $script:Repo '.agents' 'skills' 'ask-council' 'SKILL.md')) | Should -Match 'hand-written'
        }

        It 'skips with a warning and leaves the prefixed folder in place' {
            $script:Output | Should -Match 'ask-council: skipped'
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'source-command-ask-council' 'SKILL.md') | Should -BeTrue
        }

        It 'creates no commit' {
            (& git -C $script:Repo rev-parse HEAD) | Should -Be $script:Before
        }

        It 'overwrites when -Force is given' {
            Invoke-Script -RepoPath $script:Repo -Force | Out-Null
            (Get-Content -Raw (Join-Path $script:Repo '.agents' 'skills' 'ask-council' 'SKILL.md')) | Should -Match '(?m)^name: ask-council$'
            (Get-Content -Raw (Join-Path $script:Repo '.agents' 'skills' 'ask-council' 'SKILL.md')) | Should -Not -Match 'hand-written'
            (& git -C $script:Repo rev-parse HEAD) | Should -Not -Be $script:Before
        }
    }

    Context 're-running after Codex regenerates an identical skill' {
        BeforeAll {
            $script:Repo = Join-Path $TestDrive 'rerun'
            New-CodexScratchRepo -Path $script:Repo
            Add-CodexSkill -RepoPath $script:Repo -Name 'test-core' | Out-Null
            Invoke-Script -RepoPath $script:Repo | Out-Null
            $script:AfterFirst = & git -C $script:Repo rev-parse HEAD
            Add-CodexSkill -RepoPath $script:Repo -Name 'test-core' | Out-Null
            $script:Output = Invoke-Script -RepoPath $script:Repo
        }

        It 'removes the prefixed duplicate' {
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'source-command-test-core') | Should -BeFalse
            $script:Output | Should -Match 'removed the prefixed duplicate'
        }

        It 'creates no second commit' {
            (& git -C $script:Repo rev-parse HEAD) | Should -Be $script:AfterFirst
        }
    }

    Context '-NoCommit' {
        BeforeAll {
            $script:Repo = Join-Path $TestDrive 'nocommit'
            New-CodexScratchRepo -Path $script:Repo
            Add-CodexSkill -RepoPath $script:Repo -Name 'format-all' | Out-Null
            $script:Before = & git -C $script:Repo rev-parse HEAD
            Invoke-Script -RepoPath $script:Repo -NoCommit | Out-Null
        }

        It 'renames but leaves the result uncommitted and unstaged' {
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'format-all' 'SKILL.md') | Should -BeTrue
            (& git -C $script:Repo rev-parse HEAD) | Should -Be $script:Before
            @(& git -C $script:Repo diff --cached --name-only).Count | Should -Be 0
            @(& git -C $script:Repo status --porcelain) | Should -Contain '?? .agents/skills/format-all/'
        }
    }

    Context '-WhatIf' {
        BeforeAll {
            $script:Repo = Join-Path $TestDrive 'whatif'
            New-CodexScratchRepo -Path $script:Repo
            Add-CodexSkill -RepoPath $script:Repo -Name 'scan-sonar' | Out-Null
            $script:Before = & git -C $script:Repo rev-parse HEAD
            $script:Output = Invoke-Script -RepoPath $script:Repo -WhatIf
        }

        It 'reports the rename without performing it' {
            $script:Output | Should -Match '\[WhatIf\] scan-sonar: would be renamed'
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'source-command-scan-sonar' 'SKILL.md') | Should -BeTrue
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'scan-sonar') | Should -BeFalse
            (& git -C $script:Repo rev-parse HEAD) | Should -Be $script:Before
        }

        It 'is also reachable as -DryRun' {
            $out = Invoke-Script -RepoPath $script:Repo -DryRun
            $out | Should -Match '\[WhatIf\] scan-sonar: would be renamed'
        }
    }

    Context 'the index already has staged changes' {
        BeforeAll {
            $script:Repo = Join-Path $TestDrive 'dirty-index'
            New-CodexScratchRepo -Path $script:Repo
            Add-CodexSkill -RepoPath $script:Repo -Name 'clean-log' | Out-Null
            Set-Content -LiteralPath (Join-Path $script:Repo 'wip.txt') -Value 'user work'
            & git -C $script:Repo add -- wip.txt
        }

        It 'refuses before touching any folder, and never unstages the user''s work' {
            { & $script:ScriptPath -RepoRoot $script:Repo *>&1 | Out-Null } | Should -Throw -ExpectedMessage '*staged file(s)*'
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'source-command-clean-log' 'SKILL.md') | Should -BeTrue
            Test-Path (Join-Path $script:Repo '.agents' 'skills' 'clean-log') | Should -BeFalse
            @(& git -C $script:Repo diff --cached --name-only) | Should -Contain 'wip.txt'
        }
    }

    Context 'nothing to do' {
        It 'exits cleanly when no prefixed folder exists' {
            $repo = Join-Path $TestDrive 'empty'
            New-CodexScratchRepo -Path $repo
            $out = Invoke-Script -RepoPath $repo
            $out | Should -Match 'Nothing to do'
        }
    }
}
