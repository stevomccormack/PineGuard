<!-- metadata_header
type: workflow
id: workflow-fix-roslyn
version: 1.1
-->

# Workflow: Fix Roslyn

> [!NOTE]
> Fetches Roslyn compiler warnings by scope and fixes them in-place using idiomatic C#.

## Context

- **Role**: [Senior Engineer](../roles/owner.md)
- **Skill**: [Fix Roslyn Warnings](../skills/fix-roslyn/SKILL.md) — the canonical procedure
- **Reference**: `tools/code-diagnostics/Run-CompilerDiagnostics.ps1`
- **Spec**: `docs/ai/specs/tools/code-diagnostics/spec.md`

## Parameters

- **Scope**: (`All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Testing`)
- **Filter**: (optional) Regex pattern to filter warning codes (e.g. `CS86` for nullability)

## Auto-Approval

Not auto-approved on any surface — this workflow writes code. The diagnostics command it uses may
be individually approved, but the repair loop requires explicit user intent.
See [`../commands/fix.md`](../commands/fix.md).

## Steps

1. **Execute the canonical procedure** in [`../skills/fix-roslyn/SKILL.md`](../skills/fix-roslyn/SKILL.md)
   with **Scope = [SCOPE]** (and `-Filter [FILTER]` if provided): run the diagnostics script, then
   fix the warnings one file at a time — idiomatic C# per `docs/ai/specs/coding-standard.md`,
   never suppressing a warning — building after each file.

2. **Report**
   - Total warnings found
   - Warnings fixed (with file, code, line)
   - Warnings skipped (with reason)
