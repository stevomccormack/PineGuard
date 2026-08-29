<!-- metadata_header
type: agent
id: agent-audit-cli
version: 1.0
-->

# Agent: Run Audit CLI (Library / Testing / All)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/audit.md`.

2. Choose the scope and execute the matching wrapper (recommended defaults):
   - **Reproduce the CI gate (Rule50 only)** — start here; this is the only invocation that blocks a merge (see [CI parity](#ci-parity)).

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RepoRoot "." -RuleId Rule50
     ```

   - **Audit libraries (Rule01..Rule10, Rule13)** — Rule01 fails on a fresh checkout (it needs a `naming-spec.json` bootstrap that the repo does not ship); treat its output as noise, not a regression.

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditLibraryRules.ps1" -Configuration Release -RepoRoot "."
     ```

   - **Audit testing (Rule50..Rule54)** — Rule51-54 currently report 3000+ pre-existing findings (nested-group structure, tuple naming, orphan heuristics). That is known debt, not something this run introduced.

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditTestingRules.ps1" -Configuration Release -RepoRoot "."
     ```

   - **Audit all (full suite)** — inherits both caveats above; useful for a debt survey, not for deciding whether a change is mergeable.

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RepoRoot "."
     ```

3. Optional iteration flags (useful while tightening policy or diagnosing failures):
   - **Show failures + keep going**:

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RepoRoot "." -ContinueOnError -ShowFailures
     ```

   - **Emit JSON summary**:

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-All.ps1" -Configuration Release -RepoRoot "." -JsonSummary "artifacts/audit/audit-summary.json"
     ```

## CI parity

Pull requests are gated on **Rule50 only** (Theory-only tests + `Tests`/`TestData` pairing), with the two legitimate pre-existing exceptions allowlisted in [`tools/audit-cli/test-audit-exceptions.json`](../../../tools/audit-cli/test-audit-exceptions.json). Everything else in the suite runs locally but does not block a merge:

| Rules | Merge-blocking | Why |
|-------|----------------|-----|
| Rule50 | ✓ | Verified clean; this is the CI gate. |
| Rule01 | | Requires a `naming-spec.json` bootstrap that is not in the repo and not in `Run-All.ps1`'s default flow, so it fails on every fresh checkout. |
| Rule51–Rule54 | | Carry 3000+ pre-existing findings — real debt, tracked as a separate remediation effort. |

So do not report Rule01 or Rule51–54 output as a regression introduced by the change under audit. The gate widens once that debt is remediated; the CI job is section 7 of [`.github/workflows/ci.yml`](../../../.github/workflows/ci.yml).
