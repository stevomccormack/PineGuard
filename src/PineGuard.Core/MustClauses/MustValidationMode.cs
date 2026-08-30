namespace PineGuard.MustClauses;

/// <summary>
/// How a validator stops: collect every failure, or stop at the first rule that fails.
/// </summary>
/// <remarks>
/// <see cref="Aggregate"/> is the default everywhere; a host surfaces the choice per application
/// (an ASP.NET options bag, for example) and passes it to
/// <see cref="IMustValidator{T}.ValidateAsync(T, MustValidationMode, CancellationToken)"/>.
/// </remarks>
/// <seealso cref="IMustValidator{T}"/>
/// <seealso cref="MustValidator{T}"/>
public enum MustValidationMode
{
    /// <summary>
    /// Run every registered rule and collect every failure. The default.
    /// </summary>
    Aggregate = 0,

    /// <summary>
    /// Stop after the first rule that emits a failure, so the result carries only that rule's failures.
    /// </summary>
    StopOnFirstFailure = 1
}
