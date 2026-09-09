# Coverage Analyst Memory

**Role:** `docs/ai/roles/planner.md`

## Durable Patterns

- Use Coverlet coverage as the default engine on this repository.
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

- `-Format` defaults to `cobertura` in `tools/code-coverage/New-CoverageReport.ps1` — it does not need to be supplied explicitly.
- `-Engine` exists (`Coverlet` | `DotCover`, default `Coverlet`) and is forwarded through the D-9 front door
  (`New-CoverageReport.ps1`). `-Engine DotCover` is snapshot-only — it collects a Rider `.dcvr` file and cannot be
  gated; do not strip a stored `-Engine` flag, translate it if the intent changes.
- Drop `-Isolated` on multi-targeted projects (Core, MustClauses, GuardClauses); the non-isolated run is equally authoritative.

## Known Constraints

- dotCover 2025.3.3 works on net8.0 and net10.0 (fixed Mar 2026 by adding Webroot AV exclusions). The earlier
  "dotCover is broken / removed" note is stale — do not reinstate it.
- The repository ships a dotCover wrapper at `tools/code-coverage/dotcover/New-CoverageReport.ps1`, but it is
  snapshot-only: it writes a `.dcvr` file into `artifacts/code-coverage/dotcover/<scope>/snapshots/`, meant to be
  opened in Rider's coverage viewer. It is never used for CI gating — Coverlet remains the sole CI/gate-authority
  engine, and every run today goes through Coverlet by default.
- FluentValidation scope may need to be isolated when compile issues exist elsewhere.

## Canonical References

- `../agents/coverage-core.md`
- `../agents/coverage-all.md`
- `../skills/improve-coverage/SKILL.md`
- `../specs/testing/coverage.md`
