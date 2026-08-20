using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.SqlDateTimeRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

#pragma warning disable CS0618

public static class FluentSqlDateTimeExtensionsTestData
{
    public static class InSqlDateRange
    {
        public static TheoryData<FluentCase<DateOnly>> Cases => F.IsInSqlDateRange.AllNonNullScenarios.ToFluentCases(s =>
            s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be within the SQL date range."));

        public static TheoryData<FluentCase<DateOnly?>> NullableCases =>
        [
            new(nameof(F.IsInSqlDateRange.NullValue), null, new FluentExpected(true)),
            new(nameof(F.IsInSqlDateRange.Typical), F.IsInSqlDateRange.Typical, new FluentExpected(true)),
            new(nameof(F.IsInSqlDateRange.TooEarlyDate), F.IsInSqlDateRange.TooEarlyDate, new FluentExpected(false, "Value must be within the SQL date range."))
        ];
    }

    public static class InSqlDateTimeRangeOffset
    {
        public static TheoryData<FluentCase<DateTimeOffset>> Cases => F.IsInSqlDateTimeRangeDateTimeOffset.AllNonNullScenarios.ToFluentCases(s =>
            s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be within the SQL date/time range."));

        public static TheoryData<FluentCase<DateTimeOffset?>> NullableCases =>
        [
            new(nameof(F.IsInSqlDateTimeRangeDateTimeOffset.NullValue), null, new FluentExpected(true)),
            .. F.IsInSqlDateTimeRangeDateTimeOffset.AllNonNullScenarios.Select(s => new FluentCase<DateTimeOffset?>(s.Name, s.Inputs,
                s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be within the SQL date/time range.")))
        ];
    }

    public static class InSqlDateTimeRangeDateTime
    {
        public static TheoryData<FluentCase<DateTime?>> Cases
        {
            get
            {
                var td = new TheoryData<FluentCase<DateTime?>>();
                foreach (var s in F.IsInSqlDateTimeRangeDateTime.AllNonNullScenarios)
                {
                    var expected = s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be within the SQL date/time range.");
                    td.Add(new FluentCase<DateTime?>(s.Name, s.Inputs, expected));
                }
                td.Add(new FluentCase<DateTime?>(nameof(F.IsInSqlDateTimeRangeDateTime.NullValue), null, new FluentExpected(true)));
                return td;
            }
        }
    }
}
