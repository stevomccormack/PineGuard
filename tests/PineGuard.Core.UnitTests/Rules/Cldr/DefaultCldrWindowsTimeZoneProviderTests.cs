using System.Collections.Frozen;
using System.Reflection;
using PineGuard.Externals.Cldr.TimeZones;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Cldr;

public sealed class DefaultCldrWindowsTimeZoneProviderTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString.EdgeCases), MemberType = typeof(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString))]
    public void IsValidWindowsTimeZoneId_ReturnsFalse_ForNullOrWhitespace(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString.Case testCase)
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;

        // Act
        var result = provider.IsValidWindowsTimeZoneId(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Fact]
    public void IsValidWindowsTimeZoneId_ReturnsFalse_ForUnknownId()
    {
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;

        var result = provider.IsValidWindowsTimeZoneId("Definitely Not A Time Zone");

        Assert.False(result);
    }

    [Fact]
    public void IsValidWindowsTimeZoneId_ReturnsTrue_ForKnownId_AndTrims()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var windowsId = GetDeterministicWindowsTimeZoneIdWithDefaultTerritory();

        // Act
        var result = provider.IsValidWindowsTimeZoneId($"  {windowsId}  ");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryGetIanaTimeZoneIds_UsesDefaultTerritory_WhenTerritoryIsNull()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var windowsId = GetDeterministicWindowsTimeZoneIdWithDefaultTerritory();

        // Act
        var found = provider.TryGetIanaTimeZoneIds(windowsId, territory: null, out var ianaIds);

        // Assert
        Assert.True(found);
        Assert.NotEmpty(ianaIds);
    }

    [Fact]
    public void TryGetIanaTimeZoneIds_UsesDefaultTerritory_WhenTerritoryIsWhitespace_AndTrimsInputs()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var windowsId = GetDeterministicWindowsTimeZoneIdWithDefaultTerritory();

        // Act
        var found = provider.TryGetIanaTimeZoneIds($"  {windowsId}  ", territory: "  ", out var ianaIds);

        // Assert
        Assert.True(found);
        Assert.NotEmpty(ianaIds);
    }

    [Theory]
    [MemberData(nameof(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString.EdgeCases), MemberType = typeof(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString))]
    public void TryGetIanaTimeZoneIds_ReturnsFalse_ForNullOrWhitespaceWindowsId(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString.Case testCase)
    {
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;

        var found = provider.TryGetIanaTimeZoneIds(testCase.Value, territory: null, out var ianaIds);

        Assert.False(found);
        Assert.Empty(ianaIds);
    }

    [Fact]
    public void TryGetIanaTimeZoneIds_ReturnsFalse_ForUnknownWindowsId()
    {
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;

        var found = provider.TryGetIanaTimeZoneIds("Definitely Not A Time Zone", territory: null, out var ianaIds);

        Assert.False(found);
        Assert.Empty(ianaIds);
    }

    [Fact]
    public void TryGetIanaTimeZoneIds_FallsBackToDefaultTerritory_WhenTerritoryMissing()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var windowsId = GetDeterministicWindowsTimeZoneIdWithDefaultTerritory();

        // Act
        var found = provider.TryGetIanaTimeZoneIds(windowsId, territory: "ZZ", out var ianaIds);

        // Assert
        Assert.True(found);
        Assert.NotEmpty(ianaIds);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_ReturnsAValidWindowsId_ThatMapsBackToTheIanaId()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var (windowsId, ianaId) = GetDeterministicWindowsAndIanaId();

        // Act
        var found = provider.TryGetWindowsTimeZoneId(ianaId, territory: DefaultCldrWindowsTimeZoneProvider.DefaultTerritory, out var returnedWindowsId);

        // Assert
        Assert.True(found);
        Assert.True(provider.IsValidWindowsTimeZoneId(returnedWindowsId));

        Assert.True(provider.TryGetIanaTimeZoneIds(returnedWindowsId, DefaultCldrWindowsTimeZoneProvider.DefaultTerritory, out var returnedIanaIds));
        Assert.Contains(ianaId, returnedIanaIds);

        Assert.True(provider.IsValidWindowsTimeZoneId(windowsId));
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_FallsBackToDefaultTerritory_WhenTerritoryMissing()
    {
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var (_, ianaId) = GetDeterministicWindowsAndIanaId();

        var found = provider.TryGetWindowsTimeZoneId(ianaId, territory: "ZZ", out var returnedWindowsId);

        Assert.True(found);
        Assert.True(provider.IsValidWindowsTimeZoneId(returnedWindowsId));

        Assert.True(provider.TryGetIanaTimeZoneIds(returnedWindowsId, DefaultCldrWindowsTimeZoneProvider.DefaultTerritory, out var returnedIanaIds));
        Assert.Contains(ianaId, returnedIanaIds);
    }

    [Theory]
    [MemberData(nameof(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString.EdgeCases), MemberType = typeof(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString))]
    public void TryGetWindowsTimeZoneId_ReturnsFalse_ForNullOrWhitespaceIanaId(DefaultCldrWindowsTimeZoneProviderTestData.NullOrWhitespaceString.Case testCase)
    {
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;

        var found = provider.TryGetWindowsTimeZoneId(testCase.Value, territory: null, out var windowsId);

        Assert.False(found);
        Assert.Equal(string.Empty, windowsId);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_ReturnsFalse_ForUnknownIanaId()
    {
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;

        var found = provider.TryGetWindowsTimeZoneId("Definitely/Not_A_Time_Zone", territory: null, out var windowsId);

        Assert.False(found);
        Assert.Equal(string.Empty, windowsId);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_UsesDefaultTerritory_WhenTerritoryIsWhitespace_AndTrimsInputs()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var (_, ianaId) = GetDeterministicWindowsAndIanaId();

        // Act
        var found = provider.TryGetWindowsTimeZoneId($"  {ianaId}  ", territory: "  ", out var returnedWindowsId);

        // Assert
        Assert.True(found);
        Assert.True(provider.IsValidWindowsTimeZoneId(returnedWindowsId));
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_UsesLastResort_WhenNoDefaultTerritoryMappingExistsForIanaId()
    {
        // Arrange
        var provider = DefaultCldrWindowsTimeZoneProvider.Instance;
        var (ianaId, expectedWindowsIds) = GetDeterministicIanaIdWithoutDefaultTerritoryMapping();

        // Act
        var found = provider.TryGetWindowsTimeZoneId(ianaId, territory: "ZZ", out var returnedWindowsId);

        // Assert
        Assert.True(found);
        Assert.True(provider.IsValidWindowsTimeZoneId(returnedWindowsId));
        Assert.Contains(returnedWindowsId, expectedWindowsIds, StringComparer.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildWindowsIdByIanaIdAndTerritoryIndex_PrefersLexicographicallySmallestWindowsId_WhenDuplicatesExist()
    {
        var windowsToTerritoryToIana = new Dictionary<string, FrozenDictionary<string, string[]>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Zulu Time"] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["US"] = new[] { "America/Foo" },
            }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase),
            ["Alpha Time"] = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                ["US"] = new[] { "America/Foo" },
            }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase),
        };

        var index = DefaultCldrWindowsTimeZoneProvider.BuildWindowsIdByIanaIdAndTerritoryIndex(windowsToTerritoryToIana);

        Assert.True(index.TryGetValue("America/Foo", out var byTerritory));
        Assert.True(byTerritory.TryGetValue("US", out var windowsId));
        Assert.Equal("Alpha Time", windowsId);
    }

    [Fact]
    public void TryGetIanaTimeZoneIds_ReturnsFalse_WhenTerritoryMissing_AndNoDefaultTerritoryExists()
    {
        var byTerritory = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["US"] = new[] { "America/Los_Angeles" },
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var index = new Dictionary<string, FrozenDictionary<string, string[]>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Pacific Standard Time"] = byTerritory,
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var found = DefaultCldrWindowsTimeZoneProvider.TryGetIanaTimeZoneIds(
            index,
            windowsTimeZoneId: "Pacific Standard Time",
            territory: "ZZ",
            out var ianaIds);

        Assert.False(found);
        Assert.Empty(ianaIds);
    }

    [Fact]
    public void TryGetIanaTimeZoneIds_ReturnsFalse_WhenTerritoryResolvesToDefault_AndDefaultTerritoryIsMissing()
    {
        var byTerritory = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["US"] = new[] { "America/Los_Angeles" },
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var index = new Dictionary<string, FrozenDictionary<string, string[]>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Pacific Standard Time"] = byTerritory,
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var found = DefaultCldrWindowsTimeZoneProvider.TryGetIanaTimeZoneIds(
            index,
            windowsTimeZoneId: "Pacific Standard Time",
            territory: null,
            out var ianaIds);

        Assert.False(found);
        Assert.Empty(ianaIds);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_ReturnsFalse_WhenTerritoryMapIsEmpty()
    {
        var emptyByTerritory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            .ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var index = new Dictionary<string, FrozenDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["America/Empty"] = emptyByTerritory,
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var found = DefaultCldrWindowsTimeZoneProvider.TryGetWindowsTimeZoneId(
            index,
            ianaTimeZoneId: "America/Empty",
            territory: "ZZ",
            out var windowsId);

        Assert.False(found);
        Assert.Equal(string.Empty, windowsId);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_UsesLastResort_WhenNoTerritoryAndNoDefaultMappingExists()
    {
        var byTerritory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["CA"] = "Pacific Standard Time",
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var index = new Dictionary<string, FrozenDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["America/Vancouver"] = byTerritory,
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var found = DefaultCldrWindowsTimeZoneProvider.TryGetWindowsTimeZoneId(
            index,
            ianaTimeZoneId: "America/Vancouver",
            territory: "ZZ",
            out var windowsId);

        Assert.True(found);
        Assert.Equal("Pacific Standard Time", windowsId);
    }

    [Fact]
    public void TryGetWindowsTimeZoneId_UsesLastResort_WhenTerritoryResolvesToDefault_AndDefaultTerritoryIsMissing()
    {
        var byTerritory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["CA"] = "Pacific Standard Time",
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var index = new Dictionary<string, FrozenDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["America/Vancouver"] = byTerritory,
        }.ToFrozenDictionary(kvp => kvp.Key, kvp => kvp.Value, StringComparer.OrdinalIgnoreCase);

        var found = DefaultCldrWindowsTimeZoneProvider.TryGetWindowsTimeZoneId(
            index,
            ianaTimeZoneId: "America/Vancouver",
            territory: null,
            out var windowsId);

        Assert.True(found);
        Assert.Equal("Pacific Standard Time", windowsId);
    }

    private static string GetDeterministicWindowsTimeZoneIdWithDefaultTerritory()
    {
        var mappings = GetWindowsToTerritoryToIanaMap();

        var candidate = mappings
            .Where(kvp => kvp.Value.ContainsKey(DefaultCldrWindowsTimeZoneProvider.DefaultTerritory))
            .Select(kvp => kvp.Key)
            .OrderBy(k => k, StringComparer.OrdinalIgnoreCase)
            .First();

        return candidate;
    }

    private static (string WindowsId, string IanaId) GetDeterministicWindowsAndIanaId()
    {
        var mappings = GetWindowsToTerritoryToIanaMap();

        var windowsId = GetDeterministicWindowsTimeZoneIdWithDefaultTerritory();
        var byTerritory = mappings[windowsId];
        var ianaIds = byTerritory[DefaultCldrWindowsTimeZoneProvider.DefaultTerritory];

        var ianaId = ianaIds
            .OrderBy(i => i, StringComparer.OrdinalIgnoreCase)
            .First();

        return (windowsId, ianaId);
    }

    private static Dictionary<string, Dictionary<string, string[]>> GetWindowsToTerritoryToIanaMap()
    {
        var assembly = typeof(DefaultCldrWindowsTimeZoneProvider).Assembly;
        var dataType = assembly.GetType("PineGuard.Cldr.TimeZones.DefaultCldrWindowsTimeZoneData", throwOnError: true)!;

        var field = dataType.GetField("IanaTimeZoneIdsByWindowsId", BindingFlags.NonPublic | BindingFlags.Static);
        Assert.NotNull(field);

        var frozen = field!.GetValue(null);
        Assert.NotNull(frozen);

        // Avoid binding directly to FrozenDictionary in tests; treat it as dictionaries for ordering logic.
        var result = new Dictionary<string, Dictionary<string, string[]>>(StringComparer.OrdinalIgnoreCase);

        foreach (var winKvp in (System.Collections.IEnumerable)frozen!)
        {
            var winKvpType = winKvp.GetType();
            var windowsId = (string)winKvpType.GetProperty("Key")!.GetValue(winKvp)!;
            var territoryMapFrozen = winKvpType.GetProperty("Value")!.GetValue(winKvp)!;

            var territoryMap = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            foreach (var terrKvp in (System.Collections.IEnumerable)territoryMapFrozen)
            {
                var terrKvpType = terrKvp.GetType();
                var territory = (string)terrKvpType.GetProperty("Key")!.GetValue(terrKvp)!;
                var ianaIds = (string[])terrKvpType.GetProperty("Value")!.GetValue(terrKvp)!;
                territoryMap[territory] = ianaIds;
            }

            result[windowsId] = territoryMap;
        }

        return result;
    }

    private static (string IanaId, string[] WindowsIds) GetDeterministicIanaIdWithoutDefaultTerritoryMapping()
    {
        var windowsToTerritoryToIana = GetWindowsToTerritoryToIanaMap();

        // Build: IANA id -> territory -> windows id (mirrors production logic, but kept test-local).
        var ianaToTerritoryToWindows = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var winKvp in windowsToTerritoryToIana)
        {
            var windowsId = winKvp.Key;
            var byTerritory = winKvp.Value;

            foreach (var terrKvp in byTerritory)
            {
                var territory = terrKvp.Key;
                var ianaIds = terrKvp.Value;

                for (var i = 0; i < ianaIds.Length; i++)
                {
                    var ianaId = ianaIds[i];

                    if (!ianaToTerritoryToWindows.TryGetValue(ianaId, out var terrMap))
                    {
                        terrMap = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        ianaToTerritoryToWindows[ianaId] = terrMap;
                    }

                    // Deterministic selection matches provider behavior.
                    if (terrMap.TryGetValue(territory, out var existing))
                    {
                        if (string.Compare(existing, windowsId, StringComparison.OrdinalIgnoreCase) > 0)
                            terrMap[territory] = windowsId;
                    }
                    else
                    {
                        terrMap[territory] = windowsId;
                    }
                }
            }
        }

        var candidate = ianaToTerritoryToWindows
            .Where(kvp => !kvp.Value.ContainsKey(DefaultCldrWindowsTimeZoneProvider.DefaultTerritory))
            .OrderBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();

        Assert.False(string.IsNullOrWhiteSpace(candidate.Key));

        var windowsIds = candidate.Value
            .Select(kvp => kvp.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(w => w, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        Assert.NotEmpty(windowsIds);

        return (candidate.Key, windowsIds);
    }
}
