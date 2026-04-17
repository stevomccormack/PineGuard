using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── DateOnly ────────────────────────────────────────────────────

    public static class DateOnlyIsInPast
    {
        public static readonly string? PastDate = "2000-01-01";
        public static readonly string? FutureDate = "2999-01-01";
        public static readonly string? NotADate = "not-a-date";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(PastDate), PastDate, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(FutureDate), FutureDate, false), new(nameof(NotADate), NotADate, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsInFuture
    {
        public static readonly string? FutureDate = "2999-01-01";
        public static readonly string? PastDate = "2000-01-01";
        public static readonly string? NotADate = "not-a-date";
        public static readonly string? NullValue = null;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(FutureDate), FutureDate, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(PastDate), PastDate, false), new(nameof(NotADate), NotADate, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsBetween
    {
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) InsideRange = ("2020-01-15", new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 31), Inclusion.Inclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) OnMinInclusive = ("2020-01-01", new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 31), Inclusion.Inclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) OnMinExclusive = ("2020-01-01", new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 31), Inclusion.Exclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) NotADate = ("not-a-date", new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 31), Inclusion.Inclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) NullValue = (null, new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 31), Inclusion.Inclusive);

        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(InsideRange), InsideRange, true)];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] ValidEdgeScenarios => [new(nameof(OnMinInclusive), OnMinInclusive, true)];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(NotADate), NotADate, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] InvalidEdgeScenarios => [new(nameof(OnMinExclusive), OnMinExclusive, false)];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class DateOnlyIsNotBetween
    {
        private static readonly DateOnly D20200101 = new(2020, 1, 1);
        private static readonly DateOnly D20200131 = new(2020, 1, 31);

        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) OutsideRange = ("2020-02-15", D20200101, D20200131, Inclusion.Inclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) OnMinExclusive = ("2020-01-01", D20200101, D20200131, Inclusion.Exclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) InsideRange = ("2020-01-15", D20200101, D20200131, Inclusion.Inclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) NullValue = (null, D20200101, D20200131, Inclusion.Inclusive);
        public static readonly (string? value, DateOnly min, DateOnly max, Inclusion inclusion) NotADate = ("not-a-date", D20200101, D20200131, Inclusion.Inclusive);

        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] ValidScenarios => [new(nameof(OutsideRange), OutsideRange, true), new(nameof(OnMinExclusive), OnMinExclusive, true)];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(InsideRange), InsideRange, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsWithinDays
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly? reference, int days) SameDay = ("2020-01-15", D20200115, 7);
        public static readonly (string? value, DateOnly? reference, int days) WithinWindow = ("2020-01-20", D20200115, 7);
        public static readonly (string? value, DateOnly? reference, int days) OutsideWindow = ("2020-02-15", D20200115, 7);
        public static readonly (string? value, DateOnly? reference, int days) NullValue = (null, D20200115, 7);
        public static readonly (string? value, DateOnly? reference, int days) NotADate = ("not-a-date", D20200115, 7);

        public static RuleScenario<(string? value, DateOnly? reference, int days)>[] ValidScenarios => [new(nameof(SameDay), SameDay, true), new(nameof(WithinWindow), WithinWindow, true)];
        public static RuleScenario<(string? value, DateOnly? reference, int days)>[] InvalidScenarios => [new(nameof(OutsideWindow), OutsideWindow, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly? reference, int days)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsWithinCalendarMonths
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly? reference, int months) SameMonth = ("2020-01-20", D20200115, 1);
        public static readonly (string? value, DateOnly? reference, int months) WithinWindow = ("2020-02-10", D20200115, 1);
        public static readonly (string? value, DateOnly? reference, int months) OutsideWindow = ("2020-06-15", D20200115, 1);
        public static readonly (string? value, DateOnly? reference, int months) NullValue = (null, D20200115, 1);
        public static readonly (string? value, DateOnly? reference, int months) NotADate = ("not-a-date", D20200115, 1);

        public static RuleScenario<(string? value, DateOnly? reference, int months)>[] ValidScenarios => [new(nameof(SameMonth), SameMonth, true), new(nameof(WithinWindow), WithinWindow, true)];
        public static RuleScenario<(string? value, DateOnly? reference, int months)>[] InvalidScenarios => [new(nameof(OutsideWindow), OutsideWindow, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly? reference, int months)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsBefore
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly other) BeforeOther = ("2020-01-10", D20200115);
        public static readonly (string? value, DateOnly other) AfterOther = ("2020-01-20", D20200115);
        public static readonly (string? value, DateOnly other) SameDay = ("2020-01-15", D20200115);
        public static readonly (string? value, DateOnly other) NullValue = (null, D20200115);
        public static readonly (string? value, DateOnly other) NotADate = ("not-a-date", D20200115);

        public static RuleScenario<(string? value, DateOnly other)>[] ValidScenarios => [new(nameof(BeforeOther), BeforeOther, true)];
        public static RuleScenario<(string? value, DateOnly other)>[] InvalidScenarios => [new(nameof(AfterOther), AfterOther, false), new(nameof(SameDay), SameDay, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsOnOrBefore
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly other) BeforeOther = ("2020-01-10", D20200115);
        public static readonly (string? value, DateOnly other) SameDay = ("2020-01-15", D20200115);
        public static readonly (string? value, DateOnly other) AfterOther = ("2020-01-20", D20200115);
        public static readonly (string? value, DateOnly other) NullValue = (null, D20200115);
        public static readonly (string? value, DateOnly other) NotADate = ("not-a-date", D20200115);

        public static RuleScenario<(string? value, DateOnly other)>[] ValidScenarios => [new(nameof(BeforeOther), BeforeOther, true), new(nameof(SameDay), SameDay, true)];
        public static RuleScenario<(string? value, DateOnly other)>[] InvalidScenarios => [new(nameof(AfterOther), AfterOther, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsAfter
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly other) AfterOther = ("2020-01-20", D20200115);
        public static readonly (string? value, DateOnly other) BeforeOther = ("2020-01-10", D20200115);
        public static readonly (string? value, DateOnly other) SameDay = ("2020-01-15", D20200115);
        public static readonly (string? value, DateOnly other) NullValue = (null, D20200115);
        public static readonly (string? value, DateOnly other) NotADate = ("not-a-date", D20200115);

        public static RuleScenario<(string? value, DateOnly other)>[] ValidScenarios => [new(nameof(AfterOther), AfterOther, true)];
        public static RuleScenario<(string? value, DateOnly other)>[] InvalidScenarios => [new(nameof(BeforeOther), BeforeOther, false), new(nameof(SameDay), SameDay, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsOnOrAfter
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly other) AfterOther = ("2020-01-20", D20200115);
        public static readonly (string? value, DateOnly other) SameDay = ("2020-01-15", D20200115);
        public static readonly (string? value, DateOnly other) BeforeOther = ("2020-01-10", D20200115);
        public static readonly (string? value, DateOnly other) NullValue = (null, D20200115);
        public static readonly (string? value, DateOnly other) NotADate = ("not-a-date", D20200115);

        public static RuleScenario<(string? value, DateOnly other)>[] ValidScenarios => [new(nameof(AfterOther), AfterOther, true), new(nameof(SameDay), SameDay, true)];
        public static RuleScenario<(string? value, DateOnly other)>[] InvalidScenarios => [new(nameof(BeforeOther), BeforeOther, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsSame
    {
        private static readonly DateOnly D20200115 = new(2020, 1, 15);

        public static readonly (string? value, DateOnly other) SameDay = ("2020-01-15", D20200115);
        public static readonly (string? value, DateOnly other) DifferentDay = ("2020-01-20", D20200115);
        public static readonly (string? value, DateOnly other) NullValue = (null, D20200115);
        public static readonly (string? value, DateOnly other) NotADate = ("not-a-date", D20200115);

        public static RuleScenario<(string? value, DateOnly other)>[] ValidScenarios => [new(nameof(SameDay), SameDay, true)];
        public static RuleScenario<(string? value, DateOnly other)>[] InvalidScenarios => [new(nameof(DifferentDay), DifferentDay, false), new(nameof(NullValue), NullValue, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? value, DateOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsChronological
    {
        public static readonly (string? start, string? end, Inclusion inclusion) BeforeExclusive = ("2020-01-10", "2020-01-20", Inclusion.Exclusive);
        public static readonly (string? start, string? end, Inclusion inclusion) EqualInclusive = ("2020-01-15", "2020-01-15", Inclusion.Inclusive);
        public static readonly (string? start, string? end, Inclusion inclusion) Reversed = ("2020-01-20", "2020-01-10", Inclusion.Exclusive);
        public static readonly (string? start, string? end, Inclusion inclusion) EqualExclusive = ("2020-01-15", "2020-01-15", Inclusion.Exclusive);
        public static readonly (string? start, string? end, Inclusion inclusion) NullStart = (null, "2020-01-20", Inclusion.Exclusive);
        public static readonly (string? start, string? end, Inclusion inclusion) NotADate = ("not-a-date", "2020-01-20", Inclusion.Exclusive);

        public static RuleScenario<(string? start, string? end, Inclusion inclusion)>[] ValidScenarios => [new(nameof(BeforeExclusive), BeforeExclusive, true), new(nameof(EqualInclusive), EqualInclusive, true)];
        public static RuleScenario<(string? start, string? end, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(Reversed), Reversed, false), new(nameof(EqualExclusive), EqualExclusive, false), new(nameof(NullStart), NullStart, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? start, string? end, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class DateOnlyIsOverlapping
    {
        public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) OverlapExclusive = ("2020-01-01", "2020-01-20", "2020-01-10", "2020-01-31", Inclusion.Exclusive);
        public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) TouchingInclusive = ("2020-01-01", "2020-01-15", "2020-01-15", "2020-01-31", Inclusion.Inclusive);
        public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) Disjoint = ("2020-01-01", "2020-01-10", "2020-01-15", "2020-01-31", Inclusion.Exclusive);
        public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) NullStart1 = (null, "2020-01-20", "2020-01-10", "2020-01-31", Inclusion.Exclusive);
        public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) NotADate = ("not-a-date", "2020-01-20", "2020-01-10", "2020-01-31", Inclusion.Exclusive);

        public static RuleScenario<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>[] ValidScenarios => [new(nameof(OverlapExclusive), OverlapExclusive, true), new(nameof(TouchingInclusive), TouchingInclusive, true)];
        public static RuleScenario<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>[] InvalidScenarios => [new(nameof(Disjoint), Disjoint, false), new(nameof(NullStart1), NullStart1, false), new(nameof(NotADate), NotADate, false)];
        public static RuleScenario<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
