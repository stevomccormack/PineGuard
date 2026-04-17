---
spec:
  id: pineguard.ai.specs.testing.code-coverage
  title: "PineGuard Code Coverage (Global Spec)"
  version: 3
  parent:
    - ../../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "tools/code-coverage/**"
  - "src/**"
  - "tests/**"
---

# PineGuard Code Coverage (Global Spec)

This is the repo's **coverage playbook**.

Primary goal:

- Reach **100% line + 100% branch** in the repo's **filtered scope** (enforced via the xplat analyzer).

Important constraints:

- Coverage work is allowed to include **bug fixes**.
- Coverage work must stay **small and local**: do not introduce "big redesigns" just to satisfy coverage.
- Only make larger architectural changes if an **exceptional**, **high-level defect** is discovered (incorrect API contract, broken invariants, security-sensitive bug, etc.).

Libraries covered by this workflow:

- `PineGuard.Core`
- `PineGuard.MustClauses`
- `PineGuard.GuardClauses`
- `PineGuard.DataAnnotations`
- `PineGuard.FluentValidation`
- `PineGuard.Testing` _(shared test infrastructure library; has no own test runner — its code is exercised via all other `*.UnitTests` projects)_

Per-library run notes live in:

- `docs/ai/specs/core/coverage.md`
- `docs/ai/specs/must-clauses/coverage.md`
- `docs/ai/specs/guard-clauses/coverage.md`
- `docs/ai/specs/data-annotations/coverage.md`
- `docs/ai/specs/fluent-validation/coverage.md`

---

## Non-Negotiables

- The enforcement standard (when enforcing) is **exactly 100%**:
  - line coverage
  - branch coverage
  - within the analyzer's filtered scope.

- Any helper logic discovered/created during coverage work (PowerShell helpers, common fixes, repeatable diagnostics, report cleanup, etc.) should be captured as reusable `.ps1` helpers under `tools/code-coverage/**` (or adjacent tooling folders) rather than repeated inline in sessions. Keep coverage tooling DRY.

---

## What "Filtered Scope" Means (One Sentence)

Coverage is collected broadly, then the analyzer filters to a scope via `-Scope` or `-IncludeFileRegex` / `-ExcludeFileRegex`.

Note:

- Cobertura paths can be weird (absolute paths, missing drive letters). The analyzer normalizes them.

---

## The Efficient Workflow (xplat loop)

Use this loop until xplat enforcement is green.

### Quick start (Auto-Approved Workflows)

Instead of running raw PowerShell, **use the auto-approved workflows** to skip approval steps.
Read the workflow file using `view_file` to execute it (it contains `// turbo-all`).

- **Core**: `.agent/workflows/coverage-core.md`
- **Must**: `.agent/workflows/coverage-must.md`
- **Guard**: `.agent/workflows/coverage-guard.md`
- **DataAnnotations**: `.agent/workflows/coverage-annotation.md`
- **FluentValidation**: `.agent/workflows/coverage-fluent.md`
- **All**: `.agent/workflows/coverage-all.md`

Example execution (Core):

1. `view_file .agent/workflows/coverage-core.md`
2. (The workflow auto-executes the coverage run)

### Manual commands (fallback / custom args)

Only use these if you need custom arguments not covered by the workflows.

```powershell
# Generate (Cobertura XML + HTML)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope Core

# Analyze (pick targets)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Core -Top 30

# Enforce 100%
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Core -Enforce100
```

Speed rules:

- Prefer `-SkipHtml` while iterating.
- By default, generation runs the tightest test project for `Core|Must|Guard` for speed.
- Use `-ProjectFilter "*.UnitTests.csproj"` only when you need broader execution.

---

## How to Fix Coverage Efficiently

### 1) Use the analyzer output as the work list

Work from the "Lowest-covered classes" list until everything is 100%.

### 2) For branch misses, read Cobertura to see the exact missing condition outcomes

- Open the newest Cobertura XML under:
  - `artifacts/code-coverage/xplat/testresults/<ProjectName>/<RunId>/coverage.cobertura.xml`

- Search for:
  - `<line ... condition-coverage="50% (1/2)">` (or any `<100%`)

This tells you exactly what to flip (stop guessing).

### 3) Prefer TestData permutations over complicated mocks

Most misses are "we didn't test this input combination". Add the missing input rows.

See Core test data conventions:

- `docs/ai/specs/core/unit-test.md`

---

## Refactoring Guidance (Coverage-driven)

Allowed:

- Small, local refactors that preserve behavior and remove unreachable branches.
- Micro-fixes to align implementation with stated contract.

Not allowed (unless exceptional/high-level defect):

- Large architectural rewrites.
- Changing public API shapes "for coverage".

Rule:

- If you refactor, immediately rerun the xplat loop to confirm branch counts actually dropped.

---

## Scopes

Supported presets:

- `Core`, `MustClauses`, `GuardClauses`, `DataAnnotations`, `FluentValidation`, `All`
- `Custom` (analyzer only)

### PineGuard.Testing scope

`PineGuard.Testing` has no own `*.UnitTests` project and therefore has no dedicated scope preset in the scripts.

To analyze its coverage, first run the `All` scope (which collects data from all `*.UnitTests` projects), then use `Custom` scope with a class name regex:

```powershell
# Step 1: generate All-scope coverage (collects PineGuard.Testing execution data)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode Generate -Scope All -Isolated

# Step 2: analyze PineGuard.Testing specifically
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Custom -IncludeClassNameRegex "^PineGuard\.Testing\." -Top 30 -Enforce100
```

Notes:

- Use the tightest scope you can while iterating.
- If a scoped library has no real sources (`*.cs` outside `bin/`/`obj/`), analysis may skip until code exists.

---

## Artifacts (where things land)

- HTML report:
  - `artifacts/code-coverage/xplat/html/index.html`
- Stable redirect (open this most of the time):
  - `artifacts/code-coverage/xplat-report.html`
- Cobertura XML:
  - `artifacts/code-coverage/xplat/testresults/<ProjectName>/<RunId>/coverage.cobertura.xml`

---

## Troubleshooting (high signal)

- "Coverage output looked invalid … retrying once …"
  - This happens occasionally; the generator retries once automatically.

- "No test is available …"
  - Prefer running via the repo scripts. The generator tries to avoid running empty projects.

- Can't find coverage XML via search
  - Many tools exclude `artifacts/**`. Enable "include ignored files".

---

## Next Session Prompt Template

- Goal: xplat analyzer `-Enforce100` green for target scope.
- Loop: generate -> analyze -> implement -> repeat.

---

## Reference

- [tools/code-coverage/README.md](../../tools/code-coverage/README.md)
- [tools/code-coverage/Run-CodeCoverage.ps1](../../tools/code-coverage/Run-CodeCoverage.ps1)
- [tools/code-coverage/xplat/Gen-CoverageReport.ps1](../../tools/code-coverage/xplat/Gen-CoverageReport.ps1)
- [tools/code-coverage/xplat/Test-CoverageAnalysis.ps1](../../tools/code-coverage/xplat/Test-CoverageAnalysis.ps1)
- `tools/code-coverage/coverlet.runsettings`
