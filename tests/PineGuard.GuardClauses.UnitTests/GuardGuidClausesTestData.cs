using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardGuidClausesTestData
{
    public static class Empty
    {
        public static TheoryData<GuardCase<Guid>> ValidCases => F.NotEmpty.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<Guid>> InvalidCases => F.NotEmpty.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotHasGuidVersion — throws when value does NOT carry the version (delegates to Must.Be.HasGuidVersion)
    public static class NotHasGuidVersion
    {
        public static TheoryData<GuardCase<(Guid value, int version)>> ValidCases => F.HasVersion.AllValid.Project(v => (v.value!.Value, v.version)).ToGuardCases();

        public static TheoryData<GuardCase<(Guid value, int version)>> InvalidCases => F.HasVersion.AllInvalid.Except(nameof(F.HasVersion.NullValue)).Project(v => (v.value!.Value, v.version)).ToGuardCases(s => s.Name switch
        {
            nameof(F.HasVersion.VersionBelowMin) or nameof(F.HasVersion.VersionAboveMax) or nameof(F.HasVersion.NegativeVersion) => new GuardExpected(false, typeof(ArgumentException), "version", Code: MustCodes.Guid.Version.Mismatch),
            _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Guid.Version.Mismatch)
        });
    }
}
