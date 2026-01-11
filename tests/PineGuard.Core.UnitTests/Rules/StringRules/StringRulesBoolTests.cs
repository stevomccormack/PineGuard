using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesBoolTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesBoolTestData.IsTrue.Cases), MemberType = typeof(StringRulesBoolTestData.IsTrue))]
    public void IsTrue_ReturnsExpected(StringRulesBoolTestData.IsTrue.Case @case)
    {
        var result = PineGuard.Rules.StringRules.Bool.IsTrue(@case.Value);

        Assert.Equal(@case.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesBoolTestData.IsFalse.Cases), MemberType = typeof(StringRulesBoolTestData.IsFalse))]
    public void IsFalse_ReturnsExpected(StringRulesBoolTestData.IsFalse.Case @case)
    {
        var result = PineGuard.Rules.StringRules.Bool.IsFalse(@case.Value);

        Assert.Equal(@case.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesBoolTestData.IsNullOrTrue.Cases), MemberType = typeof(StringRulesBoolTestData.IsNullOrTrue))]
    public void IsNullOrTrue_ReturnsExpected(StringRulesBoolTestData.IsNullOrTrue.Case @case)
    {
        var result = PineGuard.Rules.StringRules.Bool.IsNullOrTrue(@case.Value);

        Assert.Equal(@case.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesBoolTestData.IsNullOrFalse.Cases), MemberType = typeof(StringRulesBoolTestData.IsNullOrFalse))]
    public void IsNullOrFalse_ReturnsExpected(StringRulesBoolTestData.IsNullOrFalse.Case @case)
    {
        var result = PineGuard.Rules.StringRules.Bool.IsNullOrFalse(@case.Value);

        Assert.Equal(@case.ExpectedReturn, result);
    }
}
