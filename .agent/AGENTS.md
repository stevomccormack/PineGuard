# Antigravity Adapter (PineGuard)

> **This file is an Adapter.**
> It maps Antigravity workflows to the canonical Agents in `docs/ai/agents`.
> Do not add logic here. Add logic to the Brain.
> Start at **docs/ai/README.md** for the full Brain index.

## 1. Role Adoption

Before acting, adopt a persona from the Engineering Business Unit:
Read **docs/ai/business-units/engineering.md**

Each workflow delegates to a `docs/ai/agents/<name>.md` playbook; the playbook's `roles:` header is
the authoritative persona for that command.

## 2. Workflows

Antigravity workflows live in `.agent/workflows/`. Each file is a one-line delegation to the Brain
playbook of the same name — there is no per-file logic, and there is no palette to keep in sync.

Surface coverage and the declared parity exceptions (the Claude-Code-only release family) are
recorded once in **docs/ai/meta/adapter-surfaces.md**.

## 3. Safety

Before executing commands, read the safety spec:
Read **docs/ai/specs/safety.md** (Tier 0/1/2 command classification)

The `// turbo-all` annotation carried by most workflows auto-approves **Tier 2 operations only**
(`docs/ai/specs/safety.md` §8.4). Tier 0 and Tier 1 operations are never turbo-eligible, whatever
a workflow file says — the `clean-*` and `ask-council` workflows omit the annotation for that
reason, and the `commit-*` workflows carry an explicit `-Push` / `-SafePush` exclusion.

## 4. Knowledge Base

- **Brain index**: `docs/ai/README.md`
- **Specs**: `docs/ai/specs/` (normative engineering rules, coding standards, testing specs)
- **Safety**: `docs/ai/specs/safety.md` (Tier 0/1/2 command classification)
- **Rules**: `docs/ai/rules/` (scope-specific, inheriting from `global.md`)
- **Skills**: `docs/ai/skills/` (reusable implementation recipes)
- **Agents**: `docs/ai/agents/` (canonical playbooks)
- **Workflows**: `docs/ai/workflows/` (multi-step orchestration)
- **Commands**: `docs/ai/commands/` (intent-to-agent mappings)
- **Roles**: `docs/ai/roles/` (personas and responsibilities)
- **Meta**: `docs/ai/meta/` (taxonomy, tooling alignment, adapter surfaces)
- **Plans**: `docs/ai/plans/` (implementation roadmaps)

## 5. Global Rules

Read `docs/ai/rules/global.md` for invariants that apply to all code in this repository.
