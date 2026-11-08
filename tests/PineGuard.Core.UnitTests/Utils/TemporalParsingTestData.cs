using System.Globalization;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Core.UnitTests.Utils;

public static class TemporalParsingTestData
{
    public static class Styles
    {
        public static TheoryData<RuleCase<DateTimeStyles>> DateAndTimeOnlyCases => Build(false);
        public static TheoryData<RuleCase<DateTimeStyles>> OffsetCases => Build(true);

        private static TheoryData<RuleCase<DateTimeStyles>> Build(bool offset)
        {
            var cases = new TheoryData<RuleCase<DateTimeStyles>>();
            for (var bits = -1; bits <= 512; bits++)
            {
                var expected = offset
                    ? bits >= 0 && bits < 256 && (bits & 8) == 0 && (bits & 96) != 96
                    : bits >= 0 && bits <= 7;
                cases.Add(new($"styles-{bits}", (DateTimeStyles)bits, new(expected)));
            }
            return cases;
        }
    }
}
