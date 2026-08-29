---
spec:
  id: pineguard.ai.protocol
  title: "Universal Agent Protocol"
  version: 1
  parent:
    - spec.md
  dependencies:
    - dependencies.md
applies_to:
  - "docs/ai/**"
  - "CLAUDE.md"
  - "AGENTS.md"
  - "GEMINI.md"
  - ".claude/**"
  - ".agent/**"
  - ".pi/**"
  - ".github/**"
  - ".agents/**"
  - ".codex/**"
  - ".clinerules/**"
  - ".cursor/**"
  - ".windsurf/**"
  - ".amazonq/**"
  - ".junie/**"
---

# Universal Agent Protocol

> [!IMPORTANT]
> The "Brain" is model-agnostic. Adapters translate model-specific intent into Brain functions.

## 1. The Architecture

### The Brain (`docs/ai/*`)

- **Pure Logic**: Contains NO prompt-specific syntax (no xml tags, no slash commands).
- **Structure**:
  - `business-units/`: Organizational Hierarchy.
  - `roles/`: Persona definitions.
  - `commands/`: Interface contracts (intent/trigger → canonical agent entrypoint).
  - `skills/`: Atomic capabilities (How-To).
  - `specs/`: Technical Constraints (Rules).
  - `rules/`: Scope-specific invariants inheriting from `rules/global.md`.
  - `agents/`: Canonical task playbooks / entrypoints for adapters.
  - `workflows/`: Orchestration logic.
  - `meta/`: Taxonomy, tooling alignment, and the adapter-surface inventory.
  - `plans/`: Implementation roadmaps (and their completed archive).
  - `memory/`: Durable notes the Brain keeps across sessions.

### The Adapters

Adapters are lightweight configuration files that "boot" a specific AI tool into the Brain.

The **inventory of adapter surfaces is maintained in one place only**:
[`docs/ai/meta/adapter-surfaces.md`](../meta/adapter-surfaces.md). Do not restate it here — when a
surface is added, removed, or retiered, that file changes and this spec keeps governing it unchanged.

This spec defines the **tiers** that inventory assigns:

| Tier                  | Shape                                                                       | Obligations                                                                                                                                             |
| :-------------------- | :-------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Root boot file**    | A single file loaded at session start (`CLAUDE.md`, `AGENTS.md`, `GEMINI.md`) | Role adoption, the command palette, and a link into `docs/ai/README.md`. Nothing else.                                                                    |
| **Full adapter**      | A per-command file format (a command/prompt/workflow directory)              | MAY carry one pointer file per agent. Each pointer file names exactly one `docs/ai/agents/*.md` playbook. Command parity applies (see the inventory §4).   |
| **Rules-only adapter** | A rules file or rules directory, with no per-command format                  | MUST carry path-scoped pointers ONLY. MUST NOT carry an intent-routing table mapping user phrasing to agent files — that is what `docs/ai/commands/` is for. Command parity does NOT apply, and a missing command directory is not parity debt. |

A duplicated routing table is what rots when agents are renamed. Rules-only adapters point at the
Brain by path scope; intent routing lives in `docs/ai/commands/*.md` and nowhere else.

## 2. Protocol Rules

### Rule #1: thin Triggers

Adapters MUST NOT contain logic. They must only point to a file in `docs/ai/agents`.

- **Bad**: `CLAUDE.md` contains a 50-line coverage script.
- **Good**: `CLAUDE.md` says "Execute `docs/ai/agents/coverage-core.md`".

### Rule #2: Parameterized Workflows

Workflows in `docs/ai` should be generic and accept parameters (Scope, Project, etc.) so they can be reused by all adapters.

### Rule #2.1: Commands are interfaces

Command documents in `docs/ai/commands/*.md` may list example triggers (e.g., “slash commands”), but they are treated as
**portable interface tokens**, not model-specific prompt syntax.

Adapters may expose commands through:

- slash commands,
- buttons,
- palette entries,
- or natural-language intent routing.

### Rule #3: Unidirectional Dependencies

- Adapters depend on The Brain.
- The Brain NEVER depends on Adapters.

## 3. Adding a New Skill/Workflow

1.  **Define it in the Brain**:

- Add/extend atomic guidance in `docs/ai/skills` and constraints in `docs/ai/specs`.
- Add a task playbook in `docs/ai/agents` (this is the canonical entrypoint for adapters).
- Optionally add shared orchestration helpers in `docs/ai/workflows`.

2.  **Expose it via Adapters**: Add a slash command or trigger that points to the `docs/ai/agents/*` file.
    Work the cascade checklist in [`docs/ai/meta/adapter-surfaces.md`](../meta/adapter-surfaces.md) §5 so every
    surface that owes a pointer gets one — and only those that do.
