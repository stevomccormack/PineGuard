using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DateTimeRulesFixtures
{
    public static class IsBetween
    {
        public static readonly (DateTime? value, DateTime min, DateTime max, Inclusion inclusion) MiddleInclusive = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive);
        public static readonly (DateTime? value, DateTime min, DateTime max, Inclusion inclusion) AtMinInclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive);
        public static readonly (DateTime? value, DateTime min, DateTime max, Inclusion inclusion) AtMinExclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? value, DateTime min, DateTime max, Inclusion inclusion) NullValue = (null, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive);
        public static readonly (DateTime? value, DateTime min, DateTime max, Inclusion inclusion) MixedKindWithinBounds = (new DateTime(2024, 01, 02, 12, 0, 0, DateTimeKind.Utc).ToLocalTime(), new DateTime(2024, 01, 02, 11, 59, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 12, 1, 0, DateTimeKind.Utc), Inclusion.Inclusive);

        public static RuleScenario<(DateTime? value, DateTime min, DateTime max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleInclusive), MiddleInclusive, true),
            new(nameof(AtMinInclusive), AtMinInclusive, true),
            new(nameof(MixedKindWithinBounds), MixedKindWithinBounds, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime min, DateTime max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(AtMinExclusive), AtMinExclusive, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime min, DateTime max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsBefore
    {
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) BeforeInclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantInclusive = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantExclusive = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionDay = (new DateTime(2024, 01, 01, 12, 59, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, DateTimePrecision.Day);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionUnknown = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, (DateTimePrecision)999);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) ImplicitLocalVsUtc = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Local), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) NullValue = (null, new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) NullOther = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, Inclusion.Inclusive, null);

        public static RuleScenario<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(BeforeInclusive), BeforeInclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true),
            new(nameof(PrecisionDay), PrecisionDay, true),
            new(nameof(ImplicitLocalVsUtc), ImplicitLocalVsUtc, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(PrecisionUnknown), PrecisionUnknown, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsBeforeDefaultInclusion
    {
        public static readonly (DateTime? value, DateTime? other) StrictlyBefore = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc));
        public static readonly (DateTime? value, DateTime? other) SameInstant = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc));

        public static RuleScenario<(DateTime? value, DateTime? other)>[] ValidScenarios =>
        [
            new(nameof(StrictlyBefore), StrictlyBefore, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other)>[] InvalidScenarios =>
        [
            new(nameof(SameInstant), SameInstant, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAfter
    {
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) AfterInclusive = (new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantInclusive = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) SameInstantExclusive = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionDay = (new DateTime(2020, 01, 02, 23, 59, 59, DateTimeKind.Utc), new DateTime(2020, 01, 01, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, DateTimePrecision.Day);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) PrecisionUnknown = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, (DateTimePrecision)999);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) NullValue = (null, new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive, null);
        public static readonly (DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision) NullOther = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, Inclusion.Inclusive, null);

        public static RuleScenario<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(AfterInclusive), AfterInclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true),
            new(nameof(PrecisionDay), PrecisionDay, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(PrecisionUnknown), PrecisionUnknown, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAfterDefaultInclusion
    {
        public static readonly (DateTime? value, DateTime? other) StrictlyAfter = (new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc));
        public static readonly (DateTime? value, DateTime? other) SameInstant = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc));

        public static RuleScenario<(DateTime? value, DateTime? other)>[] ValidScenarios =>
        [
            new(nameof(StrictlyAfter), StrictlyAfter, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other)>[] InvalidScenarios =>
        [
            new(nameof(SameInstant), SameInstant, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSame
    {
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) SameUtc = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), null);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) UnspecifiedVsUtc = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), null);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) PrecisionHour = (new DateTime(2020, 01, 01, 10, 59, 59, DateTimeKind.Utc), new DateTime(2020, 01, 01, 10, 0, 0, DateTimeKind.Utc), DateTimePrecision.Hour);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) BothNull = (null, null, null);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) Different = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), null);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) PrecisionUnknown = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), (DateTimePrecision)999);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) NullValue = (null, new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), null);
        public static readonly (DateTime? value, DateTime? other, DateTimePrecision? precision) NullOther = (new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), null, null);

        public static RuleScenario<(DateTime? value, DateTime? other, DateTimePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(SameUtc), SameUtc, true),
            new(nameof(UnspecifiedVsUtc), UnspecifiedVsUtc, true),
            new(nameof(PrecisionHour), PrecisionHour, true),
            new(nameof(BothNull), BothNull, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other, DateTimePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(Different), Different, false),
            new(nameof(PrecisionUnknown), PrecisionUnknown, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other, DateTimePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsChronological
    {
        public static readonly (DateTime? start, DateTime? end, Inclusion inclusion) ChronologicalExclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start, DateTime? end, Inclusion inclusion) SameInstantInclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive);
        public static readonly (DateTime? start, DateTime? end, Inclusion inclusion) SameInstantExclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start, DateTime? end, Inclusion inclusion) BothNull = (null, null, Inclusion.Exclusive);
        public static readonly (DateTime? start, DateTime? end, Inclusion inclusion) StartNullEndSet = (null, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start, DateTime? end, Inclusion inclusion) StartSetEndNull = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, Inclusion.Exclusive);

        public static RuleScenario<(DateTime? start, DateTime? end, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(ChronologicalExclusive), ChronologicalExclusive, true),
            new(nameof(SameInstantInclusive), SameInstantInclusive, true)
        ];

        public static RuleScenario<(DateTime? start, DateTime? end, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(SameInstantExclusive), SameInstantExclusive, false),
            new(nameof(BothNull), BothNull, false),
            new(nameof(StartNullEndSet), StartNullEndSet, false),
            new(nameof(StartSetEndNull), StartSetEndNull, false)
        ];

        public static RuleScenario<(DateTime? start, DateTime? end, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) TouchingInclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) TouchingExclusive = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) DisjointExclusive = (new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 04, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) DisjointInclusive = (new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 04, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Inclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) AllNull = (null, null, null, null, Inclusion.Exclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) End1Null = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) Start2Null = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) End2Null = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, Inclusion.Exclusive);
        public static readonly (DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion) FirstRangeInverted = (new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 05, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 31, 0, 0, 0, DateTimeKind.Utc), Inclusion.Exclusive);

        public static RuleScenario<(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(TouchingInclusive), TouchingInclusive, true)
        ];

        public static RuleScenario<(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(TouchingExclusive), TouchingExclusive, false),
            new(nameof(DisjointExclusive), DisjointExclusive, false),
            new(nameof(DisjointInclusive), DisjointInclusive, false),
            new(nameof(AllNull), AllNull, false),
            new(nameof(End1Null), End1Null, false),
            new(nameof(Start2Null), Start2Null, false),
            new(nameof(End2Null), End2Null, false),
            new(nameof(FirstRangeInverted), FirstRangeInverted, false)
        ];

        public static RuleScenario<(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithin
    {
        public static readonly (DateTime? value, DateTime? reference, TimeSpan window) SameInstantZeroWindow = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero);
        public static readonly (DateTime? value, DateTime? reference, TimeSpan window) WithinWindow = (new DateTime(2024, 01, 01, 0, 0, 5, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), TimeSpan.FromSeconds(10));
        public static readonly (DateTime? value, DateTime? reference, TimeSpan window) OutsideWindow = (new DateTime(2024, 01, 01, 0, 0, 11, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), TimeSpan.FromSeconds(10));
        public static readonly (DateTime? value, DateTime? reference, TimeSpan window) NegativeWindow = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), TimeSpan.FromSeconds(-1));
        public static readonly (DateTime? value, DateTime? reference, TimeSpan window) NullValue = (null, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), TimeSpan.FromSeconds(10));
        public static readonly (DateTime? value, DateTime? reference, TimeSpan window) NullReference = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null, TimeSpan.FromSeconds(10));

        public static RuleScenario<(DateTime? value, DateTime? reference, TimeSpan window)>[] ValidScenarios =>
        [
            new(nameof(SameInstantZeroWindow), SameInstantZeroWindow, true),
            new(nameof(WithinWindow), WithinWindow, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? reference, TimeSpan window)>[] InvalidScenarios =>
        [
            new(nameof(OutsideWindow), OutsideWindow, false),
            new(nameof(NegativeWindow), NegativeWindow, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? reference, TimeSpan window)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithinCalendarMonths
    {
        public static readonly (DateTime? value, DateTime? reference, int months) SameDayZeroMonths = (new DateTime(2024, 02, 15, 10, 0, 0, DateTimeKind.Utc), new DateTime(2024, 02, 15, 20, 0, 0, DateTimeKind.Utc), 0);
        public static readonly (DateTime? value, DateTime? reference, int months) WithinOneMonth = (new DateTime(2024, 03, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 02, 15, 0, 0, 0, DateTimeKind.Utc), 1);
        public static readonly (DateTime? value, DateTime? reference, int months) OutsideOneMonth = (new DateTime(2024, 04, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 02, 15, 0, 0, 0, DateTimeKind.Utc), 1);
        public static readonly (DateTime? value, DateTime? reference, int months) NegativeMonths = (new DateTime(2024, 02, 15, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 02, 15, 0, 0, 0, DateTimeKind.Utc), -1);
        public static readonly (DateTime? value, DateTime? reference, int months) NullValue = (null, new DateTime(2024, 02, 15, 0, 0, 0, DateTimeKind.Utc), 1);
        public static readonly (DateTime? value, DateTime? reference, int months) NullReference = (new DateTime(2024, 02, 15, 0, 0, 0, DateTimeKind.Utc), null, 1);

        public static RuleScenario<(DateTime? value, DateTime? reference, int months)>[] ValidScenarios =>
        [
            new(nameof(SameDayZeroMonths), SameDayZeroMonths, true),
            new(nameof(WithinOneMonth), WithinOneMonth, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? reference, int months)>[] InvalidScenarios =>
        [
            new(nameof(OutsideOneMonth), OutsideOneMonth, false),
            new(nameof(NegativeMonths), NegativeMonths, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? reference, int months)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWeekday
    {
        public static readonly DateTime? Monday = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Saturday = new(2024, 01, 06, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Sunday = new(2024, 01, 07, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(Monday), Monday, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(Saturday), Saturday, false),
            new(nameof(Sunday), Sunday, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWeekend
    {
        public static readonly DateTime? Saturday = new(2024, 01, 06, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Sunday = new(2024, 01, 07, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Monday = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(Saturday), Saturday, true),
            new(nameof(Sunday), Sunday, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(Monday), Monday, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsFirstDayOfMonth
    {
        public static readonly DateTime? FirstDay = new(2024, 02, 01, 12, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NotFirst = new(2024, 02, 02, 12, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(FirstDay), FirstDay, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(NotFirst), NotFirst, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLastDayOfMonth
    {
        public static readonly DateTime? LastDay = new(2024, 02, 29, 12, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NotLast = new(2024, 02, 28, 12, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(LastDay), LastDay, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(NotLast), NotLast, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSameDay
    {
        public static readonly (DateTime? value, DateTime? other) SameDay = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 23, 59, 59, DateTimeKind.Utc));
        public static readonly (DateTime? value, DateTime? other) DifferentDay = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc));
        public static readonly (DateTime? value, DateTime? other) SameInstantDifferentKind = (new DateTime(2024, 01, 01, 12, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 12, 0, 0, DateTimeKind.Utc).ToLocalTime());
        public static readonly (DateTime? value, DateTime? other) BothNull = (null, null);
        public static readonly (DateTime? value, DateTime? other) NullValue = (null, new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc));
        public static readonly (DateTime? value, DateTime? other) NullOther = (new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), null);

        public static RuleScenario<(DateTime? value, DateTime? other)>[] ValidScenarios =>
        [
            new(nameof(SameDay), SameDay, true),
            new(nameof(SameInstantDifferentKind), SameInstantDifferentKind, true),
            new(nameof(BothNull), BothNull, true)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other)>[] InvalidScenarios =>
        [
            new(nameof(DifferentDay), DifferentDay, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateTime? value, DateTime? other)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsUtc
    {
        public static readonly DateTime? Utc = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Local = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime? Unspecified = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(Utc), Utc, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(Local), Local, false),
            new(nameof(Unspecified), Unspecified, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLocal
    {
        public static readonly DateTime? Local = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime? Utc = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Unspecified = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(Local), Local, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(Utc), Utc, false),
            new(nameof(Unspecified), Unspecified, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsUnspecified
    {
        public static readonly DateTime? Unspecified = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified);
        public static readonly DateTime? Utc = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Local = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(Unspecified), Unspecified, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(Utc), Utc, false),
            new(nameof(Local), Local, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasExplicitKind
    {
        public static readonly DateTime? Utc = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? Local = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Local);
        public static readonly DateTime? Unspecified = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Unspecified);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios =>
        [
            new(nameof(Utc), Utc, true),
            new(nameof(Local), Local, true)
        ];

        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(Unspecified), Unspecified, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPast
    {
        public static readonly DateTime? PastDate = new(2000, 01, 10, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? FutureDate = new(2099, 01, 10, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime? NullValue = null;

        public static RuleScenario<DateTime?>[] ValidScenarios => [new(nameof(PastDate), PastDate, true)];
        public static RuleScenario<DateTime?>[] InvalidScenarios =>
        [
            new(nameof(FutureDate), FutureDate, false),
            new(nameof(NullValue), NullValue, false)
        ];
        public static RuleScenario<DateTime?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
