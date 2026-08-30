using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.VersionRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustVersionClausesTestData
{
    public static class SemVer
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsSemVer.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsSemVer.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsSemVer.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Version.Semver.Invalid),
            _ => new MustExpected(false, "value must be a valid semantic version.", Code: MustCodes.Version.Semver.Invalid)
        });
    }
}
