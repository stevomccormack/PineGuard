using PineGuard.Rules.Iso;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public sealed class IsoPaymentCardRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(IsoPaymentCardRulesTestData.DefaultSeparators.ValidCases), MemberType = typeof(IsoPaymentCardRulesTestData.DefaultSeparators))]
    public void IsIsoPaymentCard_ReturnsTrue_ForValidCardNumbers_WithDefaultSeparators(IsoPaymentCardRulesTestData.DefaultSeparators.Case testCase)
    {
        var result = IsoPaymentCardRules.IsIsoPaymentCard(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardRulesTestData.DefaultSeparators.EdgeCases), MemberType = typeof(IsoPaymentCardRulesTestData.DefaultSeparators))]
    public void IsIsoPaymentCard_ReturnsFalse_ForInvalidCardNumbers_WithDefaultSeparators(IsoPaymentCardRulesTestData.DefaultSeparators.Case testCase)
    {
        var result = IsoPaymentCardRules.IsIsoPaymentCard(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardRulesTestData.DigitsOnlySeparators.ValidCases), MemberType = typeof(IsoPaymentCardRulesTestData.DigitsOnlySeparators))]
    public void IsIsoPaymentCard_ReturnsTrue_WhenAllowedSeparatorsIsEmpty_AndDigitsOnlyValid(IsoPaymentCardRulesTestData.DigitsOnlySeparators.Case testCase)
    {
        var result = IsoPaymentCardRules.IsIsoPaymentCard(testCase.Value, allowedSeparators: []);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardRulesTestData.DigitsOnlySeparators.EdgeCases), MemberType = typeof(IsoPaymentCardRulesTestData.DigitsOnlySeparators))]
    public void IsIsoPaymentCard_ReturnsFalse_WhenAllowedSeparatorsIsEmpty_AndContainsSeparators(IsoPaymentCardRulesTestData.DigitsOnlySeparators.Case testCase)
    {
        var result = IsoPaymentCardRules.IsIsoPaymentCard(testCase.Value, allowedSeparators: []);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardRulesTestData.CustomAllowedSeparators.Cases), MemberType = typeof(IsoPaymentCardRulesTestData.CustomAllowedSeparators))]
    public void IsIsoPaymentCard_RespectsCustomAllowedSeparators(IsoPaymentCardRulesTestData.CustomAllowedSeparators.Case testCase)
    {
        var result = IsoPaymentCardRules.IsIsoPaymentCard(testCase.Value, testCase.AllowedSeparators);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Fact]
    public void IsIsoPaymentCard_Throws_WhenAllowedSeparatorsIsNull()
    {
        _ = Assert.Throws<ArgumentNullException>(() => IsoPaymentCardRules.IsIsoPaymentCard("4111111111111111", allowedSeparators: null!));
    }
}
