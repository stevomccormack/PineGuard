using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class NetworkRulesTestData
{
    public static class IsIpAddress
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("127.0.0.1 => true", "127.0.0.1", true),
            new("::1 => true", "::1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("not-an-ip => false", "not-an-ip", false),
            new("null => false", null, false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsIpv4
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("127.0.0.1 => true", "127.0.0.1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("256.0.0.1 => false", "256.0.0.1", false),
            new("127.0.0 => false", "127.0.0", false),
            new("::1 => false", "::1", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }

    public static class IsIpv6
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("::1 => true", "::1", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("127.0.0.1 => false", "127.0.0.1", false),
            new("not-an-ip => false", "not-an-ip", false),
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
