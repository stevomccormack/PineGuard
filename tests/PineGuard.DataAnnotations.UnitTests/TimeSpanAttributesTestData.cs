using PineGuard.Testing.UnitTests.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TimeSpanAttributesTestData
{
    public static class DurationBetweenTimeSpan
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("min",       TimeSpan.Parse("00:00:01"), new DataAnnotationExpected(true)),
            new("mid",       TimeSpan.Parse("00:00:05"), new DataAnnotationExpected(true)),
            new("max",       TimeSpan.Parse("00:00:10"), new DataAnnotationExpected(true)),
            new("null",      null,                        new DataAnnotationExpected(true)),
            new("too small", TimeSpan.Parse("00:00:00"), new DataAnnotationExpected(false)),
            new("too large", TimeSpan.Parse("00:00:11"), new DataAnnotationExpected(false))
        ];
    }

    public static class NotDurationBetweenTimeSpan
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("too small", TimeSpan.Parse("00:00:00"), new DataAnnotationExpected(true)),
            new("too large", TimeSpan.Parse("00:00:11"), new DataAnnotationExpected(true)),
            new("null",      null,                        new DataAnnotationExpected(true)),
            new("min",       TimeSpan.Parse("00:00:01"), new DataAnnotationExpected(false)),
            new("mid",       TimeSpan.Parse("00:00:05"), new DataAnnotationExpected(false)),
            new("max",       TimeSpan.Parse("00:00:10"), new DataAnnotationExpected(false))
        ];
    }

    public static class GreaterThanTimeSpan
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("greater", TimeSpan.Parse("00:00:06"), new DataAnnotationExpected(true)),
            new("null",    null,                        new DataAnnotationExpected(true)),
            new("equal",   TimeSpan.Parse("00:00:05"), new DataAnnotationExpected(false)),
            new("less",    TimeSpan.Parse("00:00:04"), new DataAnnotationExpected(false))
        ];
    }

    public static class LessThanTimeSpan
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("less",    TimeSpan.Parse("00:00:09"), new DataAnnotationExpected(true)),
            new("null",    null,                        new DataAnnotationExpected(true)),
            new("equal",   TimeSpan.Parse("00:00:10"), new DataAnnotationExpected(false)),
            new("greater", TimeSpan.Parse("00:00:11"), new DataAnnotationExpected(false))
        ];
    }
}
