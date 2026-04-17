---
spec:
  id: pineguard.ai.specs.project-spec
  title: "Base Project Spec (Production Code)"
  version: 1
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/**"
---

# Base Project Spec (Production Code)

This is the **base specification** for all PineGuard production code projects (`src/PineGuard.*`).
Specific project specs (e.g., `src/core/project.md`) inherit from this file.

## Inheritance Structure

1.  **Global Spec** (`docs/ai/specs/spec.md`): Universal rules (process, determinism, rigorous engineering).
2.  **Base Spec** (this file): Rules specific to **production C# code** (not tests/coverage).
3.  **Project Spec** (e.g., `core/project.md`): Domain-specific logic.

---

## 1. Feature Implementation Checklist

All production projects MUST follow the Master Checklist defined in **`docs/ai/specs/spec.md` §3**.

Summary of layering (strict):
1.  **Utils** (`PineGuard.Core.Utils`): Parsing/normalization. No throwing.
2.  **Rules** (`PineGuard.Core.Rules`): Pure predicates. No throwing.
3.  **MustClauses** (`PineGuard.MustClauses`): Canonical messages. Returns `MustResult`.
4.  **GuardClauses** (`PineGuard.GuardClauses`): Throws via MustClauses.
5.  **Integrations**: Adapters (FluentValidation/DataAnnotations).

---

## 2. Production Code Standards

### 2.1 Namespace & File Structure
- **One class per file**.
- **File name must match class name**.
- **File-scoped namespaces** required (`namespace PineGuard.Rules;`).
- **Using statements**:
    - Place at the very top.
    - Sort alphabetically.
    - Remove unused.

### 2.2 Nullability
- **Enable Nullable Reference Types (NRT)**.
- Use `?` for standard nullable intents.
- Avoid `!` (null-forgiving) in production code unless verifying a guaranteed invariant that the compiler cannot see.
- **Library invariants**: We prefer to *handle* nulls (return `false`/`Fail`) rather than throwing `NullReferenceException`.

### 2.3 Analyzer & ReSharper Compliance
See **[`docs/ai/specs/coding-standard.md`](./coding-standard.md)**.
- **No warnings** allowed in new code.
- **Do not suppress** warnings without a critical, documented reason.
- Prefer `[CodeAnalysis]` attributes over `#pragma` if suppression is inevitable.

---

## 3. Verification
- All production changes must compile without warnings.
- Verification is typically done by running the associated **Unit Tests**.
