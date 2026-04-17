---
spec:
  id: pineguard.ai.templates.project-spec
  title: "AI Spec Template — Project Spec"
  version: 2
  parent:
    - ../project.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "docs/ai/specs/**/project.md"
---

# AI Spec Template — Project Spec

Use this template when creating or normalizing any `docs/ai/specs/**/project.md`.

**Inheritance**: This template assumes the spec inherits from `docs/ai/specs/project.md` (The Base Spec).

---

## 1) YAML “Spec Header” (required)

Every child `project.md` must start with YAML front matter.

Copy/paste skeleton (replace placeholders):

```yaml
---
spec:
  id: pineguard.ai.<domain>.project-spec
  title: "<Project> Project Spec"
  version: 1
  template:
    - ../template-project.md
  parent:
    - ../project.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/<Project>/**"
---
```

Header rules:
- `spec.id` is stable and kebab/period cased.
- `spec.template` must reference `docs/ai/meta/template-project.md`.
- `parent` must reference `../project.md`.

---

## 2) Body Structure

Start with:

- `# <Project> Project Spec`
- A short paragraph describing the specific domain.

**Implicit Inheritance**:
Do not repeat "Feature Implementation Checklist", "Standard Domains Playbook", or "Coding Standards". These are inherited from the base spec.

Include these sections ONLY if you have specific addendums:

- `## Domain Specifics`
- `## 1. ...` (Numbered rules)

---

## 3) Project-specific rules

Focus on what makes this project *unique*.

- **Do not duplicate global invariants** (e.g., Analyzer rules, Nullability).
- **Do not duplicate base spec invariants** (e.g., File naming, Namespace rules).
