---
spec:
  id: pineguard.ai.must-clauses.code-coverage
  title: "PineGuard.MustClauses Code Coverage (Addendum)"
  version: 1
  template:
    - ../../meta/template-coverage.md
  parent:
    - ../spec.md
    - ../testing/coverage.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.MustClauses/**"
  - "tests/PineGuard.MustClauses.UnitTests/**"
---

# PineGuard.MustClauses Code Coverage (Addendum)

This file contains **MustClauses-specific** coverage notes only.

Global rules and workflows:

- [Global coverage spec](../testing/coverage.md)

---

## Purpose

Provide MustClauses-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for MustClauses, the scope is complete.

## Quick start (MustClauses only)

Use the auto-approved workflow: `docs/ai/agents/coverage-must.md`
(→ `docs/ai/workflows/coverage.md` with **Scope = MustClauses**).

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

## Related specs

- Unit tests addendum: `docs/ai/specs/must-clauses/unit-test.md`
