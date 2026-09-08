---
title: Adapter Surfaces
type: meta
status: normative
last_verified: 2026-09-08
---

# Adapter Surfaces

> [!IMPORTANT]
> This file is the **single inventory** of every AI-tool surface in this repository.
> `docs/ai/README.md`, `docs/ai/specs/protocol.md`, `docs/ai/skills/scaffold-workflow/SKILL.md`
> and the `surface-parity` audit rule in `apps/cli/` all reference this file rather than
> maintaining their own lists. When a surface is added, removed, or retiered, change it **here**
> and let the references follow.

The Brain lives in `docs/ai/`. Everything listed below is an **adapter**: a thin pointer layer
that routes a tool's native entry points into the Brain. Adapters carry no logic of their own
(`docs/ai/specs/protocol.md` Rule #1).

There are **twelve** adapter surfaces (five full, two skill/hook-only, five rules-only — plus two
legacy single-file variants) and four boot files. Tiering matters: what counts as parity
debt differs per tier, and treating a rules-only surface as though it were missing dozens of
command files generates directories no tool reads.

## 1. Root boot files

Loaded automatically by their tool when a session starts in the repository root. Each one is an
entry point only: role adoption, the command palette, and a link into the Brain index.

| File | Tool | Notes |
|------|------|-------|
| `CLAUDE.md` | Claude Code | Carries the canonical slash-command palette. |
| `AGENTS.md` | Generic / OpenAI-style agents | Tool-neutral phrasing of the same contract; read natively by OpenCode as its primary instructions file. |
| `GEMINI.md` | Gemini | Adds the `.agent/workflows/` pointer. |
| `.github/copilot-instructions.md` | GitHub Copilot | Carries the Copilot prompt-file palette. Lives under `.github/` rather than the root because that is where Copilot loads it from; it is injected into every Copilot request exactly as the root files are loaded. |

## 2. Full adapters

Surfaces with a **per-command file format**. These are checked for command parity against
`docs/ai/agents/*.md` (see §4).

| Surface | Tool | Command dir | Skills | Other |
|---------|------|-------------|--------|-------|
| `.claude/` | Claude Code | `commands/` | `skills/` | `rules/`, `agents/`, `agent-memory/`, `hooks/` |
| `.agent/` | Antigravity | `workflows/` | — | — |
| `.pi/` | Pi | `prompts/` | `skills/` | `AGENTS.md`, `extensions/` |
| `.github/` | GitHub Copilot | `prompts/` | `skills/` | `copilot-instructions.md`, `instructions/`, `agents/` |
| `.opencode/` | OpenCode | `commands/` | — | — |

> [!WARNING]
> `.agent/` (singular, Antigravity workflows) and `.agents/` (plural, skills — see §2.1) are
> **different surfaces**. Neither is the Gemini adapter; that is the root `GEMINI.md`. Conflating
> `.agent/` with Gemini is a long-standing documentation error — do not reintroduce it.

`.opencode/` carries commands only, by design. OpenCode reads the root `AGENTS.md` as its
instructions file and natively loads `.claude/skills/` and `.agents/skills/`, so it needs no boot
file, skills directory, or rules directory of its own. Its subagent format (`.opencode/agent/`) and
its JavaScript plugin system are incompatible with `.claude/agents/` and `.claude/hooks/`;
OpenCode-native rebuilds of those are deferred and are not parity debt.

### 2.1 Skill- and hook-only adapters

Surfaces with no per-command file format, but more than a rules file. They are **not** checked for
command parity; they are checked for skill-name and hook-path resolution.

| Surface | Tool | Shape |
|---------|------|-------|
| `.agents/skills/` | Generic `AGENTS.md`-convention tools | One `SKILL.md` directory per Brain skill, mirroring `docs/ai/skills/` (the roster lives in [`docs/ai/skills/INDEX.md`](../skills/INDEX.md) — do not maintain a count here) |
| `.codex/` | OpenAI Codex | `agents/*.toml`, `hooks/*.sh`, `hooks.json`, `config.toml` |

Skill files on these surfaces MUST use the **current** Brain skill names — a directory carrying a
retired verb (taxonomy §N.3) is drift, not a variant.

## 3. Rules-only adapters

These tools have **no per-command prompt-file format**; they read a rules file or rules directory
only. A missing command directory on these surfaces is **not** parity debt, and the parity rule
must not report one.

| Surface | Tool | Shape |
|---------|------|-------|
| `.clinerules/` | Cline | Numbered rule files (`01-global.md`, …) |
| `.cursor/rules/` | Cursor | `*.mdc` with frontmatter globs — current Cursor format |
| `.windsurf/rules/` | Windsurf | `global.md`, `layers.md` — current Windsurf format |
| `.amazonq/rules/` | Amazon Q | Plain rule files |
| `.junie/guidelines.md` | JetBrains Junie | Single guidelines file |

Rules-only adapters carry **path-scoped pointers only**. They MUST NOT carry an intent-routing
table mapping user phrasing to agent files — that is what `docs/ai/commands/` is for, and a
duplicated routing table is exactly what rots when agents are renamed.

### 3.1 Legacy single-file variants

`.cursorrules` and `.windsurfrules` are the pre-directory formats for Cursor and Windsurf. They are
retained **only** for older editor builds and are reduced to pointer stubs. Never grow them back:
the directory formats above are authoritative, and maintaining two files per tool is what let both
legacy files drift a full rename cycle behind the Brain.

## 4. Parity policy

Command parity is expected across the five **full adapters** in §2, with these declared exceptions.
Anything not listed here is parity debt and the audit-cli adapter-parity rule will fail on it.

| Exception | Surfaces | Rationale |
|-----------|----------|-----------|
| **Release family** — `github-release-publish`, `github-ruleset-enable`, `github-ruleset-disable`, `nuget-unlist` | Claude Code only | These publish releases, mutate branch protection, and unlist packages from nuget.org — the Tier 0/1 irreversible operations of `docs/ai/specs/safety.md`. They MUST NOT be exposed on surfaces that apply blanket auto-approval. |

The exception is a **decision, not a gap**. A future parity pass must read this table before
generating missing files. The former reduced GitHub Copilot prompt roster was retired on
2026-09-08: `.github/prompts/` now carries every non-release command, so the surface is checked
for full parity like the others.

## 5. Cascade checklist

When an agent is added, renamed, or removed, the change cascades to every row below. This list
supersedes the Phase 8 table in `docs/ai/plans/completed/naming-convention-rename.md`, which
omitted three surfaces and thereby produced the drift this file exists to prevent.

- [ ] `docs/ai/agents/<name>.md` — the playbook (source of truth)
- [ ] `docs/ai/commands/<family>.md` — the intent contract, if the family has one
- [ ] `.claude/commands/<name>.md`
- [ ] `CLAUDE.md` — palette row
- [ ] `.agent/workflows/<name>.md`
- [ ] `.pi/prompts/<name>.md`
- [ ] `.pi/AGENTS.md` — palette row
- [ ] `.github/prompts/<name>.prompt.md` — release family excepted (§4)
- [ ] `.github/copilot-instructions.md` — palette row
- [ ] `.opencode/commands/<name>.md` — release family excepted (§4)
- [ ] `.agents/skills/<name>/SKILL.md`, `.github/skills/<name>/SKILL.md` and `.codex/agents/<name>.toml` (§2.1) — only when the change adds or renames a **skill or subagent**, not for ordinary agent changes
- [ ] Rules-only adapters (§3) — only if the change alters a **layer mapping**, not for ordinary agent changes
- [ ] `.vscode/tasks.json` — only if the agent has a task-runner equivalent
- [ ] `.github/agents/<role>.agent.md` — only when the change adds or renames a **role** in `docs/ai/roles/`, not for ordinary agent changes

## 6. Related

- `docs/ai/README.md` — Brain index
- `docs/ai/specs/protocol.md` — normative Brain/Adapter contract
- `docs/ai/specs/safety.md` — Tier 0/1/2 command classification
- `docs/ai/meta/tooling.md` — tool-by-tool configuration detail
- `docs/ai/skills/scaffold-workflow/SKILL.md` — authoring a new agent across surfaces
