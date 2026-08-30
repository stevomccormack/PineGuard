using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class FileSignatureRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FileSignatureRulesTestData.HasSignature.Cases), MemberType = typeof(FileSignatureRulesTestData.HasSignature))]
    public void HasSignature_BehavesAsExpected(RuleCase<(byte[]? value, string extension)> tc)
    {
        // Arrange
        var (value, extension) = tc.Value;

        // Act
        var result = FileSignatureRules.HasSignature(value, extension);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FileSignatureRulesTestData.HasSignature.InvalidCases), MemberType = typeof(FileSignatureRulesTestData.HasSignature))]
    public void HasSignature_Throws_WhenExtensionHasNoRegisteredSignature(FileSignatureRulesTestData.HasSignature.InvalidCase tc)
    {
        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => FileSignatureRules.HasSignature(tc.Input.Value, tc.Input.Extension));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(FileSignatureRulesTestData.HasKnownSignature.Cases), MemberType = typeof(FileSignatureRulesTestData.HasKnownSignature))]
    public void HasKnownSignature_BehavesAsExpected(RuleCase<byte[]?> tc)
    {
        // Act
        var result = FileSignatureRules.HasKnownSignature(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
