---
name: fix-sonar
description: Fix SonarQube findings for PineGuard by following the canonical Brain workflow.
---
# Skill: Fix SonarQube Findings

## Load First

Read these files before editing code:
1. [scan/spec.md](../../../docs/ai/specs/scan/spec.md)
2. [scan.md](../../../docs/ai/rules/scan.md)
3. [fix-sonar/SKILL.md](../../../docs/ai/skills/fix-sonar/SKILL.md)
4. [validation-builder memory](../../../docs/ai/memory/validation-builder.md)
5. [code-reviewer memory](../../../docs/ai/memory/code-reviewer.md)

## Execute

Follow the canonical recipe in [docs/ai/skills/fix-sonar/SKILL.md](../../../docs/ai/skills/fix-sonar/SKILL.md) exactly.

## Verify

- Findings are addressed without architecture drift.
- The affected scope still builds and tests cleanly.
- The Brain remains the source of truth.
