<!-- metadata_header
type: plan
id: refactor-nullability-ordering
version: 1.0
status: completed
-->

# Plan: Nullability and Method Ordering Refactor

> [!NOTE]
> **Archived — shipped.** The `string?` parameter shape and the Positive-before-Negative ordering
> described below are in `src/PineGuard.MustClauses/` and
> `src/PineGuard.FluentValidation/Extensions/`. Kept as the record of the refactor, not as a task list.

This plan covers the refactoring of PineGuard libraries to the "Hybrid Nullability" and "Method Ordering" standards.

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
