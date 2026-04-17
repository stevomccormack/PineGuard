<!-- metadata_header
type: agent
id: agent-migrate-agent-1
version: 1.0
-->

> [!WARNING]
> **ARCHIVED** — Fixture migration completed Feb 2026 (commit bfec9ee). This agent is retained for reference only.

# Agent: Migration Agent 1 — Rule Group 1

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: verifier ([../roles/verifier.md](../roles/verifier.md))

## Context

- **Spec**: `docs/ai/specs/testing/fixture.md`
- **Conventions**: `docs/ai/rules/fixture-conventions.md`
- **Status**: Completed (Feb 2026)
- **Model**: Sonnet
- **Phase**: 4 (concurrent with agents 2 & 3)

## Scope

| Rule File | Location |
|---|---|
| StringRules | `src/PineGuard.Core/Rules/StringRules.cs` |
| StringRules.Casing | `src/PineGuard.Core/Rules/StringRules.Casing.cs` |
| StringRules.Numbers | `src/PineGuard.Core/Rules/StringRules.Numbers.cs` |
| StringRules.NumberTypes | `src/PineGuard.Core/Rules/StringRules.NumberTypes.cs` |
| NumberRules | `src/PineGuard.Core/Rules/NumberRules.cs` |
| DateTimeRules | `src/PineGuard.Core/Rules/DateTimeRules.cs` |
| CharRules | `src/PineGuard.Core/Rules/CharRules.cs` |
| DateOnlyRules | `src/PineGuard.Core/Rules/DateOnlyRules.cs` |
| DateTimeOffsetRules | `src/PineGuard.Core/Rules/DateTimeOffsetRules.cs` |
| OwaspRules | `src/PineGuard.Core/Rules/OwaspRules.cs` |
| Owasp/OwaspRegex | `src/PineGuard.Core/Rules/Owasp/OwaspRegex.cs` |

**CRITICAL**: Do NOT modify files outside this scope. Agents 2 and 3 are working concurrently.

## Pre-Work

1. Read `docs/ai/specs/testing/fixture.md`
2. Read `docs/ai/rules/fixture-conventions.md`
3. `dotnet build --no-restore -verbosity:quiet` → confirm GREEN

## Per Rule File — Process

For EACH Rule file in scope:

### 1. Read Rule Source
- Get exact method signatures and parameter names
- Identify constants/static readonly fields (boundary rules)
- Note which methods are format vs boundary

### 2. Enrich Fixture
- Read existing `tests/PineGuard.Testing/Fixtures/[RulesClass]Fixtures.cs`
- Add RuleScenario arrays (ValidScenarios, InvalidScenarios, etc.)
- Reference Rule constants for edge cases (boundary rules)
- Add rollup properties (AllScenarios, AllValid, AllInvalid)

### 3. Migrate Rules (Core) TestData + Tests
- Replace inline literals with `F.OpGroup.Scenarios.ToRuleCases()`
- Test class inherits `BaseRuleUnitTest`
- Method: `MethodName_BehavesAsExpected(RuleCase<T> tc)` + `AssertResult(tc, result)`

### 4. Migrate Must TestData + Tests
- Use `F.OpGroup.Scenarios.ToMustCases(...)` with switch for messages
- Test class inherits `BaseMustUnitTest`

### 5. Migrate Guard TestData + Tests
- Use `F.OpGroup.Scenarios.ToGuardCases("paramName")`
- Test class inherits `BaseGuardUnitTest`

### 6. Migrate Fluent TestData + Tests
- Use `F.OpGroup.Scenarios.ToFluentCases(...)` with switch for null/valid/invalid
- Test class inherits `BaseFluentUnitTest`

### 7. Migrate DA TestData + Tests
- Use `F.OpGroup.Scenarios.ToDataAnnotationCases(...)`
- Test class inherits `BaseDataAnnotationUnitTest`

### 8. Verify + Update Status
- `dotnet build && dotnet test` after each Rule file
- ~~Update docs/ai/migration-status.md~~ (migration complete)

## Special Considerations

- **CharRules**: Boundary rule — uses `AsciiMinValue`, `AsciiMaxValue`, `PrintableAsciiMinValue`, `PrintableAsciiMaxValue`
- **StringRules.NumberTypes**: Has `SignedIntegerPattern`, `DefaultAllowedDigitSeparators`
- **OwaspRules/OwaspRegex**: ~23 pattern constants across nested classes — use for exact-match fixture values

## Constraints

- Structural comments only (see conventions §1) — no ad-hoc comments
- Single-line formatting (max 400 chars)
- Edge case constants reference Rule class constants
- Flat test classes (no nested Op Groups in Tests files)
- Partial fixture files mirror Rule partial structure
- camelCase tuple elements matching exact method parameter names
