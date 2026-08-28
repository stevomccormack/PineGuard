#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate numeric type string representations,
/// parsing the input string before delegating to number rules.
/// </summary>
/// <seealso cref="NumberRules"/>
/// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
public static class MustStringNumberTypesClauses
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    private const string NullMessage = "{paramName} must not be null.";

    private const string InvalidRangeMessage = "{paramName} requires a valid range.";

    /// <summary>
    /// Validates that the specified value requires a non-negative decimalPlaces.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="decimalPlaces">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative decimalPlaces."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<decimal> Decimal(this IMustClause _,
        string? value,
        int decimalPlaces = 2,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail(MustCodes.Number.Format.NotDecimal, NullMessage, paramName, value);

        if (decimalPlaces < 0)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Negative, "{paramName} requires a non-negative decimalPlaces.", nameof(decimalPlaces), decimalPlaces);

        const string messageTemplate = "{paramName} must be a decimal number.";

        return StringUtility.NumberTypes.TryParseDecimal(value, decimalPlaces, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<decimal>.FromBool(true, MustCodes.Number.Format.NotDecimal, messageTemplate, paramName, value, parsed)
            : MustResult<decimal>.FromBool(false, MustCodes.Number.Format.NotDecimal, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the specified value requires a non-negative exactDecimalPlaces.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="exactDecimalPlaces">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} requires a non-negative exactDecimalPlaces."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<decimal> ExactDecimal(this IMustClause _,
        string? value,
        int exactDecimalPlaces = 2,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Mismatch, NullMessage, paramName, value);

        if (exactDecimalPlaces < 0)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Negative, "{paramName} requires a non-negative exactDecimalPlaces.", nameof(exactDecimalPlaces), exactDecimalPlaces);

        const string messageTemplate = "{paramName} must be an exact decimal number.";

        return StringUtility.NumberTypes.TryParseExactDecimal(value, exactDecimalPlaces, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<decimal>.FromBool(true, MustCodes.Number.Scale.Mismatch, messageTemplate, paramName, value, parsed)
            : MustResult<decimal>.FromBool(false, MustCodes.Number.Scale.Mismatch, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the specified value must be a 32-bit integer.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a 32-bit integer."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<int> Int32(this IMustClause _,
        string? value,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<int>.Fail(MustCodes.Number.Format.NotInt32, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 32-bit integer.";

        return StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<int>.FromBool(true, MustCodes.Number.Format.NotInt32, messageTemplate, paramName, value, parsed)
            : MustResult<int>.FromBool(false, MustCodes.Number.Format.NotInt32, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the specified value must be a 64-bit integer.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a 64-bit integer."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<long> Int64(this IMustClause _,
        string? value,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<long>.Fail(MustCodes.Number.Format.NotInt64, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 64-bit integer.";

        return StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<long>.FromBool(true, MustCodes.Number.Format.NotInt64, messageTemplate, paramName, value, parsed)
            : MustResult<long>.FromBool(false, MustCodes.Number.Format.NotInt64, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the specified value must be a 32-bit integer within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a 32-bit integer within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<int> Int32InRange(this IMustClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<int>.Fail(MustCodes.Number.Range.OutOfRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<int>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must be a 32-bit integer within the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<int>.FromBool(false, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, result: default);

        var ok = NumberRules.IsInRange(parsed, min, max, inclusion);
        return MustResult<int>.FromBool(ok, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must be a 32-bit integer out of the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a 32-bit integer out of the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<int> Int32OutOfRange(this IMustClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<int>.Fail(MustCodes.Number.Range.InRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<int>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must be a 32-bit integer out of the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<int>.FromBool(false, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, result: default);

        var ok = !NumberRules.IsInRange(parsed, min, max, inclusion);
        return MustResult<int>.FromBool(ok, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must be a 64-bit integer within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a 64-bit integer within the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<long> Int64InRange(this IMustClause _,
        string? value,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<long>.Fail(MustCodes.Number.Range.OutOfRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<long>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must be a 64-bit integer within the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<long>.FromBool(false, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, result: default);

        var ok = NumberRules.IsInRange(parsed, min, max, inclusion);
        return MustResult<long>.FromBool(ok, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the specified value must be a 64-bit integer out of the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a 64-bit integer out of the expected range."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<long> Int64OutOfRange(this IMustClause _,
        string? value,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<long>.Fail(MustCodes.Number.Range.InRange, NullMessage, paramName, value);

        if (min > max)
            return MustResult<long>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        const string messageTemplate = "{paramName} must be a 64-bit integer out of the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<long>.FromBool(false, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, result: default);

        var ok = !NumberRules.IsInRange(parsed, min, max, inclusion);
        return MustResult<long>.FromBool(ok, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, parsed);
    }
}
#endif
