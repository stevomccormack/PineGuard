using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.OwaspRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardOwaspClausesTestData
{
    public static class OwaspUnsafe
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsOwaspSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsOwaspSafe.InvalidScenarios.ToGuardCases("value");
    }

    public static class Xss
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsXssSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsXssSafe.InvalidScenarios.Except(nameof(F.IsXssSafe.Null)).ToGuardCases("value");
    }

    public static class SqlInjection
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsSqlInjectionSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsSqlInjectionSafe.InvalidScenarios.Except(nameof(F.IsSqlInjectionSafe.Null)).ToGuardCases("value");
    }

    public static class PathTraversal
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsPathTraversalSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsPathTraversalSafe.InvalidScenarios.Except(nameof(F.IsPathTraversalSafe.Null)).ToGuardCases("value");
    }

    public static class CommandInjection
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsCommandInjectionSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsCommandInjectionSafe.InvalidScenarios.Except(nameof(F.IsCommandInjectionSafe.Null)).ToGuardCases("value");
    }

    public static class CrLf
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsCrLfSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsCrLfSafe.InvalidScenarios.Except(nameof(F.IsCrLfSafe.Null)).ToGuardCases("value");
    }

    public static class LdapFilter
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLdapFilterSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLdapFilterSafe.InvalidScenarios.Except(nameof(F.IsLdapFilterSafe.Null)).ToGuardCases("value");
    }

    public static class OpenRedirect
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsOpenRedirectSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsOpenRedirectSafe.InvalidScenarios.Except(nameof(F.IsOpenRedirectSafe.Null)).ToGuardCases("value");
    }
    public static class SsrfScheme
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsSsrfSchemeSafe.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsSsrfSchemeSafe.InvalidScenarios.Except(nameof(F.IsSsrfSchemeSafe.Null)).ToGuardCases("value");
    }
}
