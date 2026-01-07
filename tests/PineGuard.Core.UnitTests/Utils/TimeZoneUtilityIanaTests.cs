using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TimeZoneUtilityIanaTests : BaseUnitTest
{
    [Fact]
    public void TryParseTimeZoneId_Trims()
    {
        var parsed = TimeZoneUtility.Iana.TryParseTimeZoneId(" America/New_York ", out var id);

        Assert.True(parsed);
        Assert.Equal("America/New_York", id);
    }

    [Fact]
    public void IsValidTimeZoneId_ReturnsTrue_ForKnownZone()
    {
        Assert.True(TimeZoneUtility.Iana.IsValidTimeZoneId("America/New_York"));
    }
}
