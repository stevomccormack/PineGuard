using PineGuard.MustClauses;

namespace PineGuard.MediatR;

/// <summary>
/// Builds the failure response for one response type, so a failed request returns a result instead of
/// throwing <see cref="MustValidationException"/>.
/// </summary>
/// <typeparam name="TResponse">The response type this factory serves.</typeparam>
/// <remarks>
/// Register one of these per response type that should short-circuit with a value —
/// <c>services.AddSingleton&lt;IMustFailureResponseFactory&lt;CreateOrderResult&gt;, CreateOrderFailureFactory&gt;()</c>.
/// Use the non-generic <see cref="IMustFailureResponseFactory"/> for a family of response types that one
/// registration should cover, and register neither to keep the default throwing behaviour.
/// </remarks>
/// <seealso cref="IMustFailureResponseFactory"/>
/// <seealso cref="MustValidationBehavior{TRequest, TResponse}"/>
public interface IMustFailureResponseFactory<out TResponse>
{
    /// <summary>
    /// Builds the failure response the pipeline returns in place of the handler's.
    /// </summary>
    /// <param name="result">The failed validation result to render.</param>
    /// <returns>The response the caller receives.</returns>
    TResponse Create(MustValidationResult result);
}
