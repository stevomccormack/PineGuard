using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class NumberRulesConcreteTestData
{
    public static TheoryData<Inclusion> InvalidInclusions => [(Inclusion)42, (Inclusion)(-1)];

    public static TheoryData<decimal?> Values => [null, decimal.MinValue, -1m, 0m, 1m, 100m, decimal.MaxValue];

    public static TheoryData<decimal?, decimal, decimal?> Approximation => new()
    {
        { null, 0m, 1m },
        { 0m, 0m, null },
        { 0m, 0m, -1m },
        { 2m, 1m, 1m },
        { 0m, 1m, 1m },
        { 3m, 1m, 1m },
        { decimal.MaxValue, decimal.MinValue, decimal.MaxValue },
        { decimal.MinValue, decimal.MaxValue, decimal.MaxValue }
    };

    public static TheoryData<decimal?, decimal> Multiples => new()
    {
        { null, 2m }, { 1m, 0m }, { 4m, 2m }, { 3m, 2m },
        { decimal.MinValue, -1m }, { decimal.MaxValue, 0.1m }
    };

    public static TheoryData<decimal?, decimal, decimal, Inclusion> Ranges => new()
    {
        { null, 0m, 1m, Inclusion.Inclusive },
        { 0m, 0m, 1m, Inclusion.Inclusive },
        { 0m, 0m, 1m, Inclusion.Exclusive },
        { 1m, 0m, 1m, Inclusion.Inclusive },
        { 1m, 0m, 1m, Inclusion.Exclusive },
        { 0.5m, 0m, 1m, Inclusion.Exclusive },
        { -1m, 0m, 1m, Inclusion.Inclusive },
        { 2m, 0m, 1m, Inclusion.Inclusive },
        { 0m, 1m, 0m, Inclusion.Inclusive }
    };
}
