---
spec:
  id: pineguard.ai.language.naming-collisions
  title: "Adapter Naming Collisions"
  version: 1
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.DataAnnotations/**"
  - "src/PineGuard.FluentValidation/**"
---

# Adapter Naming Collisions

This specification defines how PineGuard adapter layers must handle naming collisions with framework-native APIs.

## Rule

When an adapter name would collide with, closely mimic, or visually disappear into a framework-native validator, prefer a clearer PineGuard-specific public name.

Priorities:

1. Avoid exact framework collisions.
2. Avoid strong semantic near-collisions that imply framework-native behavior.
3. Prefer explicitness over mechanical parity with Must naming.
4. Do not rely on documentation to rescue an ambiguous public API.

## DataAnnotations

For `PineGuard.DataAnnotations`, compare public attribute usage names after dropping the `Attribute` suffix.

Examples:

- Prefer `WebUrlAttribute` over `UrlAttribute` to avoid colliding with framework-native `[Url]`.
- Prefer type/domain-qualified names when a short adapter name would be misleading in attribute form.

## FluentValidation

For `PineGuard.FluentValidation`, compare public extension method names against FluentValidation built-ins and strong IntelliSense expectations.

Examples:

- Prefer an explicit collision-safe name such as `WebUrl()` when a shorter name like `Url()` would be ambiguous in IntelliSense.
- Prefer `Required()` / `NotRequired()` over `NotNull()` / `Null()` when PineGuard adapts null semantics into FluentValidation, because the built-in validators already own the shorter names.
- Adapter method names may intentionally diverge from Must naming when collision avoidance materially improves clarity.
- When explicitly chosen for ergonomics, a short alias may remain as a thin facade that forwards to the clearer preferred method.

## Family rule

Rename the whole adapter family only when that materially improves clarity. Otherwise, a targeted rename for the collided adapter surface is acceptable.

When both names exist, document one explicit name as preferred and keep any alternate name as a thin forwarder only.
