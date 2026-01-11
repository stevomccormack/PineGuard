using PineGuard.Externals.Iso.Payments.Cards;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public sealed class IsoPaymentCardBrandUtilityTests : BaseUnitTest
{
    private sealed class UnknownBrand : IIsoPaymentCardBrand
    {
        public string BrandName => "Unknown";
        public int[] ValidPanLengths => [16];
        public string[] IinPrefixes => ["9"];
        public int[] DisplayFormatPattern => [4, 4, 4, 4];
        public bool MatchesPan(string? pan) => false;
    }

    [Fact]
    public void All_ContainsExpectedBrands()
    {
        // Act
        var all = IsoPaymentCardBrandUtility.All;

        // Assert
        Assert.Equal(6, all.Count);
        Assert.Contains(all, b => b.BrandName == VisaCard.Brand);
        Assert.Contains(all, b => b.BrandName == MastercardCard.Brand);
        Assert.Contains(all, b => b.BrandName == AmericanExpressCard.Brand);
        Assert.Contains(all, b => b.BrandName == DiscoverCard.Brand);
        Assert.Contains(all, b => b.BrandName == DinersClubCard.Brand);
        Assert.Contains(all, b => b.BrandName == JcbCard.Brand);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardBrandUtilityTestData.FromPan.ValidCases), MemberType = typeof(IsoPaymentCardBrandUtilityTestData.FromPan))]
    [MemberData(nameof(IsoPaymentCardBrandUtilityTestData.FromPan.EdgeCases), MemberType = typeof(IsoPaymentCardBrandUtilityTestData.FromPan))]
    public void FromPan_ReturnsExpected(IsoPaymentCardBrandUtilityTestData.FromPan.ValidCase testCase)
    {
        // Act
        var brand = IsoPaymentCardBrandUtility.FromPan(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, brand?.BrandName);
    }

    [Fact]
    public void TryFromPan_SetsOutParam()
    {
        // Act
        var ok = IsoPaymentCardBrandUtility.TryFromPan("4000000000000000", out var brand);
        var bad = IsoPaymentCardBrandUtility.TryFromPan("9999999999999999", out var noBrand);

        // Assert
        Assert.True(ok);
        Assert.NotNull(brand);
        Assert.False(bad);
        Assert.Null(noBrand);
    }

    [Theory]
    [MemberData(nameof(IsoPaymentCardBrandUtilityTestData.FromBrandName.ValidCases), MemberType = typeof(IsoPaymentCardBrandUtilityTestData.FromBrandName))]
    [MemberData(nameof(IsoPaymentCardBrandUtilityTestData.FromBrandName.EdgeCases), MemberType = typeof(IsoPaymentCardBrandUtilityTestData.FromBrandName))]
    public void FromBrandName_ReturnsExpected(IsoPaymentCardBrandUtilityTestData.FromBrandName.ValidCase testCase)
    {
        // Act
        var brand = IsoPaymentCardBrandUtility.FromBrandName(testCase.Value);

        // Assert
        Assert.Equal(testCase.ExpectedReturn, brand?.BrandName);
    }

    [Fact]
    public void TryFromBrandName_SetsOutParam()
    {
        // Act
        var ok = IsoPaymentCardBrandUtility.TryFromBrandName("visa", out var brand);
        var bad = IsoPaymentCardBrandUtility.TryFromBrandName("unknown", out var noBrand);

        // Assert
        Assert.True(ok);
        Assert.NotNull(brand);
        Assert.False(bad);
        Assert.Null(noBrand);
    }

    [Fact]
    public void ToIsoCardBrand_MapsKnownBrands_AndReturnsNullForUnknown()
    {
        // Arrange
        var unknown = new UnknownBrand();

        // Act
        var visa = IsoPaymentCardBrandUtility.ToIsoCardBrand(new VisaCard());
        var mc = IsoPaymentCardBrandUtility.ToIsoCardBrand(new MastercardCard());
        var amex = IsoPaymentCardBrandUtility.ToIsoCardBrand(new AmericanExpressCard());
        var discover = IsoPaymentCardBrandUtility.ToIsoCardBrand(new DiscoverCard());
        var diners = IsoPaymentCardBrandUtility.ToIsoCardBrand(new DinersClubCard());
        var jcb = IsoPaymentCardBrandUtility.ToIsoCardBrand(new JcbCard());
        var none = IsoPaymentCardBrandUtility.ToIsoCardBrand(null);
        var unknownResult = IsoPaymentCardBrandUtility.ToIsoCardBrand(unknown);

        // Assert
        Assert.Equal(CardBrand.Visa, visa);
        Assert.Equal(CardBrand.Mastercard, mc);
        Assert.Equal(CardBrand.AmericanExpress, amex);
        Assert.Equal(CardBrand.Discover, discover);
        Assert.Equal(CardBrand.DinersClub, diners);
        Assert.Equal(CardBrand.Jcb, jcb);
        Assert.Null(none);
        Assert.Null(unknownResult);
    }

    [Fact]
    public void ToIsoCardBrand_Extension_ForwardsToUtility()
    {
        // Arrange
        IIsoPaymentCardBrand brand = new VisaCard();

        // Act
        var viaExtension = brand.ToIsoCardBrand();

        // Assert
        Assert.Equal(CardBrand.Visa, viaExtension);
    }
}
