using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Cldr;

public static class DefaultCldrWindowsTimeZoneProviderTestData
{
    public static class NullOrWhitespaceString
    {
        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("whitespace", "\t\r\n", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
