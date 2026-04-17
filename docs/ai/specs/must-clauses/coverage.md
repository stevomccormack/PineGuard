---
spec:
  id: pineguard.ai.must-clauses.code-coverage
  title: "PineGuard.MustClauses Code Coverage (Addendum)"
  version: 1
  template:
    - ../template-coverage.md
  parent:
    - ../../spec.md
    - ../../testing/coverage.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.MustClauses/**"
  - "tests/PineGuard.MustClauses.UnitTests/**"
---

# PineGuard.MustClauses Code Coverage (Addendum)

This file contains **MustClauses-specific** coverage notes only.

Global rules and workflows:

- `docs/ai/testing/coverage.md`

---

## Purpose

Provide MustClauses-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for MustClauses, the scope is complete.

## Quick start (MustClauses only)

Use the auto-approved workflow:
`view_file .agent/workflows/coverage-must.md`

## Speed Optimization (Coverage Remediation)
> [!TIP]
> **Do not run the full coverage generation script (`Run-CodeCoverage.ps1`) for every single file.**

When remediating coverage gaps across many files (e.g. `Must*Clauses`):
1. **Use Static Analysis:** Look at the implementation file (e.g., `MustStringDateOnlyClauses.cs`) and cross-reference with the Test Data file. Missing branches are highly predictable:
   - Null parameter checks (`if (value is null)`)
   - Invalid secondary parameters (e.g., `min > max` for ranges)
   - Parsing failures (`StringUtility.X.TryParse`)
   - Early returns (`if (input is null) return;` in tests which skip evaluation)
2. **Cross Reference:** Cross reference sibling classes for Tests and TestData in PineGuard.UnitTests.Core for Rules. They are a good source of truth for what should be tested as they have 100% coverage. Especially TestData as we ultimately want to create a common test data class for all libraries as the data should really be same/similar for value/reference params.
3. **Targeted Testing:** When fixing a test, run `dotnet test --filter "FullyQualifiedName~[SpecificTestClass]"` (e.g., `dotnet test ... --filter "FullyQualifiedName~MustStringDateOnlyClausesTests"`). This finishes in 2-4 seconds instead of 30+.
4. **Batching:** Fix 2-3 files in a row using static analysis.
5. **Final Verification:** Only run the full `Run-CodeCoverage.ps1` script at the end to visually confirm 100% metrics across the board or when completely stuck.

## Rehydrate Context
After running the coverage script (or every 10mins), you need to rehydrate the context for the AI.
  - `docs/ai/testing/coverage.md`
  - `docs/ai/testing/unit-test.md`
  - `docs/ai/specs/[Project]/coverage.md`
  - `docs/ai/specs/[Project]/unit-test.md`
  - `docs/ai/specs/coding-standard.md`
  - `docs/ai/dependencies.md`
  - `docs/ai/specs/orchestration.md`

## Enforce 100% (MustClauses only)

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope MustClauses -SkipHtml -Enforce100
```

## Broader run (all unit test projects)

Use this when MustClauses coverage depends on behavior executed by other test projects:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope MustClauses -SkipHtml -ProjectFilter "*.UnitTests.csproj" -Top 30
```

## Default test project (fast loop)

- `tests/PineGuard.MustClauses.UnitTests/PineGuard.MustClauses.UnitTests.csproj`
