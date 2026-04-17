using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class TimeOnlyUtilityTestData
{
    public static class TryTruncateToPrecision
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Hour", (new TimeOnly(10, 59, 58, 123), TimePrecision.Hour), true, new TimeOnly(10, 0, 0, 0)),
            new("Minute", (new TimeOnly(10, 59, 58, 123), TimePrecision.Minute), true, new TimeOnly(10, 59, 0, 0)),
            new("Second", (new TimeOnly(10, 59, 58, 123), TimePrecision.Second), true, new TimeOnly(10, 59, 58, 0)),
            new("Millisecond", (new TimeOnly(10, 0, 0, 0).Add(TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond + 7)), TimePrecision.Millisecond), true, new TimeOnly(10, 0, 0, 0).Add(TimeSpan.FromMilliseconds(1))),
            new("Tick (NoOp)", (new TimeOnly(10, 59, 58, 123), TimePrecision.Tick), true, new TimeOnly(10, 59, 58, 123))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null Input", (null, TimePrecision.Hour), false, default),
            new("Unknown Precision", (new TimeOnly(10, 0), (TimePrecision)123), false, default)
        ];

        public sealed record ValidCase(string Name, (TimeOnly? Value, TimePrecision Precision) Value, bool Expected, TimeOnly ExpectedOutValue)
            : TryCase<(TimeOnly? Value, TimePrecision Precision), TimeOnly>(Name, Value, Expected, ExpectedOutValue);
    }
}
