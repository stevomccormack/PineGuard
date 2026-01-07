using PineGuard.Iso.Dates;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public sealed class IsoDateOnlyTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.TryParse.ValidCases), MemberType = typeof(IsoDateOnlyTestData.TryParse))]
    [MemberData(nameof(IsoDateOnlyTestData.TryParse.EdgeCases), MemberType = typeof(IsoDateOnlyTestData.TryParse))]
    public void TryParse_ReturnsExpected(IsoDateOnlyTestData.TryParse.ValidCase testCase)
    {
        // Arrange

        // Act
        var ok = IsoDateOnly.TryParse(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedResult, result);
    }

    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.Parse.ValidCases), MemberType = typeof(IsoDateOnlyTestData.Parse))]
    public void Parse_WhenValid_ReturnsExpectedDateOnly(IsoDateOnlyTestData.Parse.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = IsoDateOnly.Parse(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedResult, result);
        Assert.Matches(IsoDateOnly.ExactPatternRegex(), testCase.Value);
    }

    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.Parse.InvalidCases), MemberType = typeof(IsoDateOnlyTestData.Parse))]
    public void Parse_WhenInvalid_ThrowsExpected(IsoDateOnlyTestData.Parse.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = IsoDateOnly.Parse(testCase.Value!));

        // Assert
        if (testCase.ExpectedException.MessageContains is not null)
            Assert.Contains(testCase.ExpectedException.MessageContains, ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(IsoDateOnlyTestData.ToIsoString.ValidCases), MemberType = typeof(IsoDateOnlyTestData.ToIsoString))]
    public void ToIsoString_FormatsAsExactIsoDate(IsoDateOnlyTestData.ToIsoString.ValidCase testCase)
    {
        // Arrange

        // Act
        var result = testCase.Value.ToIsoString();

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Matches(IsoDateOnly.ExactPatternRegex(), result);
    }
}
