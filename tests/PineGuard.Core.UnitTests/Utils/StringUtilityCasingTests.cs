using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class StringUtilityCasingTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringUtilityCasingTestData.TryCreateWords.ValidCases), MemberType = typeof(StringUtilityCasingTestData.TryCreateWords))]
    [MemberData(nameof(StringUtilityCasingTestData.TryCreateWords.EdgeCases), MemberType = typeof(StringUtilityCasingTestData.TryCreateWords))]
    public void TryCreateWords_ReturnsExpected(StringUtilityCasingTestData.TryCreateWords.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryCreateWords(testCase.Value.Value, testCase.Value.Style, out var words);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, words);
    }

    [Theory]
    [MemberData(nameof(StringUtilityCasingTestData.TryToCaseFromWords.ValidCases), MemberType = typeof(StringUtilityCasingTestData.TryToCaseFromWords))]
    [MemberData(nameof(StringUtilityCasingTestData.TryToCaseFromWords.EdgeCases), MemberType = typeof(StringUtilityCasingTestData.TryToCaseFromWords))]
    public void TryToCase_FromWords_ReturnsExpected(StringUtilityCasingTestData.TryToCaseFromWords.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryToCase(testCase.Value.Words, testCase.Value.OutputStyle, out var cased);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, cased);
        if (ok && testCase.Value.OutputStyle == StringCasing.SpaceCase)
            Assert.True(StringUtility.TryCreateWords(cased, StringCasing.SpaceCase, out _));
    }

    [Theory]
    [MemberData(nameof(StringUtilityCasingTestData.TryToCaseFromValue.ValidCases), MemberType = typeof(StringUtilityCasingTestData.TryToCaseFromValue))]
    [MemberData(nameof(StringUtilityCasingTestData.TryToCaseFromValue.EdgeCases), MemberType = typeof(StringUtilityCasingTestData.TryToCaseFromValue))]
    public void TryToCase_FromValue_ReturnsExpected(StringUtilityCasingTestData.TryToCaseFromValue.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryToCase(testCase.Value.Value, testCase.Value.InputStyle, testCase.Value.OutputStyle, out var cased);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, cased);
    }

    [Theory]
    [MemberData(nameof(StringUtilityCasingTestData.TryToCaseSingleStyle.ValidCases), MemberType = typeof(StringUtilityCasingTestData.TryToCaseSingleStyle))]
    [MemberData(nameof(StringUtilityCasingTestData.TryToCaseSingleStyle.EdgeCases), MemberType = typeof(StringUtilityCasingTestData.TryToCaseSingleStyle))]
    public void TryToCase_SingleStyle_ReturnsExpected(StringUtilityCasingTestData.TryToCaseSingleStyle.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.TryToCase(testCase.Value.Value, testCase.Value.Style, out var cased);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, cased);
    }
}
