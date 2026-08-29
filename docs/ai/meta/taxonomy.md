<!-- metadata_header
type: meta
id: ai-taxonomy
version: 1.1
-->

# AI Brain Taxonomy (PineGuard)

> [!IMPORTANT]
> This document defines the **portable, DRY mental model** for `docs/ai/**`.
> It is written to be reusable across every adapter surface listed in
> `docs/ai/meta/adapter-surfaces.md`.

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

### Rules

- **What**: A scope-scoped summary of the invariants that apply to a directory or layer.
- **Where**: `docs/ai/rules/*.md`, all inheriting from `docs/ai/rules/global.md`.
- **Used by**: Path-scoped adapters (`.claude/rules/`, `.github/instructions/`, `.clinerules/`,
  `.cursor/rules/`, `.windsurf/rules/`, `.amazonq/rules/`, `.junie/guidelines.md`).
- **Rule**: Rules summarise and point at Specs. They never restate a Spec in full,
  and they never carry intent-routing tables (that is what Commands are for).

### Skills

- **What**: A reusable “how-to” for a **specific task**.
- **Where**: `docs/ai/skills/{verb}[-{target}]/SKILL.md` — one directory per skill,
  with an optional `references/` subdirectory. `docs/ai/skills/INDEX.md` lists them all.
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
- **Used by**: Adapters — the full inventory lives in `docs/ai/meta/adapter-surfaces.md`.
- **Rule**: Agents should be **thin composition**:
  - declare **business unit** and **roles**,
  - point to the relevant **command contract** (if applicable),
  - reuse **skills/workflows** rather than duplicating steps.

### Memory

- **What**: Durable learned patterns a subagent accumulates across sessions.
- **Where**: `docs/ai/memory/*.md`, one file per subagent (`test-writer.md`, `code-reviewer.md`, …).
- **Used by**: Adapters that support persistent agent memory, seeded into `.claude/agent-memory/`.
- **Rule**: Memory records **observations**, not normative rules. When a pattern hardens into a
  rule, promote it to a Spec and leave a pointer behind.

### Plans

- **What**: A phased implementation roadmap for a piece of work larger than one session.
- **Where**: `docs/ai/plans/*.md` while live; `docs/ai/plans/completed/*.md` once shipped.
- **Used by**: Humans and agents scoping multi-step work.
- **Rule**: Every plan carries a `metadata_header` with `type: plan`, a stable `id:`, and a
  `status:` of `active`, `planned`, `living`, `non-binding`, `open` (decisions awaiting an
  owner's call) or `completed`. A plan filed under `plans/completed/` is always `completed`.
- **Rule**: Everything under `plans/completed/` is a **historical record** — read it for
  provenance, never as a task list. When an archived plan has become misleading, add an archival
  banner saying what actually shipped; do not rewrite it and do not repair its links.

### Meta

- **What**: The Brain’s own conventions — naming, document shapes, tool alignment, adapter inventory.
- **Where**: `docs/ai/meta/*.md`
- **Used by**: Anyone adding to or reorganising `docs/ai/**`.
- **Rule**: Meta describes the Brain; it never contains engineering rules (those are Specs).

## Composition Rules (DRY)

Use this direction of dependencies:

- **Specs** are referenced by everything.
- **Rules** summarise specs for a scope, and are what path-scoped adapters bind to.
- **Skills/Workflows** reference specs, and are reused by agents.
- **Agents** compose skills/workflows and bind roles.
- **Commands** map triggers/intents to agents.
- **Memory** records what agents learned; it never overrides specs or rules.
- **Adapters** reference commands/agents/rules (thin pointers only).

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

**Identity files** (roles, rules, specs, plans, meta, memory, business units):

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
| `improve` | Raise a measured metric toward its target (coverage only) |
| `ask` | Put a question to an advisory body (`ask-council`) |
| `plan` | Produce a plan rather than a change (`plan-with-council`) |

Two further verbs are reserved for **vendor-prefixed operations** only — see §N.9:
`publish` (`github-release-publish`) and `unlist` (`nuget-unlist`).

### §N.3 Retired Verbs (Do Not Use)

| Old | Replacement | Reason |
|-----|-------------|--------|
| `run-` | _(drop)_ | Redundant — workflows and agents run by definition |
| `debug-and-fix-` | `fix-` | Compound; `fix` implies debugging |
| `debug-and-test-` | `fix-` | Same; testing is part of fix |
| `implement-` | `scaffold-` | Canonical verb for "create new from recipe" |
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
6. **Plans are exempt**: a plan's `{topic}` names a subject, not an action, and may be naturally
   plural (`…-decisions.md`, `…-cases.md`). The no-plural rule binds action files and identity
   nouns, not plan topics.

**Never pluralise in filenames**: ~~tests~~, ~~clauses~~, ~~annotations~~, ~~issues~~, ~~diagnostics~~, ~~docs~~, ~~commits~~, ~~libraries~~, ~~conventions~~.

Two grandfathered exceptions exist in `rules/`: `fixture-conventions.md` (plural, topic-named)
and `coordination.md` (topic-named). Both are bound by path-scoped adapters on every surface, so
renaming them costs more than the inconsistency; do not add further topic-named rule files —
fold new rules into the owning scope file instead.

### §N.6 Per-Directory Conventions

Examples are repo-relative so they resolve from anywhere.

| Directory | Pattern | Verb-first? | Example |
|-----------|---------|-------------|---------|
| `agents/` | `{verb}-{scope}.md` | Yes | `docs/ai/agents/test-core.md` |
| `workflows/` | `{verb}-{target}.md` | Yes | `docs/ai/workflows/coverage.md`, `docs/ai/workflows/scan-sonar.md` |
| `skills/` | `{verb}[-{target}]/SKILL.md` (dir name) | Yes | `docs/ai/skills/scaffold-rule/SKILL.md`, `docs/ai/skills/document/SKILL.md` |
| `commands/` | `{verb}.md`, or `{noun}.md` for a domain contract | Mostly | `docs/ai/commands/test.md`, `docs/ai/commands/council.md` |
| `roles/` | `{noun}.md` | No | `docs/ai/roles/builder.md`, `docs/ai/roles/reviewer.md` |
| `rules/` | `{scope}.md` | No | `docs/ai/rules/core.md`, `docs/ai/rules/global.md` |
| `specs/` | `{type}.md` or `{domain}/{type}.md` | No | `docs/ai/specs/spec.md`, `docs/ai/specs/core/project.md` |
| `plans/` | `{topic}.md` | No | `docs/ai/plans/cross-platform-tools-migration.md` |
| `meta/` | `{topic}.md` | No | `docs/ai/meta/taxonomy.md` |
| `memory/` | `{agent-name}.md` | No | `docs/ai/memory/test-writer.md` |
| `business-units/` | `{noun}.md` | No | `docs/ai/business-units/engineering.md` |

Metadata convention for `meta/`: narrative meta documents (`taxonomy.md`, `tooling.md`,
`template-agent.md`, `template-spec.md`) carry a `metadata_header` and a `last_verified` footer;
`adapter-surfaces.md` carries YAML front matter with `last_verified`; the `template-*.md` spec
templates carry YAML `spec:` front matter with `version` and `last_verified`. Use whichever form
the document it governs uses.

### §N.7 Scan Tool Qualifier Convention

When `scan` or `fix` targets a specific tool, the tool name becomes the target:

| Pattern | Example | Meaning |
|---------|---------|---------|
| `scan-{tool}` | `docs/ai/workflows/scan-roslyn.md` | Run Roslyn diagnostics |
| `scan-{tool}` | `docs/ai/workflows/scan-sonar.md` | Run SonarQube scan |
| `scan-{tool}` | `docs/ai/workflows/scan-qodana.md` | Run Qodana inspection |
| `fix-{tool}` | `docs/ai/workflows/fix-roslyn.md` | Fix Roslyn warnings |
| `fix-{tool}` | `docs/ai/workflows/fix-sonar.md` | Fix SonarQube issues |

When severity is relevant, append as qualifier: `docs/ai/agents/fix-sonar-blocker.md`,
`docs/ai/agents/fix-sonar-high.md`.

### §N.8 Combining Verb + Scope + Tool

When an action targets both a scope and a tool, use: `{verb}-{tool}-{scope}.md`

Example: `docs/ai/agents/scan-roslyn-core.md` (run Roslyn on Core).

When only scope: `{verb}-{scope}.md` — `docs/ai/agents/test-core.md`, `docs/ai/agents/coverage-must.md`.
When only tool: `{verb}-{tool}.md` — `docs/ai/agents/scan-sonar.md`, `docs/ai/workflows/fix-roslyn.md`.
When neither (all/default): `{verb}.md` — `docs/ai/workflows/format.md`, `docs/ai/workflows/coverage.md`.

### §N.9 Vendor-Prefixed Operations

An operation that drives a **named external service** rather than this repository is prefixed with
that vendor's short name, and the verb moves to the end:

```
{vendor}-{noun}-{verb}.md
```

| File | Vendor | Reads as |
|------|--------|----------|
| `docs/ai/agents/github-release-publish.md` | GitHub | Publish a GitHub release |
| `docs/ai/agents/github-ruleset-enable.md` | GitHub | Enable a branch ruleset |
| `docs/ai/agents/github-ruleset-disable.md` | GitHub | Disable a branch ruleset |
| `docs/ai/agents/nuget-unlist.md` | nuget.org | Unlist a published package |

The prefix is deliberate: it groups the operations that leave the repository, and every one of them
is a Tier 0/1 operation under `docs/ai/specs/safety.md`. They are exposed on Claude Code only —
see the parity exceptions in `docs/ai/meta/adapter-surfaces.md`.

## Best-Practice Guardrails

- Prefer **one** canonical place for a procedure (Skill or Workflow) and have Agents reference it.
- Prefer **one** canonical place for rules (Specs) and have Skills/Workflows reference them.
- Prefer **stable IDs** for anything a tool might index: specs already have YAML headers; roles/agents should keep stable `id:`.

## References

- Brain index: `docs/ai/README.md`
- Universal Protocol: `docs/ai/specs/protocol.md`
- Root Spec (cascading): `docs/ai/specs/spec.md`
- Adapter inventory: `docs/ai/meta/adapter-surfaces.md`
- Tooling alignment: `docs/ai/meta/tooling.md`

<!-- footer
last_verified: 2026-08-20
-->
