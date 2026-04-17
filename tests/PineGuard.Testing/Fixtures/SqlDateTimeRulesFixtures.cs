using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class SqlDateTimeRulesFixtures
{
    public static class IsInSqlDateRange
    {
        public static readonly DateOnly? NullValue = null;
        public static readonly DateOnly Typical = new(2020, 1, 1);
        public static readonly DateOnly TooEarlyDate = new(1600, 1, 1);
        public static readonly DateOnly AtMin = DateOnly.FromDateTime(SqlDateTimeRules.MinValue);
        public static readonly DateOnly AtMax = DateOnly.FromDateTime(SqlDateTimeRules.MaxValue);
        public static readonly DateOnly BelowMin = DateOnly.FromDateTime(SqlDateTimeRules.MinValue).AddDays(-1);

        public static RuleScenario<DateOnly>[] ValidScenarios =>
        [
            new(nameof(Typical), Typical, true)
        ];

        public static RuleScenario<DateOnly>[] ValidEdgeScenarios =>
        [
            new(nameof(AtMin), AtMin, true),
            new(nameof(AtMax), AtMax, true)
        ];

        public static RuleScenario<DateOnly>[] InvalidScenarios =>
        [
            new(nameof(TooEarlyDate), TooEarlyDate, false)
        ];

        public static RuleScenario<DateOnly>[] InvalidEdgeScenarios =>
        [
            new(nameof(BelowMin), BelowMin, false)
        ];

        public static RuleScenario<DateOnly>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<DateOnly>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<DateOnly>[] AllNonNullScenarios => [.. AllValid, .. AllInvalid];
        public static RuleScenario<DateOnly?>[] AllScenarios => [.. AllNonNullScenarios.Select(s => new RuleScenario<DateOnly?>(s.Name, s.Inputs, s.IsValid)), new(nameof(NullValue), NullValue, false)];
    }

    public static class IsInSqlDateTimeRangeDateTime
    {
        public static readonly DateTime? NullValue = null;
        public static readonly DateTime Typical = new(2020, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);
        public static readonly DateTime TooEarly = new(1600, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
        public static readonly DateTime AtMin = SqlDateTimeRules.MinValue;
        public static readonly DateTime AtMax = SqlDateTimeRules.MaxValue;
        public static readonly DateTime BelowMin = SqlDateTimeRules.MinValue.AddTicks(-1);
        public static readonly DateTime AboveMax = SqlDateTimeRules.MaxValue.AddMilliseconds(1);

        public static RuleScenario<DateTime>[] ValidScenarios =>
        [
            new(nameof(Typical), Typical, true)
        ];

        public static RuleScenario<DateTime>[] ValidEdgeScenarios =>
        [
            new(nameof(AtMin), AtMin, true),
            new(nameof(AtMax), AtMax, true)
        ];

        public static RuleScenario<DateTime>[] InvalidScenarios =>
        [
            new(nameof(TooEarly), TooEarly, false)
        ];

        public static RuleScenario<DateTime>[] InvalidEdgeScenarios =>
        [
            new(nameof(BelowMin), BelowMin, false),
            new(nameof(AboveMax), AboveMax, false)
        ];

        public static RuleScenario<DateTime>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<DateTime>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<DateTime>[] AllNonNullScenarios => [.. AllValid, .. AllInvalid];
        public static RuleScenario<DateTime?>[] AllScenarios => [.. AllNonNullScenarios.Select(s => new RuleScenario<DateTime?>(s.Name, s.Inputs, s.IsValid)), new(nameof(NullValue), NullValue, false)];
    }

    public static class IsInSqlDateTimeRangeDateTimeOffset
    {
        public static readonly DateTimeOffset? NullValue = null;
        public static readonly DateTimeOffset Typical = new(2020, 1, 1, 12, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset TooEarly = new(1600, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset AtMin = new(1753, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public static readonly DateTimeOffset AtMax = new(9999, 12, 31, 23, 59, 59, 997, TimeSpan.Zero);
        public static readonly DateTimeOffset BelowMin = AtMin.AddTicks(-1);
        public static readonly DateTimeOffset AboveMax = AtMax.AddMilliseconds(1);

        public static RuleScenario<DateTimeOffset>[] ValidScenarios =>
        [
            new(nameof(Typical), Typical, true)
        ];

        public static RuleScenario<DateTimeOffset>[] ValidEdgeScenarios =>
        [
            new(nameof(AtMin), AtMin, true),
            new(nameof(AtMax), AtMax, true)
        ];

        public static RuleScenario<DateTimeOffset>[] InvalidScenarios =>
        [
            new(nameof(TooEarly), TooEarly, false)
        ];

        public static RuleScenario<DateTimeOffset>[] InvalidEdgeScenarios =>
        [
            new(nameof(BelowMin), BelowMin, false),
            new(nameof(AboveMax), AboveMax, false)
        ];

        public static RuleScenario<DateTimeOffset>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<DateTimeOffset>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<DateTimeOffset>[] AllNonNullScenarios => [.. AllValid, .. AllInvalid];
        public static RuleScenario<DateTimeOffset?>[] AllScenarios => [.. AllNonNullScenarios.Select(s => new RuleScenario<DateTimeOffset?>(s.Name, s.Inputs, s.IsValid)), new(nameof(NullValue), NullValue, false)];
    }
}
