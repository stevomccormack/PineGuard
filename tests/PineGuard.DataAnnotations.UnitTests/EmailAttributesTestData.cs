using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.EmailRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class EmailAttributesTestData
{
    public static class Email
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsEmail.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsEmail.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid email address.", Code: MustCodes.Email.Address.Invalid)
        });
    }

    public static class StrictEmail
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsStrictEmail.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsStrictEmail.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid strict email address.")
        });
    }

    public static class HasEmailAlias
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasAlias.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.HasAlias.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must contain an email alias.")
        });
    }

    public static class NotHasEmailAlias
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasAlias.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.HasAlias.Null) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(false, "Value must not contain an email alias."),
            _ => new DataAnnotationExpected(true)
        });
    }
}
