using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class IsoUtilityTests : BaseUnitTest
{
    [Fact]
    public void Countries_Wrapper_ParseAndValidate()
    {
        Assert.True(IsoUtility.Countries.TryParseAlpha2(" us ", out var alpha2));
        Assert.Equal("us", alpha2);
        Assert.True(IsoUtility.Countries.IsIsoCountryAlpha2(alpha2));

        Assert.True(IsoUtility.Countries.TryParseAlpha2(" US ", out var alpha2Upper));
        Assert.Equal("US", alpha2Upper);
        Assert.True(IsoUtility.Countries.IsIsoCountryAlpha2(alpha2Upper));

        Assert.True(IsoUtility.Countries.TryParseAlpha3(" usa ", out var alpha3));
        Assert.Equal("usa", alpha3);
        Assert.True(IsoUtility.Countries.IsIsoCountryAlpha3(alpha3));

        Assert.True(IsoUtility.Countries.TryParseNumeric(" 840 ", out var numeric));
        Assert.Equal("840", numeric);
        Assert.True(IsoUtility.Countries.IsIsoCountryNumeric(numeric));
    }

    [Fact]
    public void Currencies_Wrapper_ParseAndValidate()
    {
        Assert.True(IsoUtility.Currencies.TryParseCurrencyAlpha3(" usd ", out var alpha3));
        Assert.Equal("usd", alpha3);
        Assert.True(IsoUtility.Currencies.IsIsoCurrencyAlpha3(alpha3));

        Assert.True(IsoUtility.Currencies.TryParseCurrencyAlpha3(" USD ", out var alpha3Upper));
        Assert.Equal("USD", alpha3Upper);
        Assert.True(IsoUtility.Currencies.IsIsoCurrencyAlpha3(alpha3Upper));

        Assert.True(IsoUtility.Currencies.TryParseCurrencyNumeric(" 840 ", out var numeric));
        Assert.Equal("840", numeric);
        Assert.True(IsoUtility.Currencies.IsIsoCurrencyNumeric(numeric));
    }

    [Fact]
    public void Languages_Wrapper_ParseAndValidate()
    {
        Assert.True(IsoUtility.Languages.TryParseLanguageAlpha2(" en ", out var alpha2));
        Assert.Equal("en", alpha2);
        Assert.True(IsoUtility.Languages.IsIsoLanguageAlpha2(alpha2));

        Assert.True(IsoUtility.Languages.TryParseLanguageAlpha3(" eng ", out var alpha3));
        Assert.Equal("eng", alpha3);
        Assert.True(IsoUtility.Languages.IsIsoLanguageAlpha3(alpha3));
    }

    [Fact]
    public void Dates_Wrapper_ParseMethods_Work()
    {
        Assert.True(IsoUtility.Dates.TryParseDateOnly(" 2020-01-02 ", out var dateOnly));
        Assert.Equal(new DateOnly(2020, 1, 2), dateOnly);

        Assert.True(IsoUtility.Dates.TryParseDateTime("2020-01-02T03:04:05", out var dateTime));
        Assert.Equal(new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified), dateTime);

        Assert.True(IsoUtility.Dates.TryParseDateTimeOffset("2020-01-02T03:04:05+00:00", out var dto));
        Assert.Equal(TimeSpan.Zero, dto.Offset);
    }
}
