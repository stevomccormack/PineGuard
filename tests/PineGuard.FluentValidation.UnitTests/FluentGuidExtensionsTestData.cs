using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.GuidRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentGuidExtensionsTestData
{
    public static class NotEmpty
    {
        public static TheoryData<FluentCase<Guid>> Cases => F.NotEmpty.AllScenarios.ToFluentCases(s => s.IsValid
            ? new FluentExpected(true)
            : new FluentExpected(false, "Value must not be an empty GUID.", Code: MustCodes.Guid.Emptiness.Empty));
    }

    public static class NotEmptyNullable
    {
        public static TheoryData<FluentCase<Guid?>> Cases => F.IsNotEmpty.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsNotEmpty.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be an empty GUID.")
        });
    }
}
