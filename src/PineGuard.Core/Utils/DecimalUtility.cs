namespace PineGuard.Utils;

/// <summary>
/// Provides decimal shape inspection utility methods (precision and scale).
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/decimal">Decimal Utility documentation</seealso>
public static class DecimalUtility
{
    /// <summary>
    /// Attempts to determine the precision and scale of the specified decimal value.
    /// </summary>
    /// <param name="value">The value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">
    /// When this method returns, contains the total number of digits required to store the value
    /// (the <c>p</c> of a <c>decimal(p, s)</c> column) if successful; otherwise, <c>0</c>.
    /// </param>
    /// <param name="scale">
    /// When this method returns, contains the number of digits after the decimal point
    /// (the <c>s</c> of a <c>decimal(p, s)</c> column) if successful; otherwise, <c>0</c>.
    /// </param>
    /// <returns><see langword="true"/> if the shape was determined; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// The shape is read from the numeric value, not from its representation, so trailing zeros are
    /// ignored: <c>1.500m</c> has a scale of <c>1</c> and a precision of <c>2</c>, exactly like <c>1.5m</c>.
    /// </para>
    /// <para>
    /// Precision follows the <c>decimal(p, s)</c> convention: it is the count of stored digits, so a
    /// fraction smaller than one counts the zeros that pad it out to its scale — <c>0.05m</c> has a
    /// scale of <c>2</c> and a precision of <c>2</c>, not <c>1</c>. Zero has a precision of <c>1</c>
    /// and a scale of <c>0</c>, and the sign is never counted.
    /// </para>
    /// </remarks>
    public static bool TryGetPrecisionAndScale(decimal? value, out int precision, out int scale)
    {
        precision = 0;
        scale = 0;

        if (value is null)
            return false;

        var magnitude = Math.Abs(value.Value);

        while (magnitude != decimal.Truncate(magnitude))
        {
            magnitude *= 10m;
            scale++;
        }

        var significantDigits = 1;

        while (magnitude >= 10m)
        {
            magnitude = decimal.Truncate(magnitude / 10m);
            significantDigits++;
        }

        precision = Math.Max(significantDigits, scale);
        return true;
    }
}
