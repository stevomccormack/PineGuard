using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.CronRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustCronClausesTestData
{
    public static class CronExpression
    {
        public static TheoryData<MustCase<(string? value, CronFormat format)>> ValidCases => F.IsCronExpression.AllValid.ToMustCases();

        public static TheoryData<MustCase<(string? value, CronFormat format)>> InvalidCases => F.IsCronExpression.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.IsCronExpression.NullValue) => new MustExpected(false, "value must not be null.", "value", MustCodes.Cron.Expression.Invalid),
            _ => new MustExpected(false, "value must be a valid cron expression.", Code: MustCodes.Cron.Expression.Invalid)
        });
    }
}
