using System.Globalization;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations.Common;

/// <summary>Parses attribute-compatible decimal configuration through the canonical clause.</summary>
internal static class DecimalAttributeConfiguration
{
    internal static MustResult<decimal> Parse(string? value, string paramName) =>
        Must.Be.Decimal(value, decimalPlaces: int.MaxValue, styles: NumberStyles.Float, paramName: paramName);
}
