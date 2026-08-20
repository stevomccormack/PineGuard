---
spec:
  id: pineguard.ai.templates.coverage
  title: "AI Spec Template — Code Coverage Spec (Addendum)"
  version: 3
  last_verified: 2026-08-20
  parent:
    - ../specs/testing/coverage.md
  dependencies:
    - ../specs/dependencies.md
applies_to:
  - "docs/ai/specs/**/coverage.md"
---

# AI Spec Template — Code Coverage Spec (Addendum)

Use this template when creating or normalizing any `docs/ai/specs/**/coverage.md`.

**Inheritance**: Inherits from `docs/ai/specs/testing/coverage.md`.

---

## 1) YAML “Spec Header” (required)

All relative paths below are written from the child spec's own location,
`docs/ai/specs/<domain>/coverage.md`.

```yaml
---
spec:
  id: pineguard.ai.<domain>.code-coverage
  title: "<Project> Code Coverage (Addendum)"
  version: 1
  template:
    - ../../meta/template-coverage.md
  parent:
    - ../spec.md
    - ../testing/coverage.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "tests/<Project>.UnitTests/**"
---
```

Every path must resolve to a file that exists — check it before committing.

---

## 2) Content

- `# <Project> Code Coverage (Addendum)`
- **Implicit Inheritance**:
    - Tooling (`dotnet test`, collectors).
    - Thresholds (100% logic).

Only document **project-specific** constraints (e.g., specific exclusion attribution).
