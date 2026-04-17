using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.DataAnnotations.UnitTests;

public static class IdentifierAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static TheoryData<ThrowsCase> TypeMismatchCases =>
    [
        new("wrong type", 123, new ExpectedException(typeof(InvalidOperationException), "expectedType"))
    ];

    public static class Slug
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "slug", true),
            new("with dash", "my-slug", true),
            new("numeric", "slug1", true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("uppercase", "Slug", false),
            new("space", "slug space", false),
            new("special char", "slug$", false),
            new("start dash", "-slug", false),
            new("end dash", "slug-", false)
        ];
    }
}
