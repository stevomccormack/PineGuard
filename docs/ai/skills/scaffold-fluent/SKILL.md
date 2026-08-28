# Skill: Implement FluentValidation Extension
**ID**: pineguard.skill.scaffold-fluent
**Version**: 1.0

## 1. Context & Goal
Adapt a **MustClause** into a **FluentValidation** extension method (`IRuleBuilder`). This allows FluentValidation users to use PineGuard's domain logic.

## 2. Inputs
- **MustClause**: The underlying `Must.Be.Xxx` logic to adapt.
- **Property Type**: The type being validated (e.g. `string`, `int`, `DateTimeOffset?`).

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> 1.  **Strict Adaptation**: Do NOT write validation logic here. You **MUST** call `Must.Be.Xxx`.
> 2.  **Use The Adapter**: You **MUST** use the `ruleBuilder.MustBe(...)` extension. Do not use `.Must(...)` directly.
> 3.  **ParamName Null**: You **MUST** pass `paramName: null` to the MustClause. The adapter handles message formatting.
> 3b. **Error code**: Pass the invoked clause's own `MustCodes` constant as `MustBe`'s trailing `code` argument — the same constant that clause itself passes to `Fail`/`FromBool`. `MustBe` sets it as the rule's FluentValidation `ErrorCode` directly; it does not read the delegate's own `MustResult.Code`. See `docs/ai/specs/must-clauses/project.md` ("Error codes").
> 4.  **Chaining**: Return `IRuleBuilderOptions<T, TProp>`.
> 5.  **Strict Coding**: File-scoped namespaces, arrow functions (`=>`) for implementation.
> 6.  **Naming Collisions**: If a Must-aligned method name would collide with or strongly mimic a FluentValidation built-in, prefer the clearer PineGuard-specific adapter name documented in `docs/ai/specs/language/naming-collisions.md` (for example, `Required()` / `NotRequired()` instead of `NotNull()` / `Null()`).

## 4. Execution Steps

1.  **Identify Target File**
    *   Folder: `src/PineGuard.FluentValidation/` (project root — there is no `Extensions/` subfolder).
    *   Class: `Fluent[Domain]Extensions` (e.g., `FluentStringExtensions`).

2.  **Implement Extension Method**
    ```csharp
    using FluentValidation;
    using PineGuard.FluentValidation.Common;
    using PineGuard.MustClauses;

    namespace PineGuard.FluentValidation;

    public static class FluentDomainExtensions
    {
        public static IRuleBuilderOptions<T, string?> MyExtension<T>(
            this IRuleBuilder<T, string?> ruleBuilder,
            string? message = null)
            => ruleBuilder.MustBe(value => Must.Be.MyCondition(value, paramName: null), message, MustCodes.Domain.Aspect.Condition);
    }
    ```

3.  **Handle Type Mismatches**
    *   If `TProp` is `string?` but Must returns `MustResult<string>` (non-null), use explicit generic arguments:
    *   `ruleBuilder.MustBe<T, string?, string>(...)`

## 5. Definition of Done
- [ ] Code compiles.
- [ ] Calls `Must.Be.*`.
- [ ] Passes `paramName: null` to Must.
- [ ] Passes the clause's `MustCodes` constant as `MustBe`'s `code` argument.

## 6. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | Build passes | `dotnet build` exits 0 with no warnings |
| 2 | Uses `MustBe` adapter | Calls `ruleBuilder.MustBe(...)`, never `.Must(...)` directly |
| 3 | paramName null | Every Must call passes `paramName: null` |
| 4 | Returns chainable type | Return type is `IRuleBuilderOptions<T, TProp>` |
| 5 | No validation logic | Method body is a single arrow expression delegating to Must |
| 6 | Carries an error code | `MustBe`'s trailing `code` argument matches the invoked clause's own `MustCodes` constant |

## 7. Reference Material
- `docs/ai/specs/fluent-validation/project.md`
- [Reference exemplars](references/README.md)
