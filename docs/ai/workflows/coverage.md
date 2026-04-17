# Workflow: Run Coverage

> [!NOTE]
> Master workflow for executing code coverage analysis on any PineGuard component.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Skill**: [Run Coverage](../skills/run-coverage.md)
- **Spec**: [Code Coverage Spec](../specs/testing/coverage.md)

## Parameters

This workflow accepts a **Scope** parameter to determine which project to analyze.

| Scope              | Description                                                                                                       |
| :----------------- | :---------------------------------------------------------------------------------------------------------------- |
| `All`              | Runs coverage for the entire solution (all `*.UnitTests` projects, includes PineGuard.Testing execution data).    |
| `Core`             | PineGuard.Core                                                                                                    |
| `MustClauses`      | PineGuard.MustClauses                                                                                             |
| `GuardClauses`     | PineGuard.GuardClauses                                                                                            |
| `FluentValidation` | PineGuard.FluentValidation                                                                                        |
| `DataAnnotations`  | PineGuard.DataAnnotations                                                                                         |
| `Testing`          | PineGuard.Testing — use `All` scope to collect data, then analyze via `Custom` scope (see coverage spec for cmd). |

## Steps

## Auto-Approval

- **Gemini**: `// turbo-all`
- **Claude**: `Project Rules` allow coverage.
- **Cursor**: `cmd: powershell` allowed.

// turbo-all

1. **Execute Coverage Analysis**
   Run the coverage tool with the provided scope.

   **Command Template**:

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Engine xplat -Mode GenerateAndAnalyze -Scope [SCOPE] -Top 30 -Isolated
   ```

   **Examples**:
   - If Scope is `Core`: `... -Scope Core ...`
   - If Scope is `All`: `... -Scope All ...`

> [!IMPORTANT]
> Scope values must match `tools/code-coverage/Run-CodeCoverage.ps1`.

2. **Verify Results**
   Check the output for coverage gaps (Classes < 100%).
