using PineGuard.Externals.Cldr.TimeZones;
using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.UnitTests;
using PineGuard.Utils.Cldr;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class CldrTimeZoneUtilityTests : BaseUnitTest
{
    private sealed class FakeCldrWindowsTimeZoneProvider : ICldrWindowsTimeZoneProvider
    {
        public string? LastIanaId { get; private set; }
        public string? LastTerritory { get; private set; }

        public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId) =>
            string.Equals(windowsTimeZoneId, "UTC", StringComparison.OrdinalIgnoreCase);

        public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
        {
            LastTerritory = territory;

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
            LastTerritory = territory;

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

    private sealed class FakeEmptyIanaIdsCldrProvider : ICldrWindowsTimeZoneProvider
    {
        public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId) => false;

        public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
        {
            ianaTimeZoneIds = [];
            return true;
        }

        public bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId)
        {
            windowsTimeZoneId = string.Empty;
            return false;
        }
    }

    private sealed class FakeWhitespaceFirstIanaIdCldrProvider : ICldrWindowsTimeZoneProvider
    {
        public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId) => false;

        public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
        {
            ianaTimeZoneIds = [" ", "Etc/UTC"]; // first item is whitespace -> should be rejected
            return true;
        }

        public bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId)
        {
            windowsTimeZoneId = string.Empty;
            return false;
        }
    }

    private sealed class FakeAlwaysUnknownIanaProvider : IIanaTimeZoneProvider
    {
        public bool IsValidTimeZoneId(string? value) => false;

        public bool TryGetById(string? value, out IanaTimeZone? timeZone)
        {
            timeZone = null;
            return false;
        }

        public IReadOnlyCollection<IanaTimeZone> GetAll() => [];

        public bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds)
        {
            timeZoneIds = [];
            return false;
        }
    }

    private sealed class FakeNullIanaTimeZoneProvider : IIanaTimeZoneProvider
    {
        public bool IsValidTimeZoneId(string? value) => true;

        public bool TryGetById(string? value, out IanaTimeZone? timeZone)
        {
            timeZone = null;
            return true;
        }

        public IReadOnlyCollection<IanaTimeZone> GetAll() => [];

        public bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds)
        {
            timeZoneIds = [];
            return false;
        }
    }

    private sealed class FakeBadWindowsIdCldrProvider : ICldrWindowsTimeZoneProvider
    {
        public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId) => false;

        public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
        {
            ianaTimeZoneIds = [];
            return false;
        }

        public bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId)
        {
            windowsTimeZoneId = "DefinitelyNotAZone";
            return true;
        }
    }

    [Theory]
    [MemberData(nameof(CldrTimeZoneUtilityTestData.ToWindowsTimeZoneId.EdgeCases), MemberType = typeof(CldrTimeZoneUtilityTestData.ToWindowsTimeZoneId))]
    public void ToWindowsTimeZoneId_ReturnsNull_ForNullOrWhitespace(CldrTimeZoneUtilityTestData.ToWindowsTimeZoneId.Case testCase)
    {
        Assert.Null(CldrTimeZoneUtility.ToWindowsTimeZoneId(testCase.IanaTimeZoneId));
    }

    [Fact]
    public void ToWindowsTimeZoneId_UsesDefaultTerritory_WhenNotProvided()
    {
        var provider = new FakeCldrWindowsTimeZoneProvider();

        var windowsId = CldrTimeZoneUtility.ToWindowsTimeZoneId(" Etc/UTC ", territory: null, provider);

        Assert.Equal("UTC", windowsId);
        Assert.Equal(CldrTimeZoneUtility.DefaultTerritory, provider.LastTerritory);
        Assert.Equal("Etc/UTC", provider.LastIanaId);
    }

    [Fact]
    public void ToWindowsTimeZoneId_TrimsTerritory_WhenProvided()
    {
        var provider = new FakeCldrWindowsTimeZoneProvider();

        var windowsId = CldrTimeZoneUtility.ToWindowsTimeZoneId("Etc/UTC", territory: " US ", provider);

        Assert.Equal("UTC", windowsId);
        Assert.Equal("US", provider.LastTerritory);
        Assert.Equal("Etc/UTC", provider.LastIanaId);
    }

    [Fact]
    public void ToWindowsTimeZoneId_ReturnsNull_WhenProviderCannotMap()
    {
        var provider = new FakeCldrWindowsTimeZoneProvider();

        var windowsId = CldrTimeZoneUtility.ToWindowsTimeZoneId("Not/AZone", territory: null, provider);

        Assert.Null(windowsId);
        Assert.Equal(CldrTimeZoneUtility.DefaultTerritory, provider.LastTerritory);
    }

    [Theory]
    [MemberData(nameof(CldrTimeZoneUtilityTestData.TryParseWindowsTimeZoneId.ValidCases), MemberType = typeof(CldrTimeZoneUtilityTestData.TryParseWindowsTimeZoneId))]
    [MemberData(nameof(CldrTimeZoneUtilityTestData.TryParseWindowsTimeZoneId.EdgeCases), MemberType = typeof(CldrTimeZoneUtilityTestData.TryParseWindowsTimeZoneId))]
    public void TryParseWindowsTimeZoneId_ReturnsExpected(CldrTimeZoneUtilityTestData.TryParseWindowsTimeZoneId.Case testCase)
    {
        var result = CldrTimeZoneUtility.TryParseWindowsTimeZoneId(testCase.Value, out var windowsId);

        Assert.Equal(testCase.ExpectedReturn, result);
        Assert.Equal(testCase.ExpectedOutValue, windowsId);
    }

    [Fact]
    public void TryParseWindowsTimeZoneId_UsesProvidedProvider()
    {
        var provider = new FakeCldrWindowsTimeZoneProvider();

        Assert.True(CldrTimeZoneUtility.TryParseWindowsTimeZoneId(" UTC ", out var windowsId, provider));
        Assert.Equal("UTC", windowsId);

        Assert.False(CldrTimeZoneUtility.TryParseWindowsTimeZoneId("Not/AZone", out _, provider));
    }

    [Fact]
    public void ToIanaTimeZone_ReturnsExpected_WhenProvidersSupplyMapping()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        var tz = CldrTimeZoneUtility.ToIanaTimeZone("UTC", territory: null, cldrProvider, ianaProvider);

        Assert.NotNull(tz);
        Assert.Equal("Etc/UTC", tz.Id);
    }

    [Fact]
    public void ToIanaTimeZone_ReturnsNull_WhenCldrProviderHasNoMapping()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        var tz = CldrTimeZoneUtility.ToIanaTimeZone("Not/AZone", territory: null, cldrProvider, ianaProvider);

        Assert.Null(tz);
    }

    [Fact]
    public void TryGetWindowsTimeZoneIdCore_UsesDefaultTerritory_WhenTerritoryIsNullOrWhitespace()
    {
        var provider = new FakeCldrWindowsTimeZoneProvider();
        var tz = TimeZoneInfo.CreateCustomTimeZone("Etc/UTC", TimeSpan.Zero, "Etc/UTC", "Etc/UTC");

        Assert.True(CldrTimeZoneUtility.TryGetWindowsTimeZoneIdCore(tz, territory: null, out var windowsId, provider, isWindows: false));
        Assert.Equal("UTC", windowsId);
        Assert.Equal(CldrTimeZoneUtility.DefaultTerritory, provider.LastTerritory);

        Assert.True(CldrTimeZoneUtility.TryGetWindowsTimeZoneIdCore(tz, territory: "  ", out windowsId, provider, isWindows: false));
        Assert.Equal("UTC", windowsId);
        Assert.Equal(CldrTimeZoneUtility.DefaultTerritory, provider.LastTerritory);
    }

    [Fact]
    public void TryGetWindowsTimeZoneIdCore_TrimsTerritory_WhenProvided()
    {
        var provider = new FakeCldrWindowsTimeZoneProvider();
        var tz = TimeZoneInfo.CreateCustomTimeZone("Etc/UTC", TimeSpan.Zero, "Etc/UTC", "Etc/UTC");

        Assert.True(CldrTimeZoneUtility.TryGetWindowsTimeZoneIdCore(tz, territory: " US ", out var windowsId, provider, isWindows: false));
        Assert.Equal("UTC", windowsId);
        Assert.Equal("US", provider.LastTerritory);
    }

    [Fact]
    public void TryGetSystemTimeZone_UsesDefaultTerritoryOrTrims_WhenMappingViaCldr()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        Assert.True(CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider));
        Assert.NotNull(tz);
        Assert.Equal(CldrTimeZoneUtility.DefaultTerritory, cldrProvider.LastTerritory);

        Assert.True(CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: " US ", out tz, cldrProvider, ianaProvider));
        Assert.NotNull(tz);
        Assert.Equal("US", cldrProvider.LastTerritory);
    }

    [Fact]
    public void ToIanaTimeZone_ReturnsNull_WhenCldrProviderReturnsEmptyIds()
    {
        var cldrProvider = new FakeEmptyIanaIdsCldrProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        var tz = CldrTimeZoneUtility.ToIanaTimeZone("UTC", territory: null, cldrProvider, ianaProvider);

        Assert.Null(tz);
    }

    [Fact]
    public void ToIanaTimeZone_ReturnsNull_WhenFirstIanaIdIsWhitespace()
    {
        var cldrProvider = new FakeWhitespaceFirstIanaIdCldrProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        var tz = CldrTimeZoneUtility.ToIanaTimeZone("UTC", territory: null, cldrProvider, ianaProvider);

        Assert.Null(tz);
    }

    [Fact]
    public void ToIanaTimeZone_ReturnsNull_WhenIanaProviderCannotResolveId()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeAlwaysUnknownIanaProvider();

        var tz = CldrTimeZoneUtility.ToIanaTimeZone("UTC", territory: null, cldrProvider, ianaProvider);

        Assert.Null(tz);
    }

    [Theory]
    [MemberData(nameof(CldrTimeZoneUtilityTestData.ToIanaTimeZone.EdgeCases), MemberType = typeof(CldrTimeZoneUtilityTestData.ToIanaTimeZone))]
    public void ToIanaTimeZone_ReturnsNull_ForNullOrWhitespace(CldrTimeZoneUtilityTestData.ToIanaTimeZone.Case testCase)
    {
        var tz = CldrTimeZoneUtility.ToIanaTimeZone(testCase.WindowsTimeZoneId, territory: null);

        Assert.Null(tz);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_ReturnsFalse_ForNullTimeZone()
    {
        Assert.False(CldrTimeZoneUtility.TryGetWindowsTimeZoneId(null, territory: null, out var id));
        Assert.Equal(string.Empty, id);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_ReturnsTrimmedTimeZoneId_OnWindows()
    {
        var custom = TimeZoneInfo.CreateCustomTimeZone(" CustomZone ", TimeSpan.Zero, "Custom", "Custom");

        var result = CldrTimeZoneUtility.TryGetWindowsTimeZoneId(custom, territory: null, out var id);

        if (OperatingSystem.IsWindows())
        {
            Assert.True(result);
            Assert.Equal("CustomZone", id);
        }
        else
        {
            // Non-Windows path maps via CLDR (custom IDs won't typically map).
            Assert.False(result);
            Assert.Equal(string.Empty, id);
        }
    }

    [Fact]
    public void TryGetWindowsTimeZoneIdCore_CoversWindowsAndNonWindowsPaths_Deterministically()
    {
        var windowsCustom = TimeZoneInfo.CreateCustomTimeZone(" CustomZone ", TimeSpan.Zero, "Custom", "Custom");

        Assert.True(CldrTimeZoneUtility.TryGetWindowsTimeZoneIdCore(windowsCustom, territory: null, out var windowsId, provider: null, isWindows: true));
        Assert.Equal("CustomZone", windowsId);

        // Use a custom time zone with an IANA-like ID so the embedded CLDR mapping can be exercised deterministically.
        var ianaLike = TimeZoneInfo.CreateCustomTimeZone("Etc/UTC", TimeSpan.Zero, "Etc/UTC", "Etc/UTC");

        Assert.True(CldrTimeZoneUtility.TryGetWindowsTimeZoneIdCore(ianaLike, territory: null, out var mapped, provider: null, isWindows: false));
        Assert.Equal("UTC", mapped);

        var unknown = TimeZoneInfo.CreateCustomTimeZone("Not/AZone", TimeSpan.Zero, "Not/AZone", "Not/AZone");
        Assert.False(CldrTimeZoneUtility.TryGetWindowsTimeZoneIdCore(unknown, territory: null, out var unknownMapped, provider: null, isWindows: false));
        Assert.Equal(string.Empty, unknownMapped);
    }

    [Fact]
    public void TryGetSystemTimeZone_CanResolveIanaId_OnWindows_UsingProviders()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        // Use an ID that is not expected to exist as a system time zone ID, so the utility must map via providers.
        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider);

        if (OperatingSystem.IsWindows())
        {
            Assert.True(result);
            Assert.NotNull(tz);
            Assert.Equal("UTC", tz.Id);
        }
        else
        {
            Assert.False(result);
            Assert.Null(tz);
        }
    }

    [Theory]
    [MemberData(nameof(CldrTimeZoneUtilityTestData.TryGetSystemTimeZone.EdgeCases), MemberType = typeof(CldrTimeZoneUtilityTestData.TryGetSystemTimeZone))]
    public void TryGetSystemTimeZone_ReturnsFalse_ForNullOrWhitespace(CldrTimeZoneUtilityTestData.TryGetSystemTimeZone.Case testCase)
    {
        var result = CldrTimeZoneUtility.TryGetSystemTimeZone(testCase.WindowsOrIanaTimeZoneId, territory: null, out var tz);

        Assert.False(result);
        Assert.Null(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsTrue_ForSystemTimeZoneId_WhenAvailable()
    {
        var result = CldrTimeZoneUtility.TryGetSystemTimeZone(TimeZoneInfo.Utc.Id, territory: null, out var tz);

        Assert.True(result);
        Assert.NotNull(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsFalse_WhenIdNotSystemTimeZone_AndNotWindows()
    {
        if (OperatingSystem.IsWindows())
            return;

        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz);

        Assert.False(result);
        Assert.Null(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsFalse_WhenIanaProviderCannotResolveId_OnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeAlwaysUnknownIanaProvider();

        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider);

        Assert.False(result);
        Assert.Null(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsFalse_WhenCldrProviderCannotMap_OnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var cldrProvider = new FakeEmptyIanaIdsCldrProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider);

        Assert.False(result);
        Assert.Null(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsFalse_WhenWindowsIdNotFound_OnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var cldrProvider = new FakeBadWindowsIdCldrProvider();
        var ianaProvider = new FakeIanaTimeZoneProvider();

        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider);

        Assert.False(result);
        Assert.Null(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_ReturnsFalse_WhenIanaProviderReturnsNullTimeZone()
    {
        var cldrProvider = new FakeCldrWindowsTimeZoneProvider();
        var ianaProvider = new FakeNullIanaTimeZoneProvider();

        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider, ianaProvider);

        Assert.False(result);
        Assert.Null(tz);
    }

    [Fact]
    public void TryGetSystemTimeZone_UsesDefaultCldrProvider_WhenNotProvided_OnWindows()
    {
        if (!OperatingSystem.IsWindows())
            return;

        var ianaProvider = new FakeIanaTimeZoneProvider();

        var result = CldrTimeZoneUtility.TryGetSystemTimeZone("NotSystem/UTC", territory: null, out var tz, cldrProvider: null, ianaProvider);

        Assert.True(result);
        Assert.NotNull(tz);
        Assert.Equal("UTC", tz.Id);
    }
}
