using PineGuard.Testing.UnitTests.GuardClauses;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardTaskClausesTestData
{
    private static Task GetCompletedTask() => Task.CompletedTask;
    private static Task GetInProgressTask() => new TaskCompletionSource().Task;
    private static Task GetCanceledTask() => Task.FromCanceled(new CancellationToken(canceled: true));
    private static Task GetFaultedTask() => Task.FromException(new InvalidOperationException("boom"));

    // Guard.Against.Completed — throws when task IS completed (NotCompleted must pass)
    public static class Completed
    {
        public static TheoryData<GuardCase<Func<Task>>> ValidCases =>
        [
            new("in-progress", GetInProgressTask, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<Func<Task>>> InvalidCases =>
        [
            new("completed", GetCompletedTask, new GuardExpected(false, typeof(ArgumentException), "task")),
            new("canceled", GetCanceledTask, new GuardExpected(false, typeof(ArgumentException), "task")),
            new("faulted", GetFaultedTask, new GuardExpected(false, typeof(ArgumentException), "task"))
        ];
    }

    // Guard.Against.NotCompleted — throws when task is NOT completed (Completed must pass)
    public static class NotCompleted
    {
        public static TheoryData<GuardCase<Func<Task>>> ValidCases =>
        [
            new("completed", GetCompletedTask, new GuardExpected(true)),
            new("canceled", GetCanceledTask, new GuardExpected(true)),
            new("faulted", GetFaultedTask, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<Func<Task>>> InvalidCases =>
        [
            new("in-progress", GetInProgressTask, new GuardExpected(false, typeof(ArgumentException), "task"))
        ];
    }

    // Guard.Against.Canceled — throws when task IS canceled (NotCanceled must pass)
    public static class Canceled
    {
        public static TheoryData<GuardCase<Func<Task>>> ValidCases =>
        [
            new("completed", GetCompletedTask, new GuardExpected(true)),
            new("in-progress", GetInProgressTask, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<Func<Task>>> InvalidCases =>
        [
            new("canceled", GetCanceledTask, new GuardExpected(false, typeof(ArgumentException), "task"))
        ];
    }

    // Guard.Against.NotCanceled — throws when task is NOT canceled (Canceled must pass)
    public static class NotCanceled
    {
        public static TheoryData<GuardCase<Func<Task>>> ValidCases =>
        [
            new("canceled", GetCanceledTask, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<Func<Task>>> InvalidCases =>
        [
            new("completed", GetCompletedTask, new GuardExpected(false, typeof(ArgumentException), "task")),
            new("in-progress", GetInProgressTask, new GuardExpected(false, typeof(ArgumentException), "task"))
        ];
    }

    // Guard.Against.Faulted — throws when task IS faulted (NotFaulted must pass)
    public static class Faulted
    {
        public static TheoryData<GuardCase<Func<Task>>> ValidCases =>
        [
            new("completed", GetCompletedTask, new GuardExpected(true)),
            new("in-progress", GetInProgressTask, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<Func<Task>>> InvalidCases =>
        [
            new("faulted", GetFaultedTask, new GuardExpected(false, typeof(ArgumentException), "task"))
        ];
    }

    // Guard.Against.NotFaulted — throws when task is NOT faulted (Faulted must pass)
    public static class NotFaulted
    {
        public static TheoryData<GuardCase<Func<Task>>> ValidCases =>
        [
            new("faulted", GetFaultedTask, new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<Func<Task>>> InvalidCases =>
        [
            new("completed", GetCompletedTask, new GuardExpected(false, typeof(ArgumentException), "task")),
            new("in-progress", GetInProgressTask, new GuardExpected(false, typeof(ArgumentException), "task"))
        ];
    }
}
