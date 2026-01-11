using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class IanaTimeZoneUtilityTestData
{
    public static class IsValidTimeZoneId
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("America/New_York => true", "America/New_York", true),
            new("Not/AZone => false", "Not/AZone", false)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null => false", null, false),
            new("whitespace => false", " ", false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
