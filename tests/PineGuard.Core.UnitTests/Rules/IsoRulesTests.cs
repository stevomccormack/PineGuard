using PineGuard.Rules;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class IsoRulesTests : BaseUnitTest
{
    [Fact]
    public void Dates_Wrappers_Work()
    {
        Assert.True(IsoRules.Dates.IsDateOnly("2020-01-02"));
        Assert.False(IsoRules.Dates.IsDateOnly("2020-13-01"));

        Assert.True(IsoRules.Dates.IsDateTime("2020-01-02T03:04:05"));
        Assert.False(IsoRules.Dates.IsDateTime("not-a-date"));

        Assert.True(IsoRules.Dates.IsDateTimeOffset("2020-01-02T03:04:05+00:00"));
        Assert.True(IsoRules.Dates.IsDateTimeOffset("2020-01-02T03:04:05"));
        Assert.False(IsoRules.Dates.IsDateTimeOffset("2020-01-02T03:04:05+25:00"));
    }

    [Fact]
    public void Countries_Currencies_Languages_Wrappers_Work()
    {
        Assert.True(IsoRules.Countries.IsAlpha2("US"));
        Assert.True(IsoRules.Countries.IsAlpha3("USA"));
        Assert.True(IsoRules.Countries.IsNumeric("840"));
        Assert.False(IsoRules.Countries.IsAlpha2("U"));
        Assert.False(IsoRules.Countries.IsNumeric("84"));

        Assert.True(IsoRules.Currencies.IsAlpha3("USD"));
        Assert.True(IsoRules.Currencies.IsNumeric("840"));
        Assert.False(IsoRules.Currencies.IsAlpha3("US"));
        Assert.False(IsoRules.Currencies.IsNumeric("000"));

        Assert.True(IsoRules.Languages.IsAlpha2("en"));
        Assert.True(IsoRules.Languages.IsAlpha3("eng"));
        Assert.False(IsoRules.Languages.IsAlpha2("e"));
        Assert.False(IsoRules.Languages.IsAlpha3("engl"));
    }

    [Fact]
    public void PaymentCards_Wrappers_Work()
    {
        Assert.True(IsoRules.PaymentCards.IsPaymentCard("4111 1111 1111 1111"));
        Assert.False(IsoRules.PaymentCards.IsPaymentCard("not-a-card"));

        Assert.True(IsoRules.PaymentCards.IsPaymentCard("4111-1111-1111-1111", allowedSeparators: ['-']));
        Assert.False(IsoRules.PaymentCards.IsPaymentCard("4111 1111 1111 1111", allowedSeparators: ['-']));
    }
}
