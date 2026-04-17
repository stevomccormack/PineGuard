using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class FilePathRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FilePathRulesTestData.IsSafeFileName.Cases), MemberType = typeof(FilePathRulesTestData.IsSafeFileName))]
    public void IsSafeFileName_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = FilePathRules.IsSafeFileName(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FilePathRulesTestData.HasFileExtension.Cases), MemberType = typeof(FilePathRulesTestData.HasFileExtension))]
    public void HasFileExtension_BehavesAsExpected(RuleCase<(string? path, string[]? allowed)> tc)
    {
        // Arrange
        var (path, allowed) = tc.Value;

        // Act
        var result = FilePathRules.HasFileExtension(path, allowed);

        // Assert
        AssertResult(tc, result);
    }
}
