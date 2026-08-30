using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BufferRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BufferRulesTestData.IsHex.Cases), MemberType = typeof(BufferRulesTestData.IsHex))]
    public void IsHex_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BufferRules.IsHex(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BufferRulesTestData.IsBase64.Cases), MemberType = typeof(BufferRulesTestData.IsBase64))]
    public void IsBase64_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BufferRules.IsBase64(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BufferRulesTestData.IsBase64Url.Cases), MemberType = typeof(BufferRulesTestData.IsBase64Url))]
    public void IsBase64Url_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = BufferRules.IsBase64Url(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
