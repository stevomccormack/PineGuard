using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.EmailRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentEmailExtensionsTestData
{
    public static class Email
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsEmail.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsEmail.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid email address.", Code: MustCodes.Email.Address.Invalid)
        });
    }

    public static class StrictEmail
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsStrictEmail.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsStrictEmail.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid strict email address.")
        });
    }

    public static class HasEmailAlias
    {
        public static TheoryData<FluentCase<string?>> Cases => F.HasAlias.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAlias.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must contain an email alias.")
        });
    }

    public static class NotHasEmailAlias
    {
        public static TheoryData<FluentCase<string?>> Cases => F.HasAlias.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasAlias.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(false, "Value must not contain an email alias."),
            _ => new FluentExpected(true)
        });
    }
}
