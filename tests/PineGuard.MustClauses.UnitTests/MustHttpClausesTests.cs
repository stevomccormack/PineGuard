using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustHttpClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHeaderName.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHeaderName))]
    [MemberData(nameof(MustHttpClausesTestData.IsHeaderName.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHeaderName))]
    public void IsHeaderName_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.HeaderName(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHeaderValue.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHeaderValue))]
    [MemberData(nameof(MustHttpClausesTestData.IsHeaderValue.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHeaderValue))]
    public void IsHeaderValue_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.HeaderValue(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusCode.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusCode))]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusCode.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusCode))]
    public void IsHttpStatusCode_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.HttpStatusCode(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusInformational.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusInformational))]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusInformational.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusInformational))]
    public void IsHttpStatusInformational_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.HttpStatusInformational(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusSuccess.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusSuccess))]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusSuccess.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusSuccess))]
    public void IsHttpStatusSuccess_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.HttpStatusSuccess(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusRedirect.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusRedirect))]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusRedirect.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusRedirect))]
    public void IsHttpStatusRedirect_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.HttpStatusRedirect(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusClientError.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusClientError))]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusClientError.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusClientError))]
    public void IsHttpStatusClientError_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.HttpStatusClientError(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusServerError.ValidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusServerError))]
    [MemberData(nameof(MustHttpClausesTestData.IsHttpStatusServerError.InvalidCases), MemberType = typeof(MustHttpClausesTestData.IsHttpStatusServerError))]
    public void IsHttpStatusServerError_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.HttpStatusServerError(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.HasHeader.ValidCases), MemberType = typeof(MustHttpClausesTestData.HasHeader))]
    [MemberData(nameof(MustHttpClausesTestData.HasHeader.InvalidCases), MemberType = typeof(MustHttpClausesTestData.HasHeader))]
    public void HasHeader_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)> tc)
    {
        // Arrange
        var (headers, key) = tc.Value;

        // Act
        var result = Must.Be.HasHeader(headers, key);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.HasHeaderValue.ValidCases), MemberType = typeof(MustHttpClausesTestData.HasHeaderValue))]
    [MemberData(nameof(MustHttpClausesTestData.HasHeaderValue.InvalidCases), MemberType = typeof(MustHttpClausesTestData.HasHeaderValue))]
    public void HasHeaderValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)> tc)
    {
        // Arrange
        var (headers, key) = tc.Value;

        // Act
        var result = Must.Be.HasHeaderValue(headers, key);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.HasHeaderValueEqualTo.ValidCases), MemberType = typeof(MustHttpClausesTestData.HasHeaderValueEqualTo))]
    [MemberData(nameof(MustHttpClausesTestData.HasHeaderValueEqualTo.InvalidCases), MemberType = typeof(MustHttpClausesTestData.HasHeaderValueEqualTo))]
    public void HasHeaderValueEqualTo_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key, string val)> tc)
    {
        // Arrange
        var (headers, key, val) = tc.Value;

        // Act
        var result = Must.Be.HasHeaderValueEqualTo(headers, key, val);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.HasSingleHeaderValue.ValidCases), MemberType = typeof(MustHttpClausesTestData.HasSingleHeaderValue))]
    [MemberData(nameof(MustHttpClausesTestData.HasSingleHeaderValue.InvalidCases), MemberType = typeof(MustHttpClausesTestData.HasSingleHeaderValue))]
    public void HasSingleHeaderValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)> tc)
    {
        // Arrange
        var (headers, key) = tc.Value;

        // Act
        var result = Must.Be.HasSingleHeaderValue(headers, key);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.HasContentType.ValidCases), MemberType = typeof(MustHttpClausesTestData.HasContentType))]
    [MemberData(nameof(MustHttpClausesTestData.HasContentType.InvalidCases), MemberType = typeof(MustHttpClausesTestData.HasContentType))]
    public void HasContentType_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] types)> tc)
    {
        // Arrange
        var (headers, types) = tc.Value;

        // Act
        var result = Must.Be.HasContentType(headers, types);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHeaderName.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHeaderName))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHeaderName.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHeaderName))]
    public void NotIsHeaderName_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotHeaderName(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHeaderValue.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHeaderValue))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHeaderValue.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHeaderValue))]
    public void NotIsHeaderValue_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.NotHeaderValue(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusCode.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusCode))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusCode.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusCode))]
    public void NotIsHttpStatusCode_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotHttpStatusCode(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusInformational.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusInformational))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusInformational.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusInformational))]
    public void NotIsHttpStatusInformational_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotHttpStatusInformational(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusSuccess.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusSuccess))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusSuccess.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusSuccess))]
    public void NotIsHttpStatusSuccess_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotHttpStatusSuccess(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusRedirect.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusRedirect))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusRedirect.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusRedirect))]
    public void NotIsHttpStatusRedirect_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotHttpStatusRedirect(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusClientError.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusClientError))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusClientError.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusClientError))]
    public void NotIsHttpStatusClientError_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotHttpStatusClientError(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusServerError.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusServerError))]
    [MemberData(nameof(MustHttpClausesTestData.NotIsHttpStatusServerError.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotIsHttpStatusServerError))]
    public void NotIsHttpStatusServerError_BehavesAsExpected(MustCase<int> tc)
    {
        var result = Must.Be.NotHttpStatusServerError(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotHasHeader.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotHasHeader))]
    [MemberData(nameof(MustHttpClausesTestData.NotHasHeader.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotHasHeader))]
    public void NotHasHeader_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)> tc)
    {
        var (headers, key) = tc.Value;
        var result = Must.Be.NotHasHeader(headers, key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotHasHeaderValue.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotHasHeaderValue))]
    [MemberData(nameof(MustHttpClausesTestData.NotHasHeaderValue.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotHasHeaderValue))]
    public void NotHasHeaderValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)> tc)
    {
        var (headers, key) = tc.Value;
        var result = Must.Be.NotHasHeaderValue(headers, key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotHasHeaderValueEqualTo.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotHasHeaderValueEqualTo))]
    [MemberData(nameof(MustHttpClausesTestData.NotHasHeaderValueEqualTo.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotHasHeaderValueEqualTo))]
    public void NotHasHeaderValueEqualTo_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key, string val)> tc)
    {
        var (headers, key, val) = tc.Value;
        var result = Must.Be.NotHasHeaderValueEqualTo(headers, key, val);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotHasSingleHeaderValue.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotHasSingleHeaderValue))]
    [MemberData(nameof(MustHttpClausesTestData.NotHasSingleHeaderValue.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotHasSingleHeaderValue))]
    public void NotHasSingleHeaderValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string key)> tc)
    {
        var (headers, key) = tc.Value;
        var result = Must.Be.NotHasSingleHeaderValue(headers, key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustHttpClausesTestData.NotHasContentType.ValidCases), MemberType = typeof(MustHttpClausesTestData.NotHasContentType))]
    [MemberData(nameof(MustHttpClausesTestData.NotHasContentType.InvalidCases), MemberType = typeof(MustHttpClausesTestData.NotHasContentType))]
    public void NotHasContentType_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string[] types)> tc)
    {
        var (headers, types) = tc.Value;
        var result = Must.Be.NotHasContentType(headers, types);
        AssertResult(tc, result);
    }
}
