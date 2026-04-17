# Validation Builder Memory

**Role:** `docs/ai/roles/builder.md`

## Durable Patterns

- Respect the layer order: Core Utils -> Core Rules -> MustClauses -> GuardClauses -> Integrations.
- `Must` owns user-facing messages. Guard, FluentValidation, and DataAnnotations reuse them.
- Guard methods call Must methods and throw through `GuardFailure.Throw(...)`; they do not duplicate validation logic.
- Core stays pure: no IO, no user-facing messages, and no architectural shortcuts around Must.

## Signature Heuristics

- Must signatures use `this IMustClause _`, `value`, and `[CallerArgumentExpression(nameof(value))] string? paramName = null`.
- Guard signatures use `this IGuardClause _`, `value`, `message`, and `exceptionCreator`.
- FluentValidation adapters use `.MustBe(...)`, not `.Must(...)`.
- DataAnnotations are strict adapters over Must and inherit `ValidationAttributeBase`.

## Common Mistakes

- Duplicating messages or logic outside Must.
- Forgetting `CallerArgumentExpression`.
- Using nullable value types in Rules where non-nullable values are expected.
- Skipping tests or coverage validation for new paths.

## Canonical References

- `../agents/scaffold-vertical-slice.md`
- `../skills/scaffold-rule/SKILL.md`
- `../skills/scaffold-must/SKILL.md`
- `../skills/scaffold-guard/SKILL.md`
- `../skills/scaffold-fluent/SKILL.md`
- `../skills/scaffold-annotation/SKILL.md`
- `../skills/scaffold-unit-test/SKILL.md`
