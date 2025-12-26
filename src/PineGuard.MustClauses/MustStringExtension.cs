using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PineGuard.MustClauses;

public static partial class MustStringExtension
{
    public const string DigitsOnlyPattern = "^[0-9]+$";
    public const string NumericPattern = "^[+-]?[0-9]+$";
    public const string DecimalPattern = "^[+-]?[0-9]+(\\.[0-9]+)?$";
    public const string AlphaPattern = "^[A-Za-z]+$";
    public const string AlphaNumericPattern = "^[A-Za-z0-9]+$";

    [GeneratedRegex(AlphaPattern, RegexOptions.Compiled)]
    private static partial Regex AlphaRegex();

    [GeneratedRegex(NumericPattern, RegexOptions.Compiled)]
    private static partial Regex NumericRegex();

    public static MustResult NotNullOrEmpty(
        this IMustClause _,
        string? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => MustResult.FromBool(
            !string.IsNullOrEmpty(value),
            "{paramName} must not be null or empty.",
            paramName,
            value);

    public static MustResult NotNullOrWhitespace(
        this IMustClause _,
        string? value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => MustResult.FromBool(
            !string.IsNullOrWhiteSpace(value),
            "{paramName} must not be null or whitespace.",
            paramName,
            value);

    public static MustResult Alphabetical(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => MustResult.FromBool(
            value is not null && AlphaRegex().IsMatch(value),
            "{paramName} must contain alphabetic characters only.",
            paramName,
            value);

    public static MustResult Numeric(
        this IMustClause _,
        string value,
        [CallerArgumentExpression("value")] string? paramName = null)
        => MustResult.FromBool(
            value is not null && NumericRegex().IsMatch(value),
            "{paramName} must contain numeric characters only.",
            paramName,
            value);
}
