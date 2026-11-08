using System.Globalization;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesGeoLocationTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesGeoLocationTestData.Styles.Cases), MemberType = typeof(StringRulesGeoLocationTestData.Styles))]
    public void Styles_AreAppliedWithInvariantCulture(RuleCase<(string? value, NumberStyles styles, double parsed)> tc)
    {
        // Arrange
        using var culture = UseCulture("fr-FR");

        // Act
        var latitude = PineGuard.Rules.StringRules.GeoLocation.IsLatitude(value: tc.Value.value, styles: tc.Value.styles);
        var longitude = PineGuard.Rules.StringRules.GeoLocation.IsLongitude(value: tc.Value.value, styles: tc.Value.styles);
        var pair = PineGuard.Rules.StringRules.GeoLocation.IsGeoLocation(tc.Value.value, tc.Value.value, tc.Value.styles);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
    }

    [Theory]
    [MemberData(nameof(StringRulesGeoLocationTestData.InvalidStyles.Cases), MemberType = typeof(StringRulesGeoLocationTestData.InvalidStyles))]
    public void Styles_InvalidConfigurationReturnsFalse(RuleCase<(string? value, NumberStyles styles)> tc)
    {
        // Act
        var latitude = PineGuard.Rules.StringRules.GeoLocation.IsLatitude(tc.Value.value, tc.Value.styles);
        var longitude = PineGuard.Rules.StringRules.GeoLocation.IsLongitude(tc.Value.value, tc.Value.styles);
        var pair = PineGuard.Rules.StringRules.GeoLocation.IsGeoLocation(tc.Value.value, tc.Value.value, tc.Value.styles);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
    }

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
