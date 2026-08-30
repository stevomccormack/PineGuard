using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.CronRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class CronAttributesTestData
{
    public static class CronExpression
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCronExpression.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsCronExpression.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid cron expression.", Code: MustCodes.Cron.Expression.Invalid)
        });
    }
}
