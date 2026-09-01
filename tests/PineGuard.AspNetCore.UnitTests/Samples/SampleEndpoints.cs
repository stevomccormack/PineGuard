using System.Reflection;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// Handler shapes whose <see cref="MethodInfo"/> the endpoint-filter factory inspects at build time.
/// </summary>
public static class SampleEndpoints
{
    public static void WithValidatedParameter(CreateOrder order) => _ = order;

    public static void WithoutValidatedParameter(Customer customer) => _ = customer;

    public static void WithoutParameters()
    {
    }

    public static void WithSecondParameterValidated(Customer customer, SearchQuery query) => _ = (customer, query);

    /// <summary>
    /// Gets the <see cref="MethodInfo"/> of the handler named <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The handler's method name.</param>
    public static MethodInfo Handler(string name) => typeof(SampleEndpoints).GetMethod(name)!;
}
