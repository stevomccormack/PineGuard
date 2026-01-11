using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class EmailRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(EmailRulesTestData.IsEmail.ValidCases), MemberType = typeof(EmailRulesTestData.IsEmail))]
    [MemberData(nameof(EmailRulesTestData.IsEmail.EdgeCases), MemberType = typeof(EmailRulesTestData.IsEmail))]
    public void IsEmail_ReturnsExpected(EmailRulesTestData.IsEmail.Case testCase)
    {
        var result = EmailRules.IsEmail(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(EmailRulesTestData.IsStrictEmail.ValidCases), MemberType = typeof(EmailRulesTestData.IsStrictEmail))]
    [MemberData(nameof(EmailRulesTestData.IsStrictEmail.EdgeCases), MemberType = typeof(EmailRulesTestData.IsStrictEmail))]
    public void IsStrictEmail_ReturnsExpected(EmailRulesTestData.IsStrictEmail.Case testCase)
    {
        var result = EmailRules.IsStrictEmail(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(EmailRulesTestData.HasAlias.ValidCases), MemberType = typeof(EmailRulesTestData.HasAlias))]
    [MemberData(nameof(EmailRulesTestData.HasAlias.EdgeCases), MemberType = typeof(EmailRulesTestData.HasAlias))]
    public void HasAlias_ReturnsExpected(EmailRulesTestData.HasAlias.Case testCase)
    {
        var result = EmailRules.HasAlias(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
