using PineGuard.Codes;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Codes;

public sealed class MustCodesTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    private static readonly Lazy<string[]> AllConstantValues = new(() =>
        MustCodesTestData.DiscoverConstantFields().Select(MustCodesTestData.GetConstantValue).ToArray());

    [Theory]
    [MemberData(nameof(MustCodesTestData.Constants.Cases), MemberType = typeof(MustCodesTestData.Constants))]
    public void Constant_MatchesGrammarMirrorsIdentifierPathAndIsUnique(MustCodesTestData.Constants.ConstantCase testCase)
    {
        // Arrange
        var value = MustCodesTestData.GetConstantValue(testCase.Field);
        var expectedPath = MustCodesTestData.GetIdentifierPath(testCase.Field);
        var declaringPrefix = MustCodesTestData.GetPrefix(testCase.Field.DeclaringType!);

        // Act
        var occurrences = AllConstantValues.Value.Count(v => string.Equals(v, value, StringComparison.Ordinal));

        // Assert
        Assert.Matches(MustCodesTestData.GrammarPattern, value);
        Assert.Equal(expectedPath, value);
        Assert.StartsWith(declaringPrefix + ".", value, StringComparison.Ordinal);
        Assert.Equal(1, occurrences);
    }

    [Theory]
    [MemberData(nameof(MustCodesTestData.Prefixes.Cases), MemberType = typeof(MustCodesTestData.Prefixes))]
    public void Prefix_MirrorsDomainTree(MustCodesTestData.Prefixes.PrefixCase testCase)
    {
        // Arrange
        var prefix = MustCodesTestData.GetPrefix(testCase.Type);
        var declaringType = testCase.Type.DeclaringType!;

        // Act
        var expected = declaringType == typeof(MustCodes)
            ? MustCodesTestData.ToKebabCase(testCase.Type.Name)
            : $"{MustCodesTestData.GetPrefix(declaringType)}.{MustCodesTestData.ToKebabCase(testCase.Type.Name)}";

        // Assert
        Assert.Equal(expected, prefix);
    }

    [Theory]
    [MemberData(nameof(MustCodesTestData.Prefixes.Cases), MemberType = typeof(MustCodesTestData.Prefixes))]
    public void DomainClass_IsStatic(MustCodesTestData.Prefixes.PrefixCase testCase)
    {
        // Assert
        Assert.True(testCase.Type is { IsAbstract: true, IsSealed: true });
    }
}
