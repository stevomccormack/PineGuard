<!-- metadata_header
type: command
id: cmd-document
version: 1.0
-->

# Command: Document

Generates gold-standard XML documentation comments for the public surface of one project.

## Intent Mapping

| Command | Project | Agent |
|---------|---------|-------|
| `/document-all` | Every project | `docs/ai/agents/document-all.md` |
| `/document-core` | PineGuard.Core | `docs/ai/agents/document-core.md` |
| `/document-must` | PineGuard.MustClauses | `docs/ai/agents/document-must.md` |
| `/document-guard` | PineGuard.GuardClauses | `docs/ai/agents/document-guard.md` |
| `/document-fluent` | PineGuard.FluentValidation | `docs/ai/agents/document-fluent.md` |
| `/document-annotation` | PineGuard.DataAnnotations | `docs/ai/agents/document-annotation.md` |

**Canonical procedure**: [`../skills/document/SKILL.md`](../skills/document/SKILL.md)

These commands write source files, so they are **not** auto-approved.
