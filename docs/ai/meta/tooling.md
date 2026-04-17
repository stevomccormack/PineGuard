<!-- metadata_header
type: meta
id: ai-tooling-alignment
version: 1.0
-->

# AI Tooling Alignment (GitHub + Microsoft)

> [!IMPORTANT]
> This document maps the portable Brain (`docs/ai/**`) to common enterprise tooling:
> GitHub (Issues/Projects/PRs/Wiki/Actions), VS Code tasks, and multiple AI assistants.

## Single Source of Truth

- **Rules / constraints**: `docs/ai/specs/**`
- **Reusable procedures**: `docs/ai/skills/**` and `docs/ai/workflows/**`
- **Canonical entrypoints**: `docs/ai/agents/**`
- **Interface contracts**: `docs/ai/commands/**`
- **Adapters** (thin pointers): `.github/copilot-instructions.md`, `CLAUDE.md`, `.agent/workflows/*.md`

## GitHub-First Operating Model

Treat GitHub as the system of record:

- **Planning / tracking**: GitHub Issues + Projects
- **Execution & review**: Pull Requests (traceable to Issues)
- **Decisions / durable knowledge**: GitHub Wiki or repo docs
- **Automation**: GitHub Actions

AI output is only “done” when it becomes a reviewable PR-level artifact and the checks pass.

## VS Code Tasks (.vscode/tasks.json)

VS Code tasks are the primary local automation surface.

- File: `.vscode/tasks.json`
- Guidance:
  - Keep task labels stable (they become user muscle memory and tool triggers).
  - Tasks should call repo scripts (e.g., `tools/testing/Run-Tests.ps1`) rather than embedding complex logic.
  - Prefer tasks that map 1:1 with `docs/ai/commands/*.md` “Command contracts”.

Recommended DRY mapping:

- `docs/ai/commands/test.md` ↔ tasks labelled `Test:*`
- `docs/ai/commands/coverage.md` ↔ tasks labelled `Coverage:*`
- `docs/ai/commands/fix.md` ↔ tasks labelled `Debug:*` (often interactive; not auto-approved)

## Copilot / Claude / Gemini / Other Agents

Adapters must remain thin and point to canonical agents.

- GitHub Copilot adapter: `.github/copilot-instructions.md`
- Claude adapter: `CLAUDE.md`
- Gemini adapter stubs: `.agent/workflows/*.md`

Best practice for portability (Claude Code / other tools):

- Keep the **Brain** free of tool-specific syntax.
- Put “trigger language” and auto-approval annotations in adapters.
- Use `docs/ai/commands/*.md` as the shared interface layer that adapters can implement.

## GitHub CLI, GitHub MCP, and Automation

When tooling allows, prefer:

- GitHub CLI (`gh`) for repeatable repo operations
- GitHub MCP for tool-driven automation (where available)

But keep the canonical “what to do” in Brain docs; tools are just execution surfaces.

## Quality Gates (Industry Standard)

Use automated gates that fit enterprise expectations:

- Tests: `dotnet test` (targeted projects first)
- Coverage: cross-platform collection with Cobertura output where applicable
- Inspection: JetBrains Qodana (static analysis) integrated into CI
- Auditing: repo audit CLI/tasks as defined in `tools/` and referenced by workflow docs

## Adding a New Capability (Best Practice)

Use this order so the system stays DRY and portable:

1. **Spec** (if new rules/structure needed): add/extend under `docs/ai/specs/**`
2. **Skill** (reusable how-to): add under `docs/ai/skills/**`
3. **Workflow** (orchestration): add under `docs/ai/workflows/**`
4. **Agent** (canonical entrypoint): add under `docs/ai/agents/**`
5. **Command contract** (interface mapping): add under `docs/ai/commands/**`
6. **Adapters**: wire the command/agent into Copilot/Claude/Gemini stubs
7. **VS Code task**: optionally add a matching `.vscode/tasks.json` task

## References

- Taxonomy: `docs/ai/meta/taxonomy.md`
- Universal Protocol: `docs/ai/specs/protocol.md`
- Root Spec precedence: `docs/ai/specs/spec.md`

<!-- footer
last_verified: 2026-02-05
-->
