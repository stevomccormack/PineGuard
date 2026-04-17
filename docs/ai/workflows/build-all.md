---
description: "Big Bang" implementation of Pragmatic Nullable Support across all libraries
---

This workflow executes a system-wide refactoring to enforce the "Strict Core, Pragmatic Adapter" strategy.
It updates FluentValidation and DataAnnotations to handle nullable types correctly (skipping nulls by default) while keeping Must/Guard clauses pure.

# 1. FluentValidation Refactoring
// turbo-all
1. Update `FluentTimeOnlyExtensions.cs` to handle nullable `TimeOnly?` (return `Ok(default)` if null).
2. Update `FluentTimeOnlyRangeExtensions.cs` to handle nullable `TimeOnlyRange?` (return `Ok(default)` if null).
3. Update `FluentGuidExtensions.cs` to handle nullable `Guid?` (return `Ok(Guid.Empty)` if null).
4. Update `FluentEnumExtensions.cs` to handle nullable `TEnum?` (return `Ok(default)` if null).
5. Update `FluentGeoLocationExtensions.cs` to handle nullable `double?` (return `Ok(default)` if null).
6. Update `FluentNetworkExtensions.cs` (PortNumber) to handle nullable `int?` (return `Ok(default)` if null).

# 2. DataAnnotations Refactoring
// turbo-all
7. Ensure `PineGuard.DataAnnotations/Common/ValidationAttributeBase.cs` implements the `allowNull` pattern (returns Success if value is null).
8. Scan `PineGuard.DataAnnotations` for any attributes missing the `ValidationAttributeBase` inheritance.
9. Verify all DataAnnotation attributes call **non-nullable** Must clauses (casting strictly after base null check).

# 3. Build & Verify
// turbo-all
10. Run `dotnet build src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj` to capture compilation errors.
11. Run `dotnet build src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj` to capture compilation errors.
12. Run `dotnet build` on the solution to check for ripple effects.

# 4. Final Cleanup
// turbo-all
13. If build fails, report specific error list to the user for "Batch Fix".
14. If build passes, run `dotnet test` on the combined unit test suite.
15. If tests pass, run Qodana inspection wrapper.
