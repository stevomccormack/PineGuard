# Skill: Implement DataAnnotations Attribute
**ID**: pineguard.skill.scaffold-annotation
**Version**: 1.0

## 1. Context & Goal
Adapt a **MustClause** into a **DataAnnotations** `ValidationAttribute`. This allows ASP.NET / model-binding consumers to use PineGuard's domain logic via attributes.

## 2. Inputs
- **MustClause**: The underlying `Must.Be.Xxx` logic to adapt.
- **Value Type**: The expected input type (e.g., `string`, `DateOnly`, `bool`).

## 3. Critical Rules (The "Must Dos")
> [!IMPORTANT]
> 1.  **Strict Adaptation**: Do NOT write validation logic here. You **MUST** call `Must.Be.Xxx`.
> 2.  **Inherit Base**: All attributes **MUST** inherit from `ValidationAttributeBase` (`PineGuard.DataAnnotations.Common`).
> 3.  **ParamName Null**: You **MUST** pass `paramName: null` to the MustClause. The base handles `FormatErrorMessage` substitution.
> 4.  **Null & type handling**: `ValidationAttributeBase(Type expectedType, string code, bool allowNull = true)` handles this for you. With the default `allowNull: true` the base returns `ValidationResult.Success` for null and throws `InvalidOperationException` on a type mismatch *before* `ValidateValue` is called — so do not re-guard; cast directly with `var x = (T)value!;`. Two deliberate exceptions: (a) presence-style attributes pass `allowNull: false` and therefore DO receive null in `ValidateValue` (see `NotNullAttribute`, `NotNullOrEmptyStringAttribute`, `NotNullOrWhiteSpaceStringAttribute`, `ObjectAttributeBase`, `OfTypeAttribute`, `NotOfTypeAttribute`) — they delegate the null to the Must clause; (b) polymorphic attributes registered as `typeof(object)` still `switch` on the runtime type and throw `InvalidOperationException` in the default arm (see `PastAttribute`, `PastOrPresentAttribute`, `FutureAttribute`, `FutureOrPresentAttribute`, `IpAddressAttribute`, `NumberAttributeBase`, `CollectionAttributeBase`). Presence checks otherwise belong on `[Required]`.
> 4b. **Error code**: `code` is the base constructor's second positional argument — the same `MustCodes` constant the invoked Must clause itself passes to `Fail`/`FromBool`. Exposed as the public `Code` property. Rule13 check (d) fails the build if it doesn't match. See `docs/ai/specs/must-clauses/project.md` ("Error codes").
> 5.  **Naming**: String validators suffix with `String` (e.g., `TrueStringAttribute`). General: `[MustClauseName]Attribute`. Collision avoidance: suffix Type/Domain (e.g., `PastDateOnlyAttribute`).
> 6.  **Aggregation**: Prefer grouping related attributes into a single file named after the domain (e.g., `BoolAttributes.cs`, `EmailAttributes.cs`).
> 7.  **Strict Coding**: File-scoped namespaces, primary constructors forwarding to the base (`public sealed class XAttribute() : ValidationAttributeBase(typeof(T), MustCodes.…)`), `/// <inheritdoc/>` on the `ValidateValue` override, no comments unless exceptional.

## 4. Execution Steps

1.  **Identify Target File**
    *   Folder: `src/PineGuard.DataAnnotations/`.
    *   File: `[Domain]Attributes.cs` (aggregated) or `[AttributeName].cs` (standalone).
    *   Namespace: `PineGuard.DataAnnotations` (root namespace, no subfolders in namespace).

2.  **Implement Attribute**
    ```csharp
    using System.ComponentModel.DataAnnotations;
    using PineGuard.DataAnnotations.Common;
    using PineGuard.MustClauses;

    namespace PineGuard.DataAnnotations;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class JsonAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Json.Document.Invalid)
    {
        /// <inheritdoc/>
        protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
        {
            var strValue = (string)value!;

            var result = Must.Be.Json(strValue, paramName: null);
            return FromMustResult(result, validationContext);
        }
    }
    ```

3.  **Handle Type Variations**
    *   **Value types** (e.g., `DateOnly`): pass `typeof(DateOnly)` to the base and cast directly — `var dateValue = (DateOnly)value!;`.
    *   **Polymorphic input**: register as `ValidationAttributeBase(typeof(object))` and `switch` on the runtime type, throwing `InvalidOperationException` in the default arm.
    *   **String validators**: If the Must clause name matches a non-string type, suffix with `String` (e.g., `TrueStringAttribute`).

## 5. Definition of Done
- [ ] Code compiles.
- [ ] Inherits from `ValidationAttributeBase`.
- [ ] Calls `Must.Be.*` with `paramName: null`.
- [ ] No validation logic in the attribute.
- [ ] Passes the invoked clause's `MustCodes` constant as the base constructor's `code` argument.

## 6. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | Build passes | `dotnet build` exits 0 with no warnings |
| 2 | Inherits base | Class extends `ValidationAttributeBase`, not `ValidationAttribute` |
| 3 | paramName null | Every Must call passes `paramName: null` |
| 4 | No validation logic | No regex, parsing, or conditional logic beyond the direct cast |
| 5 | Null-safe | Base returns `ValidationResult.Success` for null under the default `allowNull: true` — the attribute contains no null guard (presence via `[Required]`) |
| 6 | Carries an error code | Base constructor's `code` argument matches the invoked clause's own `MustCodes` constant; Rule13 check (d) clean |

## 7. Reference Material
- `docs/ai/specs/data-annotations/project.md`
- `docs/ai/specs/spec.md` (Root)
- [Reference exemplars](references/README.md)
