<!-- metadata_header
type: meta
id: ai-template-agent
version: 1.0
-->

# Agent Playbook Template

> [!NOTE]
> The required shape for every file in `docs/ai/agents/`. Agents are the canonical,
> model-agnostic playbooks that adapters point at; keeping them one shape is what lets
> an adapter stay a one-line pointer.

## Context

`docs/ai/meta/template-spec.md` defines the shared `metadata_header` + `last_verified` shape.
An agent adds two things on top of it: a role-binding blockquote, and literal runnable steps.

Agents are **thin composition** (`docs/ai/meta/taxonomy.md`). If a step is longer than a few
lines, it belongs in a workflow (`docs/ai/workflows/`) or a skill (`docs/ai/skills/`), and the
agent references it.

## Skeleton

```markdown
<!-- metadata_header
type: agent
id: agent-[verb]-[scope]
version: 1.0
-->

# Agent: [Imperative Title]

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: [role] ([../roles/[role].md](../roles/[role].md))

## Purpose

[One paragraph: what this playbook accomplishes and when to reach for it.]

## Inputs

| Parameter | Required | Description |
|-----------|----------|-------------|
| `Scope`   | ✓        | ... |

## Steps

1. [Literal, runnable instruction — a command, or "read X and execute it with parameter Y".]

## Related

- [Related agent / workflow / spec / script — omit on delegating stubs]
```

## Required Elements

| Element | Required | Notes |
|---------|----------|-------|
| `metadata_header` | Always | `type: agent`, `id: agent-<filename>`, `version` |
| H1 `# Agent: …` | Always | Imperative phrasing |
| `business unit` line | Always | Currently always `engineering` |
| `roles` line | Always | One or more of the eleven roles in `docs/ai/roles/` — never a display persona with no role file |
| `safety tier` line | When the playbook deletes, pushes, or publishes | Cite the tier from `docs/ai/specs/safety.md` and what the operator must confirm |
| `adapter surface` line | When the agent is a declared parity exception | Point at `docs/ai/meta/adapter-surfaces.md` §4 |
| `## Purpose` | When the title is not self-evident | Omit on trivial delegating stubs |
| `## Inputs` | When parameterised | Table form |
| `## Steps` | Always | Literal commands, or a delegation to a workflow with explicit parameters |
| `## Related` | When the agent has links beyond its Steps | Delegating stubs omit it — the workflow reference in Steps is enough. Standalone playbooks (clean, release) carry it |
| `last_verified` footer | Never on agents | Verification dates live on the narrative docs and specs; a 16-line stub is verified by the workflow it delegates to |

## Roles Are Canonical

The `roles:` line must name a file that exists in `docs/ai/roles/`: `architect`, `builder`,
`business-analyst`, `council`, `lead-engineer`, `owner`, `planner`, `principal-engineer`,
`reviewer`, `shipper`, `verifier`. Adapters may present a friendlier display persona
("Test Engineer", "Code Reviewer"), but the persona must resolve to the role declared here —
this file is the authority, not the adapter.

## Delegating Stubs

Most per-scope agents are three lines of delegation. That is the intended shape:

```markdown
## Steps

1. Read the master workflow at `docs/ai/workflows/test.md`.
2. Execute it with parameter **Scope = Core**.
```

A stub still carries the metadata header and the role blockquote; it omits `## Related` and the
footer.

## References

- Taxonomy: `docs/ai/meta/taxonomy.md`
- Document shape: `docs/ai/meta/template-spec.md`
- Safety tiers: `docs/ai/specs/safety.md`
- Adapter inventory: `docs/ai/meta/adapter-surfaces.md`

<!-- footer
last_verified: 2026-08-20
-->
