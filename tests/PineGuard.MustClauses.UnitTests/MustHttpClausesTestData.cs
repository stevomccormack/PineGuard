using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.HttpRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustHttpClausesTestData
{
    public static class IsHeaderName
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHeaderName.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHeaderName.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.HeaderName.Malformed));
    }

    public static class IsHeaderValue
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHeaderValue.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsHeaderValue.InvalidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.HeaderValue.Malformed));
    }

    public static class IsHttpStatusCode
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusCode.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusCode.InvalidScenarios.Except(nameof(F.IsHttpStatusCode.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.OutOfRange));
    }

    public static class IsHttpStatusInformational
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusInformational.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusInformational.InvalidScenarios.Except(nameof(F.IsHttpStatusInformational.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.NotInformational));
    }

    public static class IsHttpStatusSuccess
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusSuccess.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusSuccess.InvalidScenarios.Except(nameof(F.IsHttpStatusSuccess.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.NotSuccess));
    }

    public static class IsHttpStatusRedirect
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusRedirect.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusRedirect.InvalidScenarios.Except(nameof(F.IsHttpStatusRedirect.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.NotRedirect));
    }

    public static class IsHttpStatusClientError
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusClientError.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusClientError.InvalidScenarios.Except(nameof(F.IsHttpStatusClientError.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.NotClientError));
    }

    public static class IsHttpStatusServerError
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusServerError.ValidScenarios.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusServerError.InvalidScenarios.Except(nameof(F.IsHttpStatusServerError.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.NotServerError));
    }

    public static class HasHeader
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> ValidCases =>
        [
            new("exists", (F.HasContentType.JsonHeaders, "Content-Type"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> InvalidCases =>
        [
            new("missing", (F.HasContentType.JsonHeaders, "Missing"), new MustExpected(false, Code: MustCodes.Http.Header.Missing))
        ];
    }

    public static class HasHeaderValue
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> ValidCases =>
        [
            new("has-value", (F.HasHeaderValue.HeadersWithTrimmedValue, F.HasHeaderValue.HeaderName), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> InvalidCases =>
        [
            new("no-value", (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new MustExpected(false, Code: MustCodes.Http.HeaderValue.Missing))
        ];
    }

    public static class HasHeaderValueEqualTo
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key, string val)>> ValidCases =>
        [
            new("match", (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key, string val)>> InvalidCases =>
        [
            new("mismatch", (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "b"), new MustExpected(false, Code: MustCodes.Http.HeaderValue.Mismatch))
        ];
    }

    public static class HasSingleHeaderValue
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> ValidCases =>
        [
            new("single", (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> InvalidCases =>
        [
            new("multiple", (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new MustExpected(false, Code: MustCodes.Http.HeaderValue.NotSingle))
        ];
    }

    public static class HasContentType
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] types)>> ValidCases =>
        [
            new("match", (F.HasContentType.JsonHeaders, ["application/json"]), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] types)>> InvalidCases =>
        [
            new("no-match", (F.HasContentType.PlainTextHeaders, ["application/json"]), new MustExpected(false, Code: MustCodes.Http.ContentType.NotAllowed))
        ];
    }

    public static class NotIsHeaderName
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHeaderName.InvalidScenarios.Except(nameof(F.IsHeaderName.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases
        {
            get
            {
                var data = F.IsHeaderName.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.HeaderName.WellFormed));
                data.Add(new MustCase<string?>(nameof(F.IsHeaderName.Null), F.IsHeaderName.Null, new MustExpected(false, "value must not be null.", "value")));
                return data;
            }
        }
    }

    public static class NotIsHeaderValue
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsHeaderValue.InvalidScenarios.Except(nameof(F.IsHeaderValue.Null)).ToMustCases(_ => new MustExpected(true));
        public static TheoryData<MustCase<string?>> InvalidCases
        {
            get
            {
                var data = F.IsHeaderValue.ValidScenarios.ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.HeaderValue.WellFormed));
                data.Add(new MustCase<string?>(nameof(F.IsHeaderValue.Null), F.IsHeaderValue.Null, new MustExpected(false, "value must not be null.", "value")));
                return data;
            }
        }
    }

    public static class NotIsHttpStatusCode
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusCode.InvalidScenarios.Except(nameof(F.IsHttpStatusCode.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusCode.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.InRange));
    }

    public static class NotIsHttpStatusInformational
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusInformational.InvalidScenarios.Except(nameof(F.IsHttpStatusInformational.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusInformational.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.Informational));
    }

    public static class NotIsHttpStatusSuccess
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusSuccess.InvalidScenarios.Except(nameof(F.IsHttpStatusSuccess.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusSuccess.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.Success));
    }

    public static class NotIsHttpStatusRedirect
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusRedirect.InvalidScenarios.Except(nameof(F.IsHttpStatusRedirect.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusRedirect.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.Redirect));
    }

    public static class NotIsHttpStatusClientError
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusClientError.InvalidScenarios.Except(nameof(F.IsHttpStatusClientError.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusClientError.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.ClientError));
    }

    public static class NotIsHttpStatusServerError
    {
        public static TheoryData<MustCase<int>> ValidCases =>
            F.IsHttpStatusServerError.InvalidScenarios.Except(nameof(F.IsHttpStatusServerError.Null)).Project(v => v!.Value).ToMustCases(_ => new MustExpected(true));

        public static TheoryData<MustCase<int>> InvalidCases =>
            F.IsHttpStatusServerError.ValidScenarios.Project(v => v!.Value).ToMustCases(_ => new MustExpected(false, Code: MustCodes.Http.Status.ServerError));
    }

    public static class NotHasHeader
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> ValidCases =>
        [
            new("missing", (F.HasContentType.JsonHeaders, "Missing"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> InvalidCases =>
        [
            new("exists", (F.HasContentType.JsonHeaders, "Content-Type"), new MustExpected(false, Code: MustCodes.Http.Header.Present))
        ];
    }

    public static class NotHasHeaderValue
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> ValidCases =>
        [
            new("no-value", (F.HasHeaderValue.HeadersWithWhitespaceValue, F.HasHeaderValue.HeaderName), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> InvalidCases =>
        [
            new("has-value", (F.HasHeaderValue.HeadersWithTrimmedValue, F.HasHeaderValue.HeaderName), new MustExpected(false, Code: MustCodes.Http.HeaderValue.Present))
        ];
    }

    public static class NotHasHeaderValueEqualTo
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key, string val)>> ValidCases =>
        [
            new("mismatch", (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "b"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key, string val)>> InvalidCases =>
        [
            new("match", (F.HasHeaderValueEqualTo.HeadersWithValueA, "X", "a"), new MustExpected(false, Code: MustCodes.Http.HeaderValue.Match))
        ];
    }

    public static class NotHasSingleHeaderValue
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> ValidCases =>
        [
            new("multiple", (F.HasSingleHeaderValue.MultipleValueHeaders, "X"), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)>> InvalidCases =>
        [
            new("single", (F.HasSingleHeaderValue.SingleValueHeaders, "X"), new MustExpected(false, Code: MustCodes.Http.HeaderValue.Single))
        ];
    }

    public static class NotHasContentType
    {
        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] types)>> ValidCases =>
        [
            new("no-match", (F.HasContentType.PlainTextHeaders, ["application/json"]), new MustExpected(true))
        ];

        public static TheoryData<MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] types)>> InvalidCases =>
        [
            new("match", (F.HasContentType.JsonHeaders, ["application/json"]), new MustExpected(false, Code: MustCodes.Http.ContentType.Allowed))
        ];
    }
}
