using System.Globalization;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.StringPastDateOnlyRuleFixtures;
using Outcome = PineGuard.Rules.StringPastDateOnlyOutcome;

namespace PineGuard.Core.UnitTests.Rules;

public static class StringPastDateOnlyRuleTestData
{
    public static class Evaluate
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(F.Evaluate.PastDate), F.Evaluate.PastDate, (int)Outcome.Passed, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.PastDateAllowNull), F.Evaluate.PastDateAllowNull, (int)Outcome.Passed, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.Yesterday), F.Evaluate.Yesterday, (int)Outcome.Passed, new DateOnly(2026, 6, 14), 1),
            new(nameof(F.Evaluate.Today), F.Evaluate.Today, (int)Outcome.NotPast, new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.TodayAllowNull), F.Evaluate.TodayAllowNull, (int)Outcome.NotPast, new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.Tomorrow), F.Evaluate.Tomorrow, (int)Outcome.NotPast, new DateOnly(2026, 6, 16), 1),
            new(nameof(F.Evaluate.AheadOfUtcYesterday), F.Evaluate.AheadOfUtcYesterday, (int)Outcome.Passed, new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.AheadOfUtcToday), F.Evaluate.AheadOfUtcToday, (int)Outcome.NotPast, new DateOnly(2026, 6, 16), 1),
            new(nameof(F.Evaluate.AheadOfUtcTomorrow), F.Evaluate.AheadOfUtcTomorrow, (int)Outcome.NotPast, new DateOnly(2026, 6, 17), 1),
            new(nameof(F.Evaluate.BehindUtcYesterday), F.Evaluate.BehindUtcYesterday, (int)Outcome.Passed, new DateOnly(2026, 6, 13), 1),
            new(nameof(F.Evaluate.BehindUtcToday), F.Evaluate.BehindUtcToday, (int)Outcome.NotPast, new DateOnly(2026, 6, 14), 1),
            new(nameof(F.Evaluate.BehindUtcTomorrow), F.Evaluate.BehindUtcTomorrow, (int)Outcome.NotPast, new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.PaddedPastDate), F.Evaluate.PaddedPastDate, (int)Outcome.Passed, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.PaddedPastDateNoneStyles), F.Evaluate.PaddedPastDateNoneStyles, (int)Outcome.Passed, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.PastDateNoneStyles), F.Evaluate.PastDateNoneStyles, (int)Outcome.Passed, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.InvalidStyles), F.Evaluate.InvalidStyles, (int)Outcome.InvalidStyles, default, 0),
            new(nameof(F.Evaluate.InvalidStylesAllowNull), F.Evaluate.InvalidStylesAllowNull, (int)Outcome.InvalidStyles, default, 0),
            new(nameof(F.Evaluate.InvalidStylesCombination), F.Evaluate.InvalidStylesCombination, (int)Outcome.InvalidStyles, default, 0),
            new(nameof(F.Evaluate.NullValueInvalidStyles), F.Evaluate.NullValueInvalidStyles, (int)Outcome.InvalidStyles, default, 0),
            new(nameof(F.Evaluate.NullValueInvalidStylesAllowNull), F.Evaluate.NullValueInvalidStylesAllowNull, (int)Outcome.InvalidStyles, default, 0),
            new(nameof(F.Evaluate.NullValue), F.Evaluate.NullValue, (int)Outcome.Null, default, 0),
            new(nameof(F.Evaluate.NullValueAllowNull), F.Evaluate.NullValueAllowNull, (int)Outcome.Passed, default, 0),
            new(nameof(F.Evaluate.NotADate), F.Evaluate.NotADate, (int)Outcome.Unparseable, default, 0),
            new(nameof(F.Evaluate.NotADateAllowNull), F.Evaluate.NotADateAllowNull, (int)Outcome.Unparseable, default, 0),
            new(nameof(F.Evaluate.Empty), F.Evaluate.Empty, (int)Outcome.Unparseable, default, 0),
            new(nameof(F.Evaluate.EmptyAllowNull), F.Evaluate.EmptyAllowNull, (int)Outcome.Unparseable, default, 0),
            new(nameof(F.Evaluate.WhitespaceAllowNull), F.Evaluate.WhitespaceAllowNull, (int)Outcome.Unparseable, default, 0),
            new(nameof(F.Evaluate.MinValueProjection), F.Evaluate.MinValueProjection, (int)Outcome.ReferenceOutOfRange, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.MaxValueProjection), F.Evaluate.MaxValueProjection, (int)Outcome.ReferenceOutOfRange, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.MaxValueProjectionAllowNull), F.Evaluate.MaxValueProjectionAllowNull, (int)Outcome.ReferenceOutOfRange, new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.InvalidStylesBeforeProjection), F.Evaluate.InvalidStylesBeforeProjection, (int)Outcome.InvalidStyles, default, 0),
            new(nameof(F.Evaluate.NullValueBeforeProjection), F.Evaluate.NullValueBeforeProjection, (int)Outcome.Null, default, 0),
            new(nameof(F.Evaluate.NullValueAllowNullBeforeProjection), F.Evaluate.NullValueAllowNullBeforeProjection, (int)Outcome.Passed, default, 0),
            new(nameof(F.Evaluate.NotADateBeforeProjection), F.Evaluate.NotADateBeforeProjection, (int)Outcome.Unparseable, default, 0)
        ];

        public sealed record Case(string Name, (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Value, int Expected, DateOnly ExpectedOutValue, int ExpectedClockReads)
            : ReturnOutCase<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull), int, DateOnly>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class EvaluateSystemClock
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(F.Evaluate.SystemClockFarPast), F.Evaluate.SystemClockFarPast, (int)Outcome.Passed, new DateOnly(1, 1, 1)),
            new(nameof(F.Evaluate.SystemClockFarFuture), F.Evaluate.SystemClockFarFuture, (int)Outcome.NotPast, new DateOnly(9999, 12, 31))
        ];

        public sealed record Case(string Name, (string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull) Value, int Expected, DateOnly ExpectedOutValue)
            : ReturnOutCase<(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull), int, DateOnly>(Name, Value, Expected, ExpectedOutValue);
    }

    // Only one scenario is meaningful here: with a null provider "today" is the UTC calendar date of the real
    // clock, never the host's LocalTimeZone (D-1). Asserted against the real clock, also in the two CI processes
    // pinned to UTC+14 and UTC-12 (Pacific/Kiritimati, Etc/GMT+12), one of which always differs from the UTC date.
    public static class EvaluateNullProvider
    {
        public static TheoryData<string> Cases => ["SystemClock"];
    }

    public static class Code
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(Outcome.Passed), (int)Outcome.Passed, "date.relative.not-past"),
            new(nameof(Outcome.InvalidStyles), (int)Outcome.InvalidStyles, "date.relative.not-past"),
            new(nameof(Outcome.Null), (int)Outcome.Null, "date.relative.not-past"),
            new(nameof(Outcome.Unparseable), (int)Outcome.Unparseable, "date.format.invalid"),
            new(nameof(Outcome.ReferenceOutOfRange), (int)Outcome.ReferenceOutOfRange, "date.reference.out-of-range"),
            new(nameof(Outcome.NotPast), (int)Outcome.NotPast, "date.relative.not-past")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Default", 0, new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome")),
            new InvalidCase("Undefined", 99, new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome"))
        ];

        public sealed record Case(string Name, int Value, string Expected)
            : ReturnCase<int, string>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, int Value, ExpectedException ExpectedException)
            : ThrowsCase<int>(Name, Value, ExpectedException);
    }

    public static class MessageTemplate
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(Outcome.Passed), (int)Outcome.Passed, "{paramName} must be a date in the past."),
            new(nameof(Outcome.InvalidStyles), (int)Outcome.InvalidStyles, "{paramName} must be a valid combination of parsing styles."),
            new(nameof(Outcome.Null), (int)Outcome.Null, "{paramName} must not be null."),
            new(nameof(Outcome.Unparseable), (int)Outcome.Unparseable, "{paramName} must be a date in the past."),
            new(nameof(Outcome.ReferenceOutOfRange), (int)Outcome.ReferenceOutOfRange, "{paramName} requires a reference date within the supported range."),
            new(nameof(Outcome.NotPast), (int)Outcome.NotPast, "{paramName} must be a date in the past.")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Default", 0, new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome")),
            new InvalidCase("Undefined", 99, new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome"))
        ];

        public sealed record Case(string Name, int Value, string Expected)
            : ReturnCase<int, string>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, int Value, ExpectedException ExpectedException)
            : ThrowsCase<int>(Name, Value, ExpectedException);
    }

    public static class ParamName
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(Outcome.Passed), ((int)Outcome.Passed, "value"), "value"),
            new(nameof(Outcome.InvalidStyles), ((int)Outcome.InvalidStyles, "value"), "styles"),
            new(nameof(Outcome.Null), ((int)Outcome.Null, "value"), "value"),
            new(nameof(Outcome.Unparseable), ((int)Outcome.Unparseable, "value"), "value"),
            new(nameof(Outcome.ReferenceOutOfRange), ((int)Outcome.ReferenceOutOfRange, "value"), "timeProvider"),
            new(nameof(Outcome.NotPast), ((int)Outcome.NotPast, "value"), "value"),
            new($"{nameof(Outcome.Passed)}-NullParamName", ((int)Outcome.Passed, null), null),
            new($"{nameof(Outcome.InvalidStyles)}-NullParamName", ((int)Outcome.InvalidStyles, null), "styles"),
            new($"{nameof(Outcome.ReferenceOutOfRange)}-NullParamName", ((int)Outcome.ReferenceOutOfRange, null), "timeProvider")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Default", (0, "value"), new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome")),
            new InvalidCase("Undefined", (99, "value"), new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome"))
        ];

        public sealed record Case(string Name, (int outcome, string? paramName) Value, string? Expected)
            : ReturnCase<(int outcome, string? paramName), string?>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (int outcome, string? paramName) Value, ExpectedException ExpectedException)
            : ThrowsCase<(int outcome, string? paramName)>(Name, Value, ExpectedException);
    }

    public static class Value
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(Outcome.Passed), ((int)Outcome.Passed, F.Evaluate.PastDate.value, F.Evaluate.PastDate.styles, F.Evaluate.PastDate.timeProvider), F.Evaluate.PastDate.value),
            new(nameof(Outcome.InvalidStyles), ((int)Outcome.InvalidStyles, F.Evaluate.InvalidStyles.value, F.Evaluate.InvalidStyles.styles, F.Evaluate.InvalidStyles.timeProvider), F.Evaluate.InvalidStyles.styles),
            new($"{nameof(Outcome.InvalidStyles)}-NullValue", ((int)Outcome.InvalidStyles, F.Evaluate.NullValueInvalidStyles.value, F.Evaluate.NullValueInvalidStyles.styles, F.Evaluate.NullValueInvalidStyles.timeProvider), F.Evaluate.NullValueInvalidStyles.styles),
            new(nameof(Outcome.Null), ((int)Outcome.Null, F.Evaluate.NullValue.value, F.Evaluate.NullValue.styles, F.Evaluate.NullValue.timeProvider), F.Evaluate.NullValue.value),
            new(nameof(Outcome.Unparseable), ((int)Outcome.Unparseable, F.Evaluate.NotADate.value, F.Evaluate.NotADate.styles, F.Evaluate.NotADate.timeProvider), F.Evaluate.NotADate.value),
            new(nameof(Outcome.ReferenceOutOfRange), ((int)Outcome.ReferenceOutOfRange, F.Evaluate.MinValueProjection.value, F.Evaluate.MinValueProjection.styles, F.Evaluate.MinValueProjection.timeProvider), F.Evaluate.MinValueProjection.timeProvider),
            new(nameof(Outcome.NotPast), ((int)Outcome.NotPast, F.Evaluate.Tomorrow.value, F.Evaluate.Tomorrow.styles, F.Evaluate.Tomorrow.timeProvider), F.Evaluate.Tomorrow.value)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Default", (0, F.Evaluate.PastDate.value, F.Evaluate.PastDate.styles, F.Evaluate.PastDate.timeProvider), new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome")),
            new InvalidCase("Undefined", (99, F.Evaluate.PastDate.value, F.Evaluate.PastDate.styles, F.Evaluate.PastDate.timeProvider), new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome"))
        ];

        public sealed record Case(string Name, (int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider) Value, object? Expected)
            : ReturnCase<(int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider), object?>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider) Value, ExpectedException ExpectedException)
            : ThrowsCase<(int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider)>(Name, Value, ExpectedException);
    }
}
