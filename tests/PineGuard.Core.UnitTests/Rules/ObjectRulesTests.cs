using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class ObjectRulesTests : BaseUnitTest
{
    [Fact]
    public void IsEqualTo_UsesDefaultEqualityComparer()
    {
        // Arrange

        // Act
        var equal = ObjectRules.IsEqualTo("abc", "abc");
        var notEqual = ObjectRules.IsEqualTo("abc", "def");
        var nullEqual = ObjectRules.IsEqualTo<string?>(null, null);

        // Assert
        Assert.True(equal);
        Assert.False(notEqual);
        Assert.True(nullEqual);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsOfType.ValidCases), MemberType = typeof(ObjectRulesTestData.IsOfType))]
    public void IsOfType_ReturnsTrue_WhenValueIsT(ObjectRulesTestData.IsOfType.Case testCase)
    {
        // Arrange

        // Act
        var result = ObjectRules.IsOfType<string>(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsOfType.EdgeCases), MemberType = typeof(ObjectRulesTestData.IsOfType))]
    public void IsOfType_ReturnsFalse_WhenValueIsNotT(ObjectRulesTestData.IsOfType.Case testCase)
    {
        // Arrange

        // Act
        var result = ObjectRules.IsOfType<string>(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsAssignableToType.ValidCases), MemberType = typeof(ObjectRulesTestData.IsAssignableToType))]
    public void IsAssignableToType_ReturnsTrue_WhenValueIsAssignable(ObjectRulesTestData.IsAssignableToType.Case testCase)
    {
        // Arrange

        // Act
        var result = ObjectRules.IsAssignableToType<string>(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(ObjectRulesTestData.IsAssignableToType.EdgeCases), MemberType = typeof(ObjectRulesTestData.IsAssignableToType))]
    public void IsAssignableToType_ReturnsFalse_WhenNullOrNotAssignable(ObjectRulesTestData.IsAssignableToType.Case testCase)
    {
        // Arrange

        // Act
        var result = ObjectRules.IsAssignableToType<Exception>(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void IsSameReferenceAs_UsesReferenceEquals()
    {
        // Arrange
        var a = new object();
        var b = new object();

        // Act
        var same = ObjectRules.IsSameReferenceAs(a, a);
        var different = ObjectRules.IsSameReferenceAs(a, b);
        var nulls = ObjectRules.IsSameReferenceAs<string>(null, null);

        // Assert
        Assert.True(same);
        Assert.False(different);
        Assert.True(nulls);
    }
}
