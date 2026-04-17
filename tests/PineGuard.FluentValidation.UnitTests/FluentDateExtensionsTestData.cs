using PineGuard.Testing.UnitTests;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDateExtensionsTestData
{
    public static class InSqlDateRange
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Valid", new DateOnly(2023, 1, 1), true, null)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Min fails", DateOnly.MinValue, false, "Value must be within the SQL date range.")
        ];

        public sealed record ValidCase(string Name, DateOnly Value, bool Expected, string? ExpectedMessage)
            : ReturnCase<DateOnly, bool>(Name, Value, Expected);
    }

    public static class Completed
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Completed", Task.CompletedTask, true, null)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Not completed fails", new Task(() => { }), false, "Value must be completed.")
        ];

        public sealed record ValidCase(string Name, Task Value, bool Expected, string? ExpectedMessage)
            : ReturnCase<Task, bool>(Name, Value, Expected);
    }

    public static class Default
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Default null", null, true, null)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Non-default int fails", 1, false, "Value must be the default value.")
        ];

        public sealed record ValidCase(string Name, int? Value, bool Expected, string? ExpectedMessage)
            : ReturnCase<int?, bool>(Name, Value, Expected);
    }
}
