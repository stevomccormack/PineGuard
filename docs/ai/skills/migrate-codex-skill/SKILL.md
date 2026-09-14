# Skill: Migrate Codex Skills
**ID**: pineguard.skill.migrate-codex-skill
**Version**: 1.0

## 1. Context & Goal
OpenAI Codex's desktop app imports Claude Code sessions and, as a side effect, migrates every
`.claude/commands/<name>.md` into `.agents/skills/source-command-<name>/SKILL.md`. The
`source-command-` prefix is a vendor artefact with no meaning in this repository
(`docs/ai/meta/taxonomy.md` §N.2/§N.3), and those folders are gitignored. This skill completes the
migration on the repository's terms: strip the prefix, make each `SKILL.md`'s `name:` match its
directory, give it a real description, and commit only the folders that were promoted.

## 2. Inputs
- None required. The script finds every prefixed folder itself.
- Optional: `-WhatIf` (preview), `-NoCommit`, `-Force`, `-Message`, `-Exclude`.

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> - Never commit a `source-command-*` folder as-is. The prefix is banned and the folders are
>   gitignored for that reason.
> - Never overwrite an existing unprefixed skill without `-Force`, and never pass `-Force` without
>   the owner's explicit instruction. `ask-council` and `scan-sonar` are hand-written Brain skill
>   adapters that collide with the generated names; the script skips them by design.
> - The release family (`github-*`, `nuget-*`) stays excluded. Those commands are Claude Code-only
>   per `docs/ai/meta/adapter-surfaces.md` §4 and must never reach an auto-approving surface.
> - The script stages only the folders it promoted and refuses to run on a dirty index. It never
>   unstages anything (`docs/ai/specs/safety.md` §2.1). If it throws "already has N staged
>   file(s)", commit or unstage that work yourself first.

## 4. Execution Steps

1. **Preview**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1" -WhatIf
   ```

   Read the per-folder actions: `renamed`, `duplicate` (identical copy removed), `skipped`
   (collision), `excluded` (release family).

2. **Promote and commit**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1"
   ```

   One `chore(agents): promote N Codex-migrated command skills to bare names` commit is created,
   containing only `.agents/skills/<name>/` paths.

3. **Report**

   Give the user the summary counts and the commit subject. Name every `skipped` entry: each one
   is an unprefixed skill that already exists with different content and needs a human decision.

4. **"Nothing to do" is success.** It means Codex has not re-imported since the last run, or the
   import sync is off.

## 5. Definition of Done
- [ ] No `source-command-*` folder remains under `.agents/skills/` other than those reported as
      `skipped` or `excluded`
- [ ] Every promoted `SKILL.md` has `name:` equal to its directory name
- [ ] Exactly one `chore(agents)` commit, containing only `.agents/skills/<name>/` paths
- [ ] Skipped collisions reported to the owner, not force-resolved

## 6. Troubleshooting

| Symptom | Cause | Fix |
|---------|-------|-----|
| `Nothing to do: no prefixed skill folders found.` | Codex has not re-imported since the last run, or `[desktop] external-agent-import-sync-enabled` is `false` in `~/.codex/config.toml` | Nothing to fix |
| `The git index already has N staged file(s)` | Pre-existing staged work | Commit or unstage it yourself, then re-run; the script will not unstage for you |
| `<name>: skipped - an unprefixed skill with different content already exists` | A hand-written skill uses that name | Leave it; report it. Only the owner may decide on `-Force` |
| The folders come back after every Codex import | Expected: the import regenerates them, and `.gitignore` hides them | Re-run; identical duplicates are removed silently and produce no commit |

## 7. Examples

| User says | Actions | Result |
|-----------|---------|--------|
| "Codex dumped source-command folders again" | Preview, promote, commit, report | Bare-named skills committed; collisions listed |
| "Remove the source-command prefix" | Same | Same |
| "Preview the Codex skill migration" | `-WhatIf` only | Per-folder action list, nothing changed |

## 8. Reference Material (Deep Dive)
- `tools/codex/README.md` (parameters, behaviour table, how to switch the import off)
- `docs/ai/meta/adapter-surfaces.md` §2.1.1 (what `.agents/skills/` may carry)
- `docs/ai/meta/taxonomy.md` §N.2/§N.3 (why the prefix is banned)
- `docs/ai/specs/safety.md` §2 (scoped staging, clean-index guard)
