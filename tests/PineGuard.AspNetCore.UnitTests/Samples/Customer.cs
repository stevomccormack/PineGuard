namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// An argument type with no validator registered anywhere — the proof that an endpoint or action which
/// binds only unvalidated types is left alone.
/// </summary>
public sealed class Customer
{
    public string? Name { get; init; }
}
