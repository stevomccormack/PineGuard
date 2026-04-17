using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.TaskRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TaskAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class TaskCompleted
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsCompleted.Completed), F.IsCompleted.Completed(), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsCompleted.InProgress), F.IsCompleted.InProgress(), false)];
    }

    public static class TaskNotCompleted
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsCompleted.InProgress), F.IsCompleted.InProgress(), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsCompleted.Completed), F.IsCompleted.Completed(), false)];
    }

    public static class TaskCanceled
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsCanceled.Canceled), F.IsCanceled.Canceled(), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsCanceled.Completed), F.IsCanceled.Completed(), false)];
    }

    public static class TaskNotCanceled
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsCanceled.Completed), F.IsCanceled.Completed(), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsCanceled.Canceled), F.IsCanceled.Canceled(), false)];
    }

    public static class TaskFaulted
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsFaulted.Faulted), F.IsFaulted.Faulted(), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsFaulted.Completed), F.IsFaulted.Completed(), false)];
    }

    public static class TaskNotFaulted
    {
        public static TheoryData<ValidCase> ValidCases => [new(nameof(F.IsFaulted.Completed), F.IsFaulted.Completed(), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new(nameof(F.IsFaulted.Faulted), F.IsFaulted.Faulted(), false)];
    }
}
