using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.Testing.UnitTests.UnitTests.MustClauses;

public static class BaseMustValidationUnitTestTestData
{
    public static class AssertResultOps
    {
        public sealed record Case(string Name, (MustValidationCase<string> testCase, MustValidationResult result) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid result, IsValid only",
                (new MustValidationCase<string>("c1", "x", new MustValidationExpected(true)),
                 MustValidationResult.Ok())),
            new("invalid result, FailureCount checked",
                (new MustValidationCase<string>("c2", "x", new MustValidationExpected(false, FailureCount: 2)),
                 MustValidationResult.Fail(new MustFailure("A", "a.invalid", "A invalid", null), new MustFailure("B", "b.invalid", "B invalid", null)))),
            new("invalid result, PropertyPath/Code/Message checked against Failures[0]",
                (new MustValidationCase<string>("c3", "x", new MustValidationExpected(false, "Email invalid", PropertyPath: "Email", Code: "email.address.invalid")),
                 MustValidationResult.Fail(new MustFailure("Email", "email.address.invalid", "Email invalid", null)))),
            new("valid result, empty Failures guard skips indexing",
                (new MustValidationCase<string>("c4", "x", new MustValidationExpected(true, PropertyPath: "Unreachable", Code: "unreachable")),
                 MustValidationResult.Ok()))
        ];
    }

    public static class Constructor
    {
        public sealed record Case(string Name) : BaseCase(Name);
        public static TheoryData<Case> ValidCases => [new("constructs without error")];
    }
}
