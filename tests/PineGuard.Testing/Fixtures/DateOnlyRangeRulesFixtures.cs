using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DateOnlyRangeRulesFixtures
{
    public static class IsChronological
    {
        public static readonly DateOnlyRange TwoDaysExclusive = new(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02));
        public static readonly DateOnlyRange SameDayInclusive = new(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01));
        public static readonly DateOnlyRange SameDayExclusive = new(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 01));

        public static RuleScenario<(DateOnlyRange? range, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(TwoDaysExclusive), (TwoDaysExclusive, Inclusion.Exclusive), true),
            new(nameof(SameDayInclusive), (SameDayInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateOnlyRange? range, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(SameDayExclusive), (SameDayExclusive, Inclusion.Exclusive), false),
            new("Null", (null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateOnlyRange? range, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateOnlyRange range, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(TwoDaysExclusive), (TwoDaysExclusive, Inclusion.Exclusive), true),
            new(nameof(SameDayInclusive), (SameDayInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateOnlyRange range, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(SameDayExclusive), (SameDayExclusive, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateOnlyRange range, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (DateOnlyRange range1, DateOnlyRange range2) TouchingBoundary = (new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 02)), new DateOnlyRange(new DateOnly(2024, 01, 02), new DateOnly(2024, 01, 03)));
        public static readonly (DateOnlyRange range1, DateOnlyRange range2) Overlap = (new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 10)), new DateOnlyRange(new DateOnly(2024, 01, 05), new DateOnly(2024, 01, 15)));
        public static readonly (DateOnlyRange range1, DateOnlyRange range2) NoOverlap = (new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 05)), new DateOnlyRange(new DateOnly(2024, 01, 06), new DateOnly(2024, 01, 10)));

        public static RuleScenario<(DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new("OverlapExclusive", (Overlap.range1, Overlap.range2, Inclusion.Exclusive), true),
            new("TouchingInclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new("NoOverlapExclusive", (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new("TouchingExclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Exclusive), false),
            new("Range1Null", (null, TouchingBoundary.range2, Inclusion.Exclusive), false),
            new("Range2Null", (TouchingBoundary.range1, null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateOnlyRange range1, DateOnlyRange range2, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new("OverlapExclusive", (Overlap.range1, Overlap.range2, Inclusion.Exclusive), true),
            new("TouchingInclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateOnlyRange range1, DateOnlyRange range2, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new("NoOverlapExclusive", (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new("TouchingExclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateOnlyRange range1, DateOnlyRange range2, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class Contains
    {
        public static readonly DateOnlyRange Range = new(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 10));

        public static readonly DateOnly MiddleValue = new(2024, 01, 05);
        public static readonly DateOnly BeforeStartValue = new(2023, 12, 31);
        public static readonly DateOnly StartBoundaryValue = new(2024, 01, 01);
        public static readonly DateOnly EndBoundaryValue = new(2024, 01, 10);
        public static readonly DateOnly OutsideValue = new(2024, 01, 11);

        public static RuleScenario<(DateOnlyRange? range, DateOnly? value, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateOnlyRange? range, DateOnly? value, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false),
            new("NullRange", (null, MiddleValue, Inclusion.Inclusive), false),
            new("NullValue", (Range, null, Inclusion.Inclusive), false)
        ];

        public static RuleScenario<(DateOnlyRange? range, DateOnly? value, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateOnlyRange range, DateOnly value, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateOnlyRange range, DateOnly value, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateOnlyRange range, DateOnly value, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }
}
