using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class ObjectRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsEqualTo.Cases), MemberType = typeof(ObjectRulesTestData.IsEqualTo))]
    public void IsEqualTo_BehavesAsExpected(RuleCase<(string? value, string? other)> tc)
    {
        // Act
        var result = ObjectRules.IsEqualTo(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsOfType.Cases), MemberType = typeof(ObjectRulesTestData.IsOfType))]
    public void IsOfType_BehavesAsExpected(RuleCase<object?> tc)
    {
        // Act
        var result = ObjectRules.IsOfType<string>(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsAssignableToType.Cases), MemberType = typeof(ObjectRulesTestData.IsAssignableToType))]
    public void IsAssignableToType_BehavesAsExpected(RuleCase<object?> tc)
    {
        // Act
        var result = ObjectRules.IsAssignableToType<string>(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsSameReferenceAs.Cases), MemberType = typeof(ObjectRulesTestData.IsSameReferenceAs))]
    public void IsSameReferenceAs_BehavesAsExpected(RuleCase<(object? a, object? b)> tc)
    {
        // Act
        var result = ObjectRules.IsSameReferenceAs(tc.Value.a, tc.Value.b);

        // Assert
        AssertResult(tc, result);
    }
}
