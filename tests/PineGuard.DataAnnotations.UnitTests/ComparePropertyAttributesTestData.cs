using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

internal sealed class TemporalCompareModel
{
    public DateOnly DateOnlyOther { get; set; } = new(2024, 6, 15);
    public TimeOnly TimeOnlyOther { get; set; } = new(12, 0, 0);
    public DateTime DateTimeOther { get; set; } = new(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc);
    public DateTimeOffset DateTimeOffsetOther { get; set; } = new(2024, 6, 15, 12, 0, 0, TimeSpan.Zero);
    public DateTime? NullDateTimeOther { get; set; }
    public DateTime FieldDateTimeOther = new(2024, 3, 1, 0, 0, 0, DateTimeKind.Utc);
}

internal sealed class NumericCompareModel
{
    public int IntOther { get; set; } = 10;
    public int? NullIntOther { get; set; }
    public long LongOther { get; set; } = 10L;
}

internal sealed class EqualityCompareModel
{
    public string StringOther { get; set; } = "expected";
    public int IntOtherMismatch { get; set; } = 42;
}

public static class ComparePropertyAttributesTestData
{
    public static class AfterDateOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateOnly(2024, 6, 16), new DataAnnotationExpected(true)),
            new("before", new DateOnly(2024, 6, 14), new DataAnnotationExpected(false, "Value must be after the specified date.", Code: MustCodes.Date.Order.NotAfter))
        ];
    }

    public static class AfterTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new TimeOnly(13, 0, 0), new DataAnnotationExpected(true)),
            new("before", new TimeOnly(11, 0, 0), new DataAnnotationExpected(false, "Value must be after the specified time."))
        ];
    }

    public static class AfterDateTime
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateTime(2024, 6, 15, 13, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(true)),
            new("before", new DateTime(2024, 6, 15, 11, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(false, "Value must be after the specified date/time.")),
            new("null-value", null, new DataAnnotationExpected(true))
        ];
    }

    public static class AfterDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateTimeOffset(2024, 6, 15, 13, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(true)),
            new("before", new DateTimeOffset(2024, 6, 15, 11, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(false, "Value must be after the specified date/time."))
        ];
    }

    public static class AfterFieldOther
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateTime(2024, 3, 2, 0, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(true)),
            new("before", new DateTime(2024, 2, 28, 0, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(false, "Value must be after the specified date/time."))
        ];
    }

    public static class AfterUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("type-mismatch", () => new AfterPropertyAttribute(nameof(TemporalCompareModel.DateOnlyOther)).GetValidationResult(new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc), new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'DateOnlyOther' to resolve to the same type")),
            new ActionThrowsCase("null-other", () => new AfterPropertyAttribute(nameof(TemporalCompareModel.NullDateTimeOther)).GetValidationResult(new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc), new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'NullDateTimeOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class AfterMissingMember
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("missing-member", () => new AfterPropertyAttribute("DoesNotExist").GetValidationResult(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "could not find a public property or field named 'DoesNotExist'"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class OnOrAfterDateOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateOnly(2024, 6, 16), new DataAnnotationExpected(true)),
            new("before", new DateOnly(2024, 6, 14), new DataAnnotationExpected(false, "Value must be on or after the specified date.", Code: MustCodes.Date.Order.Before))
        ];
    }

    public static class OnOrAfterTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new TimeOnly(13, 0, 0), new DataAnnotationExpected(true)),
            new("before", new TimeOnly(11, 0, 0), new DataAnnotationExpected(false, "Value must be on or after the specified time."))
        ];
    }

    public static class OnOrAfter
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateTime(2024, 6, 15, 13, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(true)),
            new("before", new DateTime(2024, 6, 15, 11, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(false, "Value must be on or after the specified date/time."))
        ];
    }

    public static class OnOrAfterDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("after", new DateTimeOffset(2024, 6, 15, 13, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(true)),
            new("before", new DateTimeOffset(2024, 6, 15, 11, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(false, "Value must be on or after the specified date/time."))
        ];
    }

    public static class OnOrAfterUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("type-mismatch", () => new OnOrAfterPropertyAttribute(nameof(TemporalCompareModel.DateOnlyOther)).GetValidationResult(new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc), new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'DateOnlyOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class Before
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new DateOnly(2024, 6, 14), new DataAnnotationExpected(true)),
            new("after", new DateOnly(2024, 6, 16), new DataAnnotationExpected(false, "Value must be before the specified date."))
        ];
    }

    public static class BeforeTimeOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new TimeOnly(11, 0, 0), new DataAnnotationExpected(true)),
            new("after", new TimeOnly(13, 0, 0), new DataAnnotationExpected(false, "Value must be before the specified time."))
        ];
    }

    public static class BeforeDateTime
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new DateTime(2024, 6, 15, 11, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(true)),
            new("after", new DateTime(2024, 6, 15, 13, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(false, "Value must be before the specified date/time."))
        ];
    }

    public static class BeforeDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new DateTimeOffset(2024, 6, 15, 11, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(true)),
            new("after", new DateTimeOffset(2024, 6, 15, 13, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(false, "Value must be before the specified date/time."))
        ];
    }

    public static class BeforeUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("type-mismatch", () => new BeforePropertyAttribute(nameof(TemporalCompareModel.TimeOnlyOther)).GetValidationResult(new DateOnly(2024, 6, 15), new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'TimeOnlyOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class OnOrBeforeDateOnly
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new DateOnly(2024, 6, 14), new DataAnnotationExpected(true)),
            new("after", new DateOnly(2024, 6, 16), new DataAnnotationExpected(false, "Value must be on or before the specified date.", Code: MustCodes.Date.Order.After))
        ];
    }

    public static class OnOrBefore
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new TimeOnly(11, 0, 0), new DataAnnotationExpected(true)),
            new("after", new TimeOnly(13, 0, 0), new DataAnnotationExpected(false, "Value must be on or before the specified time."))
        ];
    }

    public static class OnOrBeforeDateTime
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new DateTime(2024, 6, 15, 11, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(true)),
            new("after", new DateTime(2024, 6, 15, 13, 0, 0, DateTimeKind.Utc), new DataAnnotationExpected(false, "Value must be on or before the specified date/time."))
        ];
    }

    public static class OnOrBeforeDateTimeOffset
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("before", new DateTimeOffset(2024, 6, 15, 11, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(true)),
            new("after", new DateTimeOffset(2024, 6, 15, 13, 0, 0, TimeSpan.Zero), new DataAnnotationExpected(false, "Value must be on or before the specified date/time."))
        ];
    }

    public static class OnOrBeforeUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("type-mismatch", () => new OnOrBeforePropertyAttribute(nameof(TemporalCompareModel.DateTimeOther)).GetValidationResult(new TimeOnly(12, 0, 0), new ValidationContext(new TemporalCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'DateTimeOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class GreaterThan
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("greater", 15, new DataAnnotationExpected(true)),
            new("lesser", 5, new DataAnnotationExpected(false, "Value must be greater than the minimum.", Code: MustCodes.Number.Range.NotGreater)),
            new("null-value", null, new DataAnnotationExpected(true))
        ];
    }

    public static class GreaterThanUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("null-other", () => new GreaterThanPropertyAttribute(nameof(NumericCompareModel.NullIntOther)).GetValidationResult(15, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'NullIntOther' to resolve to the same type")),
            new ActionThrowsCase("type-mismatch", () => new GreaterThanPropertyAttribute(nameof(NumericCompareModel.LongOther)).GetValidationResult(15, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'LongOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class GreaterThanOrEqual
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("greater", 15, new DataAnnotationExpected(true)),
            new("lesser", 5, new DataAnnotationExpected(false, "Value must be greater than or equal to the minimum."))
        ];
    }

    public static class GreaterThanOrEqualUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("null-other", () => new GreaterThanOrEqualPropertyAttribute(nameof(NumericCompareModel.NullIntOther)).GetValidationResult(15, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'NullIntOther' to resolve to the same type")),
            new ActionThrowsCase("type-mismatch", () => new GreaterThanOrEqualPropertyAttribute(nameof(NumericCompareModel.LongOther)).GetValidationResult(15, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'LongOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class LessThan
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("lesser", 5, new DataAnnotationExpected(true)),
            new("greater", 15, new DataAnnotationExpected(false, "Value must be less than the maximum."))
        ];
    }

    public static class LessThanUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("null-other", () => new LessThanPropertyAttribute(nameof(NumericCompareModel.NullIntOther)).GetValidationResult(5, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'NullIntOther' to resolve to the same type")),
            new ActionThrowsCase("type-mismatch", () => new LessThanPropertyAttribute(nameof(NumericCompareModel.LongOther)).GetValidationResult(5, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'LongOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class LessThanOrEqual
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("lesser", 5, new DataAnnotationExpected(true)),
            new("greater", 15, new DataAnnotationExpected(false, "Value must be less than or equal to the maximum."))
        ];
    }

    public static class LessThanOrEqualUnsupportedComparison
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("null-other", () => new LessThanOrEqualPropertyAttribute(nameof(NumericCompareModel.NullIntOther)).GetValidationResult(5, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'NullIntOther' to resolve to the same type")),
            new ActionThrowsCase("type-mismatch", () => new LessThanOrEqualPropertyAttribute(nameof(NumericCompareModel.LongOther)).GetValidationResult(5, new ValidationContext(new NumericCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(InvalidOperationException), null, "requires 'LongOther' to resolve to the same type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class EqualTo
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("equal", "expected", new DataAnnotationExpected(true)),
            new("not-equal", "different", new DataAnnotationExpected(false, "Value must be equal to the expected value.", Code: MustCodes.Value.Equality.NotEqual))
        ];
    }

    public static class EqualToTypeMismatch
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("type-mismatch", () => new EqualToPropertyAttribute(nameof(EqualityCompareModel.IntOtherMismatch)).GetValidationResult("expected", new ValidationContext(new EqualityCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(ArgumentException), null, "cannot be converted to type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }

    public static class NotEqualTo
    {
        public static TheoryData<DataAnnotationCase> Cases =>
        [
            new("not-equal", "different", new DataAnnotationExpected(true)),
            new("equal", "expected", new DataAnnotationExpected(false, "Value must not be equal to the expected value."))
        ];
    }

    public static class NotEqualToTypeMismatch
    {
        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase("type-mismatch", () => new NotEqualToPropertyAttribute(nameof(EqualityCompareModel.IntOtherMismatch)).GetValidationResult("expected", new ValidationContext(new EqualityCompareModel()) { MemberName = "Value" }), new ExpectedException(typeof(ArgumentException), null, "cannot be converted to type"))
        ];

        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);
    }
}
