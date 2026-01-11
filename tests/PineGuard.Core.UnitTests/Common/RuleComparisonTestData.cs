using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public static class RuleComparisonTestData
{
    public static class Equality
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("0==0", (0, 0), true),
            new("-1==-1", (-1, -1), true),
            new("max==max", (int.MaxValue, int.MaxValue), true),
            new("5!=6", (5, 6), false),
            new("-5!=-6", (-5, -6), false),
            new("0!=1", (0, 1), false),
        ];

        public static TheoryData<Case> EdgeCases => [];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, (int Value, int Other) Value, bool ExpectedReturn)
            : IsCase<(int Value, int Other)>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsBetween
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("within", (5, 1, 10, Inclusion.Inclusive), true),
            new("equal inclusive", (1, 1, 1, Inclusion.Inclusive), true),
            new("at max inclusive", (5, 1, 5, Inclusion.Inclusive), true),
            new("at min inclusive", (5, 5, 10, Inclusion.Inclusive), true),
            new("min..max inclusive", (int.MinValue, int.MinValue, int.MaxValue, Inclusion.Inclusive), true),
            new("min..max inclusive (max)", (int.MaxValue, int.MinValue, int.MaxValue, Inclusion.Inclusive), true),
            new("0..0 inclusive", (0, 0, 0, Inclusion.Inclusive), true),
            new("-1..-1 inclusive", (-1, -1, -1, Inclusion.Inclusive), true),
            new("10..10 inclusive", (10, 10, 10, Inclusion.Inclusive), true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("equal exclusive", (1, 1, 1, Inclusion.Exclusive), false),
            new("at max exclusive", (5, 1, 5, Inclusion.Exclusive), false),
            new("at min exclusive", (5, 5, 10, Inclusion.Exclusive), false),
            new("min..max exclusive (min)", (int.MinValue, int.MinValue, int.MaxValue, Inclusion.Exclusive), false),
            new("min..max exclusive (max)", (int.MaxValue, int.MinValue, int.MaxValue, Inclusion.Exclusive), false),
            new("0..0 exclusive", (0, 0, 0, Inclusion.Exclusive), false),
            new("min>max", (5, 10, 1, Inclusion.Inclusive), false),
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("invalid inclusion 123", (5, 1, 10, (Inclusion)123), new ExpectedException(typeof(ArgumentOutOfRangeException))),
            new("invalid inclusion -1", (0, 0, 0, (Inclusion)(-1)), new ExpectedException(typeof(ArgumentOutOfRangeException))),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, (int Value, int Min, int Max, Inclusion Inclusion) Value, bool ExpectedReturn)
            : IsCase<(int Value, int Min, int Max, Inclusion Inclusion)>(Name, Value, ExpectedReturn);

        public sealed record InvalidCase(string Name, (int Value, int Min, int Max, Inclusion Inclusion) Value, ExpectedException ExpectedException)
            : ThrowsCase<(int Value, int Min, int Max, Inclusion Inclusion)>(Name, Value, ExpectedException);

        #endregion
    }

    public static class Boundaries
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("equal inclusive", (5, 5, Inclusion.Inclusive), (true, true)),
            new("equal exclusive", (5, 5, Inclusion.Exclusive), (false, false)),
            new("equal negative exclusive", (-5, -5, Inclusion.Exclusive), (false, false)),
            new("greater", (6, 5, Inclusion.Inclusive), (true, false)),
            new("less", (4, 5, Inclusion.Inclusive), (false, true)),
            new("max equal inclusive", (int.MaxValue, int.MaxValue, Inclusion.Inclusive), (true, true)),
            new("min equal exclusive", (int.MinValue, int.MinValue, Inclusion.Exclusive), (false, false)),
        ];

        public static TheoryData<ValidCase> EdgeCases => [];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("boundary invalid inclusion 123", (5, 5, (Inclusion)123), new ExpectedException(typeof(ArgumentOutOfRangeException))),
            new("boundary invalid inclusion -1", (0, 1, (Inclusion)(-1)), new ExpectedException(typeof(ArgumentOutOfRangeException))),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, (int Value, int Boundary, Inclusion Inclusion) Value, (bool ExpectedGreaterThan, bool ExpectedLessThan) ExpectedReturn)
            : ReturnCase<(int Value, int Boundary, Inclusion Inclusion), (bool ExpectedGreaterThan, bool ExpectedLessThan)>(Name, Value, ExpectedReturn);

        public sealed record InvalidCase(string Name, (int Value, int Boundary, Inclusion Inclusion) Value, ExpectedException ExpectedException)
            : ThrowsCase<(int Value, int Boundary, Inclusion Inclusion)>(Name, Value, ExpectedException);

        #endregion
    }
}
