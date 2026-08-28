using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringNumbersExtensionsTestData
{
    public static class Positive
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsPositive.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsPositive.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be positive.", Code: MustCodes.Number.Sign.NotPositive)
        });
    }

    public static class Negative
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsNegative.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsNegative.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be negative.")
        });
    }

    public static class Zero
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsZero.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsZero.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be zero.")
        });
    }

    public static class NotZero
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsNotZero.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsNotZero.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must not be zero.")
        });
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsZeroOrPositive.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsZeroOrPositive.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be zero or positive.")
        });
    }

    public static class ZeroOrNegative
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsZeroOrNegative.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsZeroOrNegative.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be zero or negative.")
        });
    }

    public static class GreaterThan
    {
        public static TheoryData<FluentCase<(string? value, decimal min)>> Cases => F.NumbersIsGreaterThan.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsGreaterThan.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false)
        });
    }

    public static class GreaterThanOrEqual
    {
        public static TheoryData<FluentCase<(string? value, decimal min)>> Cases => F.NumbersIsGreaterThanOrEqual.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsGreaterThanOrEqual.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false)
        });
    }

    public static class LessThan
    {
        public static TheoryData<FluentCase<(string? value, decimal max)>> Cases => F.NumbersIsLessThan.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsLessThan.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false)
        });
    }

    public static class LessThanOrEqual
    {
        public static TheoryData<FluentCase<(string? value, decimal max)>> Cases => F.NumbersIsLessThanOrEqual.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsLessThanOrEqual.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false)
        });
    }

    public static class InRange
    {
        public static TheoryData<FluentCase<(string? value, decimal min, decimal max, PineGuard.Common.Inclusion inclusion)>> Cases => F.NumbersIsInRange.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsInRange.NullValue) => new FluentExpected(true),
            nameof(F.NumbersIsInRange.InvalidRange) => new FluentExpected(false, "min requires a valid range."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be within the expected range.")
        });
    }

    public static class OutOfRange
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("out", "5", new FluentExpected(true)),
            new("in-fails", "15", new FluentExpected(false)),
            new("letters-fails", "abc", new FluentExpected(false)),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Approximately
    {
        public static TheoryData<FluentCase<(string? value, decimal target, decimal? tolerance)>> Cases => F.NumbersIsApproximately.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsApproximately.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false)
        });
    }

    public static class NotApproximately
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("not-approx", "10.5", new FluentExpected(true)),
            new("approx-fails", "10.0", new FluentExpected(false)),
            new("letters-fails", "abc", new FluentExpected(false)),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class MultipleOf
    {
        public static TheoryData<FluentCase<(string? value, decimal factor)>> Cases => F.NumbersIsMultipleOf.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsMultipleOf.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false)
        });
    }

    public static class NotMultipleOf
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("not-multiple", "5", new FluentExpected(true)),
            new("multiple-fails", "4", new FluentExpected(false)),
            new("letters-fails", "abc", new FluentExpected(false)),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class Even
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsEven.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsEven.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be even.")
        });
    }

    public static class Odd
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsOdd.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsOdd.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be odd.")
        });
    }

    public static class Finite
    {
        public static TheoryData<FluentCase<string?>> Cases => F.NumbersIsFinite.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.NumbersIsFinite.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be finite.")
        });
    }

    public static class NotFinite
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("not-finite", "Infinity", new FluentExpected(true)),
            new("finite-fails", "1.23", new FluentExpected(false, "Value must not be finite.")),
            new("letters-fails", "abc", new FluentExpected(false, "Value must not be finite.")),
            new("null", null, new FluentExpected(true))
        ];
    }

    public static class NotNaN
    {
        public static TheoryData<FluentCase<string?>> Cases =>
        [
            new("number", "1.23", new FluentExpected(true)),
            new("nan-fails", "NaN", new FluentExpected(false, "Value must not be NaN.")),
            new("letters-fails", "abc", new FluentExpected(false, "Value must not be NaN.")),
            new("null", null, new FluentExpected(true))
        ];
    }
}
