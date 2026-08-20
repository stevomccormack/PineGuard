<!-- metadata_header
type: command
id: cmd-clean
version: 1.0
-->

# Command: Clean

Removes generated output. Nothing under version control is ever touched.

## Intent Mapping

| Command | Target | Agent |
|---------|--------|-------|
| `/clean-all` | Both safe zones below | `docs/ai/agents/clean-all.md` |
| `/clean-artifact` | `artifacts/` — coverage results, generated output, analysis data | `docs/ai/agents/clean-artifact.md` |
| `/clean-log` | `logs/` — testing and run logs | `docs/ai/agents/clean-log.md` |

Deletion is destructive. Each agent is Tier 2 only because it is confined to a declared safe zone —
read [`../specs/safety.md`](../specs/safety.md) §7.3 and preview with `-WhatIf` if the scope is unclear.
