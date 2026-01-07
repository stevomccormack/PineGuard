using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class DateTimeUtilityTests : BaseUnitTest
{
    [Fact]
    public void ToUtc_ReturnsUtc_ForUtcInput()
    {
        var value = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);

        var utc = DateTimeUtility.ToUtc(value);

        Assert.Equal(DateTimeKind.Utc, utc.Kind);
        Assert.Equal(value, utc);
    }

    [Fact]
    public void ToUtc_UsesToUniversalTime_ForLocalInput()
    {
        var local = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Local);

        var utc = DateTimeUtility.ToUtc(local);

        Assert.Equal(DateTimeKind.Utc, utc.Kind);
        Assert.Equal(local.ToUniversalTime(), utc);
    }

    [Fact]
    public void ToUtc_SetsKindToUtc_ForUnspecifiedInput_WithoutChangingTicks()
    {
        var unspecified = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified);

        var utc = DateTimeUtility.ToUtc(unspecified);

        Assert.Equal(DateTimeKind.Utc, utc.Kind);
        Assert.Equal(unspecified.Ticks, utc.Ticks);
    }

    [Fact]
    public void Diff_ComparesUsingUtcConversion()
    {
        var local = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Local);
        var utc = local.ToUniversalTime();

        Assert.Equal(TimeSpan.Zero, DateTimeUtility.Diff(local, utc));
        Assert.Equal(TimeSpan.Zero, DateTimeUtility.Diff(utc, local));

        var unspecified = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified);
        var unspecifiedAsUtc = DateTime.SpecifyKind(unspecified, DateTimeKind.Utc);

        Assert.Equal(TimeSpan.Zero, DateTimeUtility.Diff(unspecified, unspecifiedAsUtc));
    }
}
