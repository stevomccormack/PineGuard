using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iana.TimeZones;

public sealed class IanaTimeZoneTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IanaTimeZoneTestData.Constructor.ValidCases), MemberType = typeof(IanaTimeZoneTestData.Constructor))]
    [MemberData(nameof(IanaTimeZoneTestData.Constructor.EdgeCases), MemberType = typeof(IanaTimeZoneTestData.Constructor))]
    public void Constructor_NormalizesProperties(IanaTimeZoneTestData.Constructor.ValidCase testCase)
    {
        // Act
        var tz = new IanaTimeZone(testCase.Id, testCase.CountryAlpha2Codes, testCase.Coordinates, testCase.Comment);

        // Assert
        Assert.Equal(testCase.ExpectedId, tz.Id);
        Assert.Equal(testCase.ExpectedCoordinates, tz.Coordinates);
        Assert.Equal(testCase.ExpectedComment, tz.Comment);
        Assert.Equal(testCase.ExpectedCountryAlpha2Codes, tz.CountryAlpha2Codes);
        Assert.Equal(testCase.ExpectedId, tz.ToString());
    }

    [Theory]
    [MemberData(nameof(IanaTimeZoneTestData.Constructor.InvalidCases), MemberType = typeof(IanaTimeZoneTestData.Constructor))]
    public void Constructor_Throws_ForInvalidInputs(IanaTimeZoneTestData.Constructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = new IanaTimeZone(invalidCase.Id!, invalidCase.CountryAlpha2Codes!, invalidCase.Coordinates!, invalidCase.Comment));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Fact]
    public void EqualityAndHashCode_AreStable_ForSameValues()
    {
        // Arrange
        var left = new IanaTimeZone("Europe/London", ["GB"], "+513030-0000731", "Comment");
        var right = left with { };

        // Act
        var equals = left.Equals(right);
        var hashLeft = left.GetHashCode();
        var hashRight = right.GetHashCode();

        // Assert
        Assert.NotSame(left, right);
        Assert.True(equals);
        Assert.Equal(hashLeft, hashRight);
    }
}
