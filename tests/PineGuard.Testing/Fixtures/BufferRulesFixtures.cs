using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class BufferRulesFixtures
{
    public static class IsHex
    {
        public static readonly string? SingleDigit = "0";
        public static readonly string? MixedCase = "deadBEEF";
        public static readonly string? SingleChar = "F";
        public static readonly string? Long = new('a', 64);
        public static readonly string? Trimmed = " 0A1b ";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = " ";
        public static readonly string? ZeroXPrefix = "0x1";
        public static readonly string? NonHex = "GG";
        public static readonly string? Separator = "12-34";
        public static readonly string? Control = "\t\r\n";
        public static readonly string? NonAscii = "123\u0080";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(SingleDigit), SingleDigit, true),
            new(nameof(MixedCase),   MixedCase,   true),
            new(nameof(SingleChar),  SingleChar,  true),
            new(nameof(Long),        Long,         true)
        ];

        public static RuleScenario<string?>[] ValidEdgeScenarios =>
        [
            new(nameof(Trimmed), Trimmed, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NonHex),      NonHex,      false),
            new(nameof(ZeroXPrefix), ZeroXPrefix, false),
            new(nameof(Separator),   Separator,   false)
        ];

        public static RuleScenario<string?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null),      Null,      false),
            new(nameof(Empty),     Empty,     false),
            new(nameof(Whitespace), Whitespace, false),
            new(nameof(Control),   Control,   false),
            new(nameof(NonAscii),  NonAscii,  false)
        ];

        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsBase64
    {
        public static readonly string? SingleChar = "TQ==";
        public static readonly string? Hello = "SGVsbG8=";
        public static readonly string? Zero = "AA==";
        public static readonly string? NoPadding = "AAAA";
        public static readonly string? Trimmed = "  TQ==  ";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = " ";
        public static readonly string? BadPadding = "TQ=";
        public static readonly string? TooMuchPadding = "TQ===";
        public static readonly string? EmbeddedSpace = "T Q==";
        public static readonly string? InvalidChars = "****";
        public static readonly string? Length1 = "A";
        public static readonly string? Length3 = "AAA";
        public static readonly string? SpaceInMiddle = "AA A";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(SingleChar),  SingleChar,  true),
            new(nameof(Hello),       Hello,       true),
            new(nameof(Zero),        Zero,        true),
            new(nameof(NoPadding),   NoPadding,   true)
        ];

        public static RuleScenario<string?>[] ValidEdgeScenarios =>
        [
            new(nameof(Trimmed),      Trimmed,      true),
            new(nameof(EmbeddedSpace), EmbeddedSpace, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(BadPadding),      BadPadding,      false),
            new(nameof(TooMuchPadding),  TooMuchPadding,  false),
            new(nameof(InvalidChars),    InvalidChars,    false)
        ];

        public static RuleScenario<string?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null),          Null,          false),
            new(nameof(Empty),         Empty,         false),
            new(nameof(Whitespace),    Whitespace,    false),
            new(nameof(Length1),       Length1,       false),
            new(nameof(Length3),       Length3,       false),
            new(nameof(SpaceInMiddle), SpaceInMiddle, false)
        ];

        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsBase64Url
    {
        public static readonly string? Unpadded = "SGVsbG8";
        public static readonly string? Padded = "SGVsbG8=";
        public static readonly string? UrlSafeChars = "-_-_";
        public static readonly string? JwtHeaderSegment = "eyJhbGciOiJIUzI1NiJ9";
        public static readonly string? SingleQuantum = "AAAA";
        public static readonly string? TwoCharsUnpadded = "QQ";
        public static readonly string? TwoCharsPadded = "AA==";
        public static readonly string? Trimmed = "  SGVsbG8  ";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = " ";
        public static readonly string? PlusChar = "SGVsbG8+";
        public static readonly string? SlashChar = "SGVsbG8/";
        public static readonly string? EmbeddedSpace = "SG Vsb";
        public static readonly string? PaddingInMiddle = "A=BC";
        public static readonly string? Length1 = "A";
        public static readonly string? Length5 = "AAAAA";
        public static readonly string? BadPadding = "QQ=";
        public static readonly string? TooMuchPadding = "====";
        public static readonly string? OnlyPadding = "==";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Unpadded),         Unpadded,         true),
            new(nameof(Padded),           Padded,           true),
            new(nameof(UrlSafeChars),     UrlSafeChars,     true),
            new(nameof(JwtHeaderSegment), JwtHeaderSegment, true)
        ];

        public static RuleScenario<string?>[] ValidEdgeScenarios =>
        [
            new(nameof(SingleQuantum),    SingleQuantum,    true),
            new(nameof(TwoCharsUnpadded), TwoCharsUnpadded, true),
            new(nameof(TwoCharsPadded),   TwoCharsPadded,   true),
            new(nameof(Trimmed),          Trimmed,          true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(PlusChar),        PlusChar,        false),
            new(nameof(SlashChar),       SlashChar,       false),
            new(nameof(EmbeddedSpace),   EmbeddedSpace,   false),
            new(nameof(PaddingInMiddle), PaddingInMiddle, false)
        ];

        public static RuleScenario<string?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null),           Null,           false),
            new(nameof(Empty),          Empty,          false),
            new(nameof(Whitespace),     Whitespace,     false),
            new(nameof(Length1),        Length1,        false),
            new(nameof(Length5),        Length5,        false),
            new(nameof(BadPadding),     BadPadding,     false),
            new(nameof(TooMuchPadding), TooMuchPadding, false),
            new(nameof(OnlyPadding),    OnlyPadding,    false)
        ];

        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
