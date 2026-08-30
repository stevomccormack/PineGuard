using Microsoft.Extensions.DependencyInjection;
using PineGuard.MustClauses;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Extensions.DependencyInjection.UnitTests;

public sealed class ServiceProviderExtensionTests(ITestOutputHelper output)
    : BaseUnitTest(output)
{
    [Theory]
    [MemberData(nameof(ServiceProviderExtensionTestData.TryGetMustValidator.Cases), MemberType = typeof(ServiceProviderExtensionTestData.TryGetMustValidator))]
    public void TryGetMustValidator_BehavesAsExpected(MustValidatorResolutionCase tc)
    {
        // Arrange
        var (configureServices, validatedType) = tc.Value;
        var services = new ServiceCollection();
        configureServices(services);

        // Act
        using var provider = services.BuildServiceProvider();
        var found = provider.TryGetMustValidator(validatedType, out var validator);

        // Assert
        Assert.Equal(tc.Expected.IsValid, found);
        AssertValidatorType(tc.Expected, validator);
    }

    [Theory]
    [MemberData(nameof(ServiceProviderExtensionTestData.TryGetMustValidator.InvalidCases), MemberType = typeof(ServiceProviderExtensionTestData.TryGetMustValidator))]
    public void TryGetMustValidator_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    [Theory]
    [MemberData(nameof(ServiceProviderExtensionTestData.GetMustValidators.Cases), MemberType = typeof(ServiceProviderExtensionTestData.GetMustValidators))]
    public void GetMustValidators_BehavesAsExpected(MustValidatorResolutionCase tc)
    {
        // Arrange
        var (configureServices, validatedType) = tc.Value;
        var services = new ServiceCollection();
        configureServices(services);

        // Act
        using var provider = services.BuildServiceProvider();
        var validators = provider.GetMustValidators(validatedType);

        // Assert
        Assert.Equal(tc.Expected.IsValid, validators.Count > 0);
        Assert.Equal(tc.Expected.ValidatorCount, validators.Count);
        AssertValidatorType(tc.Expected, validators.FirstOrDefault());
    }

    [Theory]
    [MemberData(nameof(ServiceProviderExtensionTestData.GetMustValidators.InvalidCases), MemberType = typeof(ServiceProviderExtensionTestData.GetMustValidators))]
    public void GetMustValidators_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var action = ((ValueCase<Action>)tc).Value;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, action);
        ThrowsCaseAssert.Expected(ex, tc);
    }

    private static void AssertValidatorType(MustValidatorResolutionExpected expected, IMustValidator? validator)
    {
        if (expected.ValidatorType is null)
        {
            Assert.Null(validator);
            return;
        }

        Assert.IsType(expected.ValidatorType, validator);
    }
}
