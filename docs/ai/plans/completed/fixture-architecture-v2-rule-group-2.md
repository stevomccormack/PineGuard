<!-- metadata_header
type: plan
id: plan-fixture-architecture-v2-rule-group-2
version: 1.0
-->

> [!WARNING]
> **ARCHIVED** — historical record of one of the three parallel rule-group tracks in the Fixture Architecture v2 migration, completed Feb 2026 (commit bfec9ee). Only groups 1 and 2 were written down; the third track's scope table never existed as a file.

# Plan: Fixture Architecture v2 — Rule Group 2

> [!IMPORTANT]
> business unit: engineering ([../../business-units/engineering.md](../../business-units/engineering.md))
> roles: verifier ([../../roles/verifier.md](../../roles/verifier.md))

## Context

- **Spec**: `docs/ai/specs/testing/fixture.md`
- **Conventions**: `docs/ai/rules/fixture-conventions.md`
- **Status**: Completed (Feb 2026)
- **Model**: Sonnet
- **Phase**: 4 (concurrent with agents 1 & 3)

## Scope

| Rule File | Location |
|---|---|
| HttpSecurityHeaderRules | `src/PineGuard.Core/Rules/HttpSecurityHeaderRules.cs` |
| HttpRules | `src/PineGuard.Core/Rules/HttpRules.cs` |
| CollectionRules | `src/PineGuard.Core/Rules/CollectionRules.cs` |
| DictionaryRules | `src/PineGuard.Core/Rules/DictionaryRules.cs` |
| ReadOnlyDictionaryRules | `src/PineGuard.Core/Rules/ReadOnlyDictionaryRules.cs` |
| EnumRules | `src/PineGuard.Core/Rules/EnumRules.cs` |
| UriRules | `src/PineGuard.Core/Rules/UriRules.cs` |
| BitWiseRules | `src/PineGuard.Core/Rules/BitWiseRules.cs` |
| NetworkRules | `src/PineGuard.Core/Rules/NetworkRules.cs` |
| TimeOnlyRules | `src/PineGuard.Core/Rules/TimeOnlyRules.cs` |
| SqlDateTimeRules | `src/PineGuard.Core/Rules/SqlDateTimeRules.cs` |
| StringRules.TimeOnly | `src/PineGuard.Core/Rules/StringRules.TimeOnly.cs` |

**CRITICAL**: Do NOT modify files outside this scope. Agents 1 and 3 are working concurrently.

## Pre-Work

1. Read `docs/ai/specs/testing/fixture.md`
2. Read `docs/ai/rules/fixture-conventions.md`
3. `dotnet build --no-restore -verbosity:quiet` → confirm GREEN

## Per Rule File — Process

Same process as Agent 1. For EACH Rule file in scope:

1. Read Rule source → get method signatures, constants, format vs boundary
2. Enrich fixture with RuleScenario arrays + Rule constant references
3. Migrate Rules (Core) TestData + Tests → `BaseRuleUnitTest` + `ToRuleCases()`
4. Migrate Must TestData + Tests → `BaseMustUnitTest` + `ToMustCases(...)`
5. Migrate Guard TestData + Tests → `BaseGuardUnitTest` + `ToGuardCases("paramName")`
6. Migrate Fluent TestData + Tests → `BaseFluentUnitTest` + `ToFluentCases(...)`
7. Migrate DA TestData + Tests → `BaseDataAnnotationUnitTest` + `ToDataAnnotationCases(...)`
8. `dotnet build && dotnet test` after each Rule file
9. ~~Update docs/ai/migration-status.md~~ (migration complete)

## Special Considerations

- **HttpSecurityHeaderRules**: Boundary rule — has `DefaultStrictTransportSecurityMinMaxAgeSeconds` (int) + 7 default value string constants
- **SqlDateTimeRules**: Boundary rule — uses `MinValue`, `MaxValue` (DateTime)
- **CollectionRules**: Lambda/Func inputs cannot be fixtures (§10). Use `AdHocCases` for predicate-based tests.
- **DictionaryRules / ReadOnlyDictionaryRules**: Generic type parameters — fixture values may need type-specific entries.
- **NetworkRules**: Utils class `NetworkUtility` has private constants (`Ipv4SegmentCount`, etc.) — not directly referenceable, use literal values

## Constraints

- Structural comments only (see conventions §1) — no ad-hoc comments
- Single-line formatting (max 400 chars)
- Edge case constants reference Rule class constants
- Flat test classes (no nested Op Groups in Tests files)
- Partial fixture files mirror Rule partial structure
- camelCase tuple elements matching exact method parameter names
