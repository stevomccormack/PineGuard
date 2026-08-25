<!-- metadata_header
type: meta
id: ai-tooling-alignment
version: 1.1
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
- **Adapters** (thin pointers): every surface is inventoried in `docs/ai/meta/adapter-surfaces.md`,
  which owns the count and the tiering. Never maintain a second list here.

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

Recommended DRY mapping (label families as they exist in `.vscode/tasks.json`):

| Label family | Command contract | Notes |
|---|---|---|
| `Test:*` | `docs/ai/commands/test.md` | Per-layer and sequential runs |
| `Coverage:*` | `docs/ai/commands/coverage.md` | Also the local surface for `/fix-coverage-*` |
| `Quality:*` | `docs/ai/commands/scan.md` | Qodana inspection per layer |
| `Scanner:*` | `docs/ai/commands/scan.md` | SonarQube init, analysis, issue queries |
| `Audit:*` | `docs/ai/commands/audit.md` | Rule-scoped audit-cli runs |
| `Format:*` | `docs/ai/commands/format.md` | Per-layer `dotnet format` |
| `Git:*` | `docs/ai/commands/commit.md` | Auto-message commits per scope |
| `Clean:*` | `docs/ai/commands/clean.md` | Artifact and log removal |
| `Verify:*` | `docs/ai/commands/test.md` | Whole-solution `dotnet test` (slow) |

`Inspect:*` tasks are thin `dependsOn` aliases of the matching `Quality:*` task, not duplicates —
add new inspection work to `Quality:*` and let the alias follow.

`docs/ai/commands/fix.md` has **no** dedicated task family. The `/fix-coverage-*` and `/fix-test-*`
agents run through `Coverage:*` and `Test:*`; there is no `Debug:*` label and none should be added.

## Adapter Surfaces

Adapters must remain thin and point to canonical agents. The authoritative inventory — which surface
belongs to which tool, which ones are full adapters expected to hold command parity, which are
rules-only, and which parity exceptions are deliberate — is `docs/ai/meta/adapter-surfaces.md`.

### Worked example: `ask-council`

Shows the full adapter fan-out from one Brain capability:

| Layer | File |
|---|---|
| Brain skill | `docs/ai/skills/ask-council/SKILL.md` |
| Brain agent | `docs/ai/agents/ask-council.md` |
| Brain command | `docs/ai/commands/council.md` |
| Brain spec | `docs/ai/specs/council.md` |
| Brain roles | `docs/ai/roles/council.md` |
| Brain workflow | `docs/ai/workflows/plan-with-council.md` |
| Claude | `.claude/skills/ask-council/SKILL.md`, `.claude/commands/ask-council.md`, `.claude/commands/plan-with-council.md` |
| Copilot | `.github/skills/ask-council/SKILL.md`, `.github/prompts/ask-council.prompt.md` |
| Antigravity | `.agent/workflows/ask-council.md` |
| Pi | `.pi/skills/ask-council/SKILL.md` |

Only the four **full adapters** appear here. The rules-only surfaces (Cline, Cursor, Windsurf,
Junie, Amazon Q) carry no per-capability files, so a capability never fans out to them — see
`docs/ai/meta/adapter-surfaces.md` §3.

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
- Test shape: `tools/audit-cli/Run-All.ps1 -RuleId Rule50` gates every PR — `[Theory]` + `TheoryData`
  only, and every `*Tests.cs` file must have a paired `*TestData.cs` file. See
  `docs/ai/specs/tools/audit-cli/spec.md` and `docs/ai/specs/testing/unit-test.md` §1.
- Coverage: Cobertura output via the cross-platform collector (`xplat`) — the engine every repo
  run uses; JetBrains dotCover 2025.3.3 also works on `net8.0`/`net10.0` from the IDE, but the
  repo ships no dotCover wrapper under `tools/code-coverage/`. CI enforces the
  `MIN_CODE_COVERAGE` threshold (default 100%).
- Inspection: JetBrains Qodana (static analysis) — available in CI behind the
  `QODANA_ENABLED` repository variable (opt-in, not always-on)
- Auditing: repo audit CLI/tasks as defined in `tools/` and referenced by workflow docs

## Adding a New Capability (Best Practice)

Use this order so the system stays DRY and portable:

1. **Spec** (if new rules/structure needed): add/extend under `docs/ai/specs/**`
2. **Skill** (reusable how-to): add under `docs/ai/skills/**`
3. **Workflow** (orchestration): add under `docs/ai/workflows/**`
4. **Agent** (canonical entrypoint): add under `docs/ai/agents/**`
5. **Command contract** (interface mapping): add under `docs/ai/commands/**`
6. **Adapters**: cascade to every surface on the checklist in `docs/ai/meta/adapter-surfaces.md` §5
7. **VS Code task**: optionally add a matching `.vscode/tasks.json` task

## References

- Taxonomy: `docs/ai/meta/taxonomy.md`
- Adapter inventory: `docs/ai/meta/adapter-surfaces.md`
- Universal Protocol: `docs/ai/specs/protocol.md`
- Root Spec precedence: `docs/ai/specs/spec.md`

<!-- footer
last_verified: 2026-08-20
-->
