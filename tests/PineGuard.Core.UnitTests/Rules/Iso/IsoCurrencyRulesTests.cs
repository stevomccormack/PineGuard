using PineGuard.Externals.Iso.Currencies;
using PineGuard.Rules.Iso;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public sealed class IsoCurrencyRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCurrencyRulesTestData.IsIsoAlpha3Code.ValidCases), MemberType = typeof(IsoCurrencyRulesTestData.IsIsoAlpha3Code))]
    public void IsIsoAlpha3Code_ReturnsTrue_ForValidCodes(IsoCurrencyRulesTestData.IsIsoAlpha3Code.Case testCase)
    {
        var result = IsoCurrencyRules.IsIsoAlpha3Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyRulesTestData.IsIsoAlpha3Code.EdgeCases), MemberType = typeof(IsoCurrencyRulesTestData.IsIsoAlpha3Code))]
    public void IsIsoAlpha3Code_ReturnsFalse_ForInvalidCodes(IsoCurrencyRulesTestData.IsIsoAlpha3Code.Case testCase)
    {
        var result = IsoCurrencyRules.IsIsoAlpha3Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyRulesTestData.IsIsoNumericCode.ValidCases), MemberType = typeof(IsoCurrencyRulesTestData.IsIsoNumericCode))]
    public void IsIsoNumericCode_ReturnsTrue_ForValidCodes(IsoCurrencyRulesTestData.IsIsoNumericCode.Case testCase)
    {
        var result = IsoCurrencyRules.IsIsoNumericCode(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyRulesTestData.IsIsoNumericCode.EdgeCases), MemberType = typeof(IsoCurrencyRulesTestData.IsIsoNumericCode))]
    public void IsIsoNumericCode_ReturnsFalse_ForInvalidCodes(IsoCurrencyRulesTestData.IsIsoNumericCode.Case testCase)
    {
        var result = IsoCurrencyRules.IsIsoNumericCode(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoCurrencyRulesTestData.IsIsoAlpha3CodeWithProvider.Cases), MemberType = typeof(IsoCurrencyRulesTestData.IsIsoAlpha3CodeWithProvider))]
    public void IsIsoAlpha3Code_UsesProvider(IsoCurrencyRulesTestData.IsIsoAlpha3CodeWithProvider.Case testCase)
    {
        var result = IsoCurrencyRules.IsIsoAlpha3Code(testCase.Value, testCase.Provider);

        Assert.Equal(testCase.Expected, result);
    }
}
