using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests.Samples;

/// <summary>
/// A family factory that serves <see cref="Guid"/> responses.
/// </summary>
public sealed class GuidFailureResponseFactory : IMustFailureResponseFactory
{
    public static readonly Guid Response = new("33333333-3333-3333-3333-333333333333");

    public bool TryCreate(Type responseType, MustValidationResult result, out object? response)
    {
        if (responseType == typeof(Guid) && result?.Failed == true)
        {
            response = Response;
            return true;
        }

        response = null;
        return false;
    }
}
