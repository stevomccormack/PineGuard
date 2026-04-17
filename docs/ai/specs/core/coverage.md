---
spec:
  id: pineguard.ai.core.code-coverage
  title: "PineGuard.Core Code Coverage (Addendum)"
  version: 1
  template:
    - ../template-coverage.md
  parent:
    - ../../spec.md
    - ../../testing/coverage.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.Core/**"
  - "tests/PineGuard.Core.UnitTests/**"
---

# PineGuard.Core Code Coverage (Addendum)

This file contains **Core-specific** coverage notes only.

Global rules and workflows:

- `docs/ai/testing/coverage.md`

---

## Purpose

Provide Core-specific command lines and defaults while keeping the global coverage workflow centralized.

Important:

- After xplat reaches **100% line + 100% branch** for Core, the scope is complete.

## Quick start (Core only)

Use the auto-approved workflow:
`view_file .agent/workflows/coverage-core.md`

## Enforce 100% (Core only)

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Core -SkipHtml -Enforce100
```

## Broader run (all unit test projects)

Use this when Core coverage depends on behavior executed by other test projects:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Core -SkipHtml -ProjectFilter "*.UnitTests.csproj" -Top 30
```

## Default test project (fast loop)

- `tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj`

## Related specs

- Unit tests addendum: `docs/ai/specs/core/unit-test.md`
