using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CldrTimeZoneExtensionTestData
{
    public static class ToIanaTimeZone
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("Eastern Standard Time", "Eastern Standard Time", true) },
            { new Case("trimmed", "  Eastern Standard Time  ", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("not a zone", "Not/AZone", false) },
            { new Case("definitely not", "Definitely Not A Time Zone", false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? WindowsTimeZoneId, bool ExpectedHasValue)
            : HasCase<string?>(Name, WindowsTimeZoneId, ExpectedHasValue);

        #endregion Cases
    }
}
