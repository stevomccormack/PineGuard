using Microsoft.Extensions.DependencyInjection;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;
using Customer = PineGuard.FluentValidation.UnitTests.FluentMustValidatorTestData.Customer;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentMustValidatorServiceCollectionExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    // FluentMustValidatorServiceCollectionExtension.AddMustValidatorsFromFluentValidators
    [Theory]
    [MemberData(nameof(FluentMustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromFluentValidators.Cases), MemberType = typeof(FluentMustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromFluentValidators))]
    public void AddMustValidatorsFromFluentValidators_BehavesAsExpected(FluentMustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromFluentValidators.Case tc)
    {
        // Arrange
        var (descriptors, calls) = tc.Value;
        IServiceCollection services = new ServiceCollection();
        foreach (var descriptor in descriptors)
            services.Add(descriptor);

        // Act
        for (var call = 0; call < calls; call++)
            Assert.Same(services, services.AddMustValidatorsFromFluentValidators());

        // Assert
        var adapters = services.Where(IsAdapter).ToList();
        Assert.Equal(tc.Expected.AdapterCount, adapters.Count);
        Assert.Equal(tc.Expected.IsValid, adapters.Count > 0);
        Assert.All(adapters, adapter => Assert.Equal(tc.Expected.Lifetime, adapter.Lifetime));

        using var provider = services.BuildServiceProvider();
        var resolved = provider.GetServices<IMustValidator<Customer>>().ToList();
        Assert.Equal(tc.Expected.CustomerValidatorCount, resolved.Count);
        Assert.All(resolved.OfType<FluentMustValidator<Customer>>(), validator => Assert.False(validator.Validate(FluentMustValidatorServiceCollectionExtensionTestData.InvalidCustomer).Success));
    }

    [Theory]
    [MemberData(nameof(FluentMustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromFluentValidators.InvalidCases), MemberType = typeof(FluentMustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromFluentValidators))]
    public void AddMustValidatorsFromFluentValidators_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static bool IsAdapter(ServiceDescriptor descriptor) =>
        descriptor.ImplementationType is { IsConstructedGenericType: true } implementationType &&
        implementationType.GetGenericTypeDefinition() == typeof(FluentMustValidator<>);
}
