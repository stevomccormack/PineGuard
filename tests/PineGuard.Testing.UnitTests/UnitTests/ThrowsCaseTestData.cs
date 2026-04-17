using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.UnitTests;

public static class ThrowsCaseTestData
{
    public sealed record ConcreteThrowsCase(string Name, string? Value, ExpectedException ExpectedException)
        : ThrowsCase<string?>(Name, Value, ExpectedException);

    public sealed record ConcreteThrowsCaseTypeOnly : ThrowsCase<string?>
    {
        public ConcreteThrowsCaseTypeOnly(string name, string? value, Type exType)
            : base(name, value, exType) { }
    }

    public sealed record ConcreteThrowsCaseTypeParam : ThrowsCase<string?>
    {
        public ConcreteThrowsCaseTypeParam(string name, string? value, Type exType, string? paramName)
            : base(name, value, exType, paramName) { }
    }

    public sealed record ConcreteThrowsCaseTypeFull : ThrowsCase<string?>
    {
        public ConcreteThrowsCaseTypeFull(string name, string? value, Type exType, string? paramName, string? messageContains)
            : base(name, value, exType, paramName, messageContains) { }
    }

    public static class ConstructorWithExpectedException
    {
        public sealed record Case(string Name, (string? value, ExpectedException expectedException) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("type only", (value: "x", expectedException: new ExpectedException(typeof(ArgumentException)))),
            new("with paramName", (value: null, expectedException: new ExpectedException(typeof(ArgumentNullException), "value"))),
            new("all", (value: "", expectedException: new ExpectedException(typeof(ArgumentException), "value", "msg")))
        ];

    }

    public static class ConstructorTypeOnly
    {
        public sealed record Case(string Name, (string? value, Type exType) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("ArgumentException", (value: "x", exType: typeof(ArgumentException))),
            new("InvalidOperationException", (value: null, exType: typeof(InvalidOperationException)))
        ];

    }

    public static class ConstructorTypeAndParam
    {
        public sealed record Case(string Name, (string? value, Type exType, string? paramName) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("with paramName", (value: "x", exType: typeof(ArgumentException), paramName: "value")),
            new("null paramName", (value: null, exType: typeof(ArgumentNullException), paramName: null))
        ];

    }

    public static class ConstructorTypeFull
    {
        public sealed record Case(string Name, (string? value, Type exType, string? paramName, string? messageContains) Value)
            : BaseCase(Name);

        public static TheoryData<Case> ValidCases =>
        [
            new("all set", (value: "x", exType: typeof(ArgumentException), paramName: "v", messageContains: "msg")),
            new("nulls", (value: null, exType: typeof(Exception), paramName: null, messageContains: null))
        ];

    }
}
