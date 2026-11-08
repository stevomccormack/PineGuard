using System.Globalization;
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
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
    /// <summary>
    /// Validates that the string value represents a valid latitude (-90 to 90).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGeoLocationClauses.Latitude"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lat).Latitude();</code></example>
    /// <seealso cref="MustStringGeoLocationClauses.Latitude"/>
    public static IRuleBuilderOptions<TModel, string?> Latitude<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, NumberStyles styles = DefaultStyles, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Latitude(val, styles, paramName: null)),
            message, MustCodes.Geo.Latitude.Invalid);

    /// <summary>
    /// Validates that the string value represents a valid longitude (-180 to 180).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGeoLocationClauses.Longitude"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lng).Longitude();</code></example>
    /// <seealso cref="MustStringGeoLocationClauses.Longitude"/>
    public static IRuleBuilderOptions<TModel, string?> Longitude<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, NumberStyles styles = DefaultStyles, string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Longitude(val, styles, paramName: null)),
            message, MustCodes.Geo.Longitude.Invalid);

    /// <summary>
    /// Validates that the string values represent a valid geographic location (latitude and longitude pair).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="longitude">The string longitude value to validate alongside the latitude.</param>
    /// <param name="styles">Numeric parsing flags; defaults to sign, decimal point and surrounding whitespace.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGeoLocationClauses.GeoLocation"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example><code>RuleFor(x => x.Lat).GeoLocation("151.2");</code></example>
    /// <seealso cref="MustStringGeoLocationClauses.GeoLocation"/>
    public static IRuleBuilderOptions<TModel, string?> GeoLocation<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? longitude,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.GeoLocation(val, longitude, styles, paramName: null)),
            message, MustCodes.Geo.Coordinate.Invalid);

    private static MustResult<T> SkipAbsentValue<T>(string? value, MustResult<T> result) =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
