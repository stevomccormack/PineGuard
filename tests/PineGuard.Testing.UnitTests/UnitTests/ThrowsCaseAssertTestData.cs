using PineGuard.Testing.Common;

namespace PineGuard.Testing.UnitTests.UnitTests;

public static class ThrowsCaseAssertTestData
{
    private sealed record ConcreteThrowsCase(string Name, string? Value, ExpectedException ExpectedException)
        : ThrowsCase<string?>(Name, Value, ExpectedException);

    private sealed record NullExpectedExceptionCase : IThrowsCase
    {
        ExpectedException IThrowsCase.ExpectedException => null!;
    }

    public static class Expected
    {
        public sealed record ValidCase(string Name, (Exception ex, IThrowsCase testCase) Value)
            : BaseCase(Name);

        public sealed record InvalidCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("type match, no paramName, no message", (ex: new ArgumentException("err", "param"), testCase: new ConcreteThrowsCase("c", null, new ExpectedException(typeof(ArgumentException))))),
            new("type match with correct paramName", (ex: new ArgumentNullException("value"), testCase: new ConcreteThrowsCase("c", null, new ExpectedException(typeof(ArgumentNullException), "value")))),
            new("type match with correct messageContains", (ex: new InvalidOperationException("the message here"), testCase: new ConcreteThrowsCase("c", null, new ExpectedException(typeof(InvalidOperationException), null, "message")))),
            new("type match with correct paramName and message", (ex: new ArgumentException("bad value provided", "value"), testCase: new ConcreteThrowsCase("c", null, new ExpectedException(typeof(ArgumentException), "value", "bad value"))))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("case-insensitive message match", (ex: new InvalidOperationException("The MESSAGE Here"), testCase: new ConcreteThrowsCase("c", null, new ExpectedException(typeof(InvalidOperationException), null, "message")))),
            new("empty messageContains matches any", (ex: new InvalidOperationException("anything"), testCase: new ConcreteThrowsCase("c", null, new ExpectedException(typeof(InvalidOperationException), null, ""))))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("null testCase throws ArgumentNullException", () => ThrowsCaseAssert.Expected(new Exception("e"), null!), new ExpectedException(typeof(ArgumentNullException), "testCase")),
            new InvalidCase("null ex throws ArgumentNullException", () => ThrowsCaseAssert.Expected(null!, new ConcreteThrowsCase("c", null, new ExpectedException(typeof(ArgumentException)))), new ExpectedException(typeof(ArgumentNullException), "ex")),
            new InvalidCase("null ExpectedException throws ArgumentNullException", () => ThrowsCaseAssert.Expected(new ArgumentException("e"), new NullExpectedExceptionCase()), new ExpectedException(typeof(ArgumentNullException), "expected")),
            new InvalidCase("type mismatch throws InvalidOperationException", () => ThrowsCaseAssert.Expected(new ArgumentException("e"), new ConcreteThrowsCase("c", null, new ExpectedException(typeof(InvalidOperationException)))), new ExpectedException(typeof(InvalidOperationException))),
            new InvalidCase("paramName expected but ex not ArgumentException", () => ThrowsCaseAssert.Expected(new InvalidOperationException("e"), new ConcreteThrowsCase("c", null, new ExpectedException(typeof(InvalidOperationException), "p"))), new ExpectedException(typeof(InvalidOperationException))),
            new InvalidCase("paramName mismatch throws InvalidOperationException", () => ThrowsCaseAssert.Expected(new ArgumentException("e", "wrong"), new ConcreteThrowsCase("c", null, new ExpectedException(typeof(ArgumentException), "expected"))), new ExpectedException(typeof(InvalidOperationException))),
            new InvalidCase("messageContains not found throws InvalidOperationException", () => ThrowsCaseAssert.Expected(new InvalidOperationException("hello"), new ConcreteThrowsCase("c", null, new ExpectedException(typeof(InvalidOperationException), null, "notfound"))), new ExpectedException(typeof(InvalidOperationException)))
        ];
    }
}
