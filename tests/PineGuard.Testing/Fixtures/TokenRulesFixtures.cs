using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class TokenRulesFixtures
{
    public static class IsJwt
    {
        public const char Separator = TokenRules.JwtSegmentSeparator;

        public static readonly string HeaderSegment = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
        public static readonly string PayloadSegment = "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ";
        public static readonly string SignatureSegment = "SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        public static readonly string JsonObjectSegment = "eyJhIjoxfQ";
        public static readonly string JsonArraySegment = "WyJhIl0";
        public static readonly string JsonStringSegment = "ImEi";
        public static readonly string PlainTextSegment = "YWJj";
        public static readonly string NonUtf8Segment = "-_4";
        public static readonly string UrlSafeSegment = "-_-_";
        public static readonly string PaddedSegment = "eyJhIjoxfQ==";
        public static readonly string PlusCharSegment = "eyJhIjox+Q";
        public static readonly string SingleCharSegment = "A";

        public static readonly string? Canonical = $"{HeaderSegment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? MinimalSegments = $"{JsonObjectSegment}{Separator}{JsonObjectSegment}{Separator}{PlainTextSegment}";
        public static readonly string? UrlSafeSignature = $"{HeaderSegment}{Separator}{PayloadSegment}{Separator}{UrlSafeSegment}";
        public static readonly string? Padded = $"  {Canonical}  ";
        public static readonly string? NullValue = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = "   ";
        public static readonly string? NoSeparator = HeaderSegment;
        public static readonly string? TwoSegments = $"{HeaderSegment}{Separator}{PayloadSegment}";
        public static readonly string? FourSegments = $"{Canonical}{Separator}{SignatureSegment}";
        public static readonly string? EmptyHeader = $"{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? EmptyPayload = $"{HeaderSegment}{Separator}{Separator}{SignatureSegment}";
        public static readonly string? EmptySignature = $"{HeaderSegment}{Separator}{PayloadSegment}{Separator}";
        public static readonly string? PaddedHeader = $"{PaddedSegment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? WhitespaceInHeader = $"{HeaderSegment} {Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? NonBase64UrlHeader = $"{PlusCharSegment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? SingleCharSignature = $"{HeaderSegment}{Separator}{PayloadSegment}{Separator}{SingleCharSegment}";
        public static readonly string? HeaderNotJson = $"{PlainTextSegment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? HeaderJsonArray = $"{JsonArraySegment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? HeaderJsonString = $"{JsonStringSegment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? HeaderNotUtf8 = $"{NonUtf8Segment}{Separator}{PayloadSegment}{Separator}{SignatureSegment}";
        public static readonly string? PayloadNotJson = $"{HeaderSegment}{Separator}{PlainTextSegment}{Separator}{SignatureSegment}";
        public static readonly string? PayloadJsonArray = $"{HeaderSegment}{Separator}{JsonArraySegment}{Separator}{SignatureSegment}";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Canonical), Canonical, true), new(nameof(MinimalSegments), MinimalSegments, true), new(nameof(UrlSafeSignature), UrlSafeSignature, true)];
        public static RuleScenario<string?>[] ValidEdgeScenarios => [new(nameof(Padded), Padded, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(NullValue), NullValue, false), new(nameof(Empty), Empty, false), new(nameof(Whitespace), Whitespace, false), new(nameof(NoSeparator), NoSeparator, false), new(nameof(TwoSegments), TwoSegments, false), new(nameof(FourSegments), FourSegments, false), new(nameof(HeaderNotJson), HeaderNotJson, false), new(nameof(HeaderJsonArray), HeaderJsonArray, false), new(nameof(HeaderJsonString), HeaderJsonString, false), new(nameof(PayloadNotJson), PayloadNotJson, false), new(nameof(PayloadJsonArray), PayloadJsonArray, false)];
        public static RuleScenario<string?>[] InvalidEdgeScenarios => [new(nameof(EmptyHeader), EmptyHeader, false), new(nameof(EmptyPayload), EmptyPayload, false), new(nameof(EmptySignature), EmptySignature, false), new(nameof(PaddedHeader), PaddedHeader, false), new(nameof(WhitespaceInHeader), WhitespaceInHeader, false), new(nameof(NonBase64UrlHeader), NonBase64UrlHeader, false), new(nameof(SingleCharSignature), SingleCharSignature, false), new(nameof(HeaderNotUtf8), HeaderNotUtf8, false)];
        public static RuleScenario<string?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<string?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<string?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
