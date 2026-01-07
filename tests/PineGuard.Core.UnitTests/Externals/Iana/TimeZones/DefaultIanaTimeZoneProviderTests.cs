using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iana.TimeZones;

public sealed class DefaultIanaTimeZoneProviderTests : BaseUnitTest
{
    [Fact]
    public void Instance_UsesEmbeddedDataset()
    {
        // Arrange
        var provider = DefaultIanaTimeZoneProvider.Instance;

        // Act
        var hasAny = provider.GetAll().Count > 0;
        var isValidKnown = provider.IsValidTimeZoneId("Africa/Abidjan");

        // Assert
        Assert.True(hasAny);
        Assert.True(isValidKnown);
    }

    [Theory]
    [MemberData(nameof(DefaultIanaTimeZoneProviderTestData.IsValidTimeZoneId.ValidCases), MemberType = typeof(DefaultIanaTimeZoneProviderTestData.IsValidTimeZoneId))]
    [MemberData(nameof(DefaultIanaTimeZoneProviderTestData.IsValidTimeZoneId.EdgeCases), MemberType = typeof(DefaultIanaTimeZoneProviderTestData.IsValidTimeZoneId))]
    public void IsValidTimeZoneId_ReturnsExpected(DefaultIanaTimeZoneProviderTestData.IsValidTimeZoneId.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIanaTimeZoneProviderTestData.CreateSampleProvider();

        // Act
        var result = provider.IsValidTimeZoneId(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DefaultIanaTimeZoneProviderTestData.TryGetById.ValidCases), MemberType = typeof(DefaultIanaTimeZoneProviderTestData.TryGetById))]
    [MemberData(nameof(DefaultIanaTimeZoneProviderTestData.TryGetById.EdgeCases), MemberType = typeof(DefaultIanaTimeZoneProviderTestData.TryGetById))]
    public void TryGetById_ReturnsExpected(DefaultIanaTimeZoneProviderTestData.TryGetById.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIanaTimeZoneProviderTestData.CreateSampleProvider();

        // Act
        var result = provider.TryGetById(testCase.Value, out var tz);

        // Assert
        Assert.Equal(testCase.Expected, result);

        if (testCase.Expected)
        {
            Assert.NotNull(tz);
            Assert.Equal(testCase.ExpectedId, tz!.Id);
        }
        else
        {
            Assert.Null(tz);
            Assert.Null(testCase.ExpectedId);
        }
    }

    [Fact]
    public void GetAll_ReturnsAllZones()
    {
        // Arrange
        var provider = DefaultIanaTimeZoneProviderTestData.CreateSampleProvider();

        // Act
        var all = provider.GetAll();

        // Assert
        Assert.Equal(2, all.Count);
        Assert.Contains(DefaultIanaTimeZoneProviderTestData.SampleZoneEuropeLondon, all);
        Assert.Contains(DefaultIanaTimeZoneProviderTestData.SampleZoneAmericaNewYork, all);
    }

    [Theory]
    [MemberData(nameof(DefaultIanaTimeZoneProviderTestData.TryGetTimeZoneIdsByCountryAlpha2Code.ValidCases), MemberType = typeof(DefaultIanaTimeZoneProviderTestData.TryGetTimeZoneIdsByCountryAlpha2Code))]
    [MemberData(nameof(DefaultIanaTimeZoneProviderTestData.TryGetTimeZoneIdsByCountryAlpha2Code.EdgeCases), MemberType = typeof(DefaultIanaTimeZoneProviderTestData.TryGetTimeZoneIdsByCountryAlpha2Code))]
    public void TryGetTimeZoneIdsByCountryAlpha2Code_ReturnsExpected(DefaultIanaTimeZoneProviderTestData.TryGetTimeZoneIdsByCountryAlpha2Code.ValidCase testCase)
    {
        // Arrange
        var provider = DefaultIanaTimeZoneProviderTestData.CreateSampleProvider();

        // Act
        var result = provider.TryGetTimeZoneIdsByCountryAlpha2Code(testCase.Value, out var ids);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedIds, ids);
    }
}
