<!-- metadata_header
type: command
id: cmd-fix
version: 1.0
-->

# Command: Fix

> [!NOTE]
> **Interface Definition**: These commands change code. None of them is auto-approved — each runs an
> analyse/repair/re-verify loop that requires AI judgement.

## Intent Mapping

Map these to your Agent's slash command palette:

### Coverage gaps

| Command                    | Scope              | Auto-Approve | Description                               |
| :------------------------- | :----------------- | :----------- | :---------------------------------------- |
| `/fix-coverage-all`        | `All`              | ❌ No        | Close coverage gaps across the solution.  |
| `/fix-coverage-core`       | `Core`             | ❌ No        | Close coverage gaps in PineGuard.Core.    |
| `/fix-coverage-must`       | `MustClauses`      | ❌ No        | Close coverage gaps in MustClauses.       |
| `/fix-coverage-guard`      | `GuardClauses`     | ❌ No        | Close coverage gaps in GuardClauses.      |
| `/fix-coverage-fluent`     | `FluentValidation` | ❌ No        | Close coverage gaps in FluentValidation.  |
| `/fix-coverage-annotation` | `DataAnnotations`  | ❌ No        | Close coverage gaps in DataAnnotations.   |
| `/fix-coverage-testing`    | `Testing`          | ❌ No        | Close coverage gaps in PineGuard.Testing. |

### Test failures

| Command                | Scope              | Auto-Approve | Description                                       |
| :--------------------- | :----------------- | :----------- | :------------------------------------------------ |
| `/fix-test-all`        | `All`              | ❌ No        | Diagnose and fix failing tests solution-wide.     |
| `/fix-test-core`       | `Core`             | ❌ No        | Diagnose and fix failing PineGuard.Core tests.    |
| `/fix-test-must`       | `MustClauses`      | ❌ No        | Diagnose and fix failing MustClauses tests.       |
| `/fix-test-guard`      | `GuardClauses`     | ❌ No        | Diagnose and fix failing GuardClauses tests.      |
| `/fix-test-fluent`     | `FluentValidation` | ❌ No        | Diagnose and fix failing FluentValidation tests.  |
| `/fix-test-annotation` | `DataAnnotations`  | ❌ No        | Diagnose and fix failing DataAnnotations tests.   |
| `/fix-test-testing`    | `Testing`          | ❌ No        | Diagnose and fix failing PineGuard.Testing tests. |

### Static-analysis findings

| Command               | Target             | Auto-Approve | Description                                          |
| :-------------------- | :----------------- | :----------- | :--------------------------------------------------- |
| `/fix-roslyn-all`     | Compiler warnings  | ❌ No        | Fix Roslyn CS warnings at the root cause.            |
| `/fix-sonar-all`      | SonarQube issues   | ❌ No        | Fix all SonarQube findings.                          |
| `/fix-sonar-blocker`  | SonarQube blockers | ❌ No        | Fix blocker-severity findings only.                  |
| `/fix-sonar-high`     | SonarQube high     | ❌ No        | Fix high-severity findings only.                     |
| `/fix-sonar-medium`   | SonarQube medium   | ❌ No        | Fix medium-severity findings only.                   |
| `/fix-sonar-low`      | SonarQube low      | ❌ No        | Fix low-severity findings only.                      |

Findings are always fixed at the root cause — suppressing a diagnostic is not a fix.

> [!NOTE]
> The Sonar family is deliberately **severity-scoped** rather than project-scoped: SonarQube
> analyses the whole solution (see [`scan.md`](scan.md)), and its issue API filters by severity,
> so remediation batches follow the same axis. The Roslyn and coverage families are
> project-scoped because their tooling is.

## Execution

| Family | Agent entrypoint | Shared orchestration |
|--------|------------------|----------------------|
| Coverage | `docs/ai/agents/fix-coverage-{scope}.md` | `docs/ai/workflows/fix-coverage.md` |
| Tests | `docs/ai/agents/fix-test-{scope}.md` | `docs/ai/workflows/fix-test.md` |
| Roslyn | `docs/ai/agents/fix-roslyn-all.md` | `docs/ai/workflows/fix-roslyn.md` |
| Sonar | `docs/ai/agents/fix-sonar-{severity}.md` | `docs/ai/workflows/fix-sonar.md` |

Run the matching read-only command first — [`coverage.md`](coverage.md), [`test.md`](test.md) or
[`scan.md`](scan.md) — so the fix loop starts from a current report.

## Auto-Approval

- These workflows are **interactive**. The sub-commands they invoke (`dotnet test`, the coverage and
  scan scripts) may be auto-approved, but the repair loop itself requires explicit user intent.
