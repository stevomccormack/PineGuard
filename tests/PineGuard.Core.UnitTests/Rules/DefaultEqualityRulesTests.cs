using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DefaultEqualityRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsDefaultInt32.Cases), MemberType = typeof(DefaultEqualityRulesTestData.IsDefaultInt32))]
    public void IsDefault_Int32_BehavesAsExpected(RuleCase<int> tc)
    {
        // Act
        var result = DefaultEqualityRules.IsDefault(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsDefaultNullableInt32.Cases), MemberType = typeof(DefaultEqualityRulesTestData.IsDefaultNullableInt32))]
    public void IsDefault_NullableInt32_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = DefaultEqualityRules.IsDefault(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsDefaultString.Cases), MemberType = typeof(DefaultEqualityRulesTestData.IsDefaultString))]
    public void IsDefault_String_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = DefaultEqualityRules.IsDefault(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsNullOrDefaultInt32.Cases), MemberType = typeof(DefaultEqualityRulesTestData.IsNullOrDefaultInt32))]
    public void IsNullOrDefault_Int32_BehavesAsExpected(RuleCase<int> tc)
    {
        // Act
        var result = DefaultEqualityRules.IsNullOrDefault(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsNullOrDefaultNullableInt32.Cases), MemberType = typeof(DefaultEqualityRulesTestData.IsNullOrDefaultNullableInt32))]
    public void IsNullOrDefault_NullableInt32_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = DefaultEqualityRules.IsNullOrDefault(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityRulesTestData.IsNullOrDefaultString.Cases), MemberType = typeof(DefaultEqualityRulesTestData.IsNullOrDefaultString))]
    public void IsNullOrDefault_String_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = DefaultEqualityRules.IsNullOrDefault(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
