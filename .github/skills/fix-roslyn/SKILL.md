---
name: roslyn-fix
description: Fix Roslyn compiler diagnostics for PineGuard by following the canonical Brain workflow.
---
# Skill: Fix Roslyn Diagnostics

## Load First

Read these files before editing code:
1. [tools/code-diagnostics/spec.md](../../../docs/ai/specs/tools/code-diagnostics/spec.md)
2. [code-diagnostics.md](../../../docs/ai/rules/roslyn.md)
3. [roslyn-fix/SKILL.md](../../../docs/ai/skills/fix-roslyn/SKILL.md)
4. [validation-builder memory](../../../docs/ai/memory/validation-builder.md)
5. [code-reviewer memory](../../../docs/ai/memory/code-reviewer.md)

## Execute

Follow the canonical recipe in [docs/ai/skills/fix-roslyn/SKILL.md](../../../docs/ai/skills/fix-roslyn/SKILL.md) exactly.

## Verify

- The targeted warnings are fixed.
- Build remains green.
- The Brain remains the source of truth.
