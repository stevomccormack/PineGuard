namespace PineGuard.Core.UnitTests.Rules;

public static class TaskRulesTestData
{
    private static Task CreateCompletedTask() => Task.CompletedTask;

    private static Task CreateCompletedResultTask() => Task.FromResult(123);

    private static Task CreateInProgressTask() => new TaskCompletionSource().Task;

    private static Task CreateCanceledTask() => Task.FromCanceled(new CancellationToken(canceled: true));

    private static Task CreateFaultedTask() => Task.FromException(new InvalidOperationException("boom"));

    private static Task? CreateNullTask() => null;

    public static class IsCompleted
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("completed", CreateCompletedTask, true, false, false),
            new("completed result", CreateCompletedResultTask, true, false, false)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("in-progress", CreateInProgressTask, false, false, false),
            new("null", CreateNullTask, false, false, false),
            new("canceled", CreateCanceledTask, true, true, false),
            new("faulted", CreateFaultedTask, true, false, true)
        ];

        #region Case Records

        public sealed record Case(
            string Name,
            Func<Task?> TaskFactory,
            bool ExpectedIsCompleted,
            bool ExpectedIsCanceled,
            bool ExpectedIsFaulted)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
