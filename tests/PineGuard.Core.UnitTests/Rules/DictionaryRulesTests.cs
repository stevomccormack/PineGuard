using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DictionaryRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.IsEmpty.Cases), MemberType = typeof(DictionaryRulesTestData.IsEmpty))]
    public void IsEmpty_BehavesAsExpected(RuleCase<IDictionary<string, int>?> tc)
    {
        // Act
        var result = DictionaryRules.IsEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.IsNotEmpty.Cases), MemberType = typeof(DictionaryRulesTestData.IsNotEmpty))]
    public void IsNotEmpty_BehavesAsExpected(RuleCase<IDictionary<string, int>?> tc)
    {
        // Act
        var result = DictionaryRules.IsNotEmpty(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasKey.Cases), MemberType = typeof(DictionaryRulesTestData.HasKey))]
    public void HasKey_BehavesAsExpected(RuleCase<(IDictionary<string, int>? dictionary, string key)> tc)
    {
        // Act
        var result = DictionaryRules.HasKey(tc.Value.dictionary, tc.Value.key);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasValue.Cases), MemberType = typeof(DictionaryRulesTestData.HasValue))]
    public void HasValue_BehavesAsExpected(RuleCase<(IDictionary<string, int>? dictionary, int value)> tc)
    {
        // Act
        var result = DictionaryRules.HasValue(tc.Value.dictionary, tc.Value.value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasKeyValue.Cases), MemberType = typeof(DictionaryRulesTestData.HasKeyValue))]
    public void HasKeyValue_BehavesAsExpected(RuleCase<(IDictionary<string, int>? dictionary, string key, int value)> tc)
    {
        // Act
        var result = DictionaryRules.HasKeyValue(tc.Value.dictionary, tc.Value.key, tc.Value.value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyKey.Cases), MemberType = typeof(DictionaryRulesTestData.HasAnyKey))]
    public void HasAnyKey_BehavesAsExpected(RuleCase<(IDictionary<string, int>? dictionary, Func<string, bool> predicate)> tc)
    {
        // Act
        var result = DictionaryRules.HasAnyKey(tc.Value.dictionary, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyKey.InvalidCases), MemberType = typeof(DictionaryRulesTestData.HasAnyKey))]
    public void HasAnyKey_Throws_WhenPredicateIsNull(DictionaryRulesTestData.HasAnyKey.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => DictionaryRules.HasAnyKey(tc.Value.Dictionary, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyValue.Cases), MemberType = typeof(DictionaryRulesTestData.HasAnyValue))]
    public void HasAnyValue_BehavesAsExpected(RuleCase<(IDictionary<string, int>? dictionary, Func<int, bool> predicate)> tc)
    {
        // Act
        var result = DictionaryRules.HasAnyValue(tc.Value.dictionary, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyValue.InvalidCases), MemberType = typeof(DictionaryRulesTestData.HasAnyValue))]
    public void HasAnyValue_Throws_WhenPredicateIsNull(DictionaryRulesTestData.HasAnyValue.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => DictionaryRules.HasAnyValue(tc.Value.Dictionary, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyItem.Cases), MemberType = typeof(DictionaryRulesTestData.HasAnyItem))]
    public void HasAnyItem_BehavesAsExpected(RuleCase<(IDictionary<string, int>? dictionary, Func<string, int, bool> predicate)> tc)
    {
        // Act
        var result = DictionaryRules.HasAnyItem(tc.Value.dictionary, tc.Value.predicate);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DictionaryRulesTestData.HasAnyItem.InvalidCases), MemberType = typeof(DictionaryRulesTestData.HasAnyItem))]
    public void HasAnyItem_Throws_WhenPredicateIsNull(DictionaryRulesTestData.HasAnyItem.InvalidCase tc)
    {
        // Act
        var ex = Assert.Throws(tc.ExpectedException.Type, () => DictionaryRules.HasAnyItem(tc.Value.Dictionary, tc.Value.Predicate!));

        // Assert
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
