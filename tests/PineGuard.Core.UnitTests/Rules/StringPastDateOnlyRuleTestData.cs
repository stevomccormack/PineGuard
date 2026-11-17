using System.Globalization;
using PineGuard.Rules;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringPastDateOnlyRuleFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class StringPastDateOnlyRuleTestData
{
    public static class Evaluate
    {
        public static TheoryData<RuleCase<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull)>> Cases => F.Evaluate.AllScenarios.ToRuleCases();

        public static TheoryData<Case> AdHocCases =>
        [
            new(nameof(F.Evaluate.PastDate), F.Evaluate.PastDate, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.PastDateAllowNull), F.Evaluate.PastDateAllowNull, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.Yesterday), F.Evaluate.Yesterday, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2026, 6, 14)),
            new(nameof(F.Evaluate.Today), F.Evaluate.Today, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 15)),
            new(nameof(F.Evaluate.TodayAllowNull), F.Evaluate.TodayAllowNull, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 15)),
            new(nameof(F.Evaluate.Tomorrow), F.Evaluate.Tomorrow, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 16)),
            new(nameof(F.Evaluate.AheadOfUtcYesterday), F.Evaluate.AheadOfUtcYesterday, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2026, 6, 15)),
            new(nameof(F.Evaluate.AheadOfUtcToday), F.Evaluate.AheadOfUtcToday, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 16)),
            new(nameof(F.Evaluate.AheadOfUtcTomorrow), F.Evaluate.AheadOfUtcTomorrow, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 17)),
            new(nameof(F.Evaluate.BehindUtcYesterday), F.Evaluate.BehindUtcYesterday, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2026, 6, 13)),
            new(nameof(F.Evaluate.BehindUtcToday), F.Evaluate.BehindUtcToday, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 14)),
            new(nameof(F.Evaluate.BehindUtcTomorrow), F.Evaluate.BehindUtcTomorrow, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2026, 6, 15)),
            new(nameof(F.Evaluate.PaddedPastDate), F.Evaluate.PaddedPastDate, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.PaddedPastDateNoneStyles), F.Evaluate.PaddedPastDateNoneStyles, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.PastDateNoneStyles), F.Evaluate.PastDateNoneStyles, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.InnerSpacedPastDateNoneStyles), F.Evaluate.InnerSpacedPastDateNoneStyles, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.PaddedPastDateLeadingWhiteStyles), F.Evaluate.PaddedPastDateLeadingWhiteStyles, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.PaddedPastDateTrailingWhiteStyles), F.Evaluate.PaddedPastDateTrailingWhiteStyles, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.PaddedPastDateInnerWhiteStyles), F.Evaluate.PaddedPastDateInnerWhiteStyles, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.InvalidStyles), F.Evaluate.InvalidStyles, (int)StringPastDateOnlyOutcome.InvalidStyles, default),
            new(nameof(F.Evaluate.InvalidStylesAllowNull), F.Evaluate.InvalidStylesAllowNull, (int)StringPastDateOnlyOutcome.InvalidStyles, default),
            new(nameof(F.Evaluate.InvalidStylesCombination), F.Evaluate.InvalidStylesCombination, (int)StringPastDateOnlyOutcome.InvalidStyles, default),
            new(nameof(F.Evaluate.NullValueInvalidStyles), F.Evaluate.NullValueInvalidStyles, (int)StringPastDateOnlyOutcome.InvalidStyles, default),
            new(nameof(F.Evaluate.NullValueInvalidStylesAllowNull), F.Evaluate.NullValueInvalidStylesAllowNull, (int)StringPastDateOnlyOutcome.InvalidStyles, default),
            new(nameof(F.Evaluate.NullValue), F.Evaluate.NullValue, (int)StringPastDateOnlyOutcome.Null, default),
            new(nameof(F.Evaluate.NullValueAllowNull), F.Evaluate.NullValueAllowNull, (int)StringPastDateOnlyOutcome.Passed, default),
            new(nameof(F.Evaluate.NotADate), F.Evaluate.NotADate, (int)StringPastDateOnlyOutcome.Unparseable, default),
            new(nameof(F.Evaluate.NotADateAllowNull), F.Evaluate.NotADateAllowNull, (int)StringPastDateOnlyOutcome.Unparseable, default),
            new(nameof(F.Evaluate.Empty), F.Evaluate.Empty, (int)StringPastDateOnlyOutcome.Unparseable, default),
            new(nameof(F.Evaluate.EmptyAllowNull), F.Evaluate.EmptyAllowNull, (int)StringPastDateOnlyOutcome.Unparseable, default),
            new(nameof(F.Evaluate.WhitespaceAllowNull), F.Evaluate.WhitespaceAllowNull, (int)StringPastDateOnlyOutcome.Unparseable, default),
            new(nameof(F.Evaluate.SpaceInsideComponent), F.Evaluate.SpaceInsideComponent, (int)StringPastDateOnlyOutcome.Unparseable, default),
            new(nameof(F.Evaluate.MinValueProjection), F.Evaluate.MinValueProjection, (int)StringPastDateOnlyOutcome.ReferenceOutOfRange, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.MaxValueProjection), F.Evaluate.MaxValueProjection, (int)StringPastDateOnlyOutcome.ReferenceOutOfRange, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.MaxValueProjectionAllowNull), F.Evaluate.MaxValueProjectionAllowNull, (int)StringPastDateOnlyOutcome.ReferenceOutOfRange, new DateOnly(2000, 1, 1)),
            new(nameof(F.Evaluate.InvalidStylesBeforeProjection), F.Evaluate.InvalidStylesBeforeProjection, (int)StringPastDateOnlyOutcome.InvalidStyles, default),
            new(nameof(F.Evaluate.NullValueBeforeProjection), F.Evaluate.NullValueBeforeProjection, (int)StringPastDateOnlyOutcome.Null, default),
            new(nameof(F.Evaluate.NullValueAllowNullBeforeProjection), F.Evaluate.NullValueAllowNullBeforeProjection, (int)StringPastDateOnlyOutcome.Passed, default),
            new(nameof(F.Evaluate.NotADateBeforeProjection), F.Evaluate.NotADateBeforeProjection, (int)StringPastDateOnlyOutcome.Unparseable, default)
        ];

        public sealed record Case(string Name, (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Value, int Expected, DateOnly ExpectedOutValue)
            : ReturnOutCase<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull), int, DateOnly>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class EvaluateSystemClock
    {
        public static TheoryData<RuleCase<(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull)>> Cases => F.EvaluateSystemClock.AllScenarios.ToRuleCases();

        public static TheoryData<Case> AdHocCases =>
        [
            new(nameof(F.EvaluateSystemClock.SystemClockFarPast), F.EvaluateSystemClock.SystemClockFarPast, (int)StringPastDateOnlyOutcome.Passed, new DateOnly(2000, 1, 1)),
            new(nameof(F.EvaluateSystemClock.SystemClockFarFuture), F.EvaluateSystemClock.SystemClockFarFuture, (int)StringPastDateOnlyOutcome.NotPast, new DateOnly(2999, 1, 1))
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

    public static class EvaluateClockReads
    {
        public static TheoryData<ClockReadCase> Cases =>
        [
            new(nameof(F.Evaluate.PastDate), F.Evaluate.PastDate, 1),
            new(nameof(F.Evaluate.PastDateAllowNull), F.Evaluate.PastDateAllowNull, 1),
            new(nameof(F.Evaluate.Yesterday), F.Evaluate.Yesterday, 1),
            new(nameof(F.Evaluate.Today), F.Evaluate.Today, 1),
            new(nameof(F.Evaluate.TodayAllowNull), F.Evaluate.TodayAllowNull, 1),
            new(nameof(F.Evaluate.Tomorrow), F.Evaluate.Tomorrow, 1),
            new(nameof(F.Evaluate.AheadOfUtcYesterday), F.Evaluate.AheadOfUtcYesterday, 1),
            new(nameof(F.Evaluate.AheadOfUtcToday), F.Evaluate.AheadOfUtcToday, 1),
            new(nameof(F.Evaluate.AheadOfUtcTomorrow), F.Evaluate.AheadOfUtcTomorrow, 1),
            new(nameof(F.Evaluate.BehindUtcYesterday), F.Evaluate.BehindUtcYesterday, 1),
            new(nameof(F.Evaluate.BehindUtcToday), F.Evaluate.BehindUtcToday, 1),
            new(nameof(F.Evaluate.BehindUtcTomorrow), F.Evaluate.BehindUtcTomorrow, 1),
            new(nameof(F.Evaluate.PaddedPastDate), F.Evaluate.PaddedPastDate, 1),
            new(nameof(F.Evaluate.PaddedPastDateNoneStyles), F.Evaluate.PaddedPastDateNoneStyles, 1),
            new(nameof(F.Evaluate.PastDateNoneStyles), F.Evaluate.PastDateNoneStyles, 1),
            new(nameof(F.Evaluate.InnerSpacedPastDateNoneStyles), F.Evaluate.InnerSpacedPastDateNoneStyles, 1),
            new(nameof(F.Evaluate.PaddedPastDateLeadingWhiteStyles), F.Evaluate.PaddedPastDateLeadingWhiteStyles, 1),
            new(nameof(F.Evaluate.PaddedPastDateTrailingWhiteStyles), F.Evaluate.PaddedPastDateTrailingWhiteStyles, 1),
            new(nameof(F.Evaluate.PaddedPastDateInnerWhiteStyles), F.Evaluate.PaddedPastDateInnerWhiteStyles, 1),
            new(nameof(F.Evaluate.InvalidStyles), F.Evaluate.InvalidStyles, 0),
            new(nameof(F.Evaluate.InvalidStylesAllowNull), F.Evaluate.InvalidStylesAllowNull, 0),
            new(nameof(F.Evaluate.InvalidStylesCombination), F.Evaluate.InvalidStylesCombination, 0),
            new(nameof(F.Evaluate.NullValueInvalidStyles), F.Evaluate.NullValueInvalidStyles, 0),
            new(nameof(F.Evaluate.NullValueInvalidStylesAllowNull), F.Evaluate.NullValueInvalidStylesAllowNull, 0),
            new(nameof(F.Evaluate.NullValue), F.Evaluate.NullValue, 0),
            new(nameof(F.Evaluate.NullValueAllowNull), F.Evaluate.NullValueAllowNull, 0),
            new(nameof(F.Evaluate.NotADate), F.Evaluate.NotADate, 0),
            new(nameof(F.Evaluate.NotADateAllowNull), F.Evaluate.NotADateAllowNull, 0),
            new(nameof(F.Evaluate.Empty), F.Evaluate.Empty, 0),
            new(nameof(F.Evaluate.EmptyAllowNull), F.Evaluate.EmptyAllowNull, 0),
            new(nameof(F.Evaluate.WhitespaceAllowNull), F.Evaluate.WhitespaceAllowNull, 0),
            new(nameof(F.Evaluate.SpaceInsideComponent), F.Evaluate.SpaceInsideComponent, 0),
            new(nameof(F.Evaluate.MinValueProjection), F.Evaluate.MinValueProjection, 1),
            new(nameof(F.Evaluate.MaxValueProjection), F.Evaluate.MaxValueProjection, 1),
            new(nameof(F.Evaluate.MaxValueProjectionAllowNull), F.Evaluate.MaxValueProjectionAllowNull, 1),
            new(nameof(F.Evaluate.InvalidStylesBeforeProjection), F.Evaluate.InvalidStylesBeforeProjection, 0),
            new(nameof(F.Evaluate.NullValueBeforeProjection), F.Evaluate.NullValueBeforeProjection, 0),
            new(nameof(F.Evaluate.NullValueAllowNullBeforeProjection), F.Evaluate.NullValueAllowNullBeforeProjection, 0),
            new(nameof(F.Evaluate.NotADateBeforeProjection), F.Evaluate.NotADateBeforeProjection, 0)
        ];

        public sealed record ClockReadCase(string Name, (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Value, int Expected)
            : ReturnCase<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull), int>(Name, Value, Expected);
    }

    public static class IsPassed
    {
        public static TheoryData<RuleCase<int>> Cases =>
        [
            new RuleCase<int>(nameof(StringPastDateOnlyOutcome.Passed), (int)StringPastDateOnlyOutcome.Passed, new RuleExpected(true)),
            new RuleCase<int>(nameof(StringPastDateOnlyOutcome.InvalidStyles), (int)StringPastDateOnlyOutcome.InvalidStyles, new RuleExpected(false)),
            new RuleCase<int>(nameof(StringPastDateOnlyOutcome.Null), (int)StringPastDateOnlyOutcome.Null, new RuleExpected(false)),
            new RuleCase<int>(nameof(StringPastDateOnlyOutcome.Unparseable), (int)StringPastDateOnlyOutcome.Unparseable, new RuleExpected(false)),
            new RuleCase<int>(nameof(StringPastDateOnlyOutcome.ReferenceOutOfRange), (int)StringPastDateOnlyOutcome.ReferenceOutOfRange, new RuleExpected(false)),
            new RuleCase<int>(nameof(StringPastDateOnlyOutcome.NotPast), (int)StringPastDateOnlyOutcome.NotPast, new RuleExpected(false))
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Default", 0, new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome")),
            new InvalidCase("Undefined", 99, new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome"))
        ];

        public sealed record InvalidCase(string Name, int Value, ExpectedException ExpectedException)
            : ThrowsCase<int>(Name, Value, ExpectedException);
    }

    public static class Code
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(StringPastDateOnlyOutcome.Passed), (int)StringPastDateOnlyOutcome.Passed, "date.relative.not-past"),
            new(nameof(StringPastDateOnlyOutcome.InvalidStyles), (int)StringPastDateOnlyOutcome.InvalidStyles, "date.relative.not-past"),
            new(nameof(StringPastDateOnlyOutcome.Null), (int)StringPastDateOnlyOutcome.Null, "date.relative.not-past"),
            new(nameof(StringPastDateOnlyOutcome.Unparseable), (int)StringPastDateOnlyOutcome.Unparseable, "date.format.invalid"),
            new(nameof(StringPastDateOnlyOutcome.ReferenceOutOfRange), (int)StringPastDateOnlyOutcome.ReferenceOutOfRange, "date.reference.out-of-range"),
            new(nameof(StringPastDateOnlyOutcome.NotPast), (int)StringPastDateOnlyOutcome.NotPast, "date.relative.not-past")
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
            new(nameof(StringPastDateOnlyOutcome.Passed), (int)StringPastDateOnlyOutcome.Passed, "{paramName} must be a date in the past."),
            new(nameof(StringPastDateOnlyOutcome.InvalidStyles), (int)StringPastDateOnlyOutcome.InvalidStyles, "{paramName} must be a valid combination of parsing styles."),
            new(nameof(StringPastDateOnlyOutcome.Null), (int)StringPastDateOnlyOutcome.Null, "{paramName} must not be null."),
            new(nameof(StringPastDateOnlyOutcome.Unparseable), (int)StringPastDateOnlyOutcome.Unparseable, "{paramName} must be a date in the past."),
            new(nameof(StringPastDateOnlyOutcome.ReferenceOutOfRange), (int)StringPastDateOnlyOutcome.ReferenceOutOfRange, "{paramName} requires a reference date within the supported range."),
            new(nameof(StringPastDateOnlyOutcome.NotPast), (int)StringPastDateOnlyOutcome.NotPast, "{paramName} must be a date in the past.")
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
            new(nameof(StringPastDateOnlyOutcome.Passed), ((int)StringPastDateOnlyOutcome.Passed, "value"), "value"),
            new(nameof(StringPastDateOnlyOutcome.InvalidStyles), ((int)StringPastDateOnlyOutcome.InvalidStyles, "value"), "styles"),
            new(nameof(StringPastDateOnlyOutcome.Null), ((int)StringPastDateOnlyOutcome.Null, "value"), "value"),
            new(nameof(StringPastDateOnlyOutcome.Unparseable), ((int)StringPastDateOnlyOutcome.Unparseable, "value"), "value"),
            new(nameof(StringPastDateOnlyOutcome.ReferenceOutOfRange), ((int)StringPastDateOnlyOutcome.ReferenceOutOfRange, "value"), "timeProvider"),
            new(nameof(StringPastDateOnlyOutcome.NotPast), ((int)StringPastDateOnlyOutcome.NotPast, "value"), "value"),
            new($"{nameof(StringPastDateOnlyOutcome.Passed)}-NullParamName", ((int)StringPastDateOnlyOutcome.Passed, null), null),
            new($"{nameof(StringPastDateOnlyOutcome.InvalidStyles)}-NullParamName", ((int)StringPastDateOnlyOutcome.InvalidStyles, null), "styles"),
            new($"{nameof(StringPastDateOnlyOutcome.ReferenceOutOfRange)}-NullParamName", ((int)StringPastDateOnlyOutcome.ReferenceOutOfRange, null), "timeProvider")
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
            new(nameof(StringPastDateOnlyOutcome.Passed), ((int)StringPastDateOnlyOutcome.Passed, F.Evaluate.PastDate.value, F.Evaluate.PastDate.styles, F.Evaluate.PastDate.timeProvider), "2000-01-01"),
            new(nameof(StringPastDateOnlyOutcome.InvalidStyles), ((int)StringPastDateOnlyOutcome.InvalidStyles, F.Evaluate.InvalidStyles.value, F.Evaluate.InvalidStyles.styles, F.Evaluate.InvalidStyles.timeProvider), DateTimeStyles.AssumeUniversal),
            new($"{nameof(StringPastDateOnlyOutcome.InvalidStyles)}-NullValue", ((int)StringPastDateOnlyOutcome.InvalidStyles, F.Evaluate.NullValueInvalidStyles.value, F.Evaluate.NullValueInvalidStyles.styles, F.Evaluate.NullValueInvalidStyles.timeProvider), DateTimeStyles.AssumeUniversal),
            new(nameof(StringPastDateOnlyOutcome.Null), ((int)StringPastDateOnlyOutcome.Null, F.Evaluate.NullValue.value, F.Evaluate.NullValue.styles, F.Evaluate.NullValue.timeProvider), null),
            new(nameof(StringPastDateOnlyOutcome.Unparseable), ((int)StringPastDateOnlyOutcome.Unparseable, F.Evaluate.NotADate.value, F.Evaluate.NotADate.styles, F.Evaluate.NotADate.timeProvider), "not-a-date"),
            new(nameof(StringPastDateOnlyOutcome.ReferenceOutOfRange), ((int)StringPastDateOnlyOutcome.ReferenceOutOfRange, F.Evaluate.MinValueProjection.value, F.Evaluate.MinValueProjection.styles, F.Evaluate.MinValueProjection.timeProvider), F.Evaluate.MinValueProjection.timeProvider),
            new(nameof(StringPastDateOnlyOutcome.NotPast), ((int)StringPastDateOnlyOutcome.NotPast, F.Evaluate.Tomorrow.value, F.Evaluate.Tomorrow.styles, F.Evaluate.Tomorrow.timeProvider), "2026-06-16")
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
