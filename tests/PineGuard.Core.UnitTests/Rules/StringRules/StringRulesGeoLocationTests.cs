using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesGeoLocationTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesGeoLocationTestData.IsLatitude.Cases), MemberType = typeof(StringRulesGeoLocationTestData.IsLatitude))]
    public void IsLatitude_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.GeoLocation.IsLatitude(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGeoLocationTestData.IsLongitude.Cases), MemberType = typeof(StringRulesGeoLocationTestData.IsLongitude))]
    public void IsLongitude_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.GeoLocation.IsLongitude(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGeoLocationTestData.IsGeoLocation.Cases), MemberType = typeof(StringRulesGeoLocationTestData.IsGeoLocation))]
    public void IsGeoLocation_BehavesAsExpected(RuleCase<(string? latitude, string? longitude)> tc)
    {
        // Arrange
        var (latitude, longitude) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.GeoLocation.IsGeoLocation(latitude, longitude);

        // Assert
        AssertResult(tc, result);
    }
}
