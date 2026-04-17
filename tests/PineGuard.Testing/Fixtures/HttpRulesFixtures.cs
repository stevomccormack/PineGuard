using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class HttpRulesFixtures
{
    public static class IsHeaderName
    {
        public static readonly string? Simple = "X-Test";
        public static readonly string? AllowedSymbols = "X_Test-123";
        public static readonly string? Trimmed = "  X-Test  ";
        public static readonly string? TokenBang = "X!";
        public static readonly string? TokenHash = "X#";
        public static readonly string? TokenDollar = "X$";
        public static readonly string? TokenPercent = "X%";
        public static readonly string? TokenAmpersand = "X&";
        public static readonly string? TokenApostrophe = "X'";
        public static readonly string? TokenStar = "X*";
        public static readonly string? TokenPlus = "X+";
        public static readonly string? TokenDash = "X-";
        public static readonly string? TokenDot = "X.";
        public static readonly string? TokenCaret = "X^";
        public static readonly string? TokenUnderscore = "X_";
        public static readonly string? TokenBacktick = "X`";
        public static readonly string? TokenPipe = "X|";
        public static readonly string? TokenTilde = "X~";
        public static readonly string? InvalidSpace = "X Test";
        public static readonly string? InvalidNewline = "X\nTest";
        public static readonly string? Null = null;
        public static readonly string? Empty = "";
        public static readonly string? Whitespace = "  ";
        public static readonly string? BoundaryColon = "X:";
        public static readonly string? BoundaryBracket = "X[";
        public static readonly string? BoundaryBrace = "X{";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Simple), Simple, true),
            new(nameof(AllowedSymbols), AllowedSymbols, true),
            new(nameof(Trimmed), Trimmed, true),
            new(nameof(TokenBang), TokenBang, true),
            new(nameof(TokenHash), TokenHash, true),
            new(nameof(TokenDollar), TokenDollar, true),
            new(nameof(TokenPercent), TokenPercent, true),
            new(nameof(TokenAmpersand), TokenAmpersand, true),
            new(nameof(TokenApostrophe), TokenApostrophe, true),
            new(nameof(TokenStar), TokenStar, true),
            new(nameof(TokenPlus), TokenPlus, true),
            new(nameof(TokenDash), TokenDash, true),
            new(nameof(TokenDot), TokenDot, true),
            new(nameof(TokenCaret), TokenCaret, true),
            new(nameof(TokenUnderscore), TokenUnderscore, true),
            new(nameof(TokenBacktick), TokenBacktick, true),
            new(nameof(TokenPipe), TokenPipe, true),
            new(nameof(TokenTilde), TokenTilde, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(InvalidSpace), InvalidSpace, false),
            new(nameof(InvalidNewline), InvalidNewline, false),
            new(nameof(Null), Null, false),
            new(nameof(Empty), Empty, false),
            new(nameof(Whitespace), Whitespace, false),
            new(nameof(BoundaryColon), BoundaryColon, false),
            new(nameof(BoundaryBracket), BoundaryBracket, false),
            new(nameof(BoundaryBrace), BoundaryBrace, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHeaderValue
    {
        public static readonly string? Simple = "abc";
        public static readonly string? Quoted = "\"value\"";
        public static readonly string? RejectCr = "a\rb";
        public static readonly string? RejectLf = "a\nb";
        public static readonly string? RejectControl = "a\u0001b";
        public static readonly string? Null = null;
        public static readonly string? ControlBell = "Value\u0007";
        public static readonly string? TrailingCr = "Value\r";
        public static readonly string? TrailingLf = "Value\n";
        public static readonly string? TrailingCrLf = "Value\r\n";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Simple), Simple, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(RejectCr), RejectCr, false),
            new(nameof(RejectLf), RejectLf, false),
            new(nameof(RejectControl), RejectControl, false),
            new(nameof(Null), Null, false),
            new(nameof(ControlBell), ControlBell, false),
            new(nameof(TrailingCr), TrailingCr, false),
            new(nameof(TrailingLf), TrailingLf, false),
            new(nameof(TrailingCrLf), TrailingCrLf, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpStatusCode
    {
        public static readonly int? LowerBound = 100;
        public static readonly int? Ok = 200;
        public static readonly int? NotFound = 404;
        public static readonly int? UpperBound = 599;
        public static readonly int? BelowRange = 99;
        public static readonly int? AboveRange = 600;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(LowerBound), LowerBound, true),
            new(nameof(Ok), Ok, true),
            new(nameof(NotFound), NotFound, true),
            new(nameof(UpperBound), UpperBound, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(BelowRange), BelowRange, false),
            new(nameof(AboveRange), AboveRange, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpStatusInformational
    {
        public static readonly int? InRange = 100;
        public static readonly int? BelowRange = 99;
        public static readonly int? AboveRange = 200;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(InRange), InRange, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(BelowRange), BelowRange, false),
            new(nameof(AboveRange), AboveRange, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpStatusSuccess
    {
        public static readonly int? InRange = 200;
        public static readonly int? Created = 201;
        public static readonly int? PartialContent = 206;
        public static readonly int? BelowRange = 199;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(InRange), InRange, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(BelowRange), BelowRange, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpStatusRedirect
    {
        public static readonly int? InRange = 300;
        public static readonly int? BelowRange = 299;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(InRange), InRange, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(BelowRange), BelowRange, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpStatusClientError
    {
        public static readonly int? InRange = 400;
        public static readonly int? BelowRange = 399;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(InRange), InRange, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(BelowRange), BelowRange, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsHttpStatusServerError
    {
        public static readonly int? InRange = 500;
        public static readonly int? BelowRange = 499;
        public static readonly int? Null = null;

        public static RuleScenario<int?>[] ValidScenarios =>
        [
            new(nameof(InRange), InRange, true)
        ];

        public static RuleScenario<int?>[] InvalidScenarios =>
        [
            new(nameof(BelowRange), BelowRange, false),
            new(nameof(Null), Null, false)
        ];

        public static RuleScenario<int?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class HasHeaderValue
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithTrimmedValue =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["  a  "] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithWhitespaceValue =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["  "] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithDifferentKey =
            new Dictionary<string, IEnumerable<string>> { ["Y"] = ["a"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithValue =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["a"] };

        public const string HeaderName = "X";
    }

    public static class HasHeaderValueEqualTo
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithSpacedValue =
            new Dictionary<string, IEnumerable<string>> { ["X"] = [" a "] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithWhitespaceAndValue =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["  ", "a"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithValueA =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["a"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> HeadersWithDifferentKey =
            new Dictionary<string, IEnumerable<string>> { ["Y"] = ["a"] };
    }

    public static class HasSingleHeaderValue
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> SingleValueHeaders =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["a"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MultipleValueHeaders =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["a", "b"] };
    }

    public static class HasContentType
    {
        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> JsonHeaders =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["application/json"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> MixedCaseJsonHeaders =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["Application/Json"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> PlainTextHeaders =
            new Dictionary<string, IEnumerable<string>> { ["Content-Type"] = ["text/plain"] };

        public static readonly IReadOnlyDictionary<string, IEnumerable<string>> NonContentTypeHeaders =
            new Dictionary<string, IEnumerable<string>> { ["X"] = ["y"] };
    }
}
