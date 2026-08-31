using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.VersionRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardVersionClausesTestData
{
    // Guard.Against.NotSemVer — throws when value is NOT a SemVer 2.0.0 version (delegates to Must.Be.SemVer)
    public static class NotSemVer
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsSemVer.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsSemVer.InvalidScenarios.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Version.Semver.Invalid));
    }
}
