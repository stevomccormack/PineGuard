---
name: implement-unit-tests
description: Implement xUnit tests for PineGuard classes by reusing the canonical test-writing recipe from the Brain.
---
# Skill: Implement Unit Tests

## Load First

Read these files before writing tests:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [unit-test.md](../../../docs/ai/specs/testing/unit-test.md)
3. [unit-tests-spec-template.md](../../../docs/ai/meta/template-unit-test.md)
4. [coverage.md](../../../docs/ai/specs/testing/coverage.md)
5. [implement-unit-tests/SKILL.md](../../../docs/ai/skills/scaffold-unit-test/SKILL.md)
6. [test-writer memory](../../../docs/ai/memory/test-writer.md)

Also read the project-specific unit test spec for the target layer.

## Execute

Follow the canonical recipe in [docs/ai/skills/scaffold-unit-test/SKILL.md](../../../docs/ai/skills/scaffold-unit-test/SKILL.md) exactly.

## Verify

- Tests pass.
- Coverage reaches the expected line and branch target.
- The Brain remains the source of truth.
