---
spec:
  id: pineguard.ai.fluent-validation.code-coverage
  title: "PineGuard.FluentValidation Code Coverage (Addendum)"
  version: 1
  template:
    - ../template-coverage.md
  parent:
    - ../../spec.md
    - ../../testing/coverage.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.FluentValidation/**"
  - "tests/PineGuard.FluentValidation.UnitTests/**"
---

# PineGuard.FluentValidation Code Coverage (Addendum)

This file contains **FluentValidation-specific** coverage notes only.

Global rules and workflows:

- `docs/ai/testing/coverage.md`

---

## Purpose

Provide FluentValidation-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for FluentValidation, the scope is complete.

## Quick start (FluentValidation only)

Use the auto-approved workflow:
`view_file .agent/workflows/coverage-fluent.md`

## Enforce 100% (FluentValidation only)

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope FluentValidation -SkipHtml -Enforce100
```

## Broader run (all unit test projects)

Use this when FluentValidation coverage depends on behavior executed by other test projects:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope FluentValidation -SkipHtml -ProjectFilter "*.UnitTests.csproj" -Top 30
```

## Default test project (fast loop)

- `tests/PineGuard.FluentValidation.UnitTests/PineGuard.FluentValidation.UnitTests.csproj`
