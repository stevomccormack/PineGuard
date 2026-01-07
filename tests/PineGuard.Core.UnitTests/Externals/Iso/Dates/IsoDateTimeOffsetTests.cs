using PineGuard.Iso.Dates;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public sealed class IsoDateTimeOffsetTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.TryParse.ValidCases), MemberType = typeof(IsoDateTimeOffsetTestData.TryParse))]
    [MemberData(nameof(IsoDateTimeOffsetTestData.TryParse.EdgeCases), MemberType = typeof(IsoDateTimeOffsetTestData.TryParse))]
    public void TryParse_ReturnsExpected(IsoDateTimeOffsetTestData.TryParse.ValidCase testCase)
    {
        // Arrange

        // Act
        var ok = IsoDateTimeOffset.TryParse(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedResult, result);

        if (testCase.Expected)
        {
            Assert.Matches(IsoDateTimeOffset.AllPatternsRegex(), testCase.Value);
        }
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.Parse.ValidCases), MemberType = typeof(IsoDateTimeOffsetTestData.Parse))]
    public void Parse_WhenValid_ReturnsExpected(IsoDateTimeOffsetTestData.Parse.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = IsoDateTimeOffset.Parse(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedResult, result);
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.Parse.InvalidCases), MemberType = typeof(IsoDateTimeOffsetTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsExpected(IsoDateTimeOffsetTestData.Parse.InvalidCase testCase)
    {
        // Arrange

        // Act
        _ = Assert.Throws(testCase.ExpectedException.Type, () => _ = IsoDateTimeOffset.Parse(testCase.Value!));
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.ToIsoString.ValidCases), MemberType = typeof(IsoDateTimeOffsetTestData.ToIsoString))]
    public void ToIsoString_UsesRoundTripFormat(IsoDateTimeOffsetTestData.ToIsoString.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = testCase.Value.ToIsoString();

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.True(result.Contains('T', StringComparison.Ordinal));
    }
}
