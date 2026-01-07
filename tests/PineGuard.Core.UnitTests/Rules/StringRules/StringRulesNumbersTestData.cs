using PineGuard.Common;
using Xunit;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumbersTestData
{
    public static class IsPositive
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "1 => true", Value: "1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "0 => false", Value: "0", Expected: false) },
            { new Case(Name: "-1 => false", Value: "-1", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
            { new Case(Name: "space => false", Value: " ", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNegative
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "-1 => true", Value: "-1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "0 => false", Value: "0", Expected: false) },
            { new Case(Name: "1 => false", Value: "1", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsZero
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "0 => true", Value: "0", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 => false", Value: "1", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNotZero
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "1 => true", Value: "1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "0 => false", Value: "0", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsZeroOrPositive
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "0 => true", Value: "0", Expected: true) },
            { new Case(Name: "1 => true", Value: "1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "-1 => false", Value: "-1", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsZeroOrNegative
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "0 => true", Value: "0", Expected: true) },
            { new Case(Name: "-1 => true", Value: "-1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 => false", Value: "1", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "2 > 1 => true", Value: "2", Min: 1m, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 > 1 => false", Value: "1", Min: 1m, Expected: false) },
            { new Case(Name: "abc > 1 => false", Value: "abc", Min: 1m, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Min, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGreaterThanOrEqual
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "2 >= 1 => true", Value: "2", Min: 1m, Expected: true) },
            { new Case(Name: "1 >= 1 => true", Value: "1", Min: 1m, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "0 >= 1 => false", Value: "0", Min: 1m, Expected: false) },
            { new Case(Name: "abc >= 1 => false", Value: "abc", Min: 1m, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Min, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "0 < 1 => true", Value: "0", Max: 1m, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 < 1 => false", Value: "1", Max: 1m, Expected: false) },
            { new Case(Name: "abc < 1 => false", Value: "abc", Max: 1m, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Max, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsLessThanOrEqual
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "0 <= 1 => true", Value: "0", Max: 1m, Expected: true) },
            { new Case(Name: "1 <= 1 => true", Value: "1", Max: 1m, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "2 <= 1 => false", Value: "2", Max: 1m, Expected: false) },
            { new Case(Name: "abc <= 1 => false", Value: "abc", Max: 1m, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Max, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInRange
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "5 in [1,10] => true", Value: "5", Min: 1m, Max: 10m, Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 in (1,10) => false", Value: "1", Min: 1m, Max: 10m, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "abc in [1,10] => false", Value: "abc", Min: 1m, Max: 10m, Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Min, decimal Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsApproximately
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "10.0 ~= 10.1 +/- 0.2 => true", Value: "10.0", Target: 10.1m, Tolerance: 0.2m, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "10.0 ~= 10.3 +/- 0.2 => false", Value: "10.0", Target: 10.3m, Tolerance: 0.2m, Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Target: 10.0m, Tolerance: 0.2m, Expected: false) },
            { new Case(Name: "null tolerance => false", Value: "10.0", Target: 10.0m, Tolerance: null, Expected: false) },
            { new Case(Name: "negative tolerance => false", Value: "10.0", Target: 10.0m, Tolerance: -0.1m, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Target, decimal? Tolerance, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsMultipleOf
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "4 multiple of 2 => true", Value: "4", Factor: 2m, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "5 multiple of 2 => false", Value: "5", Factor: 2m, Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Factor: 2m, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, decimal Factor, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsEven
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "4 => true", Value: "4", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "5 => false", Value: "5", Expected: false) },
            { new Case(Name: "4.0 => false", Value: "4.0", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOdd
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "5 => true", Value: "5", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "4 => false", Value: "4", Expected: false) },
            { new Case(Name: "5.0 => false", Value: "5.0", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFinite
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "1.23 => true", Value: "1.23", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNaN
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "NaN => true", Value: "NaN", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1.23 => false", Value: "1.23", Expected: false) },
            { new Case(Name: "abc => false", Value: "abc", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
