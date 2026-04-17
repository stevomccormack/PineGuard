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
> 4.  **Null Handling**: DataAnnotations uses "skip on null" by default. Presence checks use `[Required]`. Do NOT fail on null inside the attribute.
> 5.  **Naming**: String validators suffix with `String` (e.g., `TrueStringAttribute`). General: `[MustClauseName]Attribute`. Collision avoidance: suffix Type/Domain (e.g., `PastDateOnlyAttribute`).
> 6.  **Aggregation**: Prefer grouping related attributes into a single file named after the domain (e.g., `BoolAttributes.cs`, `EmailAttributes.cs`).
> 7.  **Strict Coding**: File-scoped namespaces, single-line empty constructors, no comments unless exceptional.

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

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class JsonAttribute : ValidationAttributeBase
    {
        public JsonAttribute() : base(typeof(string)) { }

        protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
        {
            if (value is not string strValue) return ValidationResult.Success;

            var result = Must.Be.Json(strValue, paramName: null);
            return FromMustResult(result, validationContext);
        }
    }
    ```

3.  **Handle Type Variations**
    *   **Value types** (e.g., `DateOnly`): Cast with `value is not DateOnly dateValue`.
    *   **String validators**: If the Must clause name matches a non-string type, suffix with `String` (e.g., `TrueStringAttribute`).

## 5. Definition of Done
- [ ] Code compiles.
- [ ] Inherits from `ValidationAttributeBase`.
- [ ] Calls `Must.Be.*` with `paramName: null`.
- [ ] No validation logic in the attribute.

## 6. Success Criteria

| # | Criterion | Measure |
|---|-----------|---------|
| 1 | Build passes | `dotnet build` exits 0 with no warnings |
| 2 | Inherits base | Class extends `ValidationAttributeBase`, not `ValidationAttribute` |
| 3 | paramName null | Every Must call passes `paramName: null` |
| 4 | No validation logic | No regex, parsing, or conditional logic beyond type-cast guard |
| 5 | Null-safe | Returns `ValidationResult.Success` when value is null (presence via `[Required]`) |

## 7. Reference Material
- `docs/ai/specs/data-annotations/project.md`
- `docs/ai/specs/spec.md` (Root)
- [Reference exemplars](references/README.md)
