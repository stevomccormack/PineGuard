# Reference Files: scaffold-guards

> Read the exemplar files listed below before implementing. Do NOT copy them — follow the same patterns.

## Simple Domain

| Role | Exemplar File |
|------|---------------|
| GuardClause | `src/PineGuard.GuardClauses/GuardJsonClauses.cs` |
| MustClause (dependency) | `src/PineGuard.MustClauses/MustJsonClauses.cs` |

**Key points:**
- Extension method on `this IGuardClause _`
- Calls the corresponding MustClause, then throws on failure
- Uses `GuardExceptionPolicy` to determine exception type
- Returns the validated value on success (pass-through for chaining)

## Decision Tree

```
Does the corresponding MustClause use a facade?
├── YES → Use Facade pattern (mirror the MustClause structure)
└── NO  → Use Simple pattern (public static class in GuardClauses/)
```
