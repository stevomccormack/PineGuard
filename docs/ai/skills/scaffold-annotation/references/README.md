# Reference Files: scaffold-annotation

> Read the exemplar files listed below before implementing. Do NOT copy them — follow the same patterns.

## Simple Domain

| Role | Exemplar File |
|------|---------------|
| Attribute class | `src/PineGuard.DataAnnotations/JsonAttributes.cs` |
| MustClause (dependency) | `src/PineGuard.MustClauses/MustJsonClauses.cs` |

**Key points:**
- Inherits from `ValidationAttributeBase`
- Overrides `ValidateValue` — calls `Must.Be.X` with `paramName: null`
- Zero validation logic in the attribute (strict adaptation)
- Multiple attributes can live in one file grouped by domain
