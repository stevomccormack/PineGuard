using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class HttpRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHeaderName.Cases), MemberType = typeof(HttpRulesTestData.IsHeaderName))]
    public void IsHeaderName_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = HttpRules.IsHeaderName(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHeaderValue.Cases), MemberType = typeof(HttpRulesTestData.IsHeaderValue))]
    public void IsHeaderValue_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = HttpRules.IsHeaderValue(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHttpStatusCode.Cases), MemberType = typeof(HttpRulesTestData.IsHttpStatusCode))]
    public void IsHttpStatusCode_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = HttpRules.IsHttpStatusCode(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHttpStatusInformational.Cases), MemberType = typeof(HttpRulesTestData.IsHttpStatusInformational))]
    public void IsHttpStatusInformational_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = HttpRules.IsHttpStatusInformational(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHttpStatusSuccess.Cases), MemberType = typeof(HttpRulesTestData.IsHttpStatusSuccess))]
    public void IsHttpStatusSuccess_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = HttpRules.IsHttpStatusSuccess(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHttpStatusRedirect.Cases), MemberType = typeof(HttpRulesTestData.IsHttpStatusRedirect))]
    public void IsHttpStatusRedirect_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = HttpRules.IsHttpStatusRedirect(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHttpStatusClientError.Cases), MemberType = typeof(HttpRulesTestData.IsHttpStatusClientError))]
    public void IsHttpStatusClientError_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = HttpRules.IsHttpStatusClientError(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.IsHttpStatusServerError.Cases), MemberType = typeof(HttpRulesTestData.IsHttpStatusServerError))]
    public void IsHttpStatusServerError_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = HttpRules.IsHttpStatusServerError(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.HasHeaderValue.Cases), MemberType = typeof(HttpRulesTestData.HasHeaderValue))]
    public void HasHeaderValue_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>>? headers, string? name)> tc)
    {
        // Arrange
        var (headers, name) = tc.Value;

        // Act
        var result = HttpRules.HasHeaderValue(headers, name);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.HasHeaderValueEqualTo.Cases), MemberType = typeof(HttpRulesTestData.HasHeaderValueEqualTo))]
    public void HasHeaderValueEqualTo_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>> headers, string? name, string? expectedValue)> tc)
    {
        // Arrange
        var (headers, name, expectedValue) = tc.Value;

        // Act
        var result = HttpRules.HasHeaderValue(headers, name, expectedValue);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.HasContentType.Cases), MemberType = typeof(HttpRulesTestData.HasContentType))]
    public void HasContentType_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>> headers, string[]? allowed)> tc)
    {
        // Arrange
        var (headers, allowed) = tc.Value;

        // Act
        var result = HttpRules.HasContentType(headers, allowed);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(HttpRulesTestData.HasSingleHeaderValue.Cases), MemberType = typeof(HttpRulesTestData.HasSingleHeaderValue))]
    public void HasSingleHeaderValue_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, IEnumerable<string>> headers, string headerName)> tc)
    {
        // Arrange
        var (headers, headerName) = tc.Value;

        // Act
        var result = HttpRules.HasSingleHeaderValue(headers, headerName);

        // Assert
        AssertResult(tc, result);
    }
}
