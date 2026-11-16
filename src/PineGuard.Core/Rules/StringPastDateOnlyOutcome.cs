#if NET8_0_OR_GREATER
namespace PineGuard.Rules;

/// <summary>
/// The result of <see cref="StringPastDateOnlyRule.Evaluate"/>: a pass, or the one step of the rule that failed.
/// </summary>
/// <remarks>
/// The members are listed in evaluation order: the first step that fails decides the outcome. Zero is
/// deliberately not a member, so a defaulted outcome is never mistaken for <see cref="Passed"/> and every
/// <see cref="StringPastDateOnlyRule"/> lookup rejects it.
/// </remarks>
internal enum StringPastDateOnlyOutcome
{
    /// <summary>
    /// The value parsed to a date before today, or it was <see langword="null"/> and the caller's null
    /// policy lets <see langword="null"/> pass.
    /// </summary>
    Passed = 1,

    /// <summary>
    /// The parsing styles are not a supported combination. Checked before the value, so it is reported even
    /// for a <see langword="null"/> value under a null-passing policy.
    /// </summary>
    InvalidStyles,

    /// <summary>
    /// The value is <see langword="null"/> and the caller's null policy rejects <see langword="null"/>.
    /// </summary>
    Null,

    /// <summary>
    /// The value is empty, whitespace or not a date the invariant culture can parse with the given styles.
    /// </summary>
    Unparsable,

    /// <summary>
    /// The value parsed, but today's date could not be derived from the clock because the projection into
    /// its time zone is not representable.
    /// </summary>
    ReferenceOutOfRange,

    /// <summary>
    /// The value parsed to today or a later date.
    /// </summary>
    NotPast
}
#endif
