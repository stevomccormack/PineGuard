using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringDateTimeOffsetClausesTestData
{
    private static readonly DateTimeOffset RefDto = DateTimeOffset.Parse("2020-06-15T12:00:00Z");
    private static readonly DateTimeOffset RefMin = DateTimeOffset.Parse("2020-01-01T00:00:00Z");
    private static readonly DateTimeOffset RefMax = DateTimeOffset.Parse("2020-12-31T23:59:59Z");

    public static class PastDateTimeOffset
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("past", "2000-01-01T00:00:00Z", new MustExpected(true)),
            new("offset-less assumes utc deterministically", "2000-01-01T00:00:00", new MustExpected(true)),
            new("future", "2999-01-01T00:00:00Z", new MustExpected(false, "value must be a date/time in the past.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date/time in the past."))
        ];
    }

    public static class PastOrPresentDateTimeOffset
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("past", "2000-01-01T00:00:00Z", new MustExpected(true)),
            new("future", "2999-01-01T00:00:00Z", new MustExpected(false, "value must be a date/time in the past or present.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date/time in the past or present."))
        ];
    }

    public static class FutureDateTimeOffset
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("future", "2999-01-01T00:00:00Z", new MustExpected(true)),
            new("past", "2000-01-01T00:00:00Z", new MustExpected(false, "value must be a date/time in the future.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date/time in the future."))
        ];
    }

    public static class FutureOrPresentDateTimeOffset
    {
        public static TheoryData<MustCase<string?>> Cases =>
        [
            new("future", "2999-01-01T00:00:00Z", new MustExpected(true)),
            new("past", "2000-01-01T00:00:00Z", new MustExpected(false, "value must be a date/time in the future or present.")),
            new("null-value", null, new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", "not-a-date", new MustExpected(false, "value must be a date/time in the future or present."))
        ];
    }

    public static class BetweenDateTimeOffset
    {
        public static TheoryData<MustCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> Cases =>
        [
            new("in-range", ("2020-06-15T12:00:00Z", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(true)),
            new("offset-less assumes utc deterministically", ("2020-06-15T12:00:00", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(true)),
            new("out-of-range", ("2019-01-01T00:00:00Z", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date/time within the expected range.")),
            new("on-min-exclusive", ("2020-01-01T00:00:00Z", RefMin, RefMax, Inclusion.Exclusive), new MustExpected(false, "value must be a date/time within the expected range.")),
            new("null-value", (null, RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date/time within the expected range.")),
            new("min-gt-max", ("2020-06-15T12:00:00Z", RefMax, RefMin, Inclusion.Inclusive), new MustExpected(false, "min must be less than or equal to max.", "min"))
        ];
    }

    public static class NotBetweenDateTimeOffset
    {
        public static TheoryData<MustCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)>> Cases =>
        [
            new("out-of-range", ("2019-01-01T00:00:00Z", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(true)),
            new("in-range", ("2020-06-15T12:00:00Z", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date/time not within the expected range.")),
            new("null-value", (null, RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefMin, RefMax, Inclusion.Inclusive), new MustExpected(false, "value must be a date/time not within the expected range.")),
            new("min-gt-max", ("2020-06-15T12:00:00Z", RefMax, RefMin, Inclusion.Inclusive), new MustExpected(false, "min must be less than or equal to max.", "min"))
        ];
    }

    public static class WithinDateTimeOffset
    {
        public static TheoryData<MustCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> Cases =>
        [
            new("within", ("2020-06-15T13:00:00Z", RefDto, TimeSpan.FromHours(2)), new MustExpected(true)),
            new("not-within", ("2020-06-16T12:00:00Z", RefDto, TimeSpan.FromHours(2)), new MustExpected(false, "value must be a date/time within the expected time window.")),
            new("null-value", (null, RefDto, TimeSpan.FromHours(2)), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefDto, TimeSpan.FromHours(2)), new MustExpected(false, "value must be a date/time within the expected time window.")),
            new("negative-window", ("2020-06-15T12:00:00Z", RefDto, TimeSpan.FromHours(-1)), new MustExpected(false, "window requires a non-negative time window.", "window"))
        ];
    }

    public static class NotWithinDateTimeOffset
    {
        public static TheoryData<MustCase<(string? value, DateTimeOffset? reference, TimeSpan window)>> Cases =>
        [
            new("not-within", ("2020-06-16T12:00:00Z", RefDto, TimeSpan.FromHours(2)), new MustExpected(true)),
            new("within", ("2020-06-15T13:00:00Z", RefDto, TimeSpan.FromHours(2)), new MustExpected(false, "value must be a date/time not within the expected time window.")),
            new("null-value", (null, RefDto, TimeSpan.FromHours(2)), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefDto, TimeSpan.FromHours(2)), new MustExpected(false, "value must be a date/time not within the expected time window.")),
            new("negative-window", ("2020-06-15T12:00:00Z", RefDto, TimeSpan.FromHours(-1)), new MustExpected(false, "window requires a non-negative time window.", "window"))
        ];
    }

    public static class WithinCalendarMonthsDateTimeOffset
    {
        public static TheoryData<MustCase<(string? value, DateTimeOffset? reference, int months)>> Cases =>
        [
            new("within", ("2020-07-15T12:00:00Z", RefDto, 2), new MustExpected(true)),
            new("not-within", ("2020-12-15T12:00:00Z", RefDto, 2), new MustExpected(false, "value must be a date/time within the expected number of calendar months.")),
            new("null-value", (null, RefDto, 2), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefDto, 2), new MustExpected(false, "value must be a date/time within the expected number of calendar months.")),
            new("negative-months", ("2020-06-15T12:00:00Z", RefDto, -1), new MustExpected(false, "months requires a non-negative number of months.", "months"))
        ];
    }

    public static class NotWithinCalendarMonthsDateTimeOffset
    {
        public static TheoryData<MustCase<(string? value, DateTimeOffset? reference, int months)>> Cases =>
        [
            new("not-within", ("2020-12-15T12:00:00Z", RefDto, 2), new MustExpected(true)),
            new("within", ("2020-07-15T12:00:00Z", RefDto, 2), new MustExpected(false, "value must be a date/time not within the expected number of calendar months.")),
            new("null-value", (null, RefDto, 2), new MustExpected(false, "value must not be null.", "value")),
            new("unparseable", ("not-a-date", RefDto, 2), new MustExpected(false, "value must be a date/time not within the expected number of calendar months.")),
            new("negative-months", ("2020-06-15T12:00:00Z", RefDto, -1), new MustExpected(false, "months requires a non-negative number of months.", "months"))
        ];
    }
}
