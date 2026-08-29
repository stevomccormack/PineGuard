using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class DecimalRulesFixtures
{
    public static class HasMaxScale
    {
        public static readonly (decimal? value, int scale) FewerPlaces = (1.5m, 2);
        public static readonly (decimal? value, int scale) ExactPlaces = (1.25m, 2);
        public static readonly (decimal? value, int scale) TrailingZerosIgnored = (1.500m, 1);
        public static readonly (decimal? value, int scale) IntegerValue = (100m, 0);
        public static readonly (decimal? value, int scale) AtMaxScale = (1e-28m, DecimalRules.MaxScale);
        public static readonly (decimal? value, int scale) MorePlaces = (1.234m, 2);
        public static readonly (decimal? value, int scale) NullValue = (null, 2);
        public static readonly (decimal? value, int scale) NegativeScale = (1.5m, -1);
        public static readonly (decimal? value, int scale) ScaleAboveMax = (1.5m, DecimalRules.MaxScale + 1);

        public static RuleScenario<(decimal? value, int scale)>[] ValidScenarios => [new(nameof(FewerPlaces), FewerPlaces, true), new(nameof(TrailingZerosIgnored), TrailingZerosIgnored, true)];
        public static RuleScenario<(decimal? value, int scale)>[] ValidEdgeScenarios => [new(nameof(ExactPlaces), ExactPlaces, true), new(nameof(IntegerValue), IntegerValue, true), new(nameof(AtMaxScale), AtMaxScale, true)];
        public static RuleScenario<(decimal? value, int scale)>[] InvalidScenarios => [new(nameof(MorePlaces), MorePlaces, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(decimal? value, int scale)>[] InvalidEdgeScenarios => [new(nameof(NegativeScale), NegativeScale, false), new(nameof(ScaleAboveMax), ScaleAboveMax, false)];
        public static RuleScenario<(decimal? value, int scale)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(decimal? value, int scale)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(decimal? value, int scale)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class HasMaxPrecision
    {
        public static readonly (decimal? value, int precision) FewerDigits = (12.3m, 5);
        public static readonly (decimal? value, int precision) ExactDigits = (123.45m, 5);
        public static readonly (decimal? value, int precision) TrailingZerosIgnored = (1.500m, 2);
        public static readonly (decimal? value, int precision) AtMinPrecision = (9m, 1);
        public static readonly (decimal? value, int precision) AtMaxPrecision = (decimal.MaxValue, DecimalRules.MaxPrecision);
        public static readonly (decimal? value, int precision) MoreDigits = (123.45m, 4);
        public static readonly (decimal? value, int precision) NullValue = (null, 5);
        public static readonly (decimal? value, int precision) PrecisionBelowMin = (1m, 0);
        public static readonly (decimal? value, int precision) PrecisionAboveMax = (1m, DecimalRules.MaxPrecision + 1);

        public static RuleScenario<(decimal? value, int precision)>[] ValidScenarios => [new(nameof(FewerDigits), FewerDigits, true), new(nameof(TrailingZerosIgnored), TrailingZerosIgnored, true)];
        public static RuleScenario<(decimal? value, int precision)>[] ValidEdgeScenarios => [new(nameof(ExactDigits), ExactDigits, true), new(nameof(AtMinPrecision), AtMinPrecision, true), new(nameof(AtMaxPrecision), AtMaxPrecision, true)];
        public static RuleScenario<(decimal? value, int precision)>[] InvalidScenarios => [new(nameof(MoreDigits), MoreDigits, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(decimal? value, int precision)>[] InvalidEdgeScenarios => [new(nameof(PrecisionBelowMin), PrecisionBelowMin, false), new(nameof(PrecisionAboveMax), PrecisionAboveMax, false)];
        public static RuleScenario<(decimal? value, int precision)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(decimal? value, int precision)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(decimal? value, int precision)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsWithinPrecision
    {
        public static readonly (decimal? value, int precision, int scale) WithinBudget = (123.45m, 18, 2);
        public static readonly (decimal? value, int precision, int scale) TrailingZerosIgnored = (1.500m, 5, 2);
        public static readonly (decimal? value, int precision, int scale) ExactBudget = (123.45m, 5, 2);
        public static readonly (decimal? value, int precision, int scale) LeadingZeroFraction = (0.05m, 2, 2);
        public static readonly (decimal? value, int precision, int scale) ZeroScaleBudget = (123m, 3, 0);
        public static readonly (decimal? value, int precision, int scale) AtMaxBudget = (1e-28m, DecimalRules.MaxPrecision, DecimalRules.MaxScale);
        public static readonly (decimal? value, int precision, int scale) ScaleExceeded = (1.234m, 18, 2);
        public static readonly (decimal? value, int precision, int scale) IntegralExceeded = (123.4m, 5, 3);
        public static readonly (decimal? value, int precision, int scale) NullValue = (null, 18, 2);
        public static readonly (decimal? value, int precision, int scale) PrecisionBelowMin = (1m, 0, 0);
        public static readonly (decimal? value, int precision, int scale) PrecisionAboveMax = (1m, DecimalRules.MaxPrecision + 1, 2);
        public static readonly (decimal? value, int precision, int scale) NegativeScale = (1m, 18, -1);
        public static readonly (decimal? value, int precision, int scale) ScaleAboveMax = (1m, 18, DecimalRules.MaxScale + 1);
        public static readonly (decimal? value, int precision, int scale) ScaleAbovePrecision = (1m, 2, 3);

        public static RuleScenario<(decimal? value, int precision, int scale)>[] ValidScenarios => [new(nameof(WithinBudget), WithinBudget, true), new(nameof(TrailingZerosIgnored), TrailingZerosIgnored, true)];
        public static RuleScenario<(decimal? value, int precision, int scale)>[] ValidEdgeScenarios => [new(nameof(ExactBudget), ExactBudget, true), new(nameof(LeadingZeroFraction), LeadingZeroFraction, true), new(nameof(ZeroScaleBudget), ZeroScaleBudget, true), new(nameof(AtMaxBudget), AtMaxBudget, true)];
        public static RuleScenario<(decimal? value, int precision, int scale)>[] InvalidScenarios => [new(nameof(ScaleExceeded), ScaleExceeded, false), new(nameof(IntegralExceeded), IntegralExceeded, false), new(nameof(NullValue), NullValue, false)];
        public static RuleScenario<(decimal? value, int precision, int scale)>[] InvalidEdgeScenarios => [new(nameof(PrecisionBelowMin), PrecisionBelowMin, false), new(nameof(PrecisionAboveMax), PrecisionAboveMax, false), new(nameof(NegativeScale), NegativeScale, false), new(nameof(ScaleAboveMax), ScaleAboveMax, false), new(nameof(ScaleAbovePrecision), ScaleAbovePrecision, false)];
        public static RuleScenario<(decimal? value, int precision, int scale)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(decimal? value, int precision, int scale)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(decimal? value, int precision, int scale)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
