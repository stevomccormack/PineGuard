<!-- metadata_header
type: agent
id: agent-rename-codex-skills
version: 1.0
-->

# Agent: Rename Codex Skills

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **2 with protocol** — stages and commits only the `.agents/skills/<name>/` folders it promotes (never `git add -A`), refuses to run on a dirty index, and deletes only the gitignored `source-command-*` folders that Codex regenerates on its next import ([../specs/safety.md](../specs/safety.md) §2.3). Preview with `-WhatIf` if unsure what Codex has generated.

## Arguments

- `--remove-prefix "<prefix>"` maps to the script's `-RemovePrefix`. Default: `source-command-`.
  Pass it explicitly when Codex ever changes the prefix it stamps.

## Steps

1. Read the recipe at `docs/ai/skills/rename-codex-skills/SKILL.md`.
2. **Preview**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Rename-CodexSkills.ps1" -RemovePrefix 'source-command-' -WhatIf`
3. **Rename and commit**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Rename-CodexSkills.ps1" -RemovePrefix 'source-command-'`
4. **Report** the summary counts and the commit subject. Name every `skipped` entry: it is an
   unprefixed skill that already exists with different content, and only the owner may decide to
   resolve it with `-Force`.

## Related

- [`../skills/rename-codex-skills/SKILL.md`](../skills/rename-codex-skills/SKILL.md) — the recipe
- [`../commands/rename.md`](../commands/rename.md) — the intent contract
- [`../meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §2.1.1 — what `.agents/skills/` carries
- `tools/codex/README.md` — parameters and behaviour
