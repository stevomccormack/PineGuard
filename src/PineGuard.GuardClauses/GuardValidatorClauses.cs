using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// The Guard-style spelling of "validate this object or throw" — adapts an <see cref="IMustValidator{T}"/>
/// into the same <see cref="ArgumentException"/>/<see cref="GuardExceptionPolicy"/> idiom as every other
/// <c>Guard.Against.*</c> clause.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/validator">Guard Validator Clauses documentation</seealso>
public static class GuardValidatorClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> fails <paramref name="validator"/>.
    /// </summary>
    /// <typeparam name="T">The type <paramref name="validator"/> validates.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The instance to guard.</param>
    /// <param name="validator">The validator to run <paramref name="value"/> against.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns><paramref name="value"/> if the guard passes.</returns>
    /// <remarks>
    /// Throws the standard <see cref="ArgumentException"/> family — never <see cref="MustValidationException"/>,
    /// which is the boundary marker <c>validator.Validate(value).ThrowIfFailed()</c> throws when the whole
    /// <see cref="MustValidationResult"/> is wanted instead of a single Guard-style exception. The exception is
    /// built from the first failure only (<see cref="MustFailure.Code"/>, <see cref="MustFailure.Message"/>,
    /// <see cref="MustFailure.PropertyPath"/> as its <c>ParamName</c>, <see cref="MustFailure.Value"/>), so an
    /// active <see cref="GuardExceptionPolicy"/> map can route it by code like any other guard failure.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="validator"/> is <see langword="null"/>, or when <paramref name="value"/>
    /// fails validation because it is itself <see langword="null"/> and no <see cref="GuardExceptionPolicy"/>
    /// map is active.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> fails validation and no <see cref="GuardExceptionPolicy"/> map is active.
    /// </exception>
    /// <example>
    /// <code>
    /// public Order(string email, IReadOnlyList&lt;OrderLine&gt; lines)
    /// {
    ///     Guard.Against.Invalid(this, OrderValidator.Instance);
    ///     // ...
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="IMustValidator{T}"/>
    /// <seealso cref="GuardFailure"/>
    public static T Invalid<T>(this IGuardClause _,
        T value,
        IMustValidator<T> validator,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : notnull
    {
        ThrowHelper.ThrowIfNull(validator);

        var result = validator.Validate(value);
        if (result.Failed)
        {
            var failure = result.Failures[0];
            var propertyPath = string.IsNullOrEmpty(failure.PropertyPath) ? paramName : failure.PropertyPath;
            GuardFailure.Throw(MustResult<T>.FailPreformatted(failure.Code, failure.Message, failure.Message, propertyPath, failure.Value));
        }

        return value;
    }
}
