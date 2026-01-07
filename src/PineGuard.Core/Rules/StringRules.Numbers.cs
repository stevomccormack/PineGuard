using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class Numbers
    {
        public static bool IsPositive(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsPositive<decimal>(parsed);
        }

        public static bool IsNegative(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsNegative<decimal>(parsed);
        }

        public static bool IsZero(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsZero<decimal>(parsed);
        }

        public static bool IsNotZero(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsNotZero<decimal>(parsed);
        }

        public static bool IsZeroOrPositive(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsZeroOrPositive<decimal>(parsed);
        }

        public static bool IsZeroOrNegative(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsZeroOrNegative<decimal>(parsed);
        }

        public static bool IsGreaterThan(string? value, decimal min)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsGreaterThan(parsed, min);
        }

        public static bool IsGreaterThanOrEqual(string? value, decimal min)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsGreaterThanOrEqual(parsed, min);
        }

        public static bool IsLessThan(string? value, decimal max)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsLessThan(parsed, max);
        }

        public static bool IsLessThanOrEqual(string? value, decimal max)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsLessThanOrEqual(parsed, max);
        }

        public static bool IsInRange(string? value, decimal min, decimal max, Inclusion inclusion = Inclusion.Inclusive)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsInRange(parsed, min, max, inclusion);
        }

        public static bool IsApproximately(string? value, decimal target, decimal? tolerance)
        {
            if (tolerance is null)
                return false;

            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsApproximately(parsed, target, tolerance);
        }

        public static bool IsMultipleOf(string? value, decimal factor)
        {
            if (!StringUtility.NumberTypes.TryParseDecimal(value, out var parsed))
                return false;

            return NumberRules.IsMultipleOf(parsed, factor);
        }

        public static bool IsEven(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed))
                return false;

            return NumberRules.IsEven(parsed);
        }

        public static bool IsOdd(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseInt32(value, out var parsed))
                return false;

            return NumberRules.IsOdd(parsed);
        }

        public static bool IsFinite(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDouble(value, out var d))
                return false;

            return NumberRules.IsFinite(d);
        }

        public static bool IsNaN(string? value)
        {
            if (!StringUtility.NumberTypes.TryParseDouble(value, out var d))
                return false;

            return NumberRules.IsNaN(d);
        }
    }
}
