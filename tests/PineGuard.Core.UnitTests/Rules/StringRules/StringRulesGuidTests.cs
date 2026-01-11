using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesGuidTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringRulesGuidTestData.IsEmpty.ValidCases), MemberType = typeof(StringRulesGuidTestData.IsEmpty))]
    [MemberData(nameof(StringRulesGuidTestData.IsEmpty.EdgeCases), MemberType = typeof(StringRulesGuidTestData.IsEmpty))]
    public void IsEmpty_ReturnsExpected(StringRulesGuidTestData.IsEmpty.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Guid.IsEmpty(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGuidTestData.IsNullOrEmpty.ValidCases), MemberType = typeof(StringRulesGuidTestData.IsNullOrEmpty))]
    [MemberData(nameof(StringRulesGuidTestData.IsNullOrEmpty.EdgeCases), MemberType = typeof(StringRulesGuidTestData.IsNullOrEmpty))]
    public void IsNullOrEmpty_ReturnsExpected(StringRulesGuidTestData.IsNullOrEmpty.Case testCase)
    {
        var result = PineGuard.Rules.StringRules.Guid.IsNullOrEmpty(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }
}
