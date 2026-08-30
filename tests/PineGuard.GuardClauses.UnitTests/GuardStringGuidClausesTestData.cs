using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringGuidClausesTestData
{
    public static class NotGuid
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.GuidIsGuid.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.GuidIsGuid.InvalidScenarios.ToGuardCases("value");
    }

    public static class EmptyGuid
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.GuidIsNotEmpty.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.GuidIsNotEmpty.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotHasGuidVersion — throws when value does NOT parse to that version (delegates to Must.Be.HasGuidVersion)
    public static class NotHasGuidVersion
    {
        public static TheoryData<GuardCase<(string? value, int version)>> ValidCases => F.GuidHasVersion.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<(string? value, int version)>> InvalidCases => F.GuidHasVersion.AllInvalid.ToGuardCases(s => s.Name switch
        {
            nameof(F.GuidHasVersion.VersionBelowMin) or nameof(F.GuidHasVersion.VersionAboveMax) => new GuardExpected(false, typeof(ArgumentException), "version", Code: MustCodes.Guid.Version.Mismatch),
            nameof(F.GuidHasVersion.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value", Code: MustCodes.Guid.Version.Mismatch),
            _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Guid.Version.Mismatch)
        });
    }
}
