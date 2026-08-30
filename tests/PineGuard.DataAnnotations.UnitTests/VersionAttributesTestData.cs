using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.VersionRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class VersionAttributesTestData
{
    public static class SemVer
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsSemVer.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsSemVer.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid semantic version.", Code: MustCodes.Version.Semver.Invalid)
        });
    }
}
