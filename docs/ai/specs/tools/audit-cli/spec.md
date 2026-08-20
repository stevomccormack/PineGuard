---
spec:
  id: pineguard.ai.tools.audit-cli.spec
  title: "PineGuard Audit CLI Specification"
  version: 1
  template:
    - ../../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "tools/audit-cli/**"
---

# PineGuard Audit CLI Specification

This specification details the structure and operation of the `audit-cli` toolset, used for static analysis, parity checking, and specification enforcement.

## 1. Directory Structure

- **Location**: `tools/audit-cli/`
- **Rules**: `tools/audit-cli/rules/` contains rule wrappers (e.g., `Test-Rule01-Naming.ps1`, `Test-Rule02-RulesUsage.ps1`).
- **Rule Catalog**: `tools/audit-cli/rules/Load-Catalog.ps1` is the single source of truth for rule metadata (id/name/description/output).
- **Orchestrators**: `tools/audit-cli/` (folder root) contains top-level entrypoints (e.g., `Run-All.ps1`).
- **Implementations**: `tools/audit-cli/helpers/` contains the underlying finder/tester scripts (e.g., `Find-UnusedRules.ps1`, `Test-SpecNaming.ps1`).
- **Utils**: `tools/audit-cli/utils/` contains utility scripts used by the wrappers.
- **Shared Helpers**: Common helpers live under `tools/audit-cli/helpers/` (e.g., `tools/audit-cli/helpers/Load-AuditHelpers.ps1`).

## 2. Naming Standards

Follows `docs/ai/specs/tools/spec.md` with specific extensions:

- **Rule Scripts**: `Test-Rule01-Naming.ps1`, `Test-Rule08-Ordering.ps1`, etc.
  - Use a zero-padded rule number (01, 02..).
  - Use a descriptive name (e.g., `Naming`, `Ordering`, `Parity`).
- **Core Orchestrator**: `Run-All.ps1`.
- **Library Orchestrator**: `Run-AuditLibraryRules.ps1`.
- **Testing Orchestrator**: `Run-AuditTestingRules.ps1`.
- **Compatibility**: `Run-AuditRules.ps1` remains as a shim for older tasks/docs.

## 3. Rule Implementation Pattern

Each `Test-RuleXX` script is a wrapper that:

1. Accepts standard parameters (`RepoRoot`, `Configuration`, etc.).
2. Imports helpers (`Load-AuditHelpers.ps1`).
3. Sets up output paths in `artifacts/audit/`.
4. Writes the standard Audit Header (`Write-PineGuardAuditHeader`).
5. Invokes the actual "Finder" or "Tester" script (e.g., `Test-SpecNaming.ps1` or `Find-UnusedMustGuard.ps1`).

To avoid drift, wrappers should source rule `OutputPath` (and other stable metadata) from the centralized catalog:

- `tools/audit-cli/rules/Load-Catalog.ps1`

```powershell
[CmdletBinding()]
param(...)
. (Join-Path $PSScriptRoot '..\helpers\Load-AuditHelpers.ps1')
$outputPath = 'artifacts/audit/RuleXX-name.json'
Write-PineGuardAuditHeader ...
& (Join-Path $PSScriptRoot '..\helpers\Test-SpecNaming.ps1') @args
```

## 3.1 Shared vocabulary map (required)

Some audits compare concepts across projects where the **preferred public wording** may differ by surface:

- GuardClauses uses forbidden-state names derived from Must, typically `Not*` complements (e.g. `NotEmail`, `NotHttpsUrl`).
- FluentValidation often uses positive concept names like `Email`, `HttpsUrl`.

To keep parity checks stable, audits must normalize method names to shared **concepts** using:

- `docs/ai/specs/language/vocabulary.json`

Example: Rule06 (Adapters ↔ Must parity) consumes this JSON and compares **normalized concept sets** rather than raw method names.

Clarification:

- GuardClauses and FluentValidation are expected to match MustClauses **exactly** at the concept level.
- DataAnnotations is validated as an adapter of MustClauses by enforcing **no unknown concepts** (extra concepts vs Must). Missing coverage vs Must is reported for visibility but is not enforced by default.

## 3.2 Parity scope: concepts over file structure (required)

Audit parity MUST be evaluated primarily at the **public API concept** level.

Clarifications:

- GuardClauses and MustClauses are expected to use internal domain folders + a public facade aggregator.
- FluentValidation and DataAnnotations are adapter surfaces and may **aggregate** by integration shape:
  - FluentValidation: extension classes under `src/PineGuard.FluentValidation/Extensions/**`.
  - DataAnnotations: domain-aggregated files at the project root.

Therefore, audits must not treat the absence of domain folders inside adapter projects as a parity failure.

## 3.3 PowerShell normalization expectations (required)

All audit-cli scripts (wrappers + helpers) must comply with the PowerShell parse-safety rules defined in `docs/ai/specs/tools/spec.md`.

## 4. Usage

For full usage examples, parameters, rule catalog, and directory structure, see:

- `tools/audit-cli/README.md` (source of truth for operational documentation)

Allowlisted exceptions live in:

- `tools/audit-cli/test-audit-exceptions.json`

## 5. Artifacts

- All audit output goes to `artifacts/audit/`.
- Format is typically JSON or structured Text.
- Filenames should follow `RuleXX-description.<ext>`.
