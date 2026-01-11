using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeZoneRulesTestData
{
    public static class Iana
	{
        public static class IsTimeZoneId
        {
            public static TheoryData<Case> ValidCases =>
            [
                new("America/New_York => true", "America/New_York", true),
            ];

            public static TheoryData<Case> EdgeCases =>
            [
                new("Not/AZone => false", "Not/AZone", false),
                new("null => false", null, false),
            ];

            #region Case Records

            public sealed record Case(string Name, string? Value, bool ExpectedReturn)
                : IsCase<string?>(Name, Value, ExpectedReturn);

            #endregion
        }
	}

    public static class Cldr
	{
        public static class IsWindowsTimeZoneId
        {
            public static TheoryData<Case> ValidCases =>
            [
                new("UTC => true", "UTC", true),
            ];

            public static TheoryData<Case> EdgeCases =>
            [
                new("Not/AZone => false", "Not/AZone", false),
                new("null => false", null, false),
            ];

            #region Case Records

            public sealed record Case(string Name, string? Value, bool ExpectedReturn)
                : IsCase<string?>(Name, Value, ExpectedReturn);

            #endregion
        }
	}
}
