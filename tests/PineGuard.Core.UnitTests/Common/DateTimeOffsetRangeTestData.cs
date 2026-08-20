using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class DateTimeOffsetRangeTestData
{
    public static class Equality
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("end different", () =>
            {
                var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
                var range = new DateTimeOffsetRange(start, start.AddDays(10));
                var endDifferent = new DateTimeOffsetRange(start, start.AddDays(11));
                Assert.False(range.Equals(endDifferent));
            }),
            new("start different", () =>
            {
                var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
                var range = new DateTimeOffsetRange(start, start.AddDays(10));
                var startDifferent = new DateTimeOffsetRange(start.AddDays(-1), start.AddDays(10));
                Assert.False(range.Equals(startDifferent));
            })
        ];

        public sealed record ValidCase(string Name, Action Value)
            : ValueCase<Action>(Name, Value);
    }

    public static class IntersectAndUnion
    {
        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("other starts before", () =>
            {
                var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
                var range = new DateTimeOffsetRange(start, start.AddDays(10));
                var otherStartsBefore = new DateTimeOffsetRange(start.AddDays(-9), start.AddDays(5));
                var intersect1 = range.Intersect(otherStartsBefore);
                Assert.NotNull(intersect1);
                Assert.Equal(start, intersect1.Value.Start);
                Assert.Equal(start.AddDays(5), intersect1.Value.End);

                var union1 = range.Union(otherStartsBefore);
                Assert.Equal(start.AddDays(-9), union1.Start);
                Assert.Equal(start.AddDays(10), union1.End);
            }),
            new("other starts after", () =>
            {
                var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
                var range = new DateTimeOffsetRange(start, start.AddDays(10));
                var otherStartsAfter = new DateTimeOffsetRange(start.AddDays(5), start.AddDays(20));
                var intersect2 = range.Intersect(otherStartsAfter);
                Assert.NotNull(intersect2);
                Assert.Equal(start.AddDays(5), intersect2.Value.Start);
                Assert.Equal(start.AddDays(10), intersect2.Value.End);

                var union2 = range.Union(otherStartsAfter);
                Assert.Equal(start, union2.Start);
                Assert.Equal(start.AddDays(20), union2.End);
            }),
            new("other touches at boundary", () =>
            {
                var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
                var range = new DateTimeOffsetRange(start, start.AddDays(10));
                var touchingAtEnd = new DateTimeOffsetRange(start.AddDays(10), start.AddDays(15));
                var intersect3 = range.Intersect(touchingAtEnd);
                Assert.NotNull(intersect3);
                Assert.Equal(start.AddDays(10), intersect3.Value.Start);
                Assert.Equal(start.AddDays(10), intersect3.Value.End);

                var point = new DateTimeOffsetRange(start.AddDays(30), start.AddDays(30));
                var selfIntersect = point.Intersect(point);
                Assert.NotNull(selfIntersect);
                Assert.Equal(point.Start, selfIntersect.Value.Start);
                Assert.Equal(point.End, selfIntersect.Value.End);
            })
        ];

        public sealed record ValidCase(string Name, Action Value)
            : ValueCase<Action>(Name, Value);
    }

    public static class Overlaps
    {
        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("touching before", () =>
            {
                var range = new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 20, 0, 0, 0, TimeSpan.Zero));
                var beforeTouching = new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero));
                Assert.False(range.Overlaps(beforeTouching));
                Assert.True(range.Overlaps(beforeTouching, Inclusion.Inclusive));

                var strictlyBefore = new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 09, 0, 0, 0, TimeSpan.Zero));
                Assert.False(range.Overlaps(strictlyBefore, Inclusion.Inclusive));
            }),
            new("touching after", () =>
            {
                var range = new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 20, 0, 0, 0, TimeSpan.Zero));
                var afterTouching = new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 20, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 30, 0, 0, 0, TimeSpan.Zero));
                Assert.False(range.Overlaps(afterTouching));
                Assert.True(range.Overlaps(afterTouching, Inclusion.Inclusive));

                var strictlyAfter = new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 21, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2024, 01, 30, 0, 0, 0, TimeSpan.Zero));
                Assert.False(range.Overlaps(strictlyAfter, Inclusion.Inclusive));
            })
        ];

        public sealed record ValidCase(string Name, Action Value)
            : ValueCase<Action>(Name, Value);
    }

    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("utc+00 same", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero),
            new("utc+00 +1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)),
            new("+01 90 minutes", new DateTimeOffset(2020, 01, 01, 12, 00, 00, TimeSpan.FromHours(1)), new DateTimeOffset(2020, 01, 01, 13, 30, 00, TimeSpan.FromHours(1)), TimeSpan.FromMinutes(90)),
            new("leap day", new DateTimeOffset(2020, 02, 29, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 03, 01, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(1)),
            new("-05 same", new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), TimeSpan.Zero),
            new("-05 +1 second", new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2021, 01, 01, 00, 00, 01, TimeSpan.FromHours(-5)), TimeSpan.FromSeconds(1)),
            new("utc+00 +1 hour", new DateTimeOffset(2022, 06, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2022, 06, 01, 01, 00, 00, TimeSpan.Zero), TimeSpan.FromHours(1)),
            new("-05 30 minutes", new DateTimeOffset(2023, 11, 05, 01, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2023, 11, 05, 01, 30, 00, TimeSpan.FromHours(-5)), TimeSpan.FromMinutes(30)),
            new("utc+00 2024 year span", new DateTimeOffset(2024, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2024, 12, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(365)),
            new("utc+00 2025 year span", new DateTimeOffset(2025, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(364)),
            new("utc+00 +1 second (2030)", new DateTimeOffset(2030, 04, 15, 10, 00, 00, TimeSpan.Zero), new DateTimeOffset(2030, 04, 15, 10, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)),
            new("+10 12 hours", new DateTimeOffset(2031, 08, 01, 00, 00, 00, TimeSpan.FromHours(10)), new DateTimeOffset(2031, 08, 01, 12, 00, 00, TimeSpan.FromHours(10)), TimeSpan.FromHours(12)),
            new("utc+00 boundary second", new DateTimeOffset(2032, 10, 30, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromSeconds(1)),
            new("utc+00 30 days", new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(30)),
            new("utc+00 feb 2000", new DateTimeOffset(2000, 02, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(28)),
            new("+02 same", new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2)), new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2)), TimeSpan.Zero),
            new("+03 +2 seconds", new DateTimeOffset(2011, 06, 01, 00, 00, 00, TimeSpan.FromHours(3)), new DateTimeOffset(2011, 06, 01, 00, 00, 02, TimeSpan.FromHours(3)), TimeSpan.FromSeconds(2)),
            new("-03 +2 seconds", new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), TimeSpan.FromSeconds(2)),
            new("utc+00 +10 seconds", new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 10, TimeSpan.Zero), TimeSpan.FromSeconds(10)),
            new("utc+00 +1 day", new DateTimeOffset(2027, 12, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2027, 12, 02, 00, 00, 00, TimeSpan.Zero), TimeSpan.FromDays(1))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("utc+00 -1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day", new DateTimeOffset(2020, 01, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (march)", new DateTimeOffset(2020, 03, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 03, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("-05 previous day", new DateTimeOffset(2021, 01, 02, 00, 00, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2021, 01, 01, 00, 00, 00, TimeSpan.FromHours(-5)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 hour reversed", new DateTimeOffset(2022, 06, 01, 01, 00, 00, TimeSpan.Zero), new DateTimeOffset(2022, 06, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("-05 minute reversed", new DateTimeOffset(2023, 11, 05, 01, 30, 00, TimeSpan.FromHours(-5)), new DateTimeOffset(2023, 11, 05, 01, 00, 00, TimeSpan.FromHours(-5)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 -1 second (2024)", new DateTimeOffset(2024, 12, 31, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(2024, 12, 31, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 -1 second (2030)", new DateTimeOffset(2030, 04, 15, 10, 00, 01, TimeSpan.Zero), new DateTimeOffset(2030, 04, 15, 10, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (1999)", new DateTimeOffset(1999, 01, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2000)", new DateTimeOffset(2000, 02, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("+02 -1 second", new DateTimeOffset(2010, 05, 01, 00, 00, 01, TimeSpan.FromHours(2)), new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("+03 -1 second", new DateTimeOffset(2011, 06, 01, 00, 00, 02, TimeSpan.FromHours(3)), new DateTimeOffset(2011, 06, 01, 00, 00, 01, TimeSpan.FromHours(3)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("-03 -1 second", new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 01, TimeSpan.FromHours(-3)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2013)", new DateTimeOffset(2013, 01, 10, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2013, 01, 09, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2014)", new DateTimeOffset(2014, 06, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2014, 06, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2015)", new DateTimeOffset(2015, 08, 16, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2015, 08, 15, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2016)", new DateTimeOffset(2016, 11, 03, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2016, 11, 02, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2017)", new DateTimeOffset(2017, 12, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2017, 12, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2018)", new DateTimeOffset(2018, 10, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2018, 10, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2027)", new DateTimeOffset(2027, 12, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2027, 12, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("min+1tick before min", DateTimeOffset.MinValue.AddTicks(1), DateTimeOffset.MinValue, new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("max after max-1tick", DateTimeOffset.MaxValue, DateTimeOffset.MaxValue.AddTicks(-1), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("year 1 reversed", new DateTimeOffset(0001, 01, 01, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(0001, 01, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("max date reversed", new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(9999, 12, 31, 23, 59, 58, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("leap day reversed", new DateTimeOffset(2024, 02, 29, 12, 00, 01, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("+14 reversed", new DateTimeOffset(2025, 12, 31, 00, 00, 01, TimeSpan.FromHours(14)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(14)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("-12 reversed", new DateTimeOffset(2025, 12, 31, 00, 00, 01, TimeSpan.FromHours(-12)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(-12)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 reversed (2026)", new DateTimeOffset(2026, 11, 05, 00, 00, 10, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("utc+00 previous day (2027)", new DateTimeOffset(2027, 12, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2027, 12, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("+05 reversed hour", new DateTimeOffset(2020, 01, 01, 01, 00, 00, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(5)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("-07 reversed second", new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.FromHours(-7)), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("2032 reversed", new DateTimeOffset(2032, 10, 31, 00, 00, 10, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("1999 previous day", new DateTimeOffset(1999, 01, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("2000 previous day", new DateTimeOffset(2000, 02, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("+02 reversed second", new DateTimeOffset(2010, 05, 01, 00, 00, 01, TimeSpan.FromHours(2)), new DateTimeOffset(2010, 05, 01, 00, 00, 00, TimeSpan.FromHours(2)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("+03 reversed second", new DateTimeOffset(2011, 06, 01, 00, 00, 02, TimeSpan.FromHours(3)), new DateTimeOffset(2011, 06, 01, 00, 00, 01, TimeSpan.FromHours(3)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("-03 reversed second", new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 01, TimeSpan.FromHours(-3)), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("2013 previous day", new DateTimeOffset(2013, 01, 10, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2013, 01, 09, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("2014 previous day", new DateTimeOffset(2014, 06, 02, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2014, 06, 01, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("2015 previous day", new DateTimeOffset(2015, 08, 16, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2015, 08, 15, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal")),
            new("2016 previous day", new DateTimeOffset(2016, 11, 03, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2016, 11, 02, 00, 00, 00, TimeSpan.Zero), new ExpectedException(typeof(ArgumentException), "start", "less than or equal"))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("min-min", DateTimeOffset.MinValue, DateTimeOffset.MinValue, TimeSpan.Zero),
            new("max-max", DateTimeOffset.MaxValue, DateTimeOffset.MaxValue, TimeSpan.Zero),
            new("year 1 +1 second", new DateTimeOffset(0001, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(0001, 01, 01, 00, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)),
            new("max date same", new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), TimeSpan.Zero),
            new("leap day same", new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), TimeSpan.Zero),
            new("leap day +1 second", new DateTimeOffset(2024, 02, 29, 12, 00, 00, TimeSpan.Zero), new DateTimeOffset(2024, 02, 29, 12, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)),
            new("+14 same", new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(14)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(14)), TimeSpan.Zero),
            new("-12 same", new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(-12)), new DateTimeOffset(2025, 12, 31, 00, 00, 00, TimeSpan.FromHours(-12)), TimeSpan.Zero),
            new("utc+00 same", new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero),
            new("utc+00 +1 second", new DateTimeOffset(2026, 11, 05, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2026, 11, 05, 00, 00, 01, TimeSpan.Zero), TimeSpan.FromSeconds(1)),
            new("+05 +1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.FromHours(5)), TimeSpan.FromSeconds(1)),
            new("+05 +1 hour", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(5)), new DateTimeOffset(2020, 01, 01, 01, 00, 00, TimeSpan.FromHours(5)), TimeSpan.FromHours(1)),
            new("-07 same", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), TimeSpan.Zero),
            new("-07 +1 second", new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.FromHours(-7)), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.FromHours(-7)), TimeSpan.FromSeconds(1)),
            new("2032 same", new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero),
            new("2032 +10 seconds", new DateTimeOffset(2032, 10, 31, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2032, 10, 31, 00, 00, 10, TimeSpan.Zero), TimeSpan.FromSeconds(10)),
            new("1999 same", new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(1999, 01, 01, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero),
            new("2000 leap same", new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), TimeSpan.Zero),
            new("-03 same", new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), TimeSpan.Zero),
            new("-03 +2 seconds", new DateTimeOffset(2012, 07, 01, 12, 00, 00, TimeSpan.FromHours(-3)), new DateTimeOffset(2012, 07, 01, 12, 00, 02, TimeSpan.FromHours(-3)), TimeSpan.FromSeconds(2))
        ];

        public sealed record ValidCase(string Name, DateTimeOffset Start, DateTimeOffset End, TimeSpan ExpectedDuration)
            : ReturnCase<(DateTimeOffset Start, DateTimeOffset End), TimeSpan>(Name, (Start, End), ExpectedDuration);

        public sealed record InvalidCase(string Name, DateTimeOffset Start, DateTimeOffset End, ExpectedException ExpectedException)
            : ThrowsCase<(DateTimeOffset Start, DateTimeOffset End)>(Name, (Start, End), ExpectedException);
    }

    public static class TryCreate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", (new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero)), true, new DateTimeOffsetRange(new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero)), false, default),
            new("end null", (new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), null), false, default),
            new("start after end", (new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero)), false, default)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset? Start, DateTimeOffset? End) Input, bool Expected, DateTimeOffsetRange ExpectedOutValue)
            : TryCase<(DateTimeOffset? Start, DateTimeOffset? End), DateTimeOffsetRange>(Name, Input, Expected, ExpectedOutValue);
    }
}
