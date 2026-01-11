using PineGuard.Externals.Cldr.TimeZones;
using PineGuard.Rules.Cldr;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Cldr;

public sealed class CldrTimeZoneRulesTests : BaseUnitTest
{
    private sealed class StubProvider : ICldrWindowsTimeZoneProvider
    {
        public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId) =>
            string.Equals(windowsTimeZoneId, "Valid", StringComparison.Ordinal);

        public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
        {
            ianaTimeZoneIds = [];
            return false;
        }

        public bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId)
        {
            windowsTimeZoneId = string.Empty;
            return false;
        }
    }

    [Theory]
    [MemberData(nameof(CldrTimeZoneRulesTestData.IsWindowsTimeZoneId.ValidCases), MemberType = typeof(CldrTimeZoneRulesTestData.IsWindowsTimeZoneId))]
    public void IsWindowsTimeZoneId_ReturnsTrue_WhenProviderRecognizesValue(CldrTimeZoneRulesTestData.IsWindowsTimeZoneId.Case testCase)
    {
        // Arrange
        var provider = new StubProvider();

        // Act
        var result = CldrTimeZoneRules.IsWindowsTimeZoneId(testCase.Value, provider);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(CldrTimeZoneRulesTestData.IsWindowsTimeZoneId.EdgeCases), MemberType = typeof(CldrTimeZoneRulesTestData.IsWindowsTimeZoneId))]
    public void IsWindowsTimeZoneId_ReturnsFalse_ForWhitespaceOrUnknownValues(CldrTimeZoneRulesTestData.IsWindowsTimeZoneId.Case testCase)
    {
        // Arrange
        var provider = new StubProvider();

        // Act
        var result = CldrTimeZoneRules.IsWindowsTimeZoneId(testCase.Value, provider);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Fact]
    public void IsWindowsTimeZoneId_UsesDefaultProvider_WhenProviderIsNull()
    {
        // Act
        var result = CldrTimeZoneRules.IsWindowsTimeZoneId("This is not a Windows time zone id", provider: null);

        // Assert
        Assert.False(result);
    }
}
