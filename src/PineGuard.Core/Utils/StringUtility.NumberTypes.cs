using PineGuard.Rules;
using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class NumberTypes
    {
        public static bool TryParseInt32(
            string? value,
            out int result,
            NumberStyles styles = NumberStyles.AllowLeadingSign,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return int.TryParse(value.Trim(), styles, provider, out result);
        }

        public static bool TryParseInt64(
            string? value,
            out long result,
            NumberStyles styles = NumberStyles.AllowLeadingSign,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return long.TryParse(value.Trim(), styles, provider, out result);
        }

        public static bool TryParseDecimal(
            string? value,
            out decimal result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return decimal.TryParse(value.Trim(), styles, provider, out result);
        }

        public static bool TryParseDecimal(string? value, int decimalPlaces, out decimal result)
        {
            result = 0;

            if (value is null)
                return false;

            value = value.Trim();
            if (value.Length == 0)
                return false;

            if (decimalPlaces < 0)
                return false;

            if (decimalPlaces == 0)
            {
                if (!StringRules.NumberTypes.SignedIntegerRegex().IsMatch(value))
                    return false;

                return decimal.TryParse(
                    value,
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out result);
            }

            if (!decimal.TryParse(
                    value,
                    NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out result))
                return false;

            var bits = decimal.GetBits(result);
            var scale = (bits[3] >> 16) & 0xFF;

            return scale <= decimalPlaces;
        }

        public static bool TryParseExactDecimal(string? value, int exactDecimalPlaces, out decimal result)
        {
            result = 0;

            if (value is null)
                return false;

            value = value.Trim();
            if (value.Length == 0)
                return false;

            if (exactDecimalPlaces < 0)
                return false;

            if (exactDecimalPlaces == 0)
            {
                if (!StringRules.NumberTypes.SignedIntegerRegex().IsMatch(value))
                    return false;

                return decimal.TryParse(
                    value,
                    NumberStyles.AllowLeadingSign,
                    CultureInfo.InvariantCulture,
                    out result);
            }

            if (!decimal.TryParse(
                    value,
                    NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,
                    out result))
                return false;

            var bits = decimal.GetBits(result);
            var scale = (bits[3] >> 16) & 0xFF;

            return scale == exactDecimalPlaces;
        }

        public static bool TryParseSingle(
            string? value,
            out float result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return float.TryParse(value.Trim(), styles, provider, out result);
        }

        public static bool TryParseDouble(
            string? value,
            out double result,
            NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
            IFormatProvider? provider = null)
        {
            result = 0;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            provider ??= CultureInfo.InvariantCulture;
            return double.TryParse(value.Trim(), styles, provider, out result);
        }
    }
}