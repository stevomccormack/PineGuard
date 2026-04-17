using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BitWiseRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BitWiseRulesTestData.IsBitwiseEqualToInt.Cases), MemberType = typeof(BitWiseRulesTestData.IsBitwiseEqualToInt))]
    public void IsBitwiseEqualTo_BehavesAsExpected(RuleCase<(int? left, int? right, int mask)> tc)
    {
        // Act
        var result = BitWiseRules.IsBitwiseEqualTo(tc.Value.left, tc.Value.right, tc.Value.mask);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BitWiseRulesTestData.HasAllBitsInt.Cases), MemberType = typeof(BitWiseRulesTestData.HasAllBitsInt))]
    public void HasAllBits_BehavesAsExpected(RuleCase<(int? value, int mask)> tc)
    {
        // Act
        var result = BitWiseRules.HasAllBits(tc.Value.value, tc.Value.mask);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BitWiseRulesTestData.HasAnyBitsInt.Cases), MemberType = typeof(BitWiseRulesTestData.HasAnyBitsInt))]
    public void HasAnyBits_BehavesAsExpected(RuleCase<(int? value, int mask)> tc)
    {
        // Act
        var result = BitWiseRules.HasAnyBits(tc.Value.value, tc.Value.mask);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BitWiseRulesTestData.HasNoBitsInt.Cases), MemberType = typeof(BitWiseRulesTestData.HasNoBitsInt))]
    public void HasNoBits_BehavesAsExpected(RuleCase<(int? value, int mask)> tc)
    {
        // Act
        var result = BitWiseRules.HasNoBits(tc.Value.value, tc.Value.mask);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BitWiseRulesTestData.HasOnlyBitsInt.Cases), MemberType = typeof(BitWiseRulesTestData.HasOnlyBitsInt))]
    public void HasOnlyBits_BehavesAsExpected(RuleCase<(int? value, int allowedMask)> tc)
    {
        // Act
        var result = BitWiseRules.HasOnlyBits(tc.Value.value, tc.Value.allowedMask);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(BitWiseRulesTestData.IsPowerOfTwoInt.Cases), MemberType = typeof(BitWiseRulesTestData.IsPowerOfTwoInt))]
    public void IsPowerOfTwo_BehavesAsExpected(RuleCase<int?> tc)
    {
        // Act
        var result = BitWiseRules.IsPowerOfTwo(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
