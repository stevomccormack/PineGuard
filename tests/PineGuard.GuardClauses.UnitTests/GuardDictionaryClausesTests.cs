using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDictionaryClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotEmpty.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotEmpty))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotEmpty.NullCases), MemberType = typeof(GuardDictionaryClausesTestData.NotEmpty))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotEmpty.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(GuardCase<IDictionary<string, int>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotEmpty(value));
        AssertCustomMessage(tc, () => Guard.Against.NotEmpty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Same(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.Empty.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.Empty))]
    [MemberData(nameof(GuardDictionaryClausesTestData.Empty.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.Empty))]
    public void Empty_BehavesAsExpected(GuardCase<IDictionary<string, int>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Empty(value));
        AssertCustomMessage(tc, () => Guard.Against.Empty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Same(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasKey.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasKey))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasKey.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasKey))]
    public void NotHasKey_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        var value = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.NotHasKey(value, tc.Value.key));
        AssertCustomMessage(tc, () => Guard.Against.NotHasKey(value, tc.Value.key, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasKey.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasKey))]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasKey.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasKey))]
    public void HasKey_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        var value = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.HasKey(value, tc.Value.key));
        AssertCustomMessage(tc, () => Guard.Against.HasKey(value, tc.Value.key, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasValue.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasValue))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasValue.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasValue))]
    public void NotHasValue_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.NotHasValue(dict, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasValue(dict, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasValue.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasValue))]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasValue.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasValue))]
    public void HasValue_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.HasValue(dict, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasValue(dict, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasKeyValue.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasKeyValue))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasKeyValue.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasKeyValue))]
    public void NotHasKeyValue_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.NotHasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasKeyValue.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasKeyValue))]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasKeyValue.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.HasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasAnyKey.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasAnyKey))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasAnyKey.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasAnyKey))]
    public void NotHasAnyKey_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.NotHasAnyKey(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAnyKey(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasAnyKey.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasAnyKey))]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasAnyKey.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.HasAnyKey(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasAnyKey(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasAnyValue.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasAnyValue))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasAnyValue.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasAnyValue))]
    public void NotHasAnyValue_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.NotHasAnyValue(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAnyValue(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasAnyValue.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasAnyValue))]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasAnyValue.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.HasAnyValue(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasAnyValue(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasAnyItem.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasAnyItem))]
    [MemberData(nameof(GuardDictionaryClausesTestData.NotHasAnyItem.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.NotHasAnyItem))]
    public void NotHasAnyItem_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.NotHasAnyItem(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAnyItem(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasAnyItem.ValidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasAnyItem))]
    [MemberData(nameof(GuardDictionaryClausesTestData.HasAnyItem.InvalidCases), MemberType = typeof(GuardDictionaryClausesTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(GuardCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.HasAnyItem(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasAnyItem(dict, predicate, paramName: "value", message: CustomMessage));
    }
}
