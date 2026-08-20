---
name: scan-roslyn
description: Run Roslyn compiler diagnostics for PineGuard by following the canonical Brain workflow.
---
# Skill: Run Roslyn Diagnostics

## Load First

Read these files before running diagnostics:
1. [tools/code-diagnostics/spec.md](../../../docs/ai/specs/tools/code-diagnostics/spec.md)
2. [roslyn.md](../../../docs/ai/rules/roslyn.md)
3. [scan-roslyn/SKILL.md](../../../docs/ai/skills/scan-roslyn/SKILL.md)
4. [code-reviewer memory](../../../docs/ai/memory/code-reviewer.md)

## Execute

Follow the canonical recipe in [docs/ai/skills/scan-roslyn/SKILL.md](../../../docs/ai/skills/scan-roslyn/SKILL.md) exactly.

## Verify

- Build output is captured.
- Warning summaries are produced.
- Any artifacts go to `artifacts/`.
