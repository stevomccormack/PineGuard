using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class AttributeNullTestData
{
    private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
        : ThrowsCase<Action>(Name, Value, ExpectedException);

    public static class AttributesWithNullValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("all properties null", new AttributeNullTests.Model(), true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("all properties null (repeat)", new AttributeNullTests.Model(), true)
        ];

        public sealed record ValidCase(string Name, AttributeNullTests.Model Value, bool Expected)
            : ReturnCase<AttributeNullTests.Model, bool>(Name, Value, Expected);
    }

    public static class OddNumberUnsupportedType
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase(
                "string value",
                () => new OddNumberAttribute().GetValidationResult("not a number", new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException)))
        ];
    }

    public static class EvenNumberUnsupportedType
    {
        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new ActionThrowsCase(
                "string value",
                () => new EvenNumberAttribute().GetValidationResult("not a number", new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException)))
        ];
    }
}
