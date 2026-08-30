using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustBufferClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustBufferClausesTestData.Hex.ValidCases), MemberType = typeof(MustBufferClausesTestData.Hex))]
    [MemberData(nameof(MustBufferClausesTestData.Hex.InvalidCases), MemberType = typeof(MustBufferClausesTestData.Hex))]
    public void Hex_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Hex(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBufferClausesTestData.Base64.ValidCases), MemberType = typeof(MustBufferClausesTestData.Base64))]
    [MemberData(nameof(MustBufferClausesTestData.Base64.InvalidCases), MemberType = typeof(MustBufferClausesTestData.Base64))]
    public void Base64_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Base64(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBufferClausesTestData.NotHex.ValidCases), MemberType = typeof(MustBufferClausesTestData.NotHex))]
    [MemberData(nameof(MustBufferClausesTestData.NotHex.InvalidCases), MemberType = typeof(MustBufferClausesTestData.NotHex))]
    [MemberData(nameof(MustBufferClausesTestData.NotHex.NullCases), MemberType = typeof(MustBufferClausesTestData.NotHex))]
    public void NotHex_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.NotHex(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBufferClausesTestData.NotBase64.ValidCases), MemberType = typeof(MustBufferClausesTestData.NotBase64))]
    [MemberData(nameof(MustBufferClausesTestData.NotBase64.InvalidCases), MemberType = typeof(MustBufferClausesTestData.NotBase64))]
    [MemberData(nameof(MustBufferClausesTestData.NotBase64.NullCases), MemberType = typeof(MustBufferClausesTestData.NotBase64))]
    public void NotBase64_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.NotBase64(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBufferClausesTestData.Base64Url.ValidCases), MemberType = typeof(MustBufferClausesTestData.Base64Url))]
    [MemberData(nameof(MustBufferClausesTestData.Base64Url.InvalidCases), MemberType = typeof(MustBufferClausesTestData.Base64Url))]
    public void Base64Url_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.Base64Url(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBufferClausesTestData.Utf8.ValidCases), MemberType = typeof(MustBufferClausesTestData.Utf8))]
    [MemberData(nameof(MustBufferClausesTestData.Utf8.InvalidCases), MemberType = typeof(MustBufferClausesTestData.Utf8))]
    public void Utf8_BehavesAsExpected(MustCase<byte[]?> tc)
    {
        // Act
        var result = Must.Be.Utf8(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
