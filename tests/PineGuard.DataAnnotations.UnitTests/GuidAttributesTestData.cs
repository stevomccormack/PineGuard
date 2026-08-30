using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class GuidAttributesTestData
{
    public static class NotEmptyGuid
    {
        public static TheoryData<DataAnnotationCase> Cases
        {
            get
            {
                var td = F.NotEmpty.AllScenarios.ToDataAnnotationCases(s => s.IsValid
                    ? new DataAnnotationExpected(true)
                    : new DataAnnotationExpected(false, "Value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty));
                td.Add(new DataAnnotationCase("null-value", null, new DataAnnotationExpected(true)));
                return td;
            }
        }
    }

    public static class HasGuidVersion
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasVersion.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.HasVersion.NullValue) => new DataAnnotationExpected(true),
            nameof(F.HasVersion.VersionBelowMin) or nameof(F.HasVersion.VersionAboveMax) or nameof(F.HasVersion.NegativeVersion) =>
                new DataAnnotationExpected(false, "version requires a value between 1 and 8.", Code: MustCodes.Guid.Version.Mismatch),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have the specified GUID version.", Code: MustCodes.Guid.Version.Mismatch)
        });
    }
}
