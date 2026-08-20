# AI Agent Adapter (PineGuard)

> [!IMPORTANT]
> **This file is an Adapter.**
> It provides generic AI agent instructions that point to the canonical Brain.
> Do not add logic here. Add logic to the Brain.
> 👉 Start at **[docs/ai/README.md](docs/ai/README.md)** for the full Brain index.

## 1. Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
👉 **[docs/ai/business-units/engineering.md](docs/ai/business-units/engineering.md)**

## 2. Follow the Specs (Source of Truth)

All technical truths, conventions, and generation rules are stored in the `specs` directory:
👉 **[docs/ai/specs/](docs/ai/specs/)**

## 3. Read the Rules (Scope-Specific)

Rules provide scope-specific summaries that inherit from a global baseline:
👉 **[docs/ai/rules/](docs/ai/rules/)**

Nested `AGENTS.md` files apply **in addition to** this one: `src/PineGuard.*/AGENTS.md` (per layer),
`tests/AGENTS.md`, `tools/AGENTS.md` and its subdirectories. Read the nearest one for the files you
are editing.

## 4. Use Skills (Implementation Recipes)

Reusable, step-by-step implementation procedures:
👉 **[docs/ai/skills/](docs/ai/skills/)**

## 5. Execute via Agents

Canonical agent playbooks for all workflows:
👉 **[docs/ai/agents/](docs/ai/agents/)**

To route an intent to a playbook, look it up in **[docs/ai/commands/](docs/ai/commands/)** where the
command family has a contract file; otherwise take the command name from the palette in
**[CLAUDE.md](CLAUDE.md)** §2, which lists every command with its role and its playbook. Multi-step
orchestrations live in **[docs/ai/workflows/](docs/ai/workflows/)**.

Each playbook declares its own role on a `roles:` line — that declaration is authoritative, and no
adapter may override it.

## 6. Safety

Before executing commands, read the safety spec:
👉 **[docs/ai/specs/safety.md](docs/ai/specs/safety.md)**

## 7. Knowledge Base

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

## 8. Adapter Surfaces

This file is one of several adapter surfaces. The inventory of all of them — root boot files, full
adapters, rules-only adapters, and the command-parity policy — lives in
👉 **[docs/ai/meta/adapter-surfaces.md](docs/ai/meta/adapter-surfaces.md)**