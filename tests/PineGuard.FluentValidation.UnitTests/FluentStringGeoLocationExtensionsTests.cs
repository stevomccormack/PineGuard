using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentStringGeoLocationExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public string? Value { get; init; } }

    private sealed class LatitudeValidator : AbstractValidator<Model>
    {
        public LatitudeValidator() => RuleFor(x => x.Value).Latitude();
    }

    private sealed class LongitudeValidator : AbstractValidator<Model>
    {
        public LongitudeValidator() => RuleFor(x => x.Value).Longitude();
    }

    private sealed class GeoLocationValidator : AbstractValidator<Model>
    {
        public GeoLocationValidator(string? longitude) => RuleFor(x => x.Value).GeoLocation(longitude);
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
