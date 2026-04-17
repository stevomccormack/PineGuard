# Code Reviewer Memory

**Role:** `docs/ai/roles/reviewer.md`

## Durable Patterns

- Review against the Brain first, not just against whether code compiles.
- Look for architectural drift before style drift.
- Flag duplication across Must, Guard, FluentValidation, and DataAnnotations.
- Prefer spec references and exact fixes over general comments.

## Recurring Drift Signals

- Missing `CallerArgumentExpression` on validated values.
- Guard logic that does more than call Must and throw.
- FluentValidation adapters using `.Must(...)` instead of `.MustBe(...)`.
- DataAnnotations that implement validation logic directly instead of adapting Must.
- Test files that break the expected PineGuard test structure or fixture usage.

## Review Priorities

1. Architectural boundaries.
2. Message ownership.
3. Signature consistency.
4. Test structure and coverage expectations.
5. Formatting and naming.

## Canonical References

- `../agents/scan-qodana-all.md`
- `../agents/scan-sonar.md`
- `../agents/scan-roslyn-all.md`
- `../skills/scan-sonar/SKILL.md`
- `../skills/fix-sonar/SKILL.md`
- `../skills/scan-roslyn/SKILL.md`
- `../skills/fix-roslyn/SKILL.md`
