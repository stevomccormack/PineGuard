using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TimeAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
        // PastAttribute etc. throw on wrong type?
        // Check TimeAttributes.cs: "throw new InvalidOperationException..."
        // So we cannot list wrong type in valid/invalid cases if it throws.
    ];

    private static readonly DateTime TodayDateTime = DateTime.UtcNow.Date;
    private static readonly DateOnly TodayDateOnly = DateOnly.FromDateTime(TodayDateTime);

    // PastAttribute supports DateOnly, DateTime, DateTimeOffset
    public static class Past
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("dateonly", TodayDateOnly.AddDays(-1), true),
            new("datetime", DateTime.Now.AddSeconds(-10), true),
            new("offset", DateTimeOffset.Now.AddSeconds(-10), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("dateonly future", TodayDateOnly.AddDays(1), false),
            new("datetime future", DateTime.Now.AddSeconds(10), false),
            new("offset future", DateTimeOffset.Now.AddSeconds(10), false)
        ];
    }

    // PastOrPresentAttribute
    public static class PastOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("dateonly present", TodayDateOnly, true),
            new("datetime present", DateTime.Now, true),
            new("offset present", DateTimeOffset.Now, true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("dateonly future", TodayDateOnly.AddDays(1), false)
        ];
    }

    // FutureAttribute
    public static class Future
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("dateonly future", TodayDateOnly.AddDays(1), true),
            new("datetime future", DateTime.Now.AddSeconds(10), true),
            new("offset future", DateTimeOffset.Now.AddSeconds(10), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("dateonly past", TodayDateOnly.AddDays(-1), false),
            new("offset past", DateTimeOffset.Now.AddSeconds(-10), false)
        ];
    }

    // FutureOrPresentAttribute
    public static class FutureOrPresent
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("dateonly present", TodayDateOnly, true),
            new("datetime future", DateTime.Now.AddSeconds(10), true),
            new("offset future", DateTimeOffset.Now.AddSeconds(10), true)
        ];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases =>
        [
            new("datetime past", DateTime.Now.AddSeconds(-10), false),
            new("offset past", DateTimeOffset.Now.AddSeconds(-10), false)
        ];
    }

    // DateOnlyBetweenAttribute("2020-01-01", "2020-01-31")
    public static class DateOnlyBetween
    {
        public static TheoryData<ValidCase> ValidCases => [new("mid", new DateOnly(2020, 1, 15), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("out", new DateOnly(2020, 2, 1), false)];
    }

    // UtcAttribute
    public static class Utc
    {
        public static TheoryData<ValidCase> ValidCases => [new("utc", DateTime.UtcNow, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("local", DateTime.Now, false)];
    }

    // LocalAttribute
    public static class Local
    {
        public static TheoryData<ValidCase> ValidCases => [new("local", DateTime.Now, true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("utc", DateTime.UtcNow, false)];
    }

    // UnspecifiedAttribute
    public static class Unspecified
    {
        public static TheoryData<ValidCase> ValidCases => [new("unspecified", new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), true)];
        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();
        public static TheoryData<ValidCase> InvalidCases => [new("utc", DateTime.UtcNow, false)];
    }

    public static class UnsupportedType
    {
        public sealed record ThrowCase(string Name, ValidationAttribute Attribute, object Value, string ExpectedMessageContains)
        {
            public override string ToString() => Name;
        }

        public static TheoryData<ThrowCase> Cases =>
        [
            new("Past int", new PastAttribute(), 42, "does not support type Int32"),
            new("PastOrPresent int", new PastOrPresentAttribute(), 42, "does not support type Int32"),
            new("Future int", new FutureAttribute(), 42, "does not support type Int32"),
            new("FutureOrPresent int", new FutureOrPresentAttribute(), 42, "does not support type Int32")
        ];
    }

    // The polymorphic attributes dispatch on the runtime type, so each clock-injection group carries one row
    // per supported type: a switch arm that dropped the resolved provider would still pass on the other two.
    // See DateOnlyAttributesTestData for why the 2100 subject and the 2000/2200 clocks are chosen.
    private static readonly DateOnly ClockSubjectDate = new(2100, 01, 01);
    private static readonly DateTime ClockSubjectDateTime = new(2100, 01, 01, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTimeOffset ClockSubjectDateTimeOffset = new(2100, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockAfterSubject = new(2200, 01, 01, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset ClockBeforeSubject = new(2000, 01, 01, 12, 0, 0, TimeSpan.Zero);

    public static class PastOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("DateOnlyClockAfterTheSubject", ((object)ClockSubjectDate, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("DateTimeClockAfterTheSubject", ((object)ClockSubjectDateTime, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("DateTimeOffsetClockAfterTheSubject", ((object)ClockSubjectDateTimeOffset, ClockAfterSubject), new DataAnnotationExpected(true)),
            new("DateOnlyClockBeforeTheSubject", ((object)ClockSubjectDate, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be in the past.", Code: MustCodes.Date.Relative.NotPast)),
            new("DateTimeClockBeforeTheSubject", ((object)ClockSubjectDateTime, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be in the past.", Code: MustCodes.Date.Relative.NotPast)),
            new("DateTimeOffsetClockBeforeTheSubject", ((object)ClockSubjectDateTimeOffset, ClockBeforeSubject), new DataAnnotationExpected(false, "Value must be in the past.", Code: MustCodes.Date.Relative.NotPast))
        ];
    }

    public static class FutureOnAnInjectedClock
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("DateOnlyClockBeforeTheSubject", ((object)ClockSubjectDate, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("DateTimeClockBeforeTheSubject", ((object)ClockSubjectDateTime, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("DateTimeOffsetClockBeforeTheSubject", ((object)ClockSubjectDateTimeOffset, ClockBeforeSubject), new DataAnnotationExpected(true)),
            new("DateOnlyClockAfterTheSubject", ((object)ClockSubjectDate, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be in the future.", Code: MustCodes.Date.Relative.NotFuture)),
            new("DateTimeClockAfterTheSubject", ((object)ClockSubjectDateTime, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be in the future.", Code: MustCodes.Date.Relative.NotFuture)),
            new("DateTimeOffsetClockAfterTheSubject", ((object)ClockSubjectDateTimeOffset, ClockAfterSubject), new DataAnnotationExpected(false, "Value must be in the future.", Code: MustCodes.Date.Relative.NotFuture))
        ];
    }
}
