using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Iana;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class IanaTimeZoneUtilityTests : BaseUnitTest
{
    private sealed class FakeIanaTimeZoneProvider : IIanaTimeZoneProvider
    {
        public int IsValidCalls { get; private set; }
        public int TryGetByIdCalls { get; private set; }
        public int GetAllCalls { get; private set; }
        public int TryGetTimeZoneIdsByCountryCalls { get; private set; }

        public bool IsValidTimeZoneId(string? value)
        {
            IsValidCalls++;
            return string.Equals(value, "Example/Zone", StringComparison.Ordinal);
        }

        public bool TryGetById(string? value, out IanaTimeZone? timeZone)
        {
            TryGetByIdCalls++;

            if (string.Equals(value, "Example/Zone", StringComparison.Ordinal))
            {
                timeZone = new IanaTimeZone("Example/Zone", ["US"], "+0000+00000", comment: null);
                return true;
            }

            timeZone = null;
            return false;
        }

        public IReadOnlyCollection<IanaTimeZone> GetAll()
        {
            GetAllCalls++;
            return [new IanaTimeZone("Example/Zone", ["US"], "+0000+00000", comment: null)];
        }

        public bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds)
        {
            TryGetTimeZoneIdsByCountryCalls++;

            if (string.Equals(isoCountryAlpha2Code, "US", StringComparison.OrdinalIgnoreCase))
            {
                timeZoneIds = ["Example/Zone"];
                return true;
            }

            timeZoneIds = [];
            return false;
        }
    }

    [Fact]
    public void TryParseTimeZoneId_Trims()
    {
        var result = IanaTimeZoneUtility.TryParseTimeZoneId(" Etc/UTC ", out var id);

        Assert.True(result);
        Assert.Equal("Etc/UTC", id);
    }

    [Theory]
    [MemberData(nameof(IanaTimeZoneUtilityTestData.IsValidTimeZoneId.ValidCases), MemberType = typeof(IanaTimeZoneUtilityTestData.IsValidTimeZoneId))]
    [MemberData(nameof(IanaTimeZoneUtilityTestData.IsValidTimeZoneId.EdgeCases), MemberType = typeof(IanaTimeZoneUtilityTestData.IsValidTimeZoneId))]
    public void IsValidTimeZoneId_ReturnsExpected(IanaTimeZoneUtilityTestData.IsValidTimeZoneId.Case testCase)
    {
        var result = IanaTimeZoneUtility.IsValidTimeZoneId(testCase.Value);

        Assert.Equal(testCase.ExpectedReturn, result);
    }

    [Fact]
    public void Utility_Methods_UseProvidedProvider()
    {
        var provider = new FakeIanaTimeZoneProvider();

        Assert.True(IanaTimeZoneUtility.IsValidTimeZoneId("Example/Zone", provider));
        Assert.Equal(1, provider.IsValidCalls);

        Assert.True(IanaTimeZoneUtility.TryGetIanaTimeZone("Example/Zone", out var tz, provider));
        Assert.NotNull(tz);
        Assert.Equal("Example/Zone", tz.Id);
        Assert.Equal(1, provider.TryGetByIdCalls);

        var all = IanaTimeZoneUtility.GetAll(provider);
        Assert.Single(all);
        Assert.Equal(1, provider.GetAllCalls);

        Assert.True(IanaTimeZoneUtility.TryGetIanaTimeZoneIdsForCountryAlpha2("us", out var ids, provider));
        Assert.Contains("Example/Zone", ids);
        Assert.Equal(1, provider.TryGetTimeZoneIdsByCountryCalls);
    }

    [Fact]
    public void Utility_Methods_UseDefaultProvider_WhenProviderIsNull()
    {
        Assert.False(IanaTimeZoneUtility.TryGetIanaTimeZone(" ", out var tz, provider: null));
        Assert.Null(tz);

        var all = IanaTimeZoneUtility.GetAll(provider: null);
        Assert.NotNull(all);

        Assert.False(IanaTimeZoneUtility.TryGetIanaTimeZoneIdsForCountryAlpha2(" ", out var ids, provider: null));
        Assert.Empty(ids);
    }
}
