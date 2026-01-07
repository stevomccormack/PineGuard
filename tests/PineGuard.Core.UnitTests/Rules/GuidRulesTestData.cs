namespace PineGuard.Core.UnitTests.Rules;

public static class GuidRulesTestData
{
    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty", Guid.Empty, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("Non-empty", Guid.NewGuid(), false),
        };

        #region Cases

        public sealed record Case(string Name, Guid? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Null", null, true),
            new("Empty", Guid.Empty, true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Non-empty", Guid.NewGuid(), false),
        };

        #region Cases

        public sealed record Case(string Name, Guid? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGuid
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("D format", Guid.NewGuid().ToString("D"), true),
            new("N format", Guid.NewGuid().ToString("N"), true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Not a GUID", "not-a-guid", false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsGuidEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            new("Empty GUID", Guid.Empty.ToString("D"), true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Not a GUID", "not-a-guid", false),
            new("Non-empty GUID", Guid.NewGuid().ToString("D"), false),
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
