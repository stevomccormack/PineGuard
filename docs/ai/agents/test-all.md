<!-- metadata_header
type: agent
id: agent-test-all
version: 2.0
-->

# Agent: Run Unit Tests for All Projects

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: verifier ([../roles/verifier.md](../roles/verifier.md))

## Strategy

Use the **Agent tool** to launch one sub-agent per test project **in parallel**.
Each sub-agent runs tests for a single project and reports pass/fail + failure details.

> [!NOTE]
> All test commands are read-only (Tier 2 — safe without confirmation).

## Steps

### 1. Launch parallel sub-agents

Spawn **all five** sub-agents in a **single message** (so they run concurrently):

| Sub-agent name | Project | Command |
|:---|:---|:---|
| `test-core` | Core | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj"` |
| `test-must` | MustClauses | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.MustClauses.UnitTests/PineGuard.MustClauses.UnitTests.csproj"` |
| `test-guard` | GuardClauses | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.GuardClauses.UnitTests/PineGuard.GuardClauses.UnitTests.csproj"` |
| `test-fluent` | FluentValidation | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.FluentValidation.UnitTests/PineGuard.FluentValidation.UnitTests.csproj"` |
| `test-da` | DataAnnotations | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.DataAnnotations.UnitTests/PineGuard.DataAnnotations.UnitTests.csproj"` |

**Sub-agent prompt template** (use `subagent_type: general-purpose`):

```
Run unit tests for PineGuard.[Project].

1. Run: `[command from table above]`
2. Parse the output for test results.
3. Report back with:
   - Total tests, passed, failed, skipped
   - If any failures: the full failure details (test name, error message, stack trace)
   - "PASS" if all green, "FAILURES" if any failed
```

### 2. Collect results

Wait for all five sub-agents to complete. Collate their results into a single summary table:

| Project | Total | Passed | Failed | Skipped | Status |
|:---|:---|:---|:---|:---|:---|
| Core | ... | ... | ... | ... | PASS / FAILURES |
| MustClauses | ... | ... | ... | ... | PASS / FAILURES |
| GuardClauses | ... | ... | ... | ... | PASS / FAILURES |
| FluentValidation | ... | ... | ... | ... | PASS / FAILURES |
| DataAnnotations | ... | ... | ... | ... | PASS / FAILURES |

### 3. PineGuard.Testing (build verification)

> [!NOTE]
> `PineGuard.Testing` is a shared test infrastructure library with no test methods.
> It is built automatically as a dependency of all `*.UnitTests` projects above.
> If all five sub-agents succeed, Testing is implicitly verified.

If any sub-agent reported a build failure, note that Testing may also be affected.

### 4. Final report

Present the consolidated summary table to the user.
If any project has failures, list the specific failing tests with error details.
