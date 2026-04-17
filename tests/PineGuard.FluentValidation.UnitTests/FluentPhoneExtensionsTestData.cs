using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.PhoneRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentPhoneExtensionsTestData
{
    public static class PhoneNumber
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsPhoneNumber.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid phone number.")
        });
    }

    public static class PhoneNumberString
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsPhoneNumber.AllScenarios.ToFluentCases(s => s.Name switch
        {
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid phone number.")
        });
    }
}
