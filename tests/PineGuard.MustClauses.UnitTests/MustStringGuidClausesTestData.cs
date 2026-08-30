using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringGuidClausesTestData
{
    public static class Guid
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.GuidIsGuid.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.GuidIsGuid.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.GuidIsGuid.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid GUID.", Code: MustCodes.Guid.Format.Invalid)
        });
    }

    public static class NotEmptyGuid
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.GuidIsNotEmpty.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.GuidIsNotEmpty.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.GuidIsNotEmpty.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty)
        });
    }

    public static class HasGuidVersion
    {
        public static TheoryData<MustCase<(string? value, int version)>> ValidCases => F.GuidHasVersion.AllValid.ToMustCases();

        public static TheoryData<MustCase<(string? value, int version)>> InvalidCases => F.GuidHasVersion.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.GuidHasVersion.VersionBelowMin) or nameof(F.GuidHasVersion.VersionAboveMax) => new MustExpected(false, "version requires a value between 1 and 8.", "version", MustCodes.Guid.Version.Mismatch),
            nameof(F.GuidHasVersion.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Guid.Version.Mismatch),
            _ => new MustExpected(false, "value must have the specified GUID version.", "value", MustCodes.Guid.Version.Mismatch)
        });
    }
}
