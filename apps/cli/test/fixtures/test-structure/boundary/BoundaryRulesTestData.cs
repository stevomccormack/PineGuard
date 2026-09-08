using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Core.UnitTests.Rules;

public static class BoundaryRulesTestData
{
    // Single "Cases" rollup — valid per v11 §4.1 (Core/Fluent/DataAnnotations
    // layers use exactly this shape). The rule must not treat this as
    // "missing ValidCases/EdgeCases/InvalidCases".
    public static class IsRollup
    {
        public static TheoryData<RuleCase<string?>> Cases =>
        [
            new("rollup", "ok", new RuleExpected(true)),
        ];
    }

    // Full Valid/Edge/Invalid split, correctly ordered — also valid per
    // v11 §4.1/§4.4. Both group shapes must come back clean.
    public static class IsSplit
    {
        public static TheoryData<RuleCase<string?>> ValidCases =>
        [
            new("good", "ok", new RuleExpected(true)),
        ];

        public static TheoryData<RuleCase<string?>> EdgeCases =>
        [
            new("edge", "", new RuleExpected(false)),
        ];

        public static TheoryData<RuleCase<string?>> InvalidCases =>
        [
            new("bad", null, new RuleExpected(false)),
        ];
    }

    // P4.3 finding #5 (plan decision 5): NullCases WITHOUT its required
    // ValidCases/InvalidCases co-occurrence (fixture.md §12.2) is simply an
    // unrecognised dataset name, exactly like any other unknown name today
    // (no "unknown dataset name" check exists) — even though it is `=> [];`
    // empty, this must NOT be flagged. Proves the recognition is genuinely
    // conditional, not a blanket allowlist.
    public static class IsNullOnly
    {
        public static TheoryData<RuleCase<string?>> Cases =>
        [
            new("only", "ok", new RuleExpected(true)),
        ];

        public static TheoryData<RuleCase<string?>> NullCases => [];
    }
}
