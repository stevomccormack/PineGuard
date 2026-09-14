# Codex

Scripts that reconcile what OpenAI Codex writes into this repository with the repository's own
conventions.

## Directory Structure

```
tools/codex/
└── Remove-CodexSkillPrefix.ps1   # Strip `source-command-` from Codex-generated .agents/skills/ folders and commit them
```

## Why this exists

The Codex desktop app imports Claude Code sessions (`[desktop] external-agent-import-sync-enabled`
in the user's `~/.codex/config.toml`). Every import also migrates each `.claude/commands/<name>.md`
into `.agents/skills/source-command-<name>/SKILL.md`, using a placeholder template: the frontmatter
`name` carries the prefix, the description is literally `Migrated source command `<name>``, and
the body is the command file's one-line pointer at its Brain playbook.

The prefix is a vendor implementation detail with no meaning here (`docs/ai/meta/taxonomy.md`
§N.2/§N.3 bans exactly this kind of leading segment), so `.gitignore` hides every
`.agents/skills/source-command-*/` folder. `Remove-CodexSkillPrefix.ps1` finishes the migration
on the repository's terms so the skills can be committed under their bare command names.

## Usage

Run from the repository root.

```powershell
# Preview: list every prefixed folder with the action a real run would take
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1" -WhatIf

# Promote every prefixed folder and commit the result in one chore(agents) commit
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1"

# Promote only; leave the folders unstaged for a hand-written commit
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1" -NoCommit
```

## Parameters (Remove-CodexSkillPrefix.ps1)

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-RepoRoot` | string | resolved from the script's location | Repository to operate on. The Pester suite passes a scratch repo here. |
| `-SkillsDir` | string | `.agents/skills` | Skills directory, relative to `-RepoRoot`, that Codex writes into. |
| `-Prefix` | string | `source-command-` | The vendor prefix to strip. |
| `-Exclude` | string[] | `github-*`, `nuget-*` | Bare command names matching any pattern are never promoted. The default is the release family, which is Claude Code-only (`docs/ai/meta/adapter-surfaces.md` §4). |
| `-Force` | switch | `$false` | Overwrite an existing unprefixed skill whose `SKILL.md` differs. Without it such a candidate is skipped and reported. |
| `-NoCommit` | switch | `$false` | Rename and rewrite only; leave the results unstaged. |
| `-Message` | string | generated | An explicit commit message. The generated one is a `chore(agents): ...` subject plus a prose body naming every promoted skill. |
| `-WhatIf` | switch | `$false` | Report without touching the filesystem or git. `-DryRun` is an alias. |

## Behaviour

For each `.agents/skills/<prefix><name>/` folder, in name order:

1. **Excluded** — `<name>` matches `-Exclude`. Reported; the folder is left in place (still gitignored).
2. **Duplicate** — `.agents/skills/<name>/SKILL.md` already exists and is byte-identical to what
   the rewrite would produce. The prefixed copy is deleted; nothing is committed. This is what a
   re-run after another Codex import looks like when nothing changed.
3. **Skipped** — an unprefixed `SKILL.md` exists with different content and `-Force` was not
   given. Reported; nothing is touched. Today this covers `ask-council` and `scan-sonar`, whose
   unprefixed folders are hand-written Brain skill adapters.
4. **Renamed** (or **Overwritten** with `-Force`) — `SKILL.md` is rewritten and written to the bare
   folder as UTF-8 without BOM, sibling files are copied verbatim, and the prefixed folder is
   deleted.

The rewrite changes only what the prefix contaminated: the frontmatter `name` becomes the bare
directory name (the Agent Skills spec requires the two to match), the placeholder description
becomes `Run the PineGuard /<name> command: read and execute docs/ai/agents/<name>.md.`, the
heading and the "migrated source command" sentence lose the prefix, and any other occurrence of
`<prefix><name>` is replaced. The command template body is kept as-is.

Committing stages exactly the promoted folders through `tools/.shared/git.ps1`'s `Invoke-Commit`
(never `git add -A`). The script refuses to start if the index already has staged changes, and
never unstages anything (`docs/ai/specs/safety.md` §2.1).

## Turning the regeneration off

The folders reappear after every Codex import. Setting
`external-agent-import-sync-enabled = false` under `[desktop]` in `~/.codex/config.toml` stops the
import (and the transcript sync that comes with it); this script then reports "Nothing to do".

## Tests

`tools/.tests/Codex-SkillPrefix.Tests.ps1` runs the script against throwaway git repositories
under Pester's `$TestDrive`, seeded with the exact template Codex emits. Run via
`tools/testing/Test-Tools.ps1` or `Invoke-Pester -Path tools/.tests/Codex-SkillPrefix.Tests.ps1`.
