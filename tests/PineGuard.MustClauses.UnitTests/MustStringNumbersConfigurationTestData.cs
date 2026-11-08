using System.Globalization;
using PineGuard.Common;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringNumbersConfigurationTestData
{
    public static TheoryData<NumberStyles> InvalidStyles => [(NumberStyles)4096, (NumberStyles)(-1)];
    public static TheoryData<Inclusion> InvalidInclusions => [(Inclusion)42, (Inclusion)(-1)];
    public static TheoryData<decimal> NegativeTolerances => [-1m, -0.1m];
    public static TheoryData<string, string, int, int> HexadecimalPairs => new()
    {
        { "FE", "FF", 254, 255 },
        { "A", "B", 10, 11 }
    };
}
