using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.HttpRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentHttpExtensionsTestData
{
    public static class HeaderName
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHeaderName.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHeaderName.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid HTTP header name.")
        });
    }

    public static class NotHeaderName
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHeaderName.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHeaderName.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid HTTP header name."),
            _ => new FluentExpected(true)
        });
    }

    public static class HeaderValue
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHeaderValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHeaderValue.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid HTTP header value.")
        });
    }

    public static class NotHeaderValue
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsHeaderValue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHeaderValue.Null) => new FluentExpected(false, "Value must not be null."),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid HTTP header value."),
            _ => new FluentExpected(true)
        });
    }

    public static class HttpStatusCode
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusCode.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusCode.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid HTTP status code.")
        });
    }

    public static class NotHttpStatusCode
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusCode.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusCode.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a valid HTTP status code."),
            _ => new FluentExpected(true)
        });
    }

    public static class HttpStatusInformational
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusInformational.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusInformational.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be an informational HTTP status code.")
        });
    }

    public static class NotHttpStatusInformational
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusInformational.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusInformational.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be an informational HTTP status code."),
            _ => new FluentExpected(true)
        });
    }

    public static class HttpStatusSuccess
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusSuccess.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusSuccess.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a successful HTTP status code.")
        });
    }

    public static class NotHttpStatusSuccess
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusSuccess.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusSuccess.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a successful HTTP status code."),
            _ => new FluentExpected(true)
        });
    }

    public static class HttpStatusRedirect
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusRedirect.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusRedirect.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a redirect HTTP status code.")
        });
    }

    public static class NotHttpStatusRedirect
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusRedirect.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusRedirect.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a redirect HTTP status code."),
            _ => new FluentExpected(true)
        });
    }

    public static class HttpStatusClientError
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusClientError.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusClientError.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a client error HTTP status code.")
        });
    }

    public static class NotHttpStatusClientError
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusClientError.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusClientError.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a client error HTTP status code."),
            _ => new FluentExpected(true)
        });
    }

    public static class HttpStatusServerError
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusServerError.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusServerError.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a server error HTTP status code.")
        });
    }

    public static class NotHttpStatusServerError
    {
        public static TheoryData<FluentCase<int?>> Cases => F.IsHttpStatusServerError.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsHttpStatusServerError.Null) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(false, "Value must not be a server error HTTP status code."),
            _ => new FluentExpected(true)
        });
    }

    public static class HasHeader
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)>> Cases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new FluentExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new FluentExpected(false, "Value must contain the specified header."))
        ];
    }

    public static class NotHasHeader
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)>> Cases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new FluentExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new FluentExpected(false, "Value must not contain the specified header."))
        ];
    }

    public static class HasHeaderValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)>> Cases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new FluentExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithTrimmedValue), (F.HasHeaderValue.HeadersWithTrimmedValue, F.HasHeaderValue.HeaderName), new FluentExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithWhitespaceValue), (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new FluentExpected(false, "Value must contain a value for the specified header.")),
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new FluentExpected(false, "Value must contain a value for the specified header."))
        ];
    }

    public static class NotHasHeaderValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)>> Cases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithWhitespaceValue), (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new FluentExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new FluentExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new FluentExpected(false, "Value must not contain a value for the specified header."))
        ];
    }

    public static class HasHeaderValueEqualTo
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name, string expectedValue)>> Cases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithValueA), (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new FluentExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithSpacedValue), (F.HasHeaderValueEqualTo.HeadersWithSpacedValue, "X", "a"), new FluentExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithDifferentKey), (F.HasHeaderValueEqualTo.HeadersWithDifferentKey, "X", "a"), new FluentExpected(false, "Value must contain the specified header value."))
        ];
    }

    public static class NotHasHeaderValueEqualTo
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name, string expectedValue)>> Cases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithDifferentKey), (F.HasHeaderValueEqualTo.HeadersWithDifferentKey, "X", "a"), new FluentExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithValueA), (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new FluentExpected(false, "Value must not contain the specified header value."))
        ];
    }

    public static class HasSingleHeaderValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)>> Cases =>
        [
            new(nameof(F.HasSingleHeaderValue.SingleValueHeaders), (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new FluentExpected(true)),
            new(nameof(F.HasSingleHeaderValue.MultipleValueHeaders), (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new FluentExpected(false, "Value must contain a single value for the specified header."))
        ];
    }

    public static class NotHasSingleHeaderValue
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string name)>> Cases =>
        [
            new(nameof(F.HasSingleHeaderValue.MultipleValueHeaders), (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new FluentExpected(true)),
            new(nameof(F.HasSingleHeaderValue.SingleValueHeaders), (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new FluentExpected(false, "Value must not contain a single value for the specified header."))
        ];
    }

    public static class HasContentType
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] allowed)>> Cases =>
        [
            new(nameof(F.HasContentType.JsonHeaders), (F.HasContentType.JsonHeaders, ["application/json"]), new FluentExpected(true)),
            new(nameof(F.HasContentType.MixedCaseJsonHeaders), (F.HasContentType.MixedCaseJsonHeaders, ["application/json"]), new FluentExpected(true)),
            new(nameof(F.HasContentType.PlainTextHeaders), (F.HasContentType.PlainTextHeaders, ["application/json"]), new FluentExpected(false, "Value must contain an allowed Content-Type.")),
            new(nameof(F.HasContentType.NonContentTypeHeaders), (F.HasContentType.NonContentTypeHeaders, ["application/json"]), new FluentExpected(false, "Value must contain an allowed Content-Type."))
        ];
    }

    public static class NotHasContentType
    {
        public static TheoryData<FluentCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] allowed)>> Cases =>
        [
            new(nameof(F.HasContentType.PlainTextHeaders), (F.HasContentType.PlainTextHeaders, ["application/json"]), new FluentExpected(true)),
            new(nameof(F.HasContentType.NonContentTypeHeaders), (F.HasContentType.NonContentTypeHeaders, ["application/json"]), new FluentExpected(true)),
            new(nameof(F.HasContentType.JsonHeaders), (F.HasContentType.JsonHeaders, ["application/json"]), new FluentExpected(false, "Value must not contain an allowed Content-Type."))
        ];
    }
}
