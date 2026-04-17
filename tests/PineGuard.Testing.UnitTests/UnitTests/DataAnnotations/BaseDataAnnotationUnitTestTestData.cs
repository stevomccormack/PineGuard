using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;

namespace PineGuard.Testing.UnitTests.UnitTests.DataAnnotations;

public static class BaseDataAnnotationUnitTestTestData
{
    public static class AssertReturnOps
    {
        public sealed record Case(string Name, (ReturnExpected expected, bool actualIsValid, string? actualMessage) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid with message", (new DataAnnotationExpected(true, "ok"), true, "ok")),
            new("valid without message", (new DataAnnotationExpected(true), true, null)),
            new("invalid with message", (new DataAnnotationExpected(false, "error"), false, "error")),
            new("invalid without message", (new DataAnnotationExpected(false), false, null))
        ];
    }

    public static class AssertResultOps
    {
        public sealed record Case(string Name, (DataAnnotationCase testCase, ValidationResult? result) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid result (Success)",
                (new DataAnnotationCase("c1", "x", new DataAnnotationExpected(true)),
                 ValidationResult.Success)),
            new("invalid result with message and memberName",
                (new DataAnnotationCase("c2", "x", new DataAnnotationExpected(false, "error", "Field")),
                 new ValidationResult("error", ["Field"]))),
            new("invalid result without memberName",
                (new DataAnnotationCase("c3", "x", new DataAnnotationExpected(false, "error")),
                 new ValidationResult("error")))
        ];
    }

    public static class Constructor
    {
        public sealed record Case(string Name) : BaseCase(Name);
        public static TheoryData<Case> ValidCases => [new("constructs without error")];
    }
}
