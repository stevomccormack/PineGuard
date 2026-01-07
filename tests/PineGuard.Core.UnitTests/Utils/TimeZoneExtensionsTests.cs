using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Cldr;
using PineGuard.Utils.Iana;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TimeZoneExtensionsTests : BaseUnitTest
{
    [Fact]
    public void CldrExtension_ToIanaTimeZone_ReturnsNull_ForNullString()
    {
        string? windowsId = null;
        Assert.Null(CldrTimeZoneExtension.ToIanaTimeZone(windowsId));
    }

    [Fact]
    public void IanaExtension_ToWindowsTimeZoneId_ReturnsNull_ForNullString()
    {
        string? ianaId = null;
        Assert.Null(IanaTimeZoneExtension.ToWindowsTimeZoneId(ianaId));
    }
}
