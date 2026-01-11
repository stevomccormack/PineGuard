using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iso;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public sealed class IsoDateUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateUtilityTestData.TryParseDateOnly.ValidCases), MemberType = typeof(IsoDateUtilityTestData.TryParseDateOnly))]
    [MemberData(nameof(IsoDateUtilityTestData.TryParseDateOnly.EdgeCases), MemberType = typeof(IsoDateUtilityTestData.TryParseDateOnly))]
    public void TryParseDateOnly_ReturnsExpected(IsoDateUtilityTestData.TryParseDateOnly.Case testCase)
    {
        var result = IsoDateUtility.TryParseDateOnly(testCase.Value, out var date);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedDateOnly, date);
    }

    [Theory]
    [MemberData(nameof(IsoDateUtilityTestData.TryParseDateTime.ValidCases), MemberType = typeof(IsoDateUtilityTestData.TryParseDateTime))]
    [MemberData(nameof(IsoDateUtilityTestData.TryParseDateTime.EdgeCases), MemberType = typeof(IsoDateUtilityTestData.TryParseDateTime))]
    public void TryParseDateTime_ReturnsExpected(IsoDateUtilityTestData.TryParseDateTime.Case testCase)
    {
        var result = IsoDateUtility.TryParseDateTime(testCase.Value, out var dateTime);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedDateTime, dateTime);
    }

    [Theory]
    [MemberData(nameof(IsoDateUtilityTestData.TryParseDateTimeOffset.ValidCases), MemberType = typeof(IsoDateUtilityTestData.TryParseDateTimeOffset))]
    [MemberData(nameof(IsoDateUtilityTestData.TryParseDateTimeOffset.EdgeCases), MemberType = typeof(IsoDateUtilityTestData.TryParseDateTimeOffset))]
    public void TryParseDateTimeOffset_ReturnsExpected(IsoDateUtilityTestData.TryParseDateTimeOffset.Case testCase)
    {
        var result = IsoDateUtility.TryParseDateTimeOffset(testCase.Value, out var dto);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedDateTimeOffset, dto);
    }
}
