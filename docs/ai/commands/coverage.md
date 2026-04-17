# Command: Coverage

> [!NOTE]
> **Interface Definition**: This file defines the explicit triggers and contracts for Code Coverage operations.

## 1. Triggers (Slash Commands)

Map these to your Agent's slash command palette:

| Command            | Scope              | Auto-Approve | Description                                                                                  |
| :----------------- | :----------------- | :----------- | :------------------------------------------------------------------------------------------- |
| `/coverage-all`    | `All`              | ✅ Yes       | Run coverage for entire solution (includes PineGuard.Testing execution data).                |
| `/coverage-core`   | `Core`             | ✅ Yes       | Run coverage for PineGuard.Core.                                                             |
| `/coverage-must`   | `MustClauses`      | ✅ Yes       | Run coverage for MustClauses.                                                                |
| `/coverage-guard`  | `GuardClauses`     | ✅ Yes       | Run coverage for GuardClauses.                                                               |
| `/coverage-fluent` | `FluentValidation` | ✅ Yes       | Run coverage for FluentValidation.                                                           |
| `/coverage-data`   | `DataAnnotations`  | ✅ Yes       | Run coverage for DataAnnotations.                                                            |

> **PineGuard.Testing** has no dedicated `/coverage-testing` command. It is a shared test infrastructure library with no own test runner. Analyze its coverage via `/coverage-all` then use `Custom` scope — see `docs/ai/specs/testing/coverage.md` for the exact command.

## 2. Execution Logic

**Agent entrypoint**: `docs/ai/agents/coverage-[scope].md` (or `docs/ai/agents/coverage-all.md`)
**Notes**: the agent may call shared orchestration in `docs/ai/workflows/coverage.md`.

## 3. Auto-Approval Rules

- **Gemini**: `// turbo-all` active in the adapter stub `.agent/workflows/coverage-[scope].md`.
- **Claude**: Implicitly allowed via Project context.
- **Cursor**: `cmd: dotnet test` allowed in `.cursorrules`.
