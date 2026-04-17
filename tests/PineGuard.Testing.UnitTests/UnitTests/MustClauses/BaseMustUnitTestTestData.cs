using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.Testing.UnitTests.UnitTests.MustClauses;

public static class BaseMustUnitTestTestData
{
    public static class AssertReturnOps
    {
        public sealed record Case(string Name, (ReturnExpected expected, bool actualIsValid, string? actualMessage) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid with message", (new MustExpected(true, "ok"), true, "ok")),
            new("valid without message", (new MustExpected(true), true, null)),
            new("invalid with message", (new MustExpected(false, "error"), false, "error")),
            new("invalid without message", (new MustExpected(false), false, null))
        ];
    }

    public static class AssertResultOps
    {
        public sealed record Case(string Name, (MustCase<string> testCase, MustResult<bool> result) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid result with paramName",
                (new MustCase<string>("c1", "x", new MustExpected(true, null, "p")),
                 MustResult<bool>.Ok(true, "x", "p"))),
            new("valid result without paramName",
                (new MustCase<string>("c2", "x", new MustExpected(true)),
                 MustResult<bool>.Ok(true, "x"))),
            new("invalid result with paramName",
                (new MustCase<string>("c3", "x", new MustExpected(false, "error", "p")),
                 MustResult<bool>.Fail("error", "p", "x"))),
            new("invalid result without paramName",
                (new MustCase<string>("c4", "x", new MustExpected(false, "error")),
                 MustResult<bool>.Fail("error", null, "x")))
        ];
    }
}
