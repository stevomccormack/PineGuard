using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class ThrowHelperTestData
{
    public static class ThrowIfNull
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("object instance", new object()),
            new("empty string", string.Empty),
            new("boxed zero", 0)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null argument", null, new ExpectedException(typeof(ArgumentNullException), "argument"))
        ];

        public sealed record ValidCase(string Name, object Value)
            : ValueCase<object>(Name, Value);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }

    public static class ThrowIfNullExplicitParamName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("non-null custom name", (new object(), "customValue"))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("null custom name", (null, "customValue"), new ExpectedException(typeof(ArgumentNullException), "customValue")),
            new("null alternate custom name", (null, "source"), new ExpectedException(typeof(ArgumentNullException), "source"))
        ];

        public sealed record ValidCase(string Name, (object Argument, string ParamName) Value)
            : ValueCase<(object Argument, string ParamName)>(Name, Value);

        public sealed record InvalidCase(string Name, (object? Argument, string ParamName) Value, ExpectedException ExpectedException)
            : ThrowsCase<(object? Argument, string ParamName)>(Name, Value, ExpectedException);
    }
}

