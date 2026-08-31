using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.MediatR.UnitTests.Samples;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.MediatR.UnitTests;

public sealed class MediatRServiceConfigurationExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MediatRServiceConfigurationExtensionTestData.AddMustValidation.Cases), MemberType = typeof(MediatRServiceConfigurationExtensionTestData.AddMustValidation))]
    public async Task AddMustValidation_BehavesAsExpected(MustValidationCase<(CreateOrder request, bool registerFailureFactory)> tc)
    {
        // Arrange
        var (request, registerFailureFactory) = tc.Value;
        MediatRServiceConfiguration? configuration = null;
        MediatRServiceConfiguration? returned = null;

        var services = new ServiceCollection();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateOrder>();
            configuration = cfg;
            returned = cfg.AddMustValidation();
        });
        services.AddMustValidator<CreateOrderValidator>();
        services.AddMustValidator<CreateOrderQuotaValidator>();

        if (registerFailureFactory)
            services.AddSingleton<IMustFailureResponseFactory<Guid>, CreateOrderFailureResponseFactory>();

        using var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();

        // Act & Assert
        await MustValidationAssert.ResponseAsync(tc.Expected, () => mediator.Send(request, CancellationToken.None));

        Assert.Same(configuration, returned);
        Assert.Contains(services, descriptor => descriptor.ServiceType == typeof(IPipelineBehavior<,>) && descriptor.ImplementationType == typeof(MustValidationBehavior<,>));
    }

    [Theory]
    [MemberData(nameof(MediatRServiceConfigurationExtensionTestData.AddMustValidation.InvalidCases), MemberType = typeof(MediatRServiceConfigurationExtensionTestData.AddMustValidation))]
    public void AddMustValidation_ThrowsAsExpected(MediatRServiceConfigurationExtensionTestData.AddMustValidation.InvalidCase tc)
    {
        // Arrange
        var action = tc.Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
