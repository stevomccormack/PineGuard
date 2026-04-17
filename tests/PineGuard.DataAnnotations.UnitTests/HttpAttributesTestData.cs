using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.HttpRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class HttpAttributesTestData
{
    public sealed record ValidCase(string Name, object? Value, bool Expected) : ReturnCase<object?, bool>(Name, Value, Expected);

    private static TheoryData<ValidCase> CommonEdgeCases() =>
    [
        new("null", null, true)
    ];

    public static class HttpHeaderName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsHeaderName.Simple),        F.IsHeaderName.Simple,        true),
            new(nameof(F.IsHeaderName.AllowedSymbols), F.IsHeaderName.AllowedSymbols, true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsHeaderName.InvalidSpace),   F.IsHeaderName.InvalidSpace,   false),
            new(nameof(F.IsHeaderName.InvalidNewline), F.IsHeaderName.InvalidNewline, false)
        ];
    }

    public static class HttpHeaderValue
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsHeaderValue.Simple), F.IsHeaderValue.Simple, true),
            new(nameof(F.IsHeaderValue.Quoted), F.IsHeaderValue.Quoted, true)
        ];

        public static TheoryData<ValidCase> EdgeCases => CommonEdgeCases();

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsHeaderValue.TrailingCrLf), F.IsHeaderValue.TrailingCrLf, false)
        ];
    }

    public static class HttpStatusCode
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsHttpStatusCode.LowerBound), F.IsHttpStatusCode.LowerBound, true),
            new(nameof(F.IsHttpStatusCode.Ok),         F.IsHttpStatusCode.Ok,         true),
            new(nameof(F.IsHttpStatusCode.UpperBound), F.IsHttpStatusCode.UpperBound, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.IsHttpStatusCode.Null), F.IsHttpStatusCode.Null, true)
        ];

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsHttpStatusCode.BelowRange), F.IsHttpStatusCode.BelowRange, false),
            new(nameof(F.IsHttpStatusCode.AboveRange), F.IsHttpStatusCode.AboveRange, false)
        ];
    }

    public static class HttpStatusSuccess
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.IsHttpStatusSuccess.InRange),       F.IsHttpStatusSuccess.InRange,       true),
            new(nameof(F.IsHttpStatusSuccess.Created),       F.IsHttpStatusSuccess.Created,       true),
            new(nameof(F.IsHttpStatusSuccess.PartialContent), F.IsHttpStatusSuccess.PartialContent, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.IsHttpStatusSuccess.Null), F.IsHttpStatusSuccess.Null, true)
        ];

        public static TheoryData<ValidCase> InvalidCases =>
        [
            new(nameof(F.IsHttpStatusInformational.InRange), F.IsHttpStatusInformational.InRange, false),
            new(nameof(F.IsHttpStatusRedirect.InRange),      F.IsHttpStatusRedirect.InRange,      false),
            new(nameof(F.IsHttpStatusClientError.InRange),   F.IsHttpStatusClientError.InRange,   false),
            new(nameof(F.IsHttpStatusServerError.InRange),   F.IsHttpStatusServerError.InRange,   false)
        ];
    }
}
