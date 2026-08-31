using MediatR;

namespace PineGuard.MediatR.UnitTests.Samples;

public sealed record CreateOrder(string? Sku, int Quantity) : IRequest<Guid>;
