using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardCollectionClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardCollectionClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.Empty.ValidCases), MemberType = typeof(TD.Empty))]
    [MemberData(nameof(TD.Empty.InvalidCases), MemberType = typeof(TD.Empty))]
    public void Empty_BehavesAsExpected(GuardCase<IEnumerable<string>> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Empty(value));
        AssertCustomMessage(tc, () => Guard.Against.Empty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotEmpty.ValidCases), MemberType = typeof(TD.NotEmpty))]
    [MemberData(nameof(TD.NotEmpty.InvalidCases), MemberType = typeof(TD.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(GuardCase<IEnumerable<string>> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotEmpty(value));
        AssertCustomMessage(tc, () => Guard.Against.NotEmpty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasExactCount.ValidCases), MemberType = typeof(TD.NotHasExactCount))]
    [MemberData(nameof(TD.NotHasExactCount.InvalidCases), MemberType = typeof(TD.NotHasExactCount))]
    public void NotHasExactCount_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int count)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotHasExactCount(value, tc.Value.count));
        AssertCustomMessage(tc, () => Guard.Against.NotHasExactCount(value, tc.Value.count, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasExactCount.ValidCases), MemberType = typeof(TD.HasExactCount))]
    [MemberData(nameof(TD.HasExactCount.InvalidCases), MemberType = typeof(TD.HasExactCount))]
    public void HasExactCount_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int count)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.HasExactCount(value, tc.Value.count));
        AssertCustomMessage(tc, () => Guard.Against.HasExactCount(value, tc.Value.count, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasMinCount.ValidCases), MemberType = typeof(TD.NotHasMinCount))]
    [MemberData(nameof(TD.NotHasMinCount.InvalidCases), MemberType = typeof(TD.NotHasMinCount))]
    public void NotHasMinCount_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int min)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotHasMinCount(value, tc.Value.min));
        AssertCustomMessage(tc, () => Guard.Against.NotHasMinCount(value, tc.Value.min, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasMinCount.ValidCases), MemberType = typeof(TD.HasMinCount))]
    [MemberData(nameof(TD.HasMinCount.InvalidCases), MemberType = typeof(TD.HasMinCount))]
    public void HasMinCount_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int min)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.HasMinCount(value, tc.Value.min));
        AssertCustomMessage(tc, () => Guard.Against.HasMinCount(value, tc.Value.min, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasMaxCount.ValidCases), MemberType = typeof(TD.NotHasMaxCount))]
    [MemberData(nameof(TD.NotHasMaxCount.InvalidCases), MemberType = typeof(TD.NotHasMaxCount))]
    public void NotHasMaxCount_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int max)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotHasMaxCount(value, tc.Value.max));
        AssertCustomMessage(tc, () => Guard.Against.NotHasMaxCount(value, tc.Value.max, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasMaxCount.ValidCases), MemberType = typeof(TD.HasMaxCount))]
    [MemberData(nameof(TD.HasMaxCount.InvalidCases), MemberType = typeof(TD.HasMaxCount))]
    public void HasMaxCount_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int max)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.HasMaxCount(value, tc.Value.max));
        AssertCustomMessage(tc, () => Guard.Against.HasMaxCount(value, tc.Value.max, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasCountBetween.ValidCases), MemberType = typeof(TD.NotHasCountBetween))]
    [MemberData(nameof(TD.NotHasCountBetween.InvalidCases), MemberType = typeof(TD.NotHasCountBetween))]
    public void NotHasCountBetween_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotHasCountBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.NotHasCountBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasCountBetween.ValidCases), MemberType = typeof(TD.HasCountBetween))]
    [MemberData(nameof(TD.HasCountBetween.InvalidCases), MemberType = typeof(TD.HasCountBetween))]
    public void HasCountBetween_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.HasCountBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.HasCountBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.DuplicateItems.ValidCases), MemberType = typeof(TD.DuplicateItems))]
    [MemberData(nameof(TD.DuplicateItems.InvalidCases), MemberType = typeof(TD.DuplicateItems))]
    public void DuplicateItems_BehavesAsExpected(GuardCase<IEnumerable<string>> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.DuplicateItems(value));
        AssertCustomMessage(tc, () => Guard.Against.DuplicateItems(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.DistinctItems.ValidCases), MemberType = typeof(TD.DistinctItems))]
    [MemberData(nameof(TD.DistinctItems.InvalidCases), MemberType = typeof(TD.DistinctItems))]
    public void DistinctItems_BehavesAsExpected(GuardCase<IEnumerable<string>> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.DistinctItems(value));
        AssertCustomMessage(tc, () => Guard.Against.DistinctItems(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.ContainsNullItems.ValidCases), MemberType = typeof(TD.ContainsNullItems))]
    [MemberData(nameof(TD.ContainsNullItems.InvalidCases), MemberType = typeof(TD.ContainsNullItems))]
    public void ContainsNullItems_BehavesAsExpected(GuardCase<IEnumerable<string?>> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.ContainsNullItems(value));
        AssertCustomMessage(tc, () => Guard.Against.ContainsNullItems(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContains.ValidCases), MemberType = typeof(TD.NotContains))]
    [MemberData(nameof(TD.NotContains.InvalidCases), MemberType = typeof(TD.NotContains))]
    public void NotContains_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, string item)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotContains(value, tc.Value.item));
        AssertCustomMessage(tc, () => Guard.Against.NotContains(value, tc.Value.item, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.Contains.ValidCases), MemberType = typeof(TD.Contains))]
    [MemberData(nameof(TD.Contains.InvalidCases), MemberType = typeof(TD.Contains))]
    public void Contains_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, string item)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.Contains(value, tc.Value.item));
        AssertCustomMessage(tc, () => Guard.Against.Contains(value, tc.Value.item, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotSubsetOf.ValidCases), MemberType = typeof(TD.NotSubsetOf))]
    [MemberData(nameof(TD.NotSubsetOf.InvalidCases), MemberType = typeof(TD.NotSubsetOf))]
    public void NotSubsetOf_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotSubsetOf(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.NotSubsetOf(value, tc.Value.other, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.SubsetOf.ValidCases), MemberType = typeof(TD.SubsetOf))]
    [MemberData(nameof(TD.SubsetOf.InvalidCases), MemberType = typeof(TD.SubsetOf))]
    public void SubsetOf_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.SubsetOf(value, tc.Value.other));
        AssertCustomMessage(tc, () => Guard.Against.SubsetOf(value, tc.Value.other, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasIndex.ValidCases), MemberType = typeof(TD.NotHasIndex))]
    [MemberData(nameof(TD.NotHasIndex.InvalidCases), MemberType = typeof(TD.NotHasIndex))]
    public void NotHasIndex_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int index)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.NotHasIndex(value, tc.Value.index));
        AssertCustomMessage(tc, () => Guard.Against.NotHasIndex(value, tc.Value.index, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.HasIndex.ValidCases), MemberType = typeof(TD.HasIndex))]
    [MemberData(nameof(TD.HasIndex.InvalidCases), MemberType = typeof(TD.HasIndex))]
    public void HasIndex_BehavesAsExpected(GuardCase<(IEnumerable<string>? value, int index)> tc)
    {
        var value = tc.Value.value;
        AssertResult(tc, () => Guard.Against.HasIndex(value, tc.Value.index));
        AssertCustomMessage(tc, () => Guard.Against.HasIndex(value, tc.Value.index, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(TD.NotHasAny.ValidCases), MemberType = typeof(TD.NotHasAny))]
    [MemberData(nameof(TD.NotHasAny.InvalidCases), MemberType = typeof(TD.NotHasAny))]
    public void NotHasAny_BehavesAsExpected(GuardCase<IEnumerable<string>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasAny(value, TD.NotHasAny.Predicate));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAny(value, TD.NotHasAny.Predicate, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasAny.ValidCases), MemberType = typeof(TD.HasAny))]
    [MemberData(nameof(TD.HasAny.InvalidCases), MemberType = typeof(TD.HasAny))]
    public void HasAny_BehavesAsExpected(GuardCase<IEnumerable<string>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HasAny(value, TD.HasAny.Predicate));
        AssertCustomMessage(tc, () => Guard.Against.HasAny(value, TD.HasAny.Predicate, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasAll.ValidCases), MemberType = typeof(TD.NotHasAll))]
    [MemberData(nameof(TD.NotHasAll.InvalidCases), MemberType = typeof(TD.NotHasAll))]
    public void NotHasAll_BehavesAsExpected(GuardCase<IEnumerable<string>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotHasAll(value, TD.NotHasAll.Predicate));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAll(value, TD.NotHasAll.Predicate, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasAll.ValidCases), MemberType = typeof(TD.HasAll))]
    [MemberData(nameof(TD.HasAll.InvalidCases), MemberType = typeof(TD.HasAll))]
    public void HasAll_BehavesAsExpected(GuardCase<IEnumerable<string>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.HasAll(value, TD.HasAll.Predicate));
        AssertCustomMessage(tc, () => Guard.Against.HasAll(value, TD.HasAll.Predicate, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
