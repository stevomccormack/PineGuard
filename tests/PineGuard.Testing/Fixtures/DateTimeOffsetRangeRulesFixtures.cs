using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DateTimeOffsetRangeRulesFixtures
{
    public static class IsChronological
    {
        public static readonly DateTimeOffsetRange StartBeforeEndExclusive = new(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero));
        public static readonly DateTimeOffsetRange StartEqualsEndInclusive = new(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero));
        public static readonly DateTimeOffsetRange StartEqualsEndExclusive = new(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero));

        public static RuleScenario<(DateTimeOffsetRange? range, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(StartBeforeEndExclusive), (StartBeforeEndExclusive, Inclusion.Exclusive), true),
            new(nameof(StartEqualsEndInclusive), (StartEqualsEndInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeOffsetRange? range, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(StartEqualsEndExclusive), (StartEqualsEndExclusive, Inclusion.Exclusive), false),
            new("Null", (null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeOffsetRange? range, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateTimeOffsetRange range, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(StartBeforeEndExclusive), (StartBeforeEndExclusive, Inclusion.Exclusive), true),
            new(nameof(StartEqualsEndInclusive), (StartEqualsEndInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeOffsetRange range, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(StartEqualsEndExclusive), (StartEqualsEndExclusive, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeOffsetRange range, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (DateTimeOffsetRange range1, DateTimeOffsetRange range2) TouchingBoundary = (new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero)), new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 02, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 03, 0, 0, 0, TimeSpan.Zero)));
        public static readonly (DateTimeOffsetRange range1, DateTimeOffsetRange range2) Overlap = (new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero)), new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 05, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 15, 0, 0, 0, TimeSpan.Zero)));
        public static readonly (DateTimeOffsetRange range1, DateTimeOffsetRange range2) NoOverlap = (new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 05, 0, 0, 0, TimeSpan.Zero)), new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 06, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero)));

        public static RuleScenario<(DateTimeOffsetRange? range1, DateTimeOffsetRange? range2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new("OverlapExclusive", (Overlap.range1, Overlap.range2, Inclusion.Exclusive), true),
            new("TouchingInclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeOffsetRange? range1, DateTimeOffsetRange? range2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new("NoOverlapExclusive", (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new("TouchingExclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Exclusive), false),
            new("Range1Null", (null, TouchingBoundary.range2, Inclusion.Exclusive), false),
            new("Range2Null", (TouchingBoundary.range1, null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeOffsetRange? range1, DateTimeOffsetRange? range2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new("OverlapExclusive", (Overlap.range1, Overlap.range2, Inclusion.Exclusive), true),
            new("TouchingInclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new("NoOverlapExclusive", (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new("TouchingExclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class Contains
    {
        public static readonly DateTimeOffsetRange Range = new(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero));

        public static readonly DateTimeOffset MiddleValue = new(2024, 01, 05, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset BeforeStartValue = new(2023, 12, 31, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset StartBoundaryValue = new(2024, 01, 01, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset EndBoundaryValue = new(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset OutsideValue = new(2024, 01, 11, 0, 0, 0, TimeSpan.Zero);

        public static RuleScenario<(DateTimeOffsetRange? range, DateTimeOffset? value, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeOffsetRange? range, DateTimeOffset? value, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false),
            new("NullRange", (null, MiddleValue, Inclusion.Inclusive), false),
            new("NullValue", (Range, null, Inclusion.Inclusive), false)
        ];

        public static RuleScenario<(DateTimeOffsetRange? range, DateTimeOffset? value, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }
}
