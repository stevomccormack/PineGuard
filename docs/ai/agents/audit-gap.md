<!-- metadata_header
type: agent
id: agent-audit-gap
version: 1.0
-->

# Agent: Analyze Coverage Gaps (Autonomous)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: planner ([../roles/planner.md](../roles/planner.md)), verifier ([../roles/verifier.md](../roles/verifier.md))

## Layer map

Pick the layer the user named. If none was named, default to `MustClauses`.

| Layer | Coverage spec | Coverage filter | Remediation agent |
|-------|---------------|-----------------|-------------------|
| Core | [`../specs/core/coverage.md`](../specs/core/coverage.md) | `*PineGuard.Core.UnitTests*` | [`fix-coverage-core.md`](fix-coverage-core.md) |
| MustClauses | [`../specs/must-clauses/coverage.md`](../specs/must-clauses/coverage.md) | `*PineGuard.MustClauses.UnitTests*` | [`fix-coverage-must.md`](fix-coverage-must.md) |
| GuardClauses | [`../specs/guard-clauses/coverage.md`](../specs/guard-clauses/coverage.md) | `*PineGuard.GuardClauses.UnitTests*` | [`fix-coverage-guard.md`](fix-coverage-guard.md) |
| FluentValidation | [`../specs/fluent-validation/coverage.md`](../specs/fluent-validation/coverage.md) | `*PineGuard.FluentValidation.UnitTests*` | [`fix-coverage-fluent.md`](fix-coverage-fluent.md) |
| DataAnnotations | [`../specs/data-annotations/coverage.md`](../specs/data-annotations/coverage.md) | `*PineGuard.DataAnnotations.UnitTests*` | [`fix-coverage-annotation.md`](fix-coverage-annotation.md) |
| Testing | [`../specs/testing/coverage.md`](../specs/testing/coverage.md) | `*PineGuard.Testing.UnitTests*` | [`fix-coverage-testing.md`](fix-coverage-testing.md) |

## Steps

1. **Context rehydration**
   - Read the coverage spec for the chosen layer to understand reporting standards.

2. **Analyze coverage report**
   - Report every class in the latest Cobertura run below 100% line or branch coverage, using the vetted helper rather than an ad-hoc pipeline.
   - Run (substituting the layer's coverage filter):

     ```powershell
     pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/audit-cli/helpers/Test-CoverageLatest.ps1" -TargetFilter '*PineGuard.MustClauses.UnitTests*' -OutputPath 'artifacts/audit/util/audit-gap-latest-coverage.txt'
     ```

   - For a richer per-class table instead of a flat report, use `tools/code-coverage/xplat/Test-CoverageAnalysis.ps1 -Scope <Layer> -AsTable`.
   - If no report is found, generate one first via the matching `/coverage-<layer>` agent.

3. **Transition: remediate**
   - Proceed to fix the identified gaps.
   - Follow the remediation agent for the chosen layer from the table above.
