using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardFileSignatureClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    // Guard.Against.NotFileSignature
    [Theory]
    [MemberData(nameof(GuardFileSignatureClausesTestData.NotFileSignature.ValidCases), MemberType = typeof(GuardFileSignatureClausesTestData.NotFileSignature))]
    [MemberData(nameof(GuardFileSignatureClausesTestData.NotFileSignature.InvalidCases), MemberType = typeof(GuardFileSignatureClausesTestData.NotFileSignature))]
    public void NotFileSignature_BehavesAsExpected(GuardCase<(byte[]? value, string extension)> tc)
    {
        // Arrange
        var (value, extension) = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotFileSignature(value, extension));
        AssertCustomMessage(tc, () => Guard.Against.NotFileSignature(value, extension, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    // Guard.Against.NotKnownFileSignature
    [Theory]
    [MemberData(nameof(GuardFileSignatureClausesTestData.NotKnownFileSignature.ValidCases), MemberType = typeof(GuardFileSignatureClausesTestData.NotKnownFileSignature))]
    [MemberData(nameof(GuardFileSignatureClausesTestData.NotKnownFileSignature.InvalidCases), MemberType = typeof(GuardFileSignatureClausesTestData.NotKnownFileSignature))]
    public void NotKnownFileSignature_BehavesAsExpected(GuardCase<byte[]?> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.NotKnownFileSignature(value));
        AssertCustomMessage(tc, () => Guard.Against.NotKnownFileSignature(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
