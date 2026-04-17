---
spec:
  id: pineguard.ai.data-annotations.code-coverage
  title: "PineGuard.DataAnnotations Code Coverage (Addendum)"
  version: 1
  template:
    - ../template-coverage.md
  parent:
    - ../../spec.md
    - ../../testing/coverage.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.DataAnnotations/**"
  - "tests/PineGuard.DataAnnotations.UnitTests/**"
---

# PineGuard.DataAnnotations Code Coverage (Addendum)

This file contains **DataAnnotations-specific** coverage notes only.

Global rules and workflows:

- `docs/ai/testing/coverage.md`

---

## Purpose

Provide DataAnnotations-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for DataAnnotations, the scope is complete.
- If `src/PineGuard.DataAnnotations` currently contains no real `*.cs` files (outside `bin/`/`obj/`), the analyzer may skip this scope until code exists.

## Quick start (DataAnnotations only)

Use the auto-approved workflow:
`view_file .agent/workflows/coverage-annotation.md`

## Enforce 100% (DataAnnotations only)

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope DataAnnotations -SkipHtml -Enforce100
```

## Default test project (fast loop)

- `tests/PineGuard.DataAnnotations.UnitTests/PineGuard.DataAnnotations.UnitTests.csproj`
