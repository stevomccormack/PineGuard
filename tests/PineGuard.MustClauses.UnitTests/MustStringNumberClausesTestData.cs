using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringNumberClausesTestData
{
    public static class Positive
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsPositive.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsPositive.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsPositive.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Sign.NotPositive),
            _ => new MustExpected(false, "value must be positive.", Code: MustCodes.Number.Sign.NotPositive)
        });
    }

    public static class Negative
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsNegative.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsNegative.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsNegative.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Sign.NotNegative),
            _ => new MustExpected(false, "value must be negative.", Code: MustCodes.Number.Sign.NotNegative)
        });
    }

    public static class Zero
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsZero.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsZero.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsZero.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Sign.NotZero),
            _ => new MustExpected(false, "value must be zero.", Code: MustCodes.Number.Sign.NotZero)
        });
    }

    public static class NotZero
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsNotZero.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsNotZero.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsNotZero.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Sign.Zero),
            _ => new MustExpected(false, "value must not be zero.", Code: MustCodes.Number.Sign.Zero)
        });
    }

    public static class ZeroOrPositive
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsZeroOrPositive.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsZeroOrPositive.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsZeroOrPositive.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Sign.Negative),
            _ => new MustExpected(false, "value must be zero or positive.", Code: MustCodes.Number.Sign.Negative)
        });
    }

    public static class ZeroOrNegative
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsZeroOrNegative.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsZeroOrNegative.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsZeroOrNegative.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Sign.Positive),
            _ => new MustExpected(false, "value must be zero or negative.", Code: MustCodes.Number.Sign.Positive)
        });
    }

    public static class GreaterThan
    {
        public static TheoryData<MustCase<(string? text, decimal min)>> ValidCases => F.NumbersIsGreaterThan.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal min)>> InvalidCases => F.NumbersIsGreaterThan.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsGreaterThan.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Range.NotGreater),
            _ => new MustExpected(false, $"value must be greater than '{s.Inputs.min}'.", Code: MustCodes.Number.Range.NotGreater)
        });
    }

    public static class GreaterThanOrEqual
    {
        public static TheoryData<MustCase<(string? text, decimal min)>> ValidCases => F.NumbersIsGreaterThanOrEqual.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal min)>> InvalidCases => F.NumbersIsGreaterThanOrEqual.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsGreaterThanOrEqual.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Range.BelowMinimum),
            _ => new MustExpected(false, $"value must be greater than or equal to '{s.Inputs.min}'.", Code: MustCodes.Number.Range.BelowMinimum)
        });
    }

    public static class LessThan
    {
        public static TheoryData<MustCase<(string? text, decimal max)>> ValidCases => F.NumbersIsLessThan.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal max)>> InvalidCases => F.NumbersIsLessThan.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsLessThan.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Range.NotLess),
            _ => new MustExpected(false, $"value must be less than '{s.Inputs.max}'.", Code: MustCodes.Number.Range.NotLess)
        });
    }

    public static class LessThanOrEqual
    {
        public static TheoryData<MustCase<(string? text, decimal max)>> ValidCases => F.NumbersIsLessThanOrEqual.AllValid.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal max)>> InvalidCases => F.NumbersIsLessThanOrEqual.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsLessThanOrEqual.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Range.Exceeded),
            _ => new MustExpected(false, $"value must be less than or equal to '{s.Inputs.max}'.", Code: MustCodes.Number.Range.Exceeded)
        });
    }

    public static class InRange
    {
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> ValidCases => F.NumbersIsInRange.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> InvalidCases => F.NumbersIsInRange.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsInRange.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Range.OutOfRange),
            nameof(F.NumbersIsInRange.InvalidRange) => new MustExpected(false, "min requires a valid range.", "min", Code: MustCodes.Number.Range.Invalid),
            _ => new MustExpected(false, "value must be within the expected range.", Code: MustCodes.Number.Range.OutOfRange)
        });
    }

    public static class Approximately
    {
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> ValidCases => F.NumbersIsApproximately.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> InvalidCases => F.NumbersIsApproximately.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsApproximately.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Proximity.NotApproximate),
            nameof(F.NumbersIsApproximately.NullTolerance) => new MustExpected(false, "tolerance requires a non-null tolerance.", "tolerance", Code: MustCodes.Number.Tolerance.Null),
            _ => new MustExpected(false, $"value must be approximately '{s.Inputs.target}'.", Code: MustCodes.Number.Proximity.NotApproximate)
        });
    }

    public static class MultipleOf
    {
        public static TheoryData<MustCase<(string? text, decimal factor)>> ValidCases => F.NumbersIsMultipleOf.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<(string? text, decimal factor)>> InvalidCases => F.NumbersIsMultipleOf.AllInvalid.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsMultipleOf.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Divisibility.NotMultiple),
            nameof(F.NumbersIsMultipleOf.ZeroFactor) => new MustExpected(false, "factor requires a non-zero factor.", "factor", Code: MustCodes.Number.Factor.Zero),
            _ => new MustExpected(false, $"value must be a multiple of '{s.Inputs.factor}'.", Code: MustCodes.Number.Divisibility.NotMultiple)
        });
    }

    public static class Even
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsEven.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsEven.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsEven.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Parity.Odd),
            _ => new MustExpected(false, "value must be even.", Code: MustCodes.Number.Parity.Odd)
        });
    }

    public static class Odd
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsOdd.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsOdd.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsOdd.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Parity.Even),
            _ => new MustExpected(false, "value must be odd.", Code: MustCodes.Number.Parity.Even)
        });
    }

    public static class Finite
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsFinite.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsFinite.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.NumbersIsFinite.NullValue) => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Form.NotFinite),
            _ => new MustExpected(false, "value must be finite.", Code: MustCodes.Number.Form.NotFinite)
        });
    }

    public static class IsNaN
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.NumbersIsNaN.InvalidScenarios.Except(nameof(F.NumbersIsNaN.Letters), nameof(F.NumbersIsNaN.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsNaN.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be NaN.", Code: MustCodes.Number.Form.Nan));
        public static TheoryData<MustCase<string?>> NullCases => F.NumbersIsNaN.InvalidScenarios.Only(nameof(F.NumbersIsNaN.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Form.Nan));
        public static TheoryData<MustCase<string?>> LettersCases => F.NumbersIsNaN.InvalidScenarios.Only(nameof(F.NumbersIsNaN.Letters)).ToMustCases(_ => new MustExpected(false, "value must not be NaN.", Code: MustCodes.Number.Form.Nan));
    }

    public static class OutOfRange
    {
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> ValidCases => F.NumbersIsInRange.InvalidEdgeScenarios.ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> InvalidCases => F.NumbersIsInRange.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be between '1' and '10'.", Code: MustCodes.Number.Range.InRange));
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> NullCases => F.NumbersIsInRange.InvalidScenarios.Only(nameof(F.NumbersIsInRange.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Range.InRange));
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> InvalidRangeCases => F.NumbersIsInRange.InvalidScenarios.Only(nameof(F.NumbersIsInRange.InvalidRange)).ToMustCases(_ => new MustExpected(false, "min requires a valid range.", "min", Code: MustCodes.Number.Range.Invalid));
        public static TheoryData<MustCase<(string? text, decimal min, decimal max, Inclusion inclusion)>> LettersCases => F.NumbersIsInRange.InvalidScenarios.Only(nameof(F.NumbersIsInRange.Letters)).ToMustCases(_ => new MustExpected(false, "value must not be between '1' and '10'.", Code: MustCodes.Number.Range.InRange));
    }

    public static class NotApproximately
    {
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> ValidCases => F.NumbersIsApproximately.InvalidScenarios.Except(nameof(F.NumbersIsApproximately.Letters), nameof(F.NumbersIsApproximately.NullValue)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> InvalidCases => F.NumbersIsApproximately.ValidScenarios.ToMustCases(s => new MustExpected(false, $"value must not be approximately '{s.Inputs.target}'.", Code: MustCodes.Number.Proximity.Approximate));
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> NullCases => F.NumbersIsApproximately.InvalidScenarios.Only(nameof(F.NumbersIsApproximately.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Proximity.Approximate));
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> NullToleranceCases => F.NumbersIsApproximately.InvalidEdgeScenarios.Only(nameof(F.NumbersIsApproximately.NullTolerance)).ToMustCases(_ => new MustExpected(false, "tolerance requires a non-null tolerance.", "tolerance", Code: MustCodes.Number.Tolerance.Null));
        public static TheoryData<MustCase<(string? text, decimal target, decimal? tolerance)>> LettersCases => F.NumbersIsApproximately.InvalidScenarios.Only(nameof(F.NumbersIsApproximately.Letters)).ToMustCases(s => new MustExpected(false, $"value must not be approximately '{s.Inputs.target}'.", Code: MustCodes.Number.Proximity.Approximate));
    }

    public static class NotMultipleOf
    {
        public static TheoryData<MustCase<(string? text, decimal factor)>> ValidCases => F.NumbersIsMultipleOf.InvalidScenarios.Except(nameof(F.NumbersIsMultipleOf.Letters), nameof(F.NumbersIsMultipleOf.NullValue), nameof(F.NumbersIsMultipleOf.ZeroFactor)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<(string? text, decimal factor)>> InvalidCases => F.NumbersIsMultipleOf.ValidScenarios.ToMustCases(s => new MustExpected(false, $"value must not be a multiple of '{s.Inputs.factor}'.", Code: MustCodes.Number.Divisibility.Multiple));
        public static TheoryData<MustCase<(string? text, decimal factor)>> NullCases => F.NumbersIsMultipleOf.InvalidScenarios.Only(nameof(F.NumbersIsMultipleOf.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Divisibility.Multiple));
        public static TheoryData<MustCase<(string? text, decimal factor)>> ZeroFactorCases => F.NumbersIsMultipleOf.InvalidScenarios.Only(nameof(F.NumbersIsMultipleOf.ZeroFactor)).ToMustCases(_ => new MustExpected(false, "factor requires a non-zero factor.", "factor", Code: MustCodes.Number.Factor.Zero));
        public static TheoryData<MustCase<(string? text, decimal factor)>> LettersCases => F.NumbersIsMultipleOf.InvalidScenarios.Only(nameof(F.NumbersIsMultipleOf.Letters)).ToMustCases(s => new MustExpected(false, $"value must not be a multiple of '{s.Inputs.factor}'.", Code: MustCodes.Number.Divisibility.Multiple));
    }

    public static class NotFinite
    {
        public static TheoryData<MustCase<string?>> ValidCases =>
        [
            new(nameof(F.NumbersIsNaN.NaN), F.NumbersIsNaN.NaN, new MustExpected(true))
        ];
        public static TheoryData<MustCase<string?>> InvalidCases => F.NumbersIsFinite.ValidScenarios.ToMustCases(_ => new MustExpected(false, "value must not be finite.", Code: MustCodes.Number.Form.Finite));
        public static TheoryData<MustCase<string?>> NullCases => F.NumbersIsFinite.InvalidScenarios.Only(nameof(F.NumbersIsFinite.NullValue)).ToMustCases(_ => new MustExpected(false, "value must not be null.", "value", Code: MustCodes.Number.Form.Finite));
        public static TheoryData<MustCase<string?>> LettersCases => F.NumbersIsFinite.InvalidScenarios.Only(nameof(F.NumbersIsFinite.Letters)).ToMustCases(_ => new MustExpected(false, "value must not be finite.", Code: MustCodes.Number.Form.Finite));
    }
}
