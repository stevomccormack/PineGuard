using PineGuard.Codes;
using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.HttpRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardHttpClausesTestData
{
    public static class NotHeaderName
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHeaderName.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsHeaderName.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "name") : new GuardExpected(false, typeof(ArgumentException), "name"));
    }

    public static class NotHeaderValue
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHeaderValue.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsHeaderValue.InvalidScenarios.ToGuardCases(s => s.IsNull ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    public static class NotHttpStatusCode
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusCode.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusCode.InvalidScenarios.Except(nameof(F.IsHttpStatusCode.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class NotHttpStatusInformational
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusInformational.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusInformational.InvalidScenarios.Except(nameof(F.IsHttpStatusInformational.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class NotHttpStatusSuccess
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusSuccess.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusSuccess.InvalidScenarios.Except(nameof(F.IsHttpStatusSuccess.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class NotHttpStatusRedirect
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusRedirect.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusRedirect.InvalidScenarios.Except(nameof(F.IsHttpStatusRedirect.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class NotHttpStatusClientError
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusClientError.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusClientError.InvalidScenarios.Except(nameof(F.IsHttpStatusClientError.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class NotHttpStatusServerError
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusServerError.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusServerError.InvalidScenarios.Except(nameof(F.IsHttpStatusServerError.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class NotHasHeader
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new GuardExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithTrimmedValue), (F.HasHeaderValue.HeadersWithTrimmedValue, F.HasHeaderValue.HeaderName), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class NotHasHeaderValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasHeaderValue.HeadersWithWhitespaceValue), (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class NotHasHeaderValueEqualTo
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithValueA), (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new GuardExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithSpacedValue), (F.HasHeaderValueEqualTo.HeadersWithSpacedValue, "X", "a"), new GuardExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue), (F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue, "X", "a"), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithDifferentKey), (F.HasHeaderValueEqualTo.HeadersWithDifferentKey, "X", "a"), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class NotHasSingleHeaderValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> ValidCases =>
        [
            new(nameof(F.HasSingleHeaderValue.SingleValueHeaders), (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> InvalidCases =>
        [
            new(nameof(F.HasSingleHeaderValue.MultipleValueHeaders), (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class NotHasContentType
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? allowed)>> ValidCases =>
        [
            new(nameof(F.HasContentType.JsonHeaders), (F.HasContentType.JsonHeaders, ["application/json"]), new GuardExpected(true)),
            new(nameof(F.HasContentType.MixedCaseJsonHeaders), (F.HasContentType.MixedCaseJsonHeaders, ["application/json"]), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? allowed)>> InvalidCases =>
        [
            new(nameof(F.HasContentType.PlainTextHeaders), (F.HasContentType.PlainTextHeaders, ["application/json"]), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasContentType.NonContentTypeHeaders), (F.HasContentType.NonContentTypeHeaders, ["application/json"]), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    // Guard.Against.NotMediaType — throws when value is NOT a valid media type (delegates to Must.Be.MediaType)
    public static class NotMediaType
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsMediaType.ValidScenarios.ToGuardCases();

        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsMediaType.InvalidScenarios.ToGuardCases(s => new GuardExpected(false, s.IsNull ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: MustCodes.Http.MediaType.Invalid));
    }

    public static class NotHasHeaderValueOverload
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithValueA), (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new GuardExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithSpacedValue), (F.HasHeaderValueEqualTo.HeadersWithSpacedValue, "X", "a"), new GuardExpected(true)),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue), (F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue, "X", "a"), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithDifferentKey), (F.HasHeaderValueEqualTo.HeadersWithDifferentKey, "X", "a"), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class HeaderName
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHeaderName.InvalidScenarios.Except(nameof(F.IsHeaderName.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            .. F.IsHeaderName.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "name")),
            new(nameof(F.IsHeaderName.Null), null, new GuardExpected(false, typeof(ArgumentNullException), "name"))
        ];
    }

    public static class HeaderValue
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsHeaderValue.InvalidScenarios.Except(nameof(F.IsHeaderValue.Null)).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases =>
        [
            .. F.IsHeaderValue.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value")),
            new(nameof(F.IsHeaderValue.Null), null, new GuardExpected(false, typeof(ArgumentNullException), "value"))
        ];
    }

    public static class HttpStatusCode
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusCode.InvalidScenarios.Except(nameof(F.IsHttpStatusCode.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusCode.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class HttpStatusInformational
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusInformational.InvalidScenarios.Except(nameof(F.IsHttpStatusInformational.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusInformational.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class HttpStatusSuccess
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusSuccess.InvalidScenarios.Except(nameof(F.IsHttpStatusSuccess.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusSuccess.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class HttpStatusRedirect
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusRedirect.InvalidScenarios.Except(nameof(F.IsHttpStatusRedirect.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusRedirect.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class HttpStatusClientError
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusClientError.InvalidScenarios.Except(nameof(F.IsHttpStatusClientError.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusClientError.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class HttpStatusServerError
    {
        public static TheoryData<GuardCase<int>> ValidCases => F.IsHttpStatusServerError.InvalidScenarios.Except(nameof(F.IsHttpStatusServerError.Null)).Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<int>> InvalidCases => F.IsHttpStatusServerError.ValidScenarios.Project(v => v!.Value).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "status"));
    }

    public static class HasHeader
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasHeaderValue.HeadersWithTrimmedValue), (F.HasHeaderValue.HeadersWithTrimmedValue, F.HasHeaderValue.HeaderName), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class HasHeaderValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithDifferentKey), (F.HasHeaderValue.HeadersWithDifferentKey, F.HasHeaderValue.HeaderName), new GuardExpected(true)),
            new(nameof(F.HasHeaderValue.HeadersWithWhitespaceValue), (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValue.HeadersWithValue), (F.HasHeaderValue.HeadersWithValue, F.HasHeaderValue.HeaderName), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class HasHeaderValueEqualTo
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)>> ValidCases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithDifferentKey), (F.HasHeaderValueEqualTo.HeadersWithDifferentKey, "X", "a"), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)>> InvalidCases =>
        [
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithValueA), (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithSpacedValue), (F.HasHeaderValueEqualTo.HeadersWithSpacedValue, "X", "a"), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue), (F.HasHeaderValueEqualTo.HeadersWithWhitespaceAndValue, "X", "a"), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class HasSingleHeaderValue
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> ValidCases =>
        [
            new(nameof(F.HasSingleHeaderValue.MultipleValueHeaders), (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)>> InvalidCases =>
        [
            new(nameof(F.HasSingleHeaderValue.SingleValueHeaders), (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }

    public static class HasContentType
    {
        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? allowed)>> ValidCases =>
        [
            new(nameof(F.HasContentType.PlainTextHeaders), (F.HasContentType.PlainTextHeaders, ["application/json"]), new GuardExpected(true)),
            new(nameof(F.HasContentType.NonContentTypeHeaders), (F.HasContentType.NonContentTypeHeaders, ["application/json"]), new GuardExpected(true))
        ];

        public static TheoryData<GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? allowed)>> InvalidCases =>
        [
            new(nameof(F.HasContentType.JsonHeaders), (F.HasContentType.JsonHeaders, ["application/json"]), new GuardExpected(false, typeof(ArgumentException), "headers")),
            new(nameof(F.HasContentType.MixedCaseJsonHeaders), (F.HasContentType.MixedCaseJsonHeaders, ["application/json"]), new GuardExpected(false, typeof(ArgumentException), "headers"))
        ];
    }
}
