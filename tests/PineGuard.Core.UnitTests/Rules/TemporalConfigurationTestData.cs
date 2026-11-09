using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Core.UnitTests.Rules;

public static class TemporalConfigurationTestData
{
    public static class InvalidInclusion
    {
        public static TheoryData<Case> Cases =>
        [
            new("StringDateOnly.null", () => PineGuard.Rules.StringRules.DateOnly.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("StringDateTimeOffset.null", () => PineGuard.Rules.StringRules.DateTimeOffset.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("StringTimeOnly.null", () => PineGuard.Rules.StringRules.TimeOnly.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("StringTimeSpan.null", () => PineGuard.Rules.StringRules.TimeSpan.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("RuleComparison.reversed", () => RuleComparison.IsInRange(0, 2, 1, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("NumberRules.null.reversed", () => NumberRules.IsInRange<int>(null, 2, 1, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsInRange.null", () => DateOnlyRules.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsBefore.null", () => DateOnlyRules.IsBefore(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsAfter.null", () => DateOnlyRules.IsAfter(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.Chronological.null", () => DateOnlyRules.IsChronological(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.Overlapping.null", () => DateOnlyRules.IsOverlapping(null, default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsInRange.default(DateOnly)", () => DateOnlyRules.IsInRange(default(DateOnly), default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsBefore.default(DateOnly)", () => DateOnlyRules.IsBefore(default(DateOnly), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsAfter.default(DateOnly)", () => DateOnlyRules.IsAfter(default(DateOnly), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.Chronological.default(DateOnly)", () => DateOnlyRules.IsChronological(default(DateOnly), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.Overlapping.default(DateOnly)", () => DateOnlyRules.IsOverlapping(default(DateOnly), default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Chronological.null", () => DateOnlyRangeRules.IsChronological(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Overlapping.null", () => DateOnlyRangeRules.IsOverlapping(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Contains.null", () => DateOnlyRangeRules.Contains(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Chronological.default(DateOnlyRange)", () => DateOnlyRangeRules.IsChronological(default(DateOnlyRange), (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Overlapping.default(DateOnlyRange)", () => DateOnlyRangeRules.IsOverlapping(default(DateOnlyRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Contains.default(DateOnlyRange)", () => DateOnlyRangeRules.Contains(default(DateOnlyRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnlyRange.Overlaps", () => default(DateOnlyRange).Overlaps(default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsInRange.null", () => DateTimeRules.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsBefore.null", () => DateTimeRules.IsBefore(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsAfter.null", () => DateTimeRules.IsAfter(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.Chronological.null", () => DateTimeRules.IsChronological(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.Overlapping.null", () => DateTimeRules.IsOverlapping(null, default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsInRange.default(DateTime)", () => DateTimeRules.IsInRange(default(DateTime), default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsBefore.default(DateTime)", () => DateTimeRules.IsBefore(default(DateTime), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsAfter.default(DateTime)", () => DateTimeRules.IsAfter(default(DateTime), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.Chronological.default(DateTime)", () => DateTimeRules.IsChronological(default(DateTime), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.Overlapping.default(DateTime)", () => DateTimeRules.IsOverlapping(default(DateTime), default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Chronological.null", () => DateTimeRangeRules.IsChronological(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Overlapping.null", () => DateTimeRangeRules.IsOverlapping(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Contains.null", () => DateTimeRangeRules.Contains(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Chronological.default(DateTimeRange)", () => DateTimeRangeRules.IsChronological(default(DateTimeRange), (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Overlapping.default(DateTimeRange)", () => DateTimeRangeRules.IsOverlapping(default(DateTimeRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Contains.default(DateTimeRange)", () => DateTimeRangeRules.Contains(default(DateTimeRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeRange.Overlaps", () => default(DateTimeRange).Overlaps(default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsInRange.null", () => DateTimeOffsetRules.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsBefore.null", () => DateTimeOffsetRules.IsBefore(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsAfter.null", () => DateTimeOffsetRules.IsAfter(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.Chronological.null", () => DateTimeOffsetRules.IsChronological(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.Overlapping.null", () => DateTimeOffsetRules.IsOverlapping(null, default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsInRange.default(DateTimeOffset)", () => DateTimeOffsetRules.IsInRange(default(DateTimeOffset), default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsBefore.default(DateTimeOffset)", () => DateTimeOffsetRules.IsBefore(default(DateTimeOffset), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsAfter.default(DateTimeOffset)", () => DateTimeOffsetRules.IsAfter(default(DateTimeOffset), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.Chronological.default(DateTimeOffset)", () => DateTimeOffsetRules.IsChronological(default(DateTimeOffset), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.Overlapping.default(DateTimeOffset)", () => DateTimeOffsetRules.IsOverlapping(default(DateTimeOffset), default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Chronological.null", () => DateTimeOffsetRangeRules.IsChronological(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Overlapping.null", () => DateTimeOffsetRangeRules.IsOverlapping(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Contains.null", () => DateTimeOffsetRangeRules.Contains(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Chronological.default(DateTimeOffsetRange)", () => DateTimeOffsetRangeRules.IsChronological(default(DateTimeOffsetRange), (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Overlapping.default(DateTimeOffsetRange)", () => DateTimeOffsetRangeRules.IsOverlapping(default(DateTimeOffsetRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Contains.default(DateTimeOffsetRange)", () => DateTimeOffsetRangeRules.Contains(default(DateTimeOffsetRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffsetRange.Overlaps", () => default(DateTimeOffsetRange).Overlaps(default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.IsInRange.null", () => TimeOnlyRules.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.IsBefore.null", () => TimeOnlyRules.IsBefore(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.IsAfter.null", () => TimeOnlyRules.IsAfter(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.Chronological.null", () => TimeOnlyRules.IsChronological(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.Overlapping.null", () => TimeOnlyRules.IsOverlapping(null, default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.IsInRange.default(TimeOnly)", () => TimeOnlyRules.IsInRange(default(TimeOnly), default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.IsBefore.default(TimeOnly)", () => TimeOnlyRules.IsBefore(default(TimeOnly), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.IsAfter.default(TimeOnly)", () => TimeOnlyRules.IsAfter(default(TimeOnly), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.Chronological.default(TimeOnly)", () => TimeOnlyRules.IsChronological(default(TimeOnly), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnly.Overlapping.default(TimeOnly)", () => TimeOnlyRules.IsOverlapping(default(TimeOnly), default, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Chronological.null", () => TimeOnlyRangeRules.IsChronological(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Overlapping.null", () => TimeOnlyRangeRules.IsOverlapping(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Contains.null", () => TimeOnlyRangeRules.Contains(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Chronological.default(TimeOnlyRange)", () => TimeOnlyRangeRules.IsChronological(default(TimeOnlyRange), (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Overlapping.default(TimeOnlyRange)", () => TimeOnlyRangeRules.IsOverlapping(default(TimeOnlyRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Contains.default(TimeOnlyRange)", () => TimeOnlyRangeRules.Contains(default(TimeOnlyRange), default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeOnlyRange.Overlaps", () => default(TimeOnlyRange).Overlaps(default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsInPast", () => DateOnlyRules.IsInPast(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateOnly.IsInFuture", () => DateOnlyRules.IsInFuture(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsInPast", () => DateTimeRules.IsInPast(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTime.IsInFuture", () => DateTimeRules.IsInFuture(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsInPast", () => DateTimeOffsetRules.IsInPast(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("DateTimeOffset.IsInFuture", () => DateTimeOffsetRules.IsInFuture(null, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeSpan.IsInRange", () => TimeSpanRules.IsInRange(null, default, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeSpan.IsLongerThan", () => TimeSpanRules.IsLongerThan(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion")),
            new("TimeSpan.IsShorterThan", () => TimeSpanRules.IsShorterThan(null, default, (Inclusion)42), new ExpectedException(typeof(ArgumentOutOfRangeException), "inclusion"))
        ];
        public sealed record Case(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
    public static class InvalidPrecision
    {
        public static TheoryData<RuleCase<Func<bool>>> Cases =>
        [
            new("DateOnly.null, null", () => DateOnlyRules.IsSame(null, null, (DatePrecision)999), new(false)),
            new("DateOnly.default(DateOnly), default(DateOnly)", () => DateOnlyRules.IsSame(default(DateOnly), default(DateOnly), (DatePrecision)999), new(false)),
            new("DateTime.null, null", () => DateTimeRules.IsSame(null, null, (DateTimePrecision)999), new(false)),
            new("DateTime.default(DateTime), default(DateTime)", () => DateTimeRules.IsSame(default(DateTime), default(DateTime), (DateTimePrecision)999), new(false)),
            new("DateTimeOffset.null, null", () => DateTimeOffsetRules.IsSame(null, null, (DateTimePrecision)999), new(false)),
            new("DateTimeOffset.default(DateTimeOffset), default(DateTimeOffset)", () => DateTimeOffsetRules.IsSame(default(DateTimeOffset), default(DateTimeOffset), (DateTimePrecision)999), new(false)),
            new("TimeOnly.null, null", () => TimeOnlyRules.IsSame(null, null, (TimePrecision)999), new(false)),
            new("TimeOnly.default(TimeOnly), default(TimeOnly)", () => TimeOnlyRules.IsSame(default(TimeOnly), default(TimeOnly), (TimePrecision)999), new(false))
        ];
    }
}
