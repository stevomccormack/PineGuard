using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentTimeOnlyExtensionsTestData
{
    private static readonly TimeOnly Ref = F.IsKnownTimes.T1200!.Value;
    private static readonly TimeOnly RefMinus1 = F.IsKnownTimes.T1100!.Value;
    private static readonly TimeOnly RefMinus2 = F.IsKnownTimes.T1000!.Value;
    private static readonly TimeOnly RefPlus1 = F.IsKnownTimes.T1300!.Value;

    public static class Between
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("inside", Ref, new FluentExpected(true)),
            new("outside", RefMinus2, new FluentExpected(false, "Value must be within the expected range.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotBetween
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("outside", RefMinus2, new FluentExpected(true)),
            new("inside", Ref, new FluentExpected(false, "Value must not be within the expected range.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Before
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("after", RefPlus1, new FluentExpected(false, "Value must be before the specified time.")),
            new("same", Ref, new FluentExpected(false, "Value must be before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotBefore
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("after", RefPlus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must not be before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class OnOrBefore
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(true)),
            new("after", RefPlus1, new FluentExpected(false, "Value must be on or before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotOnOrBefore
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("after", RefPlus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(false, "Value must not be on or before the specified time.")),
            new("before", RefMinus1, new FluentExpected(false, "Value must not be on or before the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class After
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("after", RefPlus1, new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must be after the specified time.")),
            new("same", Ref, new FluentExpected(false, "Value must be after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotAfter
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(true)),
            new("after", RefPlus1, new FluentExpected(false, "Value must not be after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class OnOrAfter
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("after", RefPlus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must be on or after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotOnOrAfter
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(false, "Value must not be on or after the specified time.")),
            new("after", RefPlus1, new FluentExpected(false, "Value must not be on or after the specified time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Same
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("same", Ref, new FluentExpected(true)),
            new("different", RefMinus1, new FluentExpected(false, "Value must be the same time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotSame
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("different", RefMinus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(false, "Value must not be the same time.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Within
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("within", Ref, new FluentExpected(true)),
            new("outside", RefPlus1, new FluentExpected(false, "Value must be within the expected time window.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotWithin
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("outside", RefPlus1, new FluentExpected(true)),
            new("within", Ref, new FluentExpected(false, "Value must not be within the expected time window.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Chronological
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("after", RefPlus1, new FluentExpected(false, "Value must be chronological.")),
            new("same", Ref, new FluentExpected(false, "Value must be chronological.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotChronological
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("after", RefPlus1, new FluentExpected(true)),
            new("same", Ref, new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must not be chronological.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Overlapping
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("overlapping", F.IsKnownTimes.T0830!.Value, new FluentExpected(true)),
            new("disjoint", F.IsKnownTimes.T0930!.Value, new FluentExpected(false, "Value must be overlapping.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotOverlapping
    {
        public static TheoryData<FluentCase<TimeOnly?>> Cases =>
        [
            new("disjoint", F.IsKnownTimes.T0930!.Value, new FluentExpected(true)),
            new("overlapping", F.IsKnownTimes.T0830!.Value, new FluentExpected(false, "Value must not be overlapping.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    // ── Non-nullable overloads ─────────────────────────────────────────────

    public static class BetweenNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("inside",  Ref,      new FluentExpected(true)),
            new("outside", RefMinus2, new FluentExpected(false, "Value must be within the expected range."))
        ];
    }

    public static class NotBetweenNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("outside", RefMinus2, new FluentExpected(true)),
            new("inside",  Ref,       new FluentExpected(false, "Value must not be within the expected range."))
        ];
    }

    public static class BeforeNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("after",  RefPlus1,  new FluentExpected(false, "Value must be before the specified time."))
        ];
    }

    public static class NotBeforeNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("after",  RefPlus1,  new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must not be before the specified time."))
        ];
    }

    public static class OnOrBeforeNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("same",  Ref,      new FluentExpected(true)),
            new("after", RefPlus1, new FluentExpected(false, "Value must be on or before the specified time."))
        ];
    }

    public static class NotOnOrBeforeNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("after", RefPlus1,  new FluentExpected(true)),
            new("same",  Ref,       new FluentExpected(false, "Value must not be on or before the specified time."))
        ];
    }

    public static class AfterNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("after",  RefPlus1,  new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must be after the specified time."))
        ];
    }

    public static class NotAfterNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("same",  Ref,      new FluentExpected(true)),
            new("after", RefPlus1, new FluentExpected(false, "Value must not be after the specified time."))
        ];
    }

    public static class OnOrAfterNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("same",   Ref,      new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must be on or after the specified time."))
        ];
    }

    public static class NotOnOrAfterNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("same",   Ref,       new FluentExpected(false, "Value must not be on or after the specified time."))
        ];
    }

    public static class SameNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("same",      Ref,      new FluentExpected(true)),
            new("different", RefMinus1, new FluentExpected(false, "Value must be the same time."))
        ];
    }

    public static class NotSameNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("different", RefMinus1, new FluentExpected(true)),
            new("same",      Ref,       new FluentExpected(false, "Value must not be the same time."))
        ];
    }

    public static class WithinNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("within",  Ref,      new FluentExpected(true)),
            new("outside", RefPlus1, new FluentExpected(false, "Value must be within the expected time window."))
        ];
    }

    public static class NotWithinNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("outside", RefPlus1, new FluentExpected(true)),
            new("within",  Ref,      new FluentExpected(false, "Value must not be within the expected time window."))
        ];
    }

    public static class ChronologicalNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("before", RefMinus1, new FluentExpected(true)),
            new("after",  RefPlus1,  new FluentExpected(false, "Value must be chronological."))
        ];
    }

    public static class NotChronologicalNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("after",  RefPlus1,  new FluentExpected(true)),
            new("before", RefMinus1, new FluentExpected(false, "Value must not be chronological."))
        ];
    }

    public static class OverlappingNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("overlapping", F.IsKnownTimes.T0830!.Value, new FluentExpected(true)),
            new("disjoint",    F.IsKnownTimes.T0930!.Value, new FluentExpected(false, "Value must be overlapping."))
        ];
    }

    public static class NotOverlappingNonNullable
    {
        public static TheoryData<FluentCase<TimeOnly>> Cases =>
        [
            new("disjoint",    F.IsKnownTimes.T0930!.Value, new FluentExpected(true)),
            new("overlapping", F.IsKnownTimes.T0830!.Value, new FluentExpected(false, "Value must not be overlapping."))
        ];
    }
}
