using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class GeoLocationRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GeoLocationRulesTestData.IsLatitude.Cases), MemberType = typeof(GeoLocationRulesTestData.IsLatitude))]
    public void IsLatitude_BehavesAsExpected(RuleCase<double?> tc)
    {
        // Act
        var result = GeoLocationRules.IsLatitude(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(GeoLocationRulesTestData.IsLongitude.Cases), MemberType = typeof(GeoLocationRulesTestData.IsLongitude))]
    public void IsLongitude_BehavesAsExpected(RuleCase<double?> tc)
    {
        // Act
        var result = GeoLocationRules.IsLongitude(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(GeoLocationRulesTestData.IsGeoLocation.Cases), MemberType = typeof(GeoLocationRulesTestData.IsGeoLocation))]
    public void IsGeoLocation_BehavesAsExpected(RuleCase<(double? latitude, double? longitude)> tc)
    {
        // Act
        var result = GeoLocationRules.IsGeoLocation(tc.Value.latitude, tc.Value.longitude);

        // Assert
        AssertResult(tc, result);
    }
}
