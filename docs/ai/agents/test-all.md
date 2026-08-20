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

Run one pass per test project. If the host supports parallel sub-agents, launch them
concurrently — one per project; otherwise run them sequentially.
Each run covers a single project and reports pass/fail + failure details.

> [!NOTE]
> All test commands are read-only (Tier 2 — safe without confirmation).

## Steps

### 1. Launch one run per project

Start **all six** concurrently where the host allows it:

| Label | Scope |
|:---|:---|
| `test-core` | Core |
| `test-must` | MustClauses |
| `test-guard` | GuardClauses |
| `test-fluent` | FluentValidation |
| `test-annotation` | DataAnnotations |
| `test-testing` | Testing |

Resolve each scope to its `.csproj` using the **Project map** in
[`../workflows/test.md`](../workflows/test.md); the command template is
`pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "[TEST_PROJECT]"`.

The **Label** column only identifies the run in your summary table — it is not a sub-agent type.

**Per-project brief** (hand this to each worker, or follow it yourself once per project):

```
Run unit tests for PineGuard.[Scope].

1. Run: `[command resolved from the Project map]`
2. Parse the output for test results.
3. Report back with:
   - Total tests, passed, failed, skipped
   - If any failures: the full failure details (test name, error message, stack trace)
   - "PASS" if all green, "FAILURES" if any failed
```

### 2. Collect results

Wait for all six runs to complete. Collate their results into a single summary table:

| Project | Total | Passed | Failed | Skipped | Status |
|:---|:---|:---|:---|:---|:---|
| Core | ... | ... | ... | ... | PASS / FAILURES |
| MustClauses | ... | ... | ... | ... | PASS / FAILURES |
| GuardClauses | ... | ... | ... | ... | PASS / FAILURES |
| FluentValidation | ... | ... | ... | ... | PASS / FAILURES |
| DataAnnotations | ... | ... | ... | ... | PASS / FAILURES |
| Testing | ... | ... | ... | ... | PASS / FAILURES |

> [!NOTE]
> `tests/PineGuard.Testing/` is the shared test-infrastructure library, built automatically as a
> dependency of every `*.UnitTests` project. `tests/PineGuard.Testing.UnitTests/` is the test
> project that exercises it, and it is what the `Testing` scope runs.

### 3. Final report

Present the consolidated summary table to the user.
If any project has failures, list the specific failing tests with error details.
