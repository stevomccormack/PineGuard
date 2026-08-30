using PineGuard.Testing.UnitTests.Rules;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class FileSignatureUtilityTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FileSignatureUtilityTestData.TryDetectExtension.Cases), MemberType = typeof(FileSignatureUtilityTestData.TryDetectExtension))]
    public void TryDetectExtension_BehavesAsExpected(RuleCase<(byte[]? header, string? extension)> tc)
    {
        // Arrange
        var (header, expectedExtension) = tc.Value;

        // Act
        var result = FileSignatureUtility.TryDetectExtension(header, out var extension);

        // Assert
        AssertResult(tc, result);
        Assert.Equal(expectedExtension, extension);
    }

    [Theory]
    [MemberData(nameof(FileSignatureUtilityTestData.IsKnownExtension.Cases), MemberType = typeof(FileSignatureUtilityTestData.IsKnownExtension))]
    public void IsKnownExtension_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = FileSignatureUtility.IsKnownExtension(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
