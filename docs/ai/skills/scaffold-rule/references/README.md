# Reference Files: implement-core-rule

> Read the exemplar files listed below before implementing. Do NOT copy them — follow the same patterns.

## Simple Domain

A simple domain has a single public static class directly in `Rules/`.

| Role | Exemplar File |
|------|---------------|
| Rule | `src/PineGuard.Core/Rules/JsonRules.cs` |
| Utility | `src/PineGuard.Core/Utils/JsonUtility.cs` |

**Key points:**
- `public static class` in `namespace PineGuard.Rules;`
- Methods are `IsX(string? value)` predicates returning `bool`
- Delegate parsing to a Utility class — Rules only compose predicates
