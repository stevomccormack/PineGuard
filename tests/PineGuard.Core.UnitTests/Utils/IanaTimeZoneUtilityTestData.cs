using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class IanaTimeZoneUtilityTestData
{
    public static class IsValidTimeZoneId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "America/New_York => true", Input: "America/New_York", ExpectedReturn: true) },
            { new Case(Name: "Not/AZone => false", Input: "Not/AZone", ExpectedReturn: false) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Input: null, ExpectedReturn: false) },
            { new Case(Name: "whitespace => false", Input: " ", ExpectedReturn: false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedReturn)
            : HasCase<string?>(Name, Input, ExpectedReturn);

        #endregion Cases
    }
}
