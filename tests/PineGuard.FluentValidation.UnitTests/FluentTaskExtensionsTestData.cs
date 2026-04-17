using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentTaskExtensionsTestData
{
    private static Task GetCompletedTask() => Task.CompletedTask;
    private static Task GetInProgressTask() => new TaskCompletionSource().Task;
    private static Task GetCanceledTask() => Task.FromCanceled(new CancellationToken(canceled: true));
    private static Task GetFaultedTask() => Task.FromException(new InvalidOperationException("boom"));

    public static class Completed
    {
        public static TheoryData<FluentCase<Func<Task?>>> ValidCases =>
        [
            new("completed", GetCompletedTask, new FluentExpected(true)),
            new("canceled", GetCanceledTask, new FluentExpected(true)),
            new("faulted", GetFaultedTask, new FluentExpected(true))
        ];

        public static TheoryData<FluentCase<Func<Task?>>> InvalidCases =>
        [
            new("in-progress", GetInProgressTask, new FluentExpected(false, "Task must be completed.")),
            new("null", () => null, new FluentExpected(false, "Task must be completed."))
        ];
    }

    public static class NotCompleted
    {
        public static TheoryData<FluentCase<Func<Task?>>> ValidCases =>
        [
            new("in-progress", GetInProgressTask, new FluentExpected(true)),
            new("null", () => null, new FluentExpected(true))
        ];

        public static TheoryData<FluentCase<Func<Task?>>> InvalidCases =>
        [
            new("completed", GetCompletedTask, new FluentExpected(false, "Task must not be completed.")),
            new("canceled", GetCanceledTask, new FluentExpected(false, "Task must not be completed.")),
            new("faulted", GetFaultedTask, new FluentExpected(false, "Task must not be completed."))
        ];
    }

    public static class Canceled
    {
        public static TheoryData<FluentCase<Func<Task?>>> ValidCases =>
        [
            new("canceled", GetCanceledTask, new FluentExpected(true))
        ];

        public static TheoryData<FluentCase<Func<Task?>>> InvalidCases =>
        [
            new("completed", GetCompletedTask, new FluentExpected(false, "Task must be canceled.")),
            new("in-progress", GetInProgressTask, new FluentExpected(false, "Task must be canceled.")),
            new("null", () => null, new FluentExpected(false, "Task must be canceled."))
        ];
    }

    public static class NotCanceled
    {
        public static TheoryData<FluentCase<Func<Task?>>> ValidCases =>
        [
            new("completed", GetCompletedTask, new FluentExpected(true)),
            new("in-progress", GetInProgressTask, new FluentExpected(true)),
            new("null", () => null, new FluentExpected(true))
        ];

        public static TheoryData<FluentCase<Func<Task?>>> InvalidCases =>
        [
            new("canceled", GetCanceledTask, new FluentExpected(false, "Task must not be canceled."))
        ];
    }

    public static class Faulted
    {
        public static TheoryData<FluentCase<Func<Task?>>> ValidCases =>
        [
            new("faulted", GetFaultedTask, new FluentExpected(true))
        ];

        public static TheoryData<FluentCase<Func<Task?>>> InvalidCases =>
        [
            new("completed", GetCompletedTask, new FluentExpected(false, "Task must be faulted.")),
            new("in-progress", GetInProgressTask, new FluentExpected(false, "Task must be faulted.")),
            new("null", () => null, new FluentExpected(false, "Task must be faulted."))
        ];
    }

    public static class NotFaulted
    {
        public static TheoryData<FluentCase<Func<Task?>>> ValidCases =>
        [
            new("completed", GetCompletedTask, new FluentExpected(true)),
            new("in-progress", GetInProgressTask, new FluentExpected(true)),
            new("null", () => null, new FluentExpected(true))
        ];

        public static TheoryData<FluentCase<Func<Task?>>> InvalidCases =>
        [
            new("faulted", GetFaultedTask, new FluentExpected(false, "Task must not be faulted."))
        ];
    }
}
