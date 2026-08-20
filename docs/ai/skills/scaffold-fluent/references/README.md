# Reference Files: scaffold-fluent

> Read the exemplar files listed below before implementing. Do NOT copy them — follow the same patterns.

## Simple Domain

| Role | Exemplar File |
|------|---------------|
| Fluent extension | `src/PineGuard.FluentValidation/FluentJsonExtensions.cs` |
| MustClause (dependency) | `src/PineGuard.MustClauses/MustJsonClauses.cs` |

**Key points:**
- Extension method on `IRuleBuilder<T, TProperty>`
- Uses `ruleBuilder.MustBe(...)` adapter — NOT `.Must(...)` directly
- Passes `paramName: null` to MustClause (FluentValidation handles naming)
- Returns `IRuleBuilderOptions<T, TProperty>`
