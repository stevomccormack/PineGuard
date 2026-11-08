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
    /// Validates that the string parses as a decimal with at most the specified scale.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="decimalPlaces">The maximum non-negative scale of the parsed decimal.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, overflowing, or incorrectly scaled input fails validation.
    /// Scale is measured on the parsed decimal; the input is not rounded to satisfy the requested scale.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<decimal> Decimal(this IMustClause _,
        string? value,
        int decimalPlaces = 2,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!StringUtility.NumberTypes.IsValidFloatingPointStyles(styles))
            return MustResult<decimal>.Fail(MustCodes.Number.Format.NotDecimal, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (decimalPlaces < 0)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Negative, "{paramName} requires a non-negative decimalPlaces.", nameof(decimalPlaces), decimalPlaces);

        if (value is null)
            return MustResult<decimal>.Fail(MustCodes.Number.Format.NotDecimal, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a decimal number.";

        return StringUtility.NumberTypes.TryParseDecimal(value, decimalPlaces, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<decimal>.FromBool(true, MustCodes.Number.Format.NotDecimal, messageTemplate, paramName, value, parsed)
            : MustResult<decimal>.FromBool(false, MustCodes.Number.Format.NotDecimal, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the string parses as a decimal with exactly the specified scale.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="exactDecimalPlaces">The required non-negative scale of the parsed decimal, including preserved trailing zeros.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, overflowing, or incorrectly scaled input fails validation.
    /// Scale is measured on the parsed decimal; the input is not rounded to satisfy the requested scale.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<decimal> ExactDecimal(this IMustClause _,
        string? value,
        int exactDecimalPlaces = 2,
        NumberStyles styles = DefaultStyles,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!StringUtility.NumberTypes.IsValidFloatingPointStyles(styles))
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Mismatch, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (exactDecimalPlaces < 0)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Negative, "{paramName} requires a non-negative exactDecimalPlaces.", nameof(exactDecimalPlaces), exactDecimalPlaces);

        if (value is null)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Mismatch, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be an exact decimal number.";

        return StringUtility.NumberTypes.TryParseExactDecimal(value, exactDecimalPlaces, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<decimal>.FromBool(true, MustCodes.Number.Scale.Mismatch, messageTemplate, paramName, value, parsed)
            : MustResult<decimal>.FromBool(false, MustCodes.Number.Scale.Mismatch, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the string parses as a 32-bit integer.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, or overflowing input fails validation,
    /// including for an out-of-range check. Parsing uses the invariant culture.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<int> Int32(this IMustClause _,
        string? value,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!StringUtility.NumberTypes.IsValidIntegerStyles(styles))
            return MustResult<int>.Fail(MustCodes.Number.Format.NotInt32, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (value is null)
            return MustResult<int>.Fail(MustCodes.Number.Format.NotInt32, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 32-bit integer.";

        return StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<int>.FromBool(true, MustCodes.Number.Format.NotInt32, messageTemplate, paramName, value, parsed)
            : MustResult<int>.FromBool(false, MustCodes.Number.Format.NotInt32, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the string parses as a 64-bit integer.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, or overflowing input fails validation,
    /// including for an out-of-range check. Parsing uses the invariant culture.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<long> Int64(this IMustClause _,
        string? value,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!StringUtility.NumberTypes.IsValidIntegerStyles(styles))
            return MustResult<long>.Fail(MustCodes.Number.Format.NotInt64, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (value is null)
            return MustResult<long>.Fail(MustCodes.Number.Format.NotInt64, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 64-bit integer.";

        return StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles, CultureInfo.InvariantCulture)
            ? MustResult<long>.FromBool(true, MustCodes.Number.Format.NotInt64, messageTemplate, paramName, value, parsed)
            : MustResult<long>.FromBool(false, MustCodes.Number.Format.NotInt64, messageTemplate, paramName, value, result: default);
    }

    /// <summary>
    /// Validates that the string parses as a 32-bit integer within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, or overflowing input fails validation,
    /// including for an out-of-range check. Parsing uses the invariant culture.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<int> InRangeInt32(this IMustClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!EnumRules.IsDefined<Inclusion>(inclusion))
            return MustResult<int>.Fail(MustCodes.Number.Range.OutOfRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (!StringUtility.NumberTypes.IsValidIntegerStyles(styles))
            return MustResult<int>.Fail(MustCodes.Number.Range.OutOfRange, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (min > max)
            return MustResult<int>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        if (value is null)
            return MustResult<int>.Fail(MustCodes.Number.Range.OutOfRange, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 32-bit integer within the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<int>.FromBool(false, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, result: default);

        var ok = RuleComparison.IsInRange(parsed, min, max, inclusion);
        return MustResult<int>.FromBool(ok, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the string parses as a 32-bit integer out of the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, or overflowing input fails validation,
    /// including for an out-of-range check. Parsing uses the invariant culture.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<int> OutOfRangeInt32(this IMustClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!EnumRules.IsDefined<Inclusion>(inclusion))
            return MustResult<int>.Fail(MustCodes.Number.Range.InRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (!StringUtility.NumberTypes.IsValidIntegerStyles(styles))
            return MustResult<int>.Fail(MustCodes.Number.Range.InRange, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (min > max)
            return MustResult<int>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        if (value is null)
            return MustResult<int>.Fail(MustCodes.Number.Range.InRange, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 32-bit integer out of the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<int>.FromBool(false, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, result: default);

        var ok = !RuleComparison.IsInRange(parsed, min, max, inclusion);
        return MustResult<int>.FromBool(ok, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the string parses as a 64-bit integer within the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, or overflowing input fails validation,
    /// including for an out-of-range check. Parsing uses the invariant culture.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<long> InRangeInt64(this IMustClause _,
        string? value,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!EnumRules.IsDefined<Inclusion>(inclusion))
            return MustResult<long>.Fail(MustCodes.Number.Range.OutOfRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (!StringUtility.NumberTypes.IsValidIntegerStyles(styles))
            return MustResult<long>.Fail(MustCodes.Number.Range.OutOfRange, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (min > max)
            return MustResult<long>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        if (value is null)
            return MustResult<long>.Fail(MustCodes.Number.Range.OutOfRange, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 64-bit integer within the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<long>.FromBool(false, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, result: default);

        var ok = RuleComparison.IsInRange(parsed, min, max, inclusion);
        return MustResult<long>.FromBool(ok, MustCodes.Number.Range.OutOfRange, messageTemplate, paramName, value, parsed);
    }

    /// <summary>
    /// Validates that the string parses as a 64-bit integer out of the expected range.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The numeric string to parse using the invariant culture.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The inclusion mode for range boundaries.</param>
    /// <param name="styles">The permitted numeric syntax. Undefined flags and incompatible combinations fail validation.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> containing the parsed number on success and the original input as its value.
    /// </returns>
    /// <remarks>
    /// Configuration is validated before input. Null, malformed, or overflowing input fails validation,
    /// including for an out-of-range check. Parsing uses the invariant culture.
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/string-number-types">String Number Types Must Clauses documentation</seealso>
    public static MustResult<long> OutOfRangeInt64(this IMustClause _,
        string? value,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (!EnumRules.IsDefined<Inclusion>(inclusion))
            return MustResult<long>.Fail(MustCodes.Number.Range.InRange, "{paramName} must be a defined inclusion value.", nameof(inclusion), inclusion);

        if (!StringUtility.NumberTypes.IsValidIntegerStyles(styles))
            return MustResult<long>.Fail(MustCodes.Number.Range.InRange, "{paramName} must be a valid number style.", nameof(styles), styles);

        if (min > max)
            return MustResult<long>.Fail(MustCodes.Number.Range.Invalid, InvalidRangeMessage, nameof(min), min);

        if (value is null)
            return MustResult<long>.Fail(MustCodes.Number.Range.InRange, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a 64-bit integer out of the expected range.";

        if (!StringUtility.NumberTypes.TryParseInt64(value, out var parsed, styles, CultureInfo.InvariantCulture))
            return MustResult<long>.FromBool(false, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, result: default);

        var ok = !RuleComparison.IsInRange(parsed, min, max, inclusion);
        return MustResult<long>.FromBool(ok, MustCodes.Number.Range.InRange, messageTemplate, paramName, value, parsed);
    }
}
