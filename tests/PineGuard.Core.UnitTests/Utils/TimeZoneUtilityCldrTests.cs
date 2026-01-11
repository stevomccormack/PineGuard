using PineGuard.Externals.Cldr.TimeZones;
using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class TimeZoneUtilityCldrTests : BaseUnitTest
{
    private sealed class FakeCldrWindowsTimeZoneProvider : ICldrWindowsTimeZoneProvider
    {
        public string? LastIanaId { get; private set; }

        public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId) =>
            string.Equals(windowsTimeZoneId, "UTC", StringComparison.OrdinalIgnoreCase);

        public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
        {
            if (string.Equals(windowsTimeZoneId, "UTC", StringComparison.OrdinalIgnoreCase))
            {
                ianaTimeZoneIds = ["Etc/UTC"];
                return true;
            }

            ianaTimeZoneIds = [];
            return false;
        }

        public bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId)
        {
            LastIanaId = ianaTimeZoneId;

            if (string.Equals(ianaTimeZoneId, "Etc/UTC", StringComparison.Ordinal))
            {
                windowsTimeZoneId = "UTC";
                return true;
            }

            windowsTimeZoneId = string.Empty;
            return false;
        }
    }

    private sealed class FakeIanaTimeZoneProvider : IIanaTimeZoneProvider
    {
        public bool IsValidTimeZoneId(string? value) =>
            string.Equals(value, "Etc/UTC", StringComparison.Ordinal)
            || string.Equals(value, "NotSystem/UTC", StringComparison.Ordinal);

        public bool TryGetById(string? value, out IanaTimeZone? timeZone)
        {
            if (string.Equals(value, "Etc/UTC", StringComparison.Ordinal) || string.Equals(value, "NotSystem/UTC", StringComparison.Ordinal))
            {
                timeZone = new IanaTimeZone("Etc/UTC", ["US"], "+0000+00000", comment: null);
                return true;
            }

            timeZone = null;
            return false;
        }

        public IReadOnlyCollection<IanaTimeZone> GetAll() => [new("Etc/UTC", ["US"], "+0000+00000", comment: null)];

        public bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds)
        {
            timeZoneIds = [];
            return false;
        }
    }

    [Fact]
    public void TryParseWindowsTimeZoneId_TrimsAndValidates()
    {
        var parsed = TimeZoneUtility.Cldr.TryParseWindowsTimeZoneId(" UTC ", out var windowsId);

        Assert.True(parsed);
        Assert.Equal("UTC", windowsId);
        Assert.True(TimeZoneUtility.Cldr.IsWindowsTimeZoneId(windowsId));
    }

    [Fact]
    public void ProviderOverloads_Work()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        Assert.True(TimeZoneUtility.Cldr.IsWindowsTimeZoneId("UTC", cldrProvider));
        Assert.False(TimeZoneUtility.Cldr.IsWindowsTimeZoneId("NotAZone", cldrProvider));

        var mapped = TimeZoneUtility.Cldr.ToWindowsTimeZoneId(" Etc/UTC ", territory: null, cldrProvider);
        Assert.Equal("UTC", mapped);
        Assert.Equal("Etc/UTC", cldrProvider.LastIanaId);

        var iana = TimeZoneUtility.Cldr.ToIanaTimeZone("UTC", territory: null, cldrProvider, ianaProvider);
        Assert.NotNull(iana);
        Assert.Equal("Etc/UTC", iana.Id);

        if (OperatingSystem.IsWindows())
        {
            Assert.True(TimeZoneUtility.Cldr.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider));
            Assert.NotNull(tz);
        }
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsTrue_ForKnownSystemZoneId()
    {
        var systemFound = TimeZoneUtility.Cldr.TryGetSystemTimeZone("UTC", territory: null, out var tz);

        Assert.True(systemFound);
        Assert.NotNull(tz);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_Trims_OnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var custom = TimeZoneInfo.CreateCustomTimeZone(" CustomZone ", TimeSpan.Zero, "Custom", "Custom");

        Assert.True(TimeZoneUtility.Cldr.TryGetWindowsTimeZoneId(custom, territory: null, out var id));
        Assert.Equal("CustomZone", id);
    }
}
