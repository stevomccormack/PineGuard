using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.CronRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentCronExtensionsTestData
{
    public static class CronExpression
    {
        public static TheoryData<FluentCase<(string? value, CronFormat format)>> Cases => F.IsCronExpression.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCronExpression.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid cron expression.", Code: MustCodes.Cron.Expression.Invalid)
        });
    }
}
