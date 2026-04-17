using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DateTimeOffsetRulesFixtures
{
    public static class IsBetween
    {
        public static readonly (DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) MiddleInclusive = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive);
        public static readonly (DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) AtMinInclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive);
        public static readonly (DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) AtMinExclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion) NullValue = (null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive);

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleInclusive), MiddleInclusive, true),
            new(nameof(AtMinInclusive), AtMinInclusive, true)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(AtMinExclusive), AtMinExclusive, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsBefore
    {
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) BeforeInclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantInclusive = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantExclusive = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionDay = (new DateTimeOffset(2020, 1, 1, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, DateTimePrecision.Day);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionUnknown = (new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2019, 1, 1, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, (DateTimePrecision)123);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) NullValue = (null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) NullOther = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, Inclusion.Inclusive, null);

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(BeforeInclusive), BeforeInclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true),
            new(nameof(PrecisionDay), PrecisionDay, true)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(PrecisionUnknown), PrecisionUnknown, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAfter
    {
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) AfterInclusive = (new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantInclusive = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantExclusive = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionDay = (new DateTimeOffset(2020, 1, 2, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, DateTimePrecision.Day);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionUnknown = (new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2019, 1, 1, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, (DateTimePrecision)123);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) NullValue = (null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision) NullOther = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, Inclusion.Inclusive, null);

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(AfterInclusive), AfterInclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true),
            new(nameof(PrecisionDay), PrecisionDay, true)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(PrecisionUnknown), PrecisionUnknown, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSame
    {
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) SameInstant = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) BothNull = (null, null, null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) PrecisionHour = (new DateTimeOffset(2020, 1, 1, 10, 59, 59, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 10, 0, 0, TimeSpan.Zero), DateTimePrecision.Hour);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) DifferentInstant = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) PrecisionUnknown = (new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), (DateTimePrecision)123);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) NullValue = (null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), null);
        public static readonly (DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision) NullOther = (new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), null, null);

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(SameInstant), SameInstant, true),
            new(nameof(BothNull), BothNull, true),
            new(nameof(PrecisionHour), PrecisionHour, true)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(DifferentInstant), DifferentInstant, false),
            new(nameof(PrecisionUnknown), PrecisionUnknown, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsChronological
    {
        public static readonly (DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion) IncreasingExclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion) SameInstantInclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive);
        public static readonly (DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion) SameInstantExclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion) BothNull = (null, null, Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion) StartNullEndSet = (null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion) StartSetEndNull = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, Inclusion.Exclusive);

        public static RuleScenario<(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(IncreasingExclusive), IncreasingExclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true)
        ];

        public static RuleScenario<(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(BothNull), BothNull, false),
            new(nameof(StartNullEndSet), StartNullEndSet, false),
            new(nameof(StartSetEndNull), StartSetEndNull, false)
        ];

        public static RuleScenario<(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) TouchingInclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Inclusive);
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) TouchingExclusive = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) AllNull = (null, null, null, null, Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) End1Null = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) Start2Null = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) End2Null = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, Inclusion.Exclusive);
        public static readonly (DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion) Start1Null = (null, new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), Inclusion.Exclusive);

        public static RuleScenario<(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(TouchingInclusive), TouchingInclusive, true)
        ];

        public static RuleScenario<(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(TouchingExclusive), TouchingExclusive, false),
            new(nameof(AllNull), AllNull, false),
            new(nameof(End1Null), End1Null, false),
            new(nameof(Start2Null), Start2Null, false),
            new(nameof(End2Null), End2Null, false),
            new(nameof(Start1Null), Start1Null, false)
        ];

        public static RuleScenario<(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithin
    {
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window) SameInstantZeroWindow = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), TimeSpan.Zero);
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window) WithinWindow = (new DateTimeOffset(2024, 01, 01, 0, 0, 5, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(10));
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window) OutsideWindow = (new DateTimeOffset(2024, 01, 01, 0, 0, 11, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(10));
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window) NegativeWindow = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(-1));
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window) NullValue = (null, new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), TimeSpan.FromSeconds(10));
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window) NullReference = (new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), null, TimeSpan.FromSeconds(10));

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window)>[] ValidScenarios =>
        [
            new(nameof(SameInstantZeroWindow), SameInstantZeroWindow, true),
            new(nameof(WithinWindow), WithinWindow, true)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window)>[] InvalidScenarios =>
        [
            new(nameof(OutsideWindow), OutsideWindow, false),
            new(nameof(NegativeWindow), NegativeWindow, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithinCalendarMonths
    {
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, int months) SameDayZeroMonths = (new DateTimeOffset(2024, 02, 15, 10, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 02, 15, 20, 0, 0, TimeSpan.Zero), 0);
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, int months) WithinOneMonth = (new DateTimeOffset(2024, 03, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.Zero), 1);
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, int months) OutsideOneMonth = (new DateTimeOffset(2024, 04, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.Zero), 1);
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, int months) NegativeMonths = (new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.Zero), -1);
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, int months) NullValue = (null, new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.Zero), 1);
        public static readonly (DateTimeOffset? value, DateTimeOffset? reference, int months) NullReference = (new DateTimeOffset(2024, 02, 15, 0, 0, 0, TimeSpan.Zero), null, 1);

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? reference, int months)>[] ValidScenarios =>
        [
            new(nameof(SameDayZeroMonths), SameDayZeroMonths, true),
            new(nameof(WithinOneMonth), WithinOneMonth, true)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? reference, int months)>[] InvalidScenarios =>
        [
            new(nameof(OutsideOneMonth), OutsideOneMonth, false),
            new(nameof(NegativeMonths), NegativeMonths, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(DateTimeOffset? value, DateTimeOffset? reference, int months)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPast
    {
        public static readonly DateTimeOffset? PastDate = new DateTimeOffset(2000, 01, 10, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset? FutureDate = new DateTimeOffset(2099, 01, 10, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset? NullValue = null;

        public static RuleScenario<DateTimeOffset?>[] ValidScenarios => [new(nameof(PastDate), PastDate, true)];
        public static RuleScenario<DateTimeOffset?>[] InvalidScenarios =>
        [
            new(nameof(FutureDate), FutureDate, false),
            new(nameof(NullValue), NullValue, false)
        ];
        public static RuleScenario<DateTimeOffset?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
