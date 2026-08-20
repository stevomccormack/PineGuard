using System.Globalization;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class StringUtilityNumberTypesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt32.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt32))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt32.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt32))]
    public void TryParseInt32_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseInt32.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseInt32(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedInt32, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt64.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt64))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseInt64.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseInt64))]
    public void TryParseInt64_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseInt64.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseInt64(testCase.Value, out var result);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedInt64, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDecimal.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDecimal))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDecimal.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDecimal))]
    public void TryParseDecimal_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseDecimal.ValidCase testCase)
    {
        // Arrange
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        // Act
        var ok = StringUtility.NumberTypes.TryParseDecimal(testCase.Value, out var result, provider: provider);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedDecimal, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseSingle.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseSingle))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseSingle.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseSingle))]
    public void TryParseSingle_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseSingle.ValidCase testCase)
    {
        // Arrange
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        // Act
        var ok = StringUtility.NumberTypes.TryParseSingle(testCase.Value, out var result, provider: provider);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedSingle, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDouble.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDouble))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDouble.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDouble))]
    public void TryParseDouble_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseDouble.ValidCase testCase)
    {
        // Arrange
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        // Act
        var ok = StringUtility.NumberTypes.TryParseDouble(testCase.Value, out var result, provider: provider);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedDouble, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryGetLastIntegerDigit.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryGetLastIntegerDigit))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryGetLastIntegerDigit.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryGetLastIntegerDigit))]
    public void TryGetLastIntegerDigit_ReturnsExpected(StringUtilityNumberTypesTestData.TryGetLastIntegerDigit.ValidCase testCase)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryGetLastIntegerDigit(testCase.Value.value, testCase.Value.styles, out var lastDigit);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedLastDigit, lastDigit);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.IsAsciiWhite.Cases), MemberType = typeof(StringUtilityNumberTypesTestData.IsAsciiWhite))]
    public void IsAsciiWhite_ReturnsExpected(StringUtilityNumberTypesTestData.IsAsciiWhite.Case testCase)
    {
        // Act
        var result = StringUtility.NumberTypes.IsAsciiWhite(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TrimLeadingSign.Cases), MemberType = typeof(StringUtilityNumberTypesTestData.TrimLeadingSign))]
    public void TrimLeadingSign_ReturnsExpected(StringUtilityNumberTypesTestData.TrimLeadingSign.Case testCase)
    {
        // Act
        var result = StringUtility.NumberTypes.TrimLeadingSign(testCase.Value.value.AsSpan(), testCase.Value.styles);

        // Assert
        Assert.Equal(testCase.Expected, result.ToString());
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TrimAsciiWhite.StartCases), MemberType = typeof(StringUtilityNumberTypesTestData.TrimAsciiWhite))]
    public void TrimStartAsciiWhite_ReturnsExpected(StringUtilityNumberTypesTestData.TrimAsciiWhite.Case testCase)
    {
        // Act
        var result = StringUtility.NumberTypes.TrimStartAsciiWhite(testCase.Value.AsSpan());

        // Assert
        Assert.Equal(testCase.Expected, result.ToString());
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TrimAsciiWhite.EndCases), MemberType = typeof(StringUtilityNumberTypesTestData.TrimAsciiWhite))]
    public void TrimEndAsciiWhite_ReturnsExpected(StringUtilityNumberTypesTestData.TrimAsciiWhite.Case testCase)
    {
        // Act
        var result = StringUtility.NumberTypes.TrimEndAsciiWhite(testCase.Value.AsSpan());

        // Assert
        Assert.Equal(testCase.Expected, result.ToString());
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.Int32IncompatibleHexStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseInt32_ReturnsFalse_ForIncompatibleHexStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseInt32("FF", out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.Int64IncompatibleHexStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseInt64_ReturnsFalse_ForIncompatibleHexStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseInt64("FF", out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.DecimalUnsupportedStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseDecimal_ReturnsFalse_ForUnsupportedStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseDecimal("FF", out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.DecimalWithPlacesUnsupportedStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseDecimal_WithDecimalPlaces_ReturnsFalse_ForUnsupportedStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseDecimal("FF", 2, out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.ExactDecimalUnsupportedStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseExactDecimal_ReturnsFalse_ForUnsupportedStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseExactDecimal("FF", 2, out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.SingleUnsupportedStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseSingle_ReturnsFalse_ForUnsupportedStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseSingle("FF", out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.InvalidStyles.DoubleUnsupportedStyles), MemberType = typeof(StringUtilityNumberTypesTestData.InvalidStyles))]
    public void TryParseDouble_ReturnsFalse_ForUnsupportedStyles(NumberStyles styles)
    {
        // Act
        var ok = StringUtility.NumberTypes.TryParseDouble("FF", out var result, styles);

        // Assert
        Assert.False(ok);
        Assert.Equal(0, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDecimalPlaces.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDecimalPlaces))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseDecimalPlaces.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseDecimalPlaces))]
    public void TryParseDecimalPlaces_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseDecimalPlaces.ValidCase testCase)
    {
        // Arrange
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        // Act
        var ok = StringUtility.NumberTypes.TryParseDecimal(testCase.Value, testCase.DecimalPlaces, out var result, provider: provider);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedDecimal, result);
    }

    [Theory]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseExactDecimal.ValidCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseExactDecimal))]
    [MemberData(nameof(StringUtilityNumberTypesTestData.TryParseExactDecimal.EdgeCases), MemberType = typeof(StringUtilityNumberTypesTestData.TryParseExactDecimal))]
    public void TryParseExactDecimal_ReturnsExpected(StringUtilityNumberTypesTestData.TryParseExactDecimal.ValidCase testCase)
    {
        // Arrange
        var provider = StringUtilityNumberTypesTestData.GetProvider(testCase.CultureName);

        // Act
        var ok = StringUtility.NumberTypes.TryParseExactDecimal(testCase.Value, testCase.ExactDecimalPlaces, out var result, provider: provider);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedDecimal, result);
    }
}
