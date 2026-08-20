<!-- metadata_header
type: command
id: cmd-audit
version: 1.0
-->

# Command: Audit & Scaffold

Structural checks over the repository, and the one command that builds a feature across every layer.

## Intent Mapping

| Command | Intent | Agent |
|---------|--------|-------|
| `/audit-cli` | Run the audit-cli rule set (including the Rule50 CI gate) | `docs/ai/agents/audit-cli.md` |
| `/audit-gap` | Analyse coverage gaps and propose the missing cases | `docs/ai/agents/audit-gap.md` |
| `/scaffold-vertical-slice` | Implement a feature across Core → Must → Guard → adapters → tests | `docs/ai/agents/scaffold-vertical-slice.md` |

**Shared orchestration**: `docs/ai/workflows/audit.md`

`/audit-cli` and `/audit-gap` are read-only and auto-approved. `/scaffold-vertical-slice` writes
across every project and is not.
