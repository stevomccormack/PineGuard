using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iso;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public sealed class IsoCountryUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCountryUtilityTestData.TryParseAlpha2.ValidCases), MemberType = typeof(IsoCountryUtilityTestData.TryParseAlpha2))]
    [MemberData(nameof(IsoCountryUtilityTestData.TryParseAlpha2.EdgeCases), MemberType = typeof(IsoCountryUtilityTestData.TryParseAlpha2))]
    public void TryParseAlpha2_ReturnsExpected(IsoCountryUtilityTestData.TryParseAlpha2.Case testCase)
    {
        var result = IsoCountryUtility.TryParseAlpha2(testCase.CountryCode, out var alpha2);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedAlpha2, alpha2);
    }

    [Theory]
    [MemberData(nameof(IsoCountryUtilityTestData.TryParseAlpha3.ValidCases), MemberType = typeof(IsoCountryUtilityTestData.TryParseAlpha3))]
    [MemberData(nameof(IsoCountryUtilityTestData.TryParseAlpha3.EdgeCases), MemberType = typeof(IsoCountryUtilityTestData.TryParseAlpha3))]
    public void TryParseAlpha3_ReturnsExpected(IsoCountryUtilityTestData.TryParseAlpha3.Case testCase)
    {
        var result = IsoCountryUtility.TryParseAlpha3(testCase.CountryCode, out var alpha3);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedAlpha3, alpha3);
    }

    [Theory]
    [MemberData(nameof(IsoCountryUtilityTestData.TryParseNumeric.ValidCases), MemberType = typeof(IsoCountryUtilityTestData.TryParseNumeric))]
    [MemberData(nameof(IsoCountryUtilityTestData.TryParseNumeric.EdgeCases), MemberType = typeof(IsoCountryUtilityTestData.TryParseNumeric))]
    public void TryParseNumeric_ReturnsExpected(IsoCountryUtilityTestData.TryParseNumeric.Case testCase)
    {
        var result = IsoCountryUtility.TryParseNumeric(testCase.CountryNumber, out var numeric);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedNumeric, numeric);
    }
}
