using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentMiscExtensionsTestData
{
    public static class InSqlDateRange
    {
        public static TheoryData<ValidCase> ValidCases => [new("Valid", new DateOnly(2023, 1, 1), true, null)];
        public static TheoryData<ValidCase> EdgeCases => [new("Min fails", DateOnly.MinValue, false, "Value must be within the SQL date range.")];
        public sealed record ValidCase(string Name, DateOnly Value, bool Expected, string? ExpectedMessage) : ReturnCase<DateOnly, bool>(Name, Value, Expected);
    }

    public static class Completed
    {
        public static TheoryData<ValidCase> ValidCases => [new("Completed", Task.CompletedTask, true, null)];
        // Task.FromCanceled/FromException are tricky in static context if they throw/not awaited, but we just check status.
        // We'll use a task that never completes for "Not Completed" case in test logic or just basic checks.
        // Actually, just checking CompletedTask vs a dummy non-started task?
        // Task.Delay(1000) is running.
        public static TheoryData<ValidCase> EdgeCases => [ 
            // We can't easily create a running task in static context that stays running reliably without async/await context issues in some runners, 
            // but Task.Delay(10000) should be 'Running' (not Completed) when validated synchronously immediately.
             new("Running fails", Task.Delay(10000), false, "Value must be completed.")
        ];
        public sealed record ValidCase(string Name, Task Value, bool Expected, string? ExpectedMessage) : ReturnCase<Task, bool>(Name, Value, Expected);
    }

    public static class Default
    {
        public static TheoryData<ValidCase> ValidCases => [new("Default null", null, true, null)];
        public static TheoryData<ValidCase> EdgeCases => [new("Non-default int", 1, false, "Value must be the default value.")];
        public sealed record ValidCase(string Name, int? Value, bool Expected, string? ExpectedMessage) : ReturnCase<int?, bool>(Name, Value, Expected);
    }
}
