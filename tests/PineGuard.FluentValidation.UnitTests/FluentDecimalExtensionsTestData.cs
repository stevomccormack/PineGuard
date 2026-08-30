using PineGuard.Codes;
using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.DecimalRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentDecimalExtensionsTestData
{
    public static class ScaleAtMost
    {
        public static TheoryData<FluentCase<(decimal? value, int scale)>> Cases => F.HasMaxScale.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasMaxScale.NullValue) => new FluentExpected(true),
            nameof(F.HasMaxScale.NegativeScale) or nameof(F.HasMaxScale.ScaleAboveMax) => new FluentExpected(false, "scale requires a value between 0 and 28.", Code: MustCodes.Number.Scale.Exceeded),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have no more than the allowed number of decimal places.", Code: MustCodes.Number.Scale.Exceeded)
        });
    }

    public static class PrecisionAtMost
    {
        public static TheoryData<FluentCase<(decimal? value, int precision)>> Cases => F.HasMaxPrecision.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.HasMaxPrecision.NullValue) => new FluentExpected(true),
            nameof(F.HasMaxPrecision.PrecisionBelowMin) or nameof(F.HasMaxPrecision.PrecisionAboveMax) => new FluentExpected(false, "precision requires a value between 1 and 29.", Code: MustCodes.Number.Precision.Exceeded),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must have no more than the allowed number of digits.", Code: MustCodes.Number.Precision.Exceeded)
        });
    }

    public static class WithinPrecision
    {
        public static TheoryData<FluentCase<(decimal? value, int precision, int scale)>> Cases => F.IsWithinPrecision.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsWithinPrecision.NullValue) => new FluentExpected(true),
            nameof(F.IsWithinPrecision.PrecisionBelowMin) or nameof(F.IsWithinPrecision.PrecisionAboveMax) => new FluentExpected(false, "precision requires a value between 1 and 29.", Code: MustCodes.Number.Precision.OutOfRange),
            nameof(F.IsWithinPrecision.NegativeScale) or nameof(F.IsWithinPrecision.ScaleAboveMax) => new FluentExpected(false, "scale requires a value between 0 and 28.", Code: MustCodes.Number.Precision.OutOfRange),
            nameof(F.IsWithinPrecision.ScaleAbovePrecision) => new FluentExpected(false, "scale requires a value no greater than the precision.", Code: MustCodes.Number.Precision.OutOfRange),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must fit within the allowed precision and scale.", Code: MustCodes.Number.Precision.OutOfRange)
        });
    }
}
