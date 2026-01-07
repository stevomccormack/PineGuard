using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class PredicateRulesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(PredicateRulesTestData.Satisfies.ValidCases), MemberType = typeof(PredicateRulesTestData.Satisfies))]
    public void Satisfies_ReturnsTrue_WhenValueNotNullAndPredicateTrue(PredicateRulesTestData.Satisfies.Case testCase)
    {
        // Arrange
        static bool Predicate(int? x) => x == 5;

        // Act
        var result = PredicateRules.Satisfies(testCase.Value, Predicate);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(PredicateRulesTestData.Satisfies.EdgeCases), MemberType = typeof(PredicateRulesTestData.Satisfies))]
    public void Satisfies_ReturnsFalse_WhenValueNullOrPredicateFalse(PredicateRulesTestData.Satisfies.Case testCase)
    {
        // Arrange
        static bool Predicate(int? x) => x == 7;

        // Act
        var result = PredicateRules.Satisfies(testCase.Value, Predicate);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void Satisfies_Throws_WhenPredicateIsNull()
    {
        // Arrange

        // Act
        _ = Assert.Throws<ArgumentNullException>(() => PredicateRules.Satisfies(1, predicate: null!));

        // Assert
        Assert.True(true);
    }
}
