using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDateTimeOffsetClausesTestData
{
    // Guard.Against.FutureOrPresent — calls Must.Be.Past — passes when past, throws when future/present
    public static class FutureOrPresent
    {
        public static TheoryData<GuardCase<DateTimeOffset>> ValidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<DateTimeOffset>> InvalidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Future — calls Must.Be.PastOrPresent — passes when past, throws when future
    public static class Future
    {
        public static TheoryData<GuardCase<DateTimeOffset>> ValidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<DateTimeOffset>> InvalidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.PastOrPresent — calls Must.Be.Future — passes when future, throws when past/present
    public static class PastOrPresent
    {
        public static TheoryData<GuardCase<DateTimeOffset>> ValidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<DateTimeOffset>> InvalidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Past — calls Must.Be.FutureOrPresent — passes when future, throws when past
    public static class Past
    {
        public static TheoryData<GuardCase<DateTimeOffset>> ValidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.FutureDate), F.IsPast.FutureDate!.Value, true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<DateTimeOffset>> InvalidCases =>
            new RuleScenario<DateTimeOffset>[]
            {
                new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotBetween — calls Must.Be.Between — passes when between, throws when not between
    public static class NotBetween
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[]
            {
                new(nameof(F.IsBetween.MiddleInclusive), (F.IsBetween.MiddleInclusive.value!.Value, F.IsBetween.MiddleInclusive.min, F.IsBetween.MiddleInclusive.max, F.IsBetween.MiddleInclusive.inclusion), true),
                new(nameof(F.IsBetween.AtMinInclusive), (F.IsBetween.AtMinInclusive.value!.Value, F.IsBetween.AtMinInclusive.min, F.IsBetween.AtMinInclusive.max, F.IsBetween.AtMinInclusive.inclusion), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[]
            {
                new(nameof(F.IsBetween.AtMinExclusive), (F.IsBetween.AtMinExclusive.value!.Value, F.IsBetween.AtMinExclusive.min, F.IsBetween.AtMinExclusive.max, F.IsBetween.AtMinExclusive.inclusion), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Between — calls Must.Be.NotBetween — passes when not between, throws when between
    public static class Between
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[]
            {
                new(nameof(F.IsBetween.AtMinExclusive), (F.IsBetween.AtMinExclusive.value!.Value, F.IsBetween.AtMinExclusive.min, F.IsBetween.AtMinExclusive.max, F.IsBetween.AtMinExclusive.inclusion), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>[]
            {
                new(nameof(F.IsBetween.MiddleInclusive), (F.IsBetween.MiddleInclusive.value!.Value, F.IsBetween.MiddleInclusive.min, F.IsBetween.MiddleInclusive.max, F.IsBetween.MiddleInclusive.inclusion), false),
                new(nameof(F.IsBetween.AtMinInclusive), (F.IsBetween.AtMinInclusive.value!.Value, F.IsBetween.AtMinInclusive.min, F.IsBetween.AtMinInclusive.max, F.IsBetween.AtMinInclusive.inclusion), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.OnOrAfter — calls Must.Be.Before (Exclusive) — passes when strictly before, throws when on or after
    public static class OnOrAfter
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsBefore.BeforeInclusive), (F.IsBefore.BeforeInclusive.value!.Value, F.IsBefore.BeforeInclusive.other!.Value), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsBefore.SameInstantInclusive), (F.IsBefore.SameInstantInclusive.value!.Value, F.IsBefore.SameInstantInclusive.other!.Value), false),
                new(nameof(F.IsBefore.SameInstantExclusive), (F.IsBefore.SameInstantExclusive.value!.Value, F.IsBefore.SameInstantExclusive.other!.Value), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.After — calls Must.Be.OnOrBefore (Inclusive) — passes when on or before, throws when after
    public static class After
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsAfter.SameInstantInclusive), (F.IsAfter.SameInstantInclusive.value!.Value, F.IsAfter.SameInstantInclusive.other!.Value), true),
                new(nameof(F.IsAfter.SameInstantExclusive), (F.IsAfter.SameInstantExclusive.value!.Value, F.IsAfter.SameInstantExclusive.other!.Value), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsAfter.AfterInclusive), (F.IsAfter.AfterInclusive.value!.Value, F.IsAfter.AfterInclusive.other!.Value), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.OnOrBefore — calls Must.Be.After (Exclusive) — passes when strictly after, throws when on or before
    public static class OnOrBefore
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsAfter.AfterInclusive), (F.IsAfter.AfterInclusive.value!.Value, F.IsAfter.AfterInclusive.other!.Value), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsAfter.SameInstantInclusive), (F.IsAfter.SameInstantInclusive.value!.Value, F.IsAfter.SameInstantInclusive.other!.Value), false),
                new(nameof(F.IsAfter.SameInstantExclusive), (F.IsAfter.SameInstantExclusive.value!.Value, F.IsAfter.SameInstantExclusive.other!.Value), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Before — calls Must.Be.OnOrAfter (Inclusive) — passes when on or after, throws when before
    public static class Before
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsBefore.SameInstantInclusive), (F.IsBefore.SameInstantInclusive.value!.Value, F.IsBefore.SameInstantInclusive.other!.Value), true),
                new(nameof(F.IsBefore.SameInstantExclusive), (F.IsBefore.SameInstantExclusive.value!.Value, F.IsBefore.SameInstantExclusive.other!.Value), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsBefore.BeforeInclusive), (F.IsBefore.BeforeInclusive.value!.Value, F.IsBefore.BeforeInclusive.other!.Value), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotSame — calls Must.Be.Same (precision=null) — passes when same, throws when not same
    public static class NotSame
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsSame.SameInstant), (F.IsSame.SameInstant.value!.Value, F.IsSame.SameInstant.other!.Value), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsSame.DifferentInstant), (F.IsSame.DifferentInstant.value!.Value, F.IsSame.DifferentInstant.other!.Value), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Same — calls Must.Be.NotSame (precision=null) — passes when not same, throws when same
    public static class Same
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsSame.DifferentInstant), (F.IsSame.DifferentInstant.value!.Value, F.IsSame.DifferentInstant.other!.Value), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset other)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset other)>[]
            {
                new(nameof(F.IsSame.SameInstant), (F.IsSame.SameInstant.value!.Value, F.IsSame.SameInstant.other!.Value), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotChronological — calls Must.Be.Chronological — passes when chronological, throws when not
    public static class NotChronological
    {
        public static TheoryData<GuardCase<(DateTimeOffset start, DateTimeOffset end, Inclusion inclusion)>> ValidCases =>
            new RuleScenario<(DateTimeOffset start, DateTimeOffset end, Inclusion inclusion)>[]
            {
                new(nameof(F.IsChronological.IncreasingExclusive), (F.IsChronological.IncreasingExclusive.start!.Value, F.IsChronological.IncreasingExclusive.end!.Value, F.IsChronological.IncreasingExclusive.inclusion), true),
                new(nameof(F.IsChronological.SameInstantInclusive), (F.IsChronological.SameInstantInclusive.start!.Value, F.IsChronological.SameInstantInclusive.end!.Value, F.IsChronological.SameInstantInclusive.inclusion), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset start, DateTimeOffset end, Inclusion inclusion)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset start, DateTimeOffset end, Inclusion inclusion)>[]
            {
                new(nameof(F.IsChronological.SameInstantExclusive), (F.IsChronological.SameInstantExclusive.start!.Value, F.IsChronological.SameInstantExclusive.end!.Value, F.IsChronological.SameInstantExclusive.inclusion), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "start"));
    }

    // Guard.Against.Overlapping — calls Must.Be.NotOverlapping — passes when not overlapping, throws when overlapping
    public static class Overlapping
    {
        public static TheoryData<GuardCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>> ValidCases =>
            new RuleScenario<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>[]
            {
                new(nameof(F.IsOverlapping.TouchingExclusive), (F.IsOverlapping.TouchingExclusive.start1!.Value, F.IsOverlapping.TouchingExclusive.end1!.Value, F.IsOverlapping.TouchingExclusive.start2!.Value, F.IsOverlapping.TouchingExclusive.end2!.Value, F.IsOverlapping.TouchingExclusive.inclusion), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>[]
            {
                new(nameof(F.IsOverlapping.TouchingInclusive), (F.IsOverlapping.TouchingInclusive.start1!.Value, F.IsOverlapping.TouchingInclusive.end1!.Value, F.IsOverlapping.TouchingInclusive.start2!.Value, F.IsOverlapping.TouchingInclusive.end2!.Value, F.IsOverlapping.TouchingInclusive.inclusion), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "start1"));
    }

    // Guard.Against.NotOverlapping — calls Must.Be.Overlapping — passes when overlapping, throws when not overlapping
    public static class NotOverlapping
    {
        public static TheoryData<GuardCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>> ValidCases =>
            new RuleScenario<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>[]
            {
                new(nameof(F.IsOverlapping.TouchingInclusive), (F.IsOverlapping.TouchingInclusive.start1!.Value, F.IsOverlapping.TouchingInclusive.end1!.Value, F.IsOverlapping.TouchingInclusive.start2!.Value, F.IsOverlapping.TouchingInclusive.end2!.Value, F.IsOverlapping.TouchingInclusive.inclusion), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset start1, DateTimeOffset end1, DateTimeOffset start2, DateTimeOffset end2, Inclusion inclusion)>[]
            {
                new(nameof(F.IsOverlapping.TouchingExclusive), (F.IsOverlapping.TouchingExclusive.start1!.Value, F.IsOverlapping.TouchingExclusive.end1!.Value, F.IsOverlapping.TouchingExclusive.start2!.Value, F.IsOverlapping.TouchingExclusive.end2!.Value, F.IsOverlapping.TouchingExclusive.inclusion), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "start1"));
    }

    // Guard.Against.NotWithin — calls Must.Be.Within — passes when within, throws when not within
    public static class NotWithin
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>[]
            {
                new(nameof(F.IsWithin.SameInstantZeroWindow), (F.IsWithin.SameInstantZeroWindow.value!.Value, F.IsWithin.SameInstantZeroWindow.reference!.Value, F.IsWithin.SameInstantZeroWindow.window), true),
                new(nameof(F.IsWithin.WithinWindow), (F.IsWithin.WithinWindow.value!.Value, F.IsWithin.WithinWindow.reference!.Value, F.IsWithin.WithinWindow.window), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>[]
            {
                new(nameof(F.IsWithin.OutsideWindow), (F.IsWithin.OutsideWindow.value!.Value, F.IsWithin.OutsideWindow.reference!.Value, F.IsWithin.OutsideWindow.window), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.Within — calls Must.Be.NotWithin — passes when not within, throws when within
    public static class Within
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>[]
            {
                new(nameof(F.IsWithin.OutsideWindow), (F.IsWithin.OutsideWindow.value!.Value, F.IsWithin.OutsideWindow.reference!.Value, F.IsWithin.OutsideWindow.window), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, TimeSpan window)>[]
            {
                new(nameof(F.IsWithin.SameInstantZeroWindow), (F.IsWithin.SameInstantZeroWindow.value!.Value, F.IsWithin.SameInstantZeroWindow.reference!.Value, F.IsWithin.SameInstantZeroWindow.window), false),
                new(nameof(F.IsWithin.WithinWindow), (F.IsWithin.WithinWindow.value!.Value, F.IsWithin.WithinWindow.reference!.Value, F.IsWithin.WithinWindow.window), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotWithinCalendarMonths — calls Must.Be.WithinCalendarMonths — passes when within, throws when not
    public static class NotWithinCalendarMonths
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, int months)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, int months)>[]
            {
                new(nameof(F.IsWithinCalendarMonths.SameDayZeroMonths), (F.IsWithinCalendarMonths.SameDayZeroMonths.value!.Value, F.IsWithinCalendarMonths.SameDayZeroMonths.reference!.Value, F.IsWithinCalendarMonths.SameDayZeroMonths.months), true),
                new(nameof(F.IsWithinCalendarMonths.WithinOneMonth), (F.IsWithinCalendarMonths.WithinOneMonth.value!.Value, F.IsWithinCalendarMonths.WithinOneMonth.reference!.Value, F.IsWithinCalendarMonths.WithinOneMonth.months), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, int months)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, int months)>[]
            {
                new(nameof(F.IsWithinCalendarMonths.OutsideOneMonth), (F.IsWithinCalendarMonths.OutsideOneMonth.value!.Value, F.IsWithinCalendarMonths.OutsideOneMonth.reference!.Value, F.IsWithinCalendarMonths.OutsideOneMonth.months), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.WithinCalendarMonths — calls Must.Be.NotWithinCalendarMonths — passes when not within, throws when within
    public static class WithinCalendarMonths
    {
        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, int months)>> ValidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, int months)>[]
            {
                new(nameof(F.IsWithinCalendarMonths.OutsideOneMonth), (F.IsWithinCalendarMonths.OutsideOneMonth.value!.Value, F.IsWithinCalendarMonths.OutsideOneMonth.reference!.Value, F.IsWithinCalendarMonths.OutsideOneMonth.months), true)
            }.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(DateTimeOffset value, DateTimeOffset reference, int months)>> InvalidCases =>
            new RuleScenario<(DateTimeOffset value, DateTimeOffset reference, int months)>[]
            {
                new(nameof(F.IsWithinCalendarMonths.SameDayZeroMonths), (F.IsWithinCalendarMonths.SameDayZeroMonths.value!.Value, F.IsWithinCalendarMonths.SameDayZeroMonths.reference!.Value, F.IsWithinCalendarMonths.SameDayZeroMonths.months), false),
                new(nameof(F.IsWithinCalendarMonths.WithinOneMonth), (F.IsWithinCalendarMonths.WithinOneMonth.value!.Value, F.IsWithinCalendarMonths.WithinOneMonth.reference!.Value, F.IsWithinCalendarMonths.WithinOneMonth.months), false)
            }.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
