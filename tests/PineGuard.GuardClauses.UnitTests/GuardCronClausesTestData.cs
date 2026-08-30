using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.CronRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardCronClausesTestData
{
    // Guard.Against.NotCronExpression — throws when value is NOT a cron expression in the given format (delegates to Must.Be.CronExpression)
    public static class NotCronExpression
    {
        public static TheoryData<GuardCase<(string? value, CronFormat format)>> ValidCases => F.IsCronExpression.AllValid.ToGuardCases();

        public static TheoryData<GuardCase<(string? value, CronFormat format)>> InvalidCases => F.IsCronExpression.AllInvalid.ToGuardCases(s => new GuardExpected(false, s.Inputs.value is null ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Cron.Expression.Invalid));
    }
}
