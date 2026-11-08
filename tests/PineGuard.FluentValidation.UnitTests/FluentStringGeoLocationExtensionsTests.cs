using System.Globalization;
using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringGeoLocationExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    private sealed record Model { public string? Value { get; init; } }

    private sealed class LatitudeValidator : AbstractValidator<Model>
    {
        public LatitudeValidator(NumberStyles styles = DefaultStyles) => RuleFor(x => x.Value).Latitude(styles);
    }

    private sealed class LongitudeValidator : AbstractValidator<Model>
    {
        public LongitudeValidator(NumberStyles styles = DefaultStyles) => RuleFor(x => x.Value).Longitude(styles);
    }

    private sealed class GeoLocationValidator : AbstractValidator<Model>
    {
        public GeoLocationValidator(string? longitude, NumberStyles styles = DefaultStyles) => RuleFor(x => x.Value).GeoLocation(longitude, styles);
    }

    [Theory]
    [MemberData(nameof(FluentStringGeoLocationExtensionsTestData.Styles.Cases), MemberType = typeof(FluentStringGeoLocationExtensionsTestData.Styles))]
    public void Styles_ReachTheCanonicalClauses(FluentCase<(string? value, NumberStyles styles, double parsed)> tc)
    {
        // Arrange
        var model = new Model { Value = tc.Value.value };

        // Act
        var latitude = new LatitudeValidator(tc.Value.styles).Validate(model);
        var longitude = new LongitudeValidator(tc.Value.styles).Validate(model);
        var pair = new GeoLocationValidator(tc.Value.value, tc.Value.styles).Validate(model);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
    }

    [Theory]
    [MemberData(nameof(FluentStringGeoLocationExtensionsTestData.InvalidStyles.Cases), MemberType = typeof(FluentStringGeoLocationExtensionsTestData.InvalidStyles))]
    public void Styles_RejectInvalidConfigurationBeforeNullSkipping(FluentCase<(string? value, NumberStyles styles)> tc)
    {
        // Arrange
        var model = new Model { Value = tc.Value.value };

        // Act
        var latitude = new LatitudeValidator(tc.Value.styles).Validate(model);
        var longitude = new LongitudeValidator(tc.Value.styles).Validate(model);
        var pair = new GeoLocationValidator(tc.Value.value, tc.Value.styles).Validate(model);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
    }

    [Theory]
    [MemberData(nameof(FluentStringGeoLocationExtensionsTestData.Latitude.Cases), MemberType = typeof(FluentStringGeoLocationExtensionsTestData.Latitude))]
    public void Latitude_BehavesAsExpected(FluentCase<string?> tc)
    {
        AssertResult(tc, new LatitudeValidator().Validate(new Model { Value = tc.Value }));
    }

    [Theory]
    [MemberData(nameof(FluentStringGeoLocationExtensionsTestData.Longitude.Cases), MemberType = typeof(FluentStringGeoLocationExtensionsTestData.Longitude))]
    public void Longitude_BehavesAsExpected(FluentCase<string?> tc)
    {
        AssertResult(tc, new LongitudeValidator().Validate(new Model { Value = tc.Value }));
    }

    [Theory]
    [MemberData(nameof(FluentStringGeoLocationExtensionsTestData.GeoLocation.Cases), MemberType = typeof(FluentStringGeoLocationExtensionsTestData.GeoLocation))]
    public void GeoLocation_BehavesAsExpected(FluentCase<(string? latitude, string? longitude)> tc)
    {
        AssertResult(tc, new GeoLocationValidator(tc.Value.longitude).Validate(new Model { Value = tc.Value.latitude }));
    }
}
