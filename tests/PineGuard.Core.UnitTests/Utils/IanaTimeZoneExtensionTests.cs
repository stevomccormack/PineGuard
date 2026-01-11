using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iana;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class IanaTimeZoneExtensionTests : BaseUnitTest
{
    [Fact]
    public void ToWindowsTimeZoneId_OnString_ReturnsExpected()
    {
        Assert.Null(((string?)null).ToWindowsTimeZoneId());
        Assert.Null("Not/AZone".ToWindowsTimeZoneId());

        var windowsId = "America/New_York".ToWindowsTimeZoneId();
        Assert.Equal("Eastern Standard Time", windowsId);
    }

    [Fact]
    public void ToWindowsTimeZoneId_OnIanaTimeZone_ReturnsExpected()
    {
        Assert.Null(((PineGuard.Externals.Iana.TimeZones.IanaTimeZone?)null).ToWindowsTimeZoneId());

        var ok = IanaTimeZoneUtility.TryGetIanaTimeZone("America/New_York", out var tz);
        Assert.True(ok);
        Assert.NotNull(tz);

        var windowsId = tz.ToWindowsTimeZoneId();
        Assert.Equal("Eastern Standard Time", windowsId);
    }
}
