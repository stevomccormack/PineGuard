namespace PineGuard.Testing.UnitTests.UnitTests;

public static class CaseRecordTestData
{
    public static class RuleCaseOps
    {
        public sealed record Case(string Name, (string? value, bool isValid) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid case", ("hello", true)),
            new("invalid case", ("bad", false)),
            new("null value case", (null, false))
        ];

    }

    public static class MustCaseOps
    {
        public sealed record Case(string Name, (string? value, bool isValid, string? message) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid case", ("hello", true, null)),
            new("invalid with message", ("bad", false, "must be valid"))
        ];

    }

    public static class GuardCaseOps
    {
        public sealed record Case(string Name, (string? value, bool isValid, Type? exType) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid case", ("hello", true, null)),
            new("invalid ANE", (null, false, typeof(ArgumentNullException)))
        ];

    }

    public static class FluentCaseOps
    {
        public sealed record Case(string Name, (string? value, bool isValid, string? message) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid case", ("hello", true, null)),
            new("invalid with message", ("bad", false, "must be valid"))
        ];

    }

    public static class DataAnnotationCaseOps
    {
        public sealed record Case(string Name, (object? value, bool isValid, string? message) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid string", ("hello", true, null)),
            new("valid int", (42, true, null)),
            new("invalid with message", ("bad", false, "must be valid")),
            new("null value", (null, false, "required"))
        ];

    }

    public static class MustValidationCaseOps
    {
        public sealed record Case(string Name, (string? value, bool isValid, int? failureCount) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid case", ("hello", true, null)),
            new("invalid with failure count", ("bad", false, 2))
        ];
    }
}
