#if NET8_0_OR_GREATER
using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string-based geographic coordinate validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-geo-location">Fluent String Geo Location Extensions documentation</seealso>
public static class FluentStringGeoLocationExtensions
{
    /// <summary>
    /// Validates that the string value represents a valid latitude (-90 to 90).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustGeoLocationClauses.Latitude"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lat).Latitude();</code></example>
    /// <seealso cref="MustGeoLocationClauses.Latitude"/>
    public static IRuleBuilderOptions<TModel, string?> Latitude<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Latitude(val, paramName: null) : MustResult<double>.Ok(0),
            message, MustCodes.Geo.Latitude.Invalid);

    /// <summary>
    /// Validates that the string value represents a valid longitude (-180 to 180).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustGeoLocationClauses.Longitude"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lng).Longitude();</code></example>
    /// <seealso cref="MustGeoLocationClauses.Longitude"/>
    public static IRuleBuilderOptions<TModel, string?> Longitude<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Longitude(val, paramName: null) : MustResult<double>.Ok(0),
            message, MustCodes.Geo.Longitude.Invalid);

    /// <summary>
    /// Validates that the string values represent a valid geographic location (latitude and longitude pair).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="longitude">The string longitude value to validate alongside the latitude.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustGeoLocationClauses.GeoLocation"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lat).GeoLocation(x => x.Lng);</code></example>
    /// <seealso cref="MustGeoLocationClauses.GeoLocation"/>
    public static IRuleBuilderOptions<TModel, string?> GeoLocation<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? longitude,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.GeoLocation(val, longitude, paramName: null) : MustResult<(double, double)>.Ok(default),
            message, MustCodes.Geo.Coordinate.Invalid);
}
#endif
