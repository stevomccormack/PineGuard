namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The request body Plan 03's story 2 posts to <c>/orders</c>.
/// </summary>
public sealed class CreateOrder
{
    /// <summary>
    /// The only email <see cref="CreateOrderValidator"/> accepts.
    /// </summary>
    public const string ValidEmail = "buyer@example.test";

    public string? Email { get; init; }

    public static CreateOrder Valid => new() { Email = ValidEmail };

    public static CreateOrder Invalid => new() { Email = "not-an-email" };
}
