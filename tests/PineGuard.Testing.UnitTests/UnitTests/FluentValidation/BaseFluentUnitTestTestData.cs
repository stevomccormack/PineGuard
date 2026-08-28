using FluentValidation.Results;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.FluentValidation;

namespace PineGuard.Testing.UnitTests.UnitTests.FluentValidation;

public static class BaseFluentUnitTestTestData
{
    public static class AssertReturnOps
    {
        public sealed record Case(string Name, (ReturnExpected expected, bool actualIsValid, string? actualMessage) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid with message", (new FluentExpected(true, "ok"), true, "ok")),
            new("valid without message", (new FluentExpected(true), true, null)),
            new("invalid with message", (new FluentExpected(false, "error"), false, "error")),
            new("invalid without message", (new FluentExpected(false), false, null))
        ];
    }

    public static class AssertResultOps
    {
        public sealed record Case(string Name, (FluentCase<string> testCase, ValidationResult result) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid result without property name",
                (new FluentCase<string>("c1", "x", new FluentExpected(true)),
                 new ValidationResult())),
            new("invalid result with message and property name",
                (new FluentCase<string>("c2", "x", new FluentExpected(false, "error", "Prop")),
                 new ValidationResult([new ValidationFailure("Prop", "error")]))),
            new("invalid result without property name",
                (new FluentCase<string>("c3", "x", new FluentExpected(false, "error")),
                 new ValidationResult([new ValidationFailure("Prop", "error")]))),
            new("invalid result with code",
                (new FluentCase<string>("c4", "x", new FluentExpected(false, "error", "Prop", "test.code")),
                 new ValidationResult([new ValidationFailure("Prop", "error") { ErrorCode = "test.code" }])))
        ];
    }

    public static class Constructor
    {
        public sealed record Case(string Name) : BaseCase(Name);
        public static TheoryData<Case> ValidCases => [new("constructs without error")];
    }
}
