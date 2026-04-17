using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class ReadOnlyDictionaryRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.IsEmpty.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.IsEmpty))]
    public void IsEmpty_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, int>?> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.IsEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.IsNotEmpty.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.IsNotEmpty))]
    public void IsNotEmpty_BehavesAsExpected(RuleCase<IReadOnlyDictionary<string, int>?> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.IsNotEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasKey.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasKey))]
    public void HasKey_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, int>? dictionary, string key)> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.HasKey(tc.Value.dictionary, tc.Value.key);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasValue.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasValue))]
    public void HasValue_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, int>? dictionary, int value)> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.HasValue(tc.Value.dictionary, tc.Value.value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasKeyValue.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.HasKeyValue(tc.Value.dictionary, tc.Value.key, tc.Value.value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasAnyKey.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.HasAnyKey(tc.Value.dictionary, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasAnyKey.InvalidCases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasAnyKey))]
    public void HasAnyKey_Throws_WhenPredicateIsNull(ReadOnlyDictionaryRulesTestData.HasAnyKey.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => ReadOnlyDictionaryRules.HasAnyKey(tc.Value.Dictionary, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasAnyValue.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.HasAnyValue(tc.Value.dictionary, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasAnyValue.InvalidCases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasAnyValue))]
    public void HasAnyValue_Throws_WhenPredicateIsNull(ReadOnlyDictionaryRulesTestData.HasAnyValue.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => ReadOnlyDictionaryRules.HasAnyValue(tc.Value.Dictionary, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasAnyItem.Cases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(RuleCase<(IReadOnlyDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        // Act
        var result = ReadOnlyDictionaryRules.HasAnyItem(tc.Value.dictionary, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(ReadOnlyDictionaryRulesTestData.HasAnyItem.InvalidCases), MemberType = typeof(ReadOnlyDictionaryRulesTestData.HasAnyItem))]
    public void HasAnyItem_Throws_WhenPredicateIsNull(ReadOnlyDictionaryRulesTestData.HasAnyItem.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => ReadOnlyDictionaryRules.HasAnyItem(tc.Value.Dictionary, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
