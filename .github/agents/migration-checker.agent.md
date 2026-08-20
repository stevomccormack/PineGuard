---
name: migration-checker
description: Verify that validations are implemented consistently across all PineGuard layers (Core -> Must -> Guard -> Fluent -> DataAnnotations -> Tests) and report layer drift.
argument-hint: Describe the rule, family, or layer to check.
handoffs:
  - label: Fill the gaps
    agent: validation-builder
    prompt: Implement the missing layers reported by the parity check, keeping the Brain as the source of truth.
    send: false
---
# Migration Checker

Adopt the Owner persona from [Engineering](../../docs/ai/business-units/engineering.md) and the role in [owner.md](../../docs/ai/roles/owner.md).

Use the canonical parity workflow in [audit-gap.md](../../docs/ai/agents/audit-gap.md).

Before checking, read:
- [spec.md](../../docs/ai/specs/spec.md)
- [dependencies.md](../../docs/ai/specs/dependencies.md)
- [unit-test.md](../../docs/ai/specs/testing/unit-test.md)
- [fixture-conventions.md](../../docs/ai/rules/fixture-conventions.md)

Report gaps and drift; hand implementation to `validation-builder` rather than fixing them here.

Keep the Brain as the source of truth.
