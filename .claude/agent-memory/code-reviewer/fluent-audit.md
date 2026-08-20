# Fluent Layer v4 Audit Findings

## Prior Batch (Network/Uri/Csv/Dict/RODict/SqlDateTime — Mar 2026)
- `NullCases` split dataset + `_NullIsValid` split test method per op: critical violation — FluentSqlDateTimeExtensions and FluentTimeOnly* use this pattern; gold standard uses ONE unified `Cases` dataset with null handled by switch arm in `ToFluentCases`
- `InlineValidator<T>` usage: acceptable ONLY for multi-param ops where each test case requires a different validator configuration; NOT acceptable as substitute for named AbstractValidator subclasses on fixed-param ops
- `[Theory] [InlineData(0)]` with `int _` dummy param — RESOLVED (Mar 2026): FluentDictionaryExtensions + FluentReadOnlyDictionary Tests used this for IRuleBuilderOptions smoke tests; both are now `[Theory]` + `[MemberData]` over `TheoryData<FluentCase<T>>`, collapsed into a single `OverloadResolution` Theory (see `.claude/agent-memory/test-writer/fact-to-theory-conversion.md`). The earlier note that "[Fact] is correct" was wrong: `[Fact]` and `[InlineData]` are prohibited repo-wide (`docs/ai/specs/testing/unit-test.md` §1) and fail CI via audit-cli Rule50. Overload-resolution smoke tests use `[Theory]` + `TheoryData` with a `Func<TResult>` case `Value`.
- `#pragma warning disable CS0618` in TestData: FluentNetworkExtensionsTestData + FluentUriExtensionsTestData — signals obsolete case types; eliminate
- Spurious `using PineGuard.Testing.UnitTests.Rules` in FluentValidation TestData when no `RuleScenario<T>` instantiated: Network + Uri TestData
- Hardcoded `new FluentCase<T>(name, value, expected)` with literal data: FluentCsvExtensionsTestData; belongs in fixtures unless fixture cannot represent the data

## Date/Time Batch (DateOnly/DateOnlyRange/DateTime/DateTimeOffset/DateTimeOffsetRange/DateTimeRange/TimeOnly/TimeOnlyRange — Mar 2026)

### Universal failure: Items 9+10 across ALL 8 pairs
Every date/time pair fails fixture compliance (item 9) and null-switch compliance (item 10):
- Items 9+10 fail together: inline `new FluentCase<...>` entries with literal values instead of `AllScenarios.ToFluentCases(switch)`
- Null modelled as hardcoded row `new("Null", null, new FluentExpected(true))` instead of switch arm
- `Cases` must be: `F.IsXxx.AllScenarios.ToFluentCases(s => s.Name switch { nameof(F.IsXxx.Null) => new FluentExpected(true), _ when s.IsValid => ..., _ => ... })`

### FluentDateExtensions (Pair 1) — Complete legacy rewrite required
- Wrong base class: `BaseUnitTest` (no output param) instead of `BaseFluentUnitTest(output)`
- Custom `ValidCase` records extending `ReturnCase<T, bool>` — prohibited; use `FluentCase<T>` only
- Split `ValidCases` + `EdgeCases` datasets — prohibited; use single `Cases`
- Manual assertions: `Assert.Equal(testCase.Expected, result.IsValid)` + `Assert.EndsWith(...)` — use `AssertResult(tc, result)` only
- `[InlineData]` on `Satisfies_BehavesAsExpected` — prohibited; use `[MemberData]`
- No fixture reference whatsoever

### FluentTimeOnlyExtensions + FluentTimeOnlyRangeExtensions (Pairs 8+9) — Cases/NullCases split
- `Cases` (`TheoryData<FluentCase<TimeOnly>>`) + `NullCases` (`TheoryData<FluentCase<TimeOnly?>>`) per Op Group
- Paired test methods: `OpName_BehavesAsExpected` (from Cases) + `OpName_NullIsValid` / `OpName_Null_IsValid` (from NullCases)
- Fix: merge into single `Cases` dataset using nullable type `FluentCase<TimeOnly?>` with switch arm handling null name
- FluentTimeOnlyRangeExtensions TestData uses `FR.IsChronological.Chronological` fixture constants directly (not `AllScenarios.ToFluentCases`) — partial progress, still non-compliant

### Partial fixture usage (Pairs 4+5: DateTime + DateTimeOffset)
- Fixture alias `F` declared; individual constants referenced directly (`F.IsPast.PastDate!.Value`) instead of `AllScenarios.ToFluentCases(switch)`
- Accessing individual constants is NOT sufficient — `AllScenarios` must be consumed via `ToFluentCases` with switch expression

### Structural compliance (Pairs 2–9, excluding Pair 1)
All pass: base class, `FluentCase<T>` type, `AssertResult`, `[Theory]+[MemberData]`, instance methods, flat structure, `AbstractValidator<Model>`

## Primitive Types Batch (Bool/Buffer/Char/Null/Number/BitWise/Enum/DefaultEquality/Guid — Mar 2026)

### Cross-cutting: `// Act` / `// Assert` markers missing
Every pair EXCEPT gold standard Bool is missing `// Act` and `// Assert` section markers in test method bodies. Systemic omission. Gold standard includes them; all others drop them. This is the single most common violation in this batch.

### FluentCharExtensions — Item 10 FAIL (Null handling)
- All 17 Op Groups use `.Except(nameof(F.IsXxx.Null))` to strip null from dataset; output still `FluentCase<char?>` but Tests access `tc.Value!.Value` (force-unwrap)
- This is NOT the permitted `.Except() + .Project()` non-nullable variant — null is silently excluded, not tested
- Fix: add switch null arm `nameof(F.IsXxx.Null) => new FluentExpected(true)` OR use `.Project(v => v!.Value)` + model `public char? Value` for true non-nullable variant
- Also has spurious `using PineGuard.Testing.UnitTests.Rules` (line 2) — gold standard BoolTestData lacks it

### FluentNumberExtensions — Items 4 + 9 FAIL
- Item 9: Five Op Groups (`OutOfRange`, `NotApproximately`, `NotMultipleOf`, `NotFinite`, `NotNaN`) define `private static RuleScenario<T>[]` arrays inline — hard prohibition; move to `NumberRulesFixtures`
- Item 4: Six Op Groups (`Even`, `Odd`, `Finite`, `NotFinite`, `NaN`, `NotNaN`) expose `IntCases`/`LongCases` or `FloatCases`/`DoubleCases` instead of single `Cases` — fix by splitting into separate Op Group classes per type (`EvenInt`, `EvenLong`, etc.)
- Message `"tolerance requires a non-null tolerance."` starts lowercase — inconsistent with all other messages (sentence-case capital)
- `using PineGuard.Testing.UnitTests.Rules` (line 3) is NOT spurious here — file legitimately uses `RuleScenario<T>` directly for inline arrays (which is itself the violation)

### FluentEnumExtensions — `#pragma warning disable CS0612` scope
- File-scope suppress in both TestData + Tests; acceptable for Obsolete op group testing but should be narrowed to the specific method or class

### FluentBitWise — `.Where(...).ToArray()` filter pattern
- Filtering `RightNull` scenario via LINQ before `ToFluentCases` is acceptable when the filtered scenario is architecturally incompatible with the validator (null `right` param cannot be passed to validator constructor)
- This is an intentional exclusion, not a null-bypass violation

### All 9 pairs: Op-name inline comments
- Gold standard has `// FluentBoolExtensions.True` and `// FluentBoolExtensions.False` above each test method
- Buffer, Char, Null, Number, BitWise, Enum, DefaultEquality, Guid all lack these — spec §"Test Structure" requires them
