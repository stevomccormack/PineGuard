using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class PhoneUtilityTests : BaseUnitTest
{
    [Fact]
    public void TryParsePhone_ReturnsDigits_AndTrue_ForValidNumber()
    {
        var result = PhoneUtility.TryParsePhone("+1(425)555-0123", out var digits);

        Assert.True(result);
        Assert.Equal("14255550123", digits);
    }

    [Fact]
    public void TryParsePhone_ReturnsFalse_ForInvalidMinMax()
    {
        var result = PhoneUtility.TryParsePhone("4255550123", out var digits, minDigits: 10, maxDigits: 5);

        Assert.False(result);
        Assert.Equal(string.Empty, digits);
    }

    [Fact]
    public void TryParsePhone_ReturnsFalse_ForNullOrWhitespace()
    {
        Assert.False(PhoneUtility.TryParsePhone(null, out var digitsNull));
        Assert.Equal(string.Empty, digitsNull);

        Assert.False(PhoneUtility.TryParsePhone("   ", out var digitsWhitespace));
        Assert.Equal(string.Empty, digitsWhitespace);
    }

    [Fact]
    public void TryParsePhone_ReturnsFalse_WhenValueContainsDisallowedNonDigitCharacters()
    {
        var result = PhoneUtility.TryParsePhone("12x3", out var digits, allowedNonDigitCharacters: ['-']);

        Assert.False(result);
        Assert.Equal(string.Empty, digits);
    }

    [Fact]
    public void TryParsePhone_ReturnsFalse_WhenDigitsTooShortOrTooLong()
    {
        var tooShort = PhoneUtility.TryParsePhone("123", out var shortDigits, minDigits: 4, maxDigits: 10);
        Assert.False(tooShort);
        Assert.Equal("123", shortDigits);

        var tooLong = PhoneUtility.TryParsePhone("123456", out var longDigits, minDigits: 1, maxDigits: 5);
        Assert.False(tooLong);
        Assert.Equal("123456", longDigits);
    }

    [Fact]
    public void TryParsePhone_ReturnsFalse_ForMinOrMaxDigitsLessThanOne()
    {
        Assert.False(PhoneUtility.TryParsePhone("4255550123", out var digitsMin, minDigits: 0, maxDigits: 10));
        Assert.Equal(string.Empty, digitsMin);

        Assert.False(PhoneUtility.TryParsePhone("4255550123", out var digitsMax, minDigits: 1, maxDigits: 0));
        Assert.Equal(string.Empty, digitsMax);
    }
}
