using PineGuard.Externals.Iso.Countries;
using PineGuard.Rules.Iso;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public sealed class IsoCountryRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoAlpha2Code.ValidCases), MemberType = typeof(IsoCountryRulesTestData.IsIsoAlpha2Code))]
    public void IsIsoAlpha2Code_ReturnsTrue_ForValidCodes(IsoCountryRulesTestData.IsIsoAlpha2Code.Case testCase)
    {
        var result = IsoCountryRules.IsIsoAlpha2Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoAlpha2Code.EdgeCases), MemberType = typeof(IsoCountryRulesTestData.IsIsoAlpha2Code))]
    public void IsIsoAlpha2Code_ReturnsFalse_ForInvalidCodes(IsoCountryRulesTestData.IsIsoAlpha2Code.Case testCase)
    {
        var result = IsoCountryRules.IsIsoAlpha2Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoAlpha3Code.ValidCases), MemberType = typeof(IsoCountryRulesTestData.IsIsoAlpha3Code))]
    public void IsIsoAlpha3Code_ReturnsTrue_ForValidCodes(IsoCountryRulesTestData.IsIsoAlpha3Code.Case testCase)
    {
        var result = IsoCountryRules.IsIsoAlpha3Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoAlpha3Code.EdgeCases), MemberType = typeof(IsoCountryRulesTestData.IsIsoAlpha3Code))]
    public void IsIsoAlpha3Code_ReturnsFalse_ForInvalidCodes(IsoCountryRulesTestData.IsIsoAlpha3Code.Case testCase)
    {
        var result = IsoCountryRules.IsIsoAlpha3Code(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoNumericCode.ValidCases), MemberType = typeof(IsoCountryRulesTestData.IsIsoNumericCode))]
    public void IsIsoNumericCode_ReturnsTrue_ForValidCodes(IsoCountryRulesTestData.IsIsoNumericCode.Case testCase)
    {
        var result = IsoCountryRules.IsIsoNumericCode(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoNumericCode.EdgeCases), MemberType = typeof(IsoCountryRulesTestData.IsIsoNumericCode))]
    public void IsIsoNumericCode_ReturnsFalse_ForInvalidCodes(IsoCountryRulesTestData.IsIsoNumericCode.Case testCase)
    {
        var result = IsoCountryRules.IsIsoNumericCode(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoCountryRulesTestData.IsIsoAlpha2CodeWithProvider.Cases), MemberType = typeof(IsoCountryRulesTestData.IsIsoAlpha2CodeWithProvider))]
    public void IsIsoAlpha2Code_UsesProvider(IsoCountryRulesTestData.IsIsoAlpha2CodeWithProvider.Case testCase)
    {
        var result = IsoCountryRules.IsIsoAlpha2Code(testCase.Value, testCase.Provider);

        Assert.Equal(testCase.Expected, result);
    }
}
