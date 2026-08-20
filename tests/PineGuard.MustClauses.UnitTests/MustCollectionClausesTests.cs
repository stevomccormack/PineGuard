using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustCollectionClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.Empty.ValidCases), MemberType = typeof(MustCollectionClausesTestData.Empty))]
    [MemberData(nameof(MustCollectionClausesTestData.Empty.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.Empty))]
    public void Empty_BehavesAsExpected(MustCase<IEnumerable<string>> tc)
    {
        // Act
        var result = Must.Be.Empty(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotEmpty.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotEmpty))]
    [MemberData(nameof(MustCollectionClausesTestData.NotEmpty.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(MustCase<IEnumerable<string>> tc)
    {
        // Act
        var result = Must.Be.NotEmpty(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasExactCount.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasExactCount))]
    [MemberData(nameof(MustCollectionClausesTestData.HasExactCount.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasExactCount))]
    public void HasExactCount_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int count)> tc)
    {
        // Act
        var result = Must.Be.HasExactCount(tc.Value.value, tc.Value.count, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasMinCount.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasMinCount))]
    [MemberData(nameof(MustCollectionClausesTestData.HasMinCount.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasMinCount))]
    public void HasMinCount_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int min)> tc)
    {
        // Act
        var result = Must.Be.HasMinCount(tc.Value.value, tc.Value.min, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasMaxCount.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasMaxCount))]
    [MemberData(nameof(MustCollectionClausesTestData.HasMaxCount.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.HasMaxCount))]
    [MemberData(nameof(MustCollectionClausesTestData.HasMaxCount.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasMaxCount))]
    public void HasMaxCount_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int max)> tc)
    {
        // Act
        var result = Must.Be.HasMaxCount(tc.Value.value, tc.Value.max, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasCountBetween.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasCountBetween))]
    [MemberData(nameof(MustCollectionClausesTestData.HasCountBetween.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.HasCountBetween))]
    [MemberData(nameof(MustCollectionClausesTestData.HasCountBetween.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasCountBetween))]
    public void HasCountBetween_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Act
        var result = Must.Be.HasCountBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasAny.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasAny))]
    [MemberData(nameof(MustCollectionClausesTestData.HasAny.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasAny))]
    public void HasAny_BehavesAsExpected(MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.HasAny(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasAny.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasAny))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasAny.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasAny))]
    public void NotHasAny_BehavesAsExpected(MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.NotHasAny(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasAll.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasAll))]
    [MemberData(nameof(MustCollectionClausesTestData.HasAll.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasAll))]
    public void HasAll_BehavesAsExpected(MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.HasAll(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasAll.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasAll))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasAll.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasAll))]
    public void NotHasAll_BehavesAsExpected(MustCase<(IEnumerable<string>? value, Func<string, bool>? predicate)> tc)
    {
        // Act
        var result = Must.Be.NotHasAll(tc.Value.value, tc.Value.predicate!, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasDistinctItems.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasDistinctItems))]
    [MemberData(nameof(MustCollectionClausesTestData.HasDistinctItems.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.HasDistinctItems))]
    [MemberData(nameof(MustCollectionClausesTestData.HasDistinctItems.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasDistinctItems))]
    public void HasDistinctItems_BehavesAsExpected(MustCase<IEnumerable<string>> tc)
    {
        // Act
        var result = Must.Be.HasDistinctItems(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasDuplicateItems.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasDuplicateItems))]
    [MemberData(nameof(MustCollectionClausesTestData.HasDuplicateItems.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.HasDuplicateItems))]
    [MemberData(nameof(MustCollectionClausesTestData.HasDuplicateItems.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasDuplicateItems))]
    public void HasDuplicateItems_BehavesAsExpected(MustCase<IEnumerable<string>> tc)
    {
        // Act
        var result = Must.Be.HasDuplicateItems(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotContainsNullItems.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotContainsNullItems))]
    [MemberData(nameof(MustCollectionClausesTestData.NotContainsNullItems.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.NotContainsNullItems))]
    [MemberData(nameof(MustCollectionClausesTestData.NotContainsNullItems.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotContainsNullItems))]
    public void NotContainsNullItems_BehavesAsExpected(MustCase<IEnumerable<string?>> tc)
    {
        // Act
        var result = Must.Be.NotContainsNullItems(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.Contains.ValidCases), MemberType = typeof(MustCollectionClausesTestData.Contains))]
    [MemberData(nameof(MustCollectionClausesTestData.Contains.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.Contains))]
    public void Contains_BehavesAsExpected(MustCase<(IEnumerable<string>? value, string item)> tc)
    {
        // Act
        var result = Must.Be.Contains(tc.Value.value, tc.Value.item, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotContains.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotContains))]
    [MemberData(nameof(MustCollectionClausesTestData.NotContains.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotContains))]
    public void NotContains_BehavesAsExpected(MustCase<(IEnumerable<string>? value, string item)> tc)
    {
        // Act
        var result = Must.Be.NotContains(tc.Value.value, tc.Value.item, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.SubsetOf.ValidCases), MemberType = typeof(MustCollectionClausesTestData.SubsetOf))]
    [MemberData(nameof(MustCollectionClausesTestData.SubsetOf.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.SubsetOf))]
    [MemberData(nameof(MustCollectionClausesTestData.SubsetOf.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.SubsetOf))]
    public void SubsetOf_BehavesAsExpected(MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        // Act
        var result = Must.Be.SubsetOf(tc.Value.value, tc.Value.other, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotSubsetOf.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotSubsetOf))]
    [MemberData(nameof(MustCollectionClausesTestData.NotSubsetOf.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.NotSubsetOf))]
    [MemberData(nameof(MustCollectionClausesTestData.NotSubsetOf.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotSubsetOf))]
    public void NotSubsetOf_BehavesAsExpected(MustCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        // Act
        var result = Must.Be.NotSubsetOf(tc.Value.value, tc.Value.other, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.HasIndex.ValidCases), MemberType = typeof(MustCollectionClausesTestData.HasIndex))]
    [MemberData(nameof(MustCollectionClausesTestData.HasIndex.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.HasIndex))]
    public void HasIndex_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int index)> tc)
    {
        // Act
        var result = Must.Be.HasIndex(tc.Value.value, tc.Value.index, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasIndex.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasIndex))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasIndex.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasIndex))]
    public void NotHasIndex_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int index)> tc)
    {
        // Act
        var result = Must.Be.NotHasIndex(tc.Value.value, tc.Value.index, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasExactCount.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasExactCount))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasExactCount.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasExactCount))]
    public void NotHasExactCount_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int count)> tc)
    {
        // Act
        var result = Must.Be.NotHasExactCount(tc.Value.value, tc.Value.count, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasMinCount.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasMinCount))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasMinCount.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasMinCount))]
    public void NotHasMinCount_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int min)> tc)
    {
        // Act
        var result = Must.Be.NotHasMinCount(tc.Value.value, tc.Value.min, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasMaxCount.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasMaxCount))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasMaxCount.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.NotHasMaxCount))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasMaxCount.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasMaxCount))]
    public void NotHasMaxCount_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int max)> tc)
    {
        // Act
        var result = Must.Be.NotHasMaxCount(tc.Value.value, tc.Value.max, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasCountBetween.ValidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasCountBetween))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasCountBetween.EdgeCases), MemberType = typeof(MustCollectionClausesTestData.NotHasCountBetween))]
    [MemberData(nameof(MustCollectionClausesTestData.NotHasCountBetween.InvalidCases), MemberType = typeof(MustCollectionClausesTestData.NotHasCountBetween))]
    public void NotHasCountBetween_BehavesAsExpected(MustCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Act
        var result = Must.Be.NotHasCountBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
