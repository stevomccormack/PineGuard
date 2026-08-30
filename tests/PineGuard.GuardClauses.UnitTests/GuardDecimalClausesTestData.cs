using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DecimalRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardDecimalClausesTestData
{
    // Guard.Against.ScaleAbove — throws when value HAS too many decimal places (delegates to Must.Be.ScaleAtMost)
    public static class ScaleAbove
    {
        public static TheoryData<GuardCase<(decimal value, int scale)>> ValidCases => F.HasMaxScale.AllValid.Project(v => (v.value!.Value, v.scale)).ToGuardCases();
        public static TheoryData<GuardCase<(decimal value, int scale)>> InvalidCases => F.HasMaxScale.AllInvalid.Except(nameof(F.HasMaxScale.NullValue)).Project(v => (v.value!.Value, v.scale)).ToGuardCases(s => s.Name switch
        {
            nameof(F.HasMaxScale.NegativeScale) or nameof(F.HasMaxScale.ScaleAboveMax) => new GuardExpected(false, typeof(ArgumentException), "scale", Code: MustCodes.Number.Scale.Invalid),
            _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Number.Scale.Exceeded)
        });
    }

    // Guard.Against.PrecisionAbove — throws when value HAS too many digits (delegates to Must.Be.PrecisionAtMost)
    public static class PrecisionAbove
    {
        public static TheoryData<GuardCase<(decimal value, int precision)>> ValidCases => F.HasMaxPrecision.AllValid.Project(v => (v.value!.Value, v.precision)).ToGuardCases();
        public static TheoryData<GuardCase<(decimal value, int precision)>> InvalidCases => F.HasMaxPrecision.AllInvalid.Except(nameof(F.HasMaxPrecision.NullValue)).Project(v => (v.value!.Value, v.precision)).ToGuardCases(s => s.Name switch
        {
            nameof(F.HasMaxPrecision.PrecisionBelowMin) or nameof(F.HasMaxPrecision.PrecisionAboveMax) => new GuardExpected(false, typeof(ArgumentException), "precision", Code: MustCodes.Number.Precision.Invalid),
            _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Number.Precision.Exceeded)
        });
    }

    // Guard.Against.NotWithinPrecision — throws when value does NOT fit the budget (delegates to Must.Be.WithinPrecision)
    public static class NotWithinPrecision
    {
        public static TheoryData<GuardCase<(decimal value, int precision, int scale)>> ValidCases => F.IsWithinPrecision.AllValid.Project(v => (v.value!.Value, v.precision, v.scale)).ToGuardCases();
        public static TheoryData<GuardCase<(decimal value, int precision, int scale)>> InvalidCases => F.IsWithinPrecision.AllInvalid.Except(nameof(F.IsWithinPrecision.NullValue)).Project(v => (v.value!.Value, v.precision, v.scale)).ToGuardCases(s => s.Name switch
        {
            nameof(F.IsWithinPrecision.PrecisionBelowMin) or nameof(F.IsWithinPrecision.PrecisionAboveMax) => new GuardExpected(false, typeof(ArgumentException), "precision", Code: MustCodes.Number.Precision.Invalid),
            nameof(F.IsWithinPrecision.NegativeScale) or nameof(F.IsWithinPrecision.ScaleAboveMax) or nameof(F.IsWithinPrecision.ScaleAbovePrecision) => new GuardExpected(false, typeof(ArgumentException), "scale", Code: MustCodes.Number.Scale.Invalid),
            _ => new GuardExpected(false, typeof(ArgumentException), "value", Code: MustCodes.Number.Precision.OutOfRange)
        });
    }
}
