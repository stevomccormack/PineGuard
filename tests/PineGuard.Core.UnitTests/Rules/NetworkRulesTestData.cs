using Xunit;

namespace PineGuard.Core.UnitTests.Rules;

public static class NetworkRulesTestData
{
    public static class IsIpAddress
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "127.0.0.1 => true", Value: "127.0.0.1", Expected: true) },
            { new Case(Name: "::1 => true", Value: "::1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "not-an-ip => false", Value: "not-an-ip", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsIpv4
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "127.0.0.1 => true", Value: "127.0.0.1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "256.0.0.1 => false", Value: "256.0.0.1", Expected: false) },
            { new Case(Name: "127.0.0 => false", Value: "127.0.0", Expected: false) },
            { new Case(Name: "::1 => false", Value: "::1", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsIpv6
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "::1 => true", Value: "::1", Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "127.0.0.1 => false", Value: "127.0.0.1", Expected: false) },
            { new Case(Name: "not-an-ip => false", Value: "not-an-ip", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
