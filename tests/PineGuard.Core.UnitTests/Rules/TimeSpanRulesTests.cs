using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeSpanRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TimeSpanRulesTestData.IsDurationBetween.Cases), MemberType = typeof(TimeSpanRulesTestData.IsDurationBetween))]
    public void IsDurationBetween_BehavesAsExpected(RuleCase<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Act
        var result = TimeSpanRules.IsDurationBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanRulesTestData.IsGreaterThan.Cases), MemberType = typeof(TimeSpanRulesTestData.IsGreaterThan))]
    public void IsGreaterThan_BehavesAsExpected(RuleCase<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)> tc)
    {
        // Act
        var result = TimeSpanRules.IsGreaterThan(tc.Value.value, tc.Value.threshold, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeSpanRulesTestData.IsLessThan.Cases), MemberType = typeof(TimeSpanRulesTestData.IsLessThan))]
    public void IsLessThan_BehavesAsExpected(RuleCase<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)> tc)
    {
        // Act
        var result = TimeSpanRules.IsLessThan(tc.Value.value, tc.Value.threshold, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
