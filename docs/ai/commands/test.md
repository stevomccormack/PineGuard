<!-- metadata_header
type: command
id: cmd-test
version: 1.0
-->

# Command: Test

> [!NOTE]
> **Interface Definition**: This file defines the explicit triggers and contracts for Unit Test execution.

## Intent Mapping

Map these to your Agent's slash command palette:

| Command            | Scope              | Auto-Approve | Description                                                                              |
| :----------------- | :----------------- | :----------- | :--------------------------------------------------------------------------------------- |
| `/test-all`        | `All`              | ✅ Yes       | Run tests for all projects (sequential).                                                 |
| `/test-core`       | `Core`             | ✅ Yes       | Run tests for PineGuard.Core.                                                            |
| `/test-must`       | `MustClauses`      | ✅ Yes       | Run tests for MustClauses.                                                               |
| `/test-guard`      | `GuardClauses`     | ✅ Yes       | Run tests for GuardClauses.                                                              |
| `/test-fluent`     | `FluentValidation` | ✅ Yes       | Run tests for FluentValidation.                                                          |
| `/test-annotation` | `DataAnnotations`  | ✅ Yes       | Run tests for DataAnnotations.                                                           |
| `/test-testing`    | `Testing`          | ✅ Yes       | Run `tests/PineGuard.Testing.UnitTests/PineGuard.Testing.UnitTests.csproj`.               |

`tests/PineGuard.Testing/` is the shared test infrastructure library consumed by every `*.UnitTests`
project; its own tests live in `tests/PineGuard.Testing.UnitTests/` and are run by `/test-testing`.

These commands **run** the tests. The `/fix-test-*` family that diagnoses and repairs the failures
they report is contracted in [`fix.md`](fix.md) — it writes code and is never auto-approved.

## Execution

**Agent entrypoint**: `docs/ai/agents/test-[scope].md` (or `docs/ai/agents/test-all.md`)
**Notes**: the agent may call shared orchestration in `docs/ai/workflows/test.md`.

## Auto-Approval

- **Claude Code**: implicitly allowed via project context.
- **Antigravity**: `// turbo-all` active in the adapter stub `.agent/workflows/test-[scope].md`.
- **Pi**: `.pi/prompts/test-[scope].md`.
- **Copilot**: `.github/prompts/` carries one representative of this family (see
  [`../meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §4).
