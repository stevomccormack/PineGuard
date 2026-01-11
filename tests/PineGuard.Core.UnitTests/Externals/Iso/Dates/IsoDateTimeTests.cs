using PineGuard.Externals.Iso.Dates;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public sealed class IsoDateTimeTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateTimeTestData.TryParse.ValidCases), MemberType = typeof(IsoDateTimeTestData.TryParse))]
    [MemberData(nameof(IsoDateTimeTestData.TryParse.EdgeCases), MemberType = typeof(IsoDateTimeTestData.TryParse))]
    public void TryParse_ReturnsExpected(IsoDateTimeTestData.TryParse.ValidCase testCase)
    {
        // Act
        var ok = IsoDateTime.TryParse(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, result);
        Assert.Equal(DateTimeKind.Unspecified, result.Kind);

        if (testCase.ExpectedReturn)
        {
            Assert.Matches(IsoDateTime.AllPatternsRegex(), testCase.Value!);
        }
        else
        {
            Assert.DoesNotMatch(IsoDateTime.AllPatternsRegex(), testCase.Value ?? string.Empty);
        }
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeTestData.Parse.ValidCases), MemberType = typeof(IsoDateTimeTestData.Parse))]
    public void Parse_WhenValid_ReturnsExpectedDateTime(IsoDateTimeTestData.Parse.ValidCase testCase)
    {
        // Act
        var result = IsoDateTime.Parse(testCase.Value!);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Matches(IsoDateTime.AllPatternsRegex(), testCase.Value!);
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeTestData.Parse.InvalidCases), MemberType = typeof(IsoDateTimeTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsExpected(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoDateTimeTestData.Parse.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = IsoDateTime.Parse(invalidCase.Value!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(IsoDateTimeTestData.ToIsoString.ValidCases), MemberType = typeof(IsoDateTimeTestData.ToIsoString))]
    public void ToIsoString_UsesRoundTripFormat(IsoDateTimeTestData.ToIsoString.ValidCase testCase)
    {
        // Act
        var result = testCase.Value.ToIsoString();

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result.Contains('T', StringComparison.Ordinal));
    }
}
