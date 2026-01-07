using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using System.Globalization;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesDateOnlyTests : BaseUnitTest
{
    [Fact]
    public void IsInPast_IsInFuture_ReturnExpected()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);
        var tomorrow = today.AddDays(1);

        var todayText = today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var yesterdayText = yesterday.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var tomorrowText = tomorrow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsInPast(null));
        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsInFuture(null));

        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsInPast(todayText));
        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsInFuture(todayText));

        Assert.True(PineGuard.Rules.StringRules.DateOnly.IsInPast(yesterdayText));
        Assert.True(PineGuard.Rules.StringRules.DateOnly.IsInFuture(tomorrowText));
    }

    [Fact]
    public void IsBetween_ReturnExpected()
    {
        var min = new DateOnly(2020, 1, 1);
        var max = new DateOnly(2020, 1, 31);

        Assert.True(PineGuard.Rules.StringRules.DateOnly.IsBetween("2020-01-01", min, max, Inclusion.Inclusive));
        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsBetween("2020-01-01", min, max, Inclusion.Exclusive));

        Assert.True(PineGuard.Rules.StringRules.DateOnly.IsBetween("2020-01-15", min, max));
        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsBetween("not-a-date", min, max));
        Assert.False(PineGuard.Rules.StringRules.DateOnly.IsBetween(null, min, max));
    }
}
