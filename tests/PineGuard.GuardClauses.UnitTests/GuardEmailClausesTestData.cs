using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.EmailRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardEmailClausesTestData
{
    public static class NotEmail
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsEmail.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsEmail.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsEmail.Null) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class NotStrictEmail
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsStrictEmail.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsStrictEmail.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsStrictEmail.Null) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class NotHasEmailAlias
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.HasAlias.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.HasAlias.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.HasAlias.Null) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
    }

    public static class HasEmailAlias
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.HasAlias.InvalidScenarios.Except(nameof(F.HasAlias.Null)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            new(nameof(F.HasAlias.WithAlias), F.HasAlias.WithAlias, new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.HasAlias.Null), F.HasAlias.Null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }
}
