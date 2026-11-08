using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class TemporalConfigurationTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateOnlyConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.DateOnlyConfiguration))]
    public void DateOnly_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<DateOnly>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateOnlyRangeConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.DateOnlyRangeConfiguration))]
    public void DateOnlyRange_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<DateOnlyRange>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateTimeConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.DateTimeConfiguration))]
    public void DateTime_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<DateTime>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateTimeOffsetConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.DateTimeOffsetConfiguration))]
    public void DateTimeOffset_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<DateTimeOffset>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateTimeOffsetRangeConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.DateTimeOffsetRangeConfiguration))]
    public void DateTimeOffsetRange_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<DateTimeOffsetRange>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateTimeRangeConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.DateTimeRangeConfiguration))]
    public void DateTimeRange_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<DateTimeRange>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.TimeOnlyConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.TimeOnlyConfiguration))]
    public void TimeOnly_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<TimeOnly>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.TimeSpanConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.TimeSpanConfiguration))]
    public void TimeSpan_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<TimeSpan>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.TimeOnlyRangeConfiguration.InvalidCases), MemberType = typeof(TemporalConfigurationTestData.TimeOnlyRangeConfiguration))]
    public void TimeOnlyRange_RejectsConfigurationBeforeInput(MustCase<Func<MustResult<TimeOnlyRange>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateTimeInclusive.ValidCases), MemberType = typeof(TemporalConfigurationTestData.DateTimeInclusive))]
    public void DateTimeInclusive_BehavesAsExpected(MustCase<Func<MustResult<DateTime>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.DateTimeOffsetInclusive.ValidCases), MemberType = typeof(TemporalConfigurationTestData.DateTimeOffsetInclusive))]
    public void DateTimeOffsetInclusive_BehavesAsExpected(MustCase<Func<MustResult<DateTimeOffset>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TemporalConfigurationTestData.StringTimeOnlyInclusive.ValidCases), MemberType = typeof(TemporalConfigurationTestData.StringTimeOnlyInclusive))]
    public void StringTimeOnlyInclusive_BehavesAsExpected(MustCase<Func<MustResult<TimeOnly>>> tc)
    {
        // Act
        var result = tc.Value();

        // Assert
        AssertResult(tc, result);
    }

}
