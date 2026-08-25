using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    public static class StringTimeOnly
    {
        private static readonly TimeOnly T1000 = new(10, 0);
        private static readonly TimeOnly T1100 = new(11, 0);
        private static readonly TimeOnly T1200 = new(12, 0);
        private static readonly TimeOnly T1300 = new(13, 0);

        public static class IsBetween
        {
            public static readonly (string? value, TimeOnly min, TimeOnly max, Inclusion inclusion) InRangeInclusive = ("11:00", T1000, T1200, Inclusion.Inclusive);
            public static readonly (string? value, TimeOnly min, TimeOnly max, Inclusion inclusion) InRangeExclusive = ("11:00", T1000, T1200, Inclusion.Exclusive);
            public static readonly (string? value, TimeOnly min, TimeOnly max, Inclusion inclusion) NullValue = (null, T1000, T1200, Inclusion.Inclusive);
            public static readonly (string? value, TimeOnly min, TimeOnly max, Inclusion inclusion) Unparseable = ("invalid", T1000, T1200, Inclusion.Inclusive);
            public static readonly (string? value, TimeOnly min, TimeOnly max, Inclusion inclusion) OutOfRange = ("09:00", T1000, T1200, Inclusion.Inclusive);

            public static RuleScenario<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>[] ValidScenarios =>
            [
                new(nameof(InRangeInclusive), InRangeInclusive, true),
                new(nameof(InRangeExclusive), InRangeExclusive, true)
            ];

            public static RuleScenario<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>[] InvalidScenarios =>
            [
                new(nameof(NullValue), NullValue, false),
                new(nameof(Unparseable), Unparseable, false),
                new(nameof(OutOfRange), OutOfRange, false)
            ];

            public static RuleScenario<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsBefore
        {
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) BeforeExclusive = ("11:00", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) BeforeHigherOther = ("10:00", T1300, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AtBoundaryInclusive = ("12:00", T1200, Inclusion.Inclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AfterOther = ("13:00", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AfterLowerOther = ("12:00", T1100, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AtBoundaryExclusive = ("12:00", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) NullValue = (null, T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) Unparseable = ("invalid", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) InvalidPrecision = ("11:00", T1200, Inclusion.Exclusive, (TimePrecision)999);

            public static RuleScenario<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>[] ValidScenarios =>
            [
                new(nameof(BeforeExclusive), BeforeExclusive, true),
                new(nameof(BeforeHigherOther), BeforeHigherOther, true),
                new(nameof(AtBoundaryInclusive), AtBoundaryInclusive, true)
            ];

            public static RuleScenario<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>[] InvalidScenarios =>
            [
                new(nameof(AfterOther), AfterOther, false),
                new(nameof(AfterLowerOther), AfterLowerOther, false),
                new(nameof(AtBoundaryExclusive), AtBoundaryExclusive, false),
                new(nameof(NullValue), NullValue, false),
                new(nameof(Unparseable), Unparseable, false),
                new(nameof(InvalidPrecision), InvalidPrecision, false)
            ];

            public static RuleScenario<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsBeforeDefaultInclusion
        {
            public static readonly (string? value, TimeOnly other) StrictlyBefore = ("10:00", T1200);
            public static readonly (string? value, TimeOnly other) SameInstant = ("12:00", T1200);

            public static RuleScenario<(string? value, TimeOnly other)>[] ValidScenarios =>
            [
                new(nameof(StrictlyBefore), StrictlyBefore, true)
            ];

            public static RuleScenario<(string? value, TimeOnly other)>[] InvalidScenarios =>
            [
                new(nameof(SameInstant), SameInstant, false)
            ];

            public static RuleScenario<(string? value, TimeOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsAfter
        {
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AfterExclusive = ("13:00", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AtBoundaryInclusive = ("12:00", T1200, Inclusion.Inclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) BeforeOther = ("11:00", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) AtBoundaryExclusive = ("12:00", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) NullValue = (null, T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) Unparseable = ("invalid", T1200, Inclusion.Exclusive, null);
            public static readonly (string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision) InvalidPrecision = ("13:00", T1200, Inclusion.Exclusive, (TimePrecision)999);

            public static RuleScenario<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>[] ValidScenarios =>
            [
                new(nameof(AfterExclusive), AfterExclusive, true),
                new(nameof(AtBoundaryInclusive), AtBoundaryInclusive, true)
            ];

            public static RuleScenario<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>[] InvalidScenarios =>
            [
                new(nameof(BeforeOther), BeforeOther, false),
                new(nameof(AtBoundaryExclusive), AtBoundaryExclusive, false),
                new(nameof(NullValue), NullValue, false),
                new(nameof(Unparseable), Unparseable, false),
                new(nameof(InvalidPrecision), InvalidPrecision, false)
            ];

            public static RuleScenario<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsAfterDefaultInclusion
        {
            public static readonly (string? value, TimeOnly other) StrictlyAfter = ("13:00", T1200);
            public static readonly (string? value, TimeOnly other) SameInstant = ("12:00", T1200);

            public static RuleScenario<(string? value, TimeOnly other)>[] ValidScenarios =>
            [
                new(nameof(StrictlyAfter), StrictlyAfter, true)
            ];

            public static RuleScenario<(string? value, TimeOnly other)>[] InvalidScenarios =>
            [
                new(nameof(SameInstant), SameInstant, false)
            ];

            public static RuleScenario<(string? value, TimeOnly other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsSame
        {
            public static readonly (string? value, TimeOnly other, TimePrecision? precision) ExactMatch = ("12:00", T1200, null);
            public static readonly (string? value, TimeOnly other, TimePrecision? precision) SameHour = ("12:30", T1200, TimePrecision.Hour);
            public static readonly (string? value, TimeOnly other, TimePrecision? precision) Different = ("11:00", T1200, null);
            public static readonly (string? value, TimeOnly other, TimePrecision? precision) NullValue = (null, T1200, null);
            public static readonly (string? value, TimeOnly other, TimePrecision? precision) Unparseable = ("invalid", T1200, null);
            public static readonly (string? value, TimeOnly other, TimePrecision? precision) InvalidPrecision = ("12:00", T1200, (TimePrecision)999);

            public static RuleScenario<(string? value, TimeOnly other, TimePrecision? precision)>[] ValidScenarios =>
            [
                new(nameof(ExactMatch), ExactMatch, true),
                new(nameof(SameHour), SameHour, true)
            ];

            public static RuleScenario<(string? value, TimeOnly other, TimePrecision? precision)>[] InvalidScenarios =>
            [
                new(nameof(Different), Different, false),
                new(nameof(NullValue), NullValue, false),
                new(nameof(Unparseable), Unparseable, false),
                new(nameof(InvalidPrecision), InvalidPrecision, false)
            ];

            public static RuleScenario<(string? value, TimeOnly other, TimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsWithin
        {
            public static readonly (string? value, string? reference, TimeSpan window) SameInstant = ("12:00", "12:00", TimeSpan.FromMinutes(30));
            public static readonly (string? value, string? reference, TimeSpan window) WithinWindow = ("12:15", "12:00", TimeSpan.FromMinutes(30));
            public static readonly (string? value, string? reference, TimeSpan window) OutsideWindow = ("13:00", "12:00", TimeSpan.FromMinutes(30));
            public static readonly (string? value, string? reference, TimeSpan window) NullValue = (null, "12:00", TimeSpan.FromMinutes(30));
            public static readonly (string? value, string? reference, TimeSpan window) NullReference = ("12:00", null, TimeSpan.FromMinutes(30));
            public static readonly (string? value, string? reference, TimeSpan window) Unparseable = ("invalid", "12:00", TimeSpan.FromMinutes(30));
            public static readonly (string? value, string? reference, TimeSpan window) UnparseableReference = ("12:00", "invalid", TimeSpan.FromMinutes(30));

            public static RuleScenario<(string? value, string? reference, TimeSpan window)>[] ValidScenarios =>
            [
                new(nameof(SameInstant), SameInstant, true),
                new(nameof(WithinWindow), WithinWindow, true)
            ];

            public static RuleScenario<(string? value, string? reference, TimeSpan window)>[] InvalidScenarios =>
            [
                new(nameof(OutsideWindow), OutsideWindow, false),
                new(nameof(NullValue), NullValue, false),
                new(nameof(NullReference), NullReference, false),
                new(nameof(Unparseable), Unparseable, false),
                new(nameof(UnparseableReference), UnparseableReference, false)
            ];

            public static RuleScenario<(string? value, string? reference, TimeSpan window)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsChronological
        {
            public static readonly (string? start, string? end, Inclusion inclusion) BeforeExclusive = ("10:00", "12:00", Inclusion.Exclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) EqualInclusive = ("12:00", "12:00", Inclusion.Inclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) Reversed = ("13:00", "12:00", Inclusion.Exclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) EqualExclusive = ("12:00", "12:00", Inclusion.Exclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) NullStart = (null, "12:00", Inclusion.Exclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) NullEnd = ("12:00", null, Inclusion.Exclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) UnparseableStart = ("invalid", "12:00", Inclusion.Exclusive);
            public static readonly (string? start, string? end, Inclusion inclusion) UnparseableEnd = ("10:00", "invalid", Inclusion.Exclusive);

            public static RuleScenario<(string? start, string? end, Inclusion inclusion)>[] ValidScenarios =>
            [
                new(nameof(BeforeExclusive), BeforeExclusive, true),
                new(nameof(EqualInclusive), EqualInclusive, true)
            ];

            public static RuleScenario<(string? start, string? end, Inclusion inclusion)>[] InvalidScenarios =>
            [
                new(nameof(Reversed), Reversed, false),
                new(nameof(EqualExclusive), EqualExclusive, false),
                new(nameof(NullStart), NullStart, false),
                new(nameof(NullEnd), NullEnd, false),
                new(nameof(UnparseableStart), UnparseableStart, false),
                new(nameof(UnparseableEnd), UnparseableEnd, false)
            ];

            public static RuleScenario<(string? start, string? end, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }

        public static class IsOverlapping
        {
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) OverlapExclusive = ("10:00", "12:00", "11:00", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) TouchingInclusive = ("10:00", "12:00", "12:00", "14:00", Inclusion.Inclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) Disjoint = ("10:00", "11:00", "12:00", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) NullStart1 = (null, "11:00", "12:00", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) NullEnd1 = ("10:00", null, "12:00", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) NullStart2 = ("10:00", "11:00", null, "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) NullEnd2 = ("10:00", "11:00", "12:00", null, Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) UnparseableStart1 = ("invalid", "12:00", "11:00", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) UnparseableEnd1 = ("10:00", "invalid", "11:00", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) UnparseableStart2 = ("10:00", "12:00", "invalid", "13:00", Inclusion.Exclusive);
            public static readonly (string? start1, string? end1, string? start2, string? end2, Inclusion inclusion) UnparseableEnd2 = ("10:00", "12:00", "11:00", "invalid", Inclusion.Exclusive);

            public static RuleScenario<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>[] ValidScenarios =>
            [
                new(nameof(OverlapExclusive), OverlapExclusive, true),
                new(nameof(TouchingInclusive), TouchingInclusive, true)
            ];

            public static RuleScenario<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>[] InvalidScenarios =>
            [
                new(nameof(Disjoint), Disjoint, false),
                new(nameof(NullStart1), NullStart1, false),
                new(nameof(NullEnd1), NullEnd1, false),
                new(nameof(NullStart2), NullStart2, false),
                new(nameof(NullEnd2), NullEnd2, false),
                new(nameof(UnparseableStart1), UnparseableStart1, false),
                new(nameof(UnparseableEnd1), UnparseableEnd1, false),
                new(nameof(UnparseableStart2), UnparseableStart2, false),
                new(nameof(UnparseableEnd2), UnparseableEnd2, false)
            ];

            public static RuleScenario<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
        }
    }
}
