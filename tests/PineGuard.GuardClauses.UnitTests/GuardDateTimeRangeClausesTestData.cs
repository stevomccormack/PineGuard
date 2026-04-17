using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.DateTimeRangeRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDateTimeRangeClausesTestData
{
    public static class NotChronological
    {
        public static TheoryData<GuardCase<(DateTimeRange range, Inclusion inclusion)>> Cases =>
        [
            new(nameof(F.IsChronological.StartBeforeEndExclusive), (F.IsChronological.StartBeforeEndExclusive, Inclusion.Exclusive), new GuardExpected(true)),
            new(nameof(F.IsChronological.StartEqualsEndInclusive), (F.IsChronological.StartEqualsEndInclusive, Inclusion.Inclusive), new GuardExpected(true)),
            new(nameof(F.IsChronological.StartEqualsEndExclusive), (F.IsChronological.StartEqualsEndExclusive, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "range"))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<GuardCase<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)>> Cases =>
        [
            new("NoOverlapExclusive", (F.IsOverlapping.NoOverlap.range1, F.IsOverlapping.NoOverlap.range2, Inclusion.Exclusive), new GuardExpected(true)),
            new("TouchingExclusive", (F.IsOverlapping.TouchingBoundary.range1, F.IsOverlapping.TouchingBoundary.range2, Inclusion.Exclusive), new GuardExpected(true)),
            new("OverlapExclusive", (F.IsOverlapping.Overlap.range1, F.IsOverlapping.Overlap.range2, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "range1")),
            new("TouchingInclusive", (F.IsOverlapping.TouchingBoundary.range1, F.IsOverlapping.TouchingBoundary.range2, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "range1"))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<GuardCase<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)>> Cases =>
        [
            new("OverlapExclusive", (F.IsOverlapping.Overlap.range1, F.IsOverlapping.Overlap.range2, Inclusion.Exclusive), new GuardExpected(true)),
            new("TouchingInclusive", (F.IsOverlapping.TouchingBoundary.range1, F.IsOverlapping.TouchingBoundary.range2, Inclusion.Inclusive), new GuardExpected(true)),
            new("NoOverlapExclusive", (F.IsOverlapping.NoOverlap.range1, F.IsOverlapping.NoOverlap.range2, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "range1")),
            new("TouchingExclusive", (F.IsOverlapping.TouchingBoundary.range1, F.IsOverlapping.TouchingBoundary.range2, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "range1"))
        ];
    }

    public static class NotContains
    {
        public static TheoryData<GuardCase<(DateTimeRange range, DateTime value, Inclusion inclusion)>> Cases =>
        [
            new(nameof(F.Contains.MiddleValue), (F.Contains.Range, F.Contains.MiddleValue, Inclusion.Inclusive), new GuardExpected(true)),
            new(nameof(F.Contains.StartBoundaryValue), (F.Contains.Range, F.Contains.StartBoundaryValue, Inclusion.Inclusive), new GuardExpected(true)),
            new(nameof(F.Contains.EndBoundaryValue), (F.Contains.Range, F.Contains.EndBoundaryValue, Inclusion.Inclusive), new GuardExpected(true)),
            new(nameof(F.Contains.BeforeStartValue), (F.Contains.Range, F.Contains.BeforeStartValue, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "range")),
            new(nameof(F.Contains.OutsideValue), (F.Contains.Range, F.Contains.OutsideValue, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "range")),
            new("StartBoundaryExclusive", (F.Contains.Range, F.Contains.StartBoundaryValue, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "range")),
            new("EndBoundaryExclusive", (F.Contains.Range, F.Contains.EndBoundaryValue, Inclusion.Exclusive), new GuardExpected(false, typeof(ArgumentException), "range"))
        ];
    }

    public static class Contains
    {
        public static TheoryData<GuardCase<(DateTimeRange range, DateTime value, Inclusion inclusion)>> Cases =>
        [
            new(nameof(F.Contains.BeforeStartValue), (F.Contains.Range, F.Contains.BeforeStartValue, Inclusion.Inclusive), new GuardExpected(true)),
            new(nameof(F.Contains.OutsideValue), (F.Contains.Range, F.Contains.OutsideValue, Inclusion.Inclusive), new GuardExpected(true)),
            new("StartBoundaryExclusive", (F.Contains.Range, F.Contains.StartBoundaryValue, Inclusion.Exclusive), new GuardExpected(true)),
            new("EndBoundaryExclusive", (F.Contains.Range, F.Contains.EndBoundaryValue, Inclusion.Exclusive), new GuardExpected(true)),
            new(nameof(F.Contains.MiddleValue), (F.Contains.Range, F.Contains.MiddleValue, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "range")),
            new(nameof(F.Contains.StartBoundaryValue), (F.Contains.Range, F.Contains.StartBoundaryValue, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "range")),
            new(nameof(F.Contains.EndBoundaryValue), (F.Contains.Range, F.Contains.EndBoundaryValue, Inclusion.Inclusive), new GuardExpected(false, typeof(ArgumentException), "range"))
        ];
    }
}
