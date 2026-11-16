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
            new(nameof(F.Evaluate.PastDate), F.Evaluate.PastDate, "Passed", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.PastDateAllowNull), F.Evaluate.PastDateAllowNull, "Passed", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.Yesterday), F.Evaluate.Yesterday, "Passed", new DateOnly(2026, 6, 14), 1),
            new(nameof(F.Evaluate.Today), F.Evaluate.Today, "NotPast", new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.TodayAllowNull), F.Evaluate.TodayAllowNull, "NotPast", new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.Tomorrow), F.Evaluate.Tomorrow, "NotPast", new DateOnly(2026, 6, 16), 1),
            new(nameof(F.Evaluate.AheadOfUtcYesterday), F.Evaluate.AheadOfUtcYesterday, "Passed", new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.AheadOfUtcToday), F.Evaluate.AheadOfUtcToday, "NotPast", new DateOnly(2026, 6, 16), 1),
            new(nameof(F.Evaluate.AheadOfUtcTomorrow), F.Evaluate.AheadOfUtcTomorrow, "NotPast", new DateOnly(2026, 6, 17), 1),
            new(nameof(F.Evaluate.BehindUtcYesterday), F.Evaluate.BehindUtcYesterday, "Passed", new DateOnly(2026, 6, 13), 1),
            new(nameof(F.Evaluate.BehindUtcToday), F.Evaluate.BehindUtcToday, "NotPast", new DateOnly(2026, 6, 14), 1),
            new(nameof(F.Evaluate.BehindUtcTomorrow), F.Evaluate.BehindUtcTomorrow, "NotPast", new DateOnly(2026, 6, 15), 1),
            new(nameof(F.Evaluate.PaddedPastDate), F.Evaluate.PaddedPastDate, "Passed", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.PaddedPastDateNoneStyles), F.Evaluate.PaddedPastDateNoneStyles, "Passed", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.PastDateNoneStyles), F.Evaluate.PastDateNoneStyles, "Passed", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.InvalidStyles), F.Evaluate.InvalidStyles, "InvalidStyles", default, 0),
            new(nameof(F.Evaluate.InvalidStylesAllowNull), F.Evaluate.InvalidStylesAllowNull, "InvalidStyles", default, 0),
            new(nameof(F.Evaluate.InvalidStylesCombination), F.Evaluate.InvalidStylesCombination, "InvalidStyles", default, 0),
            new(nameof(F.Evaluate.NullValueInvalidStyles), F.Evaluate.NullValueInvalidStyles, "InvalidStyles", default, 0),
            new(nameof(F.Evaluate.NullValueInvalidStylesAllowNull), F.Evaluate.NullValueInvalidStylesAllowNull, "InvalidStyles", default, 0),
            new(nameof(F.Evaluate.NullValue), F.Evaluate.NullValue, "Null", default, 0),
            new(nameof(F.Evaluate.NullValueAllowNull), F.Evaluate.NullValueAllowNull, "Passed", default, 0),
            new(nameof(F.Evaluate.NotADate), F.Evaluate.NotADate, "Unparsable", default, 0),
            new(nameof(F.Evaluate.NotADateAllowNull), F.Evaluate.NotADateAllowNull, "Unparsable", default, 0),
            new(nameof(F.Evaluate.Empty), F.Evaluate.Empty, "Unparsable", default, 0),
            new(nameof(F.Evaluate.EmptyAllowNull), F.Evaluate.EmptyAllowNull, "Unparsable", default, 0),
            new(nameof(F.Evaluate.WhitespaceAllowNull), F.Evaluate.WhitespaceAllowNull, "Unparsable", default, 0),
            new(nameof(F.Evaluate.MinValueProjection), F.Evaluate.MinValueProjection, "ReferenceOutOfRange", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.MaxValueProjection), F.Evaluate.MaxValueProjection, "ReferenceOutOfRange", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.MaxValueProjectionAllowNull), F.Evaluate.MaxValueProjectionAllowNull, "ReferenceOutOfRange", new DateOnly(2000, 1, 1), 1),
            new(nameof(F.Evaluate.InvalidStylesBeforeProjection), F.Evaluate.InvalidStylesBeforeProjection, "InvalidStyles", default, 0),
            new(nameof(F.Evaluate.NullValueBeforeProjection), F.Evaluate.NullValueBeforeProjection, "Null", default, 0),
            new(nameof(F.Evaluate.NullValueAllowNullBeforeProjection), F.Evaluate.NullValueAllowNullBeforeProjection, "Passed", default, 0),
            new(nameof(F.Evaluate.NotADateBeforeProjection), F.Evaluate.NotADateBeforeProjection, "Unparsable", default, 0)
        ];

        public sealed record Case(string Name, (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Value, string Expected, DateOnly ExpectedOutValue, int ExpectedClockReads)
            : ReturnOutCase<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull), string, DateOnly>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class EvaluateSystemClock
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(F.Evaluate.SystemClockFarPast), F.Evaluate.SystemClockFarPast, "Passed", new DateOnly(1, 1, 1)),
            new(nameof(F.Evaluate.SystemClockFarFuture), F.Evaluate.SystemClockFarFuture, "NotPast", new DateOnly(9999, 12, 31))
        ];

        public sealed record Case(string Name, (string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull) Value, string Expected, DateOnly ExpectedOutValue)
            : ReturnOutCase<(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull), string, DateOnly>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class Code
    {
        public static TheoryData<Case> Cases =>
        [
            new(nameof(Outcome.Passed), (int)Outcome.Passed, "date.relative.not-past"),
            new(nameof(Outcome.InvalidStyles), (int)Outcome.InvalidStyles, "date.relative.not-past"),
            new(nameof(Outcome.Null), (int)Outcome.Null, "date.relative.not-past"),
            new(nameof(Outcome.Unparsable), (int)Outcome.Unparsable, "date.format.invalid"),
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
            new(nameof(Outcome.Unparsable), (int)Outcome.Unparsable, "{paramName} must be a date in the past."),
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
            new(nameof(Outcome.Unparsable), ((int)Outcome.Unparsable, "value"), "value"),
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
            new(nameof(Outcome.Passed), ((int)Outcome.Passed, "2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), "2000-01-01"),
            new(nameof(Outcome.InvalidStyles), ((int)Outcome.InvalidStyles, "2000-01-01", DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default), DateTimeStyles.AssumeUniversal),
            new($"{nameof(Outcome.InvalidStyles)}-NullValue", ((int)Outcome.InvalidStyles, null, DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default), DateTimeStyles.AssumeUniversal),
            new(nameof(Outcome.Null), ((int)Outcome.Null, null, DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), null),
            new(nameof(Outcome.Unparsable), ((int)Outcome.Unparsable, "not-a-date", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), "not-a-date"),
            new(nameof(Outcome.ReferenceOutOfRange), ((int)Outcome.ReferenceOutOfRange, "2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), FixedTimeProvider.Default),
            new(nameof(Outcome.NotPast), ((int)Outcome.NotPast, "2999-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), "2999-01-01")
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase("Default", (0, "2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome")),
            new InvalidCase("Undefined", (99, "2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default), new ExpectedException(typeof(ArgumentOutOfRangeException), "outcome"))
        ];

        public sealed record Case(string Name, (int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider) Value, object? Expected)
            : ReturnCase<(int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider), object?>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, (int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider) Value, ExpectedException ExpectedException)
            : ThrowsCase<(int outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider)>(Name, Value, ExpectedException);
    }
}
