---
description: Refactor Nullability and Method Ordering
---

This workflow automates the refactoring of PineGuard libraries to the "Hybrid Nullability" and "Method Ordering" standards.

// turbo-all

1.  **MustClauses**: Refactor `string` parameters to `string?`.
    *   Target: `src/PineGuard.MustClauses/MustString*.cs` (and any file with `string value` inputs).
    *   Action: Change `this IMustClause _, string value` to `this IMustClause _, string? value`.
    *   Action: Ensure `Positive` methods appear before `Negative` methods.

2.  **FluentValidation**: Refactor `string` parameters via `IRuleBuilder<T, string>`.
    *   Target: `src/PineGuard.FluentValidation/Extensions/**`.
    *   Action: Ensure extensions support `string?` (nullable).
    *   Action: Ensure `Positive` methods appear before `Negative` methods.

3.  **DataAnnotations**: Refactor attributes.
    *   Target: `src/PineGuard.DataAnnotations/**`.
    *   Action: Ensure `Positive` methods appear before `Negative` methods.

4.  **GuardClauses**: Reorder methods.
    *   Target: `src/PineGuard.GuardClauses/**`.
    *   Action: Ensure `Negative` methods appear before `Positive` methods.

5.  **Verify**: Run tests.
    *   Command: `dotnet test`
