using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class FalseAttributeTestData
{
    public static class Validation
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("false", false, true),
            new("null", null, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("true", true, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(
                "wrong type",
                123,
                new ExpectedException(typeof(InvalidOperationException), null, "[FalseAttribute] can only be applied to properties of type Boolean"))
        ];

        public sealed record ValidCase(string Name, bool? Value, bool Expected)
            : ReturnCase<bool?, bool>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, object? Value, ExpectedException ExpectedException)
            : ThrowsCase<object?>(Name, Value, ExpectedException);
    }
}
