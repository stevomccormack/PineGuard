using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Cldr;

public static class CldrTimeZoneRulesTestData
{
    public static class IsWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("valid", "Valid", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("whitespace", "\t\r\n", false),
            new("invalid", "Invalid", false),
            new("padded", " Valid ", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
