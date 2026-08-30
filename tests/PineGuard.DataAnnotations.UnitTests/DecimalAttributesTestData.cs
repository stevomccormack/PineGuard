using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.DecimalRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class DecimalAttributesTestData
{
    public static class ScaleAtMost
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasMaxScale.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.HasMaxScale.NullValue) => new DataAnnotationExpected(true),
            nameof(F.HasMaxScale.NegativeScale) or nameof(F.HasMaxScale.ScaleAboveMax) => new DataAnnotationExpected(false, "scale requires a value between 0 and 28.", Code: MustCodes.Number.Scale.Exceeded),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have no more than the allowed number of decimal places.", Code: MustCodes.Number.Scale.Exceeded)
        });
    }

    public static class PrecisionAtMost
    {
        public static TheoryData<DataAnnotationCase> Cases => F.HasMaxPrecision.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.HasMaxPrecision.NullValue) => new DataAnnotationExpected(true),
            nameof(F.HasMaxPrecision.PrecisionBelowMin) or nameof(F.HasMaxPrecision.PrecisionAboveMax) => new DataAnnotationExpected(false, "precision requires a value between 1 and 29.", Code: MustCodes.Number.Precision.Exceeded),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must have no more than the allowed number of digits.", Code: MustCodes.Number.Precision.Exceeded)
        });
    }

    public static class WithinPrecision
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsWithinPrecision.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
        {
            nameof(F.IsWithinPrecision.NullValue) => new DataAnnotationExpected(true),
            nameof(F.IsWithinPrecision.PrecisionBelowMin) or nameof(F.IsWithinPrecision.PrecisionAboveMax) => new DataAnnotationExpected(false, "precision requires a value between 1 and 29.", Code: MustCodes.Number.Precision.OutOfRange),
            nameof(F.IsWithinPrecision.NegativeScale) or nameof(F.IsWithinPrecision.ScaleAboveMax) => new DataAnnotationExpected(false, "scale requires a value between 0 and 28.", Code: MustCodes.Number.Precision.OutOfRange),
            nameof(F.IsWithinPrecision.ScaleAbovePrecision) => new DataAnnotationExpected(false, "scale requires a value no greater than the precision.", Code: MustCodes.Number.Precision.OutOfRange),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must fit within the allowed precision and scale.", Code: MustCodes.Number.Precision.OutOfRange)
        });
    }
}
