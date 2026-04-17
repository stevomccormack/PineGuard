using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustDictionaryClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.Empty.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.Empty))]
    [MemberData(nameof(MustDictionaryClausesTestData.Empty.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.Empty))]
    public void Empty_BehavesAsExpected(MustCase<IDictionary<string, int>?> tc)
    {
        var dictionary = tc.Value;
        var result = Must.Be.Empty(dictionary);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotEmpty.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotEmpty))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotEmpty.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(MustCase<IDictionary<string, int>?> tc)
    {
        var dictionary = tc.Value;
        var result = Must.Be.NotEmpty(dictionary);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.HasKey.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.HasKey))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasKey.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.HasKey))]
    public void HasKey_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasKey(dictionary, tc.Value.key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasKey.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasKey))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasKey.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasKey))]
    public void NotHasKey_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasKey(dictionary, tc.Value.key);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.HasValue.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.HasValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasValue.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.HasValue))]
    public void HasValue_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasValue(dictionary, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasValue.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasValue.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasValue))]
    public void NotHasValue_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasValue(dictionary, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.HasKeyValue.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.HasKeyValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasKeyValue.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasKeyValue(dictionary, tc.Value.key, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasKeyValue.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasKeyValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasKeyValue.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasKeyValue))]
    public void NotHasKeyValue_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasKeyValue(dictionary, tc.Value.key, tc.Value.value);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyKey.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyKey))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyKey.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyKey))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyKey.NullPredicateCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasAnyKey(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyKey.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyKey))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyKey.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyKey))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyKey.NullPredicateCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyKey))]
    public void NotHasAnyKey_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasAnyKey(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyValue.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyValue.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyValue.NullPredicateCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasAnyValue(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyValue.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyValue.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyValue))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyValue.NullPredicateCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyValue))]
    public void NotHasAnyValue_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasAnyValue(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyItem.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyItem))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyItem.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyItem))]
    [MemberData(nameof(MustDictionaryClausesTestData.HasAnyItem.NullPredicateCases), MemberType = typeof(MustDictionaryClausesTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.HasAnyItem(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyItem.ValidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyItem))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyItem.InvalidCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyItem))]
    [MemberData(nameof(MustDictionaryClausesTestData.NotHasAnyItem.NullPredicateCases), MemberType = typeof(MustDictionaryClausesTestData.NotHasAnyItem))]
    public void NotHasAnyItem_BehavesAsExpected(MustCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dictionary = tc.Value.dictionary;
        var result = Must.Be.NotHasAnyItem(dictionary, tc.Value.predicate);
        AssertResult(tc, result);
    }
}
