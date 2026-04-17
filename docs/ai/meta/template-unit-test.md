---
spec:
  id: pineguard.ai.templates.unit-test
  title: "AI Spec Template — Unit Tests Spec (Addendum)"
  version: 3
  parent:
    - ../testing/unit-test.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "docs/ai/specs/**/unit-test.md"
---

# AI Spec Template — Unit Tests Spec (Addendum)

Use this template when creating or normalizing any `docs/ai/specs/**/unit-test.md`.

**Inheritance**: Inherits from `docs/ai/specs/testing/unit-test.md`.

---

## 1) YAML “Spec Header” (required)

```yaml
---
spec:
  id: pineguard.ai.<domain>.unit-test
  title: "<Project> Unit Tests (Addendum)"
  version: 1
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../unit-test.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "tests/<Project>.UnitTests/**"
---
```

---

## 2) Content

- `# <Project> Unit Tests (Addendum)`
- **Implicit Inheritance** (all rules from `docs/ai/specs/testing/unit-test.md` apply):
    - `TheoryData` + `[Theory]` + `[MemberData]` parameterization.
    - Nested Operation Group pattern (§4–5).
    - Element ordering: datasets first, records last (§4.4).
    - Method naming: `Valid_BehavesAsExpected`, `ValidAndEdge_BehavesAsExpected`, `Invalid_ThrowsAsExpected` (§5.1).
    - Full canonical examples in §9.

Only add **project-specific** test helpers or deviations here.
