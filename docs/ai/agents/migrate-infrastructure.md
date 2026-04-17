<!-- metadata_header
type: agent
id: agent-migrate-infrastructure
version: 1.0
-->

> [!WARNING]
> **ARCHIVED** — Fixture migration completed Feb 2026 (commit bfec9ee). This agent is retained for reference only.

# Agent: Migrate Infrastructure (Fixture Architecture v2)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: builder ([../roles/builder.md](../roles/builder.md))

## Context

- **Spec**: `docs/ai/specs/testing/fixture.md`
- **Conventions**: `docs/ai/rules/fixture-conventions.md`
- **Status**: Completed (Feb 2026)
- **Model**: Opus
- **Phase**: 2 (run after Phase 1 `/migrate-brain`)

## Pre-Work

1. Read `docs/ai/specs/testing/fixture.md` (complete type reference)
2. Read `docs/ai/rules/fixture-conventions.md` (code conventions)
3. `dotnet build --no-restore -verbosity:quiet` → confirm GREEN

## Steps

### Step 1: Expected Type Hierarchy

Create `tests/PineGuard.Testing/UnitTests/Expected/`:

| File | Type |
|---|---|
| `IExpectedResult.cs` | `public interface IExpectedResult { bool IsValid { get; } }` |
| `ReturnExpected.cs` | `public abstract record ReturnExpected(bool IsValid, string? Message = null) : IExpectedResult;` |
| `ThrowExpected.cs` | `public abstract record ThrowExpected(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null) : IExpectedResult;` |
| `RuleExpected.cs` | `public sealed record RuleExpected(bool IsValid) : IExpectedResult;` |
| `GuardExpected.cs` | `public sealed record GuardExpected(...) : ThrowExpected(...)` |
| `DataAnnotationExpected.cs` | `public sealed record DataAnnotationExpected(...) : ReturnExpected(...)` |

Modify existing:
- `MustExpected.cs` → extend `ReturnExpected(IsValid, Message)`
- `FluentExpected.cs` → extend `ReturnExpected(IsValid, Message)`

### Step 2: Obsolete IsCase/HasCase

Mark `IsCase<T>` and `HasCase<T>` with `[Obsolete("Use RuleCase<T> instead.")]`.

### Step 3: Case Records

Create `tests/PineGuard.Testing/Cases/`:

| File | Type |
|---|---|
| `RuleCase.cs` | `public sealed record RuleCase<TValue>(...) : ReturnCase<TValue, RuleExpected>(...)` |
| `MustCase.cs` | `public sealed record MustCase<TValue>(...) : ReturnCase<TValue, MustExpected>(...)` |
| `GuardCase.cs` | `public sealed record GuardCase<TValue>(...) : ReturnCase<TValue, GuardExpected>(...)` |
| `FluentCase.cs` | `public sealed record FluentCase<TValue>(...) : ReturnCase<TValue, FluentExpected>(...)` |
| `DataAnnotationCase.cs` | `public sealed record DataAnnotationCase(...) : ReturnCase<object?, DataAnnotationExpected>(...)` |

### Step 4: RuleScenario

Create `tests/PineGuard.Testing/Scenarios/RuleScenario.cs`.

### Step 5: Extension Methods

Create extension methods for `.ToRuleCases()`, `.ToMustCases()`, `.ToGuardCases()`, `.ToFluentCases()`, `.ToDataAnnotationCases()` with all overloads per spec §4.

### Step 6: Filter Combinators

Create `.WhereValid()`, `.WhereInvalid()`, `.Except()`, `.Only()` per spec §5.

### Step 7: Base Test Classes

Create `BaseRuleUnitTest`, `BaseMustUnitTest`, `BaseGuardUnitTest`, `BaseFluentUnitTest`, `BaseDataAnnotationUnitTest` with `AssertResult()` per spec §6.

### Step 8: Unit Tests

Write comprehensive tests for all new infrastructure types in `tests/PineGuard.Testing.UnitTests/`.

### Step 9: Pilot — CsvRules.IsCsvLine

Enrich `CsvRulesFixtures.IsCsvLine` with RuleScenarios. Migrate all 5 layer TestData + Tests files for CsvLine using the new architecture.

### Step 10: Verify

1. `dotnet build` → GREEN
2. `dotnet test` → all pass
3. `/format-code`

## Constraints

- Structural comments only (see conventions §1) — no ad-hoc comments
- Single-line formatting (max 400 chars)
- All new files use file-scoped namespaces
- `ThrowsCase<T>` + `ThrowsCaseAssert` are UNCHANGED
- Existing tests must continue to compile (backward compat via `[Obsolete]` on IsCase/HasCase)
