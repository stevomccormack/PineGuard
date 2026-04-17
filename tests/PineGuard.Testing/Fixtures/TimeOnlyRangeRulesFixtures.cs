using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class TimeOnlyRangeRulesFixtures
{
    public static class IsChronological
    {
        public static readonly TimeOnlyRange Chronological = new(new TimeOnly(12, 0), new TimeOnly(13, 0));
        public static readonly TimeOnlyRange EqualExclusive = new(new TimeOnly(12, 0), new TimeOnly(12, 0));
        public static readonly TimeOnlyRange EqualInclusive = new(new TimeOnly(12, 0), new TimeOnly(12, 0));

        public static RuleScenario<(TimeOnlyRange? range, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(Chronological), (Chronological, Inclusion.Exclusive), true),
            new(nameof(EqualInclusive), (EqualInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(TimeOnlyRange? range, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(EqualExclusive), (EqualExclusive, Inclusion.Exclusive), false),
            new("Null", (null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(TimeOnlyRange? range, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(TimeOnlyRange range, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(Chronological), (Chronological, Inclusion.Exclusive), true),
            new(nameof(EqualInclusive), (EqualInclusive, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(TimeOnlyRange range, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(EqualExclusive), (EqualExclusive, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(TimeOnlyRange range, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class IsOverlapping
    {
        public static readonly (TimeOnlyRange range1, TimeOnlyRange range2) OverlapExclusive = (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(9, 30), new TimeOnly(9, 45)));
        public static readonly (TimeOnlyRange range1, TimeOnlyRange range2) TouchingExclusive = (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0)));
        public static readonly (TimeOnlyRange range1, TimeOnlyRange range2) TouchingInclusive = (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0)));
        public static readonly (TimeOnlyRange range1, TimeOnlyRange range2) NoOverlap = (new TimeOnlyRange(new TimeOnly(9, 0), new TimeOnly(10, 0)), new TimeOnlyRange(new TimeOnly(11, 0), new TimeOnly(12, 0)));

        public static RuleScenario<(TimeOnlyRange? range1, TimeOnlyRange? range2, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(OverlapExclusive), (OverlapExclusive.range1, OverlapExclusive.range2, Inclusion.Exclusive), true),
            new(nameof(TouchingInclusive), (TouchingInclusive.range1, TouchingInclusive.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(TimeOnlyRange? range1, TimeOnlyRange? range2, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(NoOverlap), (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new(nameof(TouchingExclusive), (TouchingExclusive.range1, TouchingExclusive.range2, Inclusion.Exclusive), false),
            new("Range1Null", (null, OverlapExclusive.range2, Inclusion.Exclusive), false),
            new("Range2Null", (OverlapExclusive.range1, null, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(TimeOnlyRange? range1, TimeOnlyRange? range2, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(OverlapExclusive), (OverlapExclusive.range1, OverlapExclusive.range2, Inclusion.Exclusive), true),
            new(nameof(TouchingInclusive), (TouchingInclusive.range1, TouchingInclusive.range2, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(NoOverlap), (NoOverlap.range1, NoOverlap.range2, Inclusion.Exclusive), false),
            new(nameof(TouchingExclusive), (TouchingExclusive.range1, TouchingExclusive.range2, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }

    public static class Contains
    {
        public static readonly TimeOnlyRange Range = new(new TimeOnly(9, 0), new TimeOnly(10, 0));

        public static readonly TimeOnly MiddleValue = new(9, 30);
        public static readonly TimeOnly BeforeStartValue = new(8, 59);
        public static readonly TimeOnly StartBoundaryValue = new(9, 0);
        public static readonly TimeOnly EndBoundaryValue = new(10, 0);
        public static readonly TimeOnly OutsideValue = new(10, 1);

        public static RuleScenario<(TimeOnlyRange? range, TimeOnly? value, Inclusion inclusion)>[] ValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(TimeOnlyRange? range, TimeOnly? value, Inclusion inclusion)>[] InvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false),
            new("NullRange", (null, MiddleValue, Inclusion.Inclusive), false),
            new("NullValue", (Range, null, Inclusion.Inclusive), false)
        ];

        public static RuleScenario<(TimeOnlyRange? range, TimeOnly? value, Inclusion inclusion)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        public static RuleScenario<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>[] NonNullValidScenarios =>
        [
            new(nameof(MiddleValue), (Range, MiddleValue, Inclusion.Inclusive), true),
            new(nameof(StartBoundaryValue), (Range, StartBoundaryValue, Inclusion.Inclusive), true),
            new(nameof(EndBoundaryValue), (Range, EndBoundaryValue, Inclusion.Inclusive), true)
        ];

        public static RuleScenario<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>[] NonNullInvalidScenarios =>
        [
            new(nameof(BeforeStartValue), (Range, BeforeStartValue, Inclusion.Inclusive), false),
            new(nameof(OutsideValue), (Range, OutsideValue, Inclusion.Inclusive), false),
            new("StartBoundaryExclusive", (Range, StartBoundaryValue, Inclusion.Exclusive), false),
            new("EndBoundaryExclusive", (Range, EndBoundaryValue, Inclusion.Exclusive), false)
        ];

        public static RuleScenario<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)>[] AllNonNullScenarios => [.. NonNullValidScenarios, .. NonNullInvalidScenarios];
    }
}
