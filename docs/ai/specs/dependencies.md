---
spec:
  id: pineguard.ai.dependencies
  title: "PineGuard AI Spec Dependencies"
  version: 1
  parent:
    - spec.md
applies_to:
  - "docs/ai/specs/**"
---

# PineGuard AI Spec Dependencies

This document is the **dependency map** for PineGuard’s AI generation specs.

It answers:

- “Which specs must be read before I change X?”
- “Where is the canonical rule for Y?”

This file should stay short and stable. Prefer links over duplicated rules.

---

## 1) Cascading model (summary)

PineGuard uses a “cascading” spec model, analogous to CSS:

- Higher-level specs define **baseline rules**.
- Lower-level (child) specs may add **narrow, domain-specific rules**.
- If a child spec conflicts with a parent spec, the **more specific** child instruction wins.

Canonical definition: `docs/ai/specs/spec.md`.

---

## 2) Root / cross-cutting documents

- Root spec (applies to all): `docs/ai/specs/spec.md`
  - Global defaults and invariants.
  - Required YAML header format for child specs.

- Process + recovery logging: `docs/ai/specs/orchestration.md`
  - What to log, when to log it, and how to avoid losing work.

---

## 3) Generation specs (source of truth)

These define PineGuard’s generator rules for production code and how layers relate.

Canonical locations:

- Core Rules/Utils: `docs/ai/specs/core/project.md`
- MustClauses: `docs/ai/specs/must-clauses/project.md`
- GuardClauses: `docs/ai/specs/guard-clauses/project.md`
- FluentValidation: `docs/ai/specs/fluent-validation/project.md`
- DataAnnotations: `docs/ai/specs/data-annotations/project.md`

---

## 4) Testing + coverage specs

Global shared:

- Unit test conventions (global): `docs/ai/specs/testing/unit-test.md`
- Coverage workflow (global): `docs/ai/specs/testing/coverage.md`

---

## 5) Required references from child specs

Every child spec must include YAML front matter at the top of the file that references:

- `docs/ai/specs/spec.md` (root)
- `docs/ai/specs/dependencies.md` (this file)

Recommended header shape:

```yaml
---
spec:
  id: pineguard.ai.<domain>.<name>
  title: "<Human readable title>"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/<Project>/**"
---
```

Notes:

- Keep `applies_to` accurate; it’s how humans and agents avoid reading the wrong spec.
