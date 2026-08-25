<!-- metadata_header
type: command
id: cmd-scaffold
version: 1.0
-->

# Command: Scaffold

Creates new code from a recipe — a single layer, or a feature across every layer.

## Intent Mapping

| Command | Intent | Agent |
|---------|--------|-------|
| `/scaffold-vertical-slice` | Implement a feature across Core → Must → Guard → adapters → tests | `docs/ai/agents/scaffold-vertical-slice.md` |

### Skill-routed scaffolds (by design, no agent)

The single-layer scaffolds are **Skills, not Agents**: they are interactive recipes invoked by
trigger phrase or adapter skill wrapper, not batch playbooks. This is a deliberate exception to the
command → agent chain — the vertical slice above is the only scaffold with enough orchestration to
warrant an agent.

| Intent | Canonical procedure |
|--------|---------------------|
| New Core rule or util | `docs/ai/skills/scaffold-rule/SKILL.md` |
| New Must clause | `docs/ai/skills/scaffold-must/SKILL.md` |
| New Guard clause | `docs/ai/skills/scaffold-guard/SKILL.md` |
| New FluentValidation extension | `docs/ai/skills/scaffold-fluent/SKILL.md` |
| New DataAnnotations attribute | `docs/ai/skills/scaffold-annotation/SKILL.md` |
| New unit tests for any class | `docs/ai/skills/scaffold-unit-test/SKILL.md` |
| Simple validation across all layers | `docs/ai/skills/new-validation/SKILL.md` |
| New quality/inspection tool (meta) | `docs/ai/skills/scaffold-quality-tool/SKILL.md` |
| New agent playbook + adapters (meta) | `docs/ai/skills/scaffold-workflow/SKILL.md` |

## Auto-Approval

Nothing in this family is auto-approved — every scaffold writes source files across one or more
projects and requires explicit user intent.
