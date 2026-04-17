using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.UnitTests.UnitTests;

public static class ExpectedTestData
{
    public static class RuleExpectedOps
    {
        public sealed record Case(string Name, (bool isValid, bool expectedIsValid) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid", (true, true)),
            new("invalid", (false, false))
        ];
    }

    public static class MustExpectedOps
    {
        public sealed record Case(string Name, (bool isValid, string? message, string? paramName) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid no message", (true, null, null)),
            new("invalid with message", (false, "must be valid", null)),
            new("invalid with message and param", (false, "must be valid", "value"))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("valid with empty message", (true, string.Empty, null))
        ];
    }

    public static class GuardExpectedOps
    {
        public sealed record Case(string Name, (bool isValid, Type? exType, string? paramName, string? msgContains) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid", (true, null, null, null)),
            new("invalid ANE", (false, typeof(ArgumentNullException), "value", null)),
            new("invalid AE with msg", (false, typeof(ArgumentException), "value", "must be"))
        ];
    }

    public static class FluentExpectedOps
    {
        public sealed record Case(string Name, (bool isValid, string? message, string? propertyName) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid", (true, null, null)),
            new("invalid with message", (false, "must be valid", null)),
            new("invalid with property", (false, "must be valid", "Value"))
        ];
    }

    public static class DataAnnotationExpectedOps
    {
        public sealed record Case(string Name, (bool isValid, string? message, string? memberName) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid", (true, null, null)),
            new("invalid with message", (false, "must be valid", null)),
            new("invalid with member", (false, "must be valid", "Value"))
        ];
    }

    public static class HierarchyOps
    {
        public sealed record Case(string Name, (IExpectedResult expected, bool expectedIsValid) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("RuleExpected implements IExpectedResult", (new RuleExpected(true), true)),
            new("MustExpected implements IExpectedResult", (new MustExpected(false), false)),
            new("GuardExpected implements IExpectedResult", (new GuardExpected(true), true)),
            new("FluentExpected implements IExpectedResult", (new FluentExpected(false), false)),
            new("DataAnnotationExpected implements IExpectedResult", (new DataAnnotationExpected(true), true))
        ];
    }
}
