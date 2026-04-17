# Gemini Adapter (PineGuard)

> [!IMPORTANT]
> **This file is an Adapter.**
> It maps Gemini interactions to the canonical Brain in `docs/ai/`.
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

## 4. Use Skills (Implementation Recipes)

Reusable, step-by-step implementation procedures:
👉 **[docs/ai/skills/](docs/ai/skills/)**

## 5. Execute via Agents

Canonical agent playbooks for all workflows:
👉 **[docs/ai/agents/](docs/ai/agents/)**

Gemini workflow stubs are in `.agent/workflows/` — each delegates to the canonical Brain agent.

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
- **Architecture**: `docs/ai/specs/` (structural design)
- **Meta**: `docs/ai/meta/` (taxonomy, tooling alignment)
- **Plans**: `docs/ai/plans/` (implementation roadmaps)