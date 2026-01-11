using PineGuard.Externals.Iso.Dates;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public sealed class IsoDateTimeOffsetTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.TryParse.ValidCases), MemberType = typeof(IsoDateTimeOffsetTestData.TryParse))]
    [MemberData(nameof(IsoDateTimeOffsetTestData.TryParse.EdgeCases), MemberType = typeof(IsoDateTimeOffsetTestData.TryParse))]
    public void TryParse_ReturnsExpected(IsoDateTimeOffsetTestData.TryParse.ValidCase testCase)
    {
        // Act
        var ok = IsoDateTimeOffset.TryParse(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, result);

        if (testCase.ExpectedReturn)
        {
            Assert.Matches(IsoDateTimeOffset.AllPatternsRegex(), testCase.Value!);
        }
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.Parse.ValidCases), MemberType = typeof(IsoDateTimeOffsetTestData.Parse))]
    public void Parse_WhenValid_ReturnsExpected(IsoDateTimeOffsetTestData.Parse.ValidCase testCase)
    {
        // Act
        var result = IsoDateTimeOffset.Parse(testCase.Value!);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.Parse.InvalidCases), MemberType = typeof(IsoDateTimeOffsetTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsExpected(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoDateTimeOffsetTestData.Parse.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = IsoDateTimeOffset.Parse(invalidCase.Value!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeOffsetTestData.ToIsoString.ValidCases), MemberType = typeof(IsoDateTimeOffsetTestData.ToIsoString))]
    public void ToIsoString_UsesRoundTripFormat(IsoDateTimeOffsetTestData.ToIsoString.ValidCase testCase)
    {
        // Act
        var result = testCase.Value.ToIsoString();

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result.Contains('T', StringComparison.Ordinal));
    }
}
