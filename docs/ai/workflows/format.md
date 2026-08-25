<!-- metadata_header
type: workflow
id: workflow-format
version: 1.1
-->

# Workflow: Format

> [!NOTE]
> Enforces `.editorconfig` formatting rules via `dotnet format` for the requested scope.

## Context

- **Role**: [Software Engineer](../roles/builder.md)
- **Skill**: [Format Code](../skills/format-code/SKILL.md) — the canonical procedure
- **Reference**: `tools/code-formatter/Run-Format.ps1`

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing, All)

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow formatting.
- **Cursor**: `cmd: dotnet format` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Execute the canonical procedure** in [`../skills/format-code/SKILL.md`](../skills/format-code/SKILL.md)
   with **Scope = [SCOPE]** — it carries the `Run-Format.ps1` invocation, the scope → project map,
   and the `-VerifyNoChanges` dry-run mode.

2. **Check Results**
   Ensure `dotnet format` exited with code 0 (no violations).
