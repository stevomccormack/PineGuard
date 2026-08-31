using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace PineGuard.AspNetCore.UnitTests.Samples;

/// <summary>
/// An <see cref="IMvcBuilder"/> over a bare service collection — MVC's own implementation is internal, and
/// <c>AddMustValidation</c> only ever touches <see cref="Services"/>.
/// </summary>
public sealed class SampleMvcBuilder(IServiceCollection services) : IMvcBuilder
{
    public IServiceCollection Services { get; } = services;

    public ApplicationPartManager PartManager { get; } = new();
}
