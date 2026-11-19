using System.Globalization;

namespace PineGuard.Benchmarks.Common;

/// <summary>
/// The value and parsing styles behind each <see cref="PastDateOnlyInput"/>. The benchmarks resolve them once, in
/// their global setup, so the measured call is the validation alone.
/// </summary>
internal static class PastDateOnlyInputs
{
    private static readonly string Today =
        DateOnly.FromDateTime(FixedTimeProvider.Default.GetUtcNow().UtcDateTime).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

    /// <summary>Gets the string validated for <paramref name="input"/>.</summary>
    /// <param name="input">The benchmark input.</param>
    /// <returns>The value to validate.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="input"/> is not a defined input.</exception>
    public static string? Value(PastDateOnlyInput input) =>
        input switch
        {
            PastDateOnlyInput.PastDate or PastDateOnlyInput.InvalidStyles => "2000-01-01",
            PastDateOnlyInput.Today => Today,
            PastDateOnlyInput.NotADate => "not-a-date",
            PastDateOnlyInput.NullValue => null,
            _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
        };

    /// <summary>Gets the parsing styles passed for <paramref name="input"/>.</summary>
    /// <param name="input">The benchmark input.</param>
    /// <returns>
    /// <see cref="DateTimeStyles.AssumeUniversal"/>, which the rule rejects, for <see cref="PastDateOnlyInput.InvalidStyles"/>;
    /// otherwise the rule's default, <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
    /// </returns>
    public static DateTimeStyles Styles(PastDateOnlyInput input) =>
        input == PastDateOnlyInput.InvalidStyles ? DateTimeStyles.AssumeUniversal : DateTimeStyles.AllowWhiteSpaces;
}
