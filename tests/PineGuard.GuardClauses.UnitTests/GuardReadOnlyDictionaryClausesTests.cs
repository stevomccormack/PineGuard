using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardReadOnlyDictionaryClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotEmpty.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotEmpty))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotEmpty.NullCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotEmpty))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotEmpty.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotEmpty))]
    public void NotEmpty_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, int>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotEmpty(value));
        AssertCustomMessage(tc, () => Guard.Against.NotEmpty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Same(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.Empty.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.Empty))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.Empty.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.Empty))]
    public void Empty_BehavesAsExpected(GuardCase<IReadOnlyDictionary<string, int>?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.Empty(value));
        AssertCustomMessage(tc, () => Guard.Against.Empty(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Same(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasKey.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasKey))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasKey.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasKey))]
    public void NotHasKey_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        var value = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.NotHasKey(value, tc.Value.key));
        AssertCustomMessage(tc, () => Guard.Against.NotHasKey(value, tc.Value.key, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasKey.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasKey))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasKey.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasKey))]
    public void HasKey_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        var value = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.HasKey(value, tc.Value.key));
        AssertCustomMessage(tc, () => Guard.Against.HasKey(value, tc.Value.key, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasValue.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasValue))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasValue.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasValue))]
    public void NotHasValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.NotHasValue(dict, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasValue(dict, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasValue.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasValue))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasValue.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasValue))]
    public void HasValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.HasValue(dict, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasValue(dict, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasKeyValue.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasKeyValue))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasKeyValue.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasKeyValue))]
    public void NotHasKeyValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.NotHasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasKeyValue.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasKeyValue))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasKeyValue.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        var dict = tc.Value.dictionary;
        AssertResult(tc, () => Guard.Against.HasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasKeyValue(dict, tc.Value.key, tc.Value.value, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyKey.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyKey))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyKey.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyKey))]
    public void NotHasAnyKey_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.NotHasAnyKey(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAnyKey(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasAnyKey.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasAnyKey))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasAnyKey.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.HasAnyKey(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasAnyKey(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyValue.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyValue))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyValue.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyValue))]
    public void NotHasAnyValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.NotHasAnyValue(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAnyValue(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasAnyValue.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasAnyValue))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasAnyValue.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.HasAnyValue(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasAnyValue(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyItem.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyItem))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyItem.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.NotHasAnyItem))]
    public void NotHasAnyItem_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.NotHasAnyItem(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotHasAnyItem(dict, predicate, paramName: "value", message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasAnyItem.ValidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasAnyItem))]
    [MemberData(nameof(GuardReadOnlyDictionaryClausesTestData.HasAnyItem.InvalidCases), MemberType = typeof(GuardReadOnlyDictionaryClausesTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(GuardCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        var dict = tc.Value.dictionary;
        var predicate = tc.Value.predicate;
        AssertResult(tc, () => Guard.Against.HasAnyItem(dict, predicate, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.HasAnyItem(dict, predicate, paramName: "value", message: CustomMessage));
    }
}
