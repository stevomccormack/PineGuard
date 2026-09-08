# Copilot Adapter (PineGuard)

> [!IMPORTANT]
> **This file is an Adapter.**
> It maps Copilot prompt files to the canonical Agents in `docs/ai/agents` and Workflows in `docs/ai/workflows`.
> Do not add logic here. Add logic to the Brain.
> 👉 Start at **[docs/ai/README.md](../docs/ai/README.md)** for the full Brain index.

## 1. Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
👉 **[engineering.md](../docs/ai/business-units/engineering.md)**

The persona named on each palette entry below is a canonical role in
**[roles](../docs/ai/roles/)** — the same role the target playbook declares on its `roles:`
line. The playbook is authoritative: if the two ever disagree, the playbook wins.

## 2. Command Palette (Prompt Files)

Every entry below is a prompt file `.github/prompts/<name>.prompt.md`. In Copilot Chat (VS Code, Visual Studio, JetBrains) type `/<name>` to run it; each prompt sets `agent:` to the custom agent for the role named here and points at the playbook. On github.com (Copilot coding agent, Copilot code review) prompt files are not available - route the intent through `../docs/ai/commands/` to the same playbook. The release family (`github-release-publish`, `github-ruleset-enable`, `github-ruleset-disable`, `nuget-unlist`) is Claude Code only by policy - see [adapter-surfaces.md](../docs/ai/meta/adapter-surfaces.md) §4 - and is deliberately absent here.

### Coverage

- `/coverage-all`: Act as **Verifier**. Execute `docs/ai/agents/coverage-all.md`.
- `/coverage-core`: Act as **Verifier**. Execute `docs/ai/agents/coverage-core.md`.
- `/coverage-must`: Act as **Verifier**. Execute `docs/ai/agents/coverage-must.md`.
- `/coverage-guard`: Act as **Verifier**. Execute `docs/ai/agents/coverage-guard.md`.
- `/coverage-fluent`: Act as **Verifier**. Execute `docs/ai/agents/coverage-fluent.md`.
- `/coverage-annotation`: Act as **Verifier**. Execute `docs/ai/agents/coverage-annotation.md`.
- `/coverage-testing`: Act as **Verifier**. Execute `docs/ai/agents/coverage-testing.md`.

### Test

- `/test-all`: Act as **Verifier**. Execute `docs/ai/agents/test-all.md`.
- `/test-core`: Act as **Verifier**. Execute `docs/ai/agents/test-core.md`.
- `/test-must`: Act as **Verifier**. Execute `docs/ai/agents/test-must.md`.
- `/test-guard`: Act as **Verifier**. Execute `docs/ai/agents/test-guard.md`.
- `/test-fluent`: Act as **Verifier**. Execute `docs/ai/agents/test-fluent.md`.
- `/test-annotation`: Act as **Verifier**. Execute `docs/ai/agents/test-annotation.md`.
- `/test-testing`: Act as **Verifier**. Execute `docs/ai/agents/test-testing.md`.

### Fix Coverage

- `/fix-coverage-all`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-all.md`.
- `/fix-coverage-core`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-core.md`.
- `/fix-coverage-must`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-must.md`.
- `/fix-coverage-guard`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-guard.md`.
- `/fix-coverage-fluent`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-fluent.md`.
- `/fix-coverage-annotation`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-annotation.md`.
- `/fix-coverage-testing`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-coverage-testing.md`.

### Fix Test

- `/fix-test-all`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-all.md`.
- `/fix-test-core`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-core.md`.
- `/fix-test-must`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-must.md`.
- `/fix-test-guard`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-guard.md`.
- `/fix-test-fluent`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-fluent.md`.
- `/fix-test-annotation`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-annotation.md`.
- `/fix-test-testing`: Act as **Owner / Verifier**. Execute `docs/ai/agents/fix-test-testing.md`.

### Format

- `/format-all`: Act as **Builder**. Execute `docs/ai/agents/format-all.md`.
- `/format-core`: Act as **Builder**. Execute `docs/ai/agents/format-core.md`.
- `/format-must`: Act as **Builder**. Execute `docs/ai/agents/format-must.md`.
- `/format-guard`: Act as **Builder**. Execute `docs/ai/agents/format-guard.md`.
- `/format-fluent`: Act as **Builder**. Execute `docs/ai/agents/format-fluent.md`.
- `/format-annotation`: Act as **Builder**. Execute `docs/ai/agents/format-annotation.md`.
- `/format-testing`: Act as **Builder**. Execute `docs/ai/agents/format-testing.md`.

### Document

- `/document-all`: Act as **Builder**. Execute `docs/ai/agents/document-all.md`.
- `/document-core`: Act as **Builder**. Execute `docs/ai/agents/document-core.md`.
- `/document-must`: Act as **Builder**. Execute `docs/ai/agents/document-must.md`.
- `/document-guard`: Act as **Builder**. Execute `docs/ai/agents/document-guard.md`.
- `/document-fluent`: Act as **Builder**. Execute `docs/ai/agents/document-fluent.md`.
- `/document-annotation`: Act as **Builder**. Execute `docs/ai/agents/document-annotation.md`.

### Scan Qodana

- `/scan-qodana-all`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-all.md`.
- `/scan-qodana-core`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-core.md`.
- `/scan-qodana-must`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-must.md`.
- `/scan-qodana-guard`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-guard.md`.
- `/scan-qodana-fluent`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-fluent.md`.
- `/scan-qodana-annotation`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-annotation.md`.
- `/scan-qodana-testing`: Act as **Reviewer**. Execute `docs/ai/agents/scan-qodana-testing.md`.

### Scan Roslyn

- `/scan-roslyn-all`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-all.md`.
- `/scan-roslyn-core`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-core.md`.
- `/scan-roslyn-must`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-must.md`.
- `/scan-roslyn-guard`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-guard.md`.
- `/scan-roslyn-fluent`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-fluent.md`.
- `/scan-roslyn-annotation`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-annotation.md`.
- `/scan-roslyn-testing`: Act as **Reviewer**. Execute `docs/ai/agents/scan-roslyn-testing.md`.
- `/fix-roslyn-all`: Act as **Owner**. Execute `docs/ai/agents/fix-roslyn-all.md`.

### Scan Sonar

- `/scan-sonar`: Act as **Reviewer**. Execute `docs/ai/agents/scan-sonar.md`.
- `/fix-sonar-all`: Act as **Owner**. Execute `docs/ai/agents/fix-sonar-all.md`.
- `/fix-sonar-blocker`: Act as **Owner**. Execute `docs/ai/agents/fix-sonar-blocker.md`.
- `/fix-sonar-high`: Act as **Owner**. Execute `docs/ai/agents/fix-sonar-high.md`.
- `/fix-sonar-medium`: Act as **Owner**. Execute `docs/ai/agents/fix-sonar-medium.md`.
- `/fix-sonar-low`: Act as **Owner**. Execute `docs/ai/agents/fix-sonar-low.md`.

### Clean

- `/clean-all`: Act as **Shipper**. Execute `docs/ai/agents/clean-all.md`.
- `/clean-artifact`: Act as **Shipper**. Execute `docs/ai/agents/clean-artifact.md`.
- `/clean-log`: Act as **Shipper**. Execute `docs/ai/agents/clean-log.md`.

### Commit

- `/commit-agent`: Act as **Shipper**. Execute `docs/ai/agents/commit-agent.md`.
- `/commit-all`: Act as **Shipper**. Execute `docs/ai/agents/commit-all.md`.
- `/commit-core`: Act as **Shipper**. Execute `docs/ai/agents/commit-core.md`.
- `/commit-must`: Act as **Shipper**. Execute `docs/ai/agents/commit-must.md`.
- `/commit-guard`: Act as **Shipper**. Execute `docs/ai/agents/commit-guard.md`.
- `/commit-fluent`: Act as **Shipper**. Execute `docs/ai/agents/commit-fluent.md`.
- `/commit-annotation`: Act as **Shipper**. Execute `docs/ai/agents/commit-annotation.md`.
- `/commit-testing`: Act as **Shipper**. Execute `docs/ai/agents/commit-testing.md`.
- `/commit-doc`: Act as **Shipper**. Execute `docs/ai/agents/commit-doc.md`.
- `/commit-solution`: Act as **Shipper**. Execute `docs/ai/agents/commit-solution.md`.
- `/commit-tool`: Act as **Shipper**. Execute `docs/ai/agents/commit-tool.md`.

### Audit

- `/audit-gap`: Act as **Planner / Verifier**. Execute `docs/ai/agents/audit-gap.md`.
- `/audit-cli`: Act as **Shipper**. Execute `docs/ai/agents/audit-cli.md`.

### Scaffold

- `/scaffold-vertical-slice`: Act as **Owner**. Execute `docs/ai/agents/scaffold-vertical-slice.md`.
- Single-layer scaffolds are **Skills, not commands** — see `docs/ai/commands/scaffold.md` for the
  skill roster (`scaffold-rule`, `scaffold-must`, `scaffold-guard`, `scaffold-fluent`,
  `scaffold-annotation`, `scaffold-unit-test`, `new-validation`).

### Council

- `/ask-council`: Act as **Architect / Council**. Execute `docs/ai/agents/ask-council.md`.
- `/plan-with-council`: Act as **Architect / Council**. Execute `docs/ai/workflows/plan-with-council.md`.

## 3. Copilot-Native Features

| Feature | Directory | Maps to Brain |
|---|---|---|
| Repo instructions | `copilot-instructions.md` (this file) | `docs/ai/README.md` |
| Path instructions | `instructions/*.instructions.md` | one per path-scoped rule in `docs/ai/rules/` (`global.md` is covered by §6 of this file) |
| Custom agents | `agents/<role>.agent.md` | one per role in `docs/ai/roles/` (name = role file basename) |
| Subagent personas | `agents/{code-reviewer,coverage-analyst,migration-checker,test-writer,validation-builder}.agent.md` | mirror `.claude/agents/` and read `docs/ai/memory/` |
| Prompt files | `prompts/<name>.prompt.md` | one per palette command above -> `docs/ai/agents/` |
| Skills | `skills/<name>/SKILL.md` | one per Brain skill in `docs/ai/skills/` |

This surface is a **full adapter**: command parity with `docs/ai/agents/` is enforced by the `surface-parity` audit rule, and the only declared exception is the release family. Custom agents carry no `tools:` restriction - what a role may run is governed by [safety.md](../docs/ai/specs/safety.md) and the playbook, not by this adapter. MCP servers and `copilot-setup-steps.yml` are not part of this surface: MCP configuration is global per tool (owner decision), and the repository has no coding-agent environment convention to mirror.

## 4. Safety

Before executing commands, read the safety spec:
👉 **[safety.md](../docs/ai/specs/safety.md)**

## 5. Knowledge Base

- **Brain index**: `../docs/ai/README.md`
- **Specs**: `../docs/ai/specs/` (normative engineering rules, coding standards, testing specs)
- **Safety**: `../docs/ai/specs/safety.md` (Tier 0/1/2 command classification)
- **Rules**: `../docs/ai/rules/` (scope-specific, inheriting from `global.md`)
- **Skills**: `../docs/ai/skills/` (reusable implementation recipes)
- **Agents**: `../docs/ai/agents/` (canonical playbooks)
- **Workflows**: `../docs/ai/workflows/` (multi-step orchestration)
- **Commands**: `../docs/ai/commands/` (intent-to-agent mappings)
- **Roles**: `../docs/ai/roles/` (personas and responsibilities)
- **Business Units**: `../docs/ai/business-units/` (departments and the role roster)
- **Memory**: `../docs/ai/memory/` (per-subagent learned patterns)
- **Meta**: `../docs/ai/meta/` (taxonomy, tooling alignment, adapter-surface inventory)
- **Plans**: `../docs/ai/plans/` (implementation roadmaps)

## 6. Global Rules

Read `../docs/ai/rules/global.md` for invariants that apply to all code in this repository; it is authoritative and also covers workflow orchestration and multi-session coordination (`../docs/ai/rules/coordination.md`). The path-scoped files in `./instructions/` inherit from it.

## 7. Comments Policy

Comment and XML-documentation discipline is a cross-cutting engineering rule, so it lives in the Brain:
👉 **[docs/ai/specs/coding-standard.md](../docs/ai/specs/coding-standard.md)**

## 8. Tooling Workarounds

### `run_build` hangs with `.slnx`

This workspace uses the `.slnx` solution format (VS 2022 17.10+ / .NET 9+). The `run_build` tool does not reliably support `.slnx` — it may hang or timeout.

**Do NOT use `run_build`.** Use the terminal instead:

```powershell
dotnet build PineGuard.slnx --verbosity quiet
```
