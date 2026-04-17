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
}
