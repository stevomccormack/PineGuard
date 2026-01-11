using PineGuard.Extensions;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Extensions;

public sealed class StringExtensionTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringExtensionTestData.TitleCase.ValidCases), MemberType = typeof(StringExtensionTestData.TitleCase))]
    [MemberData(nameof(StringExtensionTestData.TitleCase.EdgeCases), MemberType = typeof(StringExtensionTestData.TitleCase))]
    public void TitleCase_ReturnsExpected(StringExtensionTestData.TitleCase.ValidCase testCase)
    {
        // Act
        var actual = testCase.Value!.TitleCase();
        var utilityActual = StringUtility.TitleCase(testCase.Value!, out var title);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, actual);
        Assert.Equal(testCase.ExpectedReturn, utilityActual);
        Assert.Equal(testCase.ExpectedOutValue, title);
        Assert.Equal(StringUtility.TitleCase(testCase.Value!), actual);
    }
}
