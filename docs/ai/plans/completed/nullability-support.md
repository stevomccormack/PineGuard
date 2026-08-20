<!-- metadata_header
type: plan
id: nullability-support
version: 1.0
status: completed
-->

# Plan: Pragmatic Nullable Support (FluentValidation)

> [!NOTE]
> **Archived — shipped.** The nullable overloads described below are in
> `src/PineGuard.FluentValidation/Extensions/`. Kept as the record of the pattern that was applied,
> not as a task list.

This plan applies the "Pragmatic Nullable Support" pattern to FluentValidation extension files.
It ensures that nullable value types (structs) are handled by returning `Ok(default)` when the value is null, instead of forcing non-nullable overloads or failing validation.

## 1. FluentTimeOnlyExtensions.cs

1. Apply nullable overloads to `src/PineGuard.FluentValidation/Extensions/FluentTimeOnlyExtensions.cs`.
   - Pattern: verify if `HasValue` is true, if so call `Must.Be.X(val.Value)`, else return `MustResult<TimeOnly>.Ok(default)`.
   - Ensure `paramName: null` is used.

## 2. FluentTimeOnlyRangeExtensions.cs

2. Apply nullable overloads to `src/PineGuard.FluentValidation/Extensions/FluentTimeOnlyRangeExtensions.cs`.
   - Pattern: verify if `HasValue` is true, if so call `Must.Be.X(val.Value)`, else return `MustResult<TimeOnlyRange>.Ok(default)`.
   - Ensure `paramName: null` is used.

## 3. FluentGuidExtensions.cs

3. Apply nullable overloads to `src/PineGuard.FluentValidation/Extensions/FluentGuidExtensions.cs`.
   - Pattern: verify if `HasValue` is true, if so call `Must.Be.X(val.Value)`, else return `MustResult<Guid>.Ok(Guid.Empty)`.
   - Ensure `paramName: null` is used.

## 4. FluentNumberExtensions.cs

4. Verification: `FluentNumberExtensions.cs` already handles generic `T?` via `struct, INumber<T>`, so checking if specific `int?`, `long?` etc overloads need adjustment or if the generic validation covers it.
   - For `Even`, `Odd`, `Finite`, `NotFinite`, `NotNaN` which might have specific non-generic overloads, ensure `int?` / `long?` / `float?` / `double?` overloads exist and follow the pattern.

## 5. FluentEnumExtensions.cs

5. Apply nullable overloads to `src/PineGuard.FluentValidation/Extensions/FluentEnumExtensions.cs`.
   - Pattern: verify if `HasValue` is true, if so call `Must.Be.X(val.Value)`, else return `MustResult<TEnum>.Ok(default)`.
   - Ensure `paramName: null` is used.

## 6. FluentGeoLocationExtensions.cs

6. Apply nullable overloads to `src/PineGuard.FluentValidation/Extensions/FluentGeoLocationExtensions.cs`.
   - Pattern: verify if `HasValue` is true, if so call `Must.Be.X(val.Value)`, else return `MustResult<double>.Ok(default)`.
   - Ensure `paramName: null` is used.

## 7. FluentNetworkExtensions.cs

7. Apply nullable overloads to `src/PineGuard.FluentValidation/Extensions/FluentNetworkExtensions.cs` (specifically `PortNumber`).
   - Pattern for `int?`: verify if `HasValue` is true, if so call `Must.Be.X(val.Value)`, else return `MustResult<int>.Ok(default)`.
   - Ensure `paramName: null` is used.

## 8. Validation

8. Run compilation to ensure no syntax errors.
   - Command: `dotnet build src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj`
