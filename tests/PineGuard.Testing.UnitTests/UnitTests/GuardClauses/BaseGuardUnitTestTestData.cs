namespace PineGuard.Testing.UnitTests.UnitTests.GuardClauses;

public static class BaseGuardUnitTestTestData
{
    public static class AssertThrowValidOps
    {
        public sealed record Case(string Name) : BaseCase(Name);

        public static TheoryData<Case> ValidCases => [new("returns value when IsValid is true")];
    }

    public static class AssertThrowInvalidOps
    {
        public sealed record Case(string Name, (Type exceptionType, string? paramName, string? messageContains) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("ArgumentException with paramName and messageContains", (typeof(ArgumentException), (string?)"p", (string?)"bad")),
            new("ArgumentException with paramName only", (typeof(ArgumentException), (string?)"p", null)),
            new("non-ArgumentException with paramName set", (typeof(InvalidOperationException), (string?)"p", null)),
            new("exception with messageContains only", (typeof(InvalidOperationException), null, (string?)"bad"))
        ];
    }

    public static class AssertResultOps
    {
        public sealed record Case(string Name, (bool isValid, Type? exceptionType, string? paramName) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid delegates to AssertThrow", (true, null, null)),
            new("invalid delegates to AssertThrow", (false, (Type?)typeof(ArgumentException), (string?)"p"))
        ];
    }

    public static class AssertThrowCodeOps
    {
        public sealed record Case(string Name, (string code, string? paramName) Value) : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("code with paramName", ("test.code", (string?)"p")),
            new("code without paramName", ("test.code", null))
        ];
    }

    public static class AssertThrowNonGuardOps
    {
        public sealed record Case(string Name) : BaseCase(Name);
        public static TheoryData<Case> ValidCases => [new("non-GuardExpected skips the code branch entirely")];
    }

    public static class AssertCustomMessageOps
    {
        public sealed record Case(string Name, bool Value) : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("valid no-ops", true),
            new("invalid asserts custom message", false)
        ];
    }

    public static class Constructor
    {
        public sealed record Case(string Name) : BaseCase(Name);
        public static TheoryData<Case> ValidCases => [new("constructs without error")];
    }
}
