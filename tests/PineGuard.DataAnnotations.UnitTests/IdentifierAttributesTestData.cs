using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.IdentifierRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class IdentifierAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", F.IsSlug.Null, true)
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

    public static class Ulid
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsUlid.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsUlid.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid ULID.", Code: MustCodes.Identifier.Ulid.Invalid)
        });
    }
}
