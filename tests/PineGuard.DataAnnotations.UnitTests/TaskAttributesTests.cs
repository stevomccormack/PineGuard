using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class TaskAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, TaskAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(TaskAttributesTestData.TaskCompleted.ValidCases), MemberType = typeof(TaskAttributesTestData.TaskCompleted))]
    [MemberData(nameof(TaskAttributesTestData.TaskCompleted.EdgeCases), MemberType = typeof(TaskAttributesTestData.TaskCompleted))]
    [MemberData(nameof(TaskAttributesTestData.TaskCompleted.InvalidCases), MemberType = typeof(TaskAttributesTestData.TaskCompleted))]
    public void TaskCompleted_ShouldReturnExpected(TaskAttributesTestData.ValidCase testCase)
        => Verify(new TaskCompletedAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TaskAttributesTestData.TaskNotCompleted.ValidCases), MemberType = typeof(TaskAttributesTestData.TaskNotCompleted))]
    [MemberData(nameof(TaskAttributesTestData.TaskNotCompleted.EdgeCases), MemberType = typeof(TaskAttributesTestData.TaskNotCompleted))]
    [MemberData(nameof(TaskAttributesTestData.TaskNotCompleted.InvalidCases), MemberType = typeof(TaskAttributesTestData.TaskNotCompleted))]
    public void TaskNotCompleted_ShouldReturnExpected(TaskAttributesTestData.ValidCase testCase)
        => Verify(new TaskNotCompletedAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TaskAttributesTestData.TaskCanceled.ValidCases), MemberType = typeof(TaskAttributesTestData.TaskCanceled))]
    [MemberData(nameof(TaskAttributesTestData.TaskCanceled.EdgeCases), MemberType = typeof(TaskAttributesTestData.TaskCanceled))]
    [MemberData(nameof(TaskAttributesTestData.TaskCanceled.InvalidCases), MemberType = typeof(TaskAttributesTestData.TaskCanceled))]
    public void TaskCanceled_ShouldReturnExpected(TaskAttributesTestData.ValidCase testCase)
        => Verify(new TaskCanceledAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TaskAttributesTestData.TaskNotCanceled.ValidCases), MemberType = typeof(TaskAttributesTestData.TaskNotCanceled))]
    [MemberData(nameof(TaskAttributesTestData.TaskNotCanceled.EdgeCases), MemberType = typeof(TaskAttributesTestData.TaskNotCanceled))]
    [MemberData(nameof(TaskAttributesTestData.TaskNotCanceled.InvalidCases), MemberType = typeof(TaskAttributesTestData.TaskNotCanceled))]
    public void TaskNotCanceled_ShouldReturnExpected(TaskAttributesTestData.ValidCase testCase)
        => Verify(new TaskNotCanceledAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TaskAttributesTestData.TaskFaulted.ValidCases), MemberType = typeof(TaskAttributesTestData.TaskFaulted))]
    [MemberData(nameof(TaskAttributesTestData.TaskFaulted.EdgeCases), MemberType = typeof(TaskAttributesTestData.TaskFaulted))]
    [MemberData(nameof(TaskAttributesTestData.TaskFaulted.InvalidCases), MemberType = typeof(TaskAttributesTestData.TaskFaulted))]
    public void TaskFaulted_ShouldReturnExpected(TaskAttributesTestData.ValidCase testCase)
        => Verify(new TaskFaultedAttribute(), testCase);

    [Theory]
    [MemberData(nameof(TaskAttributesTestData.TaskNotFaulted.ValidCases), MemberType = typeof(TaskAttributesTestData.TaskNotFaulted))]
    [MemberData(nameof(TaskAttributesTestData.TaskNotFaulted.EdgeCases), MemberType = typeof(TaskAttributesTestData.TaskNotFaulted))]
    [MemberData(nameof(TaskAttributesTestData.TaskNotFaulted.InvalidCases), MemberType = typeof(TaskAttributesTestData.TaskNotFaulted))]
    public void TaskNotFaulted_ShouldReturnExpected(TaskAttributesTestData.ValidCase testCase)
        => Verify(new TaskNotFaultedAttribute(), testCase);
}
