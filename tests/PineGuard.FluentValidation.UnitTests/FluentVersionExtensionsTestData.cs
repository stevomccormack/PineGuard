using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.VersionRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentVersionExtensionsTestData
{
    public static class SemVer
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsSemVer.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsSemVer.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid semantic version.", Code: MustCodes.Version.Semver.Invalid)
        });
    }
}
