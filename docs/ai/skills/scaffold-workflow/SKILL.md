# Skill: Create Agent Workflow

**ID**: pineguard.skill.scaffold-workflow
**Version**: 1.0

## 1. Context & Goal

Creates a canonical, model-agnostic agent playbook in `docs/ai/agents/*.md`, and (when needed) a thin Gemini adapter stub in `.agent/workflows/*.md` that points to it.

## 2. Inputs

- **Workflow Name**: Short, descriptive name (e.g., `release-process`).
- **Description**: Brief summary of what the workflow does.
- **Steps**: Ordered list of actions (shell commands or manual instructions).
- **Turbo Mode**: Usage of `// turbo` (single step) or `// turbo-all` (entire workflow).

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
> **Use Absolute Paths**: Always use absolute paths in instructions if possible.

> [!WARNING]
> **Turbo Safety**: Only use `// turbo-all` if the user explicitly requests full automation or if all commands are read-only/safe.

> [!IMPORTANT]
> **Location**: Adapter workflow stubs MUST be placed in `.agent/workflows/`.

> [!IMPORTANT]
> **Canonical first**: The source of truth MUST live in `docs/ai/agents/*.md`. `.agent/workflows/*.md` is an adapter pointer only.

## 4. Execution Steps

1.  **Determine Workflow File Path**
    - Canonical: `docs/ai/agents/[name].md`
    - Adapter stub (Gemini): `.agent/workflows/[name].md`

2.  **Generate Canonical Agent Playbook**
    - Create `docs/ai/agents/[name].md`.
    - Include a `metadata_header` block:
      - `type: agent`
      - `id: agent-[name]`
      - `version: 1.0`
    - Include the required header links:
      - business unit: engineering (`../business-units/engineering.md`)
      - roles: choose the correct primary role for the agent’s intent (see `docs/ai/roles/*.md`).
        - Example: test/coverage agents -> `test-engineer`
        - Example: inspection agents (Qodana) -> `code-reviewer`
        - Example: debug/fix loops -> `senior-engineer`, `test-engineer`
    - Put the actual steps/commands here.

3.  **Generate Adapter Stub (Gemini)**
    - Create `.agent/workflows/[name].md` with YAML frontmatter (`description`).
    - If "Turbo All" is requested, include `// turbo-all`.
    - Body should be minimal and point at the canonical agent:
      ```markdown
      1. Read and execute `docs/ai/agents/[name].md`.
      ```

4.  **Validation**
    - Verify both files end in `.md`.
    - Verify the adapter YAML frontmatter is valid.
    - Verify the adapter contains no embedded scripts/logic.

## 5. Definition of Done

- [ ] Canonical agent playbook created in `docs/ai/agents/`.
- [ ] Adapter stub created/updated in `.agent/workflows/` (if needed).
- [ ] Adapter points to the canonical agent.

## 6. Reference Material (Deep Dive)

- `docs/ai/specs/orchestration.md` (Agent Workflows section)
- `docs/ai/meta/taxonomy.md` (how roles/agents/skills/commands compose)
