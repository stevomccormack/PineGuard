#if NET8_0_OR_GREATER
using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for geographic coordinate property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/geo-location">Fluent Geo Location Extensions documentation</seealso>
public static class FluentGeoLocationExtensions
{
    /// <summary>
    /// Validates that the nullable <see cref="double"/> value is a valid latitude (-90 to 90).
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
    public static IRuleBuilderOptions<TModel, double?> Latitude<TModel>(
        this IRuleBuilder<TModel, double?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val =>
        {
            if (!val.HasValue)
                return MustResult<double?>.Ok(null);

            var res = Must.Be.Latitude(val.Value, paramName: null);
            return MustResult<double?>.FromBool(res.Success, res.Message, res.ParamName, val, res.Result);
        }, message, MustCodes.Geo.Latitude.Invalid);

    /// <summary>
    /// Validates that the <see cref="double"/> value is a valid latitude (-90 to 90).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustGeoLocationClauses.Latitude"/>.</remarks>
    /// <example><code>RuleFor(x => x.Lat).Latitude();</code></example>
    /// <seealso cref="MustGeoLocationClauses.Latitude"/>
    public static IRuleBuilderOptions<TModel, double> Latitude<TModel>(
        this IRuleBuilder<TModel, double> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Latitude(val, paramName: null),
            message, MustCodes.Geo.Latitude.Invalid);

    /// <summary>
    /// Validates that the nullable <see cref="double"/> value is a valid longitude (-180 to 180).
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
    public static IRuleBuilderOptions<TModel, double?> Longitude<TModel>(
        this IRuleBuilder<TModel, double?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val =>
        {
            if (!val.HasValue) return MustResult<double?>.Ok(null);
            var res = Must.Be.Longitude(val.Value, paramName: null);
            return MustResult<double?>.FromBool(res.Success, res.Message, res.ParamName, val, res.Result);
        }, message, MustCodes.Geo.Longitude.Invalid);

    /// <summary>
    /// Validates that the <see cref="double"/> value is a valid longitude (-180 to 180).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustGeoLocationClauses.Longitude"/>.</remarks>
    /// <example><code>RuleFor(x => x.Lng).Longitude();</code></example>
    /// <seealso cref="MustGeoLocationClauses.Longitude"/>
    public static IRuleBuilderOptions<TModel, double> Longitude<TModel>(
        this IRuleBuilder<TModel, double> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Longitude(val, paramName: null),
            message, MustCodes.Geo.Longitude.Invalid);

    /// <summary>
    /// Validates that the nullable latitude and longitude pair form a valid geographic location.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="longitude">The longitude value to validate alongside the latitude.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustGeoLocationClauses.GeoLocation"/>. If either value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lat).GeoLocation(x => x.Lng);</code></example>
    /// <seealso cref="MustGeoLocationClauses.GeoLocation"/>
    public static IRuleBuilderOptions<TModel, double?> GeoLocation<TModel>(
        this IRuleBuilder<TModel, double?> ruleBuilder,
        double? longitude,
        string? message = null) =>
        ruleBuilder.MustBe(val =>
        {
            if (!val.HasValue || !longitude.HasValue)
                return MustResult<double?>.Ok(null);

            var res = Must.Be.GeoLocation(val.Value, longitude.Value, paramName: null);
            return MustResult<double?>.FromBool(res.Success, res.Message, res.ParamName, val, val.Value);
        }, message, MustCodes.Geo.Coordinate.Invalid);

    /// <summary>
    /// Validates that the latitude and longitude pair form a valid geographic location.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="longitude">The longitude value to validate alongside the latitude.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>Delegates to <see cref="MustGeoLocationClauses.GeoLocation"/>.</remarks>
    /// <example><code>RuleFor(x => x.Lat).GeoLocation(x => x.Lng);</code></example>
    /// <seealso cref="MustGeoLocationClauses.GeoLocation"/>
    public static IRuleBuilderOptions<TModel, double> GeoLocation<TModel>(
        this IRuleBuilder<TModel, double> ruleBuilder,
        double longitude,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.GeoLocation(val, longitude, paramName: null),
            message, MustCodes.Geo.Coordinate.Invalid);
}
#endif
