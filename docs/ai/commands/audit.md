<!-- metadata_header
type: command
id: cmd-audit
version: 1.1
-->

# Command: Audit

Structural checks over the repository — convention compliance, cross-layer mapping, and gap analysis.

## Intent Mapping

| Command | Intent | Agent |
|---------|--------|-------|
| `/audit-cli` | Run the audit-cli rule set (including the Rule50 CI gate) | `docs/ai/agents/audit-cli.md` |
| `/audit-gap` | Analyse coverage gaps and propose the missing cases | `docs/ai/agents/audit-gap.md` |

**Shared orchestration**: `docs/ai/workflows/audit.md` (used by `/audit-cli`; `/audit-gap` runs its
own layer-map procedure).

Both commands are read-only and auto-approved. Building a feature across every layer is contracted
separately in [`scaffold.md`](scaffold.md).
