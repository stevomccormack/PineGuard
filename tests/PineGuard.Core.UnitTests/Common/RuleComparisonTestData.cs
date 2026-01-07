using PineGuard.Common;
using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Common;

public static class RuleComparisonTestData
{
    public static class Equality
    {
        private static ValidCase V(string name, int value, int other, bool expected)
            => new(Name: name, Value: value, Other: other, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("0==0", 0, 0, true) },
            { V("-1==-1", -1, -1, true) },
            { V("max==max", int.MaxValue, int.MaxValue, true) },
            { V("5!=6", 5, 6, false) },
            { V("-5!=-6", -5, -6, false) },
            { V("0!=1", 0, 1, false) },
        };

        #region Cases

        public record Case(string Name, int Value, int Other);

        public sealed record ValidCase(string Name, int Value, int Other, bool Expected) 
            : Case(Name, Value, Other);

        #endregion
    }

    public static class IsBetween
    {
        private static ValidCase V(string name, int value, int min, int max, Inclusion inclusion, bool expected)
            => new(Name: name, Value: value, Min: min, Max: max, Inclusion: inclusion, Expected: expected);

        private static InvalidCase I(string name, int value, int min, int max, Inclusion inclusion, ExpectedException expectedException)
            => new(Name: name, Value: value, Min: min, Max: max, Inclusion: inclusion, ExpectedException: expectedException);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("within", 5, 1, 10, Inclusion.Inclusive, true) },
            { V("equal inclusive", 1, 1, 1, Inclusion.Inclusive, true) },
            { V("at max inclusive", 5, 1, 5, Inclusion.Inclusive, true) },
            { V("at min inclusive", 5, 5, 10, Inclusion.Inclusive, true) },
            { V("min..max inclusive", int.MinValue, int.MinValue, int.MaxValue, Inclusion.Inclusive, true) },
            { V("min..max inclusive (max)", int.MaxValue, int.MinValue, int.MaxValue, Inclusion.Inclusive, true) },
            { V("0..0 inclusive", 0, 0, 0, Inclusion.Inclusive, true) },
            { V("-1..-1 inclusive", -1, -1, -1, Inclusion.Inclusive, true) },
            { V("10..10 inclusive", 10, 10, 10, Inclusion.Inclusive, true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("equal exclusive", 1, 1, 1, Inclusion.Exclusive, false) },
            { V("at max exclusive", 5, 1, 5, Inclusion.Exclusive, false) },
            { V("at min exclusive", 5, 5, 10, Inclusion.Exclusive, false) },
            { V("min..max exclusive (min)", int.MinValue, int.MinValue, int.MaxValue, Inclusion.Exclusive, false) },
            { V("min..max exclusive (max)", int.MaxValue, int.MinValue, int.MaxValue, Inclusion.Exclusive, false) },
            { V("0..0 exclusive", 0, 0, 0, Inclusion.Exclusive, false) },
            { V("min>max", 5, 10, 1, Inclusion.Inclusive, false) },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("invalid inclusion 123", 5, 1, 10, (Inclusion)123, new ExpectedException(typeof(ArgumentOutOfRangeException))) },
            { I("invalid inclusion -1", 0, 0, 0, (Inclusion)(-1), new ExpectedException(typeof(ArgumentOutOfRangeException))) },
        };

        #region Cases
        
        public record Case(string Name, int Value, int Min, int Max, Inclusion Inclusion);

        public sealed record ValidCase(string Name, int Value, int Min, int Max, Inclusion Inclusion, bool Expected) 
            : Case(Name, Value, Min, Max, Inclusion);

        public record InvalidCase(string Name, int Value, int Min, int Max, Inclusion Inclusion, ExpectedException ExpectedException) 
            : Case(Name, Value, Min, Max, Inclusion);

        #endregion
    }

    public static class Boundaries
    {
        private static ValidCase V(string name, int value, int boundary, Inclusion inclusion, bool expectedGreaterThan, bool expectedLessThan)
            => new(Name: name, Value: value, Boundary: boundary, Inclusion: inclusion, ExpectedGreaterThan: expectedGreaterThan, ExpectedLessThan: expectedLessThan);

        private static InvalidCase I(string name, int value, int boundary, Inclusion inclusion, ExpectedException expectedException)
            => new(Name: name, Value: value, Boundary: boundary, Inclusion: inclusion, ExpectedException: expectedException);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("equal inclusive", 5, 5, Inclusion.Inclusive, true, true) },
            { V("equal exclusive", 5, 5, Inclusion.Exclusive, false, false) },
            { V("equal negative exclusive", -5, -5, Inclusion.Exclusive, false, false) },
            { V("greater", 6, 5, Inclusion.Inclusive, true, false) },
            { V("less", 4, 5, Inclusion.Inclusive, false, true) },
            { V("max equal inclusive", int.MaxValue, int.MaxValue, Inclusion.Inclusive, true, true) },
            { V("min equal exclusive", int.MinValue, int.MinValue, Inclusion.Exclusive, false, false) },
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            { I("boundary invalid inclusion 123", 5, 5, (Inclusion)123, new ExpectedException(typeof(ArgumentOutOfRangeException))) },
            { I("boundary invalid inclusion -1", 0, 1, (Inclusion)(-1), new ExpectedException(typeof(ArgumentOutOfRangeException))) },
        };

        #region Cases
        
        public record Case(string Name, int Value, int Boundary, Inclusion Inclusion);

        public sealed record ValidCase(string Name, int Value, int Boundary, Inclusion Inclusion, bool ExpectedGreaterThan, bool ExpectedLessThan) 
            : Case(Name, Value, Boundary, Inclusion);

        public record InvalidCase(string Name, int Value, int Boundary, Inclusion Inclusion, ExpectedException ExpectedException) 
            : Case(Name, Value, Boundary, Inclusion);

        #endregion
    }
}
