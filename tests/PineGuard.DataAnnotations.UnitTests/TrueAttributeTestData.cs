using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TrueAttributeTestData
{
    public static class Validation
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("true", true, true),
            new("null", null, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("false", false, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                "not a bool",
                new ExpectedException(typeof(InvalidOperationException), null, "[TrueAttribute] can only be applied to properties of type Boolean"))
        ];

        public sealed record ValidCase(string Name, bool? Value, bool Expected)
            : ReturnCase<bool?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }
}
