using PineGuard.Common;

namespace PineGuard.Core.UnitTests.Rules;

public static class NumberRulesTestData
{
    public static class IsPositiveInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("positive", 1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("zero", 0, false) },
            { new Case("negative", -1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNegativeInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("negative", -1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("zero", 0, false) },
            { new Case("positive", 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsZeroInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("zero", 0, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("non-zero", 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNotZeroInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("positive", 1, true) },
            { new Case("negative", -1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("zero", 0, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsZeroOrPositiveInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("zero", 0, true) },
            { new Case("positive", 1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("negative", -1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsZeroOrNegativeInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("zero", 0, true) },
            { new Case("negative", -1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("positive", 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGreaterThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("greater", 2, 1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("equal", 1, 1, false) },
            { new Case("null", null, 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, int Min, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGreaterThanOrEqual
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("greater", 2, 1, true) },
            { new Case("equal", 1, 1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("less", 0, 1, false) },
            { new Case("null", null, 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, int Min, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsLessThan
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("less", 0, 1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("equal", 1, 1, false) },
            { new Case("null", null, 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, int Max, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsLessThanOrEqual
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("less", 0, 1, true) },
            { new Case("equal", 1, 1, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("greater", 2, 1, false) },
            { new Case("null", null, 1, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, int Max, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsInRange
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("between inclusive", 5, 1, 10, Inclusion.Inclusive, true) },
            { new Case("min inclusive", 1, 1, 10, Inclusion.Inclusive, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("min exclusive", 1, 1, 10, Inclusion.Exclusive, false) },
            { new Case("null", null, 1, 10, Inclusion.Inclusive, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, int Min, int Max, Inclusion Inclusion, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsApproximately
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("within tolerance", 10.0m, 10.1m, 0.2m, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("outside tolerance", 10.0m, 10.3m, 0.2m, false) },
            { new Case("null value", null, 10.0m, 0.2m, false) },
            { new Case("null tolerance", 10.0m, 10.0m, null, false) },
            { new Case("negative tolerance", 10.0m, 10.0m, -0.1m, false) },
        };

        #region Cases

        public sealed record Case(string Name, decimal? Value, decimal Target, decimal? Tolerance, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsMultipleOf
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("multiple", 4, 2, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("not multiple", 5, 2, false) },
            { new Case("null", null, 2, false) },
            { new Case("zero factor", 4, 0, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, int Factor, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsEvenInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("even", 4, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("odd", 5, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsEvenLong
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("even", 4L, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("odd", 5L, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, long? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOddInt
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("odd", 5, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("even", 4, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsOddLong
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("odd", 5L, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("even", 4L, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, long? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFiniteFloat
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("finite", 1.0f, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("infinity", float.PositiveInfinity, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, float? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFiniteDouble
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("finite", 1.0, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("infinity", double.PositiveInfinity, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, double? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNaNFloat
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("NaN", float.NaN, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("finite", 1.0f, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, float? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNaNDouble
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("NaN", double.NaN, true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("finite", 1.0, false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, double? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
