using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.EmailRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustEmailClausesTestData
{
    public static class Email
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsEmail.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsEmail.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsEmail.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid email address.")
        });
    }

    public static class StrictEmail
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsStrictEmail.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsStrictEmail.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsStrictEmail.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid strict email address.")
        });
    }

    public static class HasEmailAlias
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.HasAlias.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.HasAlias.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.HasAlias.Null) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must contain an email alias.")
        });
    }

    public static class NotHasEmailAlias
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.HasAlias.InvalidScenarios.Except(nameof(F.HasAlias.Null)).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<string?>> InvalidCases =>
        [
            new(nameof(F.HasAlias.WithAlias), F.HasAlias.WithAlias, new MustExpected(false, "value must not contain an email alias.")),
            new(nameof(F.HasAlias.Null), F.HasAlias.Null, new MustExpected(false, "value must not be null.", "value"))
        ];
    }
}
