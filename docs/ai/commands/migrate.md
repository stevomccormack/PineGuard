<!-- metadata_header
type: command
id: cmd-migrate
version: 1.0
-->

# Command: Migrate

Completes a migration that an external tool started, on this repository's naming terms.

## Intent Mapping

| Command | Intent | Agent |
|---------|--------|-------|
| `/migrate-codex-skill` | Strip the `source-command-` prefix from the `.agents/skills/` folders that OpenAI Codex's session import generated, make each `SKILL.md` name match its directory, and commit only those folders | `docs/ai/agents/migrate-codex-skill.md` |

**Shared recipe**: `docs/ai/skills/migrate-codex-skill/SKILL.md`.

The command commits, so it is Tier 2 only under protocol: scoped staging of the promoted folders,
a clean-index guard, and no `-Force` without the owner's instruction
([`../specs/safety.md`](../specs/safety.md) §2.3). Preview with `-WhatIf` when unsure what Codex
has generated.
