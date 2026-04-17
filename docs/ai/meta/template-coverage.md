---
spec:
  id: pineguard.ai.templates.coverage
  title: "AI Spec Template — Code Coverage Spec (Addendum)"
  version: 2
  parent:
    - ../testing/coverage.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "docs/ai/specs/**/coverage.md"
---

# AI Spec Template — Code Coverage Spec (Addendum)

Use this template when creating or normalizing any `docs/ai/specs/**/coverage.md`.

**Inheritance**: Inherits from `docs/ai/specs/coverage.md`.

---

## 1) YAML “Spec Header” (required)

```yaml
---
spec:
  id: pineguard.ai.<domain>.code-coverage
  title: "<Project> Code Coverage (Addendum)"
  version: 1
  template:
    - ../template-coverage.md
  parent:
    - ../coverage.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "tests/<Project>.UnitTests/**"
---
```

---

## 2) Content

- `# <Project> Code Coverage (Addendum)`
- **Implicit Inheritance**:
    - Tooling (`dotnet test`, collectors).
    - Thresholds (100% logic).

Only document **project-specific** constraints (e.g., specific exclusion attribution).
