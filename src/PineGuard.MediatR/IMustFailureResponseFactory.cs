using PineGuard.MustClauses;

namespace PineGuard.MediatR;

/// <summary>
/// Builds a failure response for a whole <em>family</em> of response types — an open generic such as
/// <c>ErrorOr&lt;T&gt;</c> that closes differently for every request — instead of throwing.
/// </summary>
/// <remarks>
/// <para>
/// This seam exists because Microsoft DI cannot map an open generic
/// <c>IMustFailureResponseFactory&lt;&gt;</c> onto an implementation that closes it as
/// <c>IMustFailureResponseFactory&lt;ErrorOr&lt;T&gt;&gt;</c>: the type parameters do not line up and
/// registration throws. A family factory is registered non-generically once, is handed the runtime
/// response type, and decides for itself whether it can serve it.
/// </para>
/// <para>
/// Register <see cref="IMustFailureResponseFactory{TResponse}"/> instead when exactly one response type is
/// involved — <see cref="MustValidationBehavior{TRequest, TResponse}"/> always prefers a typed factory over
/// a family one, so a specific registration wins.
/// </para>
/// </remarks>
/// <seealso cref="IMustFailureResponseFactory{TResponse}"/>
/// <seealso cref="MustValidationBehavior{TRequest, TResponse}"/>
public interface IMustFailureResponseFactory
{
    /// <summary>
    /// Tries to build the failure response the pipeline should return for <paramref name="responseType"/>.
    /// </summary>
    /// <param name="responseType">The response type the request's handler would have produced.</param>
    /// <param name="result">The failed validation result to render.</param>
    /// <param name="response">When this method returns <see langword="true"/>, the failure response; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when this factory serves <paramref name="responseType"/>; otherwise <see langword="false"/>.</returns>
    bool TryCreate(Type responseType, MustValidationResult result, out object? response);
}
