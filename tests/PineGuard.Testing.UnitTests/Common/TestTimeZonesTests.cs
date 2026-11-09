using PineGuard.Testing.Common;
using Xunit.Abstractions;

namespace PineGuard.Testing.UnitTests.Common;

public sealed class TestTimeZonesTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TestTimeZonesTestData.Zones_HaveExpectedShape.ValidCases), MemberType = typeof(TestTimeZonesTestData.Zones_HaveExpectedShape))]
    public void Zones_HaveExpectedShape(TestTimeZonesTestData.Zones_HaveExpectedShape.Case tc)
    {
        // Act
        var actual = (baseUtcOffset: tc.Value.BaseUtcOffset, supportsDaylightSavingTime: tc.Value.SupportsDaylightSavingTime);

        // Assert
        Assert.Equal(tc.Expected.baseUtcOffset, actual.baseUtcOffset);
        Assert.Equal(tc.Expected.supportsDaylightSavingTime, actual.supportsDaylightSavingTime);
    }

    [Theory]
    [MemberData(nameof(TestTimeZonesTestData.DaylightSaving_ClassifiesLocalTimes.ValidCases), MemberType = typeof(TestTimeZonesTestData.DaylightSaving_ClassifiesLocalTimes))]
    public void DaylightSaving_ClassifiesLocalTimes(TestTimeZonesTestData.DaylightSaving_ClassifiesLocalTimes.Case tc)
    {
        // Arrange
        var (zone, localTime) = tc.Value;

        // Act
        var isInvalid = zone.IsInvalidTime(localTime);
        var isAmbiguous = zone.IsAmbiguousTime(localTime);

        // Assert
        Assert.Equal(tc.Expected.isInvalid, isInvalid);
        Assert.Equal(tc.Expected.isAmbiguous, isAmbiguous);
    }

    [Theory]
    [MemberData(nameof(TestTimeZonesTestData.GetPlatformSampleId.ValidCases), MemberType = typeof(TestTimeZonesTestData.GetPlatformSampleId))]
    public void GetPlatformSampleId_BehavesAsExpected(TestTimeZonesTestData.GetPlatformSampleId.Case tc)
    {
        // Act
        var result = TestTimeZones.GetPlatformSampleId(tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    /// <summary>
    /// Host integration test: requires the platform's time zone database to contain
    /// <see cref="TestTimeZones.PlatformSampleId"/> (a Windows id on Windows, an IANA id elsewhere).
    /// Fails, rather than skips, on a host that cannot resolve it; the id is a stated prerequisite of
    /// every test that resolves a real zone. The unknown-id row is deterministic on every host.
    /// </summary>
    [Theory]
    [MemberData(nameof(TestTimeZonesTestData.PlatformSampleId_Resolves.ValidCases), MemberType = typeof(TestTimeZonesTestData.PlatformSampleId_Resolves))]
    public void PlatformSampleId_Resolves(TestTimeZonesTestData.PlatformSampleId_Resolves.Case tc)
    {
        // Act & Assert
        if (tc.Expected)
        {
            Assert.NotNull(TimeZoneInfo.FindSystemTimeZoneById(tc.Value));
        }
        else
        {
            Assert.Throws<TimeZoneNotFoundException>(() => TimeZoneInfo.FindSystemTimeZoneById(tc.Value));
        }
    }

    /// <summary>
    /// Host integration test: requires the platform's time zone database to contain
    /// <see cref="TestTimeZones.PlatformSampleId"/> (a Windows id on Windows, an IANA id elsewhere).
    /// Fails, rather than skips, on a host that cannot resolve it; the id is a stated prerequisite of
    /// every test that resolves a real zone.
    /// </summary>
    [Theory]
    [MemberData(nameof(TestTimeZonesTestData.PlatformSample_ClassifiesLocalTimes.ValidCases), MemberType = typeof(TestTimeZonesTestData.PlatformSample_ClassifiesLocalTimes))]
    public void PlatformSample_ClassifiesLocalTimes(TestTimeZonesTestData.PlatformSample_ClassifiesLocalTimes.Case tc)
    {
        // Arrange
        var zone = TimeZoneInfo.FindSystemTimeZoneById(TestTimeZones.PlatformSampleId);

        // Act
        var isInvalid = zone.IsInvalidTime(tc.Value);
        var isAmbiguous = zone.IsAmbiguousTime(tc.Value);

        // Assert
        Assert.Equal(tc.Expected.isInvalid, isInvalid);
        Assert.Equal(tc.Expected.isAmbiguous, isAmbiguous);
    }
}
