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
   - **Audit libraries (Rule01..Rule10)**

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditLibraryRules.ps1" -Configuration Release -RepoRoot "."
     ```

   - **Audit testing (Rule50..Rule54)**

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/Run-AuditTestingRules.ps1" -Configuration Release -RepoRoot "."
     ```

   - **Audit all (full suite)**

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
