using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DateOnlyRulesFixtures
{
    public static class IsBetween
    {
        public static readonly (DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion) MiddleInclusive = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Inclusive);
        public static readonly (DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion) AtMinInclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Inclusive);
        public static readonly (DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion) AtMinExclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Exclusive);
        public static readonly (DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion) NullValue = (null, new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 03), Inclusion.Inclusive);

        public static RuleScenario<(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleInclusive), MiddleInclusive, true),
            new(nameof(AtMinInclusive), AtMinInclusive, true)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(AtMinExclusive), AtMinExclusive, false),
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsBefore
    {
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) BeforeInclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), Inclusion.Inclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) SameDayInclusive = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Inclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) SameDayExclusive = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Exclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) YearPrecision = (new DateOnly(2020, 12, 31), new DateOnly(2021, 1, 1), Inclusion.Inclusive, DatePrecision.Year);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) UnknownPrecision = (new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 1), Inclusion.Inclusive, (DatePrecision)123);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) NullValue = (null, new DateOnly(2024, 01, 02), Inclusion.Inclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) NullOther = (new DateOnly(2024, 01, 01), null, Inclusion.Inclusive, null);

        public static RuleScenario<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(BeforeInclusive), BeforeInclusive, true),
            new(nameof(SameDayInclusive), SameDayInclusive, true),
            new(nameof(YearPrecision), YearPrecision, true)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameDayExclusive), SameDayExclusive, false),
            new(nameof(UnknownPrecision), UnknownPrecision, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsAfter
    {
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) AfterInclusive = (new DateOnly(2024, 01, 03), new DateOnly(2024, 01, 02), Inclusion.Inclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) SameDayInclusive = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Inclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) SameDayExclusive = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), Inclusion.Exclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) MonthPrecision = (new DateOnly(2021, 2, 15), new DateOnly(2021, 1, 1), Inclusion.Inclusive, DatePrecision.Month);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) UnknownPrecision = (new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 1), Inclusion.Inclusive, (DatePrecision)123);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) NullValue = (null, new DateOnly(2024, 01, 02), Inclusion.Inclusive, null);
        public static readonly (DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision) NullOther = (new DateOnly(2024, 01, 01), null, Inclusion.Inclusive, null);

        public static RuleScenario<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(AfterInclusive), AfterInclusive, true),
            new(nameof(SameDayInclusive), SameDayInclusive, true),
            new(nameof(MonthPrecision), MonthPrecision, true)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(SameDayExclusive), SameDayExclusive, false),
            new(nameof(UnknownPrecision), UnknownPrecision, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsSame
    {
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) SameDay = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), null);
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) DifferentDay = (new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03), null);
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) BothNull = (null, null, null);
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) NullValue = (null, new DateOnly(2024, 01, 02), null);
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) NullOther = (new DateOnly(2024, 01, 02), null, null);
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) YearPrecision = (new DateOnly(2020, 1, 15), new DateOnly(2020, 12, 31), DatePrecision.Year);
        public static readonly (DateOnly? value, DateOnly? other, DatePrecision? precision) UnknownPrecision = (new DateOnly(2020, 1, 1), new DateOnly(2020, 1, 1), (DatePrecision)123);

        public static RuleScenario<(DateOnly? value, DateOnly? other, DatePrecision? precision)>[] ValidScenarios =>
        [
            new(nameof(SameDay), SameDay, true),
            new(nameof(BothNull), BothNull, true),
            new(nameof(YearPrecision), YearPrecision, true)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? other, DatePrecision? precision)>[] InvalidScenarios =>
        [
            new(nameof(DifferentDay), DifferentDay, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullOther), NullOther, false),
            new(nameof(UnknownPrecision), UnknownPrecision, false)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? other, DatePrecision? precision)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsChronological
    {
        public static readonly (DateOnly? start, DateOnly? end, Inclusion inclusion) IncreasingExclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), Inclusion.Exclusive);
        public static readonly (DateOnly? start, DateOnly? end, Inclusion inclusion) SameDayInclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01), Inclusion.Inclusive);
        public static readonly (DateOnly? start, DateOnly? end, Inclusion inclusion) SameDayExclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01), Inclusion.Exclusive);
        public static readonly (DateOnly? start, DateOnly? end, Inclusion inclusion) BothNull = (null, null, Inclusion.Exclusive);
        public static readonly (DateOnly? start, DateOnly? end, Inclusion inclusion) StartNullEndSet = (null, new DateOnly(2024, 01, 01), Inclusion.Exclusive);
        public static readonly (DateOnly? start, DateOnly? end, Inclusion inclusion) StartSetEndNull = (new DateOnly(2024, 01, 01), null, Inclusion.Exclusive);

        public static RuleScenario<(DateOnly? start, DateOnly? end, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(IncreasingExclusive), IncreasingExclusive, true),
            new(nameof(SameDayInclusive), SameDayInclusive, true)
        ];

        public static RuleScenario<(DateOnly? start, DateOnly? end, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(SameDayExclusive), SameDayExclusive, false),
            new(nameof(BothNull), BothNull, false),
            new(nameof(StartNullEndSet), StartNullEndSet, false),
            new(nameof(StartSetEndNull), StartSetEndNull, false)
        ];

        public static RuleScenario<(DateOnly? start, DateOnly? end, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion) TouchingInclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03), Inclusion.Inclusive);
        public static readonly (DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion) TouchingExclusive = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03), Inclusion.Exclusive);
        public static readonly (DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion) AllNull = (null, null, null, null, Inclusion.Exclusive);
        public static readonly (DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion) End1Null = (new DateOnly(2024, 01, 01), null, new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), Inclusion.Exclusive);
        public static readonly (DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion) Start2Null = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), null, new DateOnly(2024, 01, 02), Inclusion.Exclusive);
        public static readonly (DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion) End2Null = (new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 01), null, Inclusion.Exclusive);

        public static RuleScenario<(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(TouchingInclusive), TouchingInclusive, true)
        ];

        public static RuleScenario<(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(TouchingExclusive), TouchingExclusive, false),
            new(nameof(AllNull), AllNull, false),
            new(nameof(End1Null), End1Null, false),
            new(nameof(Start2Null), Start2Null, false),
            new(nameof(End2Null), End2Null, false)
        ];

        public static RuleScenario<(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithinCalendarMonths
    {
        public static readonly (DateOnly? value, DateOnly? reference, int months) SameDayZeroMonths = (new DateOnly(2024, 02, 15), new DateOnly(2024, 02, 15), 0);
        public static readonly (DateOnly? value, DateOnly? reference, int months) SameMonthDifferentDay = (new DateOnly(2024, 02, 01), new DateOnly(2024, 02, 28), 0);
        public static readonly (DateOnly? value, DateOnly? reference, int months) WithinOneMonth = (new DateOnly(2024, 03, 01), new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int months) OutsideOneMonth = (new DateOnly(2024, 04, 01), new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int months) NegativeMonths = (new DateOnly(2024, 02, 15), new DateOnly(2024, 02, 15), -1);
        public static readonly (DateOnly? value, DateOnly? reference, int months) NullValue = (null, new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int months) NullReference = (new DateOnly(2024, 02, 15), null, 1);

        public static RuleScenario<(DateOnly? value, DateOnly? reference, int months)>[] ValidScenarios =>
        [
            new(nameof(SameDayZeroMonths), SameDayZeroMonths, true),
            new(nameof(SameMonthDifferentDay), SameMonthDifferentDay, true),
            new(nameof(WithinOneMonth), WithinOneMonth, true)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? reference, int months)>[] InvalidScenarios =>
        [
            new(nameof(OutsideOneMonth), OutsideOneMonth, false),
            new(nameof(NegativeMonths), NegativeMonths, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? reference, int months)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsWithin
    {
        public static readonly (DateOnly? value, DateOnly? reference, int days) SameDayZeroDays = (new DateOnly(2024, 02, 15), new DateOnly(2024, 02, 15), 0);
        public static readonly (DateOnly? value, DateOnly? reference, int days) WithinDays = (new DateOnly(2024, 02, 16), new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int days) SymmetricEarlier = (new DateOnly(2024, 02, 14), new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int days) OutsideDays = (new DateOnly(2024, 02, 17), new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int days) NegativeDays = (new DateOnly(2024, 02, 15), new DateOnly(2024, 02, 15), -1);
        public static readonly (DateOnly? value, DateOnly? reference, int days) NullValue = (null, new DateOnly(2024, 02, 15), 1);
        public static readonly (DateOnly? value, DateOnly? reference, int days) NullReference = (new DateOnly(2024, 02, 15), null, 1);

        public static RuleScenario<(DateOnly? value, DateOnly? reference, int days)>[] ValidScenarios =>
        [
            new(nameof(SameDayZeroDays), SameDayZeroDays, true),
            new(nameof(WithinDays), WithinDays, true),
            new(nameof(SymmetricEarlier), SymmetricEarlier, true)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? reference, int days)>[] InvalidScenarios =>
        [
            new(nameof(OutsideDays), OutsideDays, false),
            new(nameof(NegativeDays), NegativeDays, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NullReference), NullReference, false)
        ];

        public static RuleScenario<(DateOnly? value, DateOnly? reference, int days)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPast
    {
        public static readonly DateOnly? PastDate = new DateOnly(2000, 01, 10);
        public static readonly DateOnly? FutureDate = new DateOnly(2099, 01, 10);
        public static readonly DateOnly? NullValue = null;

        public static RuleScenario<DateOnly?>[] ValidScenarios => [new(nameof(PastDate), PastDate, true)];
        public static RuleScenario<DateOnly?>[] InvalidScenarios =>
        [
            new(nameof(FutureDate), FutureDate, false),
            new(nameof(NullValue), NullValue, false)
        ];
        public static RuleScenario<DateOnly?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
