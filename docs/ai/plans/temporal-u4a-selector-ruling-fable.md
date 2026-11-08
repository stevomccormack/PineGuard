<!-- metadata_header
type: receipt
id: temporal-u4a-selector-ruling-fable
version: 1.0
status: binding-ruling
last_updated: 2026-09-27
-->

# T1-U4a — Fable selector ruling receipt

**Reviewer:** Claude Fable 5.1, resumed persisted session `8fc20ebe-d70e-47fe-8e1b-01d79e3bb527`.
**Date:** 2026-09-27.
**Result:** binding, narrow correction to the T1-U4a generated file-list command; no implementation
or unit-design change.

## Decision

Replace exactly `'tests/PineGuard.MustClauses.UnitTests/**/*TimeSpan*'` in the T1-U4a `git grep`
command with `'tests/PineGuard.MustClauses.UnitTests/*TimeSpan*'`. Every other regex and pathspec in
the command is preserved unchanged.

## Rationale (scope-preserving)

Root-level Must test/data files have no slash after the project directory
(`tests/PineGuard.MustClauses.UnitTests/`), so the original literal `**/` pathspec matched nothing
there and silently omitted them from the generated file list. The corrected single-level pathspec
returns exactly the four root Must TimeSpan files. This fixes the selector omission only; it adds no
file outside U4a's existing Core + codes + Must scope, and it removes no file U4a already owned.

## Current output snapshot (dated evidence, 2026-09-27 — 15 files)

This is a snapshot of what the corrected command currently returns. The generated command in
`temporal-01-foundation.md` remains the ownership authority; this list is not re-authoritative and
must not be hand-maintained independently of a re-run.

```
src/PineGuard.Core/Codes/MustCodes.Time.cs
src/PineGuard.Core/Rules/StringRules.TimeSpan.cs
src/PineGuard.Core/Rules/TimeSpanRules.cs
src/PineGuard.MustClauses/MustStringTimeSpanClauses.cs
src/PineGuard.MustClauses/MustTimeSpanClauses.cs
tests/PineGuard.Core.UnitTests/Rules/StringRules/StringRulesTimeSpanTestData.cs
tests/PineGuard.Core.UnitTests/Rules/StringRules/StringRulesTimeSpanTests.cs
tests/PineGuard.Core.UnitTests/Rules/TimeSpanRulesTestData.cs
tests/PineGuard.Core.UnitTests/Rules/TimeSpanRulesTests.cs
tests/PineGuard.MustClauses.UnitTests/MustStringTimeSpanClausesTestData.cs
tests/PineGuard.MustClauses.UnitTests/MustStringTimeSpanClausesTests.cs
tests/PineGuard.MustClauses.UnitTests/MustTimeSpanClausesTestData.cs
tests/PineGuard.MustClauses.UnitTests/MustTimeSpanClausesTests.cs
tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeSpan.cs
tests/PineGuard.Testing/Fixtures/TimeSpanRulesFixtures.cs
```

## Explicit exclusions

- **T1-U4**: the seven Guard boundary-polarity files are already completed and out of scope; this
  ruling touches none of them.
- **T1-U4b**: owns the Guard/Fluent/DataAnnotations TimeSpan rename; not touched by this ruling.
- **T1-U3**: owns TimeOnly midnight-crossing Between/NotBetween; not touched by this ruling.
- **`NumberRules` / `RuleComparison`**: never touched by this or any T1-U4a rename, per the plan's
  existing "never touch" instruction.
- **Every other unrelated family**: excluded; this ruling adds no file outside U4a's declared
  Core + codes + Must scope.
