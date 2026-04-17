namespace PineGuard.Testing.UnitTests.UnitTests.Rules;

public static class RuleScenarioTestData
{
    public static class Construction
    {
        public sealed record Case(string Name, (string scenarioName, string? inputs, bool isValid) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid scenario", ("valid-input", "hello", true)),
            new("invalid scenario", ("invalid-input", "bad", false)),
            new("null-input scenario", ("null-input", null, false))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("empty name", (string.Empty, "x", true))
        ];
    }

    public static class IsNull
    {
        public sealed record Case(string Name, (string? inputs, bool expectedIsNull) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("non-null input", ("hello", false)),
            new("null input", (null, true))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("empty string input", (string.Empty, false))
        ];
    }
}
