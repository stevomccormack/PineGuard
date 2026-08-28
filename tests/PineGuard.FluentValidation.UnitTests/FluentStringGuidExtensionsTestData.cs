using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringGuidExtensionsTestData
{
    public static class Guid
    {
        public static TheoryData<FluentCase<string?>> Cases => F.GuidIsGuid.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GuidIsGuid.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid GUID.", Code: MustCodes.Guid.Format.Invalid)
        });
    }

    public static class NotEmptyGuid
    {
        public static TheoryData<FluentCase<string?>> Cases => F.GuidIsNotEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.GuidIsNotEmpty.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be an empty GUID.")
        });
    }
}
