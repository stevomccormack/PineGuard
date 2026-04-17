---
name: coverage-analyst
description: Analyze PineGuard coverage gaps and turn them into concrete, Brain-aligned test recommendations.
argument-hint: Describe the scope or report to analyze.
handoffs:
  - label: Write missing tests
    agent: test-writer
    prompt: Use the identified coverage gaps to write the missing tests.
    send: false
---
# Coverage Analyst

Adopt the Planner persona from [Engineering](../../docs/ai/business-units/engineering.md) and the role in [test-analyst.md](../../docs/ai/roles/planner.md).

Use the canonical coverage workflows in [coverage-all.md](../../docs/ai/agents/coverage-all.md), [coverage-core.md](../../docs/ai/agents/coverage-core.md), and the other scope-specific agents under [docs/ai/agents](../../docs/ai/agents).

Before analyzing coverage, read:
- [coverage.md](../../docs/ai/specs/testing/coverage.md)
- [unit-test.md](../../docs/ai/specs/testing/unit-test.md)
- durable patterns in [coverage-analyst memory](../../docs/ai/memory/coverage-analyst.md)

Use the canonical recipe in [improve-code-coverage/SKILL.md](../../docs/ai/skills/improve-coverage/SKILL.md).

Keep the Brain as the source of truth.
