#if NET8_0_OR_GREATER
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// The rule "this string parses to a <see cref="DateOnly"/> in the past", held as one executable source of
/// truth: the predicate and its evaluation order, the null policy, the failure codes, the message templates
/// and which parameter a failure is about. Adapters map from it and carry none of that themselves.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Evaluate"/> decides the <see cref="StringPastDateOnlyOutcome"/>. The lookups
/// (<see cref="Code"/>, <see cref="MessageTemplate"/>, <see cref="ParamName"/> and <see cref="Value"/>)
/// translate that outcome into what a result type carries, and are total over every defined outcome,
/// <see cref="StringPastDateOnlyOutcome.Passed"/> included, so a single result factory call can take all
/// of them at once.
/// </para>
/// <para>
/// Parsing uses <see cref="StringUtility.DateOnly.TryParse"/> (invariant culture) and "today" is read once
/// through <see cref="DateTimeUtility.TryGetTodayDate"/>: with no clock it is the UTC calendar date, and with
/// an injected clock it is the date in that clock's <see cref="TimeProvider.LocalTimeZone"/>.
/// </para>
/// </remarks>
internal static class StringPastDateOnlyRule
{
    private const string StylesParamName = "styles";
    private const string TimeProviderParamName = "timeProvider";

    private const string NotPastMessageTemplate = "{paramName} must be a date in the past.";
    private const string InvalidStylesMessageTemplate = "{paramName} must be a valid combination of parsing styles.";
    private const string NullMessageTemplate = "{paramName} must not be null.";
    private const string ReferenceOutOfRangeMessageTemplate = "{paramName} requires a reference date within the supported range.";

    /// <summary>
    /// Evaluates whether <paramref name="value"/> parses to a date strictly before today.
    /// </summary>
    /// <param name="value">The string to evaluate.</param>
    /// <param name="styles">
    /// The parsing styles. Only <see cref="DateTimeStyles.None"/>, <see cref="DateTimeStyles.AllowWhiteSpaces"/>
    /// and its constituent flags are supported; any other flag gives <see cref="StringPastDateOnlyOutcome.InvalidStyles"/>.
    /// The supported values all parse alike, because the non-exact parser always permits leading, inner and
    /// trailing white space.
    /// </param>
    /// <param name="timeProvider">
    /// The clock that supplies today's date. If <see langword="null"/>, the system clock is used and today is its
    /// UTC calendar date.
    /// </param>
    /// <param name="allowNull">
    /// The null policy. When <see langword="true"/>, a <see langword="null"/> <paramref name="value"/> with valid
    /// <paramref name="styles"/> gives <see cref="StringPastDateOnlyOutcome.Passed"/>; when <see langword="false"/>,
    /// it gives <see cref="StringPastDateOnlyOutcome.Null"/>. Either way the styles are checked first.
    /// </param>
    /// <param name="parsed">
    /// When parsing was reached and succeeded (<see cref="StringPastDateOnlyOutcome.Passed"/> for a
    /// non-<see langword="null"/> value, <see cref="StringPastDateOnlyOutcome.ReferenceOutOfRange"/> and
    /// <see cref="StringPastDateOnlyOutcome.NotPast"/>), the parsed date; otherwise <see langword="default"/>.
    /// </param>
    /// <returns>
    /// The first failing step, in this order: <see cref="StringPastDateOnlyOutcome.InvalidStyles"/>,
    /// <see cref="StringPastDateOnlyOutcome.Null"/>, <see cref="StringPastDateOnlyOutcome.Unparseable"/>,
    /// <see cref="StringPastDateOnlyOutcome.ReferenceOutOfRange"/>, <see cref="StringPastDateOnlyOutcome.NotPast"/>;
    /// or <see cref="StringPastDateOnlyOutcome.Passed"/>.
    /// </returns>
    /// <remarks>
    /// The clock is read exactly once, and only after <paramref name="value"/> has parsed: invalid styles, a
    /// <see langword="null"/> value and an unparseable value never read it.
    /// </remarks>
    internal static StringPastDateOnlyOutcome Evaluate(string? value, DateTimeStyles styles, TimeProvider? timeProvider, bool allowNull, out DateOnly parsed)
    {
        parsed = default;

        if (!StringUtility.DateOnly.IsValidStyles(styles))
            return StringPastDateOnlyOutcome.InvalidStyles;

        if (value is null)
            return allowNull ? StringPastDateOnlyOutcome.Passed : StringPastDateOnlyOutcome.Null;

        if (!StringUtility.DateOnly.TryParse(value, out parsed, styles))
            return StringPastDateOnlyOutcome.Unparseable;

        if (!DateTimeUtility.TryGetTodayDate(timeProvider, out var referenceDate))
            return StringPastDateOnlyOutcome.ReferenceOutOfRange;

        return DateOnlyRules.IsBefore(parsed, DateOnly.FromDateTime(referenceDate), Inclusion.Exclusive)
            ? StringPastDateOnlyOutcome.Passed
            : StringPastDateOnlyOutcome.NotPast;
    }

    /// <summary>
    /// Gets the <see cref="MustCodes"/> code for <paramref name="outcome"/>.
    /// </summary>
    /// <param name="outcome">The outcome of <see cref="Evaluate"/>.</param>
    /// <returns>
    /// <see cref="MustCodes.Date.Format.Invalid"/> for <see cref="StringPastDateOnlyOutcome.Unparseable"/>,
    /// <see cref="MustCodes.Date.Reference.OutOfRange"/> for <see cref="StringPastDateOnlyOutcome.ReferenceOutOfRange"/>,
    /// and the rule's primary code, <see cref="MustCodes.Date.Relative.NotPast"/>, for every other outcome. For
    /// <see cref="StringPastDateOnlyOutcome.Passed"/> that primary code identifies the rule; it is never reported
    /// as a failure.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outcome"/> is not a defined outcome.</exception>
    internal static string Code(StringPastDateOnlyOutcome outcome) =>
        outcome switch
        {
            StringPastDateOnlyOutcome.Passed or StringPastDateOnlyOutcome.InvalidStyles or StringPastDateOnlyOutcome.Null or StringPastDateOnlyOutcome.NotPast => MustCodes.Date.Relative.NotPast,
            StringPastDateOnlyOutcome.Unparseable => MustCodes.Date.Format.Invalid,
            StringPastDateOnlyOutcome.ReferenceOutOfRange => MustCodes.Date.Reference.OutOfRange,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
        };

    /// <summary>
    /// Gets the message template for <paramref name="outcome"/>, with <c>{paramName}</c> standing for the
    /// parameter <see cref="ParamName"/> names.
    /// </summary>
    /// <param name="outcome">The outcome of <see cref="Evaluate"/>.</param>
    /// <returns>
    /// The template for the failing step. <see cref="StringPastDateOnlyOutcome.Unparseable"/> shares the rule's
    /// primary template with <see cref="StringPastDateOnlyOutcome.NotPast"/>, and
    /// <see cref="StringPastDateOnlyOutcome.Passed"/> returns that primary template too.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outcome"/> is not a defined outcome.</exception>
    internal static string MessageTemplate(StringPastDateOnlyOutcome outcome) =>
        outcome switch
        {
            StringPastDateOnlyOutcome.Passed or StringPastDateOnlyOutcome.Unparseable or StringPastDateOnlyOutcome.NotPast => NotPastMessageTemplate,
            StringPastDateOnlyOutcome.InvalidStyles => InvalidStylesMessageTemplate,
            StringPastDateOnlyOutcome.Null => NullMessageTemplate,
            StringPastDateOnlyOutcome.ReferenceOutOfRange => ReferenceOutOfRangeMessageTemplate,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
        };

    /// <summary>
    /// Gets the name of the parameter <paramref name="outcome"/> is about.
    /// </summary>
    /// <param name="outcome">The outcome of <see cref="Evaluate"/>.</param>
    /// <param name="paramName">The caller's name for the evaluated value; may be <see langword="null"/>.</param>
    /// <returns>
    /// <c>"styles"</c> for <see cref="StringPastDateOnlyOutcome.InvalidStyles"/>, <c>"timeProvider"</c> for
    /// <see cref="StringPastDateOnlyOutcome.ReferenceOutOfRange"/>, and <paramref name="paramName"/> unchanged for
    /// every outcome that is about the evaluated value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outcome"/> is not a defined outcome.</exception>
    internal static string? ParamName(StringPastDateOnlyOutcome outcome, string? paramName) =>
        outcome switch
        {
            StringPastDateOnlyOutcome.InvalidStyles => StylesParamName,
            StringPastDateOnlyOutcome.ReferenceOutOfRange => TimeProviderParamName,
            StringPastDateOnlyOutcome.Passed or StringPastDateOnlyOutcome.Null or StringPastDateOnlyOutcome.Unparseable or StringPastDateOnlyOutcome.NotPast => paramName,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
        };

    /// <summary>
    /// Gets the argument <paramref name="outcome"/> is about, from the arguments passed to <see cref="Evaluate"/>.
    /// </summary>
    /// <param name="outcome">The outcome of <see cref="Evaluate"/>.</param>
    /// <param name="value">The evaluated string.</param>
    /// <param name="styles">The parsing styles.</param>
    /// <param name="timeProvider">The clock.</param>
    /// <returns>
    /// <paramref name="styles"/> for <see cref="StringPastDateOnlyOutcome.InvalidStyles"/>,
    /// <paramref name="timeProvider"/> for <see cref="StringPastDateOnlyOutcome.ReferenceOutOfRange"/>, and
    /// <paramref name="value"/> for every outcome that is about the evaluated value.
    /// </returns>
    /// <remarks>
    /// Returning <paramref name="styles"/> boxes it. That happens on the
    /// <see cref="StringPastDateOnlyOutcome.InvalidStyles"/> outcome only, and is the same box a result type with
    /// an <see cref="object"/> value would otherwise take at the call site; no other outcome allocates.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="outcome"/> is not a defined outcome.</exception>
    internal static object? Value(StringPastDateOnlyOutcome outcome, string? value, DateTimeStyles styles, TimeProvider? timeProvider) =>
        outcome switch
        {
            StringPastDateOnlyOutcome.InvalidStyles => styles,
            StringPastDateOnlyOutcome.ReferenceOutOfRange => timeProvider,
            StringPastDateOnlyOutcome.Passed or StringPastDateOnlyOutcome.Null or StringPastDateOnlyOutcome.Unparseable or StringPastDateOnlyOutcome.NotPast => value,
            _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
        };
}
#endif
