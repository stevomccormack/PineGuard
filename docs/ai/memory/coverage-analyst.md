# Coverage Analyst Memory

**Role:** `docs/ai/roles/planner.md`

## Durable Patterns

- Use xplat coverage as the authoritative engine on this repository.
- Prefer scope-specific coverage runs over `All` when isolating failures.
- Look for branch gaps even when line coverage is already green.
- Convert gaps into concrete test-case recommendations, not vague advice.

## Common Gap Types

- Null checks not exercised.
- Partial `&&` or `||` branches.
- Guard success paths that do not assert the returned value.
- Edge values: empty strings, whitespace, min/max numbers, empty collections.
- Configuration parameter validation not explicitly tested.

## Known Constraints

- dotCover was removed (Mar 2026) due to unresolvable bugs on Windows 11 24H2. Use xplat only.
- FluentValidation scope may need to be isolated when compile issues exist elsewhere.

## Canonical References

- `../agents/coverage-core.md`
- `../agents/coverage-all.md`
- `../skills/improve-coverage/SKILL.md`
- `../specs/testing/coverage.md`
