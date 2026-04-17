# Command: Tests

> [!NOTE]
> **Interface Definition**: This file defines the explicit triggers and contracts for Unit Test execution.

## 1. Triggers (Slash Commands)

Map these to your Agent's slash command palette:

| Command        | Scope              | Auto-Approve | Description                                                                              |
| :------------- | :----------------- | :----------- | :--------------------------------------------------------------------------------------- |
| `/test-all`    | `All`              | ✅ Yes       | Run tests for all projects (sequential); builds PineGuard.Testing as a dependency.      |
| `/test-core`   | `Core`             | ✅ Yes       | Run tests for PineGuard.Core.                                                            |
| `/test-must`   | `MustClauses`      | ✅ Yes       | Run tests for MustClauses.                                                               |
| `/test-guard`  | `GuardClauses`     | ✅ Yes       | Run tests for GuardClauses.                                                              |
| `/test-fluent` | `FluentValidation` | ✅ Yes       | Run tests for FluentValidation.                                                          |
| `/test-data`   | `DataAnnotations`  | ✅ Yes       | Run tests for DataAnnotations.                                                           |

> **PineGuard.Testing** (`tests/PineGuard.Testing/`) is the **shared test infrastructure library**. It has no own test methods and no `/test-testing` command. It is built automatically as a dependency of all `*.UnitTests` projects. To verify it compiles cleanly, use the solution run: `Run-Tests.ps1 -Solution "./PineGuard.slnx"`.

## 2. Execution Logic

**Agent entrypoint**: `docs/ai/agents/test-[scope].md` (or `docs/ai/agents/test-all.md`)
**Notes**: the agent may call shared orchestration in `docs/ai/workflows/test.md`.

## 3. Auto-Approval Rules

- **Gemini**: `// turbo-all` active in the adapter stub `.agent/workflows/test-[scope].md`.
- **Claude**: Implicitly allowed via Project context.
- **Cursor**: `cmd: dotnet test` allowed in `.cursorrules`.
