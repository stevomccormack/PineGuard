namespace PineGuard.Core.UnitTests.Rules.Cldr;

public static class DefaultCldrWindowsTimeZoneProviderTestData
{
    public static class NullOrWhitespaceString
    {
        public static TheoryData<Case> EdgeCases => new()
        {
            { new("null", null, false) },
            { new("empty", string.Empty, false) },
            { new("space", " ", false) },
            { new("whitespace", "\t\r\n", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
