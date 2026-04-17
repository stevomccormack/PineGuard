<!-- metadata_header
type: agent
id: agent-document-all
version: 2.0
-->

# Agent: Generate XML Docs for All Projects

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: builder ([../roles/builder.md](../roles/builder.md))

## Strategy

Use the **Agent tool** to launch one sub-agent per source project **in parallel**.
Each sub-agent generates XML documentation for a single project using the layer-specific template.

> [!NOTE]
> All documentation commands are write operations (file edits) scoped to a single project.
> Use `isolation: "worktree"` to prevent conflicts between parallel agents editing different projects.

## Steps

### 1. Launch parallel sub-agents

Spawn **all five** sub-agents in a **single message** (so they run concurrently):

| Sub-agent name | Project | Agent playbook |
|:---|:---|:---|
| `xml-docs-core` | PineGuard.Core | `docs/ai/agents/document-core.md` |
| `xml-docs-must` | PineGuard.MustClauses | `docs/ai/agents/document-must.md` |
| `xml-docs-guard` | PineGuard.GuardClauses | `docs/ai/agents/document-guard.md` |
| `xml-docs-fluent` | PineGuard.FluentValidation | `docs/ai/agents/document-fluent.md` |
| `xml-docs-da` | PineGuard.DataAnnotations | `docs/ai/agents/document-annotation.md` |

**Sub-agent prompt template** (use `subagent_type: general-purpose`):

```
Generate gold-standard XML documentation for PineGuard.[Project].

1. Read the skill at `docs/ai/skills/document/SKILL.md` completely.
2. Read the specs: `docs/ai/specs/spec.md` and `docs/ai/specs/coding-standard.md`.
3. List all public source files in `src/PineGuard.[Project]/` (exclude obj/, bin/, Common/).
4. For each file:
   a. Read the file completely.
   b. Apply the correct layer template (§5.X from the skill).
   c. Add XML doc comments to ALL public types and members.
   d. Use <see cref> cross-references, <see href> doc links, and <example> blocks.
5. Verify: `dotnet build src/PineGuard.[Project]/PineGuard.[Project].csproj`
6. Report back with:
   - Number of files documented
   - Number of public members documented
   - Any CS1591 warnings remaining
   - "COMPLETE" if all public members documented, "PARTIAL" if gaps remain
```

### 2. Collect results

Wait for all five sub-agents to complete. Collate their results into a single summary table:

| Project | Files | Members | CS1591 Warnings | Status |
|:---|:---|:---|:---|:---|
| Core | ... | ... | ... | COMPLETE / PARTIAL |
| MustClauses | ... | ... | ... | COMPLETE / PARTIAL |
| GuardClauses | ... | ... | ... | COMPLETE / PARTIAL |
| FluentValidation | ... | ... | ... | COMPLETE / PARTIAL |
| DataAnnotations | ... | ... | ... | COMPLETE / PARTIAL |

### 3. Final verification

After all sub-agents complete, run a full solution build to verify no cross-project issues:

```bash
dotnet build PineGuard.sln
```

### 4. Final report

Present the consolidated summary table to the user.
If any project has remaining CS1591 warnings, list the specific undocumented members.
