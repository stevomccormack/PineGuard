---
spec:
  id: pineguard.ai.guard-clauses.code-coverage
  title: "PineGuard.GuardClauses Code Coverage (Addendum)"
  version: 1
  template:
    - ../../meta/template-coverage.md
  parent:
    - ../spec.md
    - ../testing/coverage.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.GuardClauses/**"
  - "tests/PineGuard.GuardClauses.UnitTests/**"
---

# PineGuard.GuardClauses Code Coverage (Addendum)

This file contains **GuardClauses-specific** coverage notes only.

Global rules and workflows:

- [Global coverage spec](../testing/coverage.md)

---

## Purpose

Provide GuardClauses-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for GuardClauses, the scope is complete.

## Quick start (GuardClauses only)

Use the auto-approved workflow:
`view_file .agent/workflows/coverage-guard.md`

## Enforce 100% (GuardClauses only)

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope GuardClauses -SkipHtml -Enforce100
```

## Broader run (all unit test projects)

Use this when GuardClauses coverage depends on behavior executed by other test projects:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope GuardClauses -SkipHtml -ProjectFilter "*.UnitTests.csproj" -Top 30
```

## Default test project (fast loop)

- `tests/PineGuard.GuardClauses.UnitTests/PineGuard.GuardClauses.UnitTests.csproj`

## Related specs

- Unit tests addendum: `docs/ai/specs/guard-clauses/unit-test.md`
