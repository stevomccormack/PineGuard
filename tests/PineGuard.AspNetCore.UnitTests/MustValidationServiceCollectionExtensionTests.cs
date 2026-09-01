using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PineGuard.AspNetCore.UnitTests.Samples;
using PineGuard.Extensions.DependencyInjection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.AspNetCore.UnitTests;

public sealed class MustValidationServiceCollectionExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidationServiceCollectionExtensionTestData.AddMustValidation.Cases), MemberType = typeof(MustValidationServiceCollectionExtensionTestData.AddMustValidation))]
    public void AddMustValidation_BehavesAsExpected(MustValidationServiceCollectionExtensionTestData.AddMustValidation.Case tc)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var returned = tc.Value(services);

        // Assert
        Assert.Same(services, returned);

        AssertResolver(services, tc.Expected.ResolverType);
        AssertExceptionHandler(services);

        using var provider = services.BuildServiceProvider();

        Assert.Equal(tc.Expected.IncludeCodes, provider.GetRequiredService<IOptions<MustValidationOptions>>().Value.IncludeCodes);
        Assert.Equal(tc.Expected.HasOrderValidator, provider.TryGetMustValidator(typeof(CreateOrder), out _));
    }

    [Theory]
    [MemberData(nameof(MustValidationServiceCollectionExtensionTestData.AddMustValidation.InvalidCases), MemberType = typeof(MustValidationServiceCollectionExtensionTestData.AddMustValidation))]
    public void AddMustValidation_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    /// <summary>
    /// Asserts the message resolver is registered exactly once, by the expected implementation — the proof
    /// that a resolver registered before the call is kept rather than added alongside.
    /// </summary>
    private static void AssertResolver(IServiceCollection services, Type expectedResolverType)
    {
        var descriptor = Assert.Single(services, service => service.ServiceType == typeof(IMustFailureMessageResolver));

        Assert.Equal(expectedResolverType, descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    private static void AssertExceptionHandler(IServiceCollection services)
    {
        var descriptor = Assert.Single(services, service => service.ServiceType == typeof(IExceptionHandler));

        Assert.Equal(typeof(MustValidationExceptionHandler), descriptor.ImplementationType);
    }
}
