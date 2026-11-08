using System.Globalization;
using FluentValidation;
using FluentValidation.Results;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class TemporalOptionalConfigurationTests(ITestOutputHelper output)
    : BaseFluentUnitTest(output)
{
    private sealed record Model<T>(T Value);

    private static ValidationResult Validate<T>(Action<InlineValidator<Model<T>>> configure)
    {
        var validator = new InlineValidator<Model<T>>();
        configure(validator);
        return validator.Validate(new Model<T>(default!));
    }

    [Theory]
    [MemberData(nameof(TemporalOptionalConfigurationTestData.InclusionOnNull.Cases), MemberType = typeof(TemporalOptionalConfigurationTestData.InclusionOnNull))]
    public void InclusionOnNull_BehavesAsExpected(FluentCase<Inclusion> tc)
    {
        // Act
        ValidationResult[] results =
        [
            Validate<DateOnly?>(validator => validator.RuleFor(model => model.Value).InRange(default(DateOnly), default(DateOnly), tc.Value)),
            Validate<DateTimeOffset?>(validator => validator.RuleFor(model => model.Value).InRange(default(DateTimeOffset), default(DateTimeOffset), tc.Value)),
            Validate<TimeOnly?>(validator => validator.RuleFor(model => model.Value).InRange(default(TimeOnly), default(TimeOnly), tc.Value)),
            Validate<DateOnlyRange?>(validator => validator.RuleFor(model => model.Value).Chronological(tc.Value)),
            Validate<DateTimeRange?>(validator => validator.RuleFor(model => model.Value).Chronological(tc.Value)),
            Validate<DateTimeOffsetRange?>(validator => validator.RuleFor(model => model.Value).Chronological(tc.Value)),
            Validate<TimeOnlyRange?>(validator => validator.RuleFor(model => model.Value).Chronological(tc.Value)),
            Validate<string?>(validator => validator.RuleFor(model => model.Value).InRangeDateOnly(default, default, tc.Value)),
            Validate<string?>(validator => validator.RuleFor(model => model.Value).InRangeDateTimeOffset(default, default, tc.Value)),
            Validate<string?>(validator => validator.RuleFor(model => model.Value).InRangeTimeOnly(default, default, tc.Value)),
            Validate<string?>(validator => validator.RuleFor(model => model.Value).InRangeTimeSpan(default, default, tc.Value))
        ];

        // Assert
        Assert.All(results, result => AssertResult(tc, result));
    }

    [Theory]
    [MemberData(nameof(TemporalOptionalConfigurationTestData.StylesOnNull.Cases), MemberType = typeof(TemporalOptionalConfigurationTestData.StylesOnNull))]
    public void StylesOnNull_BehavesAsExpected(FluentCase<DateTimeStyles> tc)
    {
        // Act
        ValidationResult[] results =
        [
            Validate<string?>(validator => validator.RuleFor(model => model.Value).PastDateOnly(styles: tc.Value)),
            Validate<string?>(validator => validator.RuleFor(model => model.Value).PastDateTimeOffset(styles: tc.Value)),
            Validate<string?>(validator => validator.RuleFor(model => model.Value).BeforeTimeOnly(default, styles: tc.Value))
        ];

        // Assert
        Assert.All(results, result => AssertResult(tc, result));
    }

    [Theory]
    [MemberData(nameof(TemporalOptionalConfigurationTestData.PrecisionOnNull.Cases), MemberType = typeof(TemporalOptionalConfigurationTestData.PrecisionOnNull))]
    public void PrecisionOnNull_BehavesAsExpected(FluentCase<DateTimePrecision?> tc)
    {
        // Act
        Func<Model<DateTime?>, DateTime> selector = _ => throw new InvalidOperationException("An absent value must not invoke its selector.");
        ValidationResult[] results =
        [
            Validate<DateTime?>(validator => validator.RuleFor(model => model.Value).Before(selector, precision: tc.Value)),
            Validate<DateTime?>(validator => validator.RuleFor(model => model.Value).OnOrBefore(selector, precision: tc.Value)),
            Validate<DateTime?>(validator => validator.RuleFor(model => model.Value).After(selector, precision: tc.Value)),
            Validate<DateTime?>(validator => validator.RuleFor(model => model.Value).OnOrAfter(selector, precision: tc.Value))
        ];

        // Assert
        Assert.All(results, result => AssertResult(tc, result));
    }
    [Theory]
    [MemberData(nameof(TemporalOptionalConfigurationTestData.NullSelector.Cases), MemberType = typeof(TemporalOptionalConfigurationTestData.NullSelector))]
    public void NullSelector_BehavesAsExpected(TemporalOptionalConfigurationTestData.NullSelector.Case tc)
    {
        // Act
        var exception = Assert.Throws(tc.ExpectedException.Type, tc.Value);

        // Assert
        ThrowsCaseAssert.Expected(exception, tc);
    }
}
