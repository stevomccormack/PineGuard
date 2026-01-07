using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class SqlDateTimeRulesTests : BaseUnitTest
{
    [Fact]
    public void IsInSqlDateRange_ReturnsTrue_ForMinAndMaxDateOnly()
    {
        Assert.True(SqlDateTimeRules.IsInSqlDateRange(DateOnly.FromDateTime(SqlDateTimeRules.MinValue)));
        Assert.True(SqlDateTimeRules.IsInSqlDateRange(DateOnly.FromDateTime(SqlDateTimeRules.MaxValue)));
    }

    [Fact]
    public void IsInSqlDateTimeRange_ReturnsFalse_ForOutOfRangeDateTime()
    {
        var tooEarly = new DateTime(1600, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

        Assert.False(SqlDateTimeRules.IsInSqlDateTimeRange(tooEarly));
        Assert.False(SqlDateTimeRules.IsInSqlDateTimeRange((DateTime?)null));
    }

    [Fact]
    public void IsInSqlDateTimeRange_ReturnsFalse_ForOutOfRangeDateTimeOffset()
    {
        var tooEarly = new DateTimeOffset(1600, 1, 1, 0, 0, 0, TimeSpan.Zero);

        Assert.False(SqlDateTimeRules.IsInSqlDateTimeRange(tooEarly));
        Assert.False(SqlDateTimeRules.IsInSqlDateTimeRange((DateTimeOffset?)null));
    }
}
