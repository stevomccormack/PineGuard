using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests.Samples;

/// <summary>
/// A family factory that serves <see cref="Uri"/> responses only — it declines every request whose
/// response type is something else, which is how the behaviour's "no factory serves this" path is reached.
/// </summary>
public sealed class UriFailureResponseFactory : IMustFailureResponseFactory
{
    public static readonly Uri Response = new("https://example.com/validation-failed");

    public bool TryCreate(Type responseType, MustValidationResult result, out object? response)
    {
        if (responseType == typeof(Uri) && result?.Failed == true)
        {
            response = Response;
            return true;
        }

        response = null;
        return false;
    }
}
