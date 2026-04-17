using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesCasingTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsCaseStyle.Cases), MemberType = typeof(StringRulesCasingTestData.IsCaseStyle))]
    public void IsCaseStyle_BehavesAsExpected(RuleCase<(string? value, StringCasing style)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsCaseStyle(tc.Value.value, tc.Value.style);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsCamelCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsCamelCase))]
    public void IsCamelCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsCamelCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsPascalCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsPascalCase))]
    public void IsPascalCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsPascalCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsSnakeCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsSnakeCase))]
    public void IsSnakeCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsSnakeCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsUpperSnakeCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsUpperSnakeCase))]
    public void IsUpperSnakeCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsUpperSnakeCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsKebabCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsKebabCase))]
    public void IsKebabCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsKebabCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsTrainCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsTrainCase))]
    public void IsTrainCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsTrainCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsDotCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsDotCase))]
    public void IsDotCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsDotCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsSpaceCase.Cases), MemberType = typeof(StringRulesCasingTestData.IsSpaceCase))]
    public void IsSpaceCase_BehavesAsExpected(RuleCase<string> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsSpaceCase(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsUpperInvariant.Cases), MemberType = typeof(StringRulesCasingTestData.IsUpperInvariant))]
    public void IsUpperInvariant_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsUpperInvariant(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesCasingTestData.IsLowerInvariant.Cases), MemberType = typeof(StringRulesCasingTestData.IsLowerInvariant))]
    public void IsLowerInvariant_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.IsLowerInvariant(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
