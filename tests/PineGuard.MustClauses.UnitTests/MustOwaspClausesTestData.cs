using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.OwaspRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustOwaspClausesTestData
{
    public static class OwaspSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsOwaspSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsOwaspSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsOwaspSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class XssSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsXssSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsXssSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsXssSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class SqlInjectionSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsSqlInjectionSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsSqlInjectionSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsSqlInjectionSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class PathTraversalSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsPathTraversalSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsPathTraversalSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsPathTraversalSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class CommandInjectionSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsCommandInjectionSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsCommandInjectionSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsCommandInjectionSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class CrLfSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsCrLfSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsCrLfSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsCrLfSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class LdapFilterSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsLdapFilterSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsLdapFilterSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsLdapFilterSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }

    public static class OpenRedirectSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsOpenRedirectSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsOpenRedirectSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsOpenRedirectSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }
    public static class SsrfSchemeSafe
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsSsrfSchemeSafe.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsSsrfSchemeSafe.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsSsrfSchemeSafe.Null) => new MustExpected(false, ParamName: "value"),
            _ => new MustExpected(false)
        });
    }
}
