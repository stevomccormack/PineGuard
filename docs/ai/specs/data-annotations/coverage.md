---
spec:
  id: pineguard.ai.data-annotations.code-coverage
  title: "PineGuard.DataAnnotations Code Coverage (Addendum)"
  version: 1
  template:
    - ../../meta/template-coverage.md
  parent:
    - ../spec.md
    - ../testing/coverage.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.DataAnnotations/**"
  - "tests/PineGuard.DataAnnotations.UnitTests/**"
---

# PineGuard.DataAnnotations Code Coverage (Addendum)

This file contains **DataAnnotations-specific** coverage notes only.

Global rules and workflows:

- [Global coverage spec](../testing/coverage.md)

---

## Purpose

Provide DataAnnotations-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for DataAnnotations, the scope is complete.
- If the analyzer reports this scope as skipped, treat it as a tooling failure — the library has full source coverage under `src/PineGuard.DataAnnotations/`.

## Quick start (DataAnnotations only)

Use the auto-approved workflow: `docs/ai/agents/coverage-annotation.md`
(→ `docs/ai/workflows/coverage.md` with **Scope = DataAnnotations**).

## Enforce 100% (DataAnnotations only)

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope DataAnnotations -SkipHtml -Enforce100
```

## Broader run (all unit test projects)

Use this when DataAnnotations coverage depends on behavior executed by other test projects:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope DataAnnotations -SkipHtml -ProjectFilter "*.UnitTests.csproj" -Top 30
```

## Default test project (fast loop)

- `tests/PineGuard.DataAnnotations.UnitTests/PineGuard.DataAnnotations.UnitTests.csproj`

## Related specs

- Unit tests addendum: `docs/ai/specs/data-annotations/unit-test.md`
