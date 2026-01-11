using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class EmailUtilityTestData
{
    public static class TryCreate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "user@example.com", true, "user@example.com"),
            new("display name", "User <user@example.com>", true, "user@example.com"),
            new("trim", " user@example.com ", true, "user@example.com"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("space", " ", false, null),
            new("whitespace", "\t\r\n", false, null),
            new("not email", "not-an-email", false, null),
            new("missing domain", "user@", false, null),
            new("missing local", "@example.com", false, null),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryStrictCreate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "user@example.com", true, "user@example.com"),
            new("uppercase local", "USER@example.com", true, "USER@example.com"),
            new("uppercase domain", "user@EXAMPLE.com", true, "user@EXAMPLE.com"),
            new("trim", " user@example.com ", true, "user@example.com"),
            new("plus alias", "user+alias@example.com", true, "user+alias@example.com"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            // Unicode domain should normalize to punycode.
            new("unicode punycode", "user@bücher.com", true, "user@xn--bcher-kva.com"),
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("display name", "User <user@example.com>", false, string.Empty),
            new("no dot domain", "user@example", false, string.Empty),
            new("trailing dot", "user@example.", false, string.Empty),
            new("double at", "user@@example.com", false, string.Empty),
            new("missing domain", "user@", false, string.Empty),
            new("missing local", "@example.com", false, string.Empty),
            new("space in domain", "user@exa mple.com", false, string.Empty),
            new("missing at", "userexample.com", false, string.Empty),
            new("local too long", new string('a', 65) + "@example.com", false, string.Empty),
            new("overall too long", new string('a', 64) + "@" + new string('b', 189) + ".com", false, string.Empty),
            new("domain too long", "user@" + new string('a', 252) + ".com", false, string.Empty),
            new("idn arg exception", "user@\u0000.com", false, string.Empty),
            new("invalid local", "\u0000user@example.com", false, string.Empty),
            new("angle brackets without space", "User<user@example.com>", false, string.Empty),
            new("no dot in domain (dup)", "user@example", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryGetAlias
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "user+alias@example.com", true, "alias"),
            new("trim", " user+alias@example.com ", true, "alias"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("no plus", "user@example.com", false, string.Empty),
            new("missing alias", "user+@example.com", false, string.Empty),
            new("display name", "User <user+alias@example.com>", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }
}
