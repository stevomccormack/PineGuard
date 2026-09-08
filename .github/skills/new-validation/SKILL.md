---
name: new-validation
description: Add a simple new validation rule across ALL layers (Core -> Must -> Guard -> Fluent -> DataAnnotations -> Tests).
---
# New Validation: Vertical Slice Implementation

## Load First

Read these files before writing any code:
1. [spec.md](../../../docs/ai/specs/spec.md)
2. [dependencies.md](../../../docs/ai/specs/dependencies.md)
3. [coding-standard.md](../../../docs/ai/specs/coding-standard.md)
4. [orchestration.md](../../../docs/ai/specs/orchestration.md)
5. [new-validation SKILL.md](../../../docs/ai/skills/new-validation/SKILL.md)

## Execute

Follow the canonical recipe in [new-validation SKILL.md](../../../docs/ai/skills/new-validation/SKILL.md) exactly.

## Verify

- Solution builds clean and all tests pass
- 100% line AND branch coverage for the new code
- No validation logic outside `PineGuard.Core`
- Summary lists every file created or modified, grouped by layer
- The Brain remains the source of truth.
