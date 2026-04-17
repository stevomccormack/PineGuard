using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustReadOnlyDictionaryClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.Empty.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.Empty))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.Empty.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.Empty))]
    public void Empty_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, int>?> tc)
    {
        var dictionary = tc.Value;
        var result = Must.Be.Empty(dictionary);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotEmpty.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotEmpty))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotEmpty.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(MustCase<IReadOnlyDictionary<string, int>?> tc)
    {
        var dictionary = tc.Value;
        var result = Must.Be.NotEmpty(dictionary);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasKey.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasKey))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasKey.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasKey))]
    public void HasKey_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasKey(dictionary, tc.Value.key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasKey.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasKey))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasKey.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasKey))]
    public void NotHasKey_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasKey(dictionary, tc.Value.key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasValue.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasValue.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasValue))]
    public void HasValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasValue(dictionary, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasValue.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasValue.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasValue))]
    public void NotHasValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasValue(dictionary, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasKeyValue.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasKeyValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasKeyValue.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasKeyValue(dictionary, tc.Value.key, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasKeyValue.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasKeyValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasKeyValue.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasKeyValue))]
    public void NotHasKeyValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasKeyValue(dictionary, tc.Value.key, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyKey.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyKey))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyKey.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyKey))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyKey.NullPredicateCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasAnyKey(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyKey.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyKey))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyKey.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyKey))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyKey.NullPredicateCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyKey))]
    public void NotHasAnyKey_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasAnyKey(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyValue.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyValue.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyValue.NullPredicateCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasAnyValue(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyValue.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyValue.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyValue))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyValue.NullPredicateCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyValue))]
    public void NotHasAnyValue_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasAnyValue(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyItem.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyItem))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyItem.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyItem))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.HasAnyItem.NullPredicateCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasAnyItem(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyItem.ValidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyItem))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyItem.InvalidCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyItem))]
    [MemberData(nameof(MustReadOnlyDictionaryClausesTestData.NotHasAnyItem.NullPredicateCases), MemberType = typeof(MustReadOnlyDictionaryClausesTestData.NotHasAnyItem))]
    public void NotHasAnyItem_BehavesAsExpected(MustCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasAnyItem(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }
}
