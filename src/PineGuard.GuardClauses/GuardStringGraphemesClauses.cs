using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for how many grapheme clusters — the characters a reader sees — a <see cref="string"/> holds.
/// </summary>
/// <remarks>
/// <see cref="string.Length"/> counts UTF-16 code units, so a family emoji reads as eleven characters and an
/// accented letter written with a combining mark reads as two. These guards enforce what a length limit shown
/// to a user is actually promising. Segmentation follows the host runtime's Unicode tables.
/// </remarks>
/// <seealso cref="MustStringGraphemesClauses"/>
/// <seealso href="https://pineguard.ai/docs/guard/string-graphemes">Guard String Graphemes Clauses documentation</seealso>
public static class GuardStringGraphemesClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> does not hold exactly <paramref name="count"/> grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="count">The required number of characters.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> does not hold exactly <paramref name="count"/> characters, or when
    /// <paramref name="count"/> is negative, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>:
    /// <c>Guard.Against.NotHasExactGraphemeCount</c> passes when the count matches.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasExactGraphemeCount(code, count: 6);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>
    public static string NotHasExactGraphemeCount(this IGuardClause _,
        string? value,
        int count,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasExactGraphemeCount(value, count, paramName); // Guard.Against.NotHasExactGraphemeCount => Must.Be.HasExactGraphemeCount (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> holds exactly <paramref name="count"/> grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="count">The number of characters that must not match.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> holds exactly <paramref name="count"/> characters, or when
    /// <paramref name="count"/> is negative, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>:
    /// <c>Guard.Against.HasExactGraphemeCount</c> passes when the count differs.
    /// </remarks>
    /// <seealso cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>
    public static string HasExactGraphemeCount(this IGuardClause _,
        string? value,
        int count,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasExactGraphemeCount(value, count, paramName); // Guard.Against.HasExactGraphemeCount => Must.Be.NotHasExactGraphemeCount (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> holds fewer than <paramref name="min"/> grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="min">The minimum required number of characters.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> holds fewer than <paramref name="min"/> characters, or when
    /// <paramref name="min"/> is negative, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>:
    /// <c>Guard.Against.NotHasMinGraphemeCount</c> passes when the minimum is met.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasMinGraphemeCount(displayName, min: 2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>
    public static string NotHasMinGraphemeCount(this IGuardClause _,
        string? value,
        int min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasMinGraphemeCount(value, min, paramName); // Guard.Against.NotHasMinGraphemeCount => Must.Be.HasMinGraphemeCount (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> holds at least <paramref name="min"/> grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="min">The minimum number of characters that must not be reached.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> holds at least <paramref name="min"/> characters, or when
    /// <paramref name="min"/> is negative, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>:
    /// <c>Guard.Against.HasMinGraphemeCount</c> passes when the minimum is not met.
    /// </remarks>
    /// <seealso cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>
    public static string HasMinGraphemeCount(this IGuardClause _,
        string? value,
        int min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasMinGraphemeCount(value, min, paramName); // Guard.Against.HasMinGraphemeCount => Must.Be.NotHasMinGraphemeCount (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> holds more than <paramref name="max"/> grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="max">The maximum allowed number of characters.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> holds more than <paramref name="max"/> characters, or when
    /// <paramref name="max"/> is negative, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>:
    /// <c>Guard.Against.NotHasMaxGraphemeCount</c> passes when the maximum is respected. This is the guard a
    /// "your name is too long" limit wants: a family emoji costs eleven code units and one character.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasMaxGraphemeCount(displayName, max: 50);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>
    public static string NotHasMaxGraphemeCount(this IGuardClause _,
        string? value,
        int max,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasMaxGraphemeCount(value, max, paramName); // Guard.Against.NotHasMaxGraphemeCount => Must.Be.HasMaxGraphemeCount (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> holds at most <paramref name="max"/> grapheme clusters.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="max">The maximum number of characters that must be exceeded.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> holds at most <paramref name="max"/> characters, or when
    /// <paramref name="max"/> is negative, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>:
    /// <c>Guard.Against.HasMaxGraphemeCount</c> passes when the maximum is exceeded.
    /// </remarks>
    /// <seealso cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>
    public static string HasMaxGraphemeCount(this IGuardClause _,
        string? value,
        int max,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasMaxGraphemeCount(value, max, paramName); // Guard.Against.HasMaxGraphemeCount => Must.Be.NotHasMaxGraphemeCount (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if the number of grapheme clusters in <paramref name="value"/> falls outside the range
    /// <paramref name="min"/> to <paramref name="max"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="min">The lower bound of the acceptable number of characters (inclusive by default).</param>
    /// <param name="max">The upper bound of the acceptable number of characters (inclusive by default).</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.HasGraphemeCountBetween"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the number of characters is outside the range, or when <paramref name="min"/> or
    /// <paramref name="max"/> is negative or the range is inverted, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.HasGraphemeCountBetween"/>:
    /// <c>Guard.Against.NotHasGraphemeCountBetween</c> passes when the count is within the range.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasGraphemeCountBetween(displayName, min: 2, max: 50);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasGraphemeCountBetween"/>
    public static string NotHasGraphemeCountBetween(this IGuardClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasGraphemeCountBetween(value, min, max, inclusion, paramName); // Guard.Against.NotHasGraphemeCountBetween => Must.Be.HasGraphemeCountBetween (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if the number of grapheme clusters in <paramref name="value"/> falls within the range
    /// <paramref name="min"/> to <paramref name="max"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string value to guard.</param>
    /// <param name="min">The lower bound of the forbidden number of characters (inclusive by default).</param>
    /// <param name="max">The upper bound of the forbidden number of characters (inclusive by default).</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustStringGraphemesClauses.NotHasGraphemeCountBetween"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the number of characters is inside the range, or when <paramref name="min"/> or
    /// <paramref name="max"/> is negative or the range is inverted, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustStringGraphemesClauses.NotHasGraphemeCountBetween"/>:
    /// <c>Guard.Against.HasGraphemeCountBetween</c> passes when the count is outside the range.
    /// </remarks>
    /// <seealso cref="MustStringGraphemesClauses.NotHasGraphemeCountBetween"/>
    public static string HasGraphemeCountBetween(this IGuardClause _,
        string? value,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasGraphemeCountBetween(value, min, max, inclusion, paramName); // Guard.Against.HasGraphemeCountBetween => Must.Be.NotHasGraphemeCountBetween (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
