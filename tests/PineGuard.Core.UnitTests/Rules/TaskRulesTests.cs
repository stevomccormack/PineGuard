using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TaskRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TaskRulesTestData.IsCompleted.ValidCases), MemberType = typeof(TaskRulesTestData.IsCompleted))]
    [MemberData(nameof(TaskRulesTestData.IsCompleted.EdgeCases), MemberType = typeof(TaskRulesTestData.IsCompleted))]
    public void IsCompleted_ReturnsExpected(TaskRulesTestData.IsCompleted.Case testCase)
    {
        // Arrange
        var task = testCase.TaskFactory();

        // Act
        var result = TaskRules.IsCompleted(task);

        // Assert
        Assert.Equal(testCase.ExpectedIsCompleted, result);
        Assert.Equal(testCase.ExpectedIsCanceled, TaskRules.IsCanceled(task));
        Assert.Equal(testCase.ExpectedIsFaulted, TaskRules.IsFaulted(task));
    }
}
