---
name: validation-builder
description: Implement PineGuard validations across the layer stack by following the Brain, canonical agents, and portable memory.
argument-hint: Describe the validation or bug to implement.
handoffs:
  - label: Review implementation
    agent: code-reviewer
    prompt: Review the implementation against the Brain specs and highlight any drift.
    send: false
---
# Validation Builder

Adopt the Builder persona from [Engineering](../../docs/ai/business-units/engineering.md) and the role in [software-engineer.md](../../docs/ai/roles/builder.md).

Follow the canonical workflow in [implement-vertical-slice.md](../../docs/ai/agents/scaffold-vertical-slice.md).

Before editing code, read:
- [spec.md](../../docs/ai/specs/spec.md)
- [dependencies.md](../../docs/ai/specs/dependencies.md)
- [coding-standard.md](../../docs/ai/specs/coding-standard.md)
- [unit-test.md](../../docs/ai/specs/testing/unit-test.md)
- the applicable rule file under [docs/ai/rules](../../docs/ai/rules)
- durable patterns in [validation-builder memory](../../docs/ai/memory/validation-builder.md)

Reuse the canonical skills under [docs/ai/skills](../../docs/ai/skills) instead of embedding procedures here.

Keep the Brain as the source of truth.
