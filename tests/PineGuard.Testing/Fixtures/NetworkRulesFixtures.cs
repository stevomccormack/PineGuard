using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class NetworkRulesFixtures
{
    public static class IsIpAddress
    {
        public static readonly string? Ipv4Loopback = "127.0.0.1";
        public static readonly string? Ipv6Loopback = "::1";
        public static readonly string? NotAnIp = "not-an-ip";
        public static readonly string? InetAtonShorthand = "1";
        public static readonly string? Ipv4PartialDottedQuad = "192.168.1";
        public static readonly string? Null = null;

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Ipv4Loopback), Ipv4Loopback, true),
            new(nameof(Ipv6Loopback), Ipv6Loopback, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NotAnIp), NotAnIp, false),
            new(nameof(InetAtonShorthand), InetAtonShorthand, false),
            new(nameof(Ipv4PartialDottedQuad), Ipv4PartialDottedQuad, false),
            new(nameof(Null),    Null,    false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsIpv4
    {
        public static readonly string? Loopback = "127.0.0.1";
        public static readonly string? OutOfRange = "256.0.0.1";
        public static readonly string? Incomplete = "127.0.0";
        public static readonly string? Ipv6Loopback = "::1";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Loopback), Loopback, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(OutOfRange),   OutOfRange,   false),
            new(nameof(Incomplete),   Incomplete,   false),
            new(nameof(Ipv6Loopback), Ipv6Loopback, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsIpv6
    {
        public static readonly string? Loopback = "::1";
        public static readonly string? Ipv4Loopback = "127.0.0.1";
        public static readonly string? NotAnIp = "not-an-ip";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Loopback), Loopback, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Ipv4Loopback), Ipv4Loopback, false),
            new(nameof(NotAnIp),      NotAnIp,      false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsInCidr
    {
        public static readonly (string? ip, string cidr) InRange = ("192.168.1.10", "192.168.1.0/24");
        public static readonly (string? ip, string cidr) OutOfRange = ("192.168.2.10", "192.168.1.0/24");
        public static readonly (string? ip, string cidr) NullIp = (null, "192.168.1.0/24");
        public static readonly (string? ip, string cidr) InvalidIp = ("x", "192.168.1.0/24");
        public static readonly (string? ip, string cidr) InvalidCidr = ("192.168.1.10", "x/24");
        public static readonly (string? ip, string cidr) EmptyCidr = ("192.168.1.10", "");
        public static readonly (string? ip, string cidr) CidrPrefixWithSign = ("10.0.0.5", "10.0.0.0/+8");

        public static RuleScenario<(string? ip, string cidr)>[] ValidScenarios =>
        [
            new(nameof(InRange), InRange, true)
        ];

        public static RuleScenario<(string? ip, string cidr)>[] InvalidScenarios =>
        [
            new(nameof(OutOfRange),  OutOfRange,  false),
            new(nameof(NullIp),      NullIp,      false),
            new(nameof(InvalidIp),   InvalidIp,   false),
            new(nameof(InvalidCidr), InvalidCidr, false),
            new(nameof(CidrPrefixWithSign), CidrPrefixWithSign, false)
        ];

        public static RuleScenario<(string? ip, string cidr)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsValidHostname
    {
        public static readonly string? Simple = "example.com";
        public static readonly string? SingleLabel = "localhost";
        public static readonly string? Trimmed = "  example.com  ";
        public static readonly string? TrailingDot = "example.com.";
        public static readonly string? MultipleTrailingDots = "example.com..";
        public static readonly string? WithHyphen = "a-b.example";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? DotOnly = ".";
        public static readonly string? InvalidSpace = "exa mple.com";
        public static readonly string? LabelWithInteriorSpace = "a. b.com";
        public static readonly string? LeadingHyphen = "-a.com";
        public static readonly string? TrailingHyphen = "a-.com";
        public static readonly string? InvalidChar = "a_.com";
        public static readonly string? LabelTooLong = new string('a', 64) + ".com";
        public static readonly string? OverallTooLong = new('a', 254);
        public static readonly string? OverallTooLongValidLabels = new string('a', 63) + "." + new string('a', 63) + "." + new string('a', 63) + "." + new string('a', 62);

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Simple),      Simple,      true),
            new(nameof(SingleLabel), SingleLabel, true),
            new(nameof(Trimmed),     Trimmed,     true),
            new(nameof(TrailingDot), TrailingDot, true),
            new(nameof(WithHyphen),  WithHyphen,  true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Null),                      Null,                      false),
            new(nameof(Empty),                     Empty,                     false),
            new(nameof(DotOnly),                   DotOnly,                   false),
            new(nameof(MultipleTrailingDots),      MultipleTrailingDots,      false),
            new(nameof(InvalidSpace),              InvalidSpace,              false),
            new(nameof(LabelWithInteriorSpace),    LabelWithInteriorSpace,    false),
            new(nameof(LeadingHyphen),             LeadingHyphen,             false),
            new(nameof(TrailingHyphen),             TrailingHyphen,             false),
            new(nameof(InvalidChar),               InvalidChar,               false),
            new(nameof(LabelTooLong),              LabelTooLong,              false),
            new(nameof(OverallTooLong),             OverallTooLong,             false),
            new(nameof(OverallTooLongValidLabels), OverallTooLongValidLabels, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsPortNumber
    {
        public static readonly int? Min = 1;
        public static readonly int? Max = 65535;
        public static readonly int? Mid = 443;
        public static readonly int? Null = null;
        public static readonly int? Zero = 0;
        public static readonly int? TooBig = 65536;
        public static readonly int? Negative = -1;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(Mid), Mid, true)
        ];

        public static RuleScenario<int?>[] ValidEdgeScenarios =>
        [
            new(nameof(Min), Min, true),
            new(nameof(Max), Max, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Zero),     Zero,     false),
            new(nameof(TooBig),   TooBig,   false),
            new(nameof(Negative), Negative, false)
        ];

        public static RuleScenario<int?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<int?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<int?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsMacAddress
    {
        public static readonly string? ColonUpper = "01:23:45:67:89:AB";
        public static readonly string? ColonLower = "01:23:45:67:89:ab";
        public static readonly string? HyphenUpper = "01-23-45-67-89-AB";
        public static readonly string? HyphenLower = "01-23-45-67-89-ab";
        public static readonly string? CiscoDotted = "0123.4567.89ab";
        public static readonly string? CiscoDottedUpper = "0123.4567.89AB";
        public static readonly string? Padded = "  01:23:45:67:89:AB  ";
        public static readonly string? AllZeros = "00:00:00:00:00:00";
        public static readonly string? Broadcast = "FF:FF:FF:FF:FF:FF";
        public static readonly string? Null = null;
        public static readonly string? EmptyString = "";
        public static readonly string? WhiteSpace = "   ";
        public static readonly string? NoSeparator = "0123456789AB";
        public static readonly string? MixedSeparators = "01-23-45-67-89:AB";
        public static readonly string? SpaceSeparated = "01 23 45 67 89 AB";
        public static readonly string? NonHexOctet = "01:23:45:67:89:AG";
        public static readonly string? CiscoNonHexGroup = "0123.4567.89zz";
        public static readonly string? CiscoWrongSeparator = "0123-4567-89ab";
        public static readonly string? TooFewOctets = "01:23:45:67:89";
        public static readonly string? TooManyOctets = "01:23:45:67:89:AB:CD";
        public static readonly string? SingleDigitOctet = "1:23:45:67:89:AB";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(ColonUpper),       ColonUpper,       true),
            new(nameof(ColonLower),       ColonLower,       true),
            new(nameof(HyphenUpper),      HyphenUpper,      true),
            new(nameof(HyphenLower),      HyphenLower,      true),
            new(nameof(CiscoDotted),      CiscoDotted,      true),
            new(nameof(CiscoDottedUpper), CiscoDottedUpper, true),
            new(nameof(Padded),           Padded,           true)
        ];

        public static RuleScenario<string?>[] ValidEdgeScenarios =>
        [
            new(nameof(AllZeros),  AllZeros,  true),
            new(nameof(Broadcast), Broadcast, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Null),                Null,                false),
            new(nameof(EmptyString),         EmptyString,         false),
            new(nameof(WhiteSpace),          WhiteSpace,          false),
            new(nameof(NoSeparator),         NoSeparator,         false),
            new(nameof(MixedSeparators),     MixedSeparators,     false),
            new(nameof(SpaceSeparated),      SpaceSeparated,      false),
            new(nameof(NonHexOctet),         NonHexOctet,         false),
            new(nameof(CiscoNonHexGroup),    CiscoNonHexGroup,    false),
            new(nameof(CiscoWrongSeparator), CiscoWrongSeparator, false)
        ];

        public static RuleScenario<string?>[] InvalidEdgeScenarios =>
        [
            new(nameof(TooFewOctets),     TooFewOctets,     false),
            new(nameof(TooManyOctets),    TooManyOctets,    false),
            new(nameof(SingleDigitOctet), SingleDigitOctet, false)
        ];

        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
