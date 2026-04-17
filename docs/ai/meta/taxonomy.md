<!-- metadata_header
type: meta
id: ai-taxonomy
version: 1.0
-->

# AI Brain Taxonomy (PineGuard)

> [!IMPORTANT]
> This document defines the **portable, DRY mental model** for `docs/ai/**`.
> It is written to be reusable across tooling (GitHub Copilot, Claude Code, Gemini, Cursor, etc.).

## Goal

Make PineGuard’s AI documentation:

- **Model-agnostic** (no tool-specific logic in the Brain)
- **Composable** (agents are built from reusable skills/workflows)
- **Deterministic** (specs are normative and enforce precedence)
- **DRY** (avoid copy/paste instructions across agents/adapters)

## Core Concepts

### Business Units

- **What**: Organizational context + governance boundary.
- **Where**: `docs/ai/business-units/*.md`
- **Used by**: Agents (to choose constraints, vocabulary, and operating norms).

### Roles

- **What**: A persona definition describing **responsibilities**, **constraints**, and **expected practices**.
- **Where**: `docs/ai/roles/*.md`
- **Used by**: Agents (declared as primary/secondary roles).
- **Rule**: Roles define _how you behave_ and _what you optimize for_.
  They should not embed long execution scripts.

### Specs

- **What**: Normative engineering constraints (“the rules”).
- **Where**: `docs/ai/specs/**/*.md`
- **Used by**: Skills, Workflows, Agents, and adapters.
- **Rule**: Specs are the **source of truth** for code structure, layering, naming, determinism, testing and coverage.

### Skills

- **What**: A reusable “how-to” for a **specific task**.
- **Where**: `docs/ai/skills/*.md`
- **Used by**: Agents and sometimes Workflows.
- **Rule**: Skills should be mostly self-contained with:
  - a clear goal,
  - inputs,
  - critical rules,
  - steps,
  - definition of done,
  - references to the deeper specs.

### Workflows

- **What**: Reusable orchestration for multi-step operations (tests, coverage, inspection, auditing, rebuilds).
- **Where**: `docs/ai/workflows/*.md`
- **Used by**: Agents and adapters.
- **Rule**: Workflows should be parameterizable and avoid duplication.

### Commands

- **What**: A stable **interface contract** for invoking an operation.
  The UI/trigger can be slash commands, buttons, palette commands, or chat intent.
- **Where**: `docs/ai/commands/*.md`
- **Used by**: Adapters and humans to map “intent” → the canonical agent entrypoint.
- **Rule**: Commands do not contain implementation logic; they reference the canonical agent/workflow.

### Agents

- **What**: Canonical, model-agnostic playbooks for doing a job end-to-end.
- **Where**: `docs/ai/agents/*.md`
- **Used by**: Adapters (Copilot/Claude/Gemini/etc.).
- **Rule**: Agents should be **thin composition**:
  - declare **business unit** and **roles**,
  - point to the relevant **command contract** (if applicable),
  - reuse **skills/workflows** rather than duplicating steps.

## Composition Rules (DRY)

Use this direction of dependencies:

- **Specs** are referenced by everything.
- **Skills/Workflows** reference specs, and are reused by agents.
- **Agents** compose skills/workflows and bind roles.
- **Commands** map triggers/intents to agents.
- **Adapters** reference commands/agents (thin pointers only).

Brain NEVER depends on adapters.

## Practical Example (Intent → Execution)

A good portable chain looks like:

1. User intent (“run coverage for Core”)
2. Command contract: `docs/ai/commands/coverage.md`
3. Canonical agent: `docs/ai/agents/coverage-core.md`
4. Reusable orchestration: `docs/ai/workflows/coverage.md`
5. Repo automation script(s): `tools/**` and/or `.vscode/tasks.json`

Each layer should add value without duplicating the others.

## Naming Convention

All files in `docs/ai/` follow a strict naming convention to ensure consistency, discoverability, and simplicity.

### §N.1 Universal Pattern

**Action files** (agents, workflows, skills, commands):

```
{verb}-{target}.md
```

- **verb**: Imperative mood, always singular. What the operation does.
- **target**: What it operates on. Always singular (see §N.5).

**Identity files** (roles, rules, specs):

```
{noun}.md
```

- **noun**: Singular unless the concept is inherently uncountable.

### §N.2 Approved Verbs (Closed Set)

Only these verbs may appear as the leading segment of an action filename. Any verb not in this list must not be used.

| Verb | Meaning |
|------|---------|
| `test` | Run unit test |
| `fix` | Debug and resolve failure |
| `scan` | Run static analysis (read-only) |
| `format` | Apply code formatting |
| `generate` | Generate code from external data |
| `coverage` | Run coverage analysis |
| `document` | Generate documentation |
| `commit` | Git commit change |
| `clean` | Remove artifact or log |
| `scaffold` | Create new code from recipe |
| `audit` | Analyze gap or compliance |
| `build` | Compile or rebuild |
| `verify` | Check a condition |
| `migrate` | Migration operation |
| `refactor` | Restructure without behaviour change |

### §N.3 Retired Verbs (Do Not Use)

| Old | Replacement | Reason |
|-----|-------------|--------|
| `run-` | _(drop)_ | Redundant — workflows and agents run by definition |
| `debug-and-fix-` | `fix-` | Compound; `fix` implies debugging |
| `debug-and-test-` | `fix-` | Same; testing is part of fix |
| `implement-` | `scaffold-` | Canonical verb for "create new from recipe" |
| `improve-` | _(use domain verb)_ | Too generic; use `coverage`, `fix`, etc. |
| `create-` | `scaffold-` | Unified under one verb |
| `rebuild-` | `build-` | Simpler |
| `xml-docs-` | `document-` | Verb-first; `xml` is implementation detail |
| `git-commit-` | `commit-` | `git` is implementation detail |
| `analyze-` | `audit-` | Canonical verb for analysis |

### §N.4 Scope Identifiers (Closed Set)

When a filename targets a PineGuard layer, use these exact short-form identifiers:

| Scope | Maps to | Note |
|-------|---------|------|
| `core` | PineGuard.Core | |
| `must` | PineGuard.MustClauses | Short form — always singular |
| `guard` | PineGuard.GuardClauses | Short form — always singular |
| `fluent` | PineGuard.FluentValidation | Short form |
| `annotation` | PineGuard.DataAnnotations | Singular form |
| `testing` | PineGuard.Testing | |
| `all` | All projects | |

When a filename targets a tool, use the tool's canonical short name:

| Tool | Identifier |
|------|-----------|
| Roslyn compiler | `roslyn` |
| JetBrains Qodana | `qodana` |
| SonarQube | `sonar` |

### §N.5 Singular/Plural Rules

1. **Verbs**: Always imperative singular — `test`, never `tests`.
2. **Scope identifiers**: Always singular per §N.4 — `must`, never `must-clauses`.
3. **Target nouns**: Always singular — `artifact`, `log`, `gap`, `rule`, `test`, `standard`, `tool`, `workflow`.
4. **Qualifiers**: Always singular — `blocker`, not `blockers`.
5. **Only exception**: `all` (refers to a collection by design).

**Never pluralise in filenames**: ~~tests~~, ~~clauses~~, ~~annotations~~, ~~issues~~, ~~diagnostics~~, ~~docs~~, ~~commits~~, ~~libraries~~, ~~conventions~~.

### §N.6 Per-Directory Conventions

| Directory | Pattern | Verb-first? | Example |
|-----------|---------|-------------|---------|
| `agents/` | `{verb}-{scope}.md` | Yes | `test-core.md` |
| `workflows/` | `{verb}-{target}.md` | Yes | `coverage.md`, `scan-sonar.md` |
| `skills/` | `{verb}/SKILL.md` (dir name) | Yes | `scaffold-rule/SKILL.md` |
| `commands/` | `{verb}.md` | Yes | `test.md`, `fix.md` |
| `roles/` | `{noun}.md` | No | `builder.md`, `reviewer.md` |
| `rules/` | `{scope}.md` | No | `core.md`, `must.md`, `global.md` |
| `specs/` | `{type}.md` or `{domain}/{type}.md` | No | `spec.md`, `core/project.md` |
| `plans/` | `{topic}.md` | No | `naming-convention.md` |
| `meta/` | `{topic}.md` | No | `taxonomy.md` |

### §N.7 Scan Tool Qualifier Convention

When `scan` or `fix` targets a specific tool, the tool name becomes the target:

| Pattern | Example | Meaning |
|---------|---------|---------|
| `scan-{tool}` | `scan-roslyn.md` | Run Roslyn diagnostics |
| `scan-{tool}` | `scan-sonar.md` | Run SonarQube scan |
| `scan-{tool}` | `scan-qodana.md` | Run Qodana inspection |
| `fix-{tool}` | `fix-roslyn.md` | Fix Roslyn warnings |
| `fix-{tool}` | `fix-sonar.md` | Fix SonarQube issues |

When severity is relevant, append as qualifier: `fix-sonar-blocker.md`, `fix-sonar-high.md`.

### §N.8 Combining Verb + Scope + Tool

When an action targets both a scope and a tool, use: `{verb}-{tool}-{scope}.md`

Example: `scan-roslyn-core.md` (run Roslyn on Core).

When only scope: `{verb}-{scope}.md` — `test-core.md`, `coverage-must.md`.
When only tool: `{verb}-{tool}.md` — `scan-sonar.md`, `fix-roslyn.md`.
When neither (all/default): `{verb}.md` — `format.md`, `coverage.md`.

## Best-Practice Guardrails

- Prefer **one** canonical place for a procedure (Skill or Workflow) and have Agents reference it.
- Prefer **one** canonical place for rules (Specs) and have Skills/Workflows reference them.
- Prefer **stable IDs** for anything a tool might index: specs already have YAML headers; roles/agents should keep stable `id:`.

## References

- Universal Protocol: `docs/ai/specs/protocol.md`
- Root Spec (cascading): `docs/ai/specs/spec.md`

<!-- footer
last_verified: 2026-04-15
-->
