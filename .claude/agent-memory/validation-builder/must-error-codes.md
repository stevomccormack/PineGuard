---
name: must-error-codes
description: Every Must clause carries a MustCodes catalogue constant on every failure path — grammar, catalogue shape, and the reflection invariants that will reject a sloppy constant
metadata:
  type: project
---

Since Phase 1 of `docs/ai/plans/new-surfaces-missing-validation-cases-01-structural-validation.md`,
`MustResult<T>` carries a machine-readable `Code`. Every `Fail`/`FromBool` call inside a public clause
passes a `MustCodes.<Domain>.<Aspect>.<Condition>` constant — `Fail(code, template, paramName, value)`,
`FromBool(ok, code, template, paramName, value, result)`.

**Why:** the code is the stable contract for 400 bodies, log lines, localisation tables and the Guard
exception map; the message is the human half and is free to change. Plan 00 §5.4 is the grammar; Rule13
(audit) and `MustCodesTests` (reflection, in Core) are the two gates.

**How to apply when writing or reviewing a clause:**
- One clause, one code — the null early-return and any configuration-parameter path (`nameof(length)`,
  `nameof(pattern)`) reuse the clause's semantic constant. The *message* differentiates, not the code.
- The condition names what was **observed on failure**, never the requirement: `NotNullOrWhiteSpace`
  fails when the text IS blank → `text.content.blank`; `LongerThan` fails when it IS too short →
  `text.length.too-short`. Exact antonym where one exists, otherwise `not-` prefix.
- Type-variant / overloaded clauses share one constant (both `DigitsOnly` overloads → `text.charset.not-digits`).
- Catalogue files live in `src/PineGuard.Core/Codes/MustCodes.<Domain>.cs`, one nested class per aspect,
  a `Prefix` const per node, every value composed (`Prefix + ".blank"`) so the compiler folds it.

**The invariants that bite** (`tests/PineGuard.Core.UnitTests/Codes/MustCodesTests.cs` reflects over the
whole tree, so a new domain file is auto-enrolled):
- The constant's **value must equal its kebab-cased identifier path** — `Text.Content.NullOrEmpty` ↔
  `text.content.null-or-empty`. Kebab = `-` inserted at every lower/digit→upper boundary, so pick
  identifiers that kebab correctly (`NotUpperSnake` → `not-upper-snake`).
- **Values are globally unique.** Two clauses sharing a failure state must reference the *same* constant,
  never two constants with the same string.
- Exactly three segments → constants live at `Domain.Aspect.Name`, never deeper or shallower.
- Every nested class declares `Prefix`; every public member needs a `<summary>` (CS1591 is an error), and
  leaves carry the folded literal in `<c>…</c>` so text search still finds the code.
- Domain classes are compiled with the full analyzer set: CA1711 (no `…Dictionary`/`…Enum`/`…Attribute`
  type names) and CA1716 (no `Date`, `Alias`, other reserved words) will fail the build for a domain or
  aspect class name — check the name before writing the file.

Test wiring: `MustExpected`/`FluentExpected`/`GuardExpected`/`DataAnnotationExpected` take a trailing
`Code`, asserted by the layer base class only when non-null. Spot-check one representative group per
clause; Rule13 is the exhaustive check. Legacy-shaped test files (raw `IsCase<T>` + `Assert.Equal`, e.g.
`MustStringClausesTests`) have no `MustExpected` to hang it on — add a small
`AssertCode(expectedCode, result)` helper that asserts only when `result.Failed` rather than rewriting
the file to the v2 fixture architecture.

Related: [[fixture-architecture]]
