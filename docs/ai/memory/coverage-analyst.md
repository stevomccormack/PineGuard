# Coverage Analyst Memory

**Role:** `docs/ai/roles/planner.md`

## Durable Patterns

- Use xplat (Coverlet) coverage as the default engine on this repository.
- Prefer scope-specific coverage runs over `All` when isolating failures.
- Look for branch gaps even when line coverage is already green.
- Convert gaps into concrete test-case recommendations, not vague advice.

## Common Gap Types

- Null checks not exercised.
- Partial `&&` or `||` branches.
- Guard success paths that do not assert the returned value.
- Edge values: empty strings, whitespace, min/max numbers, empty collections.
- Configuration parameter validation not explicitly tested.

## Coverage Tool Usage

The flags, valid scopes, and output paths are operational docs owned by
`tools/code-coverage/README.md` — read that, don't rely on remembered commands. Gotchas this
agent has actually hit:

- `-Format cobertura` MUST be supplied explicitly — omitting it fails the `Gen-CoverageReport.ps1` ValidateSet.
- There is no `-Engine` parameter. Any stored command carrying one is stale — delete the flag, do not translate it.
- Drop `-Isolated` on multi-targeted projects (Core, MustClauses, GuardClauses); the non-isolated run is equally authoritative.

## Known Constraints

- dotCover 2025.3.3 works on net8.0 and net10.0 (fixed Mar 2026 by adding Webroot AV exclusions). The earlier
  "dotCover is broken / removed" note is stale — do not reinstate it.
- The repository ships no dotCover wrapper under `tools/code-coverage/`, so every run today goes through xplat.
  The engine is supported; the wrapper is what needs restoring if a dotCover run is required.
- FluentValidation scope may need to be isolated when compile issues exist elsewhere.

## Canonical References

- `../agents/coverage-core.md`
- `../agents/coverage-all.md`
- `../skills/improve-coverage/SKILL.md`
- `../specs/testing/coverage.md`
