using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DecimalRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustDecimalClausesTestData
{
    public static class ScaleAtMost
    {
        public static TheoryData<MustCase<(decimal value, int scale)>> ValidCases => F.HasMaxScale.AllValid.Project(v => (v.value!.Value, v.scale)).ToMustCases();
        public static TheoryData<MustCase<(decimal value, int scale)>> InvalidCases => F.HasMaxScale.AllInvalid.Except(nameof(F.HasMaxScale.NullValue)).Project(v => (v.value!.Value, v.scale)).ToMustCases(s => s.Name switch
        {
            nameof(F.HasMaxScale.NegativeScale) or nameof(F.HasMaxScale.ScaleAboveMax) => new MustExpected(false, "scale requires a value between 0 and 28.", "scale", Code: MustCodes.Number.Scale.Invalid),
            _ => new MustExpected(false, "value must have no more than the allowed number of decimal places.", Code: MustCodes.Number.Scale.Exceeded)
        });
    }

    public static class PrecisionAtMost
    {
        public static TheoryData<MustCase<(decimal value, int precision)>> ValidCases => F.HasMaxPrecision.AllValid.Project(v => (v.value!.Value, v.precision)).ToMustCases();
        public static TheoryData<MustCase<(decimal value, int precision)>> InvalidCases => F.HasMaxPrecision.AllInvalid.Except(nameof(F.HasMaxPrecision.NullValue)).Project(v => (v.value!.Value, v.precision)).ToMustCases(s => s.Name switch
        {
            nameof(F.HasMaxPrecision.PrecisionBelowMin) or nameof(F.HasMaxPrecision.PrecisionAboveMax) => new MustExpected(false, "precision requires a value between 1 and 29.", "precision", Code: MustCodes.Number.Precision.Invalid),
            _ => new MustExpected(false, "value must have no more than the allowed number of digits.", Code: MustCodes.Number.Precision.Exceeded)
        });
    }

    public static class WithinPrecision
    {
        public static TheoryData<MustCase<(decimal value, int precision, int scale)>> ValidCases => F.IsWithinPrecision.AllValid.Project(v => (v.value!.Value, v.precision, v.scale)).ToMustCases();
        public static TheoryData<MustCase<(decimal value, int precision, int scale)>> InvalidCases => F.IsWithinPrecision.AllInvalid.Except(nameof(F.IsWithinPrecision.NullValue)).Project(v => (v.value!.Value, v.precision, v.scale)).ToMustCases(s => s.Name switch
        {
            nameof(F.IsWithinPrecision.PrecisionBelowMin) or nameof(F.IsWithinPrecision.PrecisionAboveMax) => new MustExpected(false, "precision requires a value between 1 and 29.", "precision", Code: MustCodes.Number.Precision.Invalid),
            nameof(F.IsWithinPrecision.NegativeScale) or nameof(F.IsWithinPrecision.ScaleAboveMax) => new MustExpected(false, "scale requires a value between 0 and 28.", "scale", Code: MustCodes.Number.Scale.Invalid),
            nameof(F.IsWithinPrecision.ScaleAbovePrecision) => new MustExpected(false, "scale requires a value no greater than the precision.", "scale", Code: MustCodes.Number.Scale.Invalid),
            _ => new MustExpected(false, "value must fit within the allowed precision and scale.", Code: MustCodes.Number.Precision.OutOfRange)
        });
    }
}
