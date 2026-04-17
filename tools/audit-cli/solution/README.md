# PineGuard.AuditCli

`PineGuard.AuditCli` is a small .NET console tool used by the PowerShell wrappers under `tools/audit-cli/**` to run static audits against the PineGuard repo.

It is designed for **local developer workflows** and CI validation:

- Reads source code via Roslyn (`MSBuildWorkspace`)
- Writes reports under `artifacts/audit/`
- Exits non-zero when violations are found (unless explicitly allowed)

## Prerequisites

- .NET SDK (repo targets .NET 8)
- The solution must be buildable on the machine (Roslyn workspace loads projects via MSBuild)

If you see `MSBuildWorkspace` diagnostics, they usually indicate missing workloads/SDKs or a restore/build issue.

## Quick start

Show help:

```powershell
dotnet run --project tools/audit-cli/solution/PineGuard.AuditCli.csproj -c Release -- --help
```

Show version:

```powershell
dotnet run --project tools/audit-cli/solution/PineGuard.AuditCli.csproj -c Release -- --version
```

## Recommended usage (via orchestrator)

Most of the time you should run audits via the orchestrator, which provides rule catalog output, filtering, and summary reporting:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/audit-cli/Run-All.ps1 -Configuration Release
```

Useful orchestrator options:

- Run a subset: `-Rule Rule06,Rule08` or `-RuleName Parity,Ordering`
- List rules: `-ListRules`
- Quiet output: `-NoCatalog -NoSummary`
- JSON summary: `-JsonSummary artifacts/audit/audit-summary.json`

## Repo tools that use this

These scripts are the main entrypoints under `tools/` that call into `PineGuard.AuditCli`:

### Orchestrator + rule wrappers

- `tools/audit-cli/Run-All.ps1`
  - Runs the full audit catalog, supports listing/filtering, and can emit JSON summaries.
- `tools/audit-cli/Run-AuditLibraryRules.ps1`
  - Runs the library/default subset (unless overridden via `-Rule` / `-RuleName`).
- `tools/audit-cli/Run-AuditTestingRules.ps1`
  - Runs the testing/default subset (unless overridden via `-Rule` / `-RuleName`).
- `tools/audit-cli/Run-AuditRules.ps1`
  - Compatibility shim for older automation that still points at the legacy entrypoint.
- `tools/audit-cli/rules/Load-Catalog.ps1`
  - Single source of truth for rule metadata (id/name/description/output paths).
- `tools/audit-cli/rules/Test-Rule01-Naming.ps1` .. `Test-Rule10-PsNormalization.ps1`
  - Thin wrappers that write headers and dispatch to the underlying audit scripts (library rules).
- `tools/audit-cli/rules/Test-Rule50-UnitTestFileStructureNormalization.ps1` .. `Test-Rule54-UnitTestTupleConventions.ps1`
  - Thin wrappers for testing rules.

### Underlying audit scripts (PowerShell)

The wrappers dispatch to scripts under `tools/audit-cli/helpers/`. These are also usable directly:

- `tools/audit-cli/helpers/Test-SpecNaming.ps1`
  - Runs the **naming** audit by executing `dotnet run --project tools/audit-cli/solution/PineGuard.AuditCli.csproj ...`.
- `tools/audit-cli/helpers/Test-SpecOrdering.ps1`
  - Runs the **ordering** audit by executing `dotnet run --project tools/audit-cli/solution/PineGuard.AuditCli.csproj ...`.

## Audits

### Ordering audit

Checks cross-layer method ordering parity (Rules/Must/Guard/FV/DA).

```powershell
dotnet run --project tools/audit-cli/solution/PineGuard.AuditCli.csproj -c Release -- \
  --audit ordering \
  --repoRoot . \
  --report artifacts/audit/Rule08-method-ordering-parity.txt
```

Options:

- `--report <path>` (required)
- `--repoRoot <path>` (optional; defaults to auto-detected)
- `--allowViolations true|false` (optional; default `false`)

### Naming audit

Validates MustClauses naming/nullability/collision policies using a JSON spec.

```powershell
dotnet run --project tools/audit-cli/solution/PineGuard.AuditCli.csproj -c Release -- \
  --audit naming \
  --project MustClauses \
  --spec artifacts/audit/naming-spec.json \
  --report artifacts/audit/Rule01-mustclauses-naming-and-collisions.json
```

Options:

- `--project <name>` (e.g. `MustClauses`)
- `--spec <path>` (default: `artifacts/audit/naming-spec.json`)
- `--report <path>`
- `--createSpecTemplate true|false`
- `--createSnapshot true|false` (if true, also writes `--snapshot`)
- `--snapshot <path>`
- `--repoRoot <path>` (optional; defaults to auto-detected)
- `--allowViolations true|false` (optional; default `false`)

## Notes

- This tool is intended to be **non-destructive**: it does not modify source files.
- Output should always be written under `artifacts/` (repo convention).
