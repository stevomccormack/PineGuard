<#
.SYNOPSIS
    Strips the `source-command-` prefix that OpenAI Codex's session import stamps on every
    `.agents/skills/` folder it generates, then commits the renamed skills.

.DESCRIPTION
    The Codex desktop app's Claude-session import (`[desktop] external-agent-import-sync-enabled`
    in the user's Codex config) migrates every `.claude/commands/<name>.md` into
    `.agents/skills/source-command-<name>/SKILL.md`, using a placeholder template whose only
    content is the command file's body. The prefix is a vendor implementation detail with no
    meaning in this repository (docs/ai/meta/taxonomy.md §N.2/§N.3), so those folders are
    gitignored and must never be committed as-is.

    This script completes the migration on the repository's terms. For every prefixed folder it:

      1. Derives the bare command name (`source-command-commit-core` -> `commit-core`).
      2. Rewrites SKILL.md so the frontmatter `name:` and the heading match the new directory
         (the Agent Skills spec requires `name` to equal the parent directory), replaces the
         placeholder description with one that names the command and its Brain playbook, and
         removes every remaining occurrence of the prefix.
      3. Moves the folder to `.agents/skills/<name>/` and deletes the prefixed original.
      4. Stages exactly the renamed folders and commits them with a conventional-commits message
         (subject + flowing prose body) through tools/.shared/git.ps1's Invoke-Commit. Nothing
         else in the index or working tree is touched; the script refuses to run if the index
         already has staged changes (never `git restore --staged`, safety.md §2.1).

    An existing, unprefixed skill is never overwritten by default: if the target folder already
    exists and its SKILL.md differs, the candidate is skipped with a warning and the prefixed
    folder is left in place (it stays gitignored). If the target exists and is byte-identical to
    what would be written, the prefixed duplicate is simply removed — re-running after another
    Codex import is therefore a no-op that produces no commit. Pass -Force to overwrite a
    differing target.

    The release family (`github-*`, `nuget-*`) is excluded by default: those commands are Tier 0/1
    operations that must never reach an auto-approving surface such as Codex
    (docs/ai/meta/adapter-surfaces.md §4). Override with -Exclude @() only deliberately.

.PARAMETER RepoRoot
    Repository root to operate on. Defaults to the root resolved from this script's location;
    the Pester suite passes a scratch repository here.

.PARAMETER SkillsDir
    Skills directory, relative to -RepoRoot, that Codex writes into. Defaults to `.agents/skills`.

.PARAMETER Prefix
    The vendor prefix to strip. Defaults to `source-command-`, the literal string Codex's
    `migrated-command-skills` template uses.

.PARAMETER Exclude
    Wildcard patterns, matched against the bare command name, that are never promoted. Defaults
    to the release family (`github-*`, `nuget-*`) per docs/ai/meta/adapter-surfaces.md §4.

.PARAMETER Force
    Overwrite an existing unprefixed skill whose SKILL.md differs from what the rename would
    write. Without it such a candidate is skipped and reported.

.PARAMETER NoCommit
    Rename and rewrite only; leave the results unstaged in the working tree.

.PARAMETER Message
    An explicit commit message. When omitted the script writes its own: a
    `chore(agents): ...` subject and a prose body naming every promoted skill.

.PARAMETER WhatIf
    Report what would be renamed, skipped, excluded or removed without touching the filesystem
    or git. -DryRun is a supported alias of the same switch.

.EXAMPLE
    ./tools/codex/Remove-CodexSkillPrefix.ps1 -WhatIf

    Lists every prefixed folder with the action that a real run would take.

.EXAMPLE
    ./tools/codex/Remove-CodexSkillPrefix.ps1

    Promotes every prefixed folder, then commits the promoted skills in one commit.

.EXAMPLE
    ./tools/codex/Remove-CodexSkillPrefix.ps1 -NoCommit

    Promotes the folders and leaves the changes in the working tree for a hand-written commit.
#>

[CmdletBinding()]
param(
    [string] $RepoRoot,
    [ValidateNotNullOrEmpty()]
    [string] $SkillsDir = '.agents/skills',
    [ValidateNotNullOrEmpty()]
    [string] $Prefix = 'source-command-',
    [string[]] $Exclude = @('github-*', 'nuget-*'),
    [switch] $Force,
    [switch] $NoCommit,
    [string] $Message,

    [Alias('DryRun')]
    [switch] $WhatIf
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

. "$PSScriptRoot/../.shared/path.ps1"
. "$PSScriptRoot/../.shared/git.ps1"
. "$PSScriptRoot/../.shared/console.ps1"

if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $RepoRoot = Get-RepoRoot -StartDirectory $PSScriptRoot
}
$RepoRoot = (Resolve-Path -LiteralPath $RepoRoot).Path

$skillsRoot = Join-Path $RepoRoot $SkillsDir
if (-not (Test-Path -LiteralPath $skillsRoot -PathType Container)) {
    throw ("Skills directory '{0}' does not exist under '{1}'." -f $SkillsDir, $RepoRoot)
}

function ConvertTo-BareSkillContent {
    <#
    .SYNOPSIS
        Rewrites one Codex-generated SKILL.md so every trace of the vendor prefix is gone and the
        frontmatter name matches the bare directory name.
    #>
    param(
        [Parameter(Mandatory)] [string] $Content,
        [Parameter(Mandatory)] [string] $Prefix,
        [Parameter(Mandatory)] [string] $Name
    )

    $prefixed = [regex]::Escape($Prefix + $Name)
    $escapedName = [regex]::Escape($Name)
    $text = $Content -replace "`r`n", "`n"

    # Frontmatter name: must equal the parent directory per the Agent Skills spec. Trailing
    # whitespace is matched with [ \t]* rather than \s* so the newline (and any blank line that
    # follows) is never swallowed into the match.
    $text = [regex]::Replace($text, "(?m)^name:[ \t]*""?$prefixed""?[ \t]*$", "name: $Name")

    # Placeholder description -> one that names the command and, when the body points at a Brain
    # playbook, that playbook. The command template is the only real content Codex carries over.
    $playbook = [regex]::Match($text, "docs/ai/agents/$escapedName\.md")
    $description = if ($playbook.Success) {
        "Run the PineGuard /$Name command: read and execute $($playbook.Value)."
    }
    else {
        "Run the PineGuard /$Name command."
    }
    $placeholder = '(?m)^description:[ \t]*"Migrated source command `' + $escapedName + '`"[ \t]*$'
    $text = [regex]::Replace($text, $placeholder, "description: ""$description""")

    # Body prose that names the prefix or calls the skill a "migrated source command".
    $migratedPhrase = 'the migrated source command `' + $escapedName + '`'
    $text = [regex]::Replace($text, $migratedPhrase, ('the `/' + $Name + '` command'))
    $text = [regex]::Replace($text, "(?m)^# $prefixed[ \t]*$", "# $Name")
    $text = [regex]::Replace($text, $prefixed, $Name)

    if (-not $text.EndsWith("`n")) {
        $text += "`n"
    }
    return $text
}

function Write-Utf8NoBom {
    <#
    .SYNOPSIS
        Writes text as UTF-8 without a byte-order mark (tools/.tests/Bom-Absence.Tests.ps1).
    #>
    param(
        [Parameter(Mandatory)] [string] $Path,
        [Parameter(Mandatory)] [string] $Content
    )
    $encoding = [System.Text.UTF8Encoding]::new($false)
    [System.IO.File]::WriteAllText($Path, $Content, $encoding)
}

function Test-Excluded {
    <#
    .SYNOPSIS
        True when the bare command name matches any -Exclude wildcard pattern.
    #>
    param(
        [Parameter(Mandatory)] [string] $Name,
        [AllowEmptyCollection()] [string[]] $Patterns
    )
    foreach ($pattern in @($Patterns)) {
        if ($Name -like $pattern) { return $true }
    }
    return $false
}

Write-Step ("Scanning {0} for '{1}*' folders" -f $SkillsDir, $Prefix)

$candidates = @(
    Get-ChildItem -LiteralPath $skillsRoot -Directory |
        Where-Object { $_.Name.StartsWith($Prefix, [System.StringComparison]::Ordinal) -and $_.Name.Length -gt $Prefix.Length } |
        Sort-Object Name
)

if ($candidates.Count -eq 0) {
    Write-Success 'Nothing to do: no prefixed skill folders found.'
    return
}

# Fail fast, before any file moves, if a real commit would be refused anyway.
$willCommit = -not ($NoCommit.IsPresent -or $WhatIf.IsPresent)
if ($willCommit) {
    Assert-IndexClean -RepoRoot $RepoRoot
}

$results = [System.Collections.Generic.List[object]]::new()
$stagePaths = [System.Collections.Generic.List[string]]::new()
$separators = [char[]]@('/', [System.IO.Path]::DirectorySeparatorChar)

foreach ($source in $candidates) {
    $name = $source.Name.Substring($Prefix.Length)
    $sourceSkill = Join-Path $source.FullName 'SKILL.md'
    $targetDir = Join-Path $skillsRoot $name
    $targetSkill = Join-Path $targetDir 'SKILL.md'
    $relativeTarget = ($SkillsDir.TrimEnd($separators) + '/' + $name).Replace([System.IO.Path]::DirectorySeparatorChar, '/')

    $record = [pscustomobject]@{ Name = $name; Action = ''; Reason = '' }
    $results.Add($record)

    if (Test-Excluded -Name $name -Patterns $Exclude) {
        $record.Action = 'Excluded'
        $record.Reason = 'matches -Exclude (release family stays Claude Code-only)'
        Write-Warn ("{0}: excluded - {1}" -f $name, $record.Reason)
        continue
    }

    if (-not (Test-Path -LiteralPath $sourceSkill -PathType Leaf)) {
        $record.Action = 'Skipped'
        $record.Reason = 'no SKILL.md in the prefixed folder'
        Write-Warn ("{0}: skipped - {1}" -f $name, $record.Reason)
        continue
    }

    $newContent = ConvertTo-BareSkillContent -Content (Get-Content -LiteralPath $sourceSkill -Raw) -Prefix $Prefix -Name $name

    if (Test-Path -LiteralPath $targetSkill -PathType Leaf) {
        $existing = (Get-Content -LiteralPath $targetSkill -Raw) -replace "`r`n", "`n"
        if ($existing -eq $newContent) {
            $record.Action = 'Duplicate'
            $record.Reason = 'target already identical; prefixed copy removed'
            if ($WhatIf.IsPresent) {
                Write-Detail ("[WhatIf] {0}: would remove the prefixed duplicate of an identical skill" -f $name)
            }
            else {
                Remove-Item -LiteralPath $source.FullName -Recurse -Force
                Write-Detail ("{0}: removed the prefixed duplicate of an identical skill" -f $name)
            }
            continue
        }

        if (-not $Force.IsPresent) {
            $record.Action = 'Skipped'
            $record.Reason = 'an unprefixed skill with different content already exists (use -Force to overwrite)'
            Write-Warn ("{0}: skipped - {1}" -f $name, $record.Reason)
            continue
        }

        $record.Action = 'Overwritten'
        $record.Reason = '-Force replaced the existing unprefixed skill'
    }
    else {
        $record.Action = 'Renamed'
        $record.Reason = ("{0} -> {1}" -f $source.Name, $name)
    }

    if ($WhatIf.IsPresent) {
        Write-Detail ("[WhatIf] {0}: would be {1} ({2})" -f $name, $record.Action.ToLowerInvariant(), $record.Reason)
        $stagePaths.Add($relativeTarget)
        continue
    }

    New-Item -ItemType Directory -Force -Path $targetDir | Out-Null
    Write-Utf8NoBom -Path $targetSkill -Content $newContent

    # Codex only ever writes SKILL.md, but carry any sibling files across verbatim rather than
    # silently dropping them.
    Get-ChildItem -LiteralPath $source.FullName -Recurse -File |
        Where-Object { $_.FullName -ne $sourceSkill } |
        ForEach-Object {
            $relative = $_.FullName.Substring($source.FullName.Length).TrimStart($separators)
            $destination = Join-Path $targetDir $relative
            New-Item -ItemType Directory -Force -Path (Split-Path -Parent $destination) | Out-Null
            Copy-Item -LiteralPath $_.FullName -Destination $destination -Force
        }

    Remove-Item -LiteralPath $source.FullName -Recurse -Force
    $stagePaths.Add($relativeTarget)
    Write-Success ("{0}: {1} ({2})" -f $name, $record.Action.ToLowerInvariant(), $record.Reason)
}

Write-Step 'Summary'
$results | Group-Object Action | Sort-Object Name | ForEach-Object {
    Write-Detail ("{0,-12} {1}" -f ($_.Name + ':'), $_.Count)
}

$promoted = @($results | Where-Object { $_.Action -in @('Renamed', 'Overwritten') })
if ($promoted.Count -eq 0) {
    Write-Success 'No skills to commit.'
    return
}

if ($WhatIf.IsPresent) {
    Write-Detail ("[WhatIf] Would stage: {0}" -f ($stagePaths -join ', '))
    Write-Detail '[WhatIf] Would commit them as one chore(agents) commit.'
    return
}

if ($NoCommit.IsPresent) {
    Write-Warn ("-NoCommit: {0} promoted skill folder(s) left unstaged under {1}." -f $promoted.Count, $SkillsDir)
    return
}

if ([string]::IsNullOrWhiteSpace($Message)) {
    $names = @($promoted | ForEach-Object { $_.Name }) -join ', '
    $noun = if ($promoted.Count -eq 1) { 'skill' } else { 'skills' }
    $subject = 'chore(agents): promote {0} Codex-migrated command {1} to bare names' -f $promoted.Count, $noun
    $body = ('Renames the {0}* folders that the OpenAI Codex session import generated under {1} to ' +
        'their bare command names ({2}), rewriting each SKILL.md so its frontmatter name and heading ' +
        'match the directory as the Agent Skills spec requires and its description names the command ' +
        'and its Brain playbook instead of the placeholder text. Produced by ' +
        'tools/codex/Remove-CodexSkillPrefix.ps1.') -f $Prefix, $SkillsDir, $names
    $Message = ($subject, '', $body) -join "`n"
}

Write-Step ("Committing {0} promoted skill folder(s)" -f $promoted.Count)
Invoke-Commit -RepoRoot $RepoRoot -Title 'Agent: codex skills' -StagePaths $stagePaths.ToArray() -Message $Message
Write-Success 'Committed.'
