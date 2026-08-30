using FluentValidation;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// An <see cref="IMustValidator{T}"/> backed by an existing FluentValidation <see cref="IValidator{T}"/>,
/// so a validator a team already owns runs unchanged at every seam that consumes PineGuard validators.
/// </summary>
/// <typeparam name="T">The type the wrapped validator validates.</typeparam>
/// <remarks>
/// <para>
/// This is the migration path for a FluentValidation shop: wrap the existing validators once and the
/// PineGuard adapters — options binding, request filters, mediator pipelines — accept them as they are.
/// The wrapper is one-way and lossless in the direction it travels: every failure keeps its
/// <c>PropertyName</c>, <c>ErrorMessage</c> and <c>ErrorCode</c> through
/// <see cref="ValidationResultExtension.ToMustValidationResult"/>.
/// </para>
/// <para>
/// <see cref="MustValidationMode"/> is not forwarded. FluentValidation expresses the same idea as
/// <c>CascadeMode</c>, configured on the validator itself rather than passed per call, so honouring the
/// mode here would mean mutating a validator the consumer owns. The interface's default behaviour
/// applies instead: the mode is ignored and every failure is aggregated.
/// </para>
/// </remarks>
/// <seealso cref="ValidationResultExtension"/>
/// <seealso cref="IMustValidator{T}"/>
public sealed class FluentMustValidator<T> : IMustValidator<T>
    where T : notnull
{
    /// <summary>
    /// Gets the wrapped FluentValidation validator.
    /// </summary>
    public IValidator<T> Validator { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentMustValidator{T}"/> class.
    /// </summary>
    /// <param name="validator">The FluentValidation validator to adapt.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="validator"/> is <see langword="null"/>.</exception>
    public FluentMustValidator(IValidator<T> validator)
    {
        ThrowHelper.ThrowIfNull(validator);

        Validator = validator;
    }

    /// <inheritdoc/>
    public MustValidationResult Validate(T value) =>
        Validator.Validate(value).ToMustValidationResult();

    /// <inheritdoc/>
    public async ValueTask<MustValidationResult> ValidateAsync(T value, CancellationToken cancellationToken = default)
    {
        var result = await Validator.ValidateAsync(value, cancellationToken).ConfigureAwait(false);
        return result.ToMustValidationResult();
    }
}
