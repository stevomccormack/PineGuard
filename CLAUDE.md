# Claude Adapter (PineGuard)

> [!IMPORTANT]
> **This file is an Adapter.**
> It maps slash commands to the canonical Agents in `docs/ai/agents` and Workflows in `docs/ai/workflows`.
> Do not add logic here. Add logic to the Brain.
> 👉 Start at **[docs/ai/README.md](docs/ai/README.md)** for the full Brain index.

## 1. Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
👉 **[docs/ai/business-units/engineering.md](docs/ai/business-units/engineering.md)**

The persona named on each palette entry below is a canonical role in
**[docs/ai/roles/](docs/ai/roles/)** — the same role the target playbook declares on its `roles:`
line. The playbook is authoritative: if the two ever disagree, the playbook wins.

## 2. Command Palette (Slash Commands)

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

### Release

- `/github-release-publish`: Act as **Shipper**. Execute `docs/ai/agents/github-release-publish.md`.
- `/github-ruleset-disable`: Act as **Shipper**. Execute `docs/ai/agents/github-ruleset-disable.md`.
- `/github-ruleset-enable`: Act as **Shipper**. Execute `docs/ai/agents/github-ruleset-enable.md`.
- `/nuget-unlist`: Act as **Shipper**. Execute `docs/ai/agents/nuget-unlist.md`.

## 3. Native Claude Code Features

Claude Code has native features in `.claude/` that **reference** the Brain (never duplicate it) —
rules, skills, agents, agent memory, hooks, and commands, each mapping to a Brain counterpart. The
full breakdown lives in **[docs/ai/README.md](docs/ai/README.md)** (the "Claude Code Adapter"
section) so it is stated once, not twice.

`.claude/` is one of several adapter surfaces in this repository. The full inventory — root boot
files, full adapters, rules-only adapters, and the parity policy — lives in
**[docs/ai/meta/adapter-surfaces.md](docs/ai/meta/adapter-surfaces.md)**.

## 4. Safety

Before executing commands, read the safety spec:
👉 **[docs/ai/specs/safety.md](docs/ai/specs/safety.md)**

## 5. Knowledge Base

- **Brain index**: `docs/ai/README.md`
- **Specs**: `docs/ai/specs/` (normative engineering rules, coding standards, testing specs)
- **Safety**: `docs/ai/specs/safety.md` (Tier 0/1/2 command classification)
- **Rules**: `docs/ai/rules/` (scope-specific, inheriting from `global.md`)
- **Skills**: `docs/ai/skills/` (reusable implementation recipes)
- **Agents**: `docs/ai/agents/` (canonical playbooks)
- **Workflows**: `docs/ai/workflows/` (multi-step orchestration)
- **Commands**: `docs/ai/commands/` (intent-to-agent mappings)
- **Roles**: `docs/ai/roles/` (personas and responsibilities)
- **Business Units**: `docs/ai/business-units/` (departments and the role roster)
- **Memory**: `docs/ai/memory/` (per-subagent learned patterns)
- **Meta**: `docs/ai/meta/` (taxonomy, tooling alignment, adapter-surface inventory)
- **Plans**: `docs/ai/plans/` (implementation roadmaps)
