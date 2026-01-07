using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BoolRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsTrue.ValidCases), MemberType = typeof(BoolRulesTestData.IsTrue))]
    public void IsTrue_ReturnsTrue_ForTrue(BoolRulesTestData.IsTrue.Case testCase)
    {
        var result = BoolRules.IsTrue(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsTrue.EdgeCases), MemberType = typeof(BoolRulesTestData.IsTrue))]
    public void IsTrue_ReturnsFalse_ForNullOrFalse(BoolRulesTestData.IsTrue.Case testCase)
    {
        var result = BoolRules.IsTrue(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsFalse.ValidCases), MemberType = typeof(BoolRulesTestData.IsFalse))]
    public void IsFalse_ReturnsTrue_ForFalse(BoolRulesTestData.IsFalse.Case testCase)
    {
        var result = BoolRules.IsFalse(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsFalse.EdgeCases), MemberType = typeof(BoolRulesTestData.IsFalse))]
    public void IsFalse_ReturnsFalse_ForNullOrTrue(BoolRulesTestData.IsFalse.Case testCase)
    {
        var result = BoolRules.IsFalse(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsNullOrTrue.ValidCases), MemberType = typeof(BoolRulesTestData.IsNullOrTrue))]
    public void IsNullOrTrue_ReturnsTrue_ForNullOrTrue(BoolRulesTestData.IsNullOrTrue.Case testCase)
    {
        var result = BoolRules.IsNullOrTrue(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsNullOrTrue.EdgeCases), MemberType = typeof(BoolRulesTestData.IsNullOrTrue))]
    public void IsNullOrTrue_ReturnsFalse_ForFalse(BoolRulesTestData.IsNullOrTrue.Case testCase)
    {
        var result = BoolRules.IsNullOrTrue(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsNullOrFalse.ValidCases), MemberType = typeof(BoolRulesTestData.IsNullOrFalse))]
    public void IsNullOrFalse_ReturnsTrue_ForNullOrFalse(BoolRulesTestData.IsNullOrFalse.Case testCase)
    {
        var result = BoolRules.IsNullOrFalse(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.True(result);
    }

    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsNullOrFalse.EdgeCases), MemberType = typeof(BoolRulesTestData.IsNullOrFalse))]
    public void IsNullOrFalse_ReturnsFalse_ForTrue(BoolRulesTestData.IsNullOrFalse.Case testCase)
    {
        var result = BoolRules.IsNullOrFalse(testCase.Value);

        Assert.Equal(testCase.Expected, result);
        Assert.False(result);
    }
}
