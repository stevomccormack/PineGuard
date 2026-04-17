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

### Test Structure Compliance (per `docs/ai/specs/testing/unit-test.md`)
- Nested Operation Group pattern: outer class has NO test methods (§5.1)
- Element ordering within Op Groups: datasets first, records last (§4.4)
- Structural correspondence: Tests groups mirror TestData groups in same order (§4.5)
- Method naming (strict): `Valid_BehavesAsExpected` / `ValidAndEdge_BehavesAsExpected` / `Invalid_ThrowsAsExpected` / `ValidEdgeAndInvalid_BehavesAsExpected` (§5.1)
- Tuple property MUST be `Value` (not `Input`), elements camelCase matching exact method param names (§4.3)
- Legacy flat-pattern files are drift — new/refactored files MUST use nested pattern
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- Test Fixtures: input values from `PineGuard.Testing.Fixtures/`, `nameof` for Name, alias `F` (§10)
- Full canonical examples in §9
- **`Expected` property** (NOT `ExpectedReturn`) — records use `Expected`, tests access `testCase.Expected`
- Layer-specific Expected types: Core=`bool`, Must=`MustExpected`, Guard=`string?` or `ExpectedException`, Fluent=`FluentExpected`, DA=`bool`
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
- `.claude/skills/` wrappers from plan silently dropped — only command wrappers created; verify if intentional

### Guard Layer Pre-Migration Drift Pattern (Mar 2026)
- Legacy TestData: inner `ValidCase`/`InvalidCase` records extending `ReturnCase<TValue,TValue>`/`ThrowsCase<TValue>` instead of `TheoryData<GuardCase<T>>` sourced from fixtures via `.ToGuardCases()`
- Legacy assertion in Tests: `ThrowsCaseAssert.Expected(ex, testCase)` + explicit `Assert.Throws()` + cast `(InvalidCase)testCase` — all replaced by single `AssertResult(tc, () => ...)` call
- Split Valid/Invalid test methods (`Valid_ReturnsExpected` / `Invalid_ThrowsExpected`) instead of unified `OpName_BehavesAsExpected` method with dual `[MemberData]` attributes
- Nested Op Group classes in Tests file (static inner classes per op) instead of flat test class with one method per op
- Wrong base class: Tests inherits nothing (plain `public class`) instead of `public sealed class ... : BaseGuardUnitTest(output)`
- Missing `using PineGuard.Testing.UnitTests.GuardClauses` — TestData and Tests both lack this namespace reference for `GuardCase<T>`, `GuardExpected`, `BaseGuardUnitTest`
- Missing `using Xunit.Abstractions` in Tests file (needed for `ITestOutputHelper`)
- Empty `EdgeCases` datasets — fixture-based approach eliminates EdgeCases as a separate dataset; edge cases are included directly in `ValidCases`/`InvalidCases` via fixture `ValidEdge`/`InvalidEdge` scenario arrays
- Fixture underutilisation: `DateOnlyRulesFixtures` has `IsPast`, `IsBefore`, `IsAfter`, `IsSame`, `IsChronological`, `IsOverlapping`, `IsWithin`, `IsWithinCalendarMonths` — all of which should drive the TestData instead of hardcoded values
- `ExpectedResult` property pattern on custom records is legacy — gold standard has no `ExpectedResult`; `GuardExpected.IsValid` drives valid/throw branching inside `AssertResult`
- Comments in TestData (e.g. `// Guard.Against.Future — throws when value IS future`) violate zero-comment rule for TestData; comments in Tests above each method are acceptable per spec
- AI deliberation artifacts left as inline comments in TestData (e.g. "// or ArgumentOutOfRangeException...") — always flag and remove
- Hardcoding `paramName: PName = "value"` in test call sites bypasses `CallerArgumentExpression`; gold standard omits `paramName` and lets compiler capture it
- Source file may have 2x the ops: "Not*" (guard-against) AND affirmative (e.g. `NotIpAddress` + `IpAddress`) — missing second half = 50% op coverage; always count source methods vs TestData Op Groups
- `NetworkRulesFixtures` groups: `IsIpAddress`, `IsIpv4`, `IsIpv6`, `IsInCidr`, `IsValidHostname`, `IsPortNumber` — `IsInCidr` uses tuple `(string? ip, string cidr)` not `(string? value, string cidr)`; guard TestData must map `ip` → `value` parameter when calling `NotInCidrRange`/`InCidrRange`

### GuardStringNumberClauses stale artifact (Mar 2026)
- `GuardStringNumberClausesTestData.cs` / `GuardStringNumberClausesTests.cs` (singular) are stale placeholder files targeting a source file that does not exist
- Source is `GuardStringNumbersClauses.cs` (plural); conforming test pair already exists at `GuardStringNumbersClausesTestData.cs` / `GuardStringNumbersClausesTests.cs`
- Singular pair violates V1+V3+V4+V6+V7+V8+V9 simultaneously — recommend deletion of both files
- Watch for file-naming mismatches: plural/singular drift between `XxxClauses.cs` and `XxxClausesTestData.cs`

### GuardStringNumbersClauses Review (Mar 2026)
- ALL six Guard pre-migration drift violations apply (base class, structure, method naming, case type, fixture, dataset shape)
- `StringRulesFixtures.Numbers*` inner classes already exist in `StringRulesFixtures.Numbers.cs` — fixture alias should be `using F = PineGuard.Testing.Fixtures.StringRulesFixtures`
- Source has 19 distinct methods; TestData creates 20 Op Groups by duplicating `Zero`, `LessThan`, `GreaterThan` (they appear both standalone AND inside `LessThanOrEqual`/`GreaterThanOrEqual` combined groups — duplication is the violation)
- Guard TestData for string numbers must use `GuardCase<string?>` with `ValidCases`+`InvalidCases` from fixture `ValidScenarios`/`InvalidScenarios` sourced via `.ToGuardCases()`
- `Assert.True(true)` placeholder in valid path is a critical dead assertion — zero confidence in test correctness
- Named `paramName:` arg in Action lambdas (`Guard.Against.ZeroOrNegative("-1", paramName: "value")`) breaks "no named arguments in new(...)" rule (§4.3) — however this is inside an Action, not a record constructor, so the violation is the Action pattern itself, not named args per se

### Guard Collection Clauses Review (Mar 2026)
- All six Guard drift violations present simultaneously (see pre-migration cluster above)
- Missing ops: `HasExactCount`, `HasMinCount`, `HasMaxCount`, `HasCountBetween` (companion `Has*` to `NotHas*` ops often omitted)
- `Inclusion` optional param omission: `NotHasCountBetween` and `HasCountBetween` both accept `Inclusion inclusion = Inclusion.Inclusive` but tests only exercise default path — non-default must have at least one case
- `NotContains` fixture name mismatch: source Guard op is `NotContains(value, item)` but fixture inner class is `Contains` — Guard TestData must use `F.Contains.InvalidScenarios.ToGuardCases(...)` for valid pass-through and `F.Contains.ValidScenarios.ToGuardCases(...)` for throws
- `SubsetOf`/`NotSubsetOf` fixture: fixture inner class is `IsSubsetOf` — match guard op to correct fixture group name
- Predicate-bearing ops (`NotHasAny`, `HasAny`, `NotHasAll`, `HasAll`): `Func<T, bool>` cannot be in fixtures (§10.7); inline lambda in test method body is correct, but the fixture `ValidScenarios`/`InvalidScenarios` provide the collection inputs only

### GuardObjectClauses Review (Mar 2026)
- All six Guard pre-migration drift violations confirmed simultaneously (see pre-migration cluster above)
- `ObjectRulesFixtures` exists with groups `IsEqualTo`, `IsOfType`, `IsAssignableToType`, `IsSameReferenceAs` — fully populated, entirely unused
- Tuple elements PascalCase violation: `(string? Value, string? Other)` and `(object? A, object? B)` — must be `(value, other)` and `(a, b)` matching source params
- Tuple property named `Input` (not `Value`) in custom records for multi-param Op Groups
- Property named `ExpectedResult` (not `Expected`) in all ValidCase records
- Semantic inversion: `NotEqualTo` uses `IsEqualTo.ValidScenarios` for throws, `InvalidScenarios` for pass-through; `EqualTo` uses the opposite; same inversion for `NotSameReferenceAs`/`SameReferenceAs`
- Reference-identity Op Groups correctly use `private static readonly object ObjA/ObjB = new()` (can't be fixtures per §10.7) — this inline approach is the ONLY correct exception
- Speculative uncertainty comment inside dataset (`// null handling depends on implementation...`) violates zero-comment rule

### GuardStringClauses Review (Mar 2026)
- All six Guard pre-migration drift violations apply (base class, structure, method naming, case type, fixture, dataset shape)
- `StringRulesFixtures.cs` exists and ready; inner class names follow `IsNotNullOrEmpty`, `IsNullOrEmpty`, `IsNotNullOrWhiteSpace`, `IsNullOrWhiteSpace`, `IsExactLength`, etc. pattern
- Missing ops: complement methods `Alphabetic`, `Numeric`, `Alphanumeric`, `DigitsOnly` (no-allowed and with-allowed overloads), and `Whitespace` entirely uncovered — tests only cover `Not*` halves
- `NotAlphabetic`/`NotNumeric`/`NotAlphanumeric` have optional `char[]? inclusions` param with no test Op Group covering the non-null path
- Tuple element PascalCase violation: inner tuple `Value`, `DisallowedChars`, `AllowedChars` must be camelCase matching source params: `value`, `disallowedChars`, `allowedChars`
- Speculative inline comment in test data (`// Empty IS whitespace?`) violates zero-comments rule; remove and file separately
- Empty `EdgeCases => []` datasets + corresponding dead `[MemberData]` attributes present throughout; delete both

### GuardFilePathClauses Review (Mar 2026)
- All six Guard pre-migration drift violations confirmed (V2/V3/V4/V5/V6/V7/V8)
- Fixture `FilePathRulesFixtures.cs` fully populated: `IsSafeFileName` (2 valid, 11 invalid) + `HasFileExtension` (4 valid, 6 invalid) — TestData must use it
- Shared `PName = "value"` across ops is a correctness bug when ops have different primary param names: `NotSafeFileName` uses `value`, `NotHasFileExtension` uses `path` — one constant silently tests wrong paramName
- `ToGuardCases("path")` overload handles null vs non-null branching (`ArgumentNullException` vs `ArgumentException`) automatically; no manual `ExpectedException` construction needed

### GuardTimeOnlyRangeClauses Review (Mar 2026)
- All six Guard pre-migration drift violations confirmed (V2/V3/V4/V5/V6/V7/V8/V9)
- `TimeOnlyRangeRulesFixtures.cs` exists with `IsChronological`, `IsOverlapping`, `Contains` inner classes — TestData must use `NonNullValidScenarios`/`NonNullInvalidScenarios` on each
- Guard semantic inversion for range ops: `Overlapping` guard uses `F.IsOverlapping.NonNullInvalidScenarios.ToGuardCases(...)` for `ValidCases` and `F.IsOverlapping.NonNullValidScenarios.ToGuardCases(...)` for `InvalidCases`; `NotOverlapping` guard inverts this
- No `IsNotOverlapping` fixture group — `NotOverlapping` guard shares `F.IsOverlapping` scenarios with inversion applied
- Fixture `Contains` group drives `NotContains` guard (pass-through on Contains.InvalidScenarios) AND `Contains` guard (throw on Contains.ValidScenarios); no separate `NotContains` fixture needed
- Multi-param ops require tuple value type: `GuardCase<(TimeOnlyRange range1, TimeOnlyRange range2)>` — check source method signature for exact tuple element names
- Dead commented-out method block (`/* Impossible to test... */`) signals a constraint discovered late — remove entirely; if guard op genuinely has no invalid cases, omit `InvalidCases` dataset or leave it empty, no comment needed in Tests

### GuardHttpSecurityHeaderClauses Review (Mar 2026)
- All six Guard drift violations present simultaneously
- `RunValid<TCase,TResult>`/`RunInvalid<TCase>` private helper bridge methods are a tell-tale drift indicator — agents generate these when they can't figure out how to unify Valid/Invalid into a single `AssertResult` call; always flag and delete
- Source has 21 `Not*` ops + 12 affirmative ops (no `Not` prefix: `ContentSecurityPolicyHeader`, `ContentSecurityPolicyWithDefaults`, `StrictTransportSecurityHeader`, `StrictTransportSecurityWithDefaults`, `XContentTypeOptionsHeader`, `XContentTypeOptionsWithDefaults`, `XFrameOptionsHeader`, `XFrameOptionsWithDefaults`, `ReferrerPolicyHeader`, `ReferrerPolicyWithDefaults`, `PermissionsPolicyHeader`, `PermissionsPolicyWithDefaults`) — only `Not*` half tested, 12 ops entirely missing
- Fixture `HttpSecurityHeaderRulesFixtures.cs` is fully populated and available; TestData must use it — inner class names in fixture are `HasXxx` (e.g. `HasContentSecurityPolicyHeader`, `HasStrictTransportSecurityHeader`, `HasXContentTypeOptionsHeader`, etc.)
- Local `Headers(key, value)` helper factory in TestData is a proxy for fixture data — always a sign fixtures are not being used
- Commented-out test case left in `InvalidCases` block is a code-smell indicator (unresolved decision); must be removed or resolved
- Inline `// Helper to create dictionary` / `// NotXxx (complex)` comments in TestData violate zero-comment rule

### Guard Gold Standard Batch Review (Mar 2026 — 8 pairs: Bool/Buffer/Char/DefaultEquality/Dictionary/Email/Enum/GeoLocation/Guid)
- Gold standard (BoolClauses) itself has 2 comments in TestData — zero-comment rule violated even in declared gold; treat as impurity, not license
- Recurring spurious `using PineGuard.Testing.UnitTests.Rules;` in TestData when no `RuleScenario<T>` directly instantiated — Buffer, Char, Dictionary, Email
- Recurring `paramName: "value"` named arg in every Tests call — Buffer, Char, DefaultEquality, Dictionary, Email, GeoLocation, Guid; gold standard (Bool) does NOT pass paramName explicitly
- GeoLocationTestData constructs `RuleScenario<T>` manually via LINQ projection instead of using fixture arrays directly — logic leak into TestData; V6 partial
- GeoLocationTestData has 3 block comments — V-zero-comment
- DefaultEqualityTestData has 4 inline comments — V-zero-comment
- GuardCharTestData missing `Control` Op Group — source has `Control` method; zero coverage
- GuardEmailTestData `HasEmailAlias.InvalidCases` uses hardcoded inline data — partial V6
- GuardEnumTestData: inline `TheoryData` acceptable because EnumRulesFixtures only provides type definitions/constants, not `RuleScenario[]` arrays; document as intentional
- GuardGuidTestData: 1 source op, 1 Op Group — fully conforming; smallest pair in set

### Guard Batch (Identifier/Json/Null/Number/Owasp/Phone/Predicate/RODict) Review (Mar 2026)
- `paramName: "value"` hardcoded in Tests call sites is a recurring violation in: Json, Number, Owasp, Phone, Predicate, RODictionary — gold standard lets CallerArgumentExpression capture it
- Comments in TestData body violate zero-comment rule: Number (deliberation artifact `// Uses double directly...`)
- `GreaterThan` Op Group in NumberClausesTestData: tuple type `(int value, int min)` but accesses `s.Inputs.max` — element name `min` is semantically wrong (the bound is max), value still correct
- `NotEmpty.NullCases` third dataset in RODictionary TestData: acceptable for null→`ArgumentNullException` split via `Only()`
- GuardPhoneClausesTestData: `NotPhoneNumber` and `NotPhoneNumberString` share IDENTICAL fixture groups — potentially correct if both source ops consume the same rule, but warrants verification against source
- `// Act` + `// Assert` section comments in GuardJsonClausesTests are noise; gold standard omits them
- `// ReSharper disable once IdentifierTypo` appears in BOTH OwaspClausesTestData AND OwaspClausesTests — duplication

### Guard SqlDateTime/Task/TimeOnly/TimeSpan/StringBool/StringCasing/StringDateOnly/StringTimeOnly/StringTimeSpan Review (Mar 2026)
- Single `Cases` dataset instead of `ValidCases`/`InvalidCases` split: recurring in TimeOnly, StringTimeOnly, StringTimeSpan — always flag, gold standard requires two `[MemberData]` attrs per method
- Explicit `paramName: "task"` or `paramName: "value"` named arg at call site in Tests: Task (6 sites), TimeSpan (4 sites) — CallerArgumentExpression captures local variable name automatically; remove the named arg
- Missing ops by 50%+: TimeOnly (5/12), StringDateOnly (6/16) — always count source methods before declaring coverage complete
- `FutureOrPresent`+`Future` sharing same fixture group is a semantic risk when underlying Must ops differ (PastDateOnly vs PastOrPresentDateOnly)
- Missing return-value assertion `if (tc.Expected.IsValid) Assert.Equal(...)`: StringBool guards return `bool`, assertion omitted — always include for guards with non-void return
- Inline test method bodies collapsed to one line: TimeOnly, StringTimeOnly, StringTimeSpan Tests — acceptable but reviewable for readability
- `#pragma warning disable CS0618` in TestData (SqlDateTime) signals a stale artifact; remove once correct overload confirmed
- `DurationBetween` inline-hardcoded while sibling `NotDurationBetween` is fixture-sourced is a consistency violation — unify both via fixture
- GuardStringCasingClauses: fully conforming gold standard — use as secondary reference for affirmative+Not* op pair pattern

## Topic Files
- `fluent-audit.md` — Fluent layer v4 audit findings (two review batches: Network/Uri/Csv/Dict/RODict/SqlDateTime + Date/Time pairs)
- `fluent-audit-batch2.md` — Fluent v4 batch 2: Json/Owasp/Phone/HttpSecHeader/Identifier/Object/TimeZone/Xml (8 pairs)
