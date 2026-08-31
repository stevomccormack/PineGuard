using PineGuard.MustClauses;

namespace PineGuard.MediatR.UnitTests.Samples;

public sealed class CreateOrderFailureResponseFactory : IMustFailureResponseFactory<Guid>
{
    public static readonly Guid Response = new("22222222-2222-2222-2222-222222222222");

    public Guid Create(MustValidationResult result) => result?.Failed == true ? Response : Guid.Empty;
}
