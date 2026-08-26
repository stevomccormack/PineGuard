using PineGuard.Testing.UnitTests;
using PineGuard.Utils;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class PropertyPathUtilityTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Combine.ValidCases), MemberType = typeof(PropertyPathUtilityTestData.Combine))]
    public void Combine_BehavesAsExpected(PropertyPathUtilityTestData.Combine.ValidCase testCase)
    {
        // Arrange
        var (parent, property) = testCase.Value;

        // Act
        var result = PropertyPathUtility.Combine(parent, property);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Combine.InvalidCases), MemberType = typeof(PropertyPathUtilityTestData.Combine))]
    public void Combine_ThrowsAsExpected(PropertyPathUtilityTestData.Combine.InvalidCase testCase)
    {
        // Arrange
        var (parent, property) = testCase.Value;

        // Act
        var exception = Assert.Throws(testCase.ExpectedException.Type, () => PropertyPathUtility.Combine(parent, property!));

        // Assert
        ThrowsCaseAssert.Expected(exception, testCase);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Index.ValidCases), MemberType = typeof(PropertyPathUtilityTestData.Index))]
    public void Index_BehavesAsExpected(PropertyPathUtilityTestData.Index.ValidCase testCase)
    {
        // Arrange
        var (parent, index) = testCase.Value;

        // Act
        var result = PropertyPathUtility.Index(parent, index);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Key.ValidCases), MemberType = typeof(PropertyPathUtilityTestData.Key))]
    public void Key_BehavesAsExpected(PropertyPathUtilityTestData.Key.ValidCase testCase)
    {
        // Arrange
        var (parent, key) = testCase.Value;

        // Act
        var result = PropertyPathUtility.Key(parent, key);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Key.InvalidCases), MemberType = typeof(PropertyPathUtilityTestData.Key))]
    public void Key_ThrowsAsExpected(PropertyPathUtilityTestData.Key.InvalidCase testCase)
    {
        // Arrange
        var (parent, key) = testCase.Value;

        // Act
        var exception = Assert.Throws(testCase.ExpectedException.Type, () => PropertyPathUtility.Key(parent, key!));

        // Assert
        ThrowsCaseAssert.Expected(exception, testCase);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Transform.ValidCases), MemberType = typeof(PropertyPathUtilityTestData.Transform))]
    public void Transform_BehavesAsExpected(PropertyPathUtilityTestData.Transform.ValidCase testCase)
    {
        // Arrange
        var (path, segmentTransform) = testCase.Value;

        // Act
        var result = PropertyPathUtility.Transform(path, segmentTransform);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.Transform.InvalidCases), MemberType = typeof(PropertyPathUtilityTestData.Transform))]
    public void Transform_ThrowsAsExpected(PropertyPathUtilityTestData.Transform.InvalidCase testCase)
    {
        // Arrange
        var (path, segmentTransform) = testCase.Value;

        // Act
        var exception = Assert.Throws(testCase.ExpectedException.Type, () => PropertyPathUtility.Transform(path, segmentTransform!));

        // Assert
        ThrowsCaseAssert.Expected(exception, testCase);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.FromExpression.ValidCases), MemberType = typeof(PropertyPathUtilityTestData.FromExpression))]
    public void FromExpression_BehavesAsExpected(PropertyPathUtilityTestData.FromExpression.ValidCase testCase)
    {
        // Act
        var result = PropertyPathUtility.FromExpression(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(PropertyPathUtilityTestData.FromExpression.InvalidCases), MemberType = typeof(PropertyPathUtilityTestData.FromExpression))]
    public void FromExpression_ThrowsAsExpected(PropertyPathUtilityTestData.FromExpression.InvalidCase testCase)
    {
        // Act
        var exception = Assert.Throws(testCase.ExpectedException.Type, () => PropertyPathUtility.FromExpression(testCase.Value!));

        // Assert
        ThrowsCaseAssert.Expected(exception, testCase);
    }
}
