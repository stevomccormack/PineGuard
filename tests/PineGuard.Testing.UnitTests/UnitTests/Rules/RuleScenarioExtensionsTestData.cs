using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests.Rules;

public static class RuleScenarioExtensionsTestData
{
    private static readonly RuleScenario<string?>[] SampleScenarios =
    [
        new("valid-a", "hello", true),
        new("valid-b", "world", true),
        new("invalid-a", "", false),
        new("null-input", null, false)
    ];

    public static class WhereValidOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("filters to valid only", (SampleScenarios, 2)),
            new("empty array", ([], 0))
        ];

    }

    public static class WhereInvalidOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("filters to invalid only", (SampleScenarios, 2)),
            new("empty array", ([], 0))
        ];

    }

    public static class ExceptOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, string[] excludeNames, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("excludes by name", (SampleScenarios, ["valid-a", "null-input"], 2)),
            new("excludes none", (SampleScenarios, ["nonexistent"], 4)),
            new("excludes all", (SampleScenarios, ["valid-a", "valid-b", "invalid-a", "null-input"], 0))
        ];

    }

    public static class OnlyOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, string[] includeNames, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("includes by name", (SampleScenarios, ["valid-a", "null-input"], 2)),
            new("includes none", (SampleScenarios, ["nonexistent"], 0)),
            new("includes all", (SampleScenarios, ["valid-a", "valid-b", "invalid-a", "null-input"], 4))
        ];

    }

    public static class ToRuleCasesOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("converts all scenarios", (SampleScenarios, 4)),
            new("empty array", ([], 0))
        ];

    }

    public static class ToMustCasesAutoOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("converts all scenarios", (SampleScenarios, 4)),
            new("empty array", ([], 0))
        ];

    }

    public static class ToGuardCasesParamNameOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, string paramName) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("auto-maps exception types", (SampleScenarios, "value"))
        ];

    }

    public static class ToDataAnnotationCasesAutoOps
    {
        public sealed record Case(string Name, (RuleScenario<string?>[] input, int expectedCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("converts all scenarios", (SampleScenarios, 4)),
            new("empty array", ([], 0))
        ];

    }
}
