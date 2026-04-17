---
name: scan-run
description: Run SonarQube analysis for PineGuard by following the canonical Brain workflow.
---
# Skill: Run SonarQube Analysis

## Load First

Read these files before running analysis:
1. [scan/spec.md](../../../docs/ai/specs/scan/spec.md)
2. [scan.md](../../../docs/ai/rules/scan.md)
3. [scan-run/SKILL.md](../../../docs/ai/skills/scan-sonar/SKILL.md)
4. [code-reviewer memory](../../../docs/ai/memory/code-reviewer.md)

## Execute

Follow the canonical recipe in [docs/ai/skills/scan-sonar/SKILL.md](../../../docs/ai/skills/scan-sonar/SKILL.md) exactly.

## Verify

- Analysis completes successfully.
- Results are reported through the expected workflow.
- The Brain remains the source of truth.
