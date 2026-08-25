---
spec:
  id: pineguard.ai.specs.project-spec
  title: "Base Project Spec (Production Code)"
  version: 1
  parent:
    - spec.md
  dependencies:
    - dependencies.md
applies_to:
  - "src/**"
---

# Base Project Spec (Production Code)

This is the **base specification** for all PineGuard production code projects (`src/PineGuard.*`).
Specific project specs (e.g., `docs/ai/specs/core/project.md`) inherit from this file.

## Inheritance Structure

1.  **Global Spec** (`docs/ai/specs/spec.md`): Universal rules (process, determinism, rigorous engineering).
2.  **Base Spec** (this file): Rules specific to **production C# code** (not tests/coverage).
3.  **Project Spec** (e.g., `core/project.md`): Domain-specific logic.

---

## 1. Feature Implementation Checklist

All production projects MUST follow the Master Checklist defined in **`docs/ai/specs/spec.md` §3**.

### 1.1 Layer pipeline (canonical)

PineGuard layers in one direction — each layer calls only the one before it:

- **Core** (`Rules`/`Utils`) owns validation logic and parsing. Utils parse/normalize, Rules are pure predicates — neither throws.
- **MustClauses** call Core and own the canonical, user-facing messages (`MustResult<T>`).
- **GuardClauses** call MustClauses and throw using `MustResult.Message`.
- **FluentValidation** adapts MustClauses into `IRuleBuilder` extensions.
- **DataAnnotations** adapts MustClauses into `ValidationAttribute`s.

Guard, Fluent and DataAnnotations are sibling adapters over Must — none calls another, and none
reimplements Core logic. Do not duplicate parsing/validation logic across layers.

This section is the **single canonical statement** of the pipeline (`spec.md` §9 — commonality
extraction). Child project specs reference it rather than restating the bullets.

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
