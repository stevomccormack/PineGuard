# Skill: Create Agent Workflow

**ID**: pineguard.skill.scaffold-workflow
**Version**: 2.0

## 1. Context & Goal

Creates a canonical, model-agnostic agent playbook in `docs/ai/agents/*.md` **and cascades it to every
adapter surface** so the new agent is reachable from every tool the repository supports.

The authoritative inventory of surfaces is `docs/ai/meta/adapter-surfaces.md`. This skill does not
maintain its own list — read that file, then work its §5 cascade checklist row by row.

## 2. Inputs

- **Agent Name**: Short, descriptive, `<verb>-<subject>[-<scope>]` (e.g. `scan-qodana-core`).
- **Description**: Brief summary of what the agent does.
- **Role**: The primary role from `docs/ai/roles/` (see §3).
- **Steps**: Ordered list of actions (shell commands or manual instructions).
- **Turbo Mode**: Usage of `// turbo` (single step) or `// turbo-all` (entire workflow).

## 3. Critical Rules (The "Must Dos")

> [!IMPORTANT]
> **Canonical first**: The source of truth MUST live in `docs/ai/agents/*.md`. Every adapter file is a
> pointer only — no logic, no restated steps (`docs/ai/specs/protocol.md` Rule #1).

> [!IMPORTANT]
> **Cascade or declare**: Every row of `docs/ai/meta/adapter-surfaces.md` §5 is either **done** or
> **N/A under a declared policy** (§4 of that file). An omitted row is parity debt, and the audit-cli
> adapter-parity rule will fail on it.

> [!IMPORTANT]
> **Role canon**: The role named in the playbook MUST be one of the eleven files in `docs/ai/roles/`
> (architect, builder, business-analyst, council, lead-engineer, owner, planner, principal-engineer,
> reviewer, shipper, verifier). Adapters may show a friendlier display persona, but it must resolve to
> the role the playbook declares — the playbook is authoritative.

> [!IMPORTANT]
> **Use Absolute Paths**: Always use absolute paths in instructions where possible.

> [!WARNING]
> **Turbo Safety**: Only use `// turbo-all` if the user explicitly requests full automation or if all
> commands are read-only/safe.

> [!WARNING]
> **Tier 0/1 agents are Claude-Code-only**: Agents performing irreversible operations (releases, branch
> protection, package unlisting — see `docs/ai/specs/safety.md`) MUST NOT be generated onto surfaces
> that apply blanket auto-approval. Mark those rows N/A citing the release-family exception.

## 4. Execution Steps

1.  **Read the surface inventory**
    - `docs/ai/meta/adapter-surfaces.md` — §2 full adapters, §3 rules-only adapters, §4 declared
      parity exceptions, §5 cascade checklist.
    - Decide up front which rows are N/A for this agent, and why.

2.  **Generate the canonical agent playbook** — `docs/ai/agents/<name>.md`
    - Include a `metadata_header` block: `type: agent`, `id: agent-<name>`, `version: 1.0`.
    - Include the required header links:
      - business unit: engineering (`docs/ai/business-units/engineering.md`)
      - roles: the canonical role file(s) from `docs/ai/roles/` (§3).
    - Put the actual steps/commands here. This is the only file that carries them.

3.  **Cascade to every surface** — produce or explicitly N/A each row:

    | # | Output | Shape | N/A when |
    |---|--------|-------|----------|
    | 1 | `docs/ai/agents/<name>.md` | The playbook (source of truth) | never |
    | 2 | `docs/ai/commands/<family>.md` | Row in the family's intent contract table | the agent belongs to no command family |
    | 3 | `.claude/commands/<name>.md` | `Act as **<Role>**. Read and execute docs/ai/agents/<name>.md.` | never |
    | 4 | `CLAUDE.md` | Palette row under the matching `###` section | never |
    | 5 | `.agent/workflows/<name>.md` | YAML frontmatter (`description`) + `1. Read and execute docs/ai/agents/<name>.md.` | release family (§4) |
    | 6 | `.pi/prompts/<name>.md` | Pi prompt pointing at the playbook | release family (§4) |
    | 7 | `.pi/AGENTS.md` | Palette row | release family (§4) |
    | 8 | `.github/prompts/<name>.prompt.md` | Copilot prompt pointing at the playbook | agent is outside the declared Copilot subset (§4) |
    | 9 | `.agents/skills/<name>/SKILL.md`, `.codex/agents/<name>.toml` (§2.1) | One `SKILL.md` per Brain skill; TOML per Codex agent | ordinary agent changes; only touch these if the change adds or renames a **skill or subagent** |
    | 10 | Rules-only adapters (§3) | — | ordinary agent changes; only touch these if the change alters a **layer mapping** |
    | 11 | `.vscode/tasks.json` | Task entry | the agent has no task-runner equivalent |

    Rows 5–7 take `// turbo-all` only under the Turbo Safety rule above.

4.  **Validation**
    - Verify every generated file exists at the path written and ends in `.md`.
    - Verify adapter YAML frontmatter is valid.
    - Verify no adapter contains embedded scripts, steps, or logic — only a pointer.
    - Verify the role named in each adapter resolves to the role the playbook declares.
    - Run `pwsh ./tools/audit-cli/Run-All.ps1` and confirm the adapter-parity rule is clean.

## 5. Definition of Done

One checkbox per cascade row. Tick **Done** or **N/A (policy)** — never leave one blank.

- [ ] `docs/ai/agents/<name>.md` created, with a canonical role from `docs/ai/roles/`
- [ ] `docs/ai/commands/<family>.md` updated (or N/A — no command family)
- [ ] `.claude/commands/<name>.md` created
- [ ] `CLAUDE.md` palette row added
- [ ] `.agent/workflows/<name>.md` created (or N/A — release family)
- [ ] `.pi/prompts/<name>.md` created (or N/A — release family)
- [ ] `.pi/AGENTS.md` palette row added (or N/A — release family)
- [ ] `.github/prompts/<name>.prompt.md` created (or N/A — outside the Copilot subset)
- [ ] `.agents/skills/<name>/SKILL.md` and `.codex/agents/<name>.toml` created (or N/A — no skill/subagent change)
- [ ] Rules-only adapters reviewed (or N/A — no layer-mapping change)
- [ ] `.vscode/tasks.json` updated (or N/A — no task-runner equivalent)
- [ ] Every adapter is a pointer only, and names the playbook's role
- [ ] `tools/audit-cli` adapter-parity rule passes

## 6. Reference Material (Deep Dive)

- `docs/ai/meta/adapter-surfaces.md` — the surface inventory and cascade checklist (authoritative)
- `docs/ai/specs/protocol.md` — normative Brain/Adapter contract
- `docs/ai/specs/safety.md` — Tier 0/1/2 command classification
- `docs/ai/specs/orchestration.md` (Agent Workflows section)
- `docs/ai/meta/taxonomy.md` (how roles/agents/skills/commands compose)
