# Reference Files: scaffold-must

> Read the exemplar files listed below before implementing. Do NOT copy them — follow the same patterns.

## Simple Domain (no Facade)

A simple domain has a single public static class directly in `MustClauses/`.

| Role | Exemplar File |
|------|---------------|
| MustClause | `src/PineGuard.MustClauses/MustJsonClauses.cs` |
| Core Rule (dependency) | `src/PineGuard.Core/Rules/JsonRules.cs` |

**Key points:**
- `public static class` in `namespace PineGuard.MustClauses;`
- Extension method on `this IMustClause _`
- Uses `[CallerArgumentExpression(nameof(value))]` for automatic `paramName`
- Calls Core Rule for logic — no raw validation in Must
- Returns `MustResult<T>.FromBool(...)` or `MustResult<T>.Fail(...)` — never throws
- Null check: return `Fail("{paramName} must not be null.", paramName, value)` for null inputs

## Decision Tree

```
Does the domain have multiple sub-domains requiring a facade?
├── YES → Use Facade pattern (internal impl + public facade)
└── NO  → Use Simple pattern (public static class directly in MustClauses/)
```
