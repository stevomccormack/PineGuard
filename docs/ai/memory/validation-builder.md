# Validation Builder Memory

**Role:** `docs/ai/roles/builder.md`

## Normative Rules

The layer architecture — Core Utils -> Core Rules -> MustClauses -> GuardClauses -> Integrations,
Must owns user-facing messages, Guard/FluentValidation/DataAnnotations reuse them, Core stays pure,
and every clause passes its `MustCodes` constant on every `Fail(...)`/`FromBool(...)` call (Rule13) —
is fully specified in `../rules/global.md`, `../rules/must.md`, `../rules/guard.md`, and
`../specs/must-clauses/project.md` ("Error codes"). Read those; this file records observations, not rules.

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
