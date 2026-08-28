namespace PineGuard.Core.UnitTests.MustClauses.Samples;

public sealed record CreateOrder(string? Email, DateTime StartDate, DateTime EndDate, bool IsPhysical, decimal Weight, IReadOnlyList<OrderLine>? Lines);
