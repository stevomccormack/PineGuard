using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iso;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public sealed class IsoLanguageUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoLanguageUtilityTestData.TryParseAlpha2.ValidCases), MemberType = typeof(IsoLanguageUtilityTestData.TryParseAlpha2))]
    [MemberData(nameof(IsoLanguageUtilityTestData.TryParseAlpha2.EdgeCases), MemberType = typeof(IsoLanguageUtilityTestData.TryParseAlpha2))]
    public void TryParseAlpha2_ReturnsExpected(IsoLanguageUtilityTestData.TryParseAlpha2.Case testCase)
    {
        var result = IsoLanguageUtility.TryParseAlpha2(testCase.LanguageCode, out var alpha2);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedAlpha2, alpha2);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageUtilityTestData.TryParseAlpha3.ValidCases), MemberType = typeof(IsoLanguageUtilityTestData.TryParseAlpha3))]
    [MemberData(nameof(IsoLanguageUtilityTestData.TryParseAlpha3.EdgeCases), MemberType = typeof(IsoLanguageUtilityTestData.TryParseAlpha3))]
    public void TryParseAlpha3_ReturnsExpected(IsoLanguageUtilityTestData.TryParseAlpha3.Case testCase)
    {
        var result = IsoLanguageUtility.TryParseAlpha3(testCase.LanguageCode, out var alpha3);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedAlpha3, alpha3);
    }
}
