<!-- metadata_header
type: agent
id: agent-coverage-all
version: 2.0
-->

# Agent: Run Code Coverage for All Projects

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: verifier ([../roles/verifier.md](../roles/verifier.md))

## Strategy

Run one coverage pass per scope. If the host supports parallel sub-agents, launch them
concurrently — one per scope; otherwise run the commands sequentially in the order listed.
Each run covers a single scope and reports back.

> [!NOTE]
> All coverage commands are read-only (Tier 2 — safe without confirmation).

## Steps

### 1. Run the five layer scopes

Start **all five** concurrently where the host allows it:

| Label | Scope | Command |
|:---|:---|:---|
| `coverage-core` | Core | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Core -Top 30 -Isolated -SkipHtml` |
| `coverage-must` | MustClauses | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope MustClauses -Top 30 -Isolated -SkipHtml` |
| `coverage-guard` | GuardClauses | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope GuardClauses -Top 30 -Isolated -SkipHtml` |
| `coverage-fluent` | FluentValidation | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope FluentValidation -Top 30 -Isolated -SkipHtml` |
| `coverage-annotation` | DataAnnotations | `pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope DataAnnotations -Top 30 -Isolated -SkipHtml` |

The **Label** column only identifies the run in your summary table — it is not a sub-agent type.
On a host that fans work out to sub-agents, every row uses the same worker; see the host's own
adapter for the worker name.

**Per-scope brief** (hand this to each worker, or follow it yourself once per scope):

```
Run code coverage for PineGuard.[Scope].

1. Run: `[command from table above]`
2. Parse the output for any classes below 100% line or branch coverage.
3. Report back with:
   - Overall line % and branch %
   - List of classes below 100% (file path, line %, branch %)
   - "PASS" if 100% across the board, "GAPS FOUND" if not
```

### 2. Collect results

Wait for all five runs to complete. Collate their results into a single summary table:

| Project | Line % | Branch % | Status |
|:---|:---|:---|:---|
| Core | ... | ... | PASS / GAPS FOUND |
| MustClauses | ... | ... | PASS / GAPS FOUND |
| GuardClauses | ... | ... | PASS / GAPS FOUND |
| FluentValidation | ... | ... | PASS / GAPS FOUND |
| DataAnnotations | ... | ... | PASS / GAPS FOUND |

### 3. PineGuard.Testing

Run the Testing scope last:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Testing -Top 30 -Isolated -SkipHtml
```

> [!NOTE]
> The `Testing` scope runs every `*.UnitTests` project and filters the report down to
> `[PineGuard.Testing]*`, so its numbers combine its own suite in
> `tests/PineGuard.Testing.UnitTests` with the incidental exercise the five layer suites give it.
> It runs last because it rebuilds all test projects.

### 4. Final report

Add the Testing row to the summary table and present the consolidated result to the user.
If any project has gaps, list the specific classes below 100%.
