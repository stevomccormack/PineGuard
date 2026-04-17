---
spec:
  id: pineguard.ai.data-annotations.project-spec
  title: "PineGuard.DataAnnotations Project Spec"
  version: 2
  template:
    - ../template-project.md
  parent:
    - ../../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.DataAnnotations/**"
---

# PineGuard.DataAnnotations Project Spec

## Purpose

Define **project-specific** rules for the `PineGuard.DataAnnotations` integration.

This integration must keep PineGuard’s layering intact:

- Core (`Rules`/`Utils`) owns validation + parsing.
- MustClauses owns canonical messages.
- DataAnnotations integration adapts MustClauses into `ValidationAttribute`s.

## Scope

This spec applies to `src/PineGuard.DataAnnotations/**`.

## Feature Implementation Checklist

See `docs/ai/specs/spec.md` §3 ("Feature Implementation Checklist (Master)").

## Related specs

- Unit tests addendum: `docs/ai/specs/data-annotations/unit-test.md`
- Coverage addendum: `docs/ai/specs/data-annotations/coverage.md`
- Naming collisions: `docs/ai/specs/language/naming-collisions.md`

## References

- Root rules: `docs/ai/specs/spec.md`
- Validated value vs configuration/dependency parameters: `docs/ai/specs/spec.md` (“Validated value vs configuration/dependency parameters”).
- Dependency graph: `docs/ai/dependencies.md`

---

## 1) Non-negotiables (integration contract)

- Do not implement validation logic in this project.
- Do not call `PineGuard.Rules.*` or `PineGuard.Utils.*` from DataAnnotations integration code.
- Always validate by calling `PineGuard.MustClauses.Must.Be.*` and using the `MustResult`.
- Do not embed datasets/tables/regex lists here; those belong in Core (`Rules`/`Utils`).
- **Base Infrastructure**: Implementations SHOULD inherit from a common base attribute (`PineGuard.DataAnnotations.Common.ValidationAttributeBase`) to centralize type checking and message adaptation logic.

## 2) Message + parameter name handling (required)

DataAnnotations error messages must reference the **Display Name** (e.g. from `DisplayAttribute` or property name).

Required pattern:

- Override `FormatErrorMessage(string name)`.
- Call Must clauses with `paramName: null` so the returned message remains a template containing `{paramName}`.
- Replace `{paramName}` with the `name` argument passed to `FormatErrorMessage`.
- Use the base attribute infrastructure to handle this consistently.

## 2.1 Formatting Rules (Strict)

- **Base Class**: Use `ValidationAttributeBase` (Namespace: `PineGuard.DataAnnotations.Common`).
- **Constructors**: Single line empty constructors successfully.
  - Example: `public TrueAttribute() : base(typeof(bool)) { }`
- **Naming Collisions**: If an attribute name would collide across domains (e.g., `Past` for `DateOnly` vs `DateTime`), suffix the Type/Domain to the attribute name for the **entire class** of that domain.
  - Example: `PastDateOnlyAttribute`, `FutureDateOnlyAttribute` (for DateOnly domain).
  - Example: `TrueStringAttribute` (for String domain if `True` exists for Bool).
- **Comments**: No comments allowed unless exceptional value.
- **Structure**: Clean, minimal implementations.

## 3) Structure & Naming (New Requirements)

### 3.1 Folder Structure (Internal Implementation vs Public Facade)

DataAnnotations is an **adapter layer** made of discoverable **types** (`ValidationAttribute`s). Unlike Must/Guard, it does not use a public static facade over internal implementations.

Required organization rules:

- Keep attribute types in the root namespace: `namespace PineGuard.DataAnnotations;`
- Prefer **aggregated, domain-named files** at the project root (no required domain subfolders).
- Use `src/PineGuard.DataAnnotations/Common/**` for shared base classes and helpers.

Rationale:

- Consumers discover attributes by **type**, so a static facade does not apply.
- Aggregation avoids file explosion and avoids implying a 1:1 folder parity requirement with Must/Guard.

### 3.2 Naming Conventions

- **String Validators**: Prefer the shortest collision-safe public name. Add `String` only when it materially improves clarity or resolves ambiguity.
  - Example: `Must.Be.True(string)` -> `[TrueStringAttribute]`.
  - Example: `Must.Be.Url(string)` -> `[WebUrlAttribute]` to avoid colliding with framework-native `[Url]`.
- **General**: `[MustClauseName]Attribute` unless the naming-collision spec requires a clearer adapter-specific name.
- **Collision Handling**: Follow `docs/ai/specs/language/naming-collisions.md` when a framework-native attribute name would create ambiguity or collision.
- **File Name**: `[Domain]Attributes.cs` (Aggregated) OR `[AttributeName].cs` (Standalone).
  - **Aggregation Strategy**: Prefer grouping related attributes into a single file named after the domain (e.g., `BoolAttributes.cs`) to prevent file explosion.

## 4) Adapter conventions

### Nullability

DataAnnotations follows the standard "skip on null" behavior:

- By default, DataAnnotations attributes should **not** fail on `null` values.
- Presence/required checks are expressed using `[Required]` (or by constructing PineGuard attributes with `allowNull: false` when that is explicitly intended).

This is intentionally different from Must/Guard behavior:

- MustClauses follow Rule07 (hybrid nullability strategy): inputs may be declared nullable (especially reference types), but **null is invalid by default** unless the method name explicitly encodes null as acceptable (e.g., `NullOrXxx`).
- The adapter layer is responsible for DataAnnotations UX: null handling is controlled by `[Required]` / `allowNull`, not by failing on null inside PineGuard validation attributes.

Implementation rule:

- `ValidationAttributeBase` must handle the null check before calling `ValidateValue(...)` when `allowNull` is enabled.

Standard pattern:

```csharp
// Constructor defaults to allowNull: true
public PastDateOnlyAttribute() : base(typeof(DateOnly)) { }

protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
{
    // Base handles null check before calling this
    if (value is not DateOnly dateValue) return ValidationResult.Success;

    var result = Must.Be.Past(dateValue, paramName: null);
    return FromMustResult(result, validationContext);
}
```

### 4.1 The Rule

`ValidationAttribute`s must strictly inherit from `ValidationAttributeBase`.

## 5) Required adapter strategy

Each `ValidationAttribute` implementation must adapt a specific Must clause:

- Extend `PineGuardValidationAttribute`.
- Pass expected type to base constructor.
- Override `ValidateValue(object? value, ValidationContext context)`.

### 5.1 Implementation Pattern (Template)

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class TrueAttribute : ValidationAttributeBase
{
    public TrueAttribute() : base(typeof(bool)) { }

    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        if (value is not bool boolValue) return ValidationResult.Success;

        var result = Must.Be.True(boolValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
```

## 6) Output expectations

When asked to add a new DataAnnotation:

- Prefer adding the attribute to the appropriate aggregated file (e.g., `BoolAttributes.cs`).
- Ensure it compiles and inherits from `ValidationAttributeBase`.
