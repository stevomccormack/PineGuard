# PI Adapter (PineGuard)

> **This file is an Adapter.**
> It maps prompt templates to the canonical Agents in `docs/ai/agents`.
> Do not add logic here. Add logic to the Brain.
> Start at **docs/ai/README.md** for the full Brain index.

## 1. Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
Read **docs/ai/business-units/engineering.md**

## 2. Prompt Templates (Slash Commands)

PI prompt templates live in `.pi/prompts/`. Type `/templatename` to invoke.

### Coverage Workflows

- `/coverage-all`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-all.md`.
- `/coverage-core`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-core.md`.
- `/coverage-must`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-must.md`.
- `/coverage-guard`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-guard.md`.
- `/coverage-fluent`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-fluent.md`.
- `/coverage-annotation`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-annotation.md`.
- `/coverage-testing`: Act as **Test Engineer**. Execute `docs/ai/agents/coverage-testing.md`.

### Testing Workflows

- `/test-all`: Act as **Test Engineer**. Execute `docs/ai/agents/test-all.md`.
- `/test-core`: Act as **Test Engineer**. Execute `docs/ai/agents/test-core.md`.
- `/test-must`: Act as **Test Engineer**. Execute `docs/ai/agents/test-must.md`.
- `/test-guard`: Act as **Test Engineer**. Execute `docs/ai/agents/test-guard.md`.
- `/test-fluent`: Act as **Test Engineer**. Execute `docs/ai/agents/test-fluent.md`.
- `/test-annotation`: Act as **Test Engineer**. Execute `docs/ai/agents/test-annotation.md`.
- `/test-testing`: Act as **Test Engineer**. Execute `docs/ai/agents/test-testing.md`.

### Debug & Fix Workflows

- `/fix-coverage-all`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-all.md`.
- `/fix-coverage-core`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-core.md`.
- `/fix-coverage-must-clauses`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-must.md`.
- `/fix-coverage-guard-clauses`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-guard.md`.
- `/fix-coverage-fluent-validation`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-fluent.md`.
- `/fix-coverage-data-annotations`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-annotation.md`.
- `/fix-coverage-testing`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-coverage-testing.md`.

### Debug & Test Workflows

- `/fix-test-all`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-all.md`.
- `/fix-test-core`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-core.md`.
- `/fix-test-must-clauses`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-must.md`.
- `/fix-test-guard-clauses`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-guard.md`.
- `/fix-test-fluent-validation`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-fluent.md`.
- `/fix-test-data-annotations`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-annotation.md`.
- `/fix-test-testing`: Act as **Senior Engineer / Test Engineer**. Execute `docs/ai/agents/fix-test-testing.md`.

### Formatting Workflows

- `/format-all`: Act as **Software Engineer**. Execute `docs/ai/agents/format-all.md`.
- `/format-core`: Act as **Software Engineer**. Execute `docs/ai/agents/format-core.md`.
- `/format-must`: Act as **Software Engineer**. Execute `docs/ai/agents/format-must.md`.
- `/format-guard`: Act as **Software Engineer**. Execute `docs/ai/agents/format-guard.md`.
- `/format-fluent`: Act as **Software Engineer**. Execute `docs/ai/agents/format-fluent.md`.
- `/format-annotation`: Act as **Software Engineer**. Execute `docs/ai/agents/format-annotation.md`.
- `/format-testing`: Act as **Software Engineer**. Execute `docs/ai/agents/format-testing.md`.

### XML Documentation Workflows

- `/document-all`: Act as **Software Engineer**. Execute `docs/ai/agents/document-all.md`.
- `/document-core`: Act as **Software Engineer**. Execute `docs/ai/agents/document-core.md`.
- `/document-must-clauses`: Act as **Software Engineer**. Execute `docs/ai/agents/document-must.md`.
- `/document-guard-clauses`: Act as **Software Engineer**. Execute `docs/ai/agents/document-guard.md`.
- `/document-fluent-validation`: Act as **Software Engineer**. Execute `docs/ai/agents/document-fluent.md`.
- `/document-data-annotations`: Act as **Software Engineer**. Execute `docs/ai/agents/document-annotation.md`.

### Qodana Workflows

- `/scan-qodana-all`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-all.md`.
- `/scan-qodana-core`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-core.md`.
- `/scan-qodana-must-clauses`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-must.md`.
- `/scan-qodana-guard-clauses`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-guard.md`.
- `/scan-qodana-fluent-validation`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-fluent.md`.
- `/scan-qodana-data-annotations`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-annotation.md`.
- `/scan-qodana-testing`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-qodana-testing.md`.

### Roslyn Workflows

- `/scan-roslyn-all`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-all.md`.
- `/scan-roslyn-core`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-core.md`.
- `/roslyn-must-clauses`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-must.md`.
- `/roslyn-guard-clauses`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-guard.md`.
- `/roslyn-fluent-validation`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-fluent.md`.
- `/roslyn-data-annotations`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-annotation.md`.
- `/roslyn-testing`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-roslyn-testing.md`.
- `/fix-roslyn-all`: Act as **Senior Engineer**. Execute `docs/ai/agents/fix-roslyn-all.md`.

### Sonar Workflows

- `/scan-sonar`: Act as **Code Reviewer**. Execute `docs/ai/agents/scan-sonar.md`.
- `/fix-sonar-all`: Act as **Senior Engineer**. Execute `docs/ai/agents/fix-sonar-all.md`.
- `/fix-sonar-blockers`: Act as **Senior Engineer**. Execute `docs/ai/agents/fix-sonar-blocker.md`.
- `/fix-sonar-high`: Act as **Senior Engineer**. Execute `docs/ai/agents/fix-sonar-high.md`.
- `/fix-sonar-medium`: Act as **Senior Engineer**. Execute `docs/ai/agents/fix-sonar-medium.md`.
- `/fix-sonar-low`: Act as **Senior Engineer**. Execute `docs/ai/agents/fix-sonar-low.md`.

### Maintenance Workflows

- `/clean-artifact`: Act as **DevOps Engineer**. Execute `docs/ai/agents/clean-artifact.md`.
- `/clean-log`: Act as **DevOps Engineer**. Execute `docs/ai/agents/clean-log.md`.
- `/clean-all`: Act as **DevOps Engineer**. Execute `docs/ai/agents/clean-all.md`.

### Git Commit Workflows

- `/commit-agent`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-agent.md`.
- `/commit-all`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-all.md`.
- `/commit-core`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-core.md`.
- `/commit-must-clauses`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-must.md`.
- `/commit-guard-clauses`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-guard.md`.
- `/commit-fluent-validation`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-fluent.md`.
- `/commit-data-annotations`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-annotation.md`.
- `/commit-testing`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-testing.md`.
- `/commit-docs`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-doc.md`.
- `/commit-solution`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-solution.md`.
- `/commit-tools`: Act as **Software Engineer**. Execute `docs/ai/agents/commit-tool.md`.

### Analysis Workflows

- `/audit-gap`: Act as **Software Engineer**. Execute `docs/ai/agents/audit-gap.md`.
- `/audit-cli`: Act as **Software Engineer**. Execute `docs/ai/agents/audit-cli.md`.
- `/scaffold-vertical-slice`: Act as **Software Engineer**. Execute `docs/ai/agents/scaffold-vertical-slice.md`.

## 3. PI-Native Features

PI has native features in `.pi/` that **reference** the Brain (never duplicate it):

| Feature | Directory | Purpose |
|---------|-----------|---------|
| Skills | `.pi/skills/` | On-demand capability packages -> `docs/ai/skills/` |
| Prompt Templates | `.pi/prompts/` | Slash command adapters -> `docs/ai/agents/` |
| Extensions | `.pi/extensions/` | TypeScript hooks for file hygiene, locking |
| Context (per-dir) | `*/AGENTS.md` | Path-scoped rules -> `docs/ai/rules/` |

## 4. Safety

Before executing commands, read the safety spec:
Read **docs/ai/specs/safety.md** (Tier 0/1/2 command classification)

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
- **Architecture**: `docs/ai/specs/` (structural design)
- **Meta**: `docs/ai/meta/` (taxonomy, tooling alignment)
- **Plans**: `docs/ai/plans/` (implementation roadmaps)

## 6. Global Rules

Read `docs/ai/rules/global.md` for invariants that apply to all code in this repository.

Key invariants:
- Layer order: Core Utils -> Core Rules -> MustClauses -> GuardClauses -> Integrations
- Must owns canonical messages; Guard/Fluent/Data reuse them (never duplicate)
- Guard calls Must (never duplicate logic)
- Deterministic: No IO in Core Rules/Utils
- File-scoped namespaces, sorted usings, arrow functions for single-line expressions
- All output files -> `artifacts/` or `logs/`, NEVER project root