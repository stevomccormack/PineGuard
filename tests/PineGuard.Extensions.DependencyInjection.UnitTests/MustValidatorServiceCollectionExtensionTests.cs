using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public sealed class MustValidatorServiceCollectionExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidator.Cases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidator))]
    public void AddMustValidator_BehavesAsExpected(MustValidatorRegistrationCase<Action<IServiceCollection>> tc)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        tc.Value(services);

        // Assert
        AssertRegistrations(tc.Expected, services);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidator.InvalidCases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidator))]
    public void AddMustValidator_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssembly.Cases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssembly))]
    public void AddMustValidatorsFromAssembly_BehavesAsExpected(MustValidatorRegistrationCase<(Assembly assembly, ServiceLifetime lifetime, Func<Type, bool>? filter)> tc)
    {
        // Arrange
        var (assembly, lifetime, filter) = tc.Value;
        var services = new ServiceCollection();

        // Act
        services.AddMustValidatorsFromAssembly(assembly, lifetime, filter);

        // Assert
        AssertRegistrations(tc.Expected, services);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssembly.InvalidCases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssembly))]
    public void AddMustValidatorsFromAssembly_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblies.Cases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblies))]
    public void AddMustValidatorsFromAssemblies_BehavesAsExpected(MustValidatorRegistrationCase<(IEnumerable<Assembly> assemblies, ServiceLifetime lifetime, Func<Type, bool>? filter)> tc)
    {
        // Arrange
        var (assemblies, lifetime, filter) = tc.Value;
        var services = new ServiceCollection();

        // Act
        services.AddMustValidatorsFromAssemblies(assemblies, lifetime, filter);

        // Assert
        AssertRegistrations(tc.Expected, services);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblies.InvalidCases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblies))]
    public void AddMustValidatorsFromAssemblies_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblyContaining.Cases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblyContaining))]
    public void AddMustValidatorsFromAssemblyContaining_BehavesAsExpected(MustValidatorRegistrationCase<Action<IServiceCollection>> tc)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        tc.Value(services);

        // Assert
        AssertRegistrations(tc.Expected, services);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblyContaining.InvalidCases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.AddMustValidatorsFromAssemblyContaining))]
    public void AddMustValidatorsFromAssemblyContaining_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.IsMustValidatorType.Cases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.IsMustValidatorType))]
    public void IsMustValidatorType_BehavesAsExpected(MustValidatorServiceCollectionExtensionTestData.IsMustValidatorType.Case tc)
    {
        // Act
        var result = MustValidatorServiceCollectionExtension.IsMustValidatorType(tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(MustValidatorServiceCollectionExtensionTestData.IsMustValidatorInterface.Cases), MemberType = typeof(MustValidatorServiceCollectionExtensionTestData.IsMustValidatorInterface))]
    public void IsMustValidatorInterface_BehavesAsExpected(MustValidatorServiceCollectionExtensionTestData.IsMustValidatorInterface.Case tc)
    {
        // Act
        var result = MustValidatorServiceCollectionExtension.IsMustValidatorInterface(tc.Value);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    private static void AssertRegistrations(MustValidatorRegistrationExpected expected, IServiceCollection services)
    {
        Assert.Equal(SortedNames(expected.ServiceTypes), SortedNames(services.Select(descriptor => descriptor.ServiceType)));
        Assert.All(services, descriptor => Assert.Equal(expected.Lifetime, descriptor.Lifetime));

        using var provider = services.BuildServiceProvider();
        Assert.All(services, descriptor => Assert.IsAssignableFrom(descriptor.ServiceType, provider.GetRequiredService(descriptor.ServiceType)));
    }

    private static string[] SortedNames(IEnumerable<Type> types) =>
        [.. types.Select(type => type.ToString()).OrderBy(name => name, StringComparer.Ordinal)];
}
