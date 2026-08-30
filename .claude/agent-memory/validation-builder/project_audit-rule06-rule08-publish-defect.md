---
name: audit-rule06-rule08-publish-defect
description: audit-cli Rule06 and Rule08 currently fail on a dotnet publish multi-TFM defect, not on your code — do not chase it
metadata:
  type: project
---

`tools/audit-cli/Run-All.ps1 -RuleId Rule06` (and Rule08, which shares the helper) fails with
`NETSDK1129: The 'Publish' target is not supported without specifying a target framework` from
`tools/audit-cli/helpers/Test-ParityAgainstMust.ps1`, which calls `dotnet publish` on
`PineGuard.MustClauses.csproj` without `-f`. That project targets netstandard2.1 / net8.0 / net10.0.
Rule08 produces no report file at all as a result. Rule07, Rule13 and Rule50 run fine.

Also note the orchestrator requires **pwsh 7**, not Windows PowerShell 5.1 — under 5.1 it dies with
`Get-PineGuardAuditRule-Catalog : A positional parameter cannot be found that accepts argument 'Rules'`.

**Why:** observed 2026-08-30 while closing Plan 05 Batch D. It is a tooling defect independent of any
validation code, so a Batch-D-era agent seeing it must not conclude their layer broke parity.

**How to apply:** if a dispatch asks for the Rule06/Rule08 gate, report the failure as pre-existing
tooling and move on unless fixing audit-cli is in scope. Verify before relying on this — the fix is a
one-line `-f net8.0` addition someone may well have landed since.
