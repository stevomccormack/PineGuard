# Code Reviewer Memory

> **Role:** `docs/ai/roles/reviewer.md` (Critic)
> Directives: Correctness First, Readability, Test Expectations, Inspect Don't Guess, AI Review Discipline.
> Constraints: No style changes without clarity/correctness benefit. Block on risk, not preference.

## Learned Patterns

### Signature Drift Indicators
- Missing `[CallerArgumentExpression(nameof(value))]` on paramName
- Wrong nullability: nullable value type (`int?`) instead of non-nullable (`int`) in Rules
- Guard method not accepting `message` and `exceptionCreator` params
- FluentValidation using `.Must(...)` instead of `.MustBe(...)`
- DataAnnotations not inheriting `ValidationAttributeBase`

### Parsed-Result Drift Indicators
- MustClause passes `result: value` (raw input) instead of parsed output from `Utility.TryXxx()` — always flag; the parsed/normalized value must flow through to `MustResult<T>.Result`
- MustClause calls `Rules.IsXxx()` when a `Utility.TryXxx()` exists that returns the parsed value — prefer the Try method to get both boolean and parsed result in one call
- Reference: `docs/ai/specs/core/project.md` §4.1

### Architectural Violations to Watch For
- Logic in GuardClauses (should only call Must and throw)
- Logic in FluentValidation (should only call Must via MustBe adapter)
- Logic in DataAnnotations (should only call Must via ValidateValue override)
- User-facing messages in Core (Core is pure logic — messages belong in Must)
- Direct Core calls from integrations (Must/Guard/Fluent/Data must go through Must, not Core)
- IO in Core Rules/Utils (no File/Network operations)

### Formatting Rules
- File-scoped namespaces (always)
- Sorted usings (always)
- Arrow functions where implementation is single expression
- No comments unless exceptional value
- Single-line empty constructors for DataAnnotations
- `value` parameter naming for validated input

### Naming Conventions
- Must: Positive semantics (`Must.Be.NotNull`, `Must.Be.Alphabetic`)
- Guard: Negative semantics (`Guard.Against.Null`, `Guard.Against.InvalidFormat`)
- FluentValidation: Match Must name (`ruleBuilder.NotNullOrEmpty()`)
- DataAnnotations: `[MustClauseName]Attribute`, String validators suffix `String`

### Test Structure Compliance

Tests-file shape is governed by `docs/ai/rules/fixture-conventions.md` §4 and
`docs/ai/specs/testing/fixture.md`; TestData-file shape by `docs/ai/specs/testing/unit-test.md` §4.

- Tests files are flat `sealed class` with one `MethodName_BehavesAsExpected` per op — no nested Operation Groups
- TestData files keep nested Operation Groups per method
- Element ordering within Op Groups: datasets first, records last (§4.4)
- Structural correspondence: Tests methods mirror TestData groups in same order (§4.5)
- Tuple property MUST be `Value` (not `Input`), elements camelCase matching exact method param names (§4.3)
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- Test Fixtures: input values from `PineGuard.Testing.Fixtures/`, `nameof` for Name, alias `F` (§9 "Test Fixtures")
- Full canonical examples in §8 "Full Canonical Examples"
- **`Expected` property** (NOT `ExpectedReturn`) — records use `Expected`, tests access `testCase.Expected`
- `MustExpected(bool IsValid, string? Message = null, string? ParamName = null)` — use `IsValid` boolean
- `FluentExpected(bool IsValid, string? Message = null)` — use `IsValid` boolean

## Fixture Architecture v2 Review Checklist

Reference: `docs/ai/specs/testing/fixture.md`
Conventions: `docs/ai/rules/fixture-conventions.md`

### Expected Type Compliance
- `RuleExpected` for Core, `MustExpected` for Must, `GuardExpected` for Guard, `FluentExpected` for Fluent, `DataAnnotationExpected` for DA
- All implement `IExpectedResult { bool IsValid }`
- `MustExpected`/`FluentExpected`/`DataAnnotationExpected` extend `ReturnExpected`
- `GuardExpected` extends `ThrowExpected`

### Case Record Compliance
- `RuleCase<T>` replaces `IsCase<T>`/`HasCase<T>` (which are `[Obsolete]`)
- `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`

### Fixture Compliance
- RuleScenario arrays with named fields referencing Rule constants
- Format rules: 2 arrays (Valid, Invalid) + AllScenarios
- Boundary rules: 4 arrays (Valid, ValidEdge, Invalid, InvalidEdge) + rollups
- Edge case constants MUST reference Rule class constants (never hardcoded)

### Convention Compliance
- Zero comments
- Single-line entries (max 400 chars)
- Flat test classes (no nested Op Groups in Tests files)
- Method naming: `MethodName_BehavesAsExpected`
- Partial fixture files mirror Rule partial structure
- camelCase tuple elements matching exact method parameter names

## Common Issues Found

### Fixture Architecture v2 Brain Review (Mar 2026)
- Agent 2 missing "Partial fixture files mirror Rule partial structure" constraint (drift from Agent 1/3 pattern)
- Phase 3 (Migrate-Fixtures) prerequisite missing from migrate-layers orchestrator — agents depend on enriched fixtures before Phase 4
- Spec §2 boundary rule list incomplete: PhoneRules omitted despite having `DefaultMinDigits`/`DefaultMaxDigits` constants
- DA layer canonical example inconsistency: spec shows `DataAnnotationExpected(false)` (no message), plan shows message included
- `.claude/skills/` wrappers from plan silently dropped — resolved: `scaffold-quality-tool` and `scaffold-workflow` wrappers added, so every Brain skill now has a Claude wrapper

## Topic Files
- `fluent-audit.md` — Fluent layer v4 audit findings (Network/Uri/Csv/Dict/RODict/SqlDateTime batch + Date/Time batch)
- `guard-audit.md` — Guard layer v2 audit findings (pre-migration drift cluster + all per-batch reviews)
