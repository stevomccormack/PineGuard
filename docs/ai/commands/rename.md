<!-- metadata_header
type: command
id: cmd-rename
version: 1.0
-->

# Command: Rename

Renames what an external tool generated into this repository's own naming.

## Intent Mapping

| Command | Intent | Agent |
|---------|--------|-------|
| `/rename-codex-skills --remove-prefix "<prefix>"` (default `source-command-`) | Remove the `source-command-` prefix from the `.agents/skills/` folders that OpenAI Codex's session import generated, make each `SKILL.md` name match its directory, and commit only those folders | `docs/ai/agents/rename-codex-skills.md` |

**Shared recipe**: `docs/ai/skills/rename-codex-skills/SKILL.md`.

The command commits, so it is Tier 2 only under protocol: scoped staging of the promoted folders,
a clean-index guard, and no `-Force` without the owner's instruction
([`../specs/safety.md`](../specs/safety.md) §2.3). Preview with `-WhatIf` when unsure what Codex
has generated.
