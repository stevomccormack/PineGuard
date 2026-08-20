using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class TimeOnlyRulesFixtures
{
    public static class IsBetween
    {
        public static readonly (TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion) MiddleInclusive = (new TimeOnly(12, 0), new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive);
        public static readonly (TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion) AtMinInclusive = (new TimeOnly(11, 0), new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive);
        public static readonly (TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion) AtMinExclusive = (new TimeOnly(11, 0), new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion) NullValue = (null, new TimeOnly(11, 0), new TimeOnly(13, 0), Inclusion.Inclusive);

        public static RuleScenario<(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleInclusive), MiddleInclusive, true),
            new(nameof(AtMinInclusive), AtMinInclusive, true)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(AtMinExclusive), AtMinExclusive, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsBefore
    {
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) BeforeInclusive = (new TimeOnly(9, 0), new TimeOnly(10, 0), Inclusion.Inclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) SameInstantInclusive = (new TimeOnly(10, 0), new TimeOnly(10, 0), Inclusion.Inclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) PrecisionSecond = (new TimeOnly(10, 0, 0, 500), new TimeOnly(10, 0, 0, 600), Inclusion.Inclusive, TimePrecision.Second);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) SameInstantExclusive = (new TimeOnly(10, 0), new TimeOnly(10, 0), Inclusion.Exclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) InvalidPrecision = (new TimeOnly(9, 0), new TimeOnly(10, 0), Inclusion.Inclusive, (TimePrecision)123);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) NullValue = (null, new TimeOnly(10, 0), Inclusion.Inclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) NullOther = (new TimeOnly(9, 0), null, Inclusion.Inclusive, null);

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(BeforeInclusive), BeforeInclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true),
            new(nameof(PrecisionSecond), PrecisionSecond, true)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(InvalidPrecision), InvalidPrecision, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAfter
    {
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) AfterInclusive = (new TimeOnly(10, 0), new TimeOnly(9, 0), Inclusion.Inclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) SameInstantInclusive = (new TimeOnly(10, 0), new TimeOnly(10, 0), Inclusion.Inclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) PrecisionSecond = (new TimeOnly(10, 0, 0, 600), new TimeOnly(10, 0, 0, 500), Inclusion.Inclusive, TimePrecision.Second);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) SameInstantExclusive = (new TimeOnly(10, 0), new TimeOnly(10, 0), Inclusion.Exclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) InvalidPrecision = (new TimeOnly(10, 0), new TimeOnly(9, 0), Inclusion.Inclusive, (TimePrecision)123);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) NullValue = (null, new TimeOnly(9, 0), Inclusion.Inclusive, null);
        public static readonly (TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision) NullOther = (new TimeOnly(10, 0), null, Inclusion.Inclusive, null);

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(AfterInclusive), AfterInclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true),
            new(nameof(PrecisionSecond), PrecisionSecond, true)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(InvalidPrecision), InvalidPrecision, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSame
    {
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) SameExact = (new TimeOnly(10, 0), new TimeOnly(10, 0), null);
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) BothNull = (null, null, null);
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) PrecisionSecond = (new TimeOnly(10, 0, 0, 500), new TimeOnly(10, 0, 0, 600), TimePrecision.Second);
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) Different = (new TimeOnly(10, 0), new TimeOnly(10, 1), null);
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) InvalidPrecision = (new TimeOnly(10, 0), new TimeOnly(10, 0), (TimePrecision)123);
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) NullValue = (null, new TimeOnly(10, 0), null);
        public static readonly (TimeOnly? value, TimeOnly? other, TimePrecision? precision) NullOther = (new TimeOnly(10, 0), null, null);

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, TimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(SameExact), SameExact, true),
            new(nameof(BothNull), BothNull, true),
            new(nameof(PrecisionSecond), PrecisionSecond, true)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, TimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(Different), Different, false),
            new(nameof(InvalidPrecision), InvalidPrecision, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? other, TimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithin
    {
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) SameInstantZeroWindow = (new TimeOnly(10, 0), new TimeOnly(10, 0), TimeSpan.Zero);
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) WithinWindow = (new TimeOnly(10, 0, 5), new TimeOnly(10, 0, 0), TimeSpan.FromSeconds(10));
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) WithinWindowEarlier = (new TimeOnly(9, 59, 55), new TimeOnly(10, 0, 0), TimeSpan.FromSeconds(10));
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) OutsideWindow = (new TimeOnly(10, 0, 11), new TimeOnly(10, 0, 0), TimeSpan.FromSeconds(10));
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) NegativeWindow = (new TimeOnly(10, 0), new TimeOnly(10, 0), TimeSpan.FromSeconds(-1));
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) WindowOverDay = (new TimeOnly(10, 0), new TimeOnly(10, 0), TimeSpan.FromDays(1) + TimeSpan.FromTicks(1));
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) NullValue = (null, new TimeOnly(10, 0), TimeSpan.FromSeconds(10));
        public static readonly (TimeOnly? value, TimeOnly? reference, TimeSpan window) NullReference = (new TimeOnly(10, 0), null, TimeSpan.FromSeconds(10));

        public static RuleScenario<(TimeOnly? value, TimeOnly? reference, TimeSpan window)>[] ValidScenarios =>
        [
            new(nameof(SameInstantZeroWindow), SameInstantZeroWindow, true),
            new(nameof(WithinWindow), WithinWindow, true),
            new(nameof(WithinWindowEarlier), WithinWindowEarlier, true)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? reference, TimeSpan window)>[] InvalidScenarios =>
        [
            new(nameof(OutsideWindow), OutsideWindow, false),
            new(nameof(NegativeWindow), NegativeWindow, false),
            new(nameof(WindowOverDay), WindowOverDay, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(TimeOnly? value, TimeOnly? reference, TimeSpan window)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsChronological
    {
        public static readonly (TimeOnly? start, TimeOnly? end, Inclusion inclusion) ChronologicalExclusive = (new TimeOnly(11, 0), new TimeOnly(12, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start, TimeOnly? end, Inclusion inclusion) SameInstantInclusive = (new TimeOnly(11, 0), new TimeOnly(11, 0), Inclusion.Inclusive);
        public static readonly (TimeOnly? start, TimeOnly? end, Inclusion inclusion) SameInstantExclusive = (new TimeOnly(11, 0), new TimeOnly(11, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start, TimeOnly? end, Inclusion inclusion) BothNull = (null, null, Inclusion.Exclusive);
        public static readonly (TimeOnly? start, TimeOnly? end, Inclusion inclusion) StartNull = (null, new TimeOnly(12, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start, TimeOnly? end, Inclusion inclusion) EndNull = (new TimeOnly(11, 0), null, Inclusion.Exclusive);

        public static RuleScenario<(TimeOnly? start, TimeOnly? end, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(ChronologicalExclusive), ChronologicalExclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true)
        ];

        public static RuleScenario<(TimeOnly? start, TimeOnly? end, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(BothNull), BothNull, false),
            new(nameof(StartNull), StartNull, false),
            new(nameof(EndNull), EndNull, false)
        ];

        public static RuleScenario<(TimeOnly? start, TimeOnly? end, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) OverlapExclusive = (new TimeOnly(9, 0), new TimeOnly(11, 0), new TimeOnly(10, 0), new TimeOnly(12, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) TouchingInclusive = (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(10, 0), new TimeOnly(11, 0), Inclusion.Inclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) TouchingExclusive = (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(10, 0), new TimeOnly(11, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) DisjointExclusive = (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(11, 0), new TimeOnly(12, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) DisjointExclusiveReversed = (new TimeOnly(14, 0), new TimeOnly(15, 0), new TimeOnly(9, 0), new TimeOnly(10, 0), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) DisjointInclusive = (new TimeOnly(14, 0), new TimeOnly(15, 0), new TimeOnly(9, 0), new TimeOnly(10, 0), Inclusion.Inclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) AllNull = (null, null, null, null, Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) Start1Null = (null, new TimeOnly(10, 0), new TimeOnly(9, 30), new TimeOnly(9, 45), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) End1Null = (new TimeOnly(9, 0), null, new TimeOnly(9, 30), new TimeOnly(9, 45), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) Start2Null = (new TimeOnly(9, 0), new TimeOnly(10, 0), null, new TimeOnly(9, 45), Inclusion.Exclusive);
        public static readonly (TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion) End2Null = (new TimeOnly(9, 0), new TimeOnly(10, 0), new TimeOnly(9, 30), null, Inclusion.Exclusive);

        public static RuleScenario<(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(OverlapExclusive), OverlapExclusive, true),
            new(nameof(TouchingInclusive), TouchingInclusive, true)
        ];

        public static RuleScenario<(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(TouchingExclusive), TouchingExclusive, false),
            new(nameof(DisjointExclusive), DisjointExclusive, false),
            new(nameof(DisjointExclusiveReversed), DisjointExclusiveReversed, false),
            new(nameof(DisjointInclusive), DisjointInclusive, false),
            new(nameof(AllNull), AllNull, false),
            new(nameof(Start1Null), Start1Null, false),
            new(nameof(End1Null), End1Null, false),
            new(nameof(Start2Null), Start2Null, false),
            new(nameof(End2Null), End2Null, false)
        ];

        public static RuleScenario<(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    /// <summary>Scalar reference times used across test layers.</summary>
    public static class IsKnownTimes
    {
        public static readonly TimeOnly? T0800 = new TimeOnly(8, 0);
        public static readonly TimeOnly? T0830 = new TimeOnly(8, 30);
        public static readonly TimeOnly? T0930 = new TimeOnly(9, 30);
        public static readonly TimeOnly? T1000 = new TimeOnly(10, 0);
        public static readonly TimeOnly? T1100 = new TimeOnly(11, 0);
        public static readonly TimeOnly? T1200 = new TimeOnly(12, 0);
        public static readonly TimeOnly? T1300 = new TimeOnly(13, 0);
        public static readonly TimeOnly? T1400 = new TimeOnly(14, 0);
        public static readonly TimeOnly? T2000 = new TimeOnly(20, 0);
    }
}
