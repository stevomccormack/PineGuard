using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TaskRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TaskRulesTestData.IsCompleted.Cases), MemberType = typeof(TaskRulesTestData.IsCompleted))]
    public void IsCompleted_BehavesAsExpected(RuleCase<Func<Task?>> tc)
    {
        // Arrange
        var task = tc.Value();

        // Act
        var result = TaskRules.IsCompleted(task);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TaskRulesTestData.IsCanceled.Cases), MemberType = typeof(TaskRulesTestData.IsCanceled))]
    public void IsCanceled_BehavesAsExpected(RuleCase<Func<Task?>> tc)
    {
        // Arrange
        var task = tc.Value();

        // Act
        var result = TaskRules.IsCanceled(task);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TaskRulesTestData.IsFaulted.Cases), MemberType = typeof(TaskRulesTestData.IsFaulted))]
    public void IsFaulted_BehavesAsExpected(RuleCase<Func<Task?>> tc)
    {
        // Arrange
        var task = tc.Value();

        // Act
        var result = TaskRules.IsFaulted(task);

        // Assert
        AssertResult(tc, result);
    }
}
