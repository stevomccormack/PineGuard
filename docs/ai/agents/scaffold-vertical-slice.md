<!-- metadata_header
type: agent
id: agent-scaffold-vertical-slice
version: 1.0
-->

# Agent: Implement a feature vertical slice (Core -> Must -> Guard -> Adapters -> Tests)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: owner ([../roles/owner.md](../roles/owner.md))

## Feature Implementation Workflow

This workflow guides the implementation of a new feature or refactoring across the entire PineGuard library stack.

### 1. Core Implementation (Rules & Utils)

- [ ] **Rules**: Implement pure boolean predicates in `PineGuard.Core/Rules`.
- [ ] **Utils**: Implement parsing/normalization in `PineGuard.Core/Utils`.
- [ ] **Tests**: Add units tests in `PineGuard.Core.UnitTests`.

### 2. MustClauses Implementation

- [ ] **Must**: Expose Fluent API in `PineGuard.MustClauses`.
- [ ] **Mapping**: Call Core Rules/Utils.
- [ ] **Tests**: Add unit tests in `PineGuard.MustClauses.UnitTests`.

### 3. GuardClauses Implementation

- [ ] **Guard**: Expose Throwing API in `PineGuard.GuardClauses`.
- [ ] **Mapping**: Call MustClauses.
- [ ] **Tests**: Add unit tests in `PineGuard.GuardClauses.UnitTests` (if requested).

### 4. Adapter Implementation

- [ ] **FluentValidation**: Add extension methods in `PineGuard.FluentValidation`.
  - Note: Support nullable value types where applicable.
- [ ] **DataAnnotations**: Add attributes in `PineGuard.DataAnnotations`.
  - Note: Inherit from `ValidationAttributeBase`.

### 5. Verification

- [ ] **Build**: Ensure solution builds.
- [ ] **Test**: Run all tests.
  - `dotnet test`
- [ ] **Inspect**: Run JetBrains Qodana.
  - See `docs/ai/workflows/scan-qodana.md`
