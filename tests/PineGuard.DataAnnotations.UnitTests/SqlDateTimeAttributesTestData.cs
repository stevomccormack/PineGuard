using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.SqlDateTimeRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class SqlDateTimeAttributesTestData
{
    public static class InSqlDateRangeTypeMismatch
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("wrong-type", () => new InSqlDateRangeAttribute().GetValidationResult("string", new ValidationContext(new object())), new ExpectedException(typeof(InvalidOperationException)))
        ];
    }

    public static class InSqlDateRange
    {
        public static TheoryData<DataAnnotationCase> Cases
        {
            get
            {
                var td = F.IsInSqlDateRange.AllNonNullScenarios.ToDataAnnotationCases(s =>
                    s.IsValid ? new DataAnnotationExpected(true) : new DataAnnotationExpected(false));
                td.Add(new DataAnnotationCase(nameof(F.IsInSqlDateRange.NullValue), null, new DataAnnotationExpected(true)));
                return td;
            }
        }
    }

    public static class InSqlDateTimeRangeDateTime
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsInSqlDateTimeRangeDateTime.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsInSqlDateTimeRangeDateTime.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class InSqlDateTimeRangeDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsInSqlDateTimeRangeDateTimeOffset.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsInSqlDateTimeRangeDateTimeOffset.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false)
        });
    }

    public static class InSqlDateTimeRangeWrongType
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("wrong-type", () => new InSqlDateTimeRangeAttribute().GetValidationResult("string", new ValidationContext(new object())), new ExpectedException(typeof(InvalidOperationException)))
        ];
    }
}
