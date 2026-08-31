using System.Globalization;
using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesDateOnlyTestData
{
    // Today as the pinned clock reports it. The boundary cases used to be read off the machine clock,
    // which made "today" a different date on either side of midnight UTC; pinning the clock instead of
    // the dates keeps the same scenarios and removes the dependency on when the suite happens to run.
    private static readonly DateOnly PinnedToday = DateOnly.FromDateTime(FixedTimeProvider.Default.GetUtcNow().UtcDateTime);

    private static string Iso(DateOnly value) => value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    public static class IsInPast
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateOnlyIsInPast.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> PinnedClockCases =>
        [
            new RuleCase<string?>("TodayIsNotInPast", Iso(PinnedToday), new RuleExpected(false)),
            new RuleCase<string?>("YesterdayIsInPast", Iso(PinnedToday.AddDays(-1)), new RuleExpected(true))
        ];
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateOnlyIsInFuture.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> PinnedClockCases =>
        [
            new RuleCase<string?>("TodayIsNotInFuture", Iso(PinnedToday), new RuleExpected(false)),
            new RuleCase<string?>("TomorrowIsInFuture", Iso(PinnedToday.AddDays(1)), new RuleExpected(true))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>> Cases => F.DateOnlyIsBetween.AllScenarios.ToRuleCases();
    }
}
