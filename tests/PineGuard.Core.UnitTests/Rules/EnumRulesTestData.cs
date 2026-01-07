using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PineGuard.Core.UnitTests.Rules;

public static class EnumRulesTestData
{
    public enum SimpleEnum
    {
        A = 1,
        B = 2,
    }

    [Flags]
    public enum SampleFlags
    {
        None = 0,
        One = 1,
        Two = 2,
        Four = 4,
    }

    public enum AttributedEnum
    {
        None = 0,

        [Description("desc")]
        WithDescription = 1,

        [Display(Name = "display")]
        WithDisplay = 2,

        [EnumMember(Value = "member")]
        WithEnumMember = 3,

        [Obsolete]
        Obsolete = 4,
    }

    public static class IsDefined
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("A", SimpleEnum.A, true),
            new("B", SimpleEnum.B, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("Undefined", (SimpleEnum)999, false),
        };

        #region Cases

        public sealed record Case(string Name, SimpleEnum? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsDefinedValue
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("A", 1, true),
            new("B", 2, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("Zero", 0, false),
            new("Undefined", 999, false),
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsDefinedName
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Exact A", "A", true, true),
            new("Trimmed a", " a ", true, true),
            new("Lowercase a (ignore case)", "a", true, true),
            new("B (case-sensitive)", "B", false, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, true, false),
            new("Whitespace", " ", true, false),
            new("Lowercase a (case-sensitive)", "a", false, false),
            new("Missing", "Missing", true, false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool IgnoreCase, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFlagsEnumCombination
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("None", SampleFlags.None, true),
            new("One", SampleFlags.One, true),
            new("One | Two", SampleFlags.One | SampleFlags.Two, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("Undefined bit", (SampleFlags)8, false),
            new("Undefined bit + One", (SampleFlags)9, false),
        };

        #region Cases

        public sealed record Case(string Name, SampleFlags? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsFlagsEnumCombinationNonFlags
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Defined", SimpleEnum.A, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Undefined", (SimpleEnum)999, false),
        };

        #region Cases

        public sealed record Case(string Name, SimpleEnum? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasFlag
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Contains Two", SampleFlags.One | SampleFlags.Two, SampleFlags.Two, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, SampleFlags.One, false),
            new("Missing flag", SampleFlags.One, SampleFlags.Two, false),
        };

        #region Cases

        public sealed record Case(string Name, SampleFlags? Value, SampleFlags Flag, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasDescription
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("WithDescription", AttributedEnum.WithDescription, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("None", AttributedEnum.None, false),
            new("Undefined", (AttributedEnum)999, false),
        };

        #region Cases

        public sealed record Case(string Name, AttributedEnum? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasDisplay
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("WithDisplay", AttributedEnum.WithDisplay, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("None", AttributedEnum.None, false),
            new("Undefined", (AttributedEnum)999, false),
        };

        #region Cases

        public sealed record Case(string Name, AttributedEnum? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class HasEnumMember
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("WithEnumMember", AttributedEnum.WithEnumMember, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("None", AttributedEnum.None, false),
            new("Undefined", (AttributedEnum)999, false),
        };

        #region Cases

        public sealed record Case(string Name, AttributedEnum? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsObsolete
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Obsolete", (AttributedEnum)4, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("None", AttributedEnum.None, false),
            new("Undefined", (AttributedEnum)999, false),
        };

        #region Cases

        public sealed record Case(string Name, AttributedEnum? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
