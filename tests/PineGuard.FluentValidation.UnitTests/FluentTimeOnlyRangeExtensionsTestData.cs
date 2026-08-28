using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using FR = PineGuard.Testing.Fixtures.TimeOnlyRangeRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentTimeOnlyRangeExtensionsTestData
{
    public static class Chronological
    {
        public static TheoryData<FluentCase<TimeOnlyRange?>> Cases =>
        [
            new(nameof(FR.IsChronological.Chronological), FR.IsChronological.Chronological, new FluentExpected(true)),
            new(nameof(FR.IsChronological.EqualExclusive), FR.IsChronological.EqualExclusive, new FluentExpected(false, "Value must be chronological.")),
            new("Null", null, new FluentExpected(true))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<FluentCase<(TimeOnlyRange? range1, TimeOnlyRange range2)>> Cases =>
        [
            new(nameof(FR.IsOverlapping.OverlapExclusive), (FR.IsOverlapping.OverlapExclusive.range1, FR.IsOverlapping.OverlapExclusive.range2), new FluentExpected(true)),
            new(nameof(FR.IsOverlapping.NoOverlap), (FR.IsOverlapping.NoOverlap.range1, FR.IsOverlapping.NoOverlap.range2), new FluentExpected(false, "Value must be overlapping.")),
            new("Null", (null, default), new FluentExpected(true))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<FluentCase<(TimeOnlyRange? range1, TimeOnlyRange range2)>> Cases =>
        [
            new(nameof(FR.IsOverlapping.NoOverlap), (FR.IsOverlapping.NoOverlap.range1, FR.IsOverlapping.NoOverlap.range2), new FluentExpected(true)),
            new(nameof(FR.IsOverlapping.OverlapExclusive), (FR.IsOverlapping.OverlapExclusive.range1, FR.IsOverlapping.OverlapExclusive.range2), new FluentExpected(false, "Value must not be overlapping.")),
            new("Null", (null, default), new FluentExpected(true))
        ];
    }

    public static class Contains
    {
        public static TheoryData<FluentCase<(TimeOnlyRange? range, TimeOnly value)>> Cases =>
        [
            new(nameof(FR.Contains.MiddleValue), (FR.Contains.Range, FR.Contains.MiddleValue), new FluentExpected(true)),
            new(nameof(FR.Contains.BeforeStartValue), (FR.Contains.Range, FR.Contains.BeforeStartValue), new FluentExpected(false, "Value must contain the specified time.")),
            new("Null", (null, FR.Contains.MiddleValue), new FluentExpected(true))
        ];
    }

    public static class NotContains
    {
        public static TheoryData<FluentCase<(TimeOnlyRange? range, TimeOnly value)>> Cases =>
        [
            new(nameof(FR.Contains.BeforeStartValue), (FR.Contains.Range, FR.Contains.BeforeStartValue), new FluentExpected(true)),
            new(nameof(FR.Contains.MiddleValue), (FR.Contains.Range, FR.Contains.MiddleValue), new FluentExpected(false, "Value must not contain the specified time.")),
            new("Null", (null, FR.Contains.MiddleValue), new FluentExpected(true))
        ];
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    public static class ChronologicalNonNullable
    {
        public static TheoryData<FluentCase<TimeOnlyRange>> Cases =>
        [
            new(nameof(FR.IsChronological.Chronological), FR.IsChronological.Chronological,   new FluentExpected(true)),
            new(nameof(FR.IsChronological.EqualExclusive), FR.IsChronological.EqualExclusive, new FluentExpected(false, "Value must be chronological.", Code: MustCodes.Range.Order.NotChronological))
        ];
    }

    public static class OverlappingNonNullable
    {
        public static TheoryData<FluentCase<(TimeOnlyRange range1, TimeOnlyRange range2)>> Cases =>
        [
            new(nameof(FR.IsOverlapping.OverlapExclusive), (FR.IsOverlapping.OverlapExclusive.range1, FR.IsOverlapping.OverlapExclusive.range2), new FluentExpected(true)),
            new(nameof(FR.IsOverlapping.NoOverlap),        (FR.IsOverlapping.NoOverlap.range1,        FR.IsOverlapping.NoOverlap.range2),        new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlappingNonNullable
    {
        public static TheoryData<FluentCase<(TimeOnlyRange range1, TimeOnlyRange range2)>> Cases =>
        [
            new(nameof(FR.IsOverlapping.NoOverlap),        (FR.IsOverlapping.NoOverlap.range1,        FR.IsOverlapping.NoOverlap.range2),        new FluentExpected(true)),
            new(nameof(FR.IsOverlapping.OverlapExclusive), (FR.IsOverlapping.OverlapExclusive.range1, FR.IsOverlapping.OverlapExclusive.range2), new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class ContainsNonNullable
    {
        public static TheoryData<FluentCase<(TimeOnlyRange range, TimeOnly value)>> Cases =>
        [
            new(nameof(FR.Contains.MiddleValue),       (FR.Contains.Range, FR.Contains.MiddleValue),       new FluentExpected(true)),
            new(nameof(FR.Contains.BeforeStartValue),  (FR.Contains.Range, FR.Contains.BeforeStartValue),  new FluentExpected(false, "Value must contain the specified time."))
        ];
    }

    public static class NotContainsNonNullable
    {
        public static TheoryData<FluentCase<(TimeOnlyRange range, TimeOnly value)>> Cases =>
        [
            new(nameof(FR.Contains.BeforeStartValue),  (FR.Contains.Range, FR.Contains.BeforeStartValue),  new FluentExpected(true)),
            new(nameof(FR.Contains.MiddleValue),       (FR.Contains.Range, FR.Contains.MiddleValue),       new FluentExpected(false, "Value must not contain the specified time."))
        ];
    }
}
