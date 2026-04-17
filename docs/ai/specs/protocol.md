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
  - `agents/`: Canonical task playbooks / entrypoints for adapters.
  - `workflows/`: Orchestration logic.

### The Adapters

Adapters are lightweight configuration files that "boot" a specific AI model into the Brain.

| Model                    | Adapter File                      | Function                                                                  |
| :----------------------- | :-------------------------------- | :------------------------------------------------------------------------ |
| **Gemini (Antigravity)** | `.agent/workflows/*.md`           | Maps "Turbo" actions to `docs/ai/agents`.                                 |
| **Claude**               | `CLAUDE.md`                       | Maps Slash Commands to `docs/ai/agents`.                                  |
| **GitHub Copilot**       | `.github/copilot-instructions.md` | Maps Natural Language intent to `docs/ai/agents` + supporting Brain docs. |

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
