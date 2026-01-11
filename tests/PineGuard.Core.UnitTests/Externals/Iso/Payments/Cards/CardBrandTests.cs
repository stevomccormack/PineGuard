using PineGuard.Externals.Iso.Payments.Cards;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public sealed class CardBrandTests : BaseUnitTest
{
    [Fact]
    public void StaticBrands_HaveExpectedValues()
    {
        // Act

        // Assert
        Assert.Equal("Visa", CardBrand.Visa.Value);
        Assert.Equal("Mastercard", CardBrand.Mastercard.Value);
        Assert.Equal("American Express", CardBrand.AmericanExpress.Value);
        Assert.Equal("Discover", CardBrand.Discover.Value);
        Assert.Equal("Diners Club", CardBrand.DinersClub.Value);
        Assert.Equal("JCB", CardBrand.Jcb.Value);
    }

    [Theory]
    [MemberData(nameof(CardBrandTestData.FromPan.ValidCases), MemberType = typeof(CardBrandTestData.FromPan))]
    [MemberData(nameof(CardBrandTestData.FromPan.EdgeCases), MemberType = typeof(CardBrandTestData.FromPan))]
    public void FromPan_ReturnsExpected(CardBrandTestData.FromPan.ValidCase testCase)
    {
        // Act
        var brand = CardBrand.FromPan(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, brand?.Value);
    }

    [Fact]
    public void TryFromPan_SetsOutParam()
    {
        // Act
        var ok = CardBrand.TryFromPan("4000000000000000", out var visa);
        var bad = CardBrand.TryFromPan("9999999999999999", out var none);

        // Assert
        Assert.True(ok);
        Assert.Equal(CardBrand.Visa, visa);
        Assert.False(bad);
        Assert.Null(none);
    }

    [Theory]
    [MemberData(nameof(CardBrandTestData.ToIsoPaymentCardBrand.ValidCases), MemberType = typeof(CardBrandTestData.ToIsoPaymentCardBrand))]
    [MemberData(nameof(CardBrandTestData.ToIsoPaymentCardBrand.EdgeCases), MemberType = typeof(CardBrandTestData.ToIsoPaymentCardBrand))]
    public void ToIsoPaymentCardBrand_ReturnsExpected(CardBrandTestData.ToIsoPaymentCardBrand.ValidCase testCase)
    {
        // Arrange
        var brand = string.IsNullOrWhiteSpace(testCase.Value)
            ? null
            : PineGuard.Common.Enumeration<string>.FromName<CardBrand>(testCase.Value);

        // Act
        var iso = IsoCardBrandUtility.ToIsoPaymentCardBrand(brand);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, iso?.BrandName);
    }

    [Fact]
    public void ToIsoPaymentCardBrand_Extension_ForwardsToUtility()
    {
        // Arrange
        var brand = CardBrand.Visa;

        // Act
        var iso = brand.ToIsoPaymentCardBrand();

        // Assert
        Assert.NotNull(iso);
        Assert.Equal("Visa", iso.BrandName);
    }

    [Fact]
    public void ToIsoPaymentCardBrand_ReturnsNull_ForUnknownCardBrandInstance()
    {
        // Arrange
        var ctor = typeof(CardBrand).GetConstructor(
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
            binder: null,
            types: [typeof(string)],
            modifiers: null);

        Assert.NotNull(ctor);

        var unknown = (CardBrand)ctor.Invoke(["DefinitelyUnknown"]);

        // Act
        var iso = IsoCardBrandUtility.ToIsoPaymentCardBrand(unknown);

        // Assert
        Assert.Null(iso);
    }
}
