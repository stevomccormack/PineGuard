using FluentValidation;
using PineGuard.Common;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentCollectionExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public IEnumerable<string>? Value { get; init; } }
    private sealed record NullableModel { public IEnumerable<string?>? Value { get; init; } }

    // FluentCollectionExtensions.Empty
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.Empty.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.Empty))]
    public void Empty_BehavesAsExpected(FluentCase<IEnumerable<string>> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).Empty();
        var result = validator.Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotEmpty
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotEmpty.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(FluentCase<IEnumerable<string>> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotEmpty();
        var result = validator.Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasExactCount
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasExactCount.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasExactCount))]
    public void HasExactCount_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int count)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasExactCount(tc.Value.count);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasExactCount
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasExactCount.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasExactCount))]
    public void NotHasExactCount_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int count)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasExactCount(tc.Value.count);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasMinCount
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasMinCount.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasMinCount))]
    public void HasMinCount_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int min)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasMinCount(tc.Value.min);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasMinCount
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasMinCount.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasMinCount))]
    public void NotHasMinCount_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int min)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasMinCount(tc.Value.min);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasMaxCount
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasMaxCount.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasMaxCount))]
    public void HasMaxCount_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int max)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasMaxCount(tc.Value.max);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasMaxCount
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasMaxCount.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasMaxCount))]
    public void NotHasMaxCount_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int max)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasMaxCount(tc.Value.max);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasCountBetween
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasCountBetween.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasCountBetween))]
    public void HasCountBetween_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasCountBetween(tc.Value.min, tc.Value.max, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasCountBetween
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasCountBetween.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasCountBetween))]
    public void NotHasCountBetween_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int min, int max, Inclusion inclusion)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasCountBetween(tc.Value.min, tc.Value.max, tc.Value.inclusion);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasDistinctItems
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasDistinctItems.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasDistinctItems))]
    public void HasDistinctItems_BehavesAsExpected(FluentCase<IEnumerable<string>> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasDistinctItems();
        var result = validator.Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasDuplicateItems
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasDuplicateItems.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasDuplicateItems))]
    public void HasDuplicateItems_BehavesAsExpected(FluentCase<IEnumerable<string>> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasDuplicateItems();
        var result = validator.Validate(new Model { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotContainsNullItems
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotContainsNullItems.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotContainsNullItems))]
    public void NotContainsNullItems_BehavesAsExpected(FluentCase<IEnumerable<string?>> tc)
    {
        var validator = new InlineValidator<NullableModel>();
        validator.RuleFor(x => x.Value).NotContainsNullItems();
        var result = validator.Validate(new NullableModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.Contains
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.Contains.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.Contains))]
    public void Contains_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, string item)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).Contains(tc.Value.item);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotContains
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotContains.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotContains))]
    public void NotContains_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, string item)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotContains(tc.Value.item);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.SubsetOf
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.SubsetOf.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.SubsetOf))]
    public void SubsetOf_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).SubsetOf(tc.Value.other);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotSubsetOf
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotSubsetOf.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotSubsetOf))]
    public void NotSubsetOf_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, IEnumerable<string>? other)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotSubsetOf(tc.Value.other);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasIndex
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasIndex.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasIndex))]
    public void HasIndex_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int index)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasIndex(tc.Value.index);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasIndex
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasIndex.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasIndex))]
    public void NotHasIndex_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, int index)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasIndex(tc.Value.index);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasAny
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasAny.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasAny))]
    public void HasAny_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasAny(tc.Value.predicate);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasAny
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasAny.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasAny))]
    public void NotHasAny_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasAny(tc.Value.predicate);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.HasAll
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.HasAll.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.HasAll))]
    public void HasAll_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).HasAll(tc.Value.predicate);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }

    // FluentCollectionExtensions.NotHasAll
    [Theory]
    [MemberData(nameof(FluentCollectionExtensionsTestData.NotHasAll.Cases), MemberType = typeof(FluentCollectionExtensionsTestData.NotHasAll))]
    public void NotHasAll_BehavesAsExpected(FluentCase<(IEnumerable<string>? value, Func<string, bool> predicate)> tc)
    {
        var validator = new InlineValidator<Model>();
        validator.RuleFor(x => x.Value).NotHasAll(tc.Value.predicate);
        var result = validator.Validate(new Model { Value = tc.Value.value });
        AssertResult(tc, result);
    }
}
