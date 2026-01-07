using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class EmailUtilityTestData
{
    public static class TryCreate
    {
        private static ValidCase V(string name, string? value, bool expected) => new(name, value, expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("simple", "user@example.com", true) },
            { V("display name", "User <user@example.com>", true) },
            { V("trim", " user@example.com ", true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, false) },
            { V("empty", string.Empty, false) },
            { V("space", " ", false) },
            { V("whitespace", "\t\r\n", false) },
            { V("not email", "not-an-email", false) },
            { V("missing domain", "user@", false) },
            { V("missing local", "@example.com", false) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : Case(Name);

        #endregion
    }

    public static class TryStrictCreate
    {
        private static ValidCase V(string name, string? value, bool expected, string expectedAddress) => new(name, value, expected, expectedAddress);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("simple", "user@example.com", true, "user@example.com") },
            { V("uppercase local", "USER@example.com", true, "USER@example.com") },
            { V("uppercase domain", "user@EXAMPLE.com", true, "user@EXAMPLE.com") },
            { V("trim", " user@example.com ", true, "user@example.com") },
            { V("plus alias", "user+alias@example.com", true, "user+alias@example.com") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            // Unicode domain should normalize to punycode.
            { V("unicode punycode", "user@bücher.com", true, "user@xn--bcher-kva.com") },
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("display name", "User <user@example.com>", false, string.Empty) },
            { V("no dot domain", "user@example", false, string.Empty) },
            { V("trailing dot", "user@example.", false, string.Empty) },
            { V("double at", "user@@example.com", false, string.Empty) },
            { V("missing domain", "user@", false, string.Empty) },
            { V("missing local", "@example.com", false, string.Empty) },
            { V("space in domain", "user@exa mple.com", false, string.Empty) },
            { V("missing at", "userexample.com", false, string.Empty) },
            { V("local too long", new string('a', 65) + "@example.com", false, string.Empty) },
            { V("overall too long", new string('a', 64) + "@" + new string('b', 189) + ".com", false, string.Empty) },
            { V("domain too long", "user@" + new string('a', 252) + ".com", false, string.Empty) },
            { V("idn arg exception", "user@\u0000.com", false, string.Empty) },
            { V("invalid local", "\u0000user@example.com", false, string.Empty) },
            { V("angle brackets without space", "User<user@example.com>", false, string.Empty) },
            { V("no dot in domain (dup)", "user@example", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedAddress)
            : Case(Name);

        #endregion
    }

    public static class TryGetAlias
    {
        private static ValidCase V(string name, string? value, bool expected, string expectedAlias) => new(name, value, expected, expectedAlias);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("simple", "user+alias@example.com", true, "alias") },
            { V("trim", " user+alias@example.com ", true, "alias") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("no plus", "user@example.com", false, string.Empty) },
            { V("missing alias", "user+@example.com", false, string.Empty) },
            { V("display name", "User <user+alias@example.com>", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedAlias)
            : Case(Name);

        #endregion
    }
}
