using PineGuard.Rules.Iso;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public sealed class IsoLanguageRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoLanguageRulesTestData.IsIsoAlpha2Code.ValidCases), MemberType = typeof(IsoLanguageRulesTestData.IsIsoAlpha2Code))]
    public void IsIsoAlpha2Code_ReturnsTrue_ForValidCodes(IsoLanguageRulesTestData.IsIsoAlpha2Code.Case testCase)
    {
        var result = IsoLanguageRules.IsIsoAlpha2Code(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageRulesTestData.IsIsoAlpha2Code.EdgeCases), MemberType = typeof(IsoLanguageRulesTestData.IsIsoAlpha2Code))]
    public void IsIsoAlpha2Code_ReturnsFalse_ForInvalidCodes(IsoLanguageRulesTestData.IsIsoAlpha2Code.Case testCase)
    {
        var result = IsoLanguageRules.IsIsoAlpha2Code(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageRulesTestData.IsIsoAlpha3Code.ValidCases), MemberType = typeof(IsoLanguageRulesTestData.IsIsoAlpha3Code))]
    public void IsIsoAlpha3Code_ReturnsTrue_ForValidCodes(IsoLanguageRulesTestData.IsIsoAlpha3Code.Case testCase)
    {
        var result = IsoLanguageRules.IsIsoAlpha3Code(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageRulesTestData.IsIsoAlpha3Code.EdgeCases), MemberType = typeof(IsoLanguageRulesTestData.IsIsoAlpha3Code))]
    public void IsIsoAlpha3Code_ReturnsFalse_ForInvalidCodes(IsoLanguageRulesTestData.IsIsoAlpha3Code.Case testCase)
    {
        var result = IsoLanguageRules.IsIsoAlpha3Code(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoLanguageRulesTestData.IsIsoAlpha2CodeWithProvider.Cases), MemberType = typeof(IsoLanguageRulesTestData.IsIsoAlpha2CodeWithProvider))]
    public void IsIsoAlpha2Code_UsesProvider(IsoLanguageRulesTestData.IsIsoAlpha2CodeWithProvider.Case testCase)
    {
        var result = IsoLanguageRules.IsIsoAlpha2Code(testCase.Value, testCase.Provider);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
