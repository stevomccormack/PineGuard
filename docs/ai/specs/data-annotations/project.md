---
spec:
  id: pineguard.ai.data-annotations.project-spec
  title: "PineGuard.DataAnnotations Project Spec"
  version: 2
  template:
    - ../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.DataAnnotations/**"
---

# PineGuard.DataAnnotations Project Spec

## Purpose

Define **project-specific** rules for the `PineGuard.DataAnnotations` integration.

This integration must keep PineGuard’s layering intact. The one-directional pipeline is
canonical in `../project.md` §1.1. This adapter calls MustClauses only — never Core directly,
never a sibling adapter, and never reimplemented logic.

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
- Dependency graph: `docs/ai/specs/dependencies.md`

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
- **Constructors**: Use a single-line primary constructor per `docs/ai/specs/coding-standard.md` ("Primary Constructors (C# 12)").
  - Example: `public sealed class TrueAttribute() : ValidationAttributeBase(typeof(bool))`
- **Naming Collisions**: If an attribute name would collide across domains (e.g., `Past` for `DateOnly` vs `DateTime`), suffix the Type/Domain to the attribute name for the **entire class** of that domain.
  - Example: `PastDateOnlyAttribute`, `FutureDateOnlyAttribute` (for DateOnly domain).
  - Example: `TrueStringAttribute` (for String domain if `True` exists for Bool).
- **Method Ordering**: Within an aggregated attribute file, the positive attribute precedes its `Not*` complement — matching `docs/ai/specs/must-clauses/project.md` ("Method ordering").
- **Comments**: XML documentation comments (`<summary>`, `<remarks>`, `<example>`, `<seealso>`) are REQUIRED on every public attribute — `<GenerateDocumentationFile>` is on in `Directory.Build.props` and the templates live in `docs/ai/skills/document/SKILL.md` §5.6. Inline `//` implementation comments are not allowed unless they carry exceptional value.
- **Structure**: Clean, minimal implementations.

## 3) Structure & Naming (New Requirements)

### 3.1 Folder Structure (Internal Implementation vs Public Facade)

DataAnnotations is an **adapter layer** made of discoverable **types** (`ValidationAttribute`s). Unlike Must/Guard, it does not use a public static facade over internal implementations.

Required organization rules:

- Keep attribute types in the root namespace: `namespace PineGuard.DataAnnotations;`
- Prefer **aggregated, domain-named files** at the project root (no required domain subfolders).
- Use `src/PineGuard.DataAnnotations/Common/**` for shared base classes and helpers — today `ValidationAttributeBase` and `GenericDictionaryAttributeBase`. The one exception is `ObjectAttributeBase`, which sits in `ObjectAttributes.cs` alongside the object attributes it serves.

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

- MustClauses follow the Rule07 hybrid nullability strategy — **null is invalid by default** unless the method name encodes it (e.g., `NullOrXxx`). Canonical statement: `../must-clauses/project.md` §Nullability.
- The adapter layer is responsible for DataAnnotations UX: null handling is controlled by `[Required]` / `allowNull`, not by failing on null inside PineGuard validation attributes.

Implementation rule:

- `ValidationAttributeBase` must handle the null check before calling `ValidateValue(...)` when `allowNull` is enabled.

Standard pattern:

```csharp
// Primary constructor defaults to allowNull: true
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly))
{
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        // Base has already returned Success for null and thrown on a type mismatch
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Past(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
```

> **No defensive type guard.** When the attribute declares a concrete expected type AND `allowNull` is
> left at its default (`true`), do NOT re-check the CLR type inside `ValidateValue` —
> `ValidationAttributeBase.IsValid` has already returned `Success` for `null` and thrown
> `InvalidOperationException` on a type mismatch, so an `is not T` guard is an uncoverable dead branch
> and breaks the 100% line + branch gate. Two exceptions:
>
> - Attributes constructed with `typeof(object)` for a polymorphic family (`TimeAttributes.cs`,
>   `NumberAttributes.cs`, `CollectionAttributes.cs`) must `switch` on the runtime type with a throwing
>   `default:` arm.
> - Attributes constructed with `allowNull: false` (`ObjectAttributes.cs`, `StringAttributes.cs`) still
>   receive `null` and must handle it explicitly.

### 4.1 The Rule

`ValidationAttribute`s must inherit from `ValidationAttributeBase` or one of its shared derived bases
(`ObjectAttributeBase`, `GenericDictionaryAttributeBase`).

## 5) Required adapter strategy

Each `ValidationAttribute` implementation must adapt a specific Must clause:

- Extend `PineGuard.DataAnnotations.Common.ValidationAttributeBase` (or one of its derived bases — `ObjectAttributeBase`, `GenericDictionaryAttributeBase`).
- Pass expected type to base constructor.
- Override `ValidateValue(object? value, ValidationContext context)`.

### 5.1 Implementation Pattern (Template)

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TrueAttribute() : ValidationAttributeBase(typeof(bool))
{
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var boolValue = (bool)value!;

        var result = Must.Be.True(boolValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
```

### 5.2 Generic / open-type attributes

When the Must clause is generic and the value's type is only known at runtime, adapt it through the
reflection path — never through `dynamic`:

```csharp
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDefaultAttribute : ObjectAttributeBase
{
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(nameof(MustDefaultEqualityClauses.NotDefault), value, validationContext);
}
```

- `ObjectAttributeBase.InvokeGenericMust(...)` infers the value type, closes the generic Must method
  over it, and calls the shared `BuildInvokeArgs` + `InvokeAndMapResult` helpers on
  `ValidationAttributeBase`.
- `GenericDictionaryAttributeBase.InvokeDictionaryMust(...)` does the same for dictionary-shaped values,
  resolving `TKey`/`TValue` from the runtime type.
- **`dynamic` and `Microsoft.CSharp` must never be reintroduced.** The DLR fails to bind members on a
  `MustResult<T>` parameterized with a non-public type argument; reflection is the deliberate
  replacement, not an accident.

## 6) Output expectations

When asked to add a new DataAnnotation:

- Prefer adding the attribute to the appropriate aggregated file (e.g., `BoolAttributes.cs`).
- Ensure it compiles and inherits from `ValidationAttributeBase` or one of its derived bases (`ObjectAttributeBase`, `GenericDictionaryAttributeBase`).
