<!-- metadata_header
type: agent
id: agent-migrate-codex-skill
version: 1.0
-->

# Agent: Migrate Codex Skills

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))
> safety tier: **2 with protocol** — stages and commits only the `.agents/skills/<name>/` folders it promotes (never `git add -A`), refuses to run on a dirty index, and deletes only the gitignored `source-command-*` folders that Codex regenerates on its next import ([../specs/safety.md](../specs/safety.md) §2.3). Preview with `-WhatIf` if unsure what Codex has generated.

## Steps

1. Read the recipe at `docs/ai/skills/migrate-codex-skill/SKILL.md`.
2. **Preview**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1" -WhatIf`
3. **Promote and commit**
   - Run: `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/codex/Remove-CodexSkillPrefix.ps1"`
4. **Report** the summary counts and the commit subject. Name every `skipped` entry: it is an
   unprefixed skill that already exists with different content, and only the owner may decide to
   resolve it with `-Force`.

## Related

- [`../skills/migrate-codex-skill/SKILL.md`](../skills/migrate-codex-skill/SKILL.md) — the recipe
- [`../commands/migrate.md`](../commands/migrate.md) — the intent contract
- [`../meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §2.1.1 — what `.agents/skills/` carries
- `tools/codex/README.md` — parameters and behaviour
