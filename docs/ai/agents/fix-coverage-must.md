<!-- metadata_header
type: agent
id: agent-fix-coverage-must
version: 1.0
-->

# Agent: Fix coverage gaps for MustClauses (Autonomous)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: owner ([../roles/owner.md](../roles/owner.md)), verifier ([../roles/verifier.md](../roles/verifier.md))

## Steps

1. **Context rehydration**
   - Read the project spec and unit test spec to ensure alignment with architectural standards.
   - Open:
     - `docs/ai/specs/must-clauses/project.md`
     - `docs/ai/specs/must-clauses/unit-test.md`

2. **Gap remediation strategy**
   - Identify classes with < 100% coverage from the latest report.
   - Analyze source code to understand missing branches.
   - Add targeted test cases in `PineGuard.MustClauses.UnitTests`.

3. **Transition: verify fixes**
   - After implementing tests, run coverage again to verify the fix.
   - Follow `docs/ai/agents/coverage-must.md`.
