<!-- metadata_header
type: meta
id: ai-document-template
version: 2.0
-->

# Brain Document Template

> [!NOTE]
> The shared shape for narrative Brain documents — agents, roles, commands, workflows,
> business units, meta documents and plans.

## Context

Most `docs/ai/**` documents open with an HTML `metadata_header` comment and close with a
`last_verified` footer. This file is the canonical form of that shape.

Two families deliberately use a different header:

- **Specs** carry YAML `spec:` front matter (`id`, `title`, `version`, `parent`, `dependencies`,
  `applies_to`). Use `docs/ai/meta/template-project.md`, `docs/ai/meta/template-coverage.md` or
  `docs/ai/meta/template-unit-test.md` instead. A handful of specs
  (`docs/ai/specs/council.md`, `docs/ai/specs/scan/spec.md`,
  `docs/ai/specs/tools/code-diagnostics/spec.md`) use the `metadata_header` form below.
- **Skills** use the `# Skill:` header form — see [Skill Header](#skill-header).

## Metadata Header Template

```markdown
<!-- metadata_header
type: [agent|role|spec|meta|command|plan|workflow|business-unit]
id: [unique_id]
version: 1.0
-->

# [Title]

> [!NOTE]
> [Brief Summary/Intent of this document]

## Context
[Why this exists. Relationship to other documents (e.g. parent specs).]

## [Content Section 1]
...

## References
- [Related Doc](../path/to/doc.md)

<!-- footer
last_verified: YYYY-MM-DD
-->
```

Rules:

- `id` is stable and kebab-cased, prefixed by its kind (`agent-test-core`, `ai-taxonomy`).
- `type` matches the directory the file lives in.
- Every relative link must resolve from the file's own location.
- `last_verified` is the date the content was last checked against the repository.

Agents add a required blockquote (business unit, roles, optional safety tier) on top of this shape —
see `docs/ai/meta/template-agent.md`.

## Skill Header

Skills open with a title line and two bold metadata lines instead of a `metadata_header`:

```markdown
# Skill: [Imperative Name]

**ID**: pineguard.skill.[verb]-[target]
**Version**: 1.0

## 1. Context & Goal
...

## 2. Inputs
...

## 3. Critical Rules (The "Must Dos")
...

## 4. Steps
...

## 5. Definition of Done
...

## References
...
```

The skill's `ID` matches its directory name: `docs/ai/skills/scaffold-rule/SKILL.md` →
`pineguard.skill.scaffold-rule`.

## References

- Taxonomy: `docs/ai/meta/taxonomy.md`
- Agent shape: `docs/ai/meta/template-agent.md`
- Spec shapes: `docs/ai/meta/template-project.md`, `docs/ai/meta/template-coverage.md`,
  `docs/ai/meta/template-unit-test.md`

<!-- footer
last_verified: 2026-08-20
-->
