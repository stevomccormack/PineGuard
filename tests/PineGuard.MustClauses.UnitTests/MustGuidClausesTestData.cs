using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustGuidClausesTestData
{
    public static class NotEmpty
    {
        public static TheoryData<MustCase<Guid>> ValidCases => F.NotEmpty.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<Guid>> InvalidCases => F.NotEmpty.InvalidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty));
    }

    public static class HasGuidVersion
    {
        public static TheoryData<MustCase<(Guid value, int version)>> ValidCases => F.HasVersion.AllValid.Project(v => (v.value!.Value, v.version)).ToMustCases();

        public static TheoryData<MustCase<(Guid value, int version)>> InvalidCases => F.HasVersion.AllInvalid.Except(nameof(F.HasVersion.NullValue)).Project(v => (v.value!.Value, v.version)).ToMustCases(s => s.Name switch
        {
            nameof(F.HasVersion.VersionBelowMin) or nameof(F.HasVersion.VersionAboveMax) or nameof(F.HasVersion.NegativeVersion) => new MustExpected(false, "version requires a value between 1 and 8.", "version", Code: MustCodes.Guid.Version.Mismatch),
            _ => new MustExpected(false, "value must have the specified GUID version.", "value", MustCodes.Guid.Version.Mismatch)
        });
    }
}
