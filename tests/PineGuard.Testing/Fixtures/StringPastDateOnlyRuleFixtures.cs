using System.Globalization;
using PineGuard.Testing.Common;

namespace PineGuard.Testing.Fixtures;

public static class StringPastDateOnlyRuleFixtures
{
    public static class Evaluate
    {
        private static readonly TimeZoneInfo PlusFourteen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.StringPastDateOnlyRule.PlusFourteen", TimeSpan.FromHours(14), "PineGuard Test +14", "PineGuard Test +14");
        private static readonly TimeZoneInfo MinusFourteen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.StringPastDateOnlyRule.MinusFourteen", TimeSpan.FromHours(-14), "PineGuard Test -14", "PineGuard Test -14");
        private static readonly TimeProvider MinValueProjectionFails = new FixedTimeProvider(DateTimeOffset.MinValue, MinusFourteen);
        private static readonly TimeProvider MaxValueProjectionFails = new FixedTimeProvider(DateTimeOffset.MaxValue, PlusFourteen);

        // ── Pinned clock ────────────────────────────────────────────────

        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PastDate = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PastDateAllowNull = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Yesterday = ("2026-06-14", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Today = ("2026-06-15", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) TodayAllowNull = ("2026-06-15", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Tomorrow = ("2026-06-16", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) AheadOfUtcYesterday = ("2026-06-15", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultAheadOfUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) AheadOfUtcToday = ("2026-06-16", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultAheadOfUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) AheadOfUtcTomorrow = ("2026-06-17", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultAheadOfUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) BehindUtcYesterday = ("2026-06-13", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultBehindUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) BehindUtcToday = ("2026-06-14", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultBehindUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) BehindUtcTomorrow = ("2026-06-15", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultBehindUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDate = (" 2000-01-01 ", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDateNoneStyles = (" 2000-01-01 ", DateTimeStyles.None, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PastDateNoneStyles = ("2000-01-01", DateTimeStyles.None, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) InvalidStyles = ("2000-01-01", DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) InvalidStylesAllowNull = ("2000-01-01", DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) InvalidStylesCombination = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueInvalidStyles = (null, DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueInvalidStylesAllowNull = (null, DateTimeStyles.AssumeUniversal, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValue = (null, DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueAllowNull = (null, DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NotADate = ("not-a-date", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NotADateAllowNull = ("not-a-date", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Empty = ("", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) EmptyAllowNull = ("", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) WhitespaceAllowNull = ("   ", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) MinValueProjection = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) MaxValueProjection = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, MaxValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) MaxValueProjectionAllowNull = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, MaxValueProjectionFails, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) InvalidStylesBeforeProjection = ("2000-01-01", DateTimeStyles.AssumeUniversal, MinValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueBeforeProjection = (null, DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueAllowNullBeforeProjection = (null, DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NotADateBeforeProjection = ("not-a-date", DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, false);

        // ── System clock ────────────────────────────────────────────────

        public static readonly (string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull) SystemClockFarPast = ("0001-01-01", DateTimeStyles.AllowWhiteSpaces, null, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull) SystemClockFarFuture = ("9999-12-31", DateTimeStyles.AllowWhiteSpaces, null, false);
    }
}
