using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── DateTimeOffset ──────────────────────────────────────────────

    public static class DateTimeOffsetIsInPast
    {
        public static readonly string? PastDate = "2000-01-01T00:00:00Z";
        public static readonly string? FutureDate = "2999-01-01T00:00:00Z";
        public static readonly string? NotADate = "not-a-date";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(PastDate), PastDate, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(FutureDate), FutureDate, false), new(nameof(NotADate), NotADate, false), new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateTimeOffsetIsInFuture
    {
        public static readonly string? FutureDate = "2999-01-01T00:00:00Z";
        public static readonly string? PastDate = "2000-01-01T00:00:00Z";
        public static readonly string? NotADate = "not-a-date";
        public static readonly string? NullValue = null;
        public static readonly string? Space = " ";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(FutureDate), FutureDate, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(PastDate), PastDate, false), new(nameof(NotADate), NotADate, false), new(nameof(NullValue), NullValue, false), new(nameof(Space), Space, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateTimeOffsetIsBetween
    {
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) InsideRange = ("2020-01-01T12:00:00Z", DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Inclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) MinExclusive = ("2020-01-01T00:00:00Z", DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Exclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) NotADate = ("not-a-date", DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Inclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) NullValue = (null, DateTimeOffset.Parse("2020-01-01T00:00:00Z"), DateTimeOffset.Parse("2020-01-02T00:00:00Z"), Inclusion.Inclusive);

        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(InsideRange), InsideRange, true)];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] ValidEdgeScenarios => [];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotADate), NotADate, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(MinExclusive), MinExclusive, false)];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class DateTimeOffsetIsNotBetween
    {
        private static readonly DateTimeOffset DtMin = DateTimeOffset.Parse("2020-01-01T00:00:00Z");
        private static readonly DateTimeOffset DtMax = DateTimeOffset.Parse("2020-01-02T00:00:00Z");

        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) OutsideRange = ("2020-01-03T00:00:00Z", DtMin, DtMax, Inclusion.Inclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) MinExclusive = ("2020-01-01T00:00:00Z", DtMin, DtMax, Inclusion.Exclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) InsideRange = ("2020-01-01T12:00:00Z", DtMin, DtMax, Inclusion.Inclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) NullValue = (null, DtMin, DtMax, Inclusion.Inclusive);
        public static readonly (string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) NotADate = ("not-a-date", DtMin, DtMax, Inclusion.Inclusive);

        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(OutsideRange), OutsideRange, true), new(nameof(MinExclusive), MinExclusive, true)];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(InsideRange), InsideRange, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateTimeOffsetIsWithin
    {
        private static readonly DateTimeOffset DtRef = DateTimeOffset.Parse("2020-01-01T12:00:00Z");

        public static readonly (string? value, DateTimeOffset? reference, TimeSpan window) SameInstant = ("2020-01-01T12:00:00Z", DtRef, TimeSpan.FromHours(1));
        public static readonly (string? value, DateTimeOffset? reference, TimeSpan window) WithinWindow = ("2020-01-01T12:30:00Z", DtRef, TimeSpan.FromHours(1));
        public static readonly (string? value, DateTimeOffset? reference, TimeSpan window) OutsideWindow = ("2020-01-01T14:00:00Z", DtRef, TimeSpan.FromHours(1));
        public static readonly (string? value, DateTimeOffset? reference, TimeSpan window) NullValue = (null, DtRef, TimeSpan.FromHours(1));
        public static readonly (string? value, DateTimeOffset? reference, TimeSpan window) NotADate = ("not-a-date", DtRef, TimeSpan.FromHours(1));

        public static RuleScenario<(string? value, DateTimeOffset? reference, TimeSpan window)>[] ValidScenarios => [new(nameof(SameInstant), SameInstant, true), new(nameof(WithinWindow), WithinWindow, true)];
        public static RuleScenario<(string? value, DateTimeOffset? reference, TimeSpan window)>[] InvalidScenarios => [new(nameof(OutsideWindow), OutsideWindow, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateTimeOffset? reference, TimeSpan window)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateTimeOffsetIsWithinCalendarMonths
    {
        private static readonly DateTimeOffset DtRef = DateTimeOffset.Parse("2020-01-15T00:00:00Z");

        public static readonly (string? value, DateTimeOffset? reference, int months) SameMonth = ("2020-01-20T00:00:00Z", DtRef, 1);
        public static readonly (string? value, DateTimeOffset? reference, int months) WithinWindow = ("2020-02-10T00:00:00Z", DtRef, 1);
        public static readonly (string? value, DateTimeOffset? reference, int months) OutsideWindow = ("2020-06-15T00:00:00Z", DtRef, 1);
        public static readonly (string? value, DateTimeOffset? reference, int months) NullValue = (null, DtRef, 1);
        public static readonly (string? value, DateTimeOffset? reference, int months) NotADate = ("not-a-date", DtRef, 1);

        public static RuleScenario<(string? value, DateTimeOffset? reference, int months)>[] ValidScenarios => [new(nameof(SameMonth), SameMonth, true), new(nameof(WithinWindow), WithinWindow, true)];
        public static RuleScenario<(string? value, DateTimeOffset? reference, int months)>[] InvalidScenarios => [new(nameof(OutsideWindow), OutsideWindow, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateTimeOffset? reference, int months)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
