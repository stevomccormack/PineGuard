using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class VariantRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(VariantRulesTestData.IsEvenNonNullable.Cases), MemberType = typeof(VariantRulesTestData.IsEvenNonNullable))]
    public void IsEven_NonNullable_BehavesAsExpected(RuleCase<int> tc)
    {
        // Act
        var result = VariantRules.IsEven(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(VariantRulesTestData.Parse.InvalidCases), MemberType = typeof(VariantRulesTestData.Parse))]
    public void Parse_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (VariantRulesTestData.Parse.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => VariantRules.Parse(t.Value!));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
