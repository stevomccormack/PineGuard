using System.Globalization;
using PineGuard.Common;

namespace PineGuard.DataAnnotations.UnitTests;

public static class NumericAttributeSyntaxTestData
{
    public static TheoryData<NumberStyles> InvalidStyles => [(NumberStyles)4096, (NumberStyles)(-1)];
    public static TheoryData<NumberStyles> ValidStyles => [NumberStyles.Float, NumberStyles.Any];
    public static TheoryData<double> NullToleranceInputs => [10d, 100d];

    public static TheoryData<string, bool> DecimalValues => new()
    {
        { "10.000000000000000000000000001", true },
        { "10.000000000000000000000000002", false }
    };
    public static TheoryData<string> MalformedDecimals => ["bad", "1,2", "NaN", "1e100"];
    public static TheoryData<Inclusion> InvalidInclusions => [(Inclusion)42, (Inclusion)(-1)];
}
