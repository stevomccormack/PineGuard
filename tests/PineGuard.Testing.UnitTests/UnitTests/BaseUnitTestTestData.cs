using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.UnitTests;

public static class BaseUnitTestTestData
{
    public static class UseCulture
    {
        public sealed record ValidCase(string Name, string CultureName)
            : BaseCase(Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en-US", "en-US"),
            new("fr-FR", "fr-FR")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null throws", null, new ExpectedException(typeof(ArgumentException), "cultureName")),
            new InvalidCase("whitespace throws", "   ", new ExpectedException(typeof(ArgumentException), "cultureName")),
            new InvalidCase("empty throws", string.Empty, new ExpectedException(typeof(ArgumentException), "cultureName"))
        ];
    }

    public static class UseEnvironmentVariable
    {
        public sealed record ValidCase(string Name, (string key, string? value) Value)
            : BaseCase(Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("set non-null value", (key: "PINE_GUARD_TEST_VAR_A", value: "hello")),
            new("set null value (clears)", (key: "PINE_GUARD_TEST_VAR_B", value: null))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("whitespace value", (key: "PINE_GUARD_TEST_VAR_C", value: " "))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null key throws", null, new ExpectedException(typeof(ArgumentException), "key")),
            new InvalidCase("whitespace key throws", "  ", new ExpectedException(typeof(ArgumentException), "key")),
            new InvalidCase("empty key throws", string.Empty, new ExpectedException(typeof(ArgumentException), "key"))
        ];
    }

    public static class CreateDeterministicRandom
    {
        public sealed record Case(string Name, int Seed)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("default seed", 123456789),
            new("zero seed", 0),
            new("custom seed", 42)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("negative seed", -1),
            new("int.MaxValue seed", int.MaxValue),
            new("int.MinValue seed", int.MinValue)
        ];

    }

    public static class CreateCancelledToken
    {
        public sealed record Case(string Name) : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("token is already cancelled")
        ];

    }

    public static class Dispose
    {
        public sealed record Case(string Name, int DisposeCount)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("dispose once", 1)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("dispose twice is idempotent", 2)
        ];

    }

    public static class WriteLine
    {
        public sealed record Case(string Name, string? Message)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("regular message", "hello world")
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null message is silent", null),
            new("whitespace message is silent", "   "),
            new("empty message is silent", string.Empty)
        ];

    }

    public static class WriteLineWithOutput
    {
        public sealed record Case(string Name, string Message)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("writes message to output helper", "hello world")
        ];

    }

    public static class ScopeDispose
    {
        public sealed record Case(string Name)
            : BaseCase(Name);

        public static TheoryData<Case> EdgeCases =>
        [
            new("double dispose is idempotent")
        ];

    }

    public static class DisposeProtected
    {
        public sealed record Case(string Name, bool Disposing)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("disposing=true calls OnDispose", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("disposing=false skips OnDispose", false)
        ];

    }

    public static class OnDisposeBase
    {
        public sealed record Case(string Name)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("base OnDispose does not throw")
        ];

    }
}
