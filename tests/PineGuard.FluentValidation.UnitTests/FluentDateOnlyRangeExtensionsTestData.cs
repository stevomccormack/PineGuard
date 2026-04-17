using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRangeRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDateOnlyRangeExtensionsTestData
{
    public static class Chronological
    {
        public static TheoryData<FluentCase<DateOnlyRange?>> Cases =>
            F.IsChronological.AllScenarios
            .Select(s => new RuleScenario<DateOnlyRange?>(s.Name, s.Inputs.range, s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                "Null" => new FluentExpected(true),
                nameof(F.IsChronological.SameDayInclusive) => new FluentExpected(false, "Value must be chronological."),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be chronological.")
            });
    }

    public static class Overlapping
    {
        public static TheoryData<FluentCase<(DateOnlyRange? value, DateOnlyRange other)>> Cases =>
            F.IsOverlapping.AllScenarios
            .Where(s => s.Inputs.range2.HasValue)
            .Select(s => new RuleScenario<(DateOnlyRange? value, DateOnlyRange other)>(s.Name, (s.Inputs.range1, s.Inputs.range2!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                "Range1Null" => new FluentExpected(true),
                "TouchingInclusive" => new FluentExpected(false, "Value must be overlapping."),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must be overlapping.")
            });
    }

    public static class NotOverlapping
    {
        public static TheoryData<FluentCase<(DateOnlyRange? value, DateOnlyRange other)>> Cases =>
            F.IsOverlapping.AllScenarios
            .Where(s => s.Inputs.range2.HasValue)
            .Select(s => new RuleScenario<(DateOnlyRange? value, DateOnlyRange other)>(s.Name, (s.Inputs.range1, s.Inputs.range2!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                "Range1Null" => new FluentExpected(true),
                "TouchingInclusive" => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(false, "Value must not be overlapping."),
                _ => new FluentExpected(true)
            });
    }

    public static class Contains
    {
        public static TheoryData<FluentCase<(DateOnlyRange? value, DateOnly item)>> Cases =>
            F.Contains.AllScenarios
            .Where(s => s.Inputs.value.HasValue)
            .Select(s => new RuleScenario<(DateOnlyRange? value, DateOnly item)>(s.Name, (s.Inputs.range, s.Inputs.value!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                "NullRange" => new FluentExpected(true),
                "StartBoundaryExclusive" => new FluentExpected(true),
                "EndBoundaryExclusive" => new FluentExpected(true),
                _ when s.IsValid => new FluentExpected(true),
                _ => new FluentExpected(false, "Value must contain the specified date.")
            });
    }

    public static class NotContains
    {
        public static TheoryData<FluentCase<(DateOnlyRange? value, DateOnly item)>> Cases =>
            F.Contains.AllScenarios
            .Where(s => s.Inputs.value.HasValue)
            .Select(s => new RuleScenario<(DateOnlyRange? value, DateOnly item)>(s.Name, (s.Inputs.range, s.Inputs.value!.Value), s.IsValid)).ToArray()
            .ToFluentCases(s => s.Name switch
            {
                "NullRange" => new FluentExpected(true),
                "StartBoundaryExclusive" => new FluentExpected(false, "Value must not contain the specified date."),
                "EndBoundaryExclusive" => new FluentExpected(false, "Value must not contain the specified date."),
                _ when s.IsValid => new FluentExpected(false, "Value must not contain the specified date."),
                _ => new FluentExpected(true)
            });
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    private static readonly DateOnly Nn1 = new(2020, 1, 1);
    private static readonly DateOnly Nn2 = new(2020, 6, 1);
    private static readonly DateOnly Nn3 = new(2020, 12, 1);
    private static readonly DateOnlyRange NnRangeChronological = new(Nn1, Nn3);  // wide range: Nn1 < Nn3, strictly chronological
    private static readonly DateOnlyRange NnRangeSameDay = new(Nn1, Nn1);        // start == end → invalid for Exclusive chronological
    private static readonly DateOnlyRange NnRangeA = new(Nn1, Nn2);              // [Nn1, Nn2]
    private static readonly DateOnlyRange NnRangeB = new(Nn2, Nn3);              // [Nn2, Nn3] — touching at Nn2 (exclusive → no overlap? test below)
    private static readonly DateOnlyRange NnRangeAWide = new(Nn1, Nn3);          // [Nn1, Nn3] overlaps with NnRangeB = [Nn2, Nn3] exclusively
    private static readonly DateOnlyRange NnRangeNoOverlap = new(Nn3, Nn3);      // single day far from Nn1..Nn2

    public static class ChronologicalNonNullable
    {
        public static TheoryData<FluentCase<DateOnlyRange>> Cases =>
        [
            new("Chronological", NnRangeChronological, new FluentExpected(true)),
            new("Same day",      NnRangeSameDay,        new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class OverlappingNonNullable
    {
        public static TheoryData<FluentCase<(DateOnlyRange value, DateOnlyRange other)>> Cases =>
        [
            new("Overlapping",     (NnRangeAWide,    NnRangeB),        new FluentExpected(true)),
            new("Not overlapping", (NnRangeA,        NnRangeNoOverlap), new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlappingNonNullable
    {
        public static TheoryData<FluentCase<(DateOnlyRange value, DateOnlyRange other)>> Cases =>
        [
            new("Not overlapping", (NnRangeA,     NnRangeNoOverlap), new FluentExpected(true)),
            new("Overlapping",     (NnRangeAWide, NnRangeB),         new FluentExpected(false, "Value must not be overlapping."))
        ];
    }

    public static class ContainsNonNullable
    {
        public static TheoryData<FluentCase<(DateOnlyRange value, DateOnly item)>> Cases =>
        [
            new("Contains",          (NnRangeChronological, Nn1), new FluentExpected(true)),
            new("Does not contain",  (NnRangeA,             Nn3), new FluentExpected(false, "Value must contain the specified date."))
        ];
    }

    public static class NotContainsNonNullable
    {
        public static TheoryData<FluentCase<(DateOnlyRange value, DateOnly item)>> Cases =>
        [
            new("Does not contain", (NnRangeA,             Nn3), new FluentExpected(true)),
            new("Contains",         (NnRangeChronological, Nn1), new FluentExpected(false, "Value must not contain the specified date."))
        ];
    }
}
