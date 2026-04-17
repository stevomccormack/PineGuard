using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.HttpRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class HttpRulesTestData
{
    public static class IsHeaderName
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsHeaderName.AllScenarios.ToRuleCases();
    }

    public static class IsHeaderValue
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsHeaderValue.AllScenarios.ToRuleCases();
    }

    public static class IsHttpStatusCode
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsHttpStatusCode.AllScenarios.ToRuleCases();
    }

    public static class IsHttpStatusInformational
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsHttpStatusInformational.AllScenarios.ToRuleCases();
    }

    public static class IsHttpStatusSuccess
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsHttpStatusSuccess.AllScenarios.ToRuleCases();
    }

    public static class IsHttpStatusRedirect
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsHttpStatusRedirect.AllScenarios.ToRuleCases();
    }

    public static class IsHttpStatusClientError
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsHttpStatusClientError.AllScenarios.ToRuleCases();
    }

    public static class IsHttpStatusServerError
    {
        public static TheoryData<RuleCase<int?>> Cases => F.IsHttpStatusServerError.AllScenarios.ToRuleCases();
    }

    public static class HasHeaderValue
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> Cases =>
        [
            new("has trimmed", (F.HasHeaderValue.HeadersWithTrimmedValue, F.HasHeaderValue.HeaderName), new RuleExpected(true)),
            new("no value", (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new RuleExpected(false)),
            new("missing header", (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new RuleExpected(false)),
            new("null headers", (null, F.HasHeaderValue.HeaderName), new RuleExpected(false)),
            new("null name", (F.HasHeaderValue.HeadersWithValue, null), new RuleExpected(false))
        ];
    }

    public static class HasHeaderValueEqualTo
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>> headers, string? name, string? expectedValue)>> Cases =>
        [
            new("matches", (F.HasHeaderValueEqualTo.HeadersWithSpacedValue, "X", "a"), new RuleExpected(true)),
            new("skips whitespace candidates", (F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue, "X", "a"), new RuleExpected(true)),
            new("no match", (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "b"), new RuleExpected(false)),
            new("expected null", (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", null), new RuleExpected(false)),
            new("missing header", (F.HasHeaderValueEqualTo.HeadersWithDifferentKey, "X", "a"), new RuleExpected(false))
        ];
    }

    public static class HasContentType
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>> headers, string[]? allowed)>> Cases =>
        [
            new("matches", (F.HasContentType.JsonHeaders, ["application/json"]), new RuleExpected(true)),
            new("case insensitive", (F.HasContentType.MixedCaseJsonHeaders, ["application/json"]), new RuleExpected(true)),
            new("skips whitespace allowed", (F.HasContentType.JsonHeaders, ["  ", "application/json"]), new RuleExpected(true)),
            new("no match", (F.HasContentType.PlainTextHeaders, ["application/json"]), new RuleExpected(false)),
            new("allowed null", (F.HasContentType.JsonHeaders, null), new RuleExpected(false)),
            new("allowed empty", (F.HasContentType.JsonHeaders, []), new RuleExpected(false)),
            new("headers missing", (F.HasContentType.NonContentTypeHeaders, ["application/json"]), new RuleExpected(false))
        ];
    }

    public static class HasSingleHeaderValue
    {
        public static TheoryData<RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>> headers, string headerName)>> Cases =>
        [
            new("single value", (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new RuleExpected(true)),
            new("multiple values", (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new RuleExpected(false)),
            new("missing header", (F.HasSingleHeaderValue.SingleValueHeaders, "Y"), new RuleExpected(false))
        ];
    }
}
