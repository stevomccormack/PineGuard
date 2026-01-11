using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesBoolTestData
{
    public static class IsTrue
    {
        public static TheoryData<Case> Cases =>
        [
            new("'true'", "true", true),
            new("trimmed true", " True ", true),
            new("'false'", "false", false),
            new("non-bool", "notabool", false),
            new("null", null, false),
            new("whitespace", " ", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsFalse
    {
        public static TheoryData<Case> Cases =>
        [
            new("'false'", "false", true),
            new("trimmed false", " False ", true),
            new("'true'", "true", false),
            new("non-bool", "notabool", false),
            new("null", null, false),
            new("whitespace", " ", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsNullOrTrue
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, true),
            new("empty", string.Empty, false),
            new("whitespace", " ", false),
            new("'true'", "true", true),
            new("trimmed true", " True ", true),
            new("'false'", "false", false),
            new("non-bool", "notabool", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class IsNullOrFalse
    {
        public static TheoryData<Case> Cases =>
        [
            new("null", null, true),
            new("'false'", "false", true),
            new("'true'", "true", false),
            new("non-bool", "notabool", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }
}
