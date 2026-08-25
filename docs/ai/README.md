<!-- metadata_header
type: meta
id: ai-docs-index
version: 2.0
-->

# PineGuard AI Brain (docs/ai)

> [!IMPORTANT]
> This directory is the **portable, model-agnostic Brain**.
> Any AI tool (GitHub Copilot, Claude Code, Gemini, Cursor, etc.) should start here.

## Start Here

1. Taxonomy (what each concept means and how they compose):
   - `docs/ai/meta/taxonomy.md`
2. Tooling alignment (GitHub + Microsoft/VS Code mapping):
   - `docs/ai/meta/tooling.md`
3. Root spec (cascading constraints and precedence):
   - `docs/ai/specs/spec.md`

## The Brain Map

| Directory | Purpose | Contents |
|-----------|---------|----------|
| `docs/ai/specs/` | Normative engineering rules + structural design | Root spec, protocol, orchestration, dependencies, per-project specs, coding standards, testing specs, [GOLD-STANDARD index](specs/testing/gold-standard.md) |
| `docs/ai/rules/` | Scope-specific rule summaries | Global invariants + per-project rules (inheriting from `global.md`) |
| `docs/ai/skills/` | Reusable "how-to" procedures | Implementation recipes for each layer + meta templates |
| `docs/ai/agents/` | Canonical playbooks / entrypoints | Agent definitions for all workflows |
| `docs/ai/workflows/` | Reusable orchestration | Multi-step workflow definitions |
| `docs/ai/commands/` | Interface contracts / triggers | Intent-to-agent mappings, one per command family |
| `docs/ai/roles/` | Personas and responsibilities | 11 roles (see Roles Inventory below) |
| `docs/ai/memory/` | Durable learned patterns | Portable, checked-in agent memory shared across tools |
| `docs/ai/business-units/` | Organisational context | Engineering business unit |
| `docs/ai/meta/` | Meta-documentation | Taxonomy, tooling alignment, [adapter surfaces](meta/adapter-surfaces.md), and the document templates (`template-spec.md`, `template-project.md`, `template-unit-test.md`, `template-coverage.md`) |
| `docs/ai/plans/` | Implementation plans | Phased execution plans for major initiatives |


### Top-Level Files

| File | Purpose |
|------|---------|
| `docs/ai/README.md` | This index |
| [`docs/ai/skills/INDEX.md`](skills/INDEX.md) | Per-skill catalog across Brain and adapter tiers |
| [`docs/ai/meta/adapter-surfaces.md`](meta/adapter-surfaces.md) | Single inventory of every AI-tool adapter surface |

### Active Plans

| Plan | Status |
|------|--------|
| [Cross-Platform Tools Migration](plans/cross-platform-tools-migration.md) | Planned |
| [Competitive Analysis](plans/competitive-analysis.md) | Planned |
| [Future Language](plans/future-language.md) | Planned |
| [Core / Common API Decisions](plans/core-common-api-decisions.md) | Open |

Completed plans (v2 Master Plan, v2 PineGuard, Multi-Target Framework, Guard Exception
Policy Uplift, Adapter Naming Collision Review, and others) live in [plans/completed/](plans/completed/).

## How To Use This (in any AI tool)

### If you have an intent ("run tests", "check coverage", "run Qodana", "check Roslyn warnings", "audit")

- Start with the matching command contract in `docs/ai/commands/` — one per family:
  [test](commands/test.md), [coverage](commands/coverage.md), [fix](commands/fix.md),
  [scan](commands/scan.md), [format](commands/format.md), [document](commands/document.md),
  [commit](commands/commit.md), [clean](commands/clean.md), [audit](commands/audit.md),
  [release](commands/release.md), [council](commands/council.md).
- Follow its canonical agent entrypoint in `docs/ai/agents/`.
- Agents should reuse `docs/ai/workflows/` and `docs/ai/skills/`.

### If you are implementing or changing code

1. Read:
   - `docs/ai/specs/spec.md`
   - `docs/ai/specs/orchestration.md`
   - `docs/ai/specs/dependencies.md`
2. Read the domain spec(s) under `docs/ai/specs/<area>/`.
3. Read the domain rules under `docs/ai/rules/<area>.md` (which inherits `docs/ai/rules/global.md`).
4. Use the relevant Skill(s) under `docs/ai/skills/`.
5. Execute via the canonical Agent playbook under `docs/ai/agents/`.

### If you are executing commands (git, PowerShell, Bash, build tools)

**Read [`docs/ai/specs/safety.md`](specs/safety.md) first.** It defines a three-tier classification for every destructive operation:

- **Tier 0 — NEVER**: `git restore .`, `git reset --hard`, `Remove-Item -Recurse` on user dirs, etc.
- **Tier 1 — ASK FIRST**: `git restore <file>`, `dotnet clean`, deleting files the agent did not create, etc.
- **Tier 2 — SAFE WITH PROTOCOL**: `git add <specific-files>`, `dotnet build`, writing to `artifacts/`, etc.

The safety spec also covers scope containment, subagent safety, common gotchas, and recovery guidance.

## Rules Hierarchy

Rules use an inheritance model to avoid duplication:

```
docs/ai/rules/global.md          (always applies — invariants, file hygiene)
    ├── core.md                   (inherits global, adds Core-specific specs)
    ├── must.md                   (inherits global, adds MustClauses specs)
    ├── guard.md                  (inherits global, adds GuardClauses + Must dependency)
    ├── fluent.md                 (inherits global, adds FluentValidation + Must dependency)
    ├── annotation.md             (inherits global, adds DataAnnotations + Must dependency)
    ├── testing.md                (inherits global, adds unit-test specs)
    ├── fixture-conventions.md    (inherits global, adds fixture partial naming + shape conventions)
    ├── tools.md                  (inherits global, adds tooling specs)
    ├── scan.md                   (inherits global, adds SonarQube scan specs)
    ├── roslyn.md                 (inherits global, adds Roslyn compiler diagnostics specs)
    ├── coordination.md           (inherits global, adds multi-session coordination rules)
```

## Skills Inventory

The 16 Brain skills. [`skills/INDEX.md`](skills/INDEX.md) carries the same list with per-skill IDs
and the adapter wrappers that delegate to each one.

| Skill | Directory | Purpose |
|-------|-----------|---------|
| Implement Core Rule/Util | `skills/scaffold-rule/` | Low-level validation primitives |
| Implement MustClauses | `skills/scaffold-must/` | Fluent validation returning MustResult |
| Implement GuardClauses | `skills/scaffold-guard/` | Throw-on-failure guard methods |
| Implement FluentValidation | `skills/scaffold-fluent/` | IRuleBuilder extensions |
| Implement DataAnnotations | `skills/scaffold-annotation/` | ValidationAttribute adapters |
| Implement Unit Tests | `skills/scaffold-unit-test/` | xUnit tests per spec |
| Generate XML Docs | `skills/document/` | Layer-aware XML documentation for all public members |
| Improve Code Coverage | `skills/improve-coverage/` | Coverage gap analysis and filling |
| Format Code | `skills/format-code/` | dotnet format enforcement |
| SonarQube Analysis | `skills/scan-sonar/` | Run SonarQube static analysis |
| Fix Sonar Issues | `skills/fix-sonar/` | Fix SonarQube findings by severity |
| Roslyn Diagnostics | `skills/scan-roslyn/` | Run Roslyn compiler diagnostics |
| Fix Roslyn Warnings | `skills/fix-roslyn/` | Fix compiler warnings by code pattern |
| Scaffold Quality Tool | `skills/scaffold-quality-tool/` | Meta-skill: add a new quality/inspection tool |
| Create Workflow | `skills/scaffold-workflow/` | Agent playbook generation |
| Ask Council | `skills/ask-council/` | Pressure-test a decision via 5 advisors + peer review + chairman synthesis |

## Roles Inventory

All roles are defined in `docs/ai/roles/` and registered in `docs/ai/business-units/engineering.md`.

| Role | Archetype | File | Primary Responsibility |
|------|-----------|------|----------------------|
| Principal Engineer | System Thinker | `roles/principal-engineer.md` | Protocol, tooling strategy, release governance |
| Architect | Guardian | `roles/architect.md` | Strategic design, pattern enforcement |
| Lead Engineer | Coordinator | `roles/lead-engineer.md` | Planning, slicing work, PR coordination |
| Senior Engineer | Owner | `roles/owner.md` | Implement + debug, root-cause analysis |
| Software Engineer | Builder | `roles/builder.md` | Tactical implementation, bug fixing |
| Test Engineer | Verifier | `roles/verifier.md` | Writing tests, running coverage |
| Test Analyst | Planner | `roles/planner.md` | Test strategy, case design, gap analysis |
| Code Reviewer | Critic | `roles/reviewer.md` | PR review, catching drift from specs |
| DevOps Engineer | Shipper | `roles/shipper.md` | CI/CD, packaging, release automation |
| Business Analyst | Clarifier | `roles/business-analyst.md` | Requirements, acceptance criteria |
| Council (Contrarian / First Principles / Expansionist / Outsider / Executor / Chairman) | Multi-perspective Reviewer | `roles/council.md` | Stateless advisor personas used only in the ask-council procedure |

### Claude Code Agent ↔ Brain Role Mapping

| Claude Agent | Brain Role | Archetype |
|---|---|---|
| `.claude/agents/validation-builder.md` | `roles/builder.md` | Builder |
| `.claude/agents/test-writer.md` | `roles/verifier.md` | Verifier |
| `.claude/agents/coverage-analyst.md` | `roles/planner.md` | Planner |
| `.claude/agents/code-reviewer.md` | `roles/reviewer.md` | Critic |
| `.claude/agents/migration-checker.md` | `roles/owner.md` | Owner |

## Adapter Layer (keeps this portable)

Adapters are thin pointers that map tool-specific features to the canonical Brain.

### Surface Inventory

[`meta/adapter-surfaces.md`](meta/adapter-surfaces.md) is the **single inventory** of every surface,
its tier, and where command parity is expected. Do not keep a second copy here — in summary:

- **Root boot files** — `CLAUDE.md`, `AGENTS.md`, `GEMINI.md`.
- **Full adapters** (per-command file format; parity expected) — `.claude/`, `.agent/` (Antigravity),
  `.pi/`, `.github/`.
- **Rules-only adapters** (no command format; parity not expected) — `.clinerules/`, `.cursor/rules/`,
  `.windsurf/rules/`, `.amazonq/rules/`, `.junie/guidelines.md`.

The two sections below detail the native features of the two richest surfaces; every other surface is
described in the inventory.

### GitHub Copilot Adapter (`.github/`)

GitHub Copilot supports native features that **reference** the Brain without duplicating it:

| Feature | Directory | How It Maps to Brain |
|---------|-----------|---------------------|
| Instructions | `.github/copilot-instructions.md` + `.github/instructions/` | Repo-wide + path-scoped adapters → `docs/ai/rules/` |
| Agents | `.github/agents/` | Custom agents → `docs/ai/roles/`, `docs/ai/agents/`, `docs/ai/memory/` |
| Prompts | `.github/prompts/` | Slash-command wrappers → `docs/ai/commands/` and `docs/ai/agents/` |
| Skills | `.github/skills/` | Agent Skills wrappers → `docs/ai/skills/` |
| Shared agent instructions | `AGENTS.md` | Cross-tool always-on adapter → `docs/ai/README.md` |

### Claude Code Adapter (`.claude/`)

Claude Code supports native features that **reference** the Brain without duplicating it:

| Feature | Directory | How It Maps to Brain |
|---------|-----------|---------------------|
| Rules | `.claude/rules/` | Path-scoped adapters → `docs/ai/rules/` |
| Skills | `.claude/skills/` | `context: fork` wrappers → `docs/ai/skills/` |
| Agents | `.claude/agents/` | Native subagents with memory, referencing Brain roles + specs |
| Agent Memory | `.claude/agent-memory/` | Persistent knowledge, seeded from Brain patterns + role directives |
| Hooks | `.claude/hooks/` | Enforcement of file hygiene rules from Brain specs |
| Commands | `.claude/commands/` | Slash commands → `docs/ai/agents/` playbooks |
| Settings | `.claude/settings.json` | Whitelisted commands (all `tools/` scripts) |

**Direction of dependency:** `.claude/` → `docs/ai/` (never the reverse).

Adapters MUST NOT embed logic; they should point to `docs/ai/`.

## Best-Practice DRY Rules

- **Specs** are the only source of truth for normative rules and constraints.
- **Tool READMEs** (`tools/*/README.md`) are the source of truth for operational documentation (usage, parameters, examples).
- **Rules** reference specs — they do not duplicate them.
- **Skills/Workflows** are the only source of truth for procedures.
- **Agents** are composed from Skills/Workflows (don't duplicate instructions).
- **Commands** define the interface contract (intent/trigger → agent).
- **Adapters** (every surface in [`meta/adapter-surfaces.md`](meta/adapter-surfaces.md)) reference the Brain — never the reverse.
- **Specs** reference tool READMEs for usage — they do not duplicate operational docs.
- **Roles** are the source of truth for personas. Agents reference roles — never the reverse.

## References

- Universal protocol: `docs/ai/specs/protocol.md`
- Adapter surfaces: `docs/ai/meta/adapter-surfaces.md`
- Document template: `docs/ai/meta/template-spec.md`

<!-- footer
last_verified: 2026-08-20
-->