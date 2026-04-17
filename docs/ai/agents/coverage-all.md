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

Use the **Agent tool** to launch one `coverage-analyst` sub-agent per project **in parallel**.
Each sub-agent runs coverage for a single scope and reports back.

> [!NOTE]
> All coverage commands are read-only (Tier 2 — safe without confirmation).

## Steps

### 1. Launch parallel sub-agents

Spawn **all five** sub-agents in a **single message** (so they run concurrently):

| Sub-agent name | Scope | Command |
|:---|:---|:---|
| `coverage-core` | Core | `./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope Core -Top 30 -Isolated -SkipHtml` |
| `coverage-must` | MustClauses | `./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope MustClauses -Top 30 -Isolated -SkipHtml` |
| `coverage-guard` | GuardClauses | `./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope GuardClauses -Top 30 -Isolated -SkipHtml` |
| `coverage-fluent` | FluentValidation | `./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope FluentValidation -Top 30 -Isolated -SkipHtml` |
| `coverage-da` | DataAnnotations | `./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope DataAnnotations -Top 30 -Isolated -SkipHtml` |

**Sub-agent prompt template** (use `subagent_type: coverage-analyst`):

```
Run code coverage for PineGuard.[Project].

1. Run: `[command from table above]`
2. Parse the output for any classes below 100% line or branch coverage.
3. Report back with:
   - Overall line % and branch %
   - List of classes below 100% (file path, line %, branch %)
   - "PASS" if 100% across the board, "GAPS FOUND" if not
```

### 2. Collect results

Wait for all five sub-agents to complete. Collate their results into a single summary table:

| Project | Line % | Branch % | Status |
|:---|:---|:---|:---|
| Core | ... | ... | PASS / GAPS FOUND |
| MustClauses | ... | ... | PASS / GAPS FOUND |
| GuardClauses | ... | ... | PASS / GAPS FOUND |
| FluentValidation | ... | ... | PASS / GAPS FOUND |
| DataAnnotations | ... | ... | PASS / GAPS FOUND |

### 3. PineGuard.Testing (sequential — depends on All-scope data)

After the parallel runs complete, run Testing coverage:

```powershell
./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode Generate -Scope All -Isolated
```

Then analyze:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Custom -IncludeClassNameRegex "^PineGuard\.Testing\." -Top 30 -Enforce100
```

> [!NOTE]
> `PineGuard.Testing` has no own test runner. Its code is exercised via the five projects above,
> so its coverage data can only be collected via the `All` scope after those runs complete.

### 4. Final report

Add the Testing row to the summary table and present the consolidated result to the user.
If any project has gaps, list the specific classes below 100%.
