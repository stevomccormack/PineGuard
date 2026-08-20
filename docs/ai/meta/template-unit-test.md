---
spec:
  id: pineguard.ai.templates.unit-test
  title: "AI Spec Template — Unit Tests Spec (Addendum)"
  version: 4
  last_verified: 2026-08-20
  parent:
    - ../specs/testing/unit-test.md
  dependencies:
    - ../specs/dependencies.md
applies_to:
  - "docs/ai/specs/**/unit-test.md"
---

# AI Spec Template — Unit Tests Spec (Addendum)

Use this template when creating or normalizing any `docs/ai/specs/**/unit-test.md`.

**Inheritance**: Inherits from `docs/ai/specs/testing/unit-test.md`.

---

## 1) YAML “Spec Header” (required)

All relative paths below are written from the child spec's own location,
`docs/ai/specs/<domain>/unit-test.md`.

```yaml
---
spec:
  id: pineguard.ai.<domain>.unit-test
  title: "<Project> Unit Tests (Addendum)"
  version: 1
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../spec.md
    - ../testing/unit-test.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "tests/<Project>.UnitTests/**"
---
```

Every path must resolve to a file that exists — check it before committing.

---

## 2) Content

- `# <Project> Unit Tests (Addendum)`
- **Implicit Inheritance** (all rules from `docs/ai/specs/testing/unit-test.md` apply):
    - `TheoryData` + `[Theory]` + `[MemberData]` parameterization.
    - Nested Operation Group pattern (§4–5).
    - Element ordering: datasets first, records last (§4.4).
    - Method naming: `Valid_BehavesAsExpected`, `ValidAndEdge_BehavesAsExpected`, `ValidEdgeAndInvalid_BehavesAsExpected`, `Invalid_ThrowsAsExpected` (§5.1).
    - Full canonical examples in §8.
    - Fixtures conventions (§9).

Only add **project-specific** test helpers or deviations here.
