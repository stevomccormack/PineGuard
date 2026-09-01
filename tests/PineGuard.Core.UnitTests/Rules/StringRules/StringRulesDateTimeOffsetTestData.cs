using System.Globalization;
using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesDateTimeOffsetTestData
{
    // Now as the pinned clock reports it. The pre-existing fixtures sit far enough either side of the
    // present to answer the same way on any run, so they keep covering the system-clock path; these
    // cases add the boundary only an injected clock can reach — the instant that is exactly "now".
    private static readonly DateTimeOffset PinnedNow = FixedTimeProvider.Default.GetUtcNow();

    private static string Iso(DateTimeOffset value) => value.ToString("O", CultureInfo.InvariantCulture);

    public static class IsInPast
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateTimeOffsetIsInPast.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> PinnedClockCases =>
        [
            new RuleCase<string?>("ThisVeryInstantIsNotInPast", Iso(PinnedNow), new RuleExpected(false)),
            new RuleCase<string?>("AnHourAgoIsInPast", Iso(PinnedNow.AddHours(-1)), new RuleExpected(true)),
            new RuleCase<string?>("AnHourFromNowIsNotInPast", Iso(PinnedNow.AddHours(1)), new RuleExpected(false))
        ];
    }

    public static class IsInFuture
    {
        public static TheoryData<RuleCase<string?>> Cases => F.DateTimeOffsetIsInFuture.AllScenarios.ToRuleCases();

        public static TheoryData<RuleCase<string?>> PinnedClockCases =>
        [
            new RuleCase<string?>("ThisVeryInstantIsNotInFuture", Iso(PinnedNow), new RuleExpected(false)),
            new RuleCase<string?>("AnHourFromNowIsInFuture", Iso(PinnedNow.AddHours(1)), new RuleExpected(true)),
            new RuleCase<string?>("AnHourAgoIsNotInFuture", Iso(PinnedNow.AddHours(-1)), new RuleExpected(false))
        ];
    }

    public static class IsBetween
    {
        public static TheoryData<RuleCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> Cases => F.DateTimeOffsetIsBetween.AllScenarios.ToRuleCases();
    }
}
