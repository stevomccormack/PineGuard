<!-- metadata_header
type: command
id: cmd-format
version: 1.0
-->

# Command: Format

Enforces `.editorconfig` via `dotnet format` over the requested scope.

## Intent Mapping

| Command | Scope | Agent |
|---------|-------|-------|
| `/format-all` | `All` | `docs/ai/agents/format-all.md` |
| `/format-core` | `Core` | `docs/ai/agents/format-core.md` |
| `/format-must` | `MustClauses` | `docs/ai/agents/format-must.md` |
| `/format-guard` | `GuardClauses` | `docs/ai/agents/format-guard.md` |
| `/format-fluent` | `FluentValidation` | `docs/ai/agents/format-fluent.md` |
| `/format-annotation` | `DataAnnotations` | `docs/ai/agents/format-annotation.md` |
| `/format-testing` | `Testing` | `docs/ai/agents/format-testing.md` |

**Shared orchestration**: `docs/ai/workflows/format.md`
**Canonical procedure**: [`../skills/format-code/SKILL.md`](../skills/format-code/SKILL.md)

Formatting is whitespace-and-style only and is auto-approved on every surface.
