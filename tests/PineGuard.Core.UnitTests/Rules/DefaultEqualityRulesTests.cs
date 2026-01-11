using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DefaultEqualityRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsDefaultInt32.ValidCases), MemberType = typeof(DefaultEqualityRulesTestData.IsDefaultInt32))]
    public void IsDefault_ReturnsExpected_ForInt32(DefaultEqualityRulesTestData.IsDefaultInt32.Case testCase)
    {
        var result = DefaultEqualityRules.IsDefault(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsDefaultNullableInt32.ValidCases), MemberType = typeof(DefaultEqualityRulesTestData.IsDefaultNullableInt32))]
    public void IsDefault_ReturnsExpected_ForNullableInt32(DefaultEqualityRulesTestData.IsDefaultNullableInt32.Case testCase)
    {
        var result = DefaultEqualityRules.IsDefault(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsDefaultString.ValidCases), MemberType = typeof(DefaultEqualityRulesTestData.IsDefaultString))]
    public void IsDefault_ReturnsExpected_ForString(DefaultEqualityRulesTestData.IsDefaultString.Case testCase)
    {
        var result = DefaultEqualityRules.IsDefault(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsNullOrDefaultInt32.ValidCases), MemberType = typeof(DefaultEqualityRulesTestData.IsNullOrDefaultInt32))]
    public void IsNullOrDefault_ReturnsExpected_ForInt32(DefaultEqualityRulesTestData.IsNullOrDefaultInt32.Case testCase)
    {
        var result = DefaultEqualityRules.IsNullOrDefault(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsNullOrDefaultNullableInt32.ValidCases), MemberType = typeof(DefaultEqualityRulesTestData.IsNullOrDefaultNullableInt32))]
    public void IsNullOrDefault_ReturnsExpected_ForNullableInt32(DefaultEqualityRulesTestData.IsNullOrDefaultNullableInt32.Case testCase)
    {
        var result = DefaultEqualityRules.IsNullOrDefault(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsNullOrDefaultString.ValidCases), MemberType = typeof(DefaultEqualityRulesTestData.IsNullOrDefaultString))]
    public void IsNullOrDefault_ReturnsExpected_ForString(DefaultEqualityRulesTestData.IsNullOrDefaultString.Case testCase)
    {
        var result = DefaultEqualityRules.IsNullOrDefault(testCase.Value);
        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
