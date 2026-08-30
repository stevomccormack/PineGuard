using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringGuidAttributeTestData
{
    public static class GuidString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GuidIsGuid.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.GuidIsGuid.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid GUID.", Code: MustCodes.Guid.Format.Invalid)
        });
    }

    public static class HasGuidVersionString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.GuidHasVersion.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.GuidHasVersion.NullValue) => new DataAnnotationExpected(true),
            nameof(F.GuidHasVersion.VersionBelowMin) or nameof(F.GuidHasVersion.VersionAboveMax) =>
                new DataAnnotationExpected(false, "version requires a value between 1 and 8.", Code: MustCodes.Guid.Version.Mismatch),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have the specified GUID version.", Code: MustCodes.Guid.Version.Mismatch)
        });
    }
}
