using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iso;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class IsoPaymentCardUtilityTests : BaseUnitTest
{
    private const string VisaDigits = "4111111111111111";
    private const string VisaSpaces = "4111 1111 1111 1111";
    private const string VisaHyphens = "4111-1111-1111-1111";

    [Fact]
    public void IsValidCardNumber_UsesDefaultSeparators_WhenSeparatorsNull()
    {
        Assert.True(IsoPaymentCardUtility.IsValidCardNumber(VisaSpaces));
    }

    [Fact]
    public void IsValidCardNumber_RespectsCustomSeparators()
    {
        Assert.True(IsoPaymentCardUtility.IsValidCardNumber(VisaDigits, allowedSeparators: []));
        Assert.False(IsoPaymentCardUtility.IsValidCardNumber(VisaSpaces, allowedSeparators: []));
    }

    [Fact]
    public void IsLuhnValid_ReturnsExpected()
    {
        Assert.True(IsoPaymentCardUtility.IsLuhnValid(VisaDigits));
        Assert.True(IsoPaymentCardUtility.IsLuhnValid(VisaHyphens));
        Assert.False(IsoPaymentCardUtility.IsLuhnValid("4111111111111112"));
        Assert.False(IsoPaymentCardUtility.IsLuhnValid("abcd"));
    }

    [Fact]
    public void Mask_ReturnsMaskedOrEmpty()
    {
        Assert.Equal("", IsoPaymentCardUtility.Mask(null));
        Assert.Equal("", IsoPaymentCardUtility.Mask("   "));
        Assert.Equal("", IsoPaymentCardUtility.Mask("1234"));

        var masked = IsoPaymentCardUtility.Mask(VisaDigits);
        Assert.Equal("4111********1111", masked);
        Assert.True(IsoPaymentCardUtility.IsMasked(masked));
    }

    [Fact]
    public void SecureMask_ReturnsMaskedOrEmpty()
    {
        Assert.Equal("", IsoPaymentCardUtility.SecureMask(null));
        Assert.Equal("", IsoPaymentCardUtility.SecureMask("   "));
        Assert.Equal("", IsoPaymentCardUtility.SecureMask("123"));

        Assert.Equal("1234", IsoPaymentCardUtility.SecureMask("1234"));

        var masked = IsoPaymentCardUtility.SecureMask(VisaDigits);
        Assert.Equal("************1111", masked);
        Assert.True(IsoPaymentCardUtility.IsMasked(masked));
    }

    [Fact]
    public void IsMasked_ReturnsFalse_ForNullOrWhitespace()
    {
        Assert.False(IsoPaymentCardUtility.IsMasked(null));
        Assert.False(IsoPaymentCardUtility.IsMasked(""));
        Assert.False(IsoPaymentCardUtility.IsMasked("   "));
    }

    [Fact]
    public void Format_Sanitize_And_GetLast4_ReturnExpected()
    {
        Assert.Equal("", IsoPaymentCardUtility.Format(null));
        Assert.Equal("", IsoPaymentCardUtility.Format("abcd"));

        Assert.Equal(VisaDigits, IsoPaymentCardUtility.Sanitize(VisaHyphens));
        Assert.Equal("", IsoPaymentCardUtility.Sanitize("abcd"));

        Assert.Equal("4111 1111 1111 1111", IsoPaymentCardUtility.Format(VisaDigits));
        Assert.Equal("4111-1111-1111-1111", IsoPaymentCardUtility.Format(VisaDigits, separator: '-'));

        Assert.Equal("", IsoPaymentCardUtility.GetLast4(null));
        Assert.Equal("", IsoPaymentCardUtility.GetLast4("123"));
        Assert.Equal("****1111", IsoPaymentCardUtility.GetLast4(VisaDigits));
    }
}
