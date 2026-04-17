using FluentValidation;
using PineGuard.Testing.UnitTests.FluentValidation;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentGeoLocationExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record DoubleModel { public double Value { get; init; } }
    private sealed record NullableDoubleModel { public double? Value { get; init; } }

    private sealed class LatitudeValidator : AbstractValidator<DoubleModel> { public LatitudeValidator() => RuleFor(x => x.Value).Latitude(); }
    private sealed class LatitudeNullableValidator : AbstractValidator<NullableDoubleModel> { public LatitudeNullableValidator() => RuleFor(x => x.Value).Latitude(); }

    private sealed class LongitudeValidator : AbstractValidator<DoubleModel> { public LongitudeValidator() => RuleFor(x => x.Value).Longitude(); }
    private sealed class LongitudeNullableValidator : AbstractValidator<NullableDoubleModel> { public LongitudeNullableValidator() => RuleFor(x => x.Value).Longitude(); }

    private sealed class GeoLocationValidator : AbstractValidator<DoubleModel>
    {
        public GeoLocationValidator(double longitude) => RuleFor(x => x.Value).GeoLocation(longitude);
    }

    private sealed class GeoLocationNullableValidator : AbstractValidator<NullableDoubleModel>
    {
        public GeoLocationNullableValidator(double? longitude) => RuleFor(x => x.Value).GeoLocation(longitude);
    }

    [Theory]
    [MemberData(nameof(FluentGeoLocationExtensionsTestData.Latitude.Cases), MemberType = typeof(FluentGeoLocationExtensionsTestData.Latitude))]
    public void Latitude_BehavesAsExpected(FluentCase<double> tc)
    {
        var result = new LatitudeValidator().Validate(new DoubleModel { Value = tc.Value });
        AssertResult(tc, result);
        var resultNullable = new LatitudeNullableValidator().Validate(new NullableDoubleModel { Value = tc.Value });
        AssertResult(tc, resultNullable);
    }

    [Theory]
    [MemberData(nameof(FluentGeoLocationExtensionsTestData.Latitude.NullCases), MemberType = typeof(FluentGeoLocationExtensionsTestData.Latitude))]
    public void Latitude_Null_BehavesAsExpected(FluentCase<double?> tc)
    {
        var result = new LatitudeNullableValidator().Validate(new NullableDoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentGeoLocationExtensionsTestData.Longitude.Cases), MemberType = typeof(FluentGeoLocationExtensionsTestData.Longitude))]
    public void Longitude_BehavesAsExpected(FluentCase<double> tc)
    {
        var result = new LongitudeValidator().Validate(new DoubleModel { Value = tc.Value });
        AssertResult(tc, result);
        var resultNullable = new LongitudeNullableValidator().Validate(new NullableDoubleModel { Value = tc.Value });
        AssertResult(tc, resultNullable);
    }

    [Theory]
    [MemberData(nameof(FluentGeoLocationExtensionsTestData.Longitude.NullCases), MemberType = typeof(FluentGeoLocationExtensionsTestData.Longitude))]
    public void Longitude_Null_BehavesAsExpected(FluentCase<double?> tc)
    {
        var result = new LongitudeNullableValidator().Validate(new NullableDoubleModel { Value = tc.Value });
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FluentGeoLocationExtensionsTestData.GeoLocation.Cases), MemberType = typeof(FluentGeoLocationExtensionsTestData.GeoLocation))]
    public void GeoLocation_BehavesAsExpected(FluentCase<(double latitude, double longitude)> tc)
    {
        var result = new GeoLocationValidator(tc.Value.longitude).Validate(new DoubleModel { Value = tc.Value.latitude });
        AssertResult(tc, result);
        var resultNullable = new GeoLocationNullableValidator(tc.Value.longitude).Validate(new NullableDoubleModel { Value = tc.Value.latitude });
        AssertResult(tc, resultNullable);
    }

    [Theory]
    [MemberData(nameof(FluentGeoLocationExtensionsTestData.GeoLocation.NullCases), MemberType = typeof(FluentGeoLocationExtensionsTestData.GeoLocation))]
    public void GeoLocation_Null_BehavesAsExpected(FluentCase<(double? latitude, double? longitude)> tc)
    {
        var result = new GeoLocationNullableValidator(tc.Value.longitude).Validate(new NullableDoubleModel { Value = tc.Value.latitude });
        AssertResult(tc, result);
    }
}
