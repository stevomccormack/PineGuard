<!-- metadata_header
type: workflow
id: workflow-coverage
version: 1.0
-->

# Workflow: Coverage

> [!NOTE]
> Master workflow for executing code coverage analysis on any PineGuard component.

## Context

- **Role**: [Test Engineer](../roles/verifier.md)
- **Skill**: [Improve Code Coverage](../skills/improve-coverage/SKILL.md)
- **Spec**: [Code Coverage Spec](../specs/testing/coverage.md)

## Parameters

This workflow accepts a **Scope** parameter to determine which project to analyze.

| Scope              | Description                                                                                                       |
| :----------------- | :---------------------------------------------------------------------------------------------------------------- |
| `All`              | Runs coverage for the entire solution (every `*.UnitTests` project).                                              |
| `Core`             | PineGuard.Core                                                                                                    |
| `MustClauses`      | PineGuard.MustClauses                                                                                             |
| `GuardClauses`     | PineGuard.GuardClauses                                                                                            |
| `FluentValidation` | PineGuard.FluentValidation                                                                                        |
| `DataAnnotations`  | PineGuard.DataAnnotations                                                                                         |
| `Testing`          | PineGuard.Testing — run directly with `-Scope Testing`.                                                           |

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow coverage.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Execute Coverage Analysis**
   Run the coverage tool with the provided scope.

   **Command Template**:

   ```powershell
   ./tools/code-coverage/Run-CodeCoverage.ps1 -Mode GenerateAndAnalyze -Scope [SCOPE] -Top 30
   ```

   **Examples**:
   - If Scope is `Core`: `... -Scope Core ...`
   - If Scope is `All`: `... -Scope All ...`

> [!IMPORTANT]
> Scope values must match `tools/code-coverage/Run-CodeCoverage.ps1`.

2. **Verify Results**
   Check the output for coverage gaps (Classes < 100%).
