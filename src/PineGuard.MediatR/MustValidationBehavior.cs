using MediatR;
using PineGuard.MustClauses;

namespace PineGuard.MediatR;

/// <summary>
/// The MediatR pipeline behaviour that runs every <see cref="IMustValidator{T}"/> registered for a request
/// before its handler, and on failure either returns a failure response or throws
/// <see cref="MustValidationException"/>.
/// </summary>
/// <typeparam name="TRequest">The request type flowing through the pipeline.</typeparam>
/// <typeparam name="TResponse">The response type the request's handler produces.</typeparam>
/// <param name="validators">Every validator registered for <typeparamref name="TRequest"/>; empty when the request has none.</param>
/// <param name="typedFactories">Every factory registered for <typeparamref name="TResponse"/> specifically; the first is used.</param>
/// <param name="familyFactories">Every factory registered for a family of response types; the first that serves <typeparamref name="TResponse"/> is used.</param>
/// <remarks>
/// <para>
/// Register it with <see cref="MediatRServiceConfigurationExtension.AddMustValidation"/> rather than by
/// hand — it is an open behaviour, so one registration covers every request in the assembly.
/// </para>
/// <para>
/// A request with no validators passes straight through to its handler. Otherwise every validator runs, in
/// registration order, and their results are merged with
/// <see cref="MustValidationResult.Combine(IEnumerable{MustValidationResult})"/> so one response carries
/// every failure rather than only the first validator's.
/// </para>
/// <para>
/// Both factory seams are taken as <see cref="IEnumerable{T}"/> rather than as optional constructor
/// parameters: that shape resolves in every container (an optional parameter only works in containers that
/// support them) and it never silently drops a second registration.
/// </para>
/// <para>
/// The continuation is invoked as <c>next(cancellationToken)</c>. That overload of
/// <c>RequestHandlerDelegate&lt;TResponse&gt;</c> exists from MediatR 12.5 — the lowest version this package
/// depends on — through 13.x, so cancellation reaches the handler on every supported line.
/// </para>
/// </remarks>
/// <seealso cref="MediatRServiceConfigurationExtension"/>
/// <seealso cref="IMustFailureResponseFactory{TResponse}"/>
/// <seealso cref="IMustFailureResponseFactory"/>
public sealed class MustValidationBehavior<TRequest, TResponse>(
    IEnumerable<IMustValidator<TRequest>> validators,
    IEnumerable<IMustFailureResponseFactory<TResponse>> typedFactories,
    IEnumerable<IMustFailureResponseFactory> familyFactories)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <summary>
    /// Validates <paramref name="request"/> and, when it passes, invokes the rest of the pipeline.
    /// </summary>
    /// <param name="request">The request to validate.</param>
    /// <param name="next">The next step in the pipeline — the handler, or the behaviour after this one.</param>
    /// <param name="cancellationToken">A token to cancel the operation; forwarded to every validator.</param>
    /// <returns>
    /// The handler's response when validation succeeds; otherwise the response built by the first typed
    /// factory, or by the first family factory that serves <typeparamref name="TResponse"/>.
    /// </returns>
    /// <exception cref="MustValidationException">Thrown when validation fails and no factory serves <typeparamref name="TResponse"/>.</exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<MustValidationResult>? results = null;

        foreach (var validator in validators)
        {
            results ??= [];
            results.Add(await validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false));
        }

        if (results is null)
            return await next(cancellationToken).ConfigureAwait(false);

        var result = MustValidationResult.Combine(results);

        return result.Success
            ? await next(cancellationToken).ConfigureAwait(false)
            : CreateFailureResponse(result);
    }

    private TResponse CreateFailureResponse(MustValidationResult result)
    {
        var typedFactory = typedFactories.FirstOrDefault();
        if (typedFactory is not null)
            return typedFactory.Create(result);

        foreach (var familyFactory in familyFactories)
        {
            if (familyFactory.TryCreate(typeof(TResponse), result, out var response))
                return (TResponse)response!;
        }

        throw new MustValidationException(result);
    }
}
