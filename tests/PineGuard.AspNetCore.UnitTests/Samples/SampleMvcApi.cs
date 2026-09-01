using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Extensions.DependencyInjection;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// The MVC application of Plan 03's story 3 — <c>AddControllers().AddMustValidation()</c> and nothing else,
/// answering the same requests <see cref="SampleMinimalApi"/> answers so the two bodies can be compared.
/// </summary>
/// <seealso cref="MustValidationActionFilter"/>
/// <seealso cref="SampleMvcController"/>
public static class SampleMvcApi
{
    /// <summary>
    /// Registers request validation, the order validator and the global action filter.
    /// </summary>
    /// <param name="services">The application's service collection.</param>
    /// <remarks>
    /// No JSON naming policy is configured: the point of the story is that the defaults already produce
    /// camel-cased error keys, because the filter follows whatever policy the application serialises with.
    /// </remarks>
    public static void ConfigureServices(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddMustValidation();
        services.AddMustValidator<CreateOrderValidator>();
        services.AddControllers().AddMustValidation();
    }

    /// <summary>
    /// Maps <see cref="SampleMvcController"/>'s attribute routes.
    /// </summary>
    /// <param name="app">The application to map the controllers on.</param>
    public static void Map(WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapControllers();
    }
}
