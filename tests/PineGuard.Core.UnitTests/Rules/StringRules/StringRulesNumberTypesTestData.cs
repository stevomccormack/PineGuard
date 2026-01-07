using PineGuard.Common;
using Xunit;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesNumberTypesTestData
{
    public static class IsDecimal
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "1.23 => true", Value: "1.23", Expected: true) },
            { new Case(Name: "-1.2 => true", Value: "-1.2", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Value: null, Expected: false) },
            { new Case(Name: "space => false", Value: " ", Expected: false) },
            { new Case(Name: "not => false", Value: "not", Expected: false) },
            { new Case(Name: "too many decimals => false", Value: "1.234", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsDecimalWithZeroPlaces
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "123 => true", Value: "123", Expected: true) },
            { new Case(Name: "+123 => true", Value: "+123", Expected: true) },
            { new Case(Name: "-0 => true", Value: "-0", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1.0 => false", Value: "1.0", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsExactDecimal
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "1.20 => true", Value: "1.20", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null => false", Value: null, Expected: false) },
            { new Case(Name: "space => false", Value: " ", Expected: false) },
            { new Case(Name: "not => false", Value: "not", Expected: false) },
            { new Case(Name: "not enough decimals => false", Value: "1.2", Expected: false) },
            { new Case(Name: "too many decimals => false", Value: "1.230", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsExactDecimalWithZeroPlaces
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "123 => true", Value: "123", Expected: true) },
            { new Case(Name: "+123 => true", Value: "+123", Expected: true) },
            { new Case(Name: "-0 => true", Value: "-0", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1.0 => false", Value: "1.0", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInt32
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "int.MaxValue => true", Value: "2147483647", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "int.MaxValue+1 => false", Value: "2147483648", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInt64
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "long.MaxValue => true", Value: "9223372036854775807", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "long.MaxValue+1 => false", Value: "9223372036854775808", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInt32InRange
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "5 in [1,10] => true", Value: "5", Min: 1, Max: 10, Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "1 in [1,10] => true", Value: "1", Min: 1, Max: 10, Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 in (1,10) => false", Value: "1", Min: 1, Max: 10, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not => false", Value: "not", Min: 1, Max: 10, Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string Value, int Min, int Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInt64InRange
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "5 in [1,10] => true", Value: "5", Min: 1L, Max: 10L, Inclusion: Inclusion.Inclusive, Expected: true) },
            { new Case(Name: "1 in [1,10] => true", Value: "1", Min: 1L, Max: 10L, Inclusion: Inclusion.Inclusive, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "1 in (1,10) => false", Value: "1", Min: 1L, Max: 10L, Inclusion: Inclusion.Exclusive, Expected: false) },
            { new Case(Name: "not => false", Value: "not", Min: 1L, Max: 10L, Inclusion: Inclusion.Inclusive, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string Value, long Min, long Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
