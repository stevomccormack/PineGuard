<!-- metadata_header
type: agent
id: agent-audit-gap
version: 1.0
-->

# Agent: Analyze Coverage Gaps for MustClauses (Autonomous)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: planner ([../roles/planner.md](../roles/planner.md)), verifier ([../roles/verifier.md](../roles/verifier.md))

## Steps

1. **Context rehydration**
   - Read the coverage spec to understand reporting standards.
   - Open `docs/ai/specs/must-clauses/coverage.md`.

2. **Analyze coverage report**
   - Query the latest Cobertura report for specific gaps (focused on RangeClauses).
   - Run:
     ```powershell
     $path = "artifacts/code-coverage/xplat/testresults/PineGuard.MustClauses.UnitTests"; $latest = Get-ChildItem $path -Recurse -Filter "coverage.cobertura.xml" | Sort-Object LastWriteTime -Descending | Select-Object -First 1; [xml]$xml = Get-Content $latest.FullName; $xml.coverage.packages.package.classes.class | Where-Object { ([double]$_.'line-rate' -lt 1.0 -or [double]$_.'branch-rate' -lt 1.0) } | Select-Object name, 'line-rate', 'branch-rate' | ConvertTo-Json
     ```

3. **Transition: remediate**
   - Proceed to fix the identified gaps.
   - Follow `docs/ai/agents/fix-coverage-must.md`.
