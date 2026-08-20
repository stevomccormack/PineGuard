using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DateTimeRangeRulesFixtures
{
    public static class IsChronological
    {
        public static readonly DateTimeRange StartBeforeEndExclusive = new(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc));
        public static readonly DateTimeRange StartEqualsEndInclusive = new(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc));
        public static readonly DateTimeRange StartEqualsEndExclusive = new(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc));

        public static RuleScenario<(DateTimeRange? range, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(StartBeforeEndExclusive), (StartBeforeEndExclusive, Inclusion.Exclusive), true),
            new(nameof(StartEqualsEndInclusive), (StartEqualsEndInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeRange? range, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(StartEqualsEndExclusive), (StartEqualsEndExclusive, Inclusion.Exclusive), false),
            new("Null", (null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeRange? range, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateTimeRange range, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(StartBeforeEndExclusive), (StartBeforeEndExclusive, Inclusion.Exclusive), true),
            new(nameof(StartEqualsEndInclusive), (StartEqualsEndInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeRange range, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(StartEqualsEndExclusive), (StartEqualsEndExclusive, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeRange range, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (DateTimeRange range1, DateTimeRange range2) TouchingBoundary = (new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc)), new DateTimeRange(new DateTime(2024, 01, 02, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 03, 0, 0, 0, DateTimeKind.Utc)));
        public static readonly (DateTimeRange range1, DateTimeRange range2) Overlap = (new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc)), new DateTimeRange(new DateTime(2024, 01, 05, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 15, 0, 0, 0, DateTimeKind.Utc)));
        public static readonly (DateTimeRange range1, DateTimeRange range2) NoOverlap = (new DateTimeRange(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 05, 0, 0, 0, DateTimeKind.Utc)), new DateTimeRange(new DateTime(2024, 01, 06, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc)));

        public static RuleScenario<(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new("OverlapExclusive", (Overlap.range1, Overlap.range2, Inclusion.Exclusive), true),
            new("TouchingInclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new("NoOverlapExclusive", (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new("TouchingExclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Exclusive), false),
            new("Range1Null", (null, TouchingBoundary.range2, Inclusion.Exclusive), false),
            new("Range2Null", (TouchingBoundary.range1, null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new("OverlapExclusive", (Overlap.range1, Overlap.range2, Inclusion.Exclusive), true),
            new("TouchingInclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new("NoOverlapExclusive", (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new("TouchingExclusive", (TouchingBoundary.range1, TouchingBoundary.range2, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class Contains
    {
        public static readonly DateTimeRange Range = new(new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc));

        public static readonly DateTime MiddleValue = new(2024, 01, 05, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime BeforeStartValue = new(2023, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime StartBoundaryValue = new(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime EndBoundaryValue = new(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime OutsideValue = new(2024, 01, 11, 0, 0, 0, DateTimeKind.Utc);
        public static readonly DateTime LocalStartBoundaryValue = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
        public static readonly DateTime LocalEndBoundaryValue = new DateTime(2024, 01, 10, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();

        public static RuleScenario<(DateTimeRange? range, DateTime? value, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(LocalStartBoundaryValue), (Range, LocalStartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(LocalEndBoundaryValue), (Range, LocalEndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeRange? range, DateTime? value, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false),
            new("LocalStartBoundaryExclusive", (Range, LocalStartBoundaryValue, Inclusion.Exclusive), false),
            new("LocalEndBoundaryExclusive", (Range, LocalEndBoundaryValue, Inclusion.Exclusive), false),
            new("NullRange", (null, MiddleValue, Inclusion.Inclusive), false),
            new("NullValue", (Range, null, Inclusion.Inclusive), false)
        ];

        public static RuleScenario<(DateTimeRange? range, DateTime? value, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(DateTimeRange range, DateTime value, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(DateTimeRange range, DateTime value, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(DateTimeRange range, DateTime value, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }
}
