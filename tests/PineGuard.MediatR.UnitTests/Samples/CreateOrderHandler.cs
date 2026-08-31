using MediatR;

namespace PineGuard.MediatR.UnitTests.Samples;

public sealed class CreateOrderHandler : IRequestHandler<CreateOrder, Guid>
{
    public static readonly Guid Response = new("11111111-1111-1111-1111-111111111111");

    public Task<Guid> Handle(CreateOrder request, CancellationToken cancellationToken) => Task.FromResult(Response);
}
