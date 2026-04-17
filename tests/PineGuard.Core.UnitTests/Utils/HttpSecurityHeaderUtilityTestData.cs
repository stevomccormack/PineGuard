using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class HttpSecurityHeaderUtilityTestData
{
    public static class TrySplitSemicolonSeparatedSegments
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("standard headers", " max-age=1 ; includeSubDomains ; ", true, ["max-age=1", "includeSubDomains"]),
            new("single segment", "no-sniff", true, ["no-sniff"])
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", "", false, null),
            new("whitespace", "   ", false, null),
            new("semicolons only", ";;;", false, null),
            new("semicolons and whitespace", " ;  ;   ", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, IReadOnlyList<string>? ExpectedOutValue)
            : TryCase<string?, IReadOnlyList<string>?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class ParseHstsDirectives
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("full HSTS", ["max-age=31536000", "includeSubDomains", "preload"], (MaxAgeSeconds: 31536000, IncludeSubDomains: true, Preload: true)),
            new("max-age only", ["max-age=0"], (MaxAgeSeconds: 0, IncludeSubDomains: false, Preload: false)),
            new("no max-age", ["includeSubDomains"], (MaxAgeSeconds: null, IncludeSubDomains: true, Preload: false)),
            new("preload only", ["preload"], (MaxAgeSeconds: null, IncludeSubDomains: false, Preload: true)),
            new("unknown directive ignored", ["unknown-directive", "max-age=300"], (MaxAgeSeconds: 300, IncludeSubDomains: false, Preload: false))
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("max-age no equals sign", ["max-age"], (MaxAgeSeconds: null, IncludeSubDomains: false, Preload: false)),
            new("max-age negative", ["max-age=-1"], (MaxAgeSeconds: null, IncludeSubDomains: false, Preload: false)),
            new("max-age non-numeric", ["max-age=abc"], (MaxAgeSeconds: null, IncludeSubDomains: false, Preload: false)),
            new("case insensitive includeSubDomains", ["INCLUDESUBDOMAINS"], (MaxAgeSeconds: null, IncludeSubDomains: true, Preload: false)),
            new("case insensitive preload", ["PRELOAD"], (MaxAgeSeconds: null, IncludeSubDomains: false, Preload: true)),
            new("case insensitive max-age", ["MAX-AGE=600"], (MaxAgeSeconds: 600, IncludeSubDomains: false, Preload: false)),
            new("max-age with whitespace value", ["max-age= 100 "], (MaxAgeSeconds: 100, IncludeSubDomains: false, Preload: false))
        ];

        public sealed record Case(string Name, IReadOnlyList<string> Value, (long? MaxAgeSeconds, bool IncludeSubDomains, bool Preload) Expected)
            : ReturnCase<IReadOnlyList<string>, (long? MaxAgeSeconds, bool IncludeSubDomains, bool Preload)>(Name, Value, Expected);
    }

    public static class HstsDirectivesWithExpression
    {
        public static TheoryData<Case> Cases =>
        [
            new("Mutate all", (MaxAgeSeconds: 31536000, IncludeSubDomains: true, Preload: true), (MaxAgeSeconds: 0, IncludeSubDomains: false, Preload: false))
        ];

        public sealed record Case(string Name, (long? MaxAgeSeconds, bool IncludeSubDomains, bool Preload) Value, (long? MaxAgeSeconds, bool IncludeSubDomains, bool Preload) Mutated)
            : ValueCase<(long? MaxAgeSeconds, bool IncludeSubDomains, bool Preload)>(Name, Value);
    }
}
