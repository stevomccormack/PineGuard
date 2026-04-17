<!-- metadata_header
type: agent
id: agent-coverage-must
version: 1.0
-->

# Agent: Run Code Coverage for PineGuard.MustClauses

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: verifier ([../roles/verifier.md](../roles/verifier.md))

## Steps

1. **Context rehydration**
   - Read the project spec and coverage spec to ensure alignment with architectural standards.
   - Open:
     - `docs/ai/specs/must-clauses/project.md`
     - `docs/ai/specs/must-clauses/coverage.md`

2. **Execute coverage analysis**
   - Run the coverage analysis for MustClauses.
   - Notes:
     - `-Isolated`: prevents test hangs by running in a separate process.
     - `-Top 30`: focuses report on the most relevant files.
     - HTML report opens implicitly by omitting `-SkipHtml`.
   - Run:
     ```powershell
     ./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope MustClauses -Top 30 -Isolated
     ```

3. **Transition: analyze gaps**
   - If coverage is < 100%, follow `docs/ai/agents/audit-gap.md`.
