using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iso;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public sealed class IsoCurrencyUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCurrencyUtilityTestData.TryParseAlpha3.ValidCases), MemberType = typeof(IsoCurrencyUtilityTestData.TryParseAlpha3))]
    [MemberData(nameof(IsoCurrencyUtilityTestData.TryParseAlpha3.EdgeCases), MemberType = typeof(IsoCurrencyUtilityTestData.TryParseAlpha3))]
    public void TryParseAlpha3_ReturnsExpected(IsoCurrencyUtilityTestData.TryParseAlpha3.Case testCase)
    {
        var result = IsoCurrencyUtility.TryParseAlpha3(testCase.CurrencyCode, out var alpha3);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedAlpha3, alpha3);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyUtilityTestData.TryParseNumeric.ValidCases), MemberType = typeof(IsoCurrencyUtilityTestData.TryParseNumeric))]
    [MemberData(nameof(IsoCurrencyUtilityTestData.TryParseNumeric.EdgeCases), MemberType = typeof(IsoCurrencyUtilityTestData.TryParseNumeric))]
    public void TryParseNumeric_ReturnsExpected(IsoCurrencyUtilityTestData.TryParseNumeric.Case testCase)
    {
        var result = IsoCurrencyUtility.TryParseNumeric(testCase.CurrencyNumber, out var numeric);

        Assert.Equal(testCase.ExpectedSuccess, result);
        Assert.Equal(testCase.ExpectedNumeric, numeric);
    }
}
