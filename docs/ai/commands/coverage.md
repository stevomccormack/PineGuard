<!-- metadata_header
type: command
id: cmd-coverage
version: 1.0
-->

# Command: Coverage

> [!NOTE]
> **Interface Definition**: This file defines the explicit triggers and contracts for Code Coverage operations.

## Intent Mapping

Map these to your Agent's slash command palette:

| Command                | Scope              | Auto-Approve | Description                                                                                  |
| :--------------------- | :----------------- | :----------- | :------------------------------------------------------------------------------------------- |
| `/coverage-all`        | `All`              | ✅ Yes       | Run coverage for entire solution.                                                            |
| `/coverage-core`       | `Core`             | ✅ Yes       | Run coverage for PineGuard.Core.                                                             |
| `/coverage-must`       | `MustClauses`      | ✅ Yes       | Run coverage for MustClauses.                                                                |
| `/coverage-guard`      | `GuardClauses`     | ✅ Yes       | Run coverage for GuardClauses.                                                               |
| `/coverage-fluent`     | `FluentValidation` | ✅ Yes       | Run coverage for FluentValidation.                                                           |
| `/coverage-annotation` | `DataAnnotations`  | ✅ Yes       | Run coverage for DataAnnotations.                                                            |
| `/coverage-testing`    | `Testing`          | ✅ Yes       | Run coverage for PineGuard.Testing via `tests/PineGuard.Testing.UnitTests`.                  |

Every `Scope` value above is a first-class member of the `-Scope` ValidateSet in
`tools/code-coverage/Run-CodeCoverage.ps1`; there is no ad-hoc scope to construct by hand.

These commands **measure** coverage. The `/fix-coverage-*` family that closes the gaps they report is
contracted in [`fix.md`](fix.md) — it writes code and is never auto-approved.

## Execution

**Agent entrypoint**: `docs/ai/agents/coverage-[scope].md` (or `docs/ai/agents/coverage-all.md`)
**Notes**: the agent may call shared orchestration in `docs/ai/workflows/coverage.md`.

## Auto-Approval

- **Claude Code**: implicitly allowed via project context.
- **Antigravity**: `// turbo-all` active in the adapter stub `.agent/workflows/coverage-[scope].md`.
- **Pi**: `.pi/prompts/coverage-[scope].md`.
- **Copilot**: `.github/prompts/` carries one representative of this family (see
  [`../meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §4).
