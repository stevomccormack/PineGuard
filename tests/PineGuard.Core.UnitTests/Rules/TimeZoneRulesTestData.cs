using Xunit;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeZoneRulesTestData
{
    public static class Iana_IsIanaTimeZoneId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "America/New_York => true", Value: "America/New_York", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Not/AZone => false", Value: "Not/AZone", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class Cldr_IsWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "UTC => true", Value: "UTC", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Not/AZone => false", Value: "Not/AZone", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
