using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardBufferClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotHex
    [Theory]
    [MemberData(nameof(GuardBufferClausesTestData.NotHex.ValidCases), MemberType = typeof(GuardBufferClausesTestData.NotHex))]
    [MemberData(nameof(GuardBufferClausesTestData.NotHex.InvalidCases), MemberType = typeof(GuardBufferClausesTestData.NotHex))]
    public void NotHex_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHex(value));
        AssertCustomMessage(tc, () => Guard.Against.NotHex(value, message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotBase64
    [Theory]
    [MemberData(nameof(GuardBufferClausesTestData.NotBase64.ValidCases), MemberType = typeof(GuardBufferClausesTestData.NotBase64))]
    [MemberData(nameof(GuardBufferClausesTestData.NotBase64.InvalidCases), MemberType = typeof(GuardBufferClausesTestData.NotBase64))]
    public void NotBase64_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotBase64(value));
        AssertCustomMessage(tc, () => Guard.Against.NotBase64(value, message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Hex (inverted — throws when IS hex)
    [Theory]
    [MemberData(nameof(GuardBufferClausesTestData.Hex.ValidCases), MemberType = typeof(GuardBufferClausesTestData.Hex))]
    [MemberData(nameof(GuardBufferClausesTestData.Hex.InvalidCases), MemberType = typeof(GuardBufferClausesTestData.Hex))]
    [MemberData(nameof(GuardBufferClausesTestData.Hex.NullCases), MemberType = typeof(GuardBufferClausesTestData.Hex))]
    public void Hex_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Hex(value));
        AssertCustomMessage(tc, () => Guard.Against.Hex(value, message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.Base64 (inverted — throws when IS base64)
    [Theory]
    [MemberData(nameof(GuardBufferClausesTestData.Base64.ValidCases), MemberType = typeof(GuardBufferClausesTestData.Base64))]
    [MemberData(nameof(GuardBufferClausesTestData.Base64.InvalidCases), MemberType = typeof(GuardBufferClausesTestData.Base64))]
    [MemberData(nameof(GuardBufferClausesTestData.Base64.NullCases), MemberType = typeof(GuardBufferClausesTestData.Base64))]
    public void Base64_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Act + Assert
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Base64(value));
        AssertCustomMessage(tc, () => Guard.Against.Base64(value, message: CustomMessage));

        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotBase64Url
    [Theory]
    [MemberData(nameof(GuardBufferClausesTestData.NotBase64Url.ValidCases), MemberType = typeof(GuardBufferClausesTestData.NotBase64Url))]
    [MemberData(nameof(GuardBufferClausesTestData.NotBase64Url.InvalidCases), MemberType = typeof(GuardBufferClausesTestData.NotBase64Url))]
    public void NotBase64Url_BehavesAsExpected(GuardCase<string?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotBase64Url(value));
        AssertCustomMessage(tc, () => Guard.Against.NotBase64Url(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotUtf8
    [Theory]
    [MemberData(nameof(GuardBufferClausesTestData.NotUtf8.ValidCases), MemberType = typeof(GuardBufferClausesTestData.NotUtf8))]
    [MemberData(nameof(GuardBufferClausesTestData.NotUtf8.InvalidCases), MemberType = typeof(GuardBufferClausesTestData.NotUtf8))]
    public void NotUtf8_BehavesAsExpected(GuardCase<byte[]?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotUtf8(value));
        AssertCustomMessage(tc, () => Guard.Against.NotUtf8(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
