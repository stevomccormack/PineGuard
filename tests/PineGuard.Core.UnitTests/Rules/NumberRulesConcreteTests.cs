using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class NumberRulesConcreteTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(NumberRulesConcreteTestData.Values), MemberType = typeof(NumberRulesConcreteTestData))]
    public void ScalarPredicates_MatchGenericDecimal(decimal? value)
    {
        // Act / Assert
        Assert.Equal(NumberRules.IsPositive<decimal>(value), NumberRules.IsPositive(value));
        Assert.Equal(NumberRules.IsNegative<decimal>(value), NumberRules.IsNegative(value));
        Assert.Equal(NumberRules.IsZero<decimal>(value), NumberRules.IsZero(value));
        Assert.Equal(NumberRules.IsNotZero<decimal>(value), NumberRules.IsNotZero(value));
        Assert.Equal(NumberRules.IsZeroOrPositive<decimal>(value), NumberRules.IsZeroOrPositive(value));
        Assert.Equal(NumberRules.IsZeroOrNegative<decimal>(value), NumberRules.IsZeroOrNegative(value));
        Assert.Equal(NumberRules.IsGreaterThan<decimal>(value, 0), NumberRules.IsGreaterThan(value, 0));
        Assert.Equal(NumberRules.IsGreaterThanOrEqual<decimal>(value, 0), NumberRules.IsGreaterThanOrEqual(value, 0));
        Assert.Equal(NumberRules.IsLessThan<decimal>(value, 0), NumberRules.IsLessThan(value, 0));
        Assert.Equal(NumberRules.IsLessThanOrEqual<decimal>(value, 0), NumberRules.IsLessThanOrEqual(value, 0));
        Assert.Equal(NumberRules.IsPercentage<decimal>(value), NumberRules.IsPercentage(value));
    }

    [Theory]
    [MemberData(nameof(NumberRulesConcreteTestData.Approximation), MemberType = typeof(NumberRulesConcreteTestData))]
    public void Approximately_MatchesGenericDecimal(decimal? value, decimal target, decimal? tolerance)
    {
        // Act
        var result = NumberRules.IsApproximately(value, target, tolerance);

        // Assert
        Assert.Equal(NumberRules.IsApproximately<decimal>(value, target, tolerance), result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesConcreteTestData.Multiples), MemberType = typeof(NumberRulesConcreteTestData))]
    public void MultipleOf_MatchesGenericDecimal(decimal? value, decimal factor)
    {
        // Act
        var result = NumberRules.IsMultipleOf(value, factor);

        // Assert
        Assert.Equal(NumberRules.IsMultipleOf<decimal>(value, factor), result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesConcreteTestData.Ranges), MemberType = typeof(NumberRulesConcreteTestData))]
    public void InRange_MatchesGenericDecimal(decimal? value, decimal min, decimal max, Inclusion inclusion)
    {
        // Act
        var result = NumberRules.IsInRange(value, min, max, inclusion);

        // Assert
        Assert.Equal(NumberRules.IsInRange<decimal>(value, min, max, inclusion), result);
    }

    [Theory]
    [MemberData(nameof(NumberRulesConcreteTestData.InvalidInclusions), MemberType = typeof(NumberRulesConcreteTestData))]
    public void InRange_UndefinedInclusion_ThrowsBeforeNullValue(Inclusion inclusion)
    {
        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => NumberRules.IsInRange(null, 0m, 1m, inclusion));

        // Assert
        Assert.Equal("inclusion", exception.ParamName);
    }
}
