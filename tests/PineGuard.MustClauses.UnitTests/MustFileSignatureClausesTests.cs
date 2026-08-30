using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustFileSignatureClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustFileSignatureClausesTestData.FileSignature.ValidCases), MemberType = typeof(MustFileSignatureClausesTestData.FileSignature))]
    [MemberData(nameof(MustFileSignatureClausesTestData.FileSignature.InvalidCases), MemberType = typeof(MustFileSignatureClausesTestData.FileSignature))]
    [MemberData(nameof(MustFileSignatureClausesTestData.FileSignature.UnknownExtensionCases), MemberType = typeof(MustFileSignatureClausesTestData.FileSignature))]
    public void FileSignature_BehavesAsExpected(MustCase<(byte[]? value, string extension)> tc)
    {
        // Act
        var result = Must.Be.FileSignature(tc.Value.value, tc.Value.extension, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustFileSignatureClausesTestData.KnownFileSignature.ValidCases), MemberType = typeof(MustFileSignatureClausesTestData.KnownFileSignature))]
    [MemberData(nameof(MustFileSignatureClausesTestData.KnownFileSignature.InvalidCases), MemberType = typeof(MustFileSignatureClausesTestData.KnownFileSignature))]
    public void KnownFileSignature_BehavesAsExpected(MustCase<byte[]?> tc)
    {
        // Act
        var result = Must.Be.KnownFileSignature(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
