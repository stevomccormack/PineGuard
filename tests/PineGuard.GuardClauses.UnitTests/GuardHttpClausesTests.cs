using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardHttpClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardHttpClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotHeaderName.ValidCases), MemberType = typeof(TD.NotHeaderName))]
    [MemberData(nameof(TD.NotHeaderName.InvalidCases), MemberType = typeof(TD.NotHeaderName))]
    public void NotHeaderName_BehavesAsExpected(GuardCase<string?> tc)
    {
        var name = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHeaderName(name));
        AssertCustomMessage(tc, () => Guard.Against.NotHeaderName(name, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(name, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHeaderValue.ValidCases), MemberType = typeof(TD.NotHeaderValue))]
    [MemberData(nameof(TD.NotHeaderValue.InvalidCases), MemberType = typeof(TD.NotHeaderValue))]
    public void NotHeaderValue_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHeaderValue(value!));
        AssertCustomMessage(tc, () => Guard.Against.NotHeaderValue(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpStatusCode.ValidCases), MemberType = typeof(TD.NotHttpStatusCode))]
    [MemberData(nameof(TD.NotHttpStatusCode.InvalidCases), MemberType = typeof(TD.NotHttpStatusCode))]
    public void NotHttpStatusCode_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpStatusCode(status));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpStatusCode(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpStatusInformational.ValidCases), MemberType = typeof(TD.NotHttpStatusInformational))]
    [MemberData(nameof(TD.NotHttpStatusInformational.InvalidCases), MemberType = typeof(TD.NotHttpStatusInformational))]
    public void NotHttpStatusInformational_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpStatusInformational(status));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpStatusInformational(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpStatusSuccess.ValidCases), MemberType = typeof(TD.NotHttpStatusSuccess))]
    [MemberData(nameof(TD.NotHttpStatusSuccess.InvalidCases), MemberType = typeof(TD.NotHttpStatusSuccess))]
    public void NotHttpStatusSuccess_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpStatusSuccess(status));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpStatusSuccess(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpStatusRedirect.ValidCases), MemberType = typeof(TD.NotHttpStatusRedirect))]
    [MemberData(nameof(TD.NotHttpStatusRedirect.InvalidCases), MemberType = typeof(TD.NotHttpStatusRedirect))]
    public void NotHttpStatusRedirect_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpStatusRedirect(status));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpStatusRedirect(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpStatusClientError.ValidCases), MemberType = typeof(TD.NotHttpStatusClientError))]
    [MemberData(nameof(TD.NotHttpStatusClientError.InvalidCases), MemberType = typeof(TD.NotHttpStatusClientError))]
    public void NotHttpStatusClientError_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpStatusClientError(status));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpStatusClientError(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHttpStatusServerError.ValidCases), MemberType = typeof(TD.NotHttpStatusServerError))]
    [MemberData(nameof(TD.NotHttpStatusServerError.InvalidCases), MemberType = typeof(TD.NotHttpStatusServerError))]
    public void NotHttpStatusServerError_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHttpStatusServerError(status));
        AssertCustomMessage(tc, () => Guard.Against.NotHttpStatusServerError(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasHeader.ValidCases), MemberType = typeof(TD.NotHasHeader))]
    [MemberData(nameof(TD.NotHasHeader.InvalidCases), MemberType = typeof(TD.NotHasHeader))]
    public void NotHasHeader_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.NotHasHeader(headers, tc.Value.name));
        AssertCustomMessage(tc, () => Guard.Against.NotHasHeader(headers, tc.Value.name, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasHeaderValue.ValidCases), MemberType = typeof(TD.NotHasHeaderValue))]
    [MemberData(nameof(TD.NotHasHeaderValue.InvalidCases), MemberType = typeof(TD.NotHasHeaderValue))]
    public void NotHasHeaderValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.NotHasHeaderValue(headers, tc.Value.name));
        AssertCustomMessage(tc, () => Guard.Against.NotHasHeaderValue(headers, tc.Value.name, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasHeaderValueEqualTo.ValidCases), MemberType = typeof(TD.NotHasHeaderValueEqualTo))]
    [MemberData(nameof(TD.NotHasHeaderValueEqualTo.InvalidCases), MemberType = typeof(TD.NotHasHeaderValueEqualTo))]
    public void NotHasHeaderValueEqualTo_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.NotHasHeaderValueEqualTo(headers, tc.Value.name, tc.Value.expectedValue));
        AssertCustomMessage(tc, () => Guard.Against.NotHasHeaderValueEqualTo(headers, tc.Value.name, tc.Value.expectedValue, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasSingleHeaderValue.ValidCases), MemberType = typeof(TD.NotHasSingleHeaderValue))]
    [MemberData(nameof(TD.NotHasSingleHeaderValue.InvalidCases), MemberType = typeof(TD.NotHasSingleHeaderValue))]
    public void NotHasSingleHeaderValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.NotHasSingleHeaderValue(headers, tc.Value.name));
        AssertCustomMessage(tc, () => Guard.Against.NotHasSingleHeaderValue(headers, tc.Value.name, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasContentType.ValidCases), MemberType = typeof(TD.NotHasContentType))]
    [MemberData(nameof(TD.NotHasContentType.InvalidCases), MemberType = typeof(TD.NotHasContentType))]
    public void NotHasContentType_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? allowed)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.NotHasContentType(headers, tc.Value.allowed));
        AssertCustomMessage(tc, () => Guard.Against.NotHasContentType(headers, tc.Value.allowed, message: CustomMessage));
    }

    // Guard.Against.NotMediaType
    [Theory]
    [MemberData(nameof(TD.NotMediaType.ValidCases), MemberType = typeof(TD.NotMediaType))]
    [MemberData(nameof(TD.NotMediaType.InvalidCases), MemberType = typeof(TD.NotMediaType))]
    public void NotMediaType_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotMediaType(value));
        AssertCustomMessage(tc, () => Guard.Against.NotMediaType(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasHeaderValueOverload.ValidCases), MemberType = typeof(TD.NotHasHeaderValueOverload))]
    [MemberData(nameof(TD.NotHasHeaderValueOverload.InvalidCases), MemberType = typeof(TD.NotHasHeaderValueOverload))]
    public void NotHasHeaderValueOverload_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.NotHasHeaderValue(headers, tc.Value.name, expectedValue: tc.Value.expectedValue));
        AssertCustomMessage(tc, () => Guard.Against.NotHasHeaderValue(headers, tc.Value.name, expectedValue: tc.Value.expectedValue, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HeaderName.ValidCases), MemberType = typeof(TD.HeaderName))]
    [MemberData(nameof(TD.HeaderName.InvalidCases), MemberType = typeof(TD.HeaderName))]
    public void HeaderName_BehavesAsExpected(GuardCase<string?> tc)
    {
        var name = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HeaderName(name));
        AssertCustomMessage(tc, () => Guard.Against.HeaderName(name, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(name, result);
    }

    [Theory]
    [MemberData(nameof(TD.HeaderValue.ValidCases), MemberType = typeof(TD.HeaderValue))]
    [MemberData(nameof(TD.HeaderValue.InvalidCases), MemberType = typeof(TD.HeaderValue))]
    public void HeaderValue_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HeaderValue(value!));
        AssertCustomMessage(tc, () => Guard.Against.HeaderValue(value!, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HttpStatusCode.ValidCases), MemberType = typeof(TD.HttpStatusCode))]
    [MemberData(nameof(TD.HttpStatusCode.InvalidCases), MemberType = typeof(TD.HttpStatusCode))]
    public void HttpStatusCode_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HttpStatusCode(status));
        AssertCustomMessage(tc, () => Guard.Against.HttpStatusCode(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.HttpStatusInformational.ValidCases), MemberType = typeof(TD.HttpStatusInformational))]
    [MemberData(nameof(TD.HttpStatusInformational.InvalidCases), MemberType = typeof(TD.HttpStatusInformational))]
    public void HttpStatusInformational_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HttpStatusInformational(status));
        AssertCustomMessage(tc, () => Guard.Against.HttpStatusInformational(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.HttpStatusSuccess.ValidCases), MemberType = typeof(TD.HttpStatusSuccess))]
    [MemberData(nameof(TD.HttpStatusSuccess.InvalidCases), MemberType = typeof(TD.HttpStatusSuccess))]
    public void HttpStatusSuccess_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HttpStatusSuccess(status));
        AssertCustomMessage(tc, () => Guard.Against.HttpStatusSuccess(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.HttpStatusRedirect.ValidCases), MemberType = typeof(TD.HttpStatusRedirect))]
    [MemberData(nameof(TD.HttpStatusRedirect.InvalidCases), MemberType = typeof(TD.HttpStatusRedirect))]
    public void HttpStatusRedirect_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HttpStatusRedirect(status));
        AssertCustomMessage(tc, () => Guard.Against.HttpStatusRedirect(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.HttpStatusClientError.ValidCases), MemberType = typeof(TD.HttpStatusClientError))]
    [MemberData(nameof(TD.HttpStatusClientError.InvalidCases), MemberType = typeof(TD.HttpStatusClientError))]
    public void HttpStatusClientError_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HttpStatusClientError(status));
        AssertCustomMessage(tc, () => Guard.Against.HttpStatusClientError(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.HttpStatusServerError.ValidCases), MemberType = typeof(TD.HttpStatusServerError))]
    [MemberData(nameof(TD.HttpStatusServerError.InvalidCases), MemberType = typeof(TD.HttpStatusServerError))]
    public void HttpStatusServerError_BehavesAsExpected(GuardCase<int> tc)
    {
        var status = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HttpStatusServerError(status));
        AssertCustomMessage(tc, () => Guard.Against.HttpStatusServerError(status, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(status, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasHeader.ValidCases), MemberType = typeof(TD.HasHeader))]
    [MemberData(nameof(TD.HasHeader.InvalidCases), MemberType = typeof(TD.HasHeader))]
    public void HasHeader_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.HasHeader(headers, tc.Value.name));
        AssertCustomMessage(tc, () => Guard.Against.HasHeader(headers, tc.Value.name, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasHeaderValue.ValidCases), MemberType = typeof(TD.HasHeaderValue))]
    [MemberData(nameof(TD.HasHeaderValue.InvalidCases), MemberType = typeof(TD.HasHeaderValue))]
    public void HasHeaderValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.HasHeaderValue(headers, tc.Value.name));
        AssertCustomMessage(tc, () => Guard.Against.HasHeaderValue(headers, tc.Value.name, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasHeaderValueEqualTo.ValidCases), MemberType = typeof(TD.HasHeaderValueEqualTo))]
    [MemberData(nameof(TD.HasHeaderValueEqualTo.InvalidCases), MemberType = typeof(TD.HasHeaderValueEqualTo))]
    public void HasHeaderValueEqualTo_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name, string? expectedValue)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.HasHeaderValueEqualTo(headers, tc.Value.name, tc.Value.expectedValue));
        AssertCustomMessage(tc, () => Guard.Against.HasHeaderValueEqualTo(headers, tc.Value.name, tc.Value.expectedValue, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasSingleHeaderValue.ValidCases), MemberType = typeof(TD.HasSingleHeaderValue))]
    [MemberData(nameof(TD.HasSingleHeaderValue.InvalidCases), MemberType = typeof(TD.HasSingleHeaderValue))]
    public void HasSingleHeaderValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.HasSingleHeaderValue(headers, tc.Value.name));
        AssertCustomMessage(tc, () => Guard.Against.HasSingleHeaderValue(headers, tc.Value.name, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasContentType.ValidCases), MemberType = typeof(TD.HasContentType))]
    [MemberData(nameof(TD.HasContentType.InvalidCases), MemberType = typeof(TD.HasContentType))]
    public void HasContentType_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[]? allowed)> tc)
    {
        var headers = tc.Value.headers;
        AssertResult(tc, () => Guard.Against.HasContentType(headers, tc.Value.allowed));
        AssertCustomMessage(tc, () => Guard.Against.HasContentType(headers, tc.Value.allowed, message: CustomMessage));
    }
}
