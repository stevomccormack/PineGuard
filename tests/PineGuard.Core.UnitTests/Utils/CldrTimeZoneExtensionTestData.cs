using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CldrTimeZoneExtensionTestData
{
    public static class ToIanaTimeZone
    {
        public static TheoryData<Case> ValidCases =>
        [
            new ("Eastern Standard Time", "Eastern Standard Time", true),
            new ("trimmed", "  Eastern Standard Time  ", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new ("null", null, false),
            new ("empty", string.Empty, false),
            new ("space", " ", false),
            new ("not a zone", "Not/AZone", false),
            new ("definitely not", "Definitely Not A Time Zone", false),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? WindowsTimeZoneId, bool ExpectedHasValue)
            : HasCase<string?>(Name, WindowsTimeZoneId, ExpectedHasValue);

        #endregion Cases
    }
}
