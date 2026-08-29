<!-- metadata_header
type: workflow
id: workflow-audit
version: 1.0
-->

# Workflow: Audit

> [!NOTE]
> Runs PineGuard's repo audits via the PowerShell wrappers under `tools/audit-cli/`.
> These audits check convention compliance and cross-layer mapping/parity.

## Context

- **Role**: [DevOps Engineer](../roles/shipper.md)
- **Reference**: `tools/audit-cli/Run-All.ps1` (compat: `tools/audit-cli/Run-AuditRules.ps1`)

## Rule08 Notes (ordering parity)

- MustClauses define the canonical concept ordering.
- GuardClauses are frequently named for forbidden states and implemented via Must complements; Rule08 compares Guard ordering using the **Must clause each Guard method invokes** (not the Guard method name).

## Parameters

- **Scope**: (`All`, `Library`, `Testing`) — implemented via wrapper scripts under `tools/audit-cli/`.
- **RuleId**: (optional, alias `-Rule`) any RuleId present in `tools/audit-cli/rules/Load-Catalog.ps1`.
  - Library rules: Rule01..Rule13
  - Testing rules: Rule50..Rule54
- **Configuration**: (`Debug`, `Release`) — used by rules that build/analyze compiled output
- **RepoRoot**: (optional) repo root path; defaults to auto-resolve
- **AllowViolations**: (optional switch) applies to rules that support policy allowlists (e.g., Rule07 + Rule08)

## CI Gate

`.github/workflows/ci.yml` runs `Run-All.ps1 -Configuration Release -RuleId Rule50` on every PR.
Rule50 (Theory-only + Tests/TestData pairing) is the only audit rule that gates merges, and a
Rule50 violation is a merge blocker — reproduce it locally before pushing:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RuleId Rule50
```

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Run all audit rules (recommended)**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RepoRoot "."
   ```

   Optional: allow violations for Rule07 (useful while tightening the policy):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RepoRoot "." -AllowViolations
   ```

   Library-only (default subset):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditLibraryRules.ps1" -Configuration Release -RepoRoot "."
   ```

   Testing-only (default subset):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditTestingRules.ps1" -Configuration Release -RepoRoot "."
   ```

2. **Run a single rule (when iterating)**

   Examples:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/rules/Test-Rule02-RulesUsage.ps1" -Configuration Release -RepoRoot "."
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/rules/Test-Rule07-Nullability.ps1" -RepoRoot "."
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/rules/Test-Rule08-Ordering.ps1" -RepoRoot "."
   ```

3. **Inspect outputs**
   - Rule output files are written under `artifacts/audit/` (e.g., `artifacts/audit/Rule02-rules-to-must-usage-scan.txt`).
   - Treat any reported violations as blocking unless the run explicitly used `-AllowViolations`.

4. **Triage + remediate**
   - Fix the highest-signal violations first (naming/collisions, missing mappings, parity).
   - Re-run the specific rule you’re iterating on, then re-run `Run-All.ps1` before finalizing.
