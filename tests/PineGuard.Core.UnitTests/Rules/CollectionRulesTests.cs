using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class CollectionRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsEmpty.Cases), MemberType = typeof(CollectionRulesTestData.IsEmpty))]
    public void IsEmpty_BehavesAsExpected(RuleCase<IEnumerable<string>> tc)
    {
        // Act
        var result = CollectionRules.IsEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsEmpty.AdHocCases), MemberType = typeof(CollectionRulesTestData.IsEmpty))]
    public void IsEmpty_AdHoc_BehavesAsExpected(RuleCase<IEnumerable<string>?> tc)
    {
        // Act
        var result = CollectionRules.IsEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsNotEmpty.Cases), MemberType = typeof(CollectionRulesTestData.IsNotEmpty))]
    public void IsNotEmpty_BehavesAsExpected(RuleCase<IEnumerable<string>> tc)
    {
        // Act
        var result = CollectionRules.IsNotEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsNotEmpty.AdHocCases), MemberType = typeof(CollectionRulesTestData.IsNotEmpty))]
    public void IsNotEmpty_AdHoc_BehavesAsExpected(RuleCase<IEnumerable<string>?> tc)
    {
        // Act
        var result = CollectionRules.IsNotEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasExactCount.Cases), MemberType = typeof(CollectionRulesTestData.HasExactCount))]
    public void HasExactCount_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, int count)> tc)
    {
        // Act
        var result = CollectionRules.HasExactCount(tc.Value.value, tc.Value.count);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasExactCount.AdHocCases), MemberType = typeof(CollectionRulesTestData.HasExactCount))]
    public void HasExactCount_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, int Count)> tc)
    {
        // Act
        var result = CollectionRules.HasExactCount(tc.Value.Value, tc.Value.Count);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMinCount.Cases), MemberType = typeof(CollectionRulesTestData.HasMinCount))]
    public void HasMinCount_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, int min)> tc)
    {
        // Act
        var result = CollectionRules.HasMinCount(tc.Value.value, tc.Value.min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMinCount.AdHocCases), MemberType = typeof(CollectionRulesTestData.HasMinCount))]
    public void HasMinCount_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, int Min)> tc)
    {
        // Act
        var result = CollectionRules.HasMinCount(tc.Value.Value, tc.Value.Min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMaxCount.Cases), MemberType = typeof(CollectionRulesTestData.HasMaxCount))]
    public void HasMaxCount_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, int max)> tc)
    {
        // Act
        var result = CollectionRules.HasMaxCount(tc.Value.value, tc.Value.max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasMaxCount.AdHocCases), MemberType = typeof(CollectionRulesTestData.HasMaxCount))]
    public void HasMaxCount_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, int Max)> tc)
    {
        // Act
        var result = CollectionRules.HasMaxCount(tc.Value.Value, tc.Value.Max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasCountBetween.Cases), MemberType = typeof(CollectionRulesTestData.HasCountBetween))]
    public void HasCountBetween_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Act
        var result = CollectionRules.HasCountBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasCountBetween.AdHocCases), MemberType = typeof(CollectionRulesTestData.HasCountBetween))]
    public void HasCountBetween_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, int Min, int Max, Inclusion Inclusion)> tc)
    {
        // Act
        var result = CollectionRules.HasCountBetween(tc.Value.Value, tc.Value.Min, tc.Value.Max, tc.Value.Inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAny.Cases), MemberType = typeof(CollectionRulesTestData.HasAny))]
    public void HasAny_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, Func<string, bool> Predicate)> tc)
    {
        // Act
        var result = CollectionRules.HasAny(tc.Value.Value, tc.Value.Predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAny.InvalidCases), MemberType = typeof(CollectionRulesTestData.HasAny))]
    public void HasAny_Throws(CollectionRulesTestData.HasAny.InvalidCase tc)
    {
        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => CollectionRules.HasAny(tc.Value.Value, tc.Value.Predicate!));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAll.Cases), MemberType = typeof(CollectionRulesTestData.HasAll))]
    public void HasAll_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, Func<string, bool> Predicate)> tc)
    {
        // Act
        var result = CollectionRules.HasAll(tc.Value.Value, tc.Value.Predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasAll.InvalidCases), MemberType = typeof(CollectionRulesTestData.HasAll))]
    public void HasAll_Throws(CollectionRulesTestData.HasAll.InvalidCase tc)
    {
        // Act + Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => CollectionRules.HasAll(tc.Value.Value, tc.Value.Predicate!));
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasDistinctItems.Cases), MemberType = typeof(CollectionRulesTestData.HasDistinctItems))]
    public void HasDistinctItems_BehavesAsExpected(RuleCase<IEnumerable<string>> tc)
    {
        // Act
        var result = CollectionRules.HasDistinctItems(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasDistinctItems.ComparerCases), MemberType = typeof(CollectionRulesTestData.HasDistinctItems))]
    public void HasDistinctItems_Comparer_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, IEqualityComparer<string>? Comparer)> tc)
    {
        // Act
        var result = CollectionRules.HasDistinctItems(tc.Value.Value, tc.Value.Comparer);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasDuplicateItems.Cases), MemberType = typeof(CollectionRulesTestData.HasDuplicateItems))]
    public void HasDuplicateItems_BehavesAsExpected(RuleCase<IEnumerable<string>> tc)
    {
        // Act
        var result = CollectionRules.HasDuplicateItems(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.ContainsNullItems.Cases), MemberType = typeof(CollectionRulesTestData.ContainsNullItems))]
    public void ContainsNullItems_BehavesAsExpected(RuleCase<IEnumerable<string?>> tc)
    {
        // Act
        var result = CollectionRules.ContainsNullItems(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.Contains.Cases), MemberType = typeof(CollectionRulesTestData.Contains))]
    public void Contains_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, string item)> tc)
    {
        // Act
        var result = CollectionRules.Contains(tc.Value.value, tc.Value.item);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.Contains.AdHocCases), MemberType = typeof(CollectionRulesTestData.Contains))]
    public void Contains_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, string Item)> tc)
    {
        // Act
        var result = CollectionRules.Contains(tc.Value.Value, tc.Value.Item);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsSubsetOf.Cases), MemberType = typeof(CollectionRulesTestData.IsSubsetOf))]
    public void IsSubsetOf_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        // Act
        var result = CollectionRules.IsSubsetOf(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.IsSubsetOf.AdHocCases), MemberType = typeof(CollectionRulesTestData.IsSubsetOf))]
    public void IsSubsetOf_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, IEnumerable<string>? Other)> tc)
    {
        // Act
        var result = CollectionRules.IsSubsetOf(tc.Value.Value, tc.Value.Other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasIndex.Cases), MemberType = typeof(CollectionRulesTestData.HasIndex))]
    public void HasIndex_BehavesAsExpected(RuleCase<(IEnumerable<string>? value, int index)> tc)
    {
        // Act
        var result = CollectionRules.HasIndex(tc.Value.value, tc.Value.index);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(CollectionRulesTestData.HasIndex.AdHocCases), MemberType = typeof(CollectionRulesTestData.HasIndex))]
    public void HasIndex_AdHoc_BehavesAsExpected(RuleCase<(IEnumerable<string>? Value, int Index)> tc)
    {
        // Act
        var result = CollectionRules.HasIndex(tc.Value.Value, tc.Value.Index);

        // Assert
        AssertResult(tc, result);
    }
}
