using PineGuard.Externals.Iso.Dates;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public sealed class IsoDateOnlyTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.TryParse.ValidCases), MemberType = typeof(IsoDateOnlyTestData.TryParse))]
    [MemberData(nameof(IsoDateOnlyTestData.TryParse.EdgeCases), MemberType = typeof(IsoDateOnlyTestData.TryParse))]
    public void TryParse_ReturnsExpected(IsoDateOnlyTestData.TryParse.ValidCase testCase)
    {
        // Act
        var ok = IsoDateOnly.TryParse(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, ok);
        Assert.Equal(testCase.ExpectedOutValue, result);
    }

    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.Parse.ValidCases), MemberType = typeof(IsoDateOnlyTestData.Parse))]
    public void Parse_WhenValid_ReturnsExpectedDateOnly(IsoDateOnlyTestData.Parse.ValidCase testCase)
    {
        // Act
        var result = IsoDateOnly.Parse(testCase.Value!);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Matches(IsoDateOnly.ExactPatternRegex(), testCase.Value!);
    }

    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.Parse.InvalidCases), MemberType = typeof(IsoDateOnlyTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsExpected(IThrowsCase testCase)
    {
        // Arrange
        var invalidCase = Assert.IsType<IsoDateOnlyTestData.Parse.InvalidCase>(testCase);

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = IsoDateOnly.Parse(invalidCase.Value!));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.ToIsoString.ValidCases), MemberType = typeof(IsoDateOnlyTestData.ToIsoString))]
    public void ToIsoString_FormatsAsExactIsoDate(IsoDateOnlyTestData.ToIsoString.ValidCase testCase)
    {
        // Act
        var result = testCase.Value.ToIsoString();

        // Assert
        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Matches(IsoDateOnly.ExactPatternRegex(), result);
    }
}
