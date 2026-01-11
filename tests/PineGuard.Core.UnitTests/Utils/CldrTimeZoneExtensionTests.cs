using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Cldr;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CldrTimeZoneExtensionTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(CldrTimeZoneExtensionTestData.ToIanaTimeZone.ValidCases), MemberType = typeof(CldrTimeZoneExtensionTestData.ToIanaTimeZone))]
    [MemberData(nameof(CldrTimeZoneExtensionTestData.ToIanaTimeZone.EdgeCases), MemberType = typeof(CldrTimeZoneExtensionTestData.ToIanaTimeZone))]
    public void ToIanaTimeZone_OnString_ReturnsExpected(CldrTimeZoneExtensionTestData.ToIanaTimeZone.Case testCase)
    {
        var tz = testCase.WindowsTimeZoneId.ToIanaTimeZone();

        if (testCase.ExpectedHasValue)
        {
            Assert.NotNull(tz);
            Assert.Equal("America/New_York", tz.Id);
        }
        else
        {
            Assert.Null(tz);
        }
    }

    [Fact]
    public void ToIanaTimeZone_OnTimeZoneInfo_ReturnsExpected()
    {
        Assert.Null(((TimeZoneInfo?)null).ToIanaTimeZone());

        var ianaTz = TimeZoneInfo.CreateCustomTimeZone("America/New_York", TimeSpan.FromHours(-5), "EST", "EST");

        var ianaResult = ianaTz.ToIanaTimeZone();
        Assert.NotNull(ianaResult);
        Assert.Equal("America/New_York", ianaResult.Id);

        var windowsTz = TimeZoneInfo.CreateCustomTimeZone("Eastern Standard Time", TimeSpan.FromHours(-5), "EST", "EST");

        var windowsResult = windowsTz.ToIanaTimeZone();
        Assert.NotNull(windowsResult);
        Assert.Equal("America/New_York", windowsResult.Id);
    }
}
