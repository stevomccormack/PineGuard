using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDateTimeClausesTestData
{
    private static readonly DateTime PastDate = new(2000, 1, 10, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime FutureDate = new(2099, 1, 10, 0, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime LeapDayBirth = new(2008, 02, 29, 0, 0, 0, DateTimeKind.Utc);

    // Guard.Against.FutureOrPresent — throws when value IS future-or-present; passes when past
    // Delegates to Must.Be.Past
    public static class FutureOrPresent
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
        [
            new("past", PastDate, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
        [
            new("future", FutureDate, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Future — throws when value IS in the future; passes when past-or-present
    // Delegates to Must.Be.PastOrPresent
    public static class Future
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
        [
            new("past", PastDate, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
        [
            new("future", FutureDate, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.PastOrPresent — throws when value IS past-or-present; passes when future
    // Delegates to Must.Be.Future
    public static class PastOrPresent
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
        [
            new("future", FutureDate, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
        [
            new("past", PastDate, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Past — throws when value IS past; passes when future-or-present
    // Delegates to Must.Be.FutureOrPresent
    public static class Past
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
        [
            new("future", FutureDate, new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
        [
            new("past", PastDate, new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotBetween — throws when value is NOT between; passes when it IS between
    // Delegates to Must.Be.Between
    public static class NotBetween
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime min, DateTime max, Inclusion inclusion)>> ValidCases =>
            F.IsBetween.ValidScenarios.Except("NullValue").Project(t => (t.value!.Value, t.min, t.max, t.inclusion)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime min, DateTime max, Inclusion inclusion)>> InvalidCases =>
            F.IsBetween.InvalidScenarios.Except("NullValue").Project(t => (t.value!.Value, t.min, t.max, t.inclusion)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Between — throws when value IS between; passes when NOT between
    // Delegates to Must.Be.NotBetween (complement)
    public static class Between
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime min, DateTime max, Inclusion inclusion)>> ValidCases =>
            F.IsBetween.InvalidScenarios.Except("NullValue").Project(t => (t.value!.Value, t.min, t.max, t.inclusion)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime min, DateTime max, Inclusion inclusion)>> InvalidCases =>
            F.IsBetween.ValidScenarios.Except("NullValue").Project(t => (t.value!.Value, t.min, t.max, t.inclusion)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.OnOrAfter — throws when value IS on-or-after; passes when before
    // Delegates to Must.Be.Before (Exclusive, no precision). SameInstantInclusive throws because Must.Be.Before(Exclusive) fails for same instant.
    public static class OnOrAfter
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
            F.IsBefore.ValidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown", "SameInstantInclusive").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
        [
            ..F.IsBefore.InvalidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new("SameInstantInclusive", (F.IsBefore.SameInstantInclusive.value!.Value, F.IsBefore.SameInstantInclusive.other!.Value), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.After — throws when value IS after; passes when on-or-before
    // Delegates to Must.Be.OnOrBefore (Inclusive, no precision). SameInstantInclusive passes because Must.Be.OnOrBefore(Inclusive) succeeds for same instant.
    public static class After
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
        [
            ..F.IsAfter.InvalidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true)),
            new("SameInstantInclusive", (F.IsAfter.SameInstantInclusive.value!.Value, F.IsAfter.SameInstantInclusive.other!.Value), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
            F.IsAfter.ValidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown", "SameInstantInclusive").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.OnOrBefore — throws when value IS on-or-before; passes when after
    // Delegates to Must.Be.After (Exclusive, no precision). SameInstantInclusive throws because Must.Be.After(Exclusive) fails for same instant.
    public static class OnOrBefore
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
            F.IsAfter.ValidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown", "SameInstantInclusive").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
        [
            ..F.IsAfter.InvalidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new("SameInstantInclusive", (F.IsAfter.SameInstantInclusive.value!.Value, F.IsAfter.SameInstantInclusive.other!.Value), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Before — throws when value IS before; passes when on-or-after
    // Delegates to Must.Be.OnOrAfter (Inclusive, no precision). SameInstantInclusive passes because Must.Be.OnOrAfter(Inclusive) succeeds for same instant.
    public static class Before
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
        [
            ..F.IsBefore.InvalidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true)),
            new("SameInstantInclusive", (F.IsBefore.SameInstantInclusive.value!.Value, F.IsBefore.SameInstantInclusive.other!.Value), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
            F.IsBefore.ValidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown", "SameInstantInclusive").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotSame — throws when value IS not-same; passes when same
    // Delegates to Must.Be.Same (no precision). PrecisionHour: different when no precision → guard throws. PrecisionUnknown: same values → guard passes.
    public static class NotSame
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
        [
            ..F.IsSame.ValidScenarios.Except("NullValue", "NullOther", "BothNull", "PrecisionHour").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true)),
            new("PrecisionUnknown", (F.IsSame.PrecisionUnknown.value!.Value, F.IsSame.PrecisionUnknown.other!.Value), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
        [
            ..F.IsSame.InvalidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new("PrecisionHour", (F.IsSame.PrecisionHour.value!.Value, F.IsSame.PrecisionHour.other!.Value), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.Same — throws when value IS same; passes when not-same
    // Delegates to Must.Be.NotSame (no precision). PrecisionHour: different when no precision → guard passes. PrecisionUnknown: same values → guard throws.
    public static class Same
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
        [
            ..F.IsSame.InvalidScenarios.Except("NullValue", "NullOther", "PrecisionUnknown").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true)),
            new("PrecisionHour", (F.IsSame.PrecisionHour.value!.Value, F.IsSame.PrecisionHour.other!.Value), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
        [
            ..F.IsSame.ValidScenarios.Except("NullValue", "NullOther", "BothNull", "PrecisionHour").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new("PrecisionUnknown", (F.IsSame.PrecisionUnknown.value!.Value, F.IsSame.PrecisionUnknown.other!.Value), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotChronological — throws when NOT chronological; passes when chronological
    // Delegates to Must.Be.Chronological
    public static class NotChronological
    {
        public static TheoryData<GuardCase<(DateTime start, DateTime end, Inclusion inclusion)>> ValidCases =>
            F.IsChronological.ValidScenarios.Except("BothNull", "StartNullEndSet", "StartSetEndNull").Project(t => (t.start!.Value, t.end!.Value, t.inclusion)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime start, DateTime end, Inclusion inclusion)>> InvalidCases =>
            F.IsChronological.InvalidScenarios.Except("BothNull", "StartNullEndSet", "StartSetEndNull").Project(t => (t.start!.Value, t.end!.Value, t.inclusion)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "start"));
    }

    // Guard.Against.Overlapping — throws when overlapping; passes when NOT overlapping
    // Delegates to Must.Be.NotOverlapping (complement)
    public static class Overlapping
    {
        public static TheoryData<GuardCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2, Inclusion inclusion)>> ValidCases =>
            F.IsOverlapping.InvalidScenarios.Except("AllNull", "End1Null", "Start2Null", "End2Null").Project(t => (t.start1!.Value, t.end1!.Value, t.start2!.Value, t.end2!.Value, t.inclusion)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2, Inclusion inclusion)>> InvalidCases =>
            F.IsOverlapping.ValidScenarios.Except("AllNull", "End1Null", "Start2Null", "End2Null").Project(t => (t.start1!.Value, t.end1!.Value, t.start2!.Value, t.end2!.Value, t.inclusion)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "start1"));
    }

    // Guard.Against.NotOverlapping — throws when NOT overlapping; passes when overlapping
    // Delegates to Must.Be.Overlapping (complement)
    public static class NotOverlapping
    {
        public static TheoryData<GuardCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2, Inclusion inclusion)>> ValidCases =>
            F.IsOverlapping.ValidScenarios.Except("AllNull", "End1Null", "Start2Null", "End2Null").Project(t => (t.start1!.Value, t.end1!.Value, t.start2!.Value, t.end2!.Value, t.inclusion)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2, Inclusion inclusion)>> InvalidCases =>
            F.IsOverlapping.InvalidScenarios.Except("AllNull", "End1Null", "Start2Null", "End2Null").Project(t => (t.start1!.Value, t.end1!.Value, t.start2!.Value, t.end2!.Value, t.inclusion)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "start1"));
    }

    // Guard.Against.NotWithin — throws when NOT within; passes when within
    // Delegates to Must.Be.Within
    public static class NotWithin
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, TimeSpan window)>> ValidCases =>
            F.IsWithin.ValidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.window)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, TimeSpan window)>> InvalidCases =>
            F.IsWithin.InvalidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.window)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Within — throws when within; passes when NOT within
    // Delegates to Must.Be.NotWithin (complement)
    public static class Within
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, TimeSpan window)>> ValidCases =>
            F.IsWithin.InvalidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.window)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, TimeSpan window)>> InvalidCases =>
            F.IsWithin.ValidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.window)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotWithinDaysFromNow — throws when NOT within days from now; passes when within
    // Delegates to Must.Be.WithinDaysFromNow
    public static class NotWithinDaysFromNow
    {
        public static TheoryData<GuardCase<(DateTime value, int days)>> ValidCases =>
        [
            new("within", (new DateTime(2099, 1, 10, 0, 0, 0, DateTimeKind.Utc), 36525), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateTime value, int days)>> InvalidCases =>
        [
            new("not-within", (new DateTime(2000, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.WithinDaysFromNow — throws when within days from now; passes when NOT within
    // Delegates to Must.Be.NotWithinDaysFromNow (complement)
    public static class WithinDaysFromNow
    {
        public static TheoryData<GuardCase<(DateTime value, int days)>> ValidCases =>
        [
            new("not-within", (new DateTime(2000, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1), new GuardExpected(true))
        ];
        public static TheoryData<GuardCase<(DateTime value, int days)>> InvalidCases =>
        [
            new("within", (new DateTime(2099, 1, 10, 0, 0, 0, DateTimeKind.Utc), 36525), new GuardExpected(false, typeof(ArgumentException), "value"))
        ];
    }

    // Guard.Against.NotWithinCalendarMonths — throws when NOT within calendar months; passes when within
    // Delegates to Must.Be.WithinCalendarMonths
    public static class NotWithinCalendarMonths
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, int months)>> ValidCases =>
            F.IsWithinCalendarMonths.ValidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.months)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, int months)>> InvalidCases =>
            F.IsWithinCalendarMonths.InvalidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.months)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.WithinCalendarMonths — throws when within calendar months; passes when NOT within
    // Delegates to Must.Be.NotWithinCalendarMonths (complement)
    public static class WithinCalendarMonths
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, int months)>> ValidCases =>
            F.IsWithinCalendarMonths.InvalidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.months)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime reference, int months)>> InvalidCases =>
            F.IsWithinCalendarMonths.ValidScenarios.Except("NullValue", "NullReference").Project(t => (t.value!.Value, t.reference!.Value, t.months)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Weekend — throws when weekend; passes when weekday
    // Delegates to Must.Be.Weekday
    public static class Weekend
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsWeekday.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsWeekday.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Weekday — throws when weekday; passes when weekend
    // Delegates to Must.Be.Weekend (complement)
    public static class Weekday
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsWeekend.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsWeekend.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotFirstDayOfMonth — throws when NOT first day; passes when first day
    // Delegates to Must.Be.FirstDayOfMonth
    public static class NotFirstDayOfMonth
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsFirstDayOfMonth.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsFirstDayOfMonth.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.FirstDayOfMonth — throws when first day; passes when NOT first day
    // Delegates to Must.Be.NotFirstDayOfMonth (complement)
    public static class FirstDayOfMonth
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsFirstDayOfMonth.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsFirstDayOfMonth.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotLastDayOfMonth — throws when NOT last day; passes when last day
    // Delegates to Must.Be.LastDayOfMonth
    public static class NotLastDayOfMonth
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsLastDayOfMonth.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsLastDayOfMonth.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.LastDayOfMonth — throws when last day; passes when NOT last day
    // Delegates to Must.Be.NotLastDayOfMonth (complement)
    public static class LastDayOfMonth
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsLastDayOfMonth.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsLastDayOfMonth.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotSameDay — throws when NOT same day; passes when same day
    // Delegates to Must.Be.SameDay
    public static class NotSameDay
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
            F.IsSameDay.ValidScenarios.Except("BothNull").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
            F.IsSameDay.InvalidScenarios.Except("NullValue", "NullOther").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.SameDay — throws when same day; passes when NOT same day
    // Delegates to Must.Be.NotSameDay (complement)
    public static class SameDay
    {
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> ValidCases =>
            F.IsSameDay.InvalidScenarios.Except("NullValue", "NullOther").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<(DateTime value, DateTime other)>> InvalidCases =>
            F.IsSameDay.ValidScenarios.Except("BothNull").Project(t => (t.value!.Value, t.other!.Value)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotUtc — throws when NOT utc; passes when utc
    // Delegates to Must.Be.Utc
    public static class NotUtc
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsUtc.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsUtc.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Utc — throws when utc; passes when NOT utc
    // Delegates to Must.Be.NotUtc (complement)
    public static class Utc
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsUtc.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsUtc.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotLocal — throws when NOT local; passes when local
    // Delegates to Must.Be.Local
    public static class NotLocal
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsLocal.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsLocal.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Local — throws when local; passes when NOT local
    // Delegates to Must.Be.NotLocal (complement)
    public static class Local
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsLocal.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsLocal.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotUnspecified — throws when NOT unspecified; passes when unspecified
    // Delegates to Must.Be.Unspecified
    public static class NotUnspecified
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsUnspecified.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsUnspecified.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Unspecified — throws when unspecified; passes when NOT unspecified
    // Delegates to Must.Be.NotUnspecified (complement)
    public static class Unspecified
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.IsUnspecified.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.IsUnspecified.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotExplicitKind — throws when NOT explicit kind; passes when explicit kind
    // Delegates to Must.Be.ExplicitKind
    public static class NotExplicitKind
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.HasExplicitKind.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.HasExplicitKind.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.ExplicitKind — throws when explicit kind; passes when NOT explicit kind
    // Delegates to Must.Be.NotExplicitKind (complement)
    public static class ExplicitKind
    {
        public static TheoryData<GuardCase<DateTime>> ValidCases =>
            F.HasExplicitKind.InvalidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<DateTime>> InvalidCases =>
            F.HasExplicitKind.ValidScenarios.Except("NullValue").Project(t => t!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.BelowMinimumAge — throws when the birth date does NOT meet the minimum age
    // Delegates to Must.Be.MinimumAge
    // The fixture's NullValue scenario is dropped: this overload takes a non-nullable DateTime.
    public static class BelowMinimumAge
    {
        public static TheoryData<GuardCase<(DateTime value, int years)>> ValidCases =>
            F.HasMinimumAge.AllValid.Project(v => (v.value!.Value, v.years)).ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTime value, int years)>> InvalidCases =>
            F.HasMinimumAge.AllInvalid.Except(nameof(F.HasMinimumAge.NullValue)).Project(v => (v.value!.Value, v.years)).ToGuardCases(s => s.Name switch
            {
                nameof(F.HasMinimumAge.NegativeYears) => new GuardExpected(false, typeof(ArgumentException), "years", Code: MustCodes.Date.Age.BelowMinimum),
                _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Age.BelowMinimum)
            });
    }

    // A 29-February birth date has no anniversary in a non-leap year, so each case pins its own clock:
    // here the boundary moves and the birth date stays put, which the shared provider cannot express.
    public static class BelowMinimumAgeOnLeapDay
    {
        public static TheoryData<GuardCase<(DateTime value, int years, DateTimeOffset utcNow)>> Cases =>
        [
            new("TwentyEighthOfFebruaryInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 02, 28)), new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Date.Age.BelowMinimum)),
            new("FirstOfMarchInANonLeapYear", (LeapDayBirth, 18, Noon(2026, 03, 01)), new GuardExpected(true)),
            new("TwentyNinthOfFebruaryInALeapYear", (LeapDayBirth, 20, Noon(2028, 02, 29)), new GuardExpected(true))
        ];
    }

    private static DateTimeOffset Noon(int year, int month, int day) => new(year, month, day, 12, 0, 0, TimeSpan.Zero);
}
