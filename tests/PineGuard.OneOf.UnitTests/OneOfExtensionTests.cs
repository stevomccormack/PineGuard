using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.OneOf.UnitTests;

public sealed class OneOfExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(OneOfExtensionTestData.ToOneOf.Cases), MemberType = typeof(OneOfExtensionTestData.ToOneOf))]
    public void ToOneOf_BehavesAsExpected(OneOfCase<MustResult<string>> tc)
    {
        // Act
        var union = tc.Value.ToOneOf();

        // Assert
        AssertArm(tc.Expected, union.IsT0, union.IsT1);

        if (tc.Expected.IsValid)
        {
            Assert.Equal(tc.Expected.Value, union.AsT0);
            return;
        }

        AssertFailure(tc.Expected.Failures![0], union.AsT1);
    }

    [Theory]
    [MemberData(nameof(OneOfExtensionTestData.ToOneOf.InvalidCases), MemberType = typeof(OneOfExtensionTestData.ToOneOf))]
    public void ToOneOf_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(OneOfExtensionTestData.ToOneOfWithValue.Cases), MemberType = typeof(OneOfExtensionTestData.ToOneOfWithValue))]
    public void ToOneOfWithValue_BehavesAsExpected(OneOfCase<(MustValidationResult result, string value)> tc)
    {
        // Arrange
        var (validationResult, value) = tc.Value;

        // Act
        var union = validationResult.ToOneOf(value);

        // Assert
        AssertArm(tc.Expected, union.IsT0, union.IsT1);

        if (tc.Expected.IsValid)
        {
            Assert.Equal(tc.Expected.Value, union.AsT0);
            return;
        }

        AssertFailures(tc.Expected.Failures!, union.AsT1.Failures);
    }

    [Theory]
    [MemberData(nameof(OneOfExtensionTestData.ToOneOfWithValue.InvalidCases), MemberType = typeof(OneOfExtensionTestData.ToOneOfWithValue))]
    public void ToOneOfWithValue_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertArm(OneOfExpected expected, bool isT0, bool isT1)
    {
        Assert.Equal(expected.IsValid, isT0);
        Assert.Equal(!expected.IsValid, isT1);
    }

    private static void AssertFailures(IReadOnlyList<(string code, string message, string propertyPath)> expected, IReadOnlyList<MustFailure> actual)
    {
        Assert.Equal(expected.Count, actual.Count);

        for (var i = 0; i < expected.Count; i++)
            AssertFailure(expected[i], actual[i]);
    }

    private static void AssertFailure((string code, string message, string propertyPath) expected, MustFailure actual)
    {
        Assert.Equal(expected.code, actual.Code);
        Assert.Equal(expected.message, actual.Message);
        Assert.Equal(expected.propertyPath, actual.PropertyPath);
    }
}
