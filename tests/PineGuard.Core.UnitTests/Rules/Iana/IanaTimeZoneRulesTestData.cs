using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iana;

public static class IanaTimeZoneRulesTestData
{
    public static class IsIanaTimeZoneId
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("valid", "America/New_York", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("invalid", "Not/AZone", false),
            new("null", null, false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
