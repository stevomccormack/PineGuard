using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public static class EmailUtilityTestData
{
    public static class TryCreate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "user@example.com", true, "user@example.com"),
            new("display name", "User <user@example.com>", true, "user@example.com"),
            new("trim", " user@example.com ", true, "user@example.com")];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("space", " ", false, null),
            new("whitespace", "\t\r\n", false, null),
            new("not email", "not-an-email", false, null),
            new("missing domain", "user@", false, null),
            new("missing local", "@example.com", false, null)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryStrictCreate
    {
        // EmailUtility.MaxDomainLength = 255; a domain exceeding this always exceeds the overall
        // MaxEmailLength (254) cap as well (local part + '@' add at least 2 more characters), so
        // this case is rejected via the total-length check rather than a standalone domain-length check.
        private static readonly string TooLongDomain = new string('a', EmailUtility.MaxDomainLength - 2) + ".com";

        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "user@example.com", true, "user@example.com"),
            new("uppercase local", "USER@example.com", true, "USER@example.com"),
            new("uppercase domain", "user@EXAMPLE.com", true, "user@EXAMPLE.com"),
            new("trim", " user@example.com ", true, "user@example.com"),
            new("plus alias", "user+alias@example.com", true, "user+alias@example.com")];

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
            new("domain exceeds MaxDomainLength (caught by overall length cap)", "user@" + TooLongDomain, false, string.Empty),
            new("idn arg exception", "user@\u0000.com", false, string.Empty),
            new("invalid local", "\u0000user@example.com", false, string.Empty),
            new("angle brackets without space", "User<user@example.com>", false, string.Empty),
            new("no dot in domain (dup)", "user@example", false, string.Empty)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryGetAlias
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "user+alias@example.com", true, "alias"),
            new("trim", " user+alias@example.com ", true, "alias")];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("no plus", "user@example.com", false, string.Empty),
            new("missing alias", "user+@example.com", false, string.Empty),
            new("display name", "User <user+alias@example.com>", false, string.Empty)];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, Expected, ExpectedOutValue);
    }
}
