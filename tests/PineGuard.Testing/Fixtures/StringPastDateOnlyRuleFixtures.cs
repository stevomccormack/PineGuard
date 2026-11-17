using System.Globalization;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class StringPastDateOnlyRuleFixtures
{
    public static class Evaluate
    {
        // Today as the pinned clock reports it, derived from FixedTimeProvider.Default rather than
        // restated as a literal. Because that instant is in the real past, "Tomorrow" is a date the
        // machine clock considers past — so a rule that ignored the supplied provider would fail here.
        private static readonly DateOnly PinnedToday = DateOnly.FromDateTime(FixedTimeProvider.Default.GetUtcNow().UtcDateTime);

        private static readonly TimeZoneInfo PlusFourteen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.StringPastDateOnlyRule.PlusFourteen", TimeSpan.FromHours(14), "PineGuard Test +14", "PineGuard Test +14");
        private static readonly TimeZoneInfo MinusFourteen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.StringPastDateOnlyRule.MinusFourteen", TimeSpan.FromHours(-14), "PineGuard Test -14", "PineGuard Test -14");
        private static readonly TimeProvider MinValueProjectionFails = new FixedTimeProvider(DateTimeOffset.MinValue, MinusFourteen);
        private static readonly TimeProvider MaxValueProjectionFails = new FixedTimeProvider(DateTimeOffset.MaxValue, PlusFourteen);

        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PastDate = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PastDateAllowNull = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Yesterday = (Iso(PinnedToday.AddDays(-1)), DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Today = (Iso(PinnedToday), DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) TodayAllowNull = (Iso(PinnedToday), DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) Tomorrow = (Iso(PinnedToday.AddDays(1)), DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) AheadOfUtcYesterday = ("2026-06-15", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultAheadOfUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) AheadOfUtcToday = ("2026-06-16", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultAheadOfUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) AheadOfUtcTomorrow = ("2026-06-17", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultAheadOfUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) BehindUtcYesterday = ("2026-06-13", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultBehindUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) BehindUtcToday = ("2026-06-14", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultBehindUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) BehindUtcTomorrow = ("2026-06-15", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.DefaultBehindUtc, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDate = (" 2000-01-01 ", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDateNoneStyles = (" 2000-01-01 ", DateTimeStyles.None, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PastDateNoneStyles = ("2000-01-01", DateTimeStyles.None, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) InnerSpacedPastDateNoneStyles = ("2000 - 01 - 01", DateTimeStyles.None, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDateLeadingWhiteStyles = (" 2000-01-01 ", DateTimeStyles.AllowLeadingWhite, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDateTrailingWhiteStyles = (" 2000-01-01 ", DateTimeStyles.AllowTrailingWhite, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) PaddedPastDateInnerWhiteStyles = (" 2000-01-01 ", DateTimeStyles.AllowInnerWhite, FixedTimeProvider.Default, false);
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
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) SpaceInsideComponent = ("2000-0 1-01", DateTimeStyles.AllowWhiteSpaces, FixedTimeProvider.Default, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) MinValueProjection = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) MaxValueProjection = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, MaxValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) MaxValueProjectionAllowNull = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, MaxValueProjectionFails, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) InvalidStylesBeforeProjection = ("2000-01-01", DateTimeStyles.AssumeUniversal, MinValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueBeforeProjection = (null, DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NullValueAllowNullBeforeProjection = (null, DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, true);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull) NotADateBeforeProjection = ("not-a-date", DateTimeStyles.AllowWhiteSpaces, MinValueProjectionFails, false);

        public static RuleScenario<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull)>[] ValidScenarios =>
        [
            new(nameof(PastDate), PastDate, true),
            new(nameof(PastDateAllowNull), PastDateAllowNull, true),
            new(nameof(Yesterday), Yesterday, true),
            new(nameof(AheadOfUtcYesterday), AheadOfUtcYesterday, true),
            new(nameof(BehindUtcYesterday), BehindUtcYesterday, true),
            new(nameof(PaddedPastDate), PaddedPastDate, true),
            new(nameof(PaddedPastDateNoneStyles), PaddedPastDateNoneStyles, true),
            new(nameof(PastDateNoneStyles), PastDateNoneStyles, true),
            new(nameof(InnerSpacedPastDateNoneStyles), InnerSpacedPastDateNoneStyles, true),
            new(nameof(PaddedPastDateLeadingWhiteStyles), PaddedPastDateLeadingWhiteStyles, true),
            new(nameof(PaddedPastDateTrailingWhiteStyles), PaddedPastDateTrailingWhiteStyles, true),
            new(nameof(PaddedPastDateInnerWhiteStyles), PaddedPastDateInnerWhiteStyles, true),
            new(nameof(NullValueAllowNull), NullValueAllowNull, true),
            new(nameof(NullValueAllowNullBeforeProjection), NullValueAllowNullBeforeProjection, true)
        ];

        public static RuleScenario<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull)>[] InvalidScenarios =>
        [
            new(nameof(Today), Today, false),
            new(nameof(TodayAllowNull), TodayAllowNull, false),
            new(nameof(Tomorrow), Tomorrow, false),
            new(nameof(AheadOfUtcToday), AheadOfUtcToday, false),
            new(nameof(AheadOfUtcTomorrow), AheadOfUtcTomorrow, false),
            new(nameof(BehindUtcToday), BehindUtcToday, false),
            new(nameof(BehindUtcTomorrow), BehindUtcTomorrow, false),
            new(nameof(InvalidStyles), InvalidStyles, false),
            new(nameof(InvalidStylesAllowNull), InvalidStylesAllowNull, false),
            new(nameof(InvalidStylesCombination), InvalidStylesCombination, false),
            new(nameof(NullValueInvalidStyles), NullValueInvalidStyles, false),
            new(nameof(NullValueInvalidStylesAllowNull), NullValueInvalidStylesAllowNull, false),
            new(nameof(NullValue), NullValue, false),
            new(nameof(NotADate), NotADate, false),
            new(nameof(NotADateAllowNull), NotADateAllowNull, false),
            new(nameof(Empty), Empty, false),
            new(nameof(EmptyAllowNull), EmptyAllowNull, false),
            new(nameof(WhitespaceAllowNull), WhitespaceAllowNull, false),
            new(nameof(SpaceInsideComponent), SpaceInsideComponent, false),
            new(nameof(MinValueProjection), MinValueProjection, false),
            new(nameof(MaxValueProjection), MaxValueProjection, false),
            new(nameof(MaxValueProjectionAllowNull), MaxValueProjectionAllowNull, false),
            new(nameof(InvalidStylesBeforeProjection), InvalidStylesBeforeProjection, false),
            new(nameof(NullValueBeforeProjection), NullValueBeforeProjection, false),
            new(nameof(NotADateBeforeProjection), NotADateBeforeProjection, false)
        ];

        public static RuleScenario<(string? value, DateTimeStyles styles, TimeProvider timeProvider, bool allowNull)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];

        private static string Iso(DateOnly value) => value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public static class EvaluateSystemClock
    {
        public static readonly (string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull) SystemClockFarPast = ("2000-01-01", DateTimeStyles.AllowWhiteSpaces, null, false);
        public static readonly (string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull) SystemClockFarFuture = ("2999-01-01", DateTimeStyles.AllowWhiteSpaces, null, false);

        public static RuleScenario<(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull)>[] ValidScenarios =>
        [
            new(nameof(SystemClockFarPast), SystemClockFarPast, true)
        ];

        public static RuleScenario<(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull)>[] InvalidScenarios =>
        [
            new(nameof(SystemClockFarFuture), SystemClockFarFuture, false)
        ];

        public static RuleScenario<(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
